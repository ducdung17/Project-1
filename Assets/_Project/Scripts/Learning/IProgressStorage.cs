namespace NongTrai.Learning
{
    /// <summary>Nơi lưu tiến độ học. Sau này thay bằng bản gửi lên server (API) mà không sửa LearningService.</summary>
    public interface IProgressStorage
    {
        /// <summary>Trả về tiến độ đã lưu của bé, hoặc null nếu chưa có.</summary>
        LearningProgress Load(string childId);

        void Save(LearningProgress progress);

        void Delete(string childId);
    }
}
