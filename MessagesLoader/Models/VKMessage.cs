using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesLoader.Models
{
    public class VKMessage
    {
        public VKMessage(VKСontactPerson person)
        {
            СontactPerson = person;
            Author = string.Empty;
            Message = "none";
            Date = DateTime.MinValue;
        }
        public VKСontactPerson СontactPerson { get; }
        public string Author { get; set; }
        public string Message { get; set; }
        public IEnumerable<VKAttachment> Attachments { get; set; }
        public DateTime Date { get; set; }
        public string FilePath { get; set; }
    }
}
