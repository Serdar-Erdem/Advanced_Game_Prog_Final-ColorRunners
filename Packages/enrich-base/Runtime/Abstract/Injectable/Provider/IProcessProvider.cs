namespace Rich.Base.Runtime.Abstract.Injectable.Provider
{
    public interface IProcessProvider
    {
        T Get<T>() where T : Process.Process, new();

        bool Return<T>(T process) where T : Process.Process;

        void ClearInactive();
    }
}