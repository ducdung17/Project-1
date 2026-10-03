namespace NongTrai.Profiles
{
    public static class ProfileMessages
    {
        public static string For(ProfileResult result)
        {
            switch (result)
            {
                case ProfileResult.Success: return string.Empty;
                case ProfileResult.EmptyName: return "Hãy đặt tên cho bé nhé.";
                case ProfileResult.NameTooLong: return $"Tên tối đa {ProfileService.MaxNameLength} ký tự.";
                case ProfileResult.DuplicateName: return "Đã có bé dùng tên này rồi.";
                case ProfileResult.InvalidAvatar: return "Hãy chọn một con vật.";
                case ProfileResult.AvatarTaken: return "Con vật này đã có bạn khác chọn rồi.";
                case ProfileResult.LimitReached: return $"Chỉ tạo được tối đa {ProfileService.MaxProfiles} hồ sơ.";
                case ProfileResult.NotFound: return "Không tìm thấy hồ sơ.";
                default: return "Có lỗi xảy ra.";
            }
        }
    }
}
