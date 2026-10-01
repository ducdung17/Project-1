using UnityEngine;

namespace NongTrai.Profiles
{
    /// <summary>
    /// Giữ ProfileService sống xuyên suốt các scene (DontDestroyOnLoad),
    /// để scene trò chơi biết bé nào đang chơi: ProfileManager.Current.
    /// Đặt component này vào scene đầu tiên của game (scene chọn hồ sơ).
    /// </summary>
    [DefaultExecutionOrder(-100)] // Chạy Awake trước các script UI.
    public class ProfileManager : MonoBehaviour
    {
        [SerializeField] AvatarCatalog avatarCatalog;

        public static ProfileManager Instance { get; private set; }

        public ProfileService Service { get; private set; }
        public AvatarCatalog Avatars => avatarCatalog;

        /// <summary>Bé đang chơi, hoặc null nếu chưa chọn.</summary>
        public static ChildProfile Current => Instance != null ? Instance.Service.SelectedProfile : null;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                // Quay lại scene chọn hồ sơ sẽ tạo bản thứ hai -> hủy bản thừa.
                Destroy(gameObject);
                return;
            }

            if (avatarCatalog == null || avatarCatalog.Count == 0)
            {
                Debug.LogError("[Profiles] Chưa gán AvatarCatalog (hoặc catalog rỗng) cho ProfileManager.", this);
                enabled = false;
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Service = new ProfileService(new PlayerPrefsProfileStorage(), avatarCatalog.Count);
        }

        void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
