namespace LevelGeneration.Interfaces
{
    public interface IMazeGenerator<T> where T : ILevel
    {
        T Generate();
    }
}