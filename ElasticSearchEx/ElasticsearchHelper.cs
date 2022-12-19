using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchEx
{
    public class ElasticsearchHelper
    {
        public static ElasticClient GetESClient()
        {
            //If you work local, then use SingleNodeConnection
            /*var uri = new Uri("http://localhost:9200");
            var pool = new SingleNodeConnectionPool(uri);
            var client = new ElasticClient(new ConnectionSettings(pool));*/

            var credentials = new BasicAuthenticationCredentials("elastic", "example123");
            string cloudId = "Sample:examle123";
            var pool = new CloudConnectionPool(cloudId, credentials);
            var client = new ElasticClient(new ConnectionSettings(pool));
            return client;
        }
        public static void CreateDocument(ElasticClient client, string indexName, Product product, string documentId)
        {
            var response = client.Index(product, i => i
            .Index(indexName)
            .Id(documentId)
            .Refresh(Elasticsearch.Net.Refresh.True));
        }
        public static void GetDocument(ElasticClient client, string indexName, string documentId)
        {
            var response = client.Search<Product>(s => s
            .Index(indexName)
            .Query(q => q.Term(t => t.Field("_id").Value(documentId))));
            foreach (var hit in response.Hits)
            {
                Console.WriteLine("Id:{0} Name:{1} Description:{2} Category:{3} Price:{4}", hit.Source.Id, hit.Source.Name, hit.Source.Description, hit.Source.Category, hit.Source.Price);
            }
        }
        public static void GetAllDocument(ElasticClient client, string indexName)
        {
            var response = client.Search<Product>(q => q
            .Index(indexName)
            .MatchAll());
            //.SearchType(Elasticsearch.Net.SearchType.QueryThenFetch).Scroll("5m"));
            foreach (var hit in response.Hits)
            {
                Console.WriteLine("Id:{0} Name:{1} Description:{2} Category:{3} Price:{4}", hit.Source.Id, hit.Source.Name, hit.Source.Description, hit.Source.Category, hit.Source.Price);
            }

        }
        public static void UpdateDocument(ElasticClient client, string indexName, Product product, string documentId)
        {
            var response = client.Index(product, i => i
            .Index(indexName)
            .Id(documentId)
            .Refresh(Elasticsearch.Net.Refresh.True));
        }
        public static void DeleteDocument(ElasticClient client, string indexName, string documentId)
        {
            var response = client.Delete<Product>(documentId, d => d.Index(indexName));
        }

        public static void GetProductByCategory(ElasticClient client, string indexName, string category)
        {
            var response = client.Search<Product>(s => s
            .Index(indexName)
            .Size(50)
            .Query(q => q
            .Match(m => m
            .Field(f => f.Category)
            .Query(category))));
            Console.WriteLine("Product category matches to {0}", category);
            foreach (var hit in response.Hits)
            {
                Console.WriteLine("Id:{0} Name:{1} Description:{2} Category:{3} Price:{4}", hit.Source.Id, hit.Source.Name, hit.Source.Description, hit.Source.Category, hit.Source.Price);
            }
        }
        public static void GetProductByPriceRange(ElasticClient client, string indexName, int priceLowerLimit, int priceHigherLimit)
        {
            var response = client.Search<Product>(s => s
            .Index(indexName)
            .Size(50)
            .Query(q => q
            .Range(m => m
            .Field(f => f.Price)
            .GreaterThanOrEquals(priceLowerLimit)
            .LessThan(priceHigherLimit))));
            Console.WriteLine("Product Price range between {0}-{1}", priceLowerLimit, priceHigherLimit);
            foreach (var hit in response.Hits)
            {
                Console.WriteLine("Id:{0} Name:{1} Description:{2} Category:{3} Price:{4}", hit.Source.Id, hit.Source.Name, hit.Source.Description, hit.Source.Category, hit.Source.Price);
            }
        }
        public static void GetProductByCategoryPriceRange(ElasticClient elasticClient, string indexName, string category, int priceLowerLimit, int priceHigherLimit)
        {
            var response = elasticClient.Search<Product>(s => s
            .Index(indexName)
            .Size(50)
            .Query(q => q
            .Bool(b => b
            .Must(m =>
            m.Match(mt1 => mt1.Field(f1 => f1.Category).Query(category)) &&
            m.Range(ran => ran.Field(f1 => f1.Price).GreaterThanOrEquals(priceLowerLimit).LessThan(priceHigherLimit))))));
            Console.WriteLine("Product Category:{0} Price range between {1}-{2}", category, priceLowerLimit, priceHigherLimit);
            foreach (var hit in response.Hits)
            {
                Console.WriteLine("Id:{0} Name:{1} Description:{2} Category:{3} Price:{4}", hit.Source.Id, hit.Source.Name, hit.Source.Description, hit.Source.Category, hit.Source.Price);
            }
        }
    }
}

