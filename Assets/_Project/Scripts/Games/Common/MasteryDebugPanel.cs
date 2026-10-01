using System.Linq;
using System.Text;
using NongTrai.Animals;
using NongTrai.Learning;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NongTrai.Games
{
    /// <summary>
    /// Bảng "AI đang nghĩ gì" để demo: mức độ khó, kết quả gần đây và điểm thuộc bài từng con vật.
    /// Nút bật/tắt chỉ hiện trong Editor và bản Development Build, phụ huynh và bé không thấy.
    /// </summary>
    public class MasteryDebugPanel : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] TMP_Text text;
        [SerializeField] Button toggleButton;

        LearningService learning;
        AnimalDatabase database;
        Question current;

        void Start()
        {
            bool allowed = Application.isEditor || Debug.isDebugBuild;
            toggleButton.gameObject.SetActive(allowed);
            panel.SetActive(false);
            if (allowed)
                toggleButton.onClick.AddListener(() =>
                {
                    panel.SetActive(!panel.activeSelf);
                    Redraw();
                });
        }

        public void Bind(LearningService service, AnimalDatabase db)
        {
            learning = service;
            database = db;
        }

        public void Show(Question question)
        {
            current = question;
            Redraw();
        }

        public void Redraw()
        {
            if (!panel.activeSelf || learning == null || database == null) return;

            var sb = new StringBuilder();
            int level = learning.Level;
            sb.AppendLine($"<b>Mức {level}</b>: {Difficulty.OptionCount(level)} lựa chọn" +
                          (Difficulty.UseSimilarDistractors(level) ? ", nhiễu bằng con dễ nhầm" : ""));
            string recent = string.Concat(learning.Progress.recent.Select(r => r ? "o" : "x"));
            sb.AppendLine($"Gần đây ({learning.Progress.recent.Count}/{Difficulty.Window}): {recent}");
            if (current != null)
                sb.AppendLine($"Đang hỏi: <b>{current.Target.NameVi}</b>");
            sb.AppendLine();

            foreach (AnimalData a in database.All.Where(a => a != null)
                         .OrderBy(a => learning.GetMastery(a.Id)))
            {
                float s = learning.GetMastery(a.Id);
                int filled = Mathf.RoundToInt(s * 10);
                string bar = new string('#', filled) + new string('-', 10 - filled);
                string mark = MasteryModel.IsMastered(s) ? " <color=#97C459>thuộc</color>" : "";
                sb.AppendLine($"{a.NameVi,-6} [{bar}] {Mathf.RoundToInt(s * 100),3}%{mark}");
            }
            text.text = sb.ToString();
        }
    }
}
