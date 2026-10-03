using System;
using System.Linq;
using NUnit.Framework;
using NongTrai.Profiles;

namespace NongTrai.Tests.EditMode
{
    public class ParentGateChallengeTests
    {
        static readonly int[] Seeds = Enumerable.Range(0, 200).ToArray();

        [TestCaseSource(nameof(Seeds))]
        public void Generate_OptionsContainAnswer_AndAreDistinct(int seed)
        {
            ParentGateChallenge c = ParentGateChallenge.Generate(new Random(seed), 3);

            Assert.AreEqual(3, c.Options.Count);
            CollectionAssert.Contains(c.Options, c.Answer);
            CollectionAssert.AllItemsAreUnique(c.Options);
            Assert.IsTrue(c.Options.All(o => o > 0));
        }

        [TestCaseSource(nameof(Seeds))]
        public void Generate_SumIsAtLeastTen(int seed)
        {
            ParentGateChallenge c = ParentGateChallenge.Generate(new Random(seed));
            Assert.GreaterOrEqual(c.Answer, 10);
        }

        [Test]
        public void Check_OnlyCorrectAnswerPasses()
        {
            ParentGateChallenge c = ParentGateChallenge.Generate(new Random(42));

            Assert.IsTrue(c.Check(c.Answer));
            foreach (int wrong in c.Options.Where(o => o != c.Answer))
                Assert.IsFalse(c.Check(wrong));
        }

        [Test]
        public void Generate_TooFewOptions_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ParentGateChallenge.Generate(new Random(1), 1));
        }
    }
}
