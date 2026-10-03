using System;
using System.Collections.Generic;

namespace NongTrai.Profiles
{
    [Serializable]
    public class ChildProfile
    {
        public string id;
        public string nickname;
        public int avatarId;
        public long createdAtTicks;

        public string Id => id;
        public string Nickname => nickname;
        public int AvatarId => avatarId;
        public DateTime CreatedAt => new DateTime(createdAtTicks, DateTimeKind.Utc);

        public ChildProfile() { }

        public ChildProfile(string id, string nickname, int avatarId, DateTime createdAtUtc)
        {
            this.id = id;
            this.nickname = nickname;
            this.avatarId = avatarId;
            createdAtTicks = createdAtUtc.Ticks;
        }
    }

    [Serializable]
    public class ProfileData
    {
        public List<ChildProfile> profiles = new List<ChildProfile>();
        public string selectedProfileId;
    }
}
