using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NongTrai.UI
{
    /// <summary>Gắn vào một Button: bấm là chuyển sang scene đã điền (ví dụ nút về màn hình chính).</summary>
    [RequireComponent(typeof(Button))]
    public class LoadSceneButton : MonoBehaviour
    {
        [Tooltip("Tên scene cần mở (phải có trong Build Profiles).")]
        [SerializeField] string sceneName = "Start Scene";

        void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Load);
        }

        void Load()
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("[LoadSceneButton] Chưa điền tên scene.", this);
                return;
            }
            SceneManager.LoadScene(sceneName);
        }
    }
}
