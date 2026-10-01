using System;
using System.Collections.Generic;
using System.Linq;
using NongTrai.Animals;
using NongTrai.Learning;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;

namespace NongTrai.Tests.EditMode
{
    /// <summary>Lưu tiến độ trong RAM cho test (qua JSON để giống lưu thật).</summary>
    public class InMemoryProgressStorage : IProgressStorage
    {
        readonly Dictionary<string, string> data = new Dictionary<string, string>();
        public int SaveCount { get; private set; }

        public LearningProgress Load(string childId) =>
            data.TryGetValue(childId, out string json) ? JsonUtility.FromJson<LearningProgress>(json) : null;

        public void Save(LearningProgress progress)
        {
            data[progress.childId] = JsonUtility.ToJson(progress);
            SaveCount++;
        }

        public void Delete(string childId) => data.Remove(childId);
    }

    // =====================================================================
    public class MasteryModelTests
    {
        [Test]
        public void Correct_IncreasesScore_FastMoreThanSlow()
        {
            float fast = MasteryModel.Update(0.5f, true, 2f);
            float slow = MasteryModel.Update(0.5f, true, 8f);

            Assert.Greater(fast, 0.5f);
            Assert.Greater(slow, 0.5f);
            Assert.Greater(fast, slow);
        }

        [Test]
        public void Wrong_DecreasesScore()
        {
            Assert.Less(MasteryModel.Update(0.5f, false, 1f), 0.5f);
        }

        [Test]
        public void Score_StaysBetweenZeroAndOne()
        {
            float s = 0f;
            for (int i = 0; i < 100; i++) s = MasteryModel.Update(s, true, 1f);
            Assert.LessOrEqual(s, 1f);
            Assert.IsTrue(MasteryModel.IsMastered(s));

            for (int i = 0; i < 100; i++) s = MasteryModel.Update(s, false, 1f);
            Assert.GreaterOrEqual(s, 0f);
            Assert.IsFalse(MasteryModel.IsMastered(s));
        }

        [Test]
        public void FourFastCorrectAnswers_FromZero_ReachMastered()
        {
            // Nói được khi trình bày: điểm sau n lần đúng nhanh = 1 - 0.65^n.
            // 3 lần = 0.73 (chưa thuộc), 4 lần = 0.82 (thuộc).
            float s = 0f;
            for (int i = 0; i < 3; i++) s = MasteryModel.Update(s, true, 1f);
            Assert.IsFalse(MasteryModel.IsMastered(s), "3 lần chưa đủ");
            s = MasteryModel.Update(s, true, 1f);
            Assert.IsTrue(MasteryModel.IsMastered(s), "4 lần là thuộc");
        }
    }

    // =====================================================================
    public class DifficultyTests
    {
        static LearningProgress Progress(int level) => new LearningProgress { childId = "c", level = level };

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        public void OptionCount_MatchesLevel(int level, int options)
        {
            Assert.AreEqual(options, Difficulty.OptionCount(level));
        }

        [Test]
        public void SixCorrect_LevelsUp_AndResetsWindow()
        {
            LearningProgress p = Progress(1);
            for (int i = 0; i < 5; i++) Assert.IsFalse(Difficulty.Register(p, true));
            Assert.IsTrue(Difficulty.Register(p, true));

            Assert.AreEqual(2, p.level);
            Assert.IsEmpty(p.recent);
        }

        [Test]
        public void MostlyWrong_LevelsDown()
        {
            LearningProgress p = Progress(2);
            foreach (bool r in new[] { true, false, false, true, false, false })
                Difficulty.Register(p, r);

            Assert.AreEqual(1, p.level);
        }

        [Test]
        public void MixedResults_KeepLevel()
        {
            LearningProgress p = Progress(2);
            foreach (bool r in new[] { true, true, false, true, true, false }) // 4/6 = 67%
                Difficulty.Register(p, r);

            Assert.AreEqual(2, p.level);
        }

