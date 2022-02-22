using MessagesLoader;
using System.Text;

//var elasticAssistant = new ElasticAssistant();

//elasticAssistant.CreateIndex();

foreach (var messageBanch in MessageParser.Parse(@"e:\VKArch\Archive\messages"))
{
    //foreach (var message in messageBanch)
    //{
    //    Console.WriteLine($"ContactName: {message.СontactPersonName}; " +
    //        $"ContactLink: {message.СontactPersonLink}; " +
    //        $"Author: {message.Author}; " +
    //        $"Message: {message.Message}; " +
    //        $"Date: {message.Date}; " +
    //        $"FilePath: {message.FilePath};");
    //}

    foreach (var message in messageBanch)
    {
        Console.WriteLine($"FilePath: {message.FilePath};");
    }

    //elasticAssistant.IndexDocuments(messageBanch);

    //Console.WriteLine($"Number of uploaded messages: {messageBanch.Count()}");
}
