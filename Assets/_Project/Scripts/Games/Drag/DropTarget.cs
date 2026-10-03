using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NongTrai.Games
{
    public class DropTarget : MonoBehaviour, IDropHandler
    {
        public event Action<DropTarget, DraggableItem> Dropped;

        public OptionCardView Card { get; private set; }

        void Awake()
        {
            Card = GetComponent<OptionCardView>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            DraggableItem item = eventData.pointerDrag != null ? eventData.pointerDrag.GetComponent<DraggableItem>() : null;
            if (item == null || !item.Dragging) return;
            item.NotifyDropped();
            Dropped?.Invoke(this, item);
        }
    }
}
