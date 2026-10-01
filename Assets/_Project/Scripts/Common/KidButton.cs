using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NongTrai.UI
{
    /// <summary>
    /// Gắn thêm vào bất kỳ Button nào để nút "sống động" cho trẻ em:
    /// - Rê chuột vào: nút phóng to nhẹ và đọc tên nút (bé chưa biết chữ vẫn biết nút làm gì).
    /// - Nhấn xuống: nút lún xuống; thả ra: nảy lại và phát tiếng "bụp".
    /// Không cần code gì thêm, chỉ Add Component và kéo âm thanh vào.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class KidButton : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Tooltip("Giọng đọc tên nút khi rê chuột vào, ví dụ \"Ai kêu thế nhỉ?\" (không bắt buộc).")]
        [SerializeField] AudioClip hoverVoice;
        [Tooltip("Tiếng khi bấm, ví dụ tiếng \"bụp\" (không bắt buộc).")]
        [SerializeField] AudioClip clickSound;

        [Header("Hiệu ứng")]
        [SerializeField] float hoverScale = 1.06f;
        [SerializeField] float pressScale = 0.92f;
        [Tooltip("Tốc độ co giãn, càng lớn càng nhanh.")]
        [SerializeField] float speed = 14f;

        Button button;
        Vector3 baseScale;
        float targetScale = 1f;
        bool hovered;

        void Awake()
        {
            button = GetComponent<Button>();
            baseScale = transform.localScale;
            button.onClick.AddListener(() => UiAudio.Play(clickSound));
        }

        void OnEnable()
        {
            // Panel bị tắt khi đang rê chuột thì lần bật lại nút không bị kẹt ở kích thước to.
            hovered = false;
            targetScale = 1f;
            if (baseScale != Vector3.zero)
                transform.localScale = baseScale;
        }

        void Update()
        {
            Vector3 target = baseScale * targetScale;
            if (transform.localScale == target)
                return;

            // Dùng unscaledDeltaTime để nút vẫn nảy khi game tạm dừng (Time.timeScale = 0).
            float t = 1f - Mathf.Exp(-speed * Time.unscaledDeltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, target, t);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hovered = true;
            if (!button.interactable) return;
            targetScale = hoverScale;
            UiAudio.PlayVoice(hoverVoice);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hovered = false;
            targetScale = 1f;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!button.interactable) return;
            targetScale = pressScale;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            targetScale = hovered && button.interactable ? hoverScale : 1f;
        }
    }
}
