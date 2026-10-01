using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NongTrai.Profiles.UI
{
    /// <summary>Hộp thoại xác nhận dùng chung (ví dụ: xác nhận xóa hồ sơ).</summary>
    public class ConfirmDialog : MonoBehaviour
    {
        [SerializeField] TMP_Text messageText;
        [SerializeField] Button confirmButton;
        [SerializeField] Button cancelButton;

        Action onConfirm;
        bool initialized;

        void EnsureInitialized()
        {
            if (initialized) return;
            initialized = true;
            confirmButton.onClick.AddListener(() =>
            {
                Action callback = onConfirm;
                Hide();
                callback?.Invoke();
            });
            cancelButton.onClick.AddListener(Hide);
        }

        public void Show(string message, Action confirmAction)
        {
            EnsureInitialized();
            messageText.text = message;
            onConfirm = confirmAction;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            onConfirm = null;
            gameObject.SetActive(false);
        }
    }
}
