using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NongTrai.Profiles.UI
{
    /// <summary>Hộp thoại thêm bé mới: chọn con vật đại diện + nhập biệt danh.</summary>
    public class AddProfileDialog : MonoBehaviour
    {
        [SerializeField] Transform avatarGrid;
        [SerializeField] AvatarOptionView avatarOptionPrefab;
        [SerializeField] TMP_InputField nameInput;
        [SerializeField] TMP_Text errorText;
        [SerializeField] Button saveButton;
        [SerializeField] Button cancelButton;

        readonly List<AvatarOptionView> options = new List<AvatarOptionView>();
        ProfileService service;
        int selectedAvatarId = -1;
        bool initialized;

        // Không dùng Awake: hộp thoại để ẩn sẵn trong scene nên Awake chỉ chạy khi mở lần đầu.
        void EnsureInitialized()
        {
            if (initialized) return;
            initialized = true;
            saveButton.onClick.AddListener(OnSave);
            cancelButton.onClick.AddListener(Close);
            nameInput.characterLimit = ProfileService.MaxNameLength;
            nameInput.onValueChanged.AddListener(_ => ShowError(string.Empty));
            nameInput.onSubmit.AddListener(_ => OnSave()); // Enter để lưu
        }

        public void Open(ProfileService profileService, AvatarCatalog catalog)
        {
            EnsureInitialized();
            service = profileService;
            gameObject.SetActive(true);
            nameInput.text = string.Empty;
            ShowError(string.Empty);
            BuildAvatarGrid(catalog);

            // Chọn sẵn con vật đầu tiên còn trống để phụ huynh chỉ cần gõ tên.
            selectedAvatarId = -1;
            for (int i = 0; i < catalog.Count; i++)
            {
                if (!service.IsAvatarTaken(i))
                {
                    PickAvatar(i);
                    break;
                }
            }

            nameInput.Select();
            nameInput.ActivateInputField();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        void BuildAvatarGrid(AvatarCatalog catalog)
        {
            foreach (AvatarOptionView option in options)
                Destroy(option.gameObject);
            options.Clear();

            for (int i = 0; i < catalog.Count; i++)
            {
                AvatarCatalog.AvatarEntry entry = catalog.Get(i);
                AvatarOptionView option = Instantiate(avatarOptionPrefab, avatarGrid);
                option.Bind(i, entry, service.IsAvatarTaken(i), PickAvatar);
                options.Add(option);
            }
        }

        void PickAvatar(int avatarId)
        {
            selectedAvatarId = avatarId;
            foreach (AvatarOptionView option in options)
                option.SetSelected(option.AvatarId == avatarId);
            ShowError(string.Empty);
        }

        void OnSave()
        {
            ProfileResult result = service.Add(nameInput.text, selectedAvatarId);
            if (result == ProfileResult.Success)
                Close();
            else
                ShowError(ProfileMessages.For(result));
        }

        void ShowError(string message)
        {
            errorText.text = message;
            errorText.gameObject.SetActive(!string.IsNullOrEmpty(message));
        }
    }
}
