using System.IO;
using NongTrai.Animals;
using NongTrai.Games;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using B = NongTrai.EditorTools.SoundGameBuilder;

namespace NongTrai.EditorTools
{
    public static class DragGameBuilder
    {
        public static void BuildShadow() => RunShadow(true);

        internal static string RunShadow(bool interactive)
        {
            return Build(interactive, new Spec
            {
                Mode = MatchMode.Shadow,
                SceneName = "GameShadow",
                Title = "Tìm cái bóng",
                TileLabel = "cái bóng",
                Hint = "Kéo bạn vào đúng cái bóng nhé",
                VoiceNames = new[] { "voice_game_shadow", "game2" },
                Tone = UiTheme.Tone.Turquoise,
                Tray = B.Hex("E1F5EE"),
                Ink = B.Hex("0F6E56"),
            });
        }

        public static void BuildFood() => RunFood(true);

        internal static string RunFood(bool interactive)
        {
            return Build(interactive, new Spec
            {
                Mode = MatchMode.Food,
                SceneName = "GameFood",
                Title = "Cho bạn ăn",
                TileLabel = "bạn ăn",
                Hint = "Kéo thức ăn tới bạn thích ăn món này",
                VoiceNames = new[] { "voice_game_food", "game3" },
                Tone = UiTheme.Tone.Green,
                Tray = B.Hex("EAF3DE"),
                Ink = B.Hex("3B6D11"),
            });
        }

        class Spec
        {
            public MatchMode Mode;
            public string SceneName;
            public string Title;
            public string TileLabel;
            public string Hint;
            public string[] VoiceNames;
            public UiTheme.Tone Tone;
            public Color Tray;
            public Color Ink;
        }

        static string Build(bool interactive, Spec spec)
        {
            if (B.FindByType<AnimalDatabase>() == null)
            {
                if (interactive) EditorUtility.DisplayDialog("Thiếu dữ liệu", "Chưa tìm thấy file AnimalDatabase trong project.", "OK");
                return $"{spec.SceneName}: thiếu AnimalDatabase, bỏ qua.";
            }

            string mainMenuPath = B.FindScenePath("MainMenu");
            string sceneFolder = mainMenuPath != null
                ? Path.GetDirectoryName(mainMenuPath).Replace('\\', '/')
                : "Assets/_Project/Scenes";
            string scenePath = sceneFolder + "/" + spec.SceneName + ".unity";
            string prefabFolder = AssetDatabase.IsValidFolder("Assets/_Project/Prefab") ? "Assets/_Project/Prefab" : "Assets/_Project/Prefabs/UI";

            if (interactive)
            {
                if (File.Exists(scenePath) &&
                    !EditorUtility.DisplayDialog("Tạo lại?", $"{scenePath} đã có. Xóa và tạo lại từ đầu?", "Tạo lại", "Hủy"))
                    return null;
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    return null;
            }

            string menuResult = B.ConnectGameTile(mainMenuPath, spec.SceneName, spec.TileLabel, out string profileSceneName);

            B.LoadSkin();
            AudioClip pop = B.FindByName<AudioClip>("ui_pop");

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            OptionCardView cardPrefab = B.BuildCardPrefab(pop, prefabFolder);

            B.CreateCamera();
            B.CreateEventSystem();
            RectTransform canvas = B.CreateCanvas();

            B.BuildHeader(canvas, spec.Title, spec.Tone, pop, out Button home, out RoundProgressView progressView);

            RectTransform tray = B.NewUI("Tray", canvas);
            B.Place(tray, new Vector2(0.5f, 0.5f), new Vector2(0, 185), new Vector2(300, 300));
            Sprite round = UiTheme.RoundedBox;
            Image trayImage = B.AddImage(tray, Color.white, round != null ? round : B.Knob, false);
            UiTheme.FitCorners(trayImage, 0.5f, 1000f);
            RectTransform trayInner = B.NewUI("Inner", tray);
            B.Stretch(trayInner, 14);
            Image innerImage = B.AddImage(trayInner, spec.Tray, round != null ? round : B.Knob, false);
            UiTheme.FitCorners(innerImage, 0.5f, 1000f);

            B.Pill(canvas, "HintText", new Vector2(0.5f, 0.5f), new Vector2(0, 0), new Vector2(720, 64), spec.Hint, 30, spec.Ink);

            RectTransform options = B.NewUI("Options", canvas);
            B.Place(options, new Vector2(0.5f, 0.5f), new Vector2(0, -240), new Vector2(1760, 340));
            B.ConfigureRow(options.gameObject.AddComponent<HorizontalLayoutGroup>(), 50, TextAnchor.MiddleCenter);

            RectTransform item = B.NewUI("DragItem", canvas);
            B.Place(item, new Vector2(0.5f, 0.5f), new Vector2(0, 185), new Vector2(230, 230));
            Image itemImage = B.AddImage(item, Color.white, null, true);
            itemImage.preserveAspect = true;
            item.gameObject.AddComponent<CanvasGroup>();
            DraggableItem draggable = item.gameObject.AddComponent<DraggableItem>();
            B.Set(draggable, "image", itemImage);

            RoundEndPanel endPanel = B.BuildEndPanel(canvas, pop, spec.Ink);
            MasteryDebugPanel debugPanel = B.BuildDebugPanel(canvas);

            DragMatchGameController controller = canvas.gameObject.AddComponent<DragMatchGameController>();
            B.Set(controller, "database", B.FindByType<AnimalDatabase>());
            SetEnum(controller, "mode", (int)spec.Mode);
            B.Set(controller, "optionsContainer", options);
            B.Set(controller, "optionPrefab", cardPrefab);
            B.Set(controller, "dragItem", draggable);
            B.Set(controller, "homeButton", home);
            B.Set(controller, "progressView", progressView);
            B.Set(controller, "endPanel", endPanel);
            B.Set(controller, "debugPanel", debugPanel);
            B.Set(controller, "correctSound", B.FindByName<AudioClip>("ui_correct"));
            B.Set(controller, "wrongSound", B.FindByName<AudioClip>("ui_wrong"));
            B.Set(controller, "celebrateSound", B.FindByName<AudioClip>("ui_celebrate"));
            AudioClip voice = null;
            foreach (string name in spec.VoiceNames)
                if (voice == null) voice = B.FindByName<AudioClip>(name);
            B.Set(controller, "promptVoice", voice);
            B.SetString(controller, "mainMenuScene", mainMenuPath != null ? Path.GetFileNameWithoutExtension(mainMenuPath) : "MainMenu");
            B.SetString(controller, "profileScene", profileSceneName);

            B.EnsureFolder(sceneFolder);
            EditorSceneManager.SaveScene(scene, scenePath);
            B.AddToBuildSettings(scenePath);

            if (interactive)
                EditorUtility.DisplayDialog($"Đã tạo xong màn {spec.SceneName}",
                    $"Scene: {scenePath}\nĐã thêm vào Build Profiles.\n\nMain Menu: {menuResult}\n\n" +
                    "Chạy thử: menu NongTrai > Chạy thử từ scene đầu tiên.", "OK");
            return $"{spec.SceneName}: đã dựng lại ({scenePath}).";
        }

        static void SetEnum(Object target, string field, int index)
        {
            var so = new SerializedObject(target);
            SerializedProperty p = so.FindProperty(field);
            if (p == null)
            {
                Debug.LogError($"[DragGameBuilder] Không thấy trường \"{field}\" trên {target.GetType().Name}.");
                return;
            }
            p.enumValueIndex = index;
            so.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
