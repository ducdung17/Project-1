using System.IO;
using System.Linq;
using System.Text;
using NongTrai.MainMenu;
using NongTrai.Profiles.UI;
using NongTrai.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using B = NongTrai.EditorTools.SoundGameBuilder;

namespace NongTrai.EditorTools
{
    public static class HomeButtonInstaller
    {
        const string ButtonName = "BackToStart";

        public static void Install()
        {
            string startPath = EditorBuildSettings.scenes.FirstOrDefault(s => s.enabled)?.path;
            if (string.IsNullOrEmpty(startPath))
            {
                EditorUtility.DisplayDialog("Chưa có scene", "Build Profiles chưa có scene nào được tick.", "OK");
                return;
            }
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            string startScene = Path.GetFileNameWithoutExtension(startPath);
            string firstOpen = EditorSceneManager.GetActiveScene().path;
            var report = new StringBuilder($"Nút về \"{startScene}\":\n");

            foreach (string path in EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).Skip(1))
            {
                Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                Component screen = (Component)Object.FindObjectsByType<ProfileSelectScreen>(FindObjectsInactive.Include).FirstOrDefault()
                                   ?? Object.FindObjectsByType<MainMenuScreen>(FindObjectsInactive.Include).FirstOrDefault();
                if (screen == null) continue;

                Canvas canvas = screen.GetComponentInParent<Canvas>(true);
                if (canvas == null) canvas = Object.FindObjectsByType<Canvas>(FindObjectsInactive.Include).FirstOrDefault(c => c.isRootCanvas);
                if (canvas == null)
                {
                    report.AppendLine($"- {scene.name}: không thấy Canvas, bỏ qua.");
                    continue;
                }
                RectTransform root = (RectTransform)canvas.rootCanvas.transform;

                Transform old = root.Find(ButtonName);
                if (old != null) Object.DestroyImmediate(old.gameObject);

                B.LoadSkin();
                Sprite blank = UiTheme.RoundBlank(UiTheme.Tone.LightBlue);
                Sprite homeIcon = B.FindSprite("icon_home");
                AudioClip pop = B.FindByName<AudioClip>("ui_pop");
                Button button = blank != null
                    ? B.MakeButton(root, ButtonName, Vector2.zero, new Vector2(36, 30), new Vector2(140, 140),
                        Color.white, blank, homeIcon, homeIcon == null ? "Về" : null, 34, Color.white, pop)
                    : B.MakeButton(root, ButtonName, Vector2.zero, new Vector2(36, 30), new Vector2(130, 130),
                        B.Soft, B.Knob, homeIcon, homeIcon == null ? "Về" : null, 34, B.Ink, pop);

                int index = 0;
                for (int i = 0; i < root.childCount; i++)
                {
                    string n = root.GetChild(i).name.ToLowerInvariant();
                    if (n.Contains("background") || n == "bg") index = i + 1;
                }
                button.transform.SetSiblingIndex(index);

                LoadSceneButton loader = button.gameObject.AddComponent<LoadSceneButton>();
                B.SetString(loader, "sceneName", startScene);

                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
                report.AppendLine($"- {scene.name}: đã thêm (góc dưới trái).");
            }

            if (!string.IsNullOrEmpty(firstOpen) && File.Exists(firstOpen))
                EditorSceneManager.OpenScene(firstOpen, OpenSceneMode.Single);
            Debug.Log("[HomeButtonInstaller] " + report);
            EditorUtility.DisplayDialog("Đã thêm nút về màn hình chính", report.ToString(), "OK");
        }
    }
}
