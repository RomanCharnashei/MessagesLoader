using HtmlAgilityPack;
using MessagesLoader.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesLoader
{
    public class MessageParser
    {
        static MessageParser()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public static IEnumerable<IEnumerable<VKMessageFlat>> Parse(string rootFolderPath, uint pageCountInBatch = 10)
        {            
            var cyrillica = Encoding.GetEncoding("windows-1251");
            var rusCult = Utility.CreateRusCulture();
            var directories = Directory.GetDirectories(rootFolderPath);
            var pageCounter = 0;
            var banch = new List<VKMessage>();

            foreach (var dir in directories)
            {
                var person = new VKСontactPerson();

                foreach (var file in Directory.GetFiles(dir, "*.html", SearchOption.TopDirectoryOnly))
                {

                    if (pageCounter == pageCountInBatch)
                    {
                        yield return banch.Select(x => VKMessageFlat.CreateFrom(x));

                        banch.Clear();

                        pageCounter = 0;
                    }

                    var doc = new HtmlDocument();

                    doc.OptionDefaultStreamEncoding = cyrillica;
                    doc.DetectEncodingAndLoad(file);

                    person.Name = doc.DocumentNode.SelectSingleNode("//div[@class='page_block_header_inner _header_inner']/div[last()]").GetDirectInnerText();

                    foreach (var messageNode in doc.DocumentNode.SelectNodes("//div[@class='wrap_page_content']/div[@class='item']//div[@class='message']"))
                    {
                        var messageChilds = messageNode.SelectNodes("div");

                        var authorNode = messageChilds[0];
                        var messageTextNode = messageChilds[1];
                        var authorLinkNode = authorNode.SelectSingleNode("a");
                        var message = new VKMessage(person);


                        if (authorLinkNode != null)
                        {
                            message.СontactPerson.Name = authorLinkNode.GetDirectInnerText();
                            message.Author = message.СontactPerson.Name;
                            message.СontactPerson.Link = authorLinkNode.GetAttributeValue("href", "none");
                        }

                        message.Message = messageTextNode.GetDirectInnerText();

                        message.Attachments = ParseAttachments(messageTextNode);

                        message.Author += authorNode.GetDirectInnerText();

                        var authorAndDateArray = message.Author.Split(", ");

                        message.Author = authorAndDateArray[0].Trim();

                        message.Date = DateTime.ParseExact(authorAndDateArray[1].Trim(), "d MMM yyyy в H:mm:ss", rusCult, DateTimeStyles.None);

                        message.FilePath = file;

                        banch.Add(message);
                    }

                    pageCounter++;
                }
            }

            if (banch.Count > 0)
            {
                yield return banch.Select(x => VKMessageFlat.CreateFrom(x));
            }
        }

        public static IEnumerable<VKAttachment> ParseAttachments(HtmlNode messageTextNode)
        {
            var attachmentNodes = messageTextNode.SelectNodes("div[@class='kludges']/div[@class='attachment']") ?? new HtmlNodeCollection(messageTextNode);

            foreach (var node in attachmentNodes)
            {
                var attachmentDescription = node.SelectSingleNode("div[@class='attachment__description']").GetDirectInnerText();

                var attachmentLinkNode = node.SelectSingleNode("a[@class='attachment__link']");

                if (attachmentLinkNode == null)
                    continue;

                var attachmentLink = attachmentLinkNode.GetAttributeValue("href", "none");

                yield return new VKAttachment { Type = attachmentDescription, Link = attachmentLink };
            }
        }
    }
}
