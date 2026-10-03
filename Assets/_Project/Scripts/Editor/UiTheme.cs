using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace NongTrai.EditorTools
{
    internal static class UiTheme
    {
        internal enum Tone { Purple = 1, Pink = 3, Green = 5, Turquoise = 7, LightBlue = 9, Blue = 11, Gray = 13 }

        const string FontName = "Mali-Bold SDF";

        const float RoundBorder = 44f;
        const float CardBorder = 48f;
        static readonly Vector4 BigBorder = new Vector4(100, 90, 100, 90);

        static TMP_FontAsset font;

        internal static TMP_FontAsset Font
        {
            get
            {
                if (font != null) return font;
                foreach (string guid in AssetDatabase.FindAssets(FontName + " t:TMP_FontAsset"))
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (Path.GetFileNameWithoutExtension(path) == FontName)
                        return font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
                }
                return null;
            }
        }

        internal static bool ApplyFont(TMP_Text text)
        {
            TMP_FontAsset f = Font;
            if (f == null || text == null || text.font == f) return false;
            text.font = f;
            text.fontSharedMaterial = f.material;
            text.fontStyle &= ~FontStyles.Bold;
            EditorUtility.SetDirty(text);
            return true;
        }

        internal static bool SetDefaultFont()
        {
            TMP_FontAsset f = Font;
            TMP_Settings settings = Resources.Load<TMP_Settings>("TMP Settings");
            if (f == null || settings == null) return false;
            var so = new SerializedObject(settings);
            SerializedProperty p = so.FindProperty("m_defaultFontAsset");
            if (p == null) return false;
            if (p.objectReferenceValue == f) return true;
            p.objectReferenceValue = f;
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(settings);
            return true;
        }

        internal static Sprite Background => SoundGameBuilder.FindSprite("bg_farm_soft");
        internal static Sprite RoundedBox => SlicedSprite("ui_round", Vector4.one * RoundBorder);
        internal static Sprite Card => SlicedSprite("ui_card", Vector4.one * CardBorder);
        internal static Sprite RoundBlank(Tone t) => SoundGameBuilder.FindSprite($"ButtonsStyle{(int)t}_01");
        internal static Sprite RoundSpeaker(Tone t) => SoundGameBuilder.FindSprite($"ButtonsStyle{(int)t}_23");

        internal static Sprite Big(Tone t)
        {
            string name;
            switch (t)
            {
                case Tone.Pink: name = "BigPink"; break;
                case Tone.Green: name = "BigGreen"; break;
                case Tone.Turquoise: name = "BigTurquoise"; break;
                case Tone.LightBlue: name = "BigLigthBlue"; break;
                case Tone.Blue: name = "BigBlue"; break;
                case Tone.Gray: name = "BigGray"; break;
                default: name = "BigPurple"; break;
            }
            return SlicedSprite(name, BigBorder);
        }

        internal static Sprite SlicedSprite(string fileName, Vector4 border)
        {
            Sprite sprite = SoundGameBuilder.FindSprite(fileName);
            if (sprite == null) return null;
            string path = AssetDatabase.GetAssetPath(sprite);
            if (AssetImporter.GetAtPath(path) is TextureImporter importer)
            {
                var settings = new TextureImporterSettings();
                importer.ReadTextureSettings(settings);
                bool changed = importer.spriteBorder != border
                    || settings.spriteMeshType != SpriteMeshType.FullRect
                    || importer.wrapMode != TextureWrapMode.Clamp;
                if (changed)
                {
                    settings.spriteMeshType = SpriteMeshType.FullRect;
                    importer.SetTextureSettings(settings);
                    importer.spriteBorder = border;
                    importer.wrapMode = TextureWrapMode.Clamp;
                    importer.SaveAndReimport();
                    sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                }
            }
            return sprite;
        }

        internal static void ApplySliced(Image img, Sprite sprite, float ratio = 0.25f, float maxRadius = 44f)
        {
            if (img == null || sprite == null) return;
            img.sprite = sprite;
            img.type = Image.Type.Sliced;
            img.fillCenter = true;
            FitCorners(img, ratio, maxRadius);
            EditorUtility.SetDirty(img);
        }

        internal static void FitCorners(Image img, float ratio, float maxRadius)
        {
            Sprite s = img.sprite;
            if (s == null || s.border == Vector4.zero) return;
            Rect r = img.rectTransform.rect;
            float minSide = Mathf.Min(r.width, r.height);
            if (minSide <= 1f) minSide = 100f;
            float borderPx = Mathf.Max(s.border.y, s.border.w);
            float desired = Mathf.Clamp(minSide * ratio, 10f, maxRadius);
            img.pixelsPerUnitMultiplier = Mathf.Max(0.05f, borderPx / desired);
        }

        internal static bool IsBuiltinPlainSprite(Sprite s)
        {
            if (s == null) return true;
            string path = AssetDatabase.GetAssetPath(s);
            if (!path.StartsWith("Resources/unity_builtin_extra") && !path.StartsWith("Library/")) return false;
            return s.name == "UISprite" || s.name == "Background" || s.name == "InputFieldBackground";
        }

        internal static bool NameLooksLikeBackground(string name)
        {
            string n = name.ToLowerInvariant();
            return new[] { "background", "bg", "dim", "overlay", "mask", "viewport", "blocker", "shade" }.Any(n.Contains);
        }
    }
}
