using System;
using System.Collections.Generic;
using System.Linq;
using NongTrai.Animals;
using NongTrai.Learning;
using NongTrai.Online;
using NongTrai.Profiles;
using NUnit.Framework;
using UnityEngine;

namespace NongTrai.Tests.EditMode
{
    public class SessionRecorderTests
    {
        readonly List<UnityEngine.Object> created = new List<UnityEngine.Object>();
        AnimalDatabase db;
        ChildProfile child;

        [SetUp]
        public void SetUp()
        {
            var animals = new List<AnimalData>();
            foreach (string id in new[] { "cat", "dog", "pig" })
            {
                var a = ScriptableObject.CreateInstance<AnimalData>();
                a.InitForTests(id, id, null);
                animals.Add(a);
                created.Add(a);
            }
            db = ScriptableObject.CreateInstance<AnimalDatabase>();
            db.InitForTests(animals);
            created.Add(db);
            child = new ChildProfile("kid-1", "Na", 2, new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc));
        }

        [TearDown]
        public void TearDown()
        {
            foreach (UnityEngine.Object o in created) UnityEngine.Object.DestroyImmediate(o);
            created.Clear();
        }

        [Test]
        public void Add_SetsCorrectFlagFromIds()
        {
            var r = new SessionRecorder("sound", child);
            r.Add("cat", "dog", firstTry: true, seconds: 2f);
            r.Add("cat", "cat", firstTry: false, seconds: 3f);

            Assert.IsFalse(r.Payload.answers[0].correct);
            Assert.IsTrue(r.Payload.answers[1].correct);
            Assert.IsFalse(r.Payload.answers[1].firstTry);
        }

        [Test]
        public void Complete_FillsChildInfo_MasteryForEveryAnimal_AndUtcTimes()
        {
            var learning = new LearningService(db, child.Id, new InMemoryProgressStorage(), new System.Random(1));
            var start = new DateTime(2026, 10, 1, 3, 0, 0, DateTimeKind.Utc);
            var r = new SessionRecorder("food", child, start);
            r.Add("dog", "dog", true, 1.5f);

            SessionPayload p = r.Complete(learning, db, start.AddMinutes(4));

            Assert.AreEqual("kid-1", p.childId);
            Assert.AreEqual("Na", p.nickname);
            Assert.AreEqual(2, p.avatarId);
            Assert.AreEqual("food", p.game);
            Assert.AreEqual(learning.Level, p.difficultyLevel);
            CollectionAssert.AreEquivalent(new[] { "cat", "dog", "pig" }, p.mastery.Select(m => m.animalId).ToArray());
            StringAssert.EndsWith("Z", p.startedAt);
            Assert.Less(DateTime.Parse(p.startedAt).ToUniversalTime(), DateTime.Parse(p.endedAt).ToUniversalTime());
        }

        [Test]
        public void EachRecorder_HasItsOwnSessionId()
        {
            var a = new SessionRecorder("sound", child);
            var b = new SessionRecorder("sound", child);
            Assert.AreNotEqual(a.Payload.clientSessionId, b.Payload.clientSessionId);
            Assert.LessOrEqual(a.Payload.clientSessionId.Length, 64);
        }

        [Test]
        public void Json_UsesFieldNamesTheServerExpects()
        {
            var r = new SessionRecorder("shadow", child);
            r.Add("pig", "pig", true, 1f);
            string json = JsonUtility.ToJson(r.Payload);

            foreach (string field in new[] { "clientSessionId", "childId", "nickname", "avatarId", "game",
                         "startedAt", "endedAt", "difficultyLevel", "answers", "mastery", "animalId", "chosenId",
                         "correct", "firstTry", "seconds" })
                StringAssert.Contains("\"" + field + "\"", json);
        }
    }
}
