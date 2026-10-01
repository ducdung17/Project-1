using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NongTrai.Profiles.UI
{
    /// <summary>
    /// Màn "Ai đang chơi nào?".
    /// - Chế độ bình thường: bé bấm thẻ của mình -> vào màn chính.
    /// - Nút khóa (qua cổng phụ huynh) -> chế độ quản lý: hiện nút xóa trên từng thẻ.
    /// - Thẻ "Thêm bé" (qua cổng phụ huynh) -> hộp thoại thêm hồ sơ.
    /// </summary>
    public class ProfileSelectScreen : MonoBehaviour
    {
        [Header("Danh sách thẻ")]
        [SerializeField] Transform cardContainer;
        [SerializeField] ProfileCardView cardPrefab;
        [Tooltip("Thẻ \"Thêm bé\" đã đặt sẵn trong Card Container.")]
        [SerializeField] Button addCardButton;

        [Header("Thanh trên cùng")]
        [SerializeField] TMP_Text titleText;
        [SerializeField] Button manageButton;
        [SerializeField] Button doneButton;

        [Header("Hộp thoại")]
        [SerializeField] AddProfileDialog addDialog;
        [SerializeField] ConfirmDialog confirmDialog;
        [SerializeField] ParentGateView parentGate;

        [Header("Chuyển cảnh")]
        [SerializeField] string mainMenuScene = "MainMenu";

        const string NormalTitle = "Ai đang chơi nào?";
        const string ManageTitle = "Quản lý hồ sơ";
        const string EmptyTitle = "Chào bé! Nhờ bố mẹ tạo hồ sơ nhé";

        readonly List<ProfileCardView> cards = new List<ProfileCardView>();
        bool manageMode;
        string lastPlayedId;

        ProfileService Service => ProfileManager.Instance.Service;
        AvatarCatalog Avatars => ProfileManager.Instance.Avatars;

        void Start()
        {
            if (ProfileManager.Instance == null || ProfileManager.Instance.Service == null)
            {
                Debug.LogError("[Profiles] Không tìm thấy ProfileManager trong scene.", this);
                enabled = false;
                return;
            }

            addCardButton.onClick.AddListener(OnAddClicked);
            manageButton.onClick.AddListener(() => RequireParent(() => SetManageMode(true)));
            doneButton.onClick.AddListener(() => SetManageMode(false));

            Service.ProfilesChanged += Refresh;

            // Nhớ bé chơi lần trước để đánh dấu, rồi bỏ chọn: mỗi lần mở game bé phải tự chọn lại.
            lastPlayedId = Service.SelectedProfile?.Id;
            Service.ClearSelection();

            SetManageMode(false);
        }

        void OnDestroy()
        {
            // ProfileManager sống qua nhiều scene, nên phải hủy đăng ký để tránh gọi vào object đã bị hủy.
            if (ProfileManager.Instance != null && ProfileManager.Instance.Service != null)
                ProfileManager.Instance.Service.ProfilesChanged -= Refresh;
        }

        void SetManageMode(bool value)
        {
            manageMode = value;
            manageButton.gameObject.SetActive(!manageMode);
            doneButton.gameObject.SetActive(manageMode);
            Refresh();
        }

        void Refresh()
        {
            foreach (ProfileCardView card in cards)
                Destroy(card.gameObject);
            cards.Clear();

            foreach (ChildProfile profile in Service.Profiles)
            {
                ProfileCardView card = Instantiate(cardPrefab, cardContainer);
                card.Bind(
                    profile,
                    Avatars.Get(profile.AvatarId),
                    manageMode,
                    profile.Id == lastPlayedId,
                    OnCardClicked,
                    OnDeleteClicked);
                cards.Add(card);
            }

            // Thẻ "Thêm bé" luôn ở cuối và ẩn khi đã đủ số hồ sơ tối đa.
            addCardButton.gameObject.SetActive(Service.CanAddMore);
            addCardButton.transform.SetAsLastSibling();

            if (manageMode)
                titleText.text = ManageTitle;
            else
                titleText.text = Service.Count == 0 ? EmptyTitle : NormalTitle;

            // Chưa có hồ sơ thì chưa cần nút quản lý.
            manageButton.gameObject.SetActive(!manageMode && Service.Count > 0);
        }

        // ---------------------------------------------------------------- Chọn

        void OnCardClicked(ChildProfile profile)
        {
            if (manageMode)
                return; // Đang quản lý thì bấm thẻ không vào game, tránh vô tình rời màn.

            if (Service.Select(profile.Id) == ProfileResult.Success)
                SceneManager.LoadScene(mainMenuScene);
        }

        // ---------------------------------------------------------------- Thêm

        void OnAddClicked()
        {
            RequireParent(() => addDialog.Open(Service, Avatars));
        }

        // ---------------------------------------------------------------- Xóa

        void OnDeleteClicked(ChildProfile profile)
        {
            confirmDialog.Show(
                $"Xóa hồ sơ \"{profile.Nickname}\"?\nToàn bộ nhãn dán và tiến độ học của bé sẽ bị mất.",
                () =>
                {
                    Service.Remove(profile.Id);
                    if (profile.Id == lastPlayedId)
                        lastPlayedId = null;
                });
        }

        // ---------------------------------------------------------------- Cổng phụ huynh

        /// <summary>Ở chế độ quản lý thì phụ huynh đã qua cổng rồi, không hỏi lại.</summary>
        void RequireParent(System.Action action)
        {
            if (manageMode)
                action();
            else
                parentGate.Show(action);
        }
    }
}
