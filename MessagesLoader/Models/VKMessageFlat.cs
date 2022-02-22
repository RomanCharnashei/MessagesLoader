using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesLoader.Models
{
    public class VKMessageFlat
    {
        public string СontactPersonName { get; private set; }
        public string СontactPersonLink { get; private set; }
        public string Author { get; private set; }
        public string Message { get; private set; }
        public IEnumerable<VKAttachment> Attachments { get; set; }
        public DateTime Date { get; private set; }
        public string FilePath { get; private set; }

        public static VKMessageFlat CreateFrom(VKMessage message)
        {
            return new VKMessageFlat
            {
                СontactPersonName = message.СontactPerson.Name,
                СontactPersonLink = message.СontactPerson.Link,
                Author = message.Author,
                Message = message.Message,
                Attachments = message.Attachments,
                Date = message.Date,
                FilePath = message.FilePath
            };
        }
    }
}
