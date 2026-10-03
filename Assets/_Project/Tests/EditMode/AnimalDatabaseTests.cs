using System.Collections.Generic;
using System.Linq;
using NongTrai.Animals;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace NongTrai.Tests.EditMode
{
    public class AnimalDatabaseTests
    {
        readonly List<Object> created = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            foreach (Object o in created)
                Object.DestroyImmediate(o);
            created.Clear();
        }

        AnimalData Animal(string id, string nameVi = "Con vật", string group = null)
        {
            var a = ScriptableObject.CreateInstance<AnimalData>();
            a.InitForTests(id, nameVi, group);
            created.Add(a);
            return a;
        }

        AnimalDatabase Database(params AnimalData[] animals)
        {
            var db = ScriptableObject.CreateInstance<AnimalDatabase>();
            db.InitForTests(animals.ToList());
            created.Add(db);
            return db;
        }

        static void AssertHas(List<string> problems, string text)
        {
            Assert.IsTrue(problems.Any(p => p.Contains(text)),
                $"Mong đợi có lỗi chứa \"{text}\", nhưng nhận được:\n- " + string.Join("\n- ", problems));
        }

        static void AssertHasNot(List<string> problems, string text)
        {
            Assert.IsFalse(problems.Any(p => p.Contains(text)),
                $"Không mong đợi lỗi chứa \"{text}\", nhưng nhận được:\n- " + string.Join("\n- ", problems));
        }

        [Test]
        public void Get_ExistingId_ReturnsAnimal()
        {
            AnimalData cat = Animal("cat", "Mèo");
            AnimalDatabase db = Database(cat, Animal("dog", "Chó"));

            Assert.AreSame(cat, db.Get("cat"));
        }

        [TestCase("lion")]
        [TestCase("")]
        [TestCase(null)]
        public void Get_UnknownOrEmptyId_ReturnsNull(string id)
        {
            AnimalDatabase db = Database(Animal("cat"));
            Assert.IsNull(db.Get(id));
        }

        [Test]
        public void GetSimilar_ReturnsSameGroup_ExcludingItself()
        {
            AnimalData chicken = Animal("chicken", "Gà", "gia-cam");
            AnimalData duck = Animal("duck", "Vịt", "gia-cam");
            AnimalData cat = Animal("cat", "Mèo", "thu-nho");
            AnimalData dog = Animal("dog", "Chó", "thu-nho");
            AnimalDatabase db = Database(chicken, duck, cat, dog);

            CollectionAssert.AreEquivalent(new[] { duck }, db.GetSimilar(chicken));
        }

        [Test]
        public void GetSimilar_NoGroup_ReturnsEmpty()
        {
            AnimalData fish = Animal("fish", "Cá");
            AnimalDatabase db = Database(fish, Animal("cat", "Mèo"));

            Assert.IsEmpty(db.GetSimilar(fish));
        }

        [Test]
        public void Validate_DuplicateId_IsReported()
        {
            AnimalDatabase db = Database(Animal("cat"), Animal("cat"));
            AssertHas(db.Validate(), "bị trùng");
        }

        [Test]
        public void Validate_EmptyId_IsReported()
        {
            AnimalDatabase db = Database(Animal(""));
            AssertHas(db.Validate(), "chưa có Id");
        }

        [TestCase("Mèo")]
        [TestCase("Cat")]
        [TestCase("con meo")]
        [TestCase("cat-")]
        public void Validate_InvalidIdFormat_IsReported(string badId)
        {
            AnimalDatabase db = Database(Animal(badId));
            AssertHas(db.Validate(), "không hợp lệ");
        }

        [TestCase("cat")]
        [TestCase("guinea-pig")]
        [TestCase("cat2")]
        public void Validate_ValidIdFormat_IsNotReported(string goodId)
        {
            AnimalDatabase db = Database(Animal(goodId));
            AssertHasNot(db.Validate(), "không hợp lệ");
        }

        [Test]
        public void Validate_EmptySlot_IsReported()
        {
            AnimalDatabase db = Database(Animal("cat"), null);
            AssertHas(db.Validate(), "đang trống");
        }

        [Test]
        public void Validate_GroupWithOnlyOneAnimal_IsReported()
        {
            AnimalDatabase db = Database(Animal("chicken", "Gà", "gia-cam"), Animal("duck", "Vịt", "gia_cam"));
            AssertHas(db.Validate(), "chỉ có 1 con");
        }

        [Test]
        public void Validate_MissingAssets_AreReported()
        {
            List<string> problems = Database(Animal("cat", "Mèo")).Validate();

            AssertHas(problems, "thiếu hình");
            AssertHas(problems, "thiếu tiếng kêu");
            AssertHas(problems, "thiếu giọng đọc");
            AssertHas(problems, "chưa chọn thức ăn");
        }

        [Test]
        public void RealDatabase_HasNoProblems()
        {
            string[] guids = AssetDatabase.FindAssets("t:AnimalDatabase");
            if (guids.Length == 0)
                Assert.Ignore("Chưa tạo file AnimalDatabase trong project.");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var db = AssetDatabase.LoadAssetAtPath<AnimalDatabase>(path);
                List<string> problems = db.Validate();

                Assert.IsEmpty(problems, $"{path} có lỗi:\n- " + string.Join("\n- ", problems));
                Assert.GreaterOrEqual(db.Count, 4, $"{path}: cần ít nhất 4 con vật để trò chơi có đủ đáp án.");
            }
        }
    }
}
