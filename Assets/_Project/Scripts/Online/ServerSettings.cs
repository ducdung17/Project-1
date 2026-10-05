using UnityEngine;

namespace NongTrai.Online
{
    [CreateAssetMenu(menuName = "NongTrai/Server Settings", fileName = "ServerSettings")]
    public class ServerSettings : ScriptableObject
    {
       
        public bool enabled = true;
        public string baseUrl = "http://localhost:5180";
        public string apiKey = "nongtrai-dev-key";
        public int timeoutSeconds = 10;
        public float retryEverySeconds = 60f;

        public static ServerSettings Load() => Resources.Load<ServerSettings>("ServerSettings");
    }
}
