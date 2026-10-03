using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NongTrai.Animals;
using NongTrai.Learning;
using NongTrai.Online;
using NongTrai.Profiles;
using NongTrai.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NongTrai.Games
{
    public enum MatchMode
    {
        Shadow,
        Food
    }

    public class DragMatchGameController : MonoBehaviour
    {
        [Header("Dữ liệu")]
        [SerializeField] AnimalDatabase database;
        [SerializeField] MatchMode mode = MatchMode.Shadow;

        [Header("Giao diện")]
        [SerializeField] Transform optionsContainer;
        [SerializeField] OptionCardView optionPrefab;
        [SerializeField] DraggableItem dragItem;
        [SerializeField] Button homeButton;
        [SerializeField] RoundProgressView progressView;
        [SerializeField] RoundEndPanel endPanel;
        [SerializeField] MasteryDebugPanel debugPanel;

        [Header("Âm thanh")]
        [SerializeField] AudioClip correctSound;
        [SerializeField] AudioClip wrongSound;
        [SerializeField] AudioClip celebrateSound;
        [Tooltip("Giọng đọc tên trò chơi, phát ở câu đầu mỗi lượt (không bắt buộc).")]
        [SerializeField] AudioClip promptVoice;

        [Header("Cài đặt")]
        [SerializeField] int questionsPerRound = 8;
        [SerializeField] float delayAfterCorrect = 2f;
        [Tooltip("Bao lâu không làm gì thì gợi ý thẻ đúng.")]
        [SerializeField] float hintAfterSeconds = 10f;
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
        Coroutine hintRoutine;
        HashSet<string> masteredAtStart;
        SessionRecorder recorder;

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
                Debug.LogError("[DragGame] Chưa gán AnimalDatabase hoặc có ít hơn 2 con vật.", this);
                enabled = false;
                return;
            }

            animalSource = gameObject.AddComponent<AudioSource>();
            animalSource.playOnAwake = false;

            learning = new LearningService(database, child.Id, new PlayerPrefsProgressStorage());
            if (debugPanel != null) debugPanel.Bind(learning, database);

            homeButton.onClick.AddListener(GoHome);
            endPanel.Init(StartRound, GoHome);
            dragItem.DragStarted += OnDragStarted;

            StartRound();
        }

        void StartRound()
        {
            endPanel.Hide();
            questionIndex = 0;
            perfectCount = 0;
            recorder = new SessionRecorder(mode == MatchMode.Shadow ? "shadow" : "food", ProfileManager.Current);
            masteredAtStart = new HashSet<string>(database.All.Where(a => a != null && learning.IsMastered(a.Id)).Select(a => a.Id));
            progressView.Build(questionsPerRound);
            if (promptVoice != null) UiAudio.PlayVoice(promptVoice);
            ShowNextQuestion();
        }

        Question CreateQuestion()
        {
            if (mode == MatchMode.Shadow)
                return learning.NextQuestion();

            for (int attempt = 0; attempt < 20; attempt++)
            {
                Question q = learning.NextQuestion((target, other) =>
                    target.Food == null || other.Food == null || other.Food != target.Food);
                if (q.Target.Food != null && q.Target.Food.Sprite != null)
                    return q;
            }
            Debug.LogWarning("[DragGame] Không tìm được con vật có hình thức ăn. Kiểm tra Food của các AnimalData.");
            return learning.NextQuestion();
        }

        void ShowNextQuestion()
        {
            current = CreateQuestion();
            firstTry = true;
            answered = false;
            questionStart = Time.realtimeSinceStartup;

            foreach (OptionCardView c in cards) Destroy(c.gameObject);
            cards.Clear();
            foreach (AnimalData animal in current.Options)
            {
                OptionCardView card = Instantiate(optionPrefab, optionsContainer);
                card.Bind(animal, OnCardTapped);
                card.SetSilhouette(mode == MatchMode.Shadow);
                DropTarget target = card.gameObject.GetComponent<DropTarget>();
                if (target == null) target = card.gameObject.AddComponent<DropTarget>();
                target.Dropped += OnDropped;
                cards.Add(card);
            }

            Sprite itemSprite = mode == MatchMode.Shadow ? current.Target.Sprite : current.Target.Food?.Sprite;
            dragItem.Show(itemSprite);

            if (mode == MatchMode.Shadow) PlayTargetSound(0.4f);

            if (debugPanel != null) debugPanel.Show(current);
            RestartHintTimer();
        }

        void OnDropped(DropTarget target, DraggableItem item) => Choose(target.Card, fromDrag: true);

        void OnCardTapped(OptionCardView card) => Choose(card, fromDrag: false);

        void OnDragStarted()
        {
            if (!answered) RestartHintTimer();
        }

        void Choose(OptionCardView card, bool fromDrag)
        {
            if (answered || card == null) return;

            float seconds = Time.realtimeSinceStartup - questionStart;
            bool correct = learning.RecordAnswer(current, card.Animal.Id, seconds, firstTry);
            recorder?.Add(current.Target.Id, card.Animal.Id, firstTry, seconds);

            if (correct)
            {
                answered = true;
                StopHintTimer();
                foreach (OptionCardView c in cards)
                {
                    c.SetInteractable(false);
                    if (c != card) c.StopEffect();
                }
                dragItem.FlyInto(card.Rect);
                card.SetSilhouette(false);
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
                if (fromDrag) dragItem.ReturnHome();
                HintCorrectCard();
            }

            if (debugPanel != null) debugPanel.Redraw();
        }

        IEnumerator AfterCorrect(bool perfect)
        {
            yield return new WaitForSecondsRealtime(0.4f);
            PlayTargetSound(0f);
            yield return new WaitForSecondsRealtime(0.9f);
            UiAudio.PlayVoice(current.Target.NameVoice);
            progressView.Fill(questionIndex, perfect);
            questionIndex++;

            yield return new WaitForSecondsRealtime(delayAfterCorrect);

            if (questionIndex >= questionsPerRound) EndRound();
            else ShowNextQuestion();
        }

        void EndRound()
        {
            recorder?.Finish(learning, database);
            recorder = null;

            foreach (OptionCardView c in cards) Destroy(c.gameObject);
            cards.Clear();
            dragItem.gameObject.SetActive(false);

            List<string> newlyMastered = database.All
                .Where(a => a != null && learning.IsMastered(a.Id) && !masteredAtStart.Contains(a.Id))
                .Select(a => a.NameVi)
                .ToList();

            UiAudio.Play(celebrateSound);
            endPanel.Show(perfectCount, questionsPerRound, newlyMastered);
        }

        void PlayTargetSound(float delay)
        {
            if (current == null || current.Target.Sound == null) return;
            animalSource.Stop();
            animalSource.clip = current.Target.Sound;
            animalSource.PlayDelayed(delay);
        }

        void HintCorrectCard()
        {
            OptionCardView target = cards.FirstOrDefault(c => c.Animal == current.Target);
            if (target != null) target.StartHint();
        }

        void RestartHintTimer()
        {
            StopHintTimer();
            hintRoutine = StartCoroutine(HintRoutine());
        }

        void StopHintTimer()
        {
            if (hintRoutine != null) StopCoroutine(hintRoutine);
            hintRoutine = null;
        }

        IEnumerator HintRoutine()
        {
            yield return new WaitForSecondsRealtime(hintAfterSeconds);
            if (!answered) HintCorrectCard();
            hintRoutine = null;
        }

        void GoHome() => SceneManager.LoadScene(mainMenuScene);
    }
}
