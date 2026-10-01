using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NongTrai.Games
{
    /// <summary>
    /// Vật bé kéo đi (con vật màu trong "Tìm cái bóng", thức ăn trong "Cho bạn ăn").
    /// Thả trúng một DropTarget thì DropTarget báo cho bộ điều khiển; thả ra ngoài thì tự bay về chỗ cũ.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] Image image;
        [SerializeField] float moveSeconds = 0.25f;

        RectTransform rect;
        CanvasGroup group;
        Canvas rootCanvas;
        Vector2 home;
        bool droppedOnTarget;
        Coroutine moving;

        public bool Dragging { get; private set; }

        /// <summary>Bé bắt đầu kéo (để tắt gợi ý, tính giờ...).</summary>
        public event Action DragStarted;

        void Awake()
        {
            rect = (RectTransform)transform;
            group = GetComponent<CanvasGroup>();
            rootCanvas = GetComponentInParent<Canvas>().rootCanvas;
            home = rect.anchoredPosition; // vị trí đặt trong scene là "chỗ cũ"
        }

        /// <summary>Hiện vật ở chỗ cũ với hình mới, sẵn sàng để kéo.</summary>
        public void Show(Sprite sprite)
        {
            StopMoving();
            image.sprite = sprite;
            image.preserveAspect = true;
            rect.anchoredPosition = home;
            rect.localScale = Vector3.one;
            group.alpha = 1f;
            SetInteractable(true);
            gameObject.SetActive(true);
        }

        public void SetInteractable(bool value)
        {
            group.interactable = value;
            group.blocksRaycasts = value;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!group.interactable) return;
            Dragging = true;
            droppedOnTarget = false;
            StopMoving();
            group.blocksRaycasts = false; // để thẻ bên dưới nhận được sự kiện thả
            rect.localScale = Vector3.one * 1.1f;
            DragStarted?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!Dragging) return;
            rect.anchoredPosition += eventData.delta / rootCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!Dragging) return;
            Dragging = false;
            rect.localScale = Vector3.one;
            if (!droppedOnTarget)
                ReturnHome();
        }

        /// <summary>DropTarget gọi khi vật được thả trúng nó (chạy trước OnEndDrag).</summary>
        public void NotifyDropped() => droppedOnTarget = true;

        public void ReturnHome()
        {
            group.blocksRaycasts = group.interactable;
            Move(home, 1f, false);
        }

        /// <summary>Bay vào giữa thẻ đích rồi biến mất (khi đúng).</summary>
        public void FlyInto(RectTransform target)
        {
            SetInteractable(false);
            Vector2 screen = RectTransformUtility.WorldToScreenPoint(null, target.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)rect.parent, screen, null, out Vector2 local);
            Move(local, 0.6f, true);
        }

        void Move(Vector2 to, float endScale, bool hideAtEnd)
        {
            StopMoving();
            if (isActiveAndEnabled)
                moving = StartCoroutine(MoveRoutine(to, endScale, hideAtEnd));
        }

        void StopMoving()
        {
            if (moving != null) StopCoroutine(moving);
            moving = null;
        }

        IEnumerator MoveRoutine(Vector2 to, float endScale, bool hideAtEnd)
        {
            Vector2 from = rect.anchoredPosition;
            Vector3 fromScale = rect.localScale;
            for (float t = 0; t < 1f; t += Time.unscaledDeltaTime / moveSeconds)
            {
                float e = 1f - (1f - t) * (1f - t); // chậm dần
                rect.anchoredPosition = Vector2.LerpUnclamped(from, to, e);
                rect.localScale = Vector3.LerpUnclamped(fromScale, Vector3.one * endScale, e);
                yield return null;
            }
            rect.anchoredPosition = to;
            rect.localScale = Vector3.one * endScale;
            if (hideAtEnd) group.alpha = 0f;
            moving = null;
        }
    }
}
