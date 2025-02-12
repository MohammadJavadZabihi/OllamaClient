using Ollama.Core.ViewModels;
using Ollama.Data.Entities;

namespace Ollama.Core.Interface
{
    public interface IChatDataBaseService
    {
        Task AddChatTitle(AddChatViewModel addChatViewModel);
        Task<bool> UpdateChatTitle(string chatTitle);
        Task<bool> DeleteChatTitle(ChatViewModel deteleChatViewModel);
        Task<List<Chat>> GetMessageTitle();
        Task<int> GetChatTitleId(string chatName);
    }
}
