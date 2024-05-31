namespace customCopilot.Services
{
    public interface IMemoryService
    {
        Task<string> AskQuestion(string question);
    }
}
