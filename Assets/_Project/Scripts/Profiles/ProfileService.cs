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

    public class ProfileService
    {
        public const int MaxProfiles = 6;
        public const int MaxNameLength = 12;

        readonly IProfileStorage storage;
        readonly int avatarCount;
        readonly Func<string> idGenerator;
        readonly Func<DateTime> clock;
        readonly ProfileData data;

        public event Action ProfilesChanged;

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
            if (IsAvatarTaken(avatarId))
                return ProfileResult.AvatarTaken;

            return ProfileResult.Success;
        }

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
