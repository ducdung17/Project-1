namespace NongTrai.Profiles
{
    /// <summary>
    /// Nơi lưu hồ sơ. Tách thành interface để:
    /// - Unit test dùng bộ nhớ tạm (InMemoryProfileStorage), không đụng tới ổ đĩa.
    /// - Sau này đổi sang lưu trên server (API) mà không phải sửa ProfileService.
    /// </summary>
    public interface IProfileStorage
    {
        /// <summary>Trả về dữ liệu đã lưu, hoặc null nếu chưa có.</summary>
        ProfileData Load();

        void Save(ProfileData data);
    }
}
