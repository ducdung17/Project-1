using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NongTrai.EditorTools
{
    public static class UiPolisher
    {
        static readonly string[] GeneratedScenes = { "GameSound", "GameShadow", "GameFood" };

        public static void PolishAll()
        {
            if (UiTheme.Font == null)
            {
                EditorUtility.DisplayDialog("Thiếu font", "Không tìm thấy font asset \"Mali-Bold SDF\" trong project.", "OK");
                return;
            }
            if (!EditorUtility.DisplayDialog("Làm đẹp giao diện",
                    "Công cụ sẽ:\n• Đổi toàn bộ chữ sang Mali-Bold SDF\n• Bo góc các khung/nút trơn, nền một màu đổi sang nền nông trại\n" +
                    "• Dựng lại 3 màn chơi với nút và nền mới\n\nScene và prefab được sao lưu vào thư mục UIBackup trước khi sửa.",
                    "Làm đẹp", "Hủy"))
                return;
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            var report = new StringBuilder();
            string firstScene = EditorSceneManager.GetActiveScene().path;

            try
            {
                string backup = Backup();
                report.AppendLine($"Sao lưu: {backup}");
                report.AppendLine(UiTheme.SetDefaultFont() ? "Font mặc định của TextMeshPro: Mali-Bold SDF" : "Không đặt được font mặc định (thiếu TMP Settings).");

                _ = UiTheme.RoundedBox;
                _ = UiTheme.Card;

                EditorUtility.DisplayProgressBar("Làm đẹp giao diện", "Prefab...", 0.1f);
                foreach (string guid in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/_Project" }))
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (Path.GetFileNameWithoutExtension(path) == "OptionCard") continue;
                    GameObject root = PrefabUtility.LoadPrefabContents(path);
                    try
                    {
                        Stats s = Polish(root, new Vector2(1920, 1080));
                        if (s.Changed) PrefabUtility.SaveAsPrefabAsset(root, path);
                        report.AppendLine($"Prefab {Path.GetFileName(path)}: {s}");
                    }
                    finally
                    {
                        PrefabUtility.UnloadPrefabContents(root);
                    }
                }

                List<string> scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path)
                    .Where(p => !GeneratedScenes.Contains(Path.GetFileNameWithoutExtension(p))).ToList();
                for (int i = 0; i < scenes.Count; i++)
                {
                    EditorUtility.DisplayProgressBar("Làm đẹp giao diện", scenes[i], 0.2f + 0.4f * i / Math.Max(1, scenes.Count));
                    Scene scene = EditorSceneManager.OpenScene(scenes[i], OpenSceneMode.Single);
                    var total = new Stats();
                    foreach (GameObject root in scene.GetRootGameObjects())
                    {
                        Canvas canvas = root.GetComponentInChildren<Canvas>(true);
                        Vector2 size = canvas != null ? ((RectTransform)canvas.rootCanvas.transform).rect.size : new Vector2(1920, 1080);
                        if (size.x < 10 || size.y < 10) size = new Vector2(1920, 1080);
                        total.Add(Polish(root, size));
                    }
                    if (total.Changed)
                    {
                        EditorSceneManager.MarkSceneDirty(scene);
                        EditorSceneManager.SaveScene(scene);
                    }
                    report.AppendLine($"Scene {Path.GetFileNameWithoutExtension(scenes[i])}: {total}");
                }

                EditorUtility.DisplayProgressBar("Làm đẹp giao diện", "Dựng lại GameSound...", 0.7f);
                report.AppendLine(SoundGameBuilder.Run(false));
                EditorUtility.DisplayProgressBar("Làm đẹp giao diện", "Dựng lại GameShadow...", 0.8f);
                report.AppendLine(DragGameBuilder.RunShadow(false));
                EditorUtility.DisplayProgressBar("Làm đẹp giao diện", "Dựng lại GameFood...", 0.9f);
                report.AppendLine(DragGameBuilder.RunFood(false));

                AssetDatabase.SaveAssets();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            if (!string.IsNullOrEmpty(firstScene) && File.Exists(firstScene))
                EditorSceneManager.OpenScene(firstScene, OpenSceneMode.Single);

            Debug.Log("[UiPolisher]\n" + report);
            EditorUtility.DisplayDialog("Đã làm đẹp xong", report.ToString(), "OK");
        }

        struct Stats
        {
            public int Fonts;
            public int Rounded;
            public int Backgrounds;
            public bool Changed => Fonts + Rounded + Backgrounds > 0;
            public void Add(Stats o) { Fonts += o.Fonts; Rounded += o.Rounded; Backgrounds += o.Backgrounds; }
            public override string ToString() =>
                $"đổi font {Fonts} chữ, bo góc {Rounded} khung" + (Backgrounds > 0 ? $", thay {Backgrounds} nền" : "");
        }

        static Stats Polish(GameObject root, Vector2 canvasSize)
        {
            var stats = new Stats();
            foreach (TMP_Text text in root.GetComponentsInChildren<TMP_Text>(true))
                if (UiTheme.ApplyFont(text)) stats.Fonts++;

            Sprite rounded = UiTheme.RoundedBox;
            Sprite card = UiTheme.Card;
            Sprite background = UiTheme.Background;
            if (rounded == null) return stats;
            foreach (Image img in root.GetComponentsInChildren<Image>(true))
            {
                if (background != null && IsPlainFullScreenBackground(img, canvasSize))
                {
                    img.sprite = background;
                    img.type = Image.Type.Simple;
                    img.color = Color.white;
                    img.preserveAspect = false;
                    EditorUtility.SetDirty(img);
                    stats.Backgrounds++;
                    continue;
                }
                if (!ShouldRound(img, canvasSize, rounded, card)) continue;
                Rect r = img.rectTransform.rect;
                bool big = Mathf.Min(r.width, r.height) >= 120f && card != null;
                UiTheme.ApplySliced(img, big ? card : rounded);
                stats.Rounded++;
            }
            return stats;
        }

        static bool IsPlainFullScreenBackground(Image img, Vector2 canvasSize)
        {
            if (img == null || img.GetType() != typeof(Image) || img.sprite != null) return false;
            if (img.color.a < 0.9f) return false;
            string n = img.gameObject.name.ToLowerInvariant();
            if (!n.Contains("background") && n != "bg") return false;
            Rect r = img.rectTransform.rect;
            return r.width >= canvasSize.x * 0.8f && r.height >= canvasSize.y * 0.8f;
        }

        static bool ShouldRound(Image img, Vector2 canvasSize, Sprite rounded, Sprite card)
        {
            if (img is null || img.GetType() != typeof(Image)) return false;
            bool ours = img.sprite != null && (img.sprite == rounded || img.sprite == card);
            if (!ours && !UiTheme.IsBuiltinPlainSprite(img.sprite)) return false;
            if (img.color.a < 0.6f) return false;
            if (UiTheme.NameLooksLikeBackground(img.gameObject.name)) return false;
            if (img.GetComponent<Mask>() != null || img.GetComponent<RectMask2D>() != null) return false;
            if (img.GetComponentInParent<Scrollbar>(true) != null) return false;

            Rect r = img.rectTransform.rect;
            if (Mathf.Min(r.width, r.height) < 12f) return false;
            if (r.width >= canvasSize.x * 0.8f && r.height >= canvasSize.y * 0.8f) return false;
            return true;
        }

        static string Backup()
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string dest = Path.Combine(projectRoot, "UIBackup", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            IEnumerable<string> files = EditorBuildSettings.scenes.Select(s => s.path)
                .Concat(AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/_Project" }).Select(AssetDatabase.GUIDToAssetPath))
                .Append("Assets/TextMesh Pro/Resources/TMP Settings.asset")
                .Distinct();

            foreach (string assetPath in files)
            {
                string src = Path.Combine(projectRoot, assetPath);
                if (!File.Exists(src)) continue;
                string target = Path.Combine(dest, assetPath);
                Directory.CreateDirectory(Path.GetDirectoryName(target));
                File.Copy(src, target, true);
                if (File.Exists(src + ".meta")) File.Copy(src + ".meta", target + ".meta", true);
            }
            return dest;
        }
    }
}
