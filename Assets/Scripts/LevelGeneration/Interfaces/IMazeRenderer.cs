namespace LevelGeneration.Interfaces
{
    public interface IMazeRenderer<T> where T : ILevel
    {
        void Render(T level);
    }
}