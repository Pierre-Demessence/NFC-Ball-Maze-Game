namespace LevelGeneration.Interfaces
{
    public interface IMeshGenerator<T> where T : ILevel
    {
        void Render(T level);
    }
}