using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NongTrai.Profiles.UI
{
    /// <summary>
    /// Cổng phụ huynh: hiện một phép cộng với vài đáp án.
    /// Đúng thì chạy hành động được bảo vệ; sai thì đóng lại (không gợi ý đáp án, để bé không đoán mò).
    /// </summary>
    public class ParentGateView : MonoBehaviour
    {
        [SerializeField] TMP_Text questionText;
        [Tooltip("Các nút đáp án, số lượng nút = số đáp án hiển thị.")]
        [SerializeField] Button[] answerButtons;
        [SerializeField] Button closeButton;

        readonly System.Random random = new System.Random();
        Action onPassed;
        ParentGateChallenge challenge;
        bool initialized;

        void EnsureInitialized()
        {
            if (initialized) return;
            initialized = true;
            closeButton.onClick.AddListener(Hide);
        }

        public void Show(Action passedAction)
        {
            EnsureInitialized();
            onPassed = passedAction;
            challenge = ParentGateChallenge.Generate(random, answerButtons.Length);
            questionText.text = $"Dành cho người lớn\n{challenge.Question}";

            for (int i = 0; i < answerButtons.Length; i++)
            {
                int value = challenge.Options[i];
                Button button = answerButtons[i];
                button.GetComponentInChildren<TMP_Text>().text = value.ToString();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnAnswer(value));
            }

            gameObject.SetActive(true);
        }

        void OnAnswer(int value)
        {
            Action callback = onPassed;
            bool passed = challenge.Check(value);
            Hide();
            if (passed)
                callback?.Invoke();
        }

        public void Hide()
        {
            onPassed = null;
            gameObject.SetActive(false);
        }
    }
}
