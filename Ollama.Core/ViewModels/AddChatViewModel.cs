using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ollama.Core.ViewModels
{
    public class AddChatViewModel
    {
        public int ChatId { get; set; }
        public string ChatTitle { get; set; } = string.Empty;
        public bool IUserChat { get; set; }
        public string Content { get; set; }
    }
}
