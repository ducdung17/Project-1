using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NongTrai.Profiles.UI
{
    /// <summary>Một thẻ hồ sơ: con vật + biệt danh, và nút xóa (chỉ hiện ở chế độ quản lý).</summary>
    public class ProfileCardView : MonoBehaviour
    {
        [SerializeField] Button cardButton;
        [SerializeField] Image background;
        [SerializeField] Image avatarImage;
        [SerializeField] TMP_Text nameText;
        [SerializeField] Button deleteButton;
        [Tooltip("Viền/ngôi sao đánh dấu bé chơi gần nhất (không bắt buộc).")]
        [SerializeField] GameObject lastPlayedMark;
        [SerializeField] AudioSource audioSource;

        ChildProfile profile;
        AudioClip sound;

        public ChildProfile Profile => profile;

        public void Bind(
            ChildProfile profile,
            AvatarCatalog.AvatarEntry avatar,
            bool manageMode,
            bool isLastPlayed,
            Action<ChildProfile> onClick,
            Action<ChildProfile> onDelete)
        {
            this.profile = profile;
            sound = avatar?.sound;

            nameText.text = profile.Nickname;
            if (avatar != null)
            {
                avatarImage.sprite = avatar.sprite;
                background.color = avatar.cardColor;
            }

            deleteButton.gameObject.SetActive(manageMode);
            if (lastPlayedMark != null)
                lastPlayedMark.SetActive(isLastPlayed && !manageMode);

            // Xóa listener cũ trước khi gắn mới, tránh bấm một lần mà gọi nhiều lần.
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(() =>
            {
                PlaySound();
                onClick?.Invoke(this.profile);
            });

            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => onDelete?.Invoke(this.profile));
        }

        void PlaySound()
        {
            if (audioSource != null && sound != null)
                audioSource.PlayOneShot(sound);
        }
    }
}
