using System;
using System.Collections.Generic;
using NUnit.Framework;
using NongTrai.Profiles;

namespace NongTrai.Tests.EditMode
{
    /// <summary>Lưu hồ sơ trong RAM, chỉ dùng cho test. Đếm số lần Save để kiểm tra dữ liệu có được lưu.</summary>
    public class InMemoryProfileStorage : IProfileStorage
    {
        string json; // Lưu dạng JSON để mô phỏng đúng việc ghi/đọc thật (tạo object mới mỗi lần Load).
        public int SaveCount { get; private set; }

        public ProfileData Load()
        {
            return json == null ? null : UnityEngine.JsonUtility.FromJson<ProfileData>(json);
        }

        public void Save(ProfileData data)
        {
            json = UnityEngine.JsonUtility.ToJson(data);
            SaveCount++;
        }
    }

    public class ProfileServiceTests
    {
        const int AvatarCount = 8;

        InMemoryProfileStorage storage;
        ProfileService service;
        int nextId;

        [SetUp]
        public void SetUp()
        {
            storage = new InMemoryProfileStorage();
            nextId = 0;
            service = CreateService();
        }

        ProfileService CreateService()
        {
            // Id cố định (p1, p2...) và thời gian cố định để test luôn cho cùng kết quả.
            return new ProfileService(
                storage,
                AvatarCount,
                () => "p" + (++nextId),
                () => new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc));
        }

        // ================================================================ Thêm

        [Test]
        public void Add_ValidProfile_Succeeds()
        {
            ProfileResult result = service.Add("Bin", 0, out ChildProfile created);

            Assert.AreEqual(ProfileResult.Success, result);
            Assert.AreEqual(1, service.Count);
            Assert.AreEqual("p1", created.Id);
            Assert.AreEqual("Bin", created.Nickname);
            Assert.AreEqual(0, created.AvatarId);
        }

