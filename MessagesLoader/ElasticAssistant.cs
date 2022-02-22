using MessagesLoader.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesLoader
{
    public class ElasticAssistant
    {
        private readonly string _indexName = "vkmessages";
        private ElasticClient _elasticClient;

        public ElasticAssistant()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex(_indexName);

            _elasticClient = new ElasticClient(settings);
        }

        public void CreateIndex()
        {
            var existResp = _elasticClient.Indices.Exists(new IndexExistsRequest(_indexName));

            if (existResp.Exists)
            {
                return;
            }

            var createIndexResponse = _elasticClient.Indices.Create(_indexName, c => c
                .Settings(s => s
                    .Analysis(a => a
                        .TokenFilters(tf => tf
                            .Stop("russian_stop", st => st.StopWords("_russian_"))
                            .Stemmer("russian_stemmer", stem => stem.Language("russian"))
                        )
                        .Analyzers(an => an
                            .Custom("rebuilt_russian", ca => ca
                                .Tokenizer("standard")
                                .Filters("lowercase", "russian_stop", "russian_stemmer")
                            )
                        )
                    )
                )
                .Map<VKMessageFlat>(m => m
                    .AutoMap()
                    .Properties(ps => ps
                        .Text(s => s
                            .Name(e => e.СontactPersonName).Analyzer("rebuilt_russian").SearchAnalyzer("rebuilt_russian")
                        )
                        .Text(s => s
                            .Name(e => e.Author).Analyzer("rebuilt_russian").SearchAnalyzer("rebuilt_russian")
                        )
                        .Text(s => s
                            .Name(e => e.Message).Analyzer("rebuilt_russian").SearchAnalyzer("rebuilt_russian")
                        )
                        .Object<VKAttachment>(o => o
                            .Name(e => e.Attachments)
                            .Properties(eps => eps
                                .Text(e => e
                                    .Name(e => e.Type).Analyzer("rebuilt_russian").SearchAnalyzer("rebuilt_russian")
                                )
                            )
                        )
                    )  
                )
            );

            if (!createIndexResponse.Acknowledged)
            {
                throw new Exception("Elasticsearch index has not been created! ");
            }
        }

        public void IndexDocuments(IEnumerable<VKMessageFlat> messages)
        {
            var indexManyResponse = _elasticClient.IndexMany(messages);

            if (indexManyResponse.Errors)
            {
                foreach (var itemWithError in indexManyResponse.ItemsWithErrors)
                {
                    Console.WriteLine($"Failed to index document {itemWithError.Id}: {itemWithError.Error}");
                }
            }
        }
    }
}
