using System;
using System.IO;
using System.Linq;
using System.Text;
using NongTrai.Animals;
using NongTrai.Games;
using NongTrai.Games.Sound;
using NongTrai.MainMenu;
using NongTrai.UI;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace NongTrai.EditorTools
{
    public static class SoundGameBuilder
    {
        const string SceneName = "GameSound";

        internal static readonly Color Cream = Hex("FFF8E7");
        internal static readonly Color Ink = Hex("444441");
        internal static readonly Color Soft = Hex("F1EFE8");
        internal static readonly Color Coral = Hex("FAECE7");
        internal static readonly Color CoralInk = Hex("993C1D");
        internal static readonly Color Green = Hex("97C459");

        internal static Sprite rounded;
        internal static Sprite knob;

        public static void Build() => Run(true);

        internal static string Run(bool interactive)
        {
            AnimalDatabase db = FindByType<AnimalDatabase>();
            if (db == null)
            {
                if (interactive)
                    EditorUtility.DisplayDialog("Thiếu dữ liệu",
                        "Chưa tìm thấy file AnimalDatabase trong project.\nHãy làm Bước 5 trong tài liệu trước.", "OK");
                return "GameSound: thiếu AnimalDatabase, bỏ qua.";
            }
            string mainMenuPath = FindScenePath("MainMenu");
            string sceneFolder = mainMenuPath != null
                ? Path.GetDirectoryName(mainMenuPath).Replace('\\', '/')
                : "Assets/_Project/Scenes";
            string scenePath = sceneFolder + "/" + SceneName + ".unity";
            string prefabFolder = AssetDatabase.IsValidFolder("Assets/_Project/Prefab") ? "Assets/_Project/Prefab" : "Assets/_Project/Prefabs/UI";

            if (interactive)
            {
                if (File.Exists(scenePath) &&
                    !EditorUtility.DisplayDialog("Tạo lại?", $"{scenePath} đã có. Xóa và tạo lại từ đầu?", "Tạo lại", "Hủy"))
                    return null;
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    return null;
            }

            string menuResult = ConnectMainMenu(mainMenuPath, out string profileSceneName);

            LoadSkin();
            Sprite speakerIcon = FindSprite("icon_speaker");
            AudioClip pop = FindByName<AudioClip>("ui_pop");

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            OptionCardView cardPrefab = BuildCardPrefab(pop, prefabFolder);

            CreateCamera();
            CreateEventSystem();
            RectTransform canvas = CreateCanvas();

            BuildHeader(canvas, "Ai kêu thế nhỉ?", UiTheme.Tone.Pink, pop, out Button home, out RoundProgressView progressView);

            Sprite speakerButton = UiTheme.RoundSpeaker(UiTheme.Tone.Pink);
            Button replay = speakerButton != null
                ? MakeButton(canvas, "ReplayButton", new Vector2(0.5f, 0.5f), new Vector2(0, 190), new Vector2(260, 260),
                    Color.white, speakerButton, null, null, 0, Ink, null)
                : MakeButton(canvas, "ReplayButton", new Vector2(0.5f, 0.5f), new Vector2(0, 190), new Vector2(240, 240),
                    Coral, knob, speakerIcon, speakerIcon == null ? "Nghe" : null, 48, CoralInk, null);
            Pill(canvas, "ReplayHint", new Vector2(0.5f, 0.5f), new Vector2(0, 25), new Vector2(460, 64), "Bấm loa để nghe lại", 30, Ink);

            RectTransform options = NewUI("Options", canvas);
            Place(options, new Vector2(0.5f, 0.5f), new Vector2(0, -230), new Vector2(1760, 340));
            ConfigureRow(options.gameObject.AddComponent<HorizontalLayoutGroup>(), 50, TextAnchor.MiddleCenter);

            RoundEndPanel endPanel = BuildEndPanel(canvas, pop, Hex("993556"));

            MasteryDebugPanel debugPanel = BuildDebugPanel(canvas);

            SoundGameController controller = canvas.gameObject.AddComponent<SoundGameController>();
            Set(controller, "database", FindByType<AnimalDatabase>());
            Set(controller, "optionsContainer", options);
            Set(controller, "optionPrefab", cardPrefab);
            Set(controller, "replayButton", replay);
            Set(controller, "homeButton", home);
            Set(controller, "progressView", progressView);
            Set(controller, "endPanel", endPanel);
            Set(controller, "debugPanel", debugPanel);
            Set(controller, "correctSound", FindByName<AudioClip>("ui_correct"));
            Set(controller, "wrongSound", FindByName<AudioClip>("ui_wrong"));
            Set(controller, "celebrateSound", FindByName<AudioClip>("ui_celebrate"));
            Set(controller, "promptVoice", FindByName<AudioClip>("voice_game_sound") ?? FindByName<AudioClip>("game1"));
            SetString(controller, "mainMenuScene", mainMenuPath != null ? Path.GetFileNameWithoutExtension(mainMenuPath) : "MainMenu");
            SetString(controller, "profileScene", profileSceneName);

            EnsureFolder(sceneFolder);
            EditorSceneManager.SaveScene(scene, scenePath);
            AddToBuildSettings(scenePath);

            if (interactive)
            {
                string missing = "";
                if (FindSprite("icon_home") == null || FindSprite("icon_star") == null) missing += "\n- Thiếu icon (icon_home / icon_star): đang dùng chữ hoặc hình tròn thay thế.";
                if (pop == null) missing += "\n- Thiếu ui_pop: nút bấm sẽ không có tiếng.";
                EditorUtility.DisplayDialog("Đã tạo xong màn GameSound",
                    $"Scene: {scenePath}\nPrefab: {prefabFolder}/OptionCard.prefab\nĐã thêm vào Build Profiles.\n\nMain Menu: {menuResult}{missing}\n\n" +
                    "Chạy thử: menu NongTrai > Chạy thử từ scene đầu tiên.", "OK");
            }
            return $"GameSound: đã dựng lại ({scenePath}).";
        }

        internal static void BuildHeader(RectTransform canvas, string title, UiTheme.Tone tone, AudioClip pop,
            out Button home, out RoundProgressView progressView)
        {
            RectTransform bg = NewUI("Background", canvas);
            Stretch(bg);
            Sprite bgSprite = UiTheme.Background;
            AddImage(bg, bgSprite != null ? Color.white : Cream, bgSprite, false);

            Sprite homeIcon = FindSprite("icon_home");
            Sprite homeBlank = UiTheme.RoundBlank(UiTheme.Tone.LightBlue);
            home = homeBlank != null
                ? MakeButton(canvas, "HomeButton", new Vector2(0, 1), new Vector2(36, -30), new Vector2(140, 140),
                    Color.white, homeBlank, homeIcon, homeIcon == null ? "Nhà" : null, 34, Color.white, pop)
                : MakeButton(canvas, "HomeButton", new Vector2(0, 1), new Vector2(40, -40), new Vector2(130, 130),
                    Soft, knob, homeIcon, homeIcon == null ? "Nhà" : null, 34, Ink, pop);

            RectTransform banner = NewUI("TitleBanner", canvas);
            Place(banner, new Vector2(0.5f, 1), new Vector2(0, -24), new Vector2(680, 150));
            Sprite big = UiTheme.Big(tone);
            AddImage(banner, big != null ? Color.white : Coral, big != null ? big : rounded, false);
            RectTransform prompt = NewUI("Prompt", banner);
            Stretch(prompt);
            prompt.offsetMin = new Vector2(40, 18);
            prompt.offsetMax = new Vector2(-40, -10);
            AddText(prompt, title, 62, big != null ? Color.white : CoralInk);

            Sprite starIcon = FindSprite("icon_star");
            RectTransform progress = NewUI("Progress", canvas);
            Place(progress, new Vector2(1, 1), new Vector2(-30, -52), new Vector2(510, 84));
            AddImage(progress, new Color(1, 1, 1, 0.9f), UiTheme.RoundedBox ?? rounded, false);
            HorizontalLayoutGroup pl = progress.gameObject.AddComponent<HorizontalLayoutGroup>();
            ConfigureRow(pl, 8, TextAnchor.MiddleCenter);
            RectTransform dot = NewUI("DotTemplate", progress);
            dot.sizeDelta = new Vector2(52, 52);
            Image dotImage = AddImage(dot, Color.white, starIcon != null ? starIcon : knob, false);
            dotImage.preserveAspect = true;
            dot.gameObject.SetActive(false);
            progressView = progress.gameObject.AddComponent<RoundProgressView>();
            Set(progressView, "dotTemplate", dotImage);
        }

        internal static TMP_Text Pill(Transform parent, string name, Vector2 anchor, Vector2 position, Vector2 size,
            string text, float fontSize, Color color)
        {
            RectTransform rt = NewUI(name, parent);
            Place(rt, anchor, position, size);
            AddImage(rt, new Color(1, 1, 1, 0.85f), UiTheme.RoundedBox ?? rounded, false);
            RectTransform label = NewUI("Text", rt);
            Stretch(label, 6);
            return AddText(label, text, fontSize, color);
        }

        public static void PlayFromFirstScene()
        {
            string path = EditorBuildSettings.scenes.FirstOrDefault(s => s.enabled)?.path;
            if (string.IsNullOrEmpty(path))
            {
                EditorUtility.DisplayDialog("Chưa có scene", "Build Profiles chưa có scene nào được tick.", "OK");
                return;
            }
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            EditorApplication.isPlaying = true;
        }

        internal static OptionCardView BuildCardPrefab(AudioClip tapSound, string prefabFolder)
        {
            EnsureFolder(prefabFolder);

            var root = new GameObject("OptionCard", typeof(RectTransform));
            root.layer = LayerMask.NameToLayer("UI");
            var rt = (RectTransform)root.transform;
            rt.sizeDelta = new Vector2(300, 300);

            Sprite card = UiTheme.Card;
            Image bg = AddImage(rt, Color.white, card != null ? card : rounded, true);
            Button button = root.AddComponent<Button>();
            button.targetGraphic = bg;
            ColorBlock colors = button.colors;
            colors.disabledColor = Color.white;
            colors.highlightedColor = Hex("FFF4E0");
            button.colors = colors;
            CanvasGroup group = root.AddComponent<CanvasGroup>();

            RectTransform icon = NewUI("Icon", rt);
            Stretch(icon, 28);
            Image iconImage = AddImage(icon, Color.white, null, false);
            iconImage.preserveAspect = true;

            OptionCardView view = root.AddComponent<OptionCardView>();
            Set(view, "button", button);
            Set(view, "icon", iconImage);
            Set(view, "canvasGroup", group);
            Set(view, "tapSound", tapSound);

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, prefabFolder + "/OptionCard.prefab");
            Object.DestroyImmediate(root);
            return prefab.GetComponent<OptionCardView>();
        }

        internal static RoundEndPanel BuildEndPanel(RectTransform canvas, AudioClip pop, Color? titleColor = null)
        {
            RectTransform root = NewUI("EndPanel", canvas);
            Stretch(root);

            RectTransform dim = NewUI("Dim", root);
            Stretch(dim);
            AddImage(dim, new Color(0, 0, 0, 0.45f), null, true);

            RectTransform panel = NewUI("Panel", root);
            Place(panel, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(960, 620));
            Sprite card = UiTheme.Card;
            AddImage(panel, Color.white, card != null ? card : rounded, true);

            RectTransform title = NewUI("Title", panel);
            Place(title, new Vector2(0.5f, 1), new Vector2(0, -60), new Vector2(880, 120));
            TMP_Text titleText = AddText(title, "Giỏi quá!", 84, titleColor ?? CoralInk);

            RectTransform detail = NewUI("Detail", panel);
            Place(detail, new Vector2(0.5f, 0.5f), new Vector2(0, 30), new Vector2(860, 200));
            TMP_Text detailText = AddText(detail, "Bé đúng ngay 8/8 câu", 42, Ink);

            Sprite green = UiTheme.Big(UiTheme.Tone.Green);
            Sprite blue = UiTheme.Big(UiTheme.Tone.LightBlue);
            Button again = MakeButton(panel, "AgainButton", new Vector2(0.5f, 0), new Vector2(-185, 60), new Vector2(330, 140),
                green != null ? Color.white : Green, green != null ? green : rounded, null, "Chơi tiếp", 44, Color.white, pop);
            Button home = MakeButton(panel, "HomeButton", new Vector2(0.5f, 0), new Vector2(185, 60), new Vector2(330, 140),
                blue != null ? Color.white : Soft, blue != null ? blue : rounded, null, "Về nhà", 44, blue != null ? Color.white : Ink, pop);

            RoundEndPanel endPanel = root.gameObject.AddComponent<RoundEndPanel>();
            Set(endPanel, "titleText", titleText);
            Set(endPanel, "detailText", detailText);
            Set(endPanel, "againButton", again);
            Set(endPanel, "homeButton", home);
            root.gameObject.SetActive(false);
            return endPanel;
        }

        internal static MasteryDebugPanel BuildDebugPanel(RectTransform canvas)
        {
            RectTransform root = NewUI("AiDebug", canvas);
            Stretch(root);

            Sprite purple = UiTheme.Big(UiTheme.Tone.Purple);
            Button toggle = MakeButton(root, "AiToggle", Vector2.zero, new Vector2(24, 20), new Vector2(150, 84),
                purple != null ? Color.white : Hex("3C3489"), purple != null ? purple : rounded, null, "AI", 32, Color.white, null);

            RectTransform panel = NewUI("AiPanel", root);
            Place(panel, Vector2.zero, new Vector2(30, 110), new Vector2(720, 640));
            AddImage(panel, new Color(0.12f, 0.1f, 0.25f, 0.88f), UiTheme.RoundedBox ?? rounded, true);
            RectTransform textRt = NewUI("Text", panel);
            Stretch(textRt, 24);
            TMP_Text text = AddText(textRt, "", 28, Color.white, false, TextAlignmentOptions.TopLeft);
            panel.gameObject.SetActive(false);

            MasteryDebugPanel debug = root.gameObject.AddComponent<MasteryDebugPanel>();
            Set(debug, "panel", panel.gameObject);
            Set(debug, "text", text);
            Set(debug, "toggleButton", toggle);
            return debug;
        }

        internal static string ConnectMainMenu(string path, out string profileSceneName)
        {
            profileSceneName = "ProfileSelect";
            if (path == null) return "không tìm thấy scene MainMenu, hãy tự điền Scene Name = GameSound.";

            Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            MainMenuScreen screen = Object.FindObjectsByType<MainMenuScreen>(FindObjectsInactive.Include)
                .FirstOrDefault();
            if (screen == null) return "scene MainMenu chưa có component Main Menu Screen.";

            var so = new SerializedObject(screen);
            SerializedProperty profileProp = so.FindProperty("profileSceneName");
            if (profileProp != null && !string.IsNullOrEmpty(profileProp.stringValue))
                profileSceneName = profileProp.stringValue;
            SerializedProperty games = so.FindProperty("games");
            if (games == null || games.arraySize == 0)
                return "ô Games trong Main Menu Screen đang trống, hãy thêm ô trò chơi rồi chạy lại.";

            for (int i = 0; i < games.arraySize; i++)
                if (games.GetArrayElementAtIndex(i).FindPropertyRelative("sceneName").stringValue == SceneName)
                    return $"ô số {i} đã trỏ tới {SceneName}, không cần sửa.";

            int index = 0;
            for (int i = 0; i < games.arraySize; i++)
            {
                Object button = games.GetArrayElementAtIndex(i).FindPropertyRelative("button").objectReferenceValue;
                if (button != null && button.name.IndexOf("Sound", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    index = i;
                    break;
                }
            }
            games.GetArrayElementAtIndex(index).FindPropertyRelative("sceneName").stringValue = SceneName;
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            return $"đã nối ô số {index} với {SceneName}.";
        }

        internal static void LoadSkin()
        {
            rounded = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            knob = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        }

        internal static Sprite Rounded => rounded;
        internal static Sprite Knob => knob;

        internal static string ConnectGameTile(string path, string sceneName, string label, out string profileSceneName)
        {
            profileSceneName = "ProfileSelect";
            if (path == null) return $"không tìm thấy scene MainMenu, hãy tự điền Scene Name = {sceneName}.";

            Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            MainMenuScreen screen = Object.FindObjectsByType<MainMenuScreen>(FindObjectsInactive.Include).FirstOrDefault();
            if (screen == null) return "scene MainMenu chưa có component Main Menu Screen.";

            var so = new SerializedObject(screen);
            SerializedProperty profileProp = so.FindProperty("profileSceneName");
            if (profileProp != null && !string.IsNullOrEmpty(profileProp.stringValue))
                profileSceneName = profileProp.stringValue;

            SerializedProperty games = so.FindProperty("games");
            for (int i = 0; i < games.arraySize; i++)
                if (games.GetArrayElementAtIndex(i).FindPropertyRelative("sceneName").stringValue == sceneName)
                    return $"ô số {i} đã trỏ tới {sceneName}, không cần sửa.";

            string want = NormalizeText(label);
            Button button = Object.FindObjectsByType<Button>(FindObjectsInactive.Include)
                .FirstOrDefault(b => b.GetComponentsInChildren<TMP_Text>(true).Any(t => NormalizeText(t.text).Contains(want)));
            if (button == null)
                return $"không tìm thấy ô có chữ \"{label}\" ở MainMenu, hãy tự thêm ô với Scene Name = {sceneName}.";

            int index = -1;
            for (int i = 0; i < games.arraySize; i++)
                if (games.GetArrayElementAtIndex(i).FindPropertyRelative("button").objectReferenceValue == button)
                    index = i;
            if (index < 0)
            {
                index = games.arraySize;
                games.arraySize++;
                games.GetArrayElementAtIndex(index).FindPropertyRelative("button").objectReferenceValue = button;
            }
            games.GetArrayElementAtIndex(index).FindPropertyRelative("sceneName").stringValue = sceneName;
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            return $"đã nối ô \"{button.name}\" (số {index}) với {sceneName}.";
        }

        static string NormalizeText(string s) => (s ?? "").Normalize(NormalizationForm.FormC).ToLowerInvariant();

        internal static void CreateCamera()
        {
            var go = new GameObject("Main Camera");
            go.tag = "MainCamera";
            go.transform.position = new Vector3(0, 0, -10);
            Camera cam = go.AddComponent<Camera>();
            cam.orthographic = true;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Cream;
            go.AddComponent<AudioListener>();
        }

        internal static void CreateEventSystem()
        {
            var go = new GameObject("EventSystem", typeof(EventSystem));
            Type inputSystemModule = Type.GetType("UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");
            if (inputSystemModule != null) go.AddComponent(inputSystemModule);
            else go.AddComponent<StandaloneInputModule>();
        }

        internal static RectTransform CreateCanvas()
        {
            var go = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            go.layer = LayerMask.NameToLayer("UI");
            go.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = go.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            return (RectTransform)go.transform;
        }

        internal static RectTransform NewUI(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.layer = LayerMask.NameToLayer("UI");
            go.transform.SetParent(parent, false);
            return (RectTransform)go.transform;
        }

        internal static void Stretch(RectTransform rt, float padding = 0)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.offsetMin = new Vector2(padding, padding);
            rt.offsetMax = new Vector2(-padding, -padding);
        }

        internal static void Place(RectTransform rt, Vector2 anchor, Vector2 position, Vector2 size)
        {
            rt.anchorMin = anchor;
            rt.anchorMax = anchor;
            rt.pivot = anchor;
            rt.anchoredPosition = position;
            rt.sizeDelta = size;
        }

        internal static void ConfigureRow(HorizontalLayoutGroup row, float spacing, TextAnchor alignment)
        {
            row.spacing = spacing;
            row.childAlignment = alignment;
            row.childControlWidth = false;
            row.childControlHeight = false;
            row.childForceExpandWidth = false;
            row.childForceExpandHeight = false;
        }

        internal static Image AddImage(RectTransform rt, Color color, Sprite sprite, bool raycast)
        {
            Image img = rt.gameObject.AddComponent<Image>();
            img.sprite = sprite;
            img.color = color;
            if (sprite != null && sprite.border != Vector4.zero)
            {
                img.type = Image.Type.Sliced;
                bool bigButton = Mathf.Max(sprite.border.y, sprite.border.w) >= 80f;
                UiTheme.FitCorners(img, bigButton ? 0.42f : 0.25f, bigButton ? 70f : 44f);
            }
            img.raycastTarget = raycast;
            return img;
        }

        internal static TextMeshProUGUI AddText(RectTransform rt, string text, float size, Color color, bool bold = false,
            TextAlignmentOptions alignment = TextAlignmentOptions.Center)
        {
            TextMeshProUGUI t = rt.gameObject.AddComponent<TextMeshProUGUI>();
            TMP_FontAsset font = UiTheme.Font;
            if (font != null) t.font = font;
            t.text = text;
            t.fontSize = size;
            t.color = color;
            t.alignment = alignment;
            if (bold && font == null) t.fontStyle = FontStyles.Bold;
            t.raycastTarget = false;
            return t;
        }

        internal static Button MakeButton(Transform parent, string name, Vector2 anchor, Vector2 position, Vector2 size,
            Color color, Sprite background, Sprite icon, string label, float labelSize, Color labelColor, AudioClip clickSound)
        {
            RectTransform rt = NewUI(name, parent);
            Place(rt, anchor, position, size);
            Image img = AddImage(rt, color, background, true);
            Button button = rt.gameObject.AddComponent<Button>();
            button.targetGraphic = img;

            if (icon != null)
            {
                RectTransform iconRt = NewUI("Icon", rt);
                Stretch(iconRt, size.x * 0.24f);
                AddImage(iconRt, Color.white, icon, false).preserveAspect = true;
            }
            if (!string.IsNullOrEmpty(label))
            {
                RectTransform labelRt = NewUI("Label", rt);
                Stretch(labelRt, 8);
                AddText(labelRt, label, labelSize, labelColor, true);
            }

            KidButton kid = rt.gameObject.AddComponent<KidButton>();
            Set(kid, "clickSound", clickSound);
            return button;
        }

        internal static void Set(Object target, string field, Object value)
        {
            var so = new SerializedObject(target);
            SerializedProperty p = so.FindProperty(field);
            if (p == null)
            {
                Debug.LogError($"[SoundGameBuilder] Không thấy trường \"{field}\" trên {target.GetType().Name}.");
                return;
            }
            p.objectReferenceValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        internal static void SetString(Object target, string field, string value)
        {
            var so = new SerializedObject(target);
            SerializedProperty p = so.FindProperty(field);
            if (p == null)
            {
                Debug.LogError($"[SoundGameBuilder] Không thấy trường \"{field}\" trên {target.GetType().Name}.");
                return;
            }
            p.stringValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        internal static T FindByType<T>() where T : Object
        {
            string guid = AssetDatabase.FindAssets("t:" + typeof(T).Name).FirstOrDefault();
            return guid == null ? null : AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
        }

        internal static T FindByName<T>(string fileName) where T : Object
        {
            foreach (string guid in AssetDatabase.FindAssets(fileName + " t:" + typeof(T).Name))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetFileNameWithoutExtension(path) == fileName)
                    return AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return null;
        }

        internal static Sprite FindSprite(string fileName)
        {
            foreach (string guid in AssetDatabase.FindAssets(fileName + " t:Texture2D"))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetFileNameWithoutExtension(path) != fileName) continue;

                if (AssetImporter.GetAtPath(path) is TextureImporter importer && importer.textureType != TextureImporterType.Sprite)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Single;
                    importer.SaveAndReimport();
                }
                return AssetDatabase.LoadAssetAtPath<Sprite>(path);
            }
            return null;
        }

        internal static string FindScenePath(string sceneName)
        {
            foreach (string guid in AssetDatabase.FindAssets(sceneName + " t:Scene"))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetFileNameWithoutExtension(path) == sceneName)
                    return path;
            }
            return null;
        }

        internal static void EnsureFolder(string folder)
        {
            if (AssetDatabase.IsValidFolder(folder)) return;
            string parent = Path.GetDirectoryName(folder).Replace('\\', '/');
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, Path.GetFileName(folder));
        }

        internal static void AddToBuildSettings(string path)
        {
            var scenes = EditorBuildSettings.scenes.ToList();
            if (scenes.Any(s => s.path == path)) return;
            scenes.Add(new EditorBuildSettingsScene(path, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        internal static Color Hex(string hex)
        {
            ColorUtility.TryParseHtmlString("#" + hex, out Color c);
            return c;
        }
    }
}
