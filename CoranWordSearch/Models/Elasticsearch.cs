using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoranWordSearch.Models
{
    public class Elasticsearch
    {
        const string CORAN_INDEX_NAME = "coran_index_raw_words";
        const string SOURATE_TYPE_NAME = "sourate";
        const string WORD_TYPE_NAME = "word";
        const string VERSET_TYPE_NAME = "verset";
        const string VERSETWORD_TYPE_NAME = "versetword";
        public Elasticsearch()
        {
            connectionSettings = new ConnectionSettings(new Uri("http://coranelk.northeurope.cloudapp.azure.com:9200/"));
            connectionSettings.DisableDirectStreaming(true);
            elasticClient = new ElasticClient(connectionSettings);
        }
        public ConnectionSettings connectionSettings { get; }
        public ElasticClient elasticClient { get; }


        public List<VersetWord> GetWords(string name = "")
        {
            var wordsToReturn = new List<VersetWord>();
            var response = elasticClient.Search<VersetWord>(s => s
              .Index(CORAN_INDEX_NAME)
              .Type(VERSETWORD_TYPE_NAME)
              .Size(10000)
              //.Sort(tg=>tg.Ascending(t=> t.Name))
              .Query(q => q.QueryString(qs => qs.Query(name + "*"))));
            
            foreach (var hit in response.Hits)
            {
                wordsToReturn.Add(new VersetWord { Value = hit.Source.Value, VersetId = hit.Source.VersetId});
            }

            return wordsToReturn;
        }

        public List<Verset> GetVersets(string word)
        {
            var sourates = new List<Sourate>();
            var versets = new List<Verset>();
            var response = elasticClient.Search<Verset>(s => s
              .Index(CORAN_INDEX_NAME)
              .Type(VERSET_TYPE_NAME)
              .Size(10000)
              //.Sort(tg=>tg.Ascending(t=> t.Name))
              .Query(q => q.QueryString(qs => qs.Query(word + "*"))));
            ;

            foreach (var hit in response.Hits)
            {
                

                    versets.Add(new Verset { VersetId = hit.Source.VersetId, SourateId = hit.Source.SourateId, Content = hit.Source.Content, NumVerset = hit.Source.NumVerset });
                
            }

            return versets;
        }

        public List<Sourate> GetSourates(List<VersetWord> versetWords)
        {
            var sourates = new List<Sourate>();
            var versets = new List<Verset>();
            var response = elasticClient.Search<Verset>(s => s
              .Index(CORAN_INDEX_NAME)
              .Type(VERSET_TYPE_NAME)
              .Size(10000)
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
             .Size(10000)
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