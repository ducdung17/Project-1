using UnityEngine;

namespace NongTrai.Profiles
{
    [DefaultExecutionOrder(-100)]
    public class ProfileManager : MonoBehaviour
    {
        [SerializeField] AvatarCatalog avatarCatalog;

        public static ProfileManager Instance { get; private set; }

        public ProfileService Service { get; private set; }
        public AvatarCatalog Avatars => avatarCatalog;

        public static ChildProfile Current => Instance != null ? Instance.Service.SelectedProfile : null;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
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
