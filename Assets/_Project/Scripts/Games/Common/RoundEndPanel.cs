using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NongTrai.Games
{
    public class RoundEndPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text titleText;
        [SerializeField] TMP_Text detailText;
        [SerializeField] Button againButton;
        [SerializeField] Button homeButton;

        bool initialized;

        public void Init(Action onAgain, Action onHome)
        {
            if (initialized) return;
            initialized = true;
            againButton.onClick.AddListener(() => onAgain?.Invoke());
            homeButton.onClick.AddListener(() => onHome?.Invoke());
        }

        public void Show(int perfectCount, int total, IReadOnlyList<string> newlyMastered)
        {
            float ratio = total == 0 ? 0 : perfectCount / (float)total;
            titleText.text = ratio >= 0.75f ? "Giỏi quá!" : (ratio >= 0.4f ? "Bé làm tốt lắm!" : "Cố lên nào!");

            string detail = $"Bé đúng ngay {perfectCount}/{total} câu";
            if (newlyMastered != null && newlyMastered.Count > 0)
                detail += $"\nBé vừa thuộc: {string.Join(", ", newlyMastered)}";
            detailText.text = detail;

            gameObject.SetActive(true);
        }

        public void Hide() => gameObject.SetActive(false);
    }
}
