using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NongTrai.Animals;
using NongTrai.Learning;
using NongTrai.Profiles;
using NongTrai.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NongTrai.Games.Sound
{
    /// <summary>
    /// Trò "Ai kêu thế nhỉ?": nghe tiếng kêu, bấm đúng con vật.
    /// - Câu hỏi do LearningService chọn: con bé chưa thuộc được hỏi nhiều hơn, độ khó tự đổi.
    /// - Sai không bị phạt: thẻ sai mờ đi, thẻ đúng lắc lư gợi ý, bé chọn lại.
    /// - Ngồi lâu không chọn: tự phát lại tiếng kêu rồi gợi ý.
    /// </summary>
    public class SoundGameController : MonoBehaviour
    {
        [Header("Dữ liệu")]
        [SerializeField] AnimalDatabase database;

        [Header("Giao diện")]
        [SerializeField] Transform optionsContainer;
        [SerializeField] OptionCardView optionPrefab;
        [SerializeField] Button replayButton;
        [SerializeField] Button homeButton;
        [SerializeField] RoundProgressView progressView;
        [SerializeField] RoundEndPanel endPanel;
        [SerializeField] MasteryDebugPanel debugPanel;

        [Header("Âm thanh")]
        [SerializeField] AudioClip correctSound;
        [SerializeField] AudioClip wrongSound;
        [SerializeField] AudioClip celebrateSound;
        [Tooltip("Giọng đọc \"Ai kêu thế nhỉ?\" phát ở câu đầu mỗi lượt (không bắt buộc).")]
        [SerializeField] AudioClip promptVoice;

        [Header("Cài đặt")]
        [SerializeField] int questionsPerRound = 8;
        [SerializeField] float delayAfterCorrect = 1.8f;
        [Tooltip("Bao lâu không chọn thì phát lại tiếng kêu; gấp đôi thời gian này thì gợi ý.")]
        [SerializeField] float idleSeconds = 6f;
        [SerializeField] string mainMenuScene = "MainMenu";
        [SerializeField] string profileScene = "ProfileSelect";

        readonly List<OptionCardView> cards = new List<OptionCardView>();
        AudioSource animalSource;
        LearningService learning;
        Question current;
        int questionIndex;
        int perfectCount;
        bool firstTry;
        bool answered;
        float questionStart;
        Coroutine idleRoutine;
        HashSet<string> masteredAtStart;

        void Start()
        {
            ChildProfile child = ProfileManager.Current;
            if (child == null)
            {
                SceneManager.LoadScene(profileScene);
                return;
            }
            if (database == null || database.Count < 2)
            {
                Debug.LogError("[SoundGame] Chưa gán AnimalDatabase hoặc có ít hơn 2 con vật.", this);
                enabled = false;
                return;
            }

            animalSource = gameObject.AddComponent<AudioSource>();
            animalSource.playOnAwake = false;

            learning = new LearningService(database, child.Id, new PlayerPrefsProgressStorage());
            if (debugPanel != null) debugPanel.Bind(learning, database);

            replayButton.onClick.AddListener(PlayAnimalSound);
            homeButton.onClick.AddListener(GoHome);
            endPanel.Init(StartRound, GoHome);

            StartRound();
        }

        void StartRound()
        {
            endPanel.Hide();
            questionIndex = 0;
            perfectCount = 0;
            masteredAtStart = new HashSet<string>(database.All.Where(a => a != null && learning.IsMastered(a.Id)).Select(a => a.Id));
            progressView.Build(questionsPerRound);
            ShowNextQuestion();
        }

        void ShowNextQuestion()
        {
            current = learning.NextQuestion();
            firstTry = true;
            answered = false;
            questionStart = Time.realtimeSinceStartup;

            foreach (OptionCardView c in cards) Destroy(c.gameObject);
            cards.Clear();
            foreach (AnimalData animal in current.Options)
            {
                OptionCardView card = Instantiate(optionPrefab, optionsContainer);
                card.Bind(animal, OnCardChosen);
                cards.Add(card);
            }

            if (debugPanel != null) debugPanel.Show(current);
            RestartIdleTimer(introFirst: questionIndex == 0);
        }

        void PlayAnimalSound()
        {
            if (current == null || current.Target.Sound == null) return;
            animalSource.Stop();
            animalSource.clip = current.Target.Sound;
            animalSource.Play();
        }

        void OnCardChosen(OptionCardView card)
        {
            if (answered) return;

            float seconds = Time.realtimeSinceStartup - questionStart;
            bool correct = learning.RecordAnswer(current, card.Animal.Id, seconds, firstTry);

            if (correct)
            {
                answered = true;
                StopIdleTimer();
                foreach (OptionCardView c in cards)
                {
                    c.SetInteractable(false);
                    if (c != card) c.StopEffect();
                }
                card.PlayCorrect();
                UiAudio.Play(correctSound);
                if (firstTry) perfectCount++;
                StartCoroutine(AfterCorrect(firstTry));
            }
            else
            {
                firstTry = false;
                card.PlayWrong();
                UiAudio.Play(wrongSound);
                HintCorrectCard();
                RestartIdleTimer(introFirst: false, replayNow: false);
            }

            if (debugPanel != null) debugPanel.Redraw();
        }

        IEnumerator AfterCorrect(bool perfect)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            UiAudio.PlayVoice(current.Target.NameVoice); // "Con mèo!"
            progressView.Fill(questionIndex, perfect);
            questionIndex++;

            yield return new WaitForSecondsRealtime(delayAfterCorrect);

            if (questionIndex >= questionsPerRound) EndRound();
            else ShowNextQuestion();
        }

        void EndRound()
        {
            foreach (OptionCardView c in cards) Destroy(c.gameObject);
            cards.Clear();

            List<string> newlyMastered = database.All
                .Where(a => a != null && learning.IsMastered(a.Id) && !masteredAtStart.Contains(a.Id))
                .Select(a => a.NameVi)
                .ToList();

            UiAudio.Play(celebrateSound);
            endPanel.Show(perfectCount, questionsPerRound, newlyMastered);
        }

        void HintCorrectCard()
        {
            OptionCardView target = cards.FirstOrDefault(c => c.Animal == current.Target);
            if (target != null) target.StartHint();
        }

        // ------------------------------------------------------------ Chờ lâu thì nhắc

        void RestartIdleTimer(bool introFirst, bool replayNow = true)
        {
            StopIdleTimer();
            idleRoutine = StartCoroutine(IdleRoutine(introFirst, replayNow));
        }

        void StopIdleTimer()
        {
            if (idleRoutine != null) StopCoroutine(idleRoutine);
            idleRoutine = null;
        }

        IEnumerator IdleRoutine(bool introFirst, bool replayNow)
        {
            if (replayNow)
            {
                if (introFirst && promptVoice != null)
                {
                    UiAudio.PlayVoice(promptVoice);
                    yield return new WaitForSecondsRealtime(promptVoice.length + 0.2f);
                }
                yield return new WaitForSecondsRealtime(0.3f);
                PlayAnimalSound();
                questionStart = Time.realtimeSinceStartup; // tính giờ từ lúc bé nghe tiếng kêu
            }

            yield return new WaitForSecondsRealtime(idleSeconds);
            if (!answered) PlayAnimalSound();

            yield return new WaitForSecondsRealtime(idleSeconds);
            if (!answered) HintCorrectCard();
            idleRoutine = null;
        }

        void GoHome() => SceneManager.LoadScene(mainMenuScene);
    }
}
