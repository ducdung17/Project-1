using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NongTrai.Games
{
    /// <summary>Hàng ngôi sao cho biết bé đã làm bao nhiêu câu trong lượt.</summary>
    public class RoundProgressView : MonoBehaviour
    {
        [Tooltip("Một ngôi sao mẫu (để TẮT). Script nhân bản nó.")]
        [SerializeField] Image dotTemplate;
        [SerializeField] Color emptyColor = new Color32(0xE0, 0xDC, 0xD3, 0xFF);
        [Tooltip("Đúng ngay lần đầu.")]
        [SerializeField] Color perfectColor = new Color32(0xEF, 0x9F, 0x27, 0xFF);
        [Tooltip("Đúng sau khi chọn lại.")]
        [SerializeField] Color retryColor = new Color32(0x97, 0xC4, 0x59, 0xFF);

        readonly List<Image> dots = new List<Image>();

        public void Build(int total)
        {
            foreach (Image d in dots) Destroy(d.gameObject);
            dots.Clear();
            dotTemplate.gameObject.SetActive(false);

            for (int i = 0; i < total; i++)
            {
                Image dot = Instantiate(dotTemplate, dotTemplate.transform.parent);
                dot.gameObject.SetActive(true);
                dot.color = emptyColor;
                dots.Add(dot);
            }
        }

        public void Fill(int index, bool perfect)
        {
            if (index < 0 || index >= dots.Count) return;
            dots[index].color = perfect ? perfectColor : retryColor;
        }
    }
}
