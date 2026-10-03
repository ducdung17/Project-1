using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NongTrai.UI
{
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
