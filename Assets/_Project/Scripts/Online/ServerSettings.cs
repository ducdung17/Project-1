using UnityEngine;

namespace NongTrai.Online
{
    [CreateAssetMenu(menuName = "NongTrai/Server Settings", fileName = "ServerSettings")]
    public class ServerSettings : ScriptableObject
    {
        [Tooltip("Bật/tắt gửi dữ liệu lên server.")]
        public bool enabled = true;

        [Tooltip("Địa chỉ server, không có dấu / ở cuối. Ví dụ http://localhost:5180")]
        public string baseUrl = "http://localhost:5180";

        [Tooltip("Phải giống Upload:ApiKey trong appsettings.json của server.")]
        public string apiKey = "nongtrai-dev-key";

        [Tooltip("Thời gian chờ mỗi lần gửi (giây).")]
        public int timeoutSeconds = 10;

        [Tooltip("Bao lâu thử gửi lại các lượt chưa gửi được (giây).")]
        public float retryEverySeconds = 60f;

        public static ServerSettings Load() => Resources.Load<ServerSettings>("ServerSettings");
    }
}
