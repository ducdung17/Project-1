using System;
using System.Collections.Generic;
using System.Linq;

namespace NongTrai.Profiles
{
    /// <summary>
    /// Câu hỏi "cổng phụ huynh": một phép cộng mà người lớn giải trong 2 giây,
    /// còn trẻ mầm non chưa làm được. Logic thuần C# để unit test được.
    /// </summary>
    public class ParentGateChallenge
    {
        public int A { get; }
        public int B { get; }
        public int Answer => A + B;
        public IReadOnlyList<int> Options { get; }

        public string Question => $"{A} + {B} = ?";

        ParentGateChallenge(int a, int b, IReadOnlyList<int> options)
        {
            A = a;
            B = b;
            Options = options;
        }

        public bool Check(int chosen) => chosen == Answer;

        /// <param name="optionCount">Số đáp án hiển thị (gồm cả đáp án đúng).</param>
        public static ParentGateChallenge Generate(Random random, int optionCount = 3)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            if (optionCount < 2) throw new ArgumentOutOfRangeException(nameof(optionCount));

            // Cả hai số từ 3..9 và tổng >= 10 để phải "nhớ", trẻ nhỏ khó đoán mò.
            int a, b;
            do
            {
                a = random.Next(3, 10);
                b = random.Next(3, 10);
            } while (a + b < 10);

            int answer = a + b;
            var options = new HashSet<int> { answer };
            while (options.Count < optionCount)
            {
                int offset = random.Next(1, 6) * (random.Next(2) == 0 ? -1 : 1);
                int wrong = answer + offset;
                if (wrong > 0)
                    options.Add(wrong);
            }

            List<int> shuffled = options.OrderBy(_ => random.Next()).ToList();
            return new ParentGateChallenge(a, b, shuffled);
        }
    }
}
