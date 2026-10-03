namespace NongTrai.Learning
{
    public interface IProgressStorage
    {
        LearningProgress Load(string childId);

        void Save(LearningProgress progress);

        void Delete(string childId);
    }
}
