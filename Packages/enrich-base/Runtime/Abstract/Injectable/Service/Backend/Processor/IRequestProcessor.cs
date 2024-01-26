namespace Rich.Base.Runtime.Abstract.Injectable.Service.Backend.Processor
{
    public interface IRequestProcessor
    {
        IProcessorService Service { set; }

        void Process(string command, string data);

        string Command { get; }
    }
}