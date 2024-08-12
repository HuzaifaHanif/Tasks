using Confluent.Kafka;
using Nest;
using ServiceContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly ElasticClient _client;

        public ElasticSearchService() 
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                            .DefaultIndex("users");
            _client = new ElasticClient(settings);
        }
        public string IndexUser(User user)
        {
            var response = _client.IndexDocument(user);

            if (!response.IsValid)
            {
                return response.ServerError.Error.Reason;
            }

            return string.Empty;
        }

        public async Task<List<User>> GetUser(string text)
        {
            //var searchResponse = await _client.SearchAsync<User>(s => s
            //.Query(q => q
            //   .QueryString(qs => qs
            //       .Query(text)
            //        )
            //    )
            //);

            var searchResponse = await _client.SearchAsync<User>(s => s
                    .Query(q => q
                        .MultiMatch(mm => mm
                            .Fields(f => f
                                .Field(user => user.Name)
                                .Field(user => user.Street)
                                .Field(user => user.message)
                            )
                            .Query(text)
                        )
                    )
                );


            if (!searchResponse.IsValid) 
            {
                Console.WriteLine($"Failed to search documents: {searchResponse.ServerError?.Error?.Reason}");
                return null; 
            }

            return searchResponse.Documents.ToList();

        }

    }
}
