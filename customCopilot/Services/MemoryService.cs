using Microsoft.Extensions.Configuration;
using Microsoft.KernelMemory;

namespace customCopilot.Services
{
    public class MemoryService : IMemoryService
    {
        private readonly IKernelMemory _kernelMemory;

        public MemoryService(IConfiguration configuration)
        {
            string apiKey = configuration["AzureOpenAI:ApiKey"];
            string deploymentChatName = configuration["AzureOpenAI:DeploymentChatName"];
            string deploymentEmbeddingName = configuration["AzureOpenAI:DeploymentEmbeddingName"];
            string endpoint = configuration["AzureOpenAI:Endpoint"];

            var embeddingConfig = new AzureOpenAIConfig
            {
                APIKey = apiKey,
                Deployment = deploymentEmbeddingName,
                Endpoint = endpoint,
                APIType = AzureOpenAIConfig.APITypes.EmbeddingGeneration,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey
            };

            var chatConfig = new AzureOpenAIConfig
            {
                APIKey = apiKey,
                Deployment = deploymentChatName,
                Endpoint = endpoint,
                APIType = AzureOpenAIConfig.APITypes.ChatCompletion,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey
            };

            _kernelMemory = new KernelMemoryBuilder()
                .WithAzureOpenAITextGeneration(chatConfig)
                .WithAzureOpenAITextEmbeddingGeneration(embeddingConfig)
                .WithSimpleVectorDb()
                .Build<MemoryServerless>();
        }

        public async Task<string> AskQuestion(string question)
        {
            var answer = await _kernelMemory.AskAsync(question);

            return answer.Result;
        }

    }
}