        [Test]
        public void Add_TrimsAndCollapsesWhitespace()
        {
            service.Add("   Bé   Na  ", 1, out ChildProfile created);
            Assert.AreEqual("Bé Na", created.Nickname);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Add_EmptyName_Fails(string name)
        {
            Assert.AreEqual(ProfileResult.EmptyName, service.Add(name, 0));
            Assert.AreEqual(0, service.Count);
        }

        [Test]
        public void Add_NameAtMaxLength_Succeeds()
        {
            string name = new string('a', ProfileService.MaxNameLength);
            Assert.AreEqual(ProfileResult.Success, service.Add(name, 0));
        }

        [Test]
        public void Add_NameTooLong_Fails()
        {
            string name = new string('a', ProfileService.MaxNameLength + 1);
            Assert.AreEqual(ProfileResult.NameTooLong, service.Add(name, 0));
        }

        [Test]
        public void Add_DuplicateNameIgnoringCase_Fails()
        {
            service.Add("Bin", 0);
            Assert.AreEqual(ProfileResult.DuplicateName, service.Add("  bIN ", 1));
        }

        [Test]
        public void Add_SameVietnameseNameDifferentUnicodeForm_IsDuplicate()
        {
            // "Bé" dựng sẵn (NFC) và "Be" + dấu sắc rời (NFD) nhìn giống nhau nhưng khác byte.
            string composed = "Bé";
            string decomposed = "Bé";
            service.Add(composed, 0);
            Assert.AreEqual(ProfileResult.DuplicateName, service.Add(decomposed, 1));
        }

        [TestCase(-1)]
        [TestCase(AvatarCount)]
        public void Add_InvalidAvatar_Fails(int avatarId)
        {
            Assert.AreEqual(ProfileResult.InvalidAvatar, service.Add("Bin", avatarId));
        }

        [Test]
        public void Add_AvatarAlreadyTaken_Fails()
        {
            service.Add("Bin", 3);
            Assert.AreEqual(ProfileResult.AvatarTaken, service.Add("Na", 3));
        }

        [Test]
        public void Add_WhenLimitReached_Fails()
        {
            for (int i = 0; i < ProfileService.MaxProfiles; i++)
                Assert.AreEqual(ProfileResult.Success, service.Add("Be" + i, i));

            Assert.IsFalse(service.CanAddMore);
            Assert.AreEqual(ProfileResult.LimitReached, service.Add("Them", 7));
        }

        [Test]
        public void Add_RaisesProfilesChanged()
        {
            int calls = 0;
            service.ProfilesChanged += () => calls++;

            service.Add("Bin", 0);
            service.Add("", 1); // thất bại -> không bắn sự kiện

            Assert.AreEqual(1, calls);
        }

        [Test]
        public void Add_IsPersisted_AndLoadedByNewService()
        {
            service.Add("Bin", 2);

            ProfileService reloaded = CreateService();

            Assert.AreEqual(1, reloaded.Count);
            Assert.AreEqual("Bin", reloaded.Profiles[0].Nickname);
            Assert.AreEqual(2, reloaded.Profiles[0].AvatarId);
        }

        // ================================================================ Xóa

        [Test]
        public void Remove_ExistingProfile_Succeeds()
        {
            service.Add("Bin", 0, out ChildProfile bin);
            service.Add("Na", 1);

            Assert.AreEqual(ProfileResult.Success, service.Remove(bin.Id));
            Assert.AreEqual(1, service.Count);
            Assert.IsNull(service.Find(bin.Id));
        }

        [Test]
        public void Remove_UnknownId_ReturnsNotFound()
        {
            Assert.AreEqual(ProfileResult.NotFound, service.Remove("khong-ton-tai"));
        }

        [Test]
        public void Remove_FreesAvatarAndName_ForReuse()
        {
            service.Add("Bin", 0, out ChildProfile bin);
            service.Remove(bin.Id);

            Assert.AreEqual(ProfileResult.Success, service.Add("Bin", 0));
        }

        [Test]
        public void Remove_SelectedProfile_ClearsSelection()
        {
            service.Add("Bin", 0, out ChildProfile bin);
            service.Select(bin.Id);
            ChildProfile notified = bin;
            service.SelectionChanged += p => notified = p;

            service.Remove(bin.Id);

            Assert.IsNull(service.SelectedProfile);
            Assert.IsNull(notified);
        }

        [Test]
        public void Remove_OtherProfile_KeepsSelection()
        {
            service.Add("Bin", 0, out ChildProfile bin);
            service.Add("Na", 1, out ChildProfile na);
            service.Select(bin.Id);

            service.Remove(na.Id);

            Assert.AreEqual(bin.Id, service.SelectedProfile.Id);
        }

        // ================================================================ Chọn

        [Test]
        public void Select_ExistingProfile_SetsSelectedAndRaisesEvent()
        {
            service.Add("Bin", 0, out ChildProfile bin);
            ChildProfile notified = null;
            service.SelectionChanged += p => notified = p;

            Assert.AreEqual(ProfileResult.Success, service.Select(bin.Id));
            Assert.AreEqual(bin.Id, service.SelectedProfile.Id);
            Assert.AreEqual(bin.Id, notified.Id);
        }

        [Test]
        public void Select_UnknownId_ReturnsNotFound_AndKeepsOldSelection()
        {
            service.Add("Bin", 0, out ChildProfile bin);
            service.Select(bin.Id);

            Assert.AreEqual(ProfileResult.NotFound, service.Select("khong-ton-tai"));
            Assert.AreEqual(bin.Id, service.SelectedProfile.Id);
        }

        [Test]
        public void Select_IsPersisted()
        {
            service.Add("Bin", 0);
            service.Add("Na", 1, out ChildProfile na);
            service.Select(na.Id);

            Assert.AreEqual(na.Id, CreateService().SelectedProfile.Id);
        }

        [Test]
        public void ClearSelection_RemovesSelection()
        {
            service.Add("Bin", 0, out ChildProfile bin);
            service.Select(bin.Id);

            service.ClearSelection();

            Assert.IsNull(service.SelectedProfile);
            Assert.IsNull(CreateService().SelectedProfile);
        }

        // ================================================================ Tải dữ liệu

        [Test]
        public void Load_WithStaleSelectedId_ClearsIt()
        {
            storage.Save(new ProfileData
            {
                profiles = new List<ChildProfile> { new ChildProfile("a", "Bin", 0, DateTime.UtcNow) },
                selectedProfileId = "da-bi-xoa"
            });

            ProfileService loaded = CreateService();

            Assert.IsNull(loaded.SelectedProfile);
            Assert.AreEqual(1, loaded.Count);
        }

        [Test]
        public void Constructor_WithNoSavedData_StartsEmpty()
        {
            Assert.AreEqual(0, service.Count);
            Assert.IsNull(service.SelectedProfile);
            Assert.IsTrue(service.CanAddMore);
        }

        [Test]
        public void Constructor_NullStorage_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new ProfileService(null, AvatarCount));
        }
    }
}
