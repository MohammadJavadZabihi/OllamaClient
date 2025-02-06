using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ollama.Data.Entities
{
    public class Message
    {
        public Message()
        {
            
        }

        [Key]
        public int MessageId { get; set; }

        [ForeignKey("Chat")]
        [Required]
        public int ChatId { get; set; }

        [Required]
        public bool IsUserChat { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime SendAt { get; set; } = DateTime.UtcNow;

        public Chat Chat { get; set; }
    }
}