        [Test]
        public void Level_NeverLeavesRange()
        {
            LearningProgress p = Progress(3);
            for (int i = 0; i < 30; i++) Difficulty.Register(p, true);
            Assert.AreEqual(Difficulty.MaxLevel, p.level);

            for (int i = 0; i < 30; i++) Difficulty.Register(p, false);
            Assert.AreEqual(Difficulty.MinLevel, p.level);
        }

        [Test]
        public void SimilarDistractors_OnlyAtHardestLevel()
        {
            Assert.IsFalse(Difficulty.UseSimilarDistractors(1));
            Assert.IsFalse(Difficulty.UseSimilarDistractors(2));
            Assert.IsTrue(Difficulty.UseSimilarDistractors(3));
        }
    }

    // =====================================================================
    public class QuestionPickerTests
    {
        static readonly string[] Ids = { "cat", "dog", "chicken", "duck", "pig", "cow" };
        static readonly Dictionary<string, string> Groups = new Dictionary<string, string>
        {
            { "cat", "thu-nho" }, { "dog", "thu-nho" }, { "chicken", "gia-cam" },
            { "duck", "gia-cam" }, { "pig", "gia-suc" }, { "cow", "gia-suc" }
        };
        static string GroupOf(string id) => Groups[id];

        [Test]
        public void PickTarget_NeverRepeatsLastAnimal()
        {
            var rng = new Random(1);
            for (int i = 0; i < 300; i++)
                Assert.AreNotEqual("cat", QuestionPicker.PickTarget(Ids, _ => 0f, "cat", rng));
        }

        [Test]
        public void PickTarget_SingleAnimal_ReturnsIt()
        {
            Assert.AreEqual("cat", QuestionPicker.PickTarget(new[] { "cat" }, _ => 0f, "cat", new Random(1)));
        }

        [Test]
        public void PickTarget_PrefersAnimalsNotYetLearned()
        {
            // "cat" chưa thuộc (0), các con khác đã thuộc hẳn (1).
            var rng = new Random(42);
            int catCount = 0;
            const int draws = 2000;
            for (int i = 0; i < draws; i++)
                if (QuestionPicker.PickTarget(Ids, id => id == "cat" ? 0f : 1f, null, rng) == "cat")
                    catCount++;

            // Kỳ vọng: 1.1 / (1.1 + 5 x 0.1) ≈ 69%. Ngẫu nhiên đều chỉ là 17%.
            Assert.Greater(catCount, draws * 0.6);
        }

        [Test]
        public void PickDistractors_AreUnique_ExcludeTarget_AndHaveRightCount()
        {
            var rng = new Random(3);
            for (int i = 0; i < 100; i++)
            {
                List<string> d = QuestionPicker.PickDistractors("cat", Ids, 3, GroupOf, false, rng);
                Assert.AreEqual(3, d.Count);
                CollectionAssert.DoesNotContain(d, "cat");
                CollectionAssert.AllItemsAreUnique(d);
            }
        }

        [Test]
        public void PickDistractors_CountLargerThanPool_IsCapped()
        {
            List<string> d = QuestionPicker.PickDistractors("cat", new[] { "cat", "dog" }, 3, GroupOf, false, new Random(1));
            CollectionAssert.AreEqual(new[] { "dog" }, d);
        }

        [Test]
        public void PickDistractors_PreferSimilar_IncludesSameGroupFirst()
        {
            var rng = new Random(5);
            for (int i = 0; i < 50; i++)
            {
                List<string> d = QuestionPicker.PickDistractors("chicken", Ids, 3, GroupOf, true, rng);
                CollectionAssert.Contains(d, "duck", "Ở mức khó, hỏi gà thì phải có vịt làm đáp án nhiễu");
            }
        }

        [Test]
        public void PickDistractors_RespectsFilter()
        {
            var rng = new Random(9);
            for (int i = 0; i < 50; i++)
            {
                List<string> d = QuestionPicker.PickDistractors("chicken", Ids, 3, GroupOf, true, rng, id => id != "duck");
                CollectionAssert.DoesNotContain(d, "duck");
            }
        }

