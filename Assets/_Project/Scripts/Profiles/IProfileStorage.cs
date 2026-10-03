namespace NongTrai.Profiles
{
    public interface IProfileStorage
    {
        ProfileData Load();

        void Save(ProfileData data);
    }
}
