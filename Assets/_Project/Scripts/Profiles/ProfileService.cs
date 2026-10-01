using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NongTrai.Profiles
{
    public enum ProfileResult
    {
        Success,
        EmptyName,
        NameTooLong,
        DuplicateName,
        InvalidAvatar,
        AvatarTaken,
        LimitReached,
        NotFound
    }

    /// <summary>
    /// Toàn bộ logic thêm / xóa / chọn hồ sơ.
    /// Là class C# thuần, không phụ thuộc UnityEngine, nên test được bằng EditMode test
    /// mà không cần mở scene.
    /// </summary>
    public class ProfileService
    {
        public const int MaxProfiles = 6;
        public const int MaxNameLength = 12;

        readonly IProfileStorage storage;
        readonly int avatarCount;
        readonly Func<string> idGenerator;
        readonly Func<DateTime> clock;
        readonly ProfileData data;

        /// <summary>Danh sách hồ sơ thay đổi (thêm hoặc xóa).</summary>
        public event Action ProfilesChanged;

        /// <summary>Một hồ sơ vừa được chọn (null nếu bỏ chọn).</summary>
        public event Action<ChildProfile> SelectionChanged;

        public ProfileService(
            IProfileStorage storage,
            int avatarCount,
            Func<string> idGenerator = null,
            Func<DateTime> clock = null)
        {
            if (avatarCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(avatarCount), "Cần ít nhất 1 avatar.");

            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            this.avatarCount = avatarCount;
            this.idGenerator = idGenerator ?? (() => Guid.NewGuid().ToString("N"));
            this.clock = clock ?? (() => DateTime.UtcNow);

            data = storage.Load() ?? new ProfileData();
            if (data.profiles == null)
                data.profiles = new List<ChildProfile>();

            // Hồ sơ đang chọn có thể đã bị xóa ở phiên trước (hoặc dữ liệu hỏng) -> bỏ chọn.
            if (data.selectedProfileId != null && Find(data.selectedProfileId) == null)
            {
                data.selectedProfileId = null;
                storage.Save(data);
            }
        }

        public IReadOnlyList<ChildProfile> Profiles => data.profiles;
        public int Count => data.profiles.Count;
        public bool CanAddMore => data.profiles.Count < MaxProfiles;
        public ChildProfile SelectedProfile => Find(data.selectedProfileId);

        public ChildProfile Find(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            return data.profiles.FirstOrDefault(p => p.id == id);
        }

        public bool IsAvatarTaken(int avatarId) => data.profiles.Any(p => p.avatarId == avatarId);

        // ---------------------------------------------------------------- Thêm

        public ProfileResult Add(string nickname, int avatarId, out ChildProfile created)
        {
            created = null;
            ProfileResult check = ValidateNew(nickname, avatarId, out string cleanName);
            if (check != ProfileResult.Success)
                return check;

            created = new ChildProfile(idGenerator(), cleanName, avatarId, clock());
            data.profiles.Add(created);
            storage.Save(data);
            ProfilesChanged?.Invoke();
            return ProfileResult.Success;
        }

        public ProfileResult Add(string nickname, int avatarId) => Add(nickname, avatarId, out _);

        /// <summary>Kiểm tra dữ liệu trước khi thêm. UI có thể gọi để báo lỗi ngay khi bé/phụ huynh nhập.</summary>
        public ProfileResult ValidateNew(string nickname, int avatarId, out string cleanName)
        {
            cleanName = NormalizeName(nickname);

            if (!CanAddMore)
                return ProfileResult.LimitReached;
            if (cleanName.Length == 0)
                return ProfileResult.EmptyName;
            if (cleanName.Length > MaxNameLength)
                return ProfileResult.NameTooLong;

            string nameForCheck = cleanName;
            if (data.profiles.Any(p => string.Equals(p.nickname, nameForCheck, StringComparison.OrdinalIgnoreCase)))
                return ProfileResult.DuplicateName;

            if (avatarId < 0 || avatarId >= avatarCount)
                return ProfileResult.InvalidAvatar;
            // Mỗi bé một con vật riêng: trẻ chưa biết đọc nhận ra hồ sơ của mình qua con vật.
            if (IsAvatarTaken(avatarId))
                return ProfileResult.AvatarTaken;

            return ProfileResult.Success;
        }

        // ---------------------------------------------------------------- Xóa

        public ProfileResult Remove(string id)
        {
            ChildProfile profile = Find(id);
            if (profile == null)
                return ProfileResult.NotFound;

            data.profiles.Remove(profile);

            bool wasSelected = data.selectedProfileId == id;
            if (wasSelected)
                data.selectedProfileId = null;

            storage.Save(data);
            ProfilesChanged?.Invoke();
            if (wasSelected)
                SelectionChanged?.Invoke(null);
            return ProfileResult.Success;
        }

        // ---------------------------------------------------------------- Chọn

        public ProfileResult Select(string id)
        {
            ChildProfile profile = Find(id);
            if (profile == null)
                return ProfileResult.NotFound;

            data.selectedProfileId = profile.id;
            storage.Save(data);
            SelectionChanged?.Invoke(profile);
            return ProfileResult.Success;
        }

        public void ClearSelection()
        {
            if (data.selectedProfileId == null)
                return;
            data.selectedProfileId = null;
            storage.Save(data);
            SelectionChanged?.Invoke(null);
        }

        // ---------------------------------------------------------------- Tiện ích

        /// <summary>
        /// Chuẩn hóa tên: bỏ khoảng trắng thừa, gộp nhiều dấu cách thành một,
        /// và chuẩn hóa Unicode (NFC) để "Bé" gõ bằng Telex/VNI khác nhau vẫn được coi là cùng một tên.
        /// </summary>
        public static string NormalizeName(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return string.Empty;

            string nfc = raw.Normalize(NormalizationForm.FormC);
            string[] parts = nfc.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", parts);
        }
    }
}
