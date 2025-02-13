using Microsoft.EntityFrameworkCore;
using Ollama.Core.Interface;
using Ollama.Core.ViewModels;
using Ollama.Data.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ollama.Data.Entites;
using System.Reflection.Metadata.Ecma335;

namespace Ollama.Core.Services
{
    public class ChatDataBaseService : IChatDataBaseService
    {
        private readonly OllamaClientContext _clientContext;
        public ChatDataBaseService()
        {
            _clientContext = new OllamaClientContext();
        }

        public async Task AddChatTitle(AddChatViewModel addChatViewModel)
        {
            var existingChat = _clientContext.Chats.FirstOrDefault(c => c.ChatTitle == addChatViewModel.ChatTitle);
            if (existingChat == null)
            {
                var addNewChat = new Chat
                {
                    ChatTitle = addChatViewModel.ChatTitle,
                };

                await _clientContext.Chats.AddAsync(addNewChat);
                await _clientContext.SaveChangesAsync();

                var addMessage = new Message
                {
                    ChatId = addNewChat.ChatId,
                    Content = addChatViewModel.Content,
                    IsUserChat = addChatViewModel.IUserChat
                };

                await _clientContext.Messages.AddAsync(addMessage);
                await _clientContext.SaveChangesAsync();
            }
            else
            {
                var addMessage = new Message
                {
                    ChatId = existingChat.ChatId,
                    Content = addChatViewModel.Content,
                    IsUserChat = addChatViewModel.IUserChat
                };

                await _clientContext.Messages.AddAsync(addMessage);
                await _clientContext.SaveChangesAsync();
            }
        }

        public async Task<List<Message>> GetMessagesForChat(int chatId)
        {
            return await _clientContext.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.SendAt)
                .ToListAsync();
        }

        public async Task<List<Chat>> GetMessageTitle()
        {
            return await _clientContext.Chats.ToListAsync();
        }

        public Task<bool> DeleteChatTitle(ChatViewModel deleteChatViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> GetChatTitleId(string chatName)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateChatTitle(string chatTitle)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Message>> GetMessagesFromChatName(string chatName)
        {
            var exixstChat = await _clientContext.Chats.FirstOrDefaultAsync(c => c.ChatTitle == chatName);

            if(exixstChat != null)
            {
                return await _clientContext.Messages.Where(m => m.ChatId == exixstChat.ChatId).ToListAsync();
            }

            return null;
        }
    }
}
