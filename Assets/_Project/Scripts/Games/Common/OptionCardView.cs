using System;
using System.Collections;
using NongTrai.Animals;
using NongTrai.UI;
using UnityEngine;
using UnityEngine.UI;

namespace NongTrai.Games
{
    /// <summary>
    /// Một thẻ đáp án (hình con vật) dùng chung cho các trò chơi.
    /// Có sẵn hiệu ứng: nảy khi đúng, lắc và mờ đi khi sai, lắc lư nhẹ để gợi ý.
    /// </summary>
    public class OptionCardView : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] Image icon;
        [SerializeField] CanvasGroup canvasGroup;
        [Tooltip("Tiếng khi bấm (không bắt buộc).")]
        [SerializeField] AudioClip tapSound;

        public AnimalData Animal { get; private set; }

        Coroutine effect;
        RectTransform iconRect;
        Vector2 iconBasePos;

        void Awake()
        {
            iconRect = icon.rectTransform;
            iconBasePos = iconRect.anchoredPosition;
        }

        public void Bind(AnimalData animal, Action<OptionCardView> onChosen)
        {
            if (iconRect == null) Awake();
            Animal = animal;
            icon.sprite = animal.Sprite;
            icon.color = Color.white;
            canvasGroup.alpha = 1f;
            button.interactable = true;
            ResetVisual();

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                UiAudio.Play(tapSound);
                onChosen?.Invoke(this);
            });
        }

        public void SetInteractable(bool value) => button.interactable = value;

        /// <summary>Bật = tô đen hình con vật thành cái bóng (trò "Tìm cái bóng"); tắt = hiện màu thật.</summary>
        public void SetSilhouette(bool on) => icon.color = on ? Color.black : Color.white;

        public RectTransform Rect => (RectTransform)transform;

        /// <summary>Đúng: nảy lên 2 lần.</summary>
        public void PlayCorrect() => Run(Bounce());

        /// <summary>Sai: lắc ngang rồi mờ đi, không bấm lại được.</summary>
        public void PlayWrong()
        {
            button.interactable = false;
            Run(Shake());
        }

        /// <summary>Gợi ý: lắc lư nhẹ liên tục cho tới khi gọi StopEffect().</summary>
        public void StartHint() => Run(Wiggle());

        public void StopEffect()
        {
            if (effect != null) StopCoroutine(effect);
            effect = null;
            ResetVisual();
        }

        void Run(IEnumerator routine)
        {
            StopEffect();
            if (isActiveAndEnabled)
                effect = StartCoroutine(routine);
        }

        void ResetVisual()
        {
            transform.localScale = Vector3.one;
            if (iconRect == null) return;
            iconRect.anchoredPosition = iconBasePos;
            iconRect.localRotation = Quaternion.identity;
        }

        IEnumerator Bounce()
        {
            for (int n = 0; n < 2; n++)
            {
                for (float t = 0; t < 1f; t += Time.unscaledDeltaTime / 0.3f)
                {
                    transform.localScale = Vector3.one * (1f + 0.22f * Mathf.Sin(t * Mathf.PI));
                    yield return null;
                }
            }
            transform.localScale = Vector3.one;
            effect = null;
        }

        IEnumerator Shake()
        {
            for (float t = 0; t < 1f; t += Time.unscaledDeltaTime / 0.4f)
            {
                float x = Mathf.Sin(t * Mathf.PI * 8f) * 18f * (1f - t);
                iconRect.anchoredPosition = iconBasePos + new Vector2(x, 0);
                yield return null;
            }
            iconRect.anchoredPosition = iconBasePos;
            canvasGroup.alpha = 0.35f;
            effect = null;
        }

        IEnumerator Wiggle()
        {
            float t = 0;
            while (true)
            {
                t += Time.unscaledDeltaTime;
                iconRect.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(t * 9f) * 8f);
                transform.localScale = Vector3.one * (1f + 0.05f * Mathf.Abs(Mathf.Sin(t * 4.5f)));
                yield return null;
            }
        }
    }
}
