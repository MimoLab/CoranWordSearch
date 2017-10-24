using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoranWordSearch.Models
{
    public class Elasticsearch
    {
        const string CORAN_INDEX_NAME = "coran_index";
        const string SOURATE_TYPE_NAME = "sourate";
        const string WORD_TYPE_NAME = "word";
        const string VERSET_TYPE_NAME = "verset";
        const string VERSETWORD_TYPE_NAME = "versetword";
        public static ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("http://elasticsearchserver.northeurope.cloudapp.azure.com:9200"));
        public static ElasticClient elasticClient = new ElasticClient(connectionSettings);


        public static List<VersetWord> GetWords(string name = "")
        {
            var wordsToReturn = new List<VersetWord>();
            var response = elasticClient.Search<VersetWord>(s => s
              .Index(CORAN_INDEX_NAME)
              .Type(VERSETWORD_TYPE_NAME)
              .Size(75000)
              //.Sort(tg=>tg.Ascending(t=> t.Name))
              .Query(q => q.QueryString(qs => qs.Query(name + "*"))));
            
            foreach (var hit in response.Hits)
            {
                wordsToReturn.Add(new VersetWord { Value = hit.Source.Value, VersetId = hit.Source.VersetId});
            }

            return wordsToReturn;
        }

        public static List<Sourate> GetSourates(List<VersetWord> versetWords)
        {
            var sourates = new List<Sourate>();
            var versets = new List<Verset>();
            var response = elasticClient.Search<Verset>(s => s
              .Index(CORAN_INDEX_NAME)
              .Type(VERSET_TYPE_NAME)
              .Size(75000)
              //.Sort(tg=>tg.Ascending(t=> t.Name))
              .Query(q => q.Terms(c => c
                            .Field(p => p.VersetId)
                            .Terms<int>(versetWords.Select(x => x.VersetId))
                            )))
                        ;
            var souratesIds = new List<int>();

            foreach (var hit in response.Hits)
            {
                souratesIds.Add(hit.Source.SourateId);
            }
            var responseSourates = elasticClient.Search<Sourate>(s => s
             .Index(CORAN_INDEX_NAME)
             .Type(SOURATE_TYPE_NAME)
             .Size(75000)
             //.Sort(tg=>tg.Ascending(t=> t.Name))
             .Query(q => q.Terms(c => c
                           .Field(p => p.SourateId)
                           .Terms<int>(souratesIds)
                           )))
                       ;
            foreach (var hit in responseSourates.Hits)
            {
                sourates.Add(hit.Source);
            }
            return sourates;
        }
    }
}