using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NongTrai.Profiles.UI
{
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

        static Color CardTint(Color c)
        {
            c.a = 1f;
            return Color.Lerp(Color.white, c, 0.35f);
        }

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
                background.color = CardTint(avatar.cardColor);
            }

            deleteButton.gameObject.SetActive(manageMode);
            if (lastPlayedMark != null)
                lastPlayedMark.SetActive(isLastPlayed && !manageMode);

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
