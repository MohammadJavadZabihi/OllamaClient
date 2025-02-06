using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ollama.Data.Entities
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        [Required]
        [MaxLength(150)]
        public string ChatTitle { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