        [Test]
        public void Shuffle_KeepsAllItems()
        {
            List<int> s = QuestionPicker.Shuffle(Enumerable.Range(0, 20), new Random(2));
            CollectionAssert.AreEquivalent(Enumerable.Range(0, 20), s);
        }
    }

    // =====================================================================
    public class LearningServiceTests
    {
        readonly List<UnityEngine.Object> created = new List<UnityEngine.Object>();
        AnimalDatabase db;
        InMemoryProgressStorage storage;

        [SetUp]
        public void SetUp()
        {
            var animals = new List<AnimalData>();
            foreach ((string id, string group) in new (string, string)[] { ("cat", "thu-nho"), ("dog", "thu-nho"), ("chicken", "gia-cam"), ("duck", "gia-cam"), ("pig", null) })
            {
                var a = ScriptableObject.CreateInstance<AnimalData>();
                a.InitForTests(id, id, group);
                animals.Add(a);
                created.Add(a);
            }
            db = ScriptableObject.CreateInstance<AnimalDatabase>();
            db.InitForTests(animals);
            created.Add(db);
            storage = new InMemoryProgressStorage();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (UnityEngine.Object o in created) UnityEngine.Object.DestroyImmediate(o);
            created.Clear();
        }

        LearningService Service(int seed = 1) => new LearningService(db, "child-1", storage, new Random(seed));

        [Test]
        public void NextQuestion_HasTargetAmongOptions_AndOptionCountMatchesLevel()
        {
            LearningService s = Service();
            for (int i = 0; i < 30; i++)
            {
                Question q = s.NextQuestion();
                Assert.AreEqual(Difficulty.OptionCount(s.Level), q.Options.Count);
                CollectionAssert.Contains(q.Options, q.Target);
                CollectionAssert.AllItemsAreUnique(q.Options);
            }
        }

        [Test]
        public void RecordAnswer_FirstTry_UpdatesMasteryAndSaves()
        {
            LearningService s = Service();
            Question q = s.NextQuestion();

            Assert.IsTrue(s.RecordAnswer(q, q.Target.Id, 1f, true));
            Assert.Greater(s.GetMastery(q.Target.Id), 0f);
            Assert.AreEqual(1, storage.SaveCount);
        }

        [Test]
        public void RecordAnswer_RetryAfterWrong_DoesNotChangeScore()
        {
            LearningService s = Service();
            Question q = s.NextQuestion();
            string wrong = q.Options.First(o => o != q.Target).Id;

            Assert.IsFalse(s.RecordAnswer(q, wrong, 1f, true));
            float afterWrong = s.GetMastery(q.Target.Id);

            Assert.IsTrue(s.RecordAnswer(q, q.Target.Id, 1f, false));
            Assert.AreEqual(afterWrong, s.GetMastery(q.Target.Id));
            Assert.AreEqual(1, storage.SaveCount);
        }

        [Test]
        public void Progress_IsSavedPerChild_AndReloaded()
        {
            LearningService s = Service();
            Question q = s.NextQuestion();
            s.RecordAnswer(q, q.Target.Id, 1f, true);
            float score = s.GetMastery(q.Target.Id);

            LearningService reloaded = Service(2);
            Assert.AreEqual(score, reloaded.GetMastery(q.Target.Id), 1e-5);

            var otherChild = new LearningService(db, "child-2", storage, new Random(1));
            Assert.AreEqual(0f, otherChild.GetMastery(q.Target.Id));
        }

        [Test]
        public void AnsweringWell_RaisesLevel()
        {
            LearningService s = Service();
            for (int i = 0; i < Difficulty.Window; i++)
            {
                Question q = s.NextQuestion();
                s.RecordAnswer(q, q.Target.Id, 1f, true);
            }
            Assert.AreEqual(2, s.Level);
        }

        [Test]
        public void DistractorFilter_IsApplied()
        {
            LearningService s = Service();
            for (int i = 0; i < 30; i++)
            {
                Question q = s.NextQuestion((target, other) => other.Id != "pig");
                if (q.Target.Id != "pig")
                    Assert.IsFalse(q.Options.Any(o => o.Id == "pig"));
            }
        }
    }
}
