using NongTrai.Online;
using UnityEditor;
using UnityEngine;

namespace NongTrai.EditorTools
{
    public static class ServerSetup
    {
        const string Folder = "Assets/_Project/Resources";
        const string AssetPath = Folder + "/ServerSettings.asset";

        public static void Setup()
        {
            string report = Run();
            Debug.Log("[ServerSetup] " + report);
            EditorUtility.DisplayDialog("Kết nối server", report, "OK");
        }

        internal static string Run()
        {
            SoundGameBuilder.EnsureFolder(Folder);
            var settings = AssetDatabase.LoadAssetAtPath<ServerSettings>(AssetPath);
            string created = "đã có sẵn";
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<ServerSettings>();
                AssetDatabase.CreateAsset(settings, AssetPath);
                AssetDatabase.SaveAssets();
                created = "đã tạo mới";
            }

            PlayerSettings.insecureHttpOption = InsecureHttpOption.AlwaysAllowed;
            AssetDatabase.SaveAssets();

            Selection.activeObject = settings;
            return $"ServerSettings: {created} ({AssetPath})\n" +
                   $"Địa chỉ: {settings.baseUrl}\nKhóa API: {settings.apiKey}\nGửi dữ liệu: {(settings.enabled ? "bật" : "tắt")}\n" +
                   "Player Settings > Allow downloads over HTTP: Always allowed.";
        }
    }
}
