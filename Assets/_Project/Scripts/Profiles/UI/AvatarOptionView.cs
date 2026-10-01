using System;
using UnityEngine;
using UnityEngine.UI;

namespace NongTrai.Profiles.UI
{
    /// <summary>Một ô con vật trong hộp thoại thêm hồ sơ.</summary>
    public class AvatarOptionView : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] Image icon;
        [SerializeField] GameObject selectedFrame;
        [SerializeField] CanvasGroup canvasGroup;
        [Range(0f, 1f)] [SerializeField] float takenAlpha = 0.35f;

        public int AvatarId { get; private set; }

        public void Bind(int avatarId, AvatarCatalog.AvatarEntry entry, bool taken, Action<int> onPick)
        {
            AvatarId = avatarId;
            icon.sprite = entry.sprite;

            // Con vật đã có bé khác dùng: làm mờ và không cho chọn.
            button.interactable = !taken;
            if (canvasGroup != null)
                canvasGroup.alpha = taken ? takenAlpha : 1f;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onPick?.Invoke(AvatarId));
            SetSelected(false);
        }

        public void SetSelected(bool selected)
        {
            if (selectedFrame != null)
                selectedFrame.SetActive(selected);
        }
    }
}
