using Nest;

namespace ElasticSearchEx
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string INDEX_NAME = "newtestdb1";

            Product product = new Product
            {
                Id = "1",
                Name = "Samsung TV",
                Category = "LCD TV",
                Description = "Samsung LCD 100 inch TV",
                IsPopularProduct = 1,
                Price = 50000,
                Keywords = "Tv,LCD,SAMSUNG TV"
            };
            Product product2 = new Product
            {
                Id = "2",
                Name = "Apple Phone",
                Category = "PHONE",
                Description = "Iphone SE",
                IsPopularProduct = 1,
                Price = 600,
                Keywords = "Phone,Apple,SE"
            };
            Product product3 = new Product
            {
                Id = "3",
                Name = "Xiomi Phone",
                Category = "PHONE",
                Description = "Mi Note 6",
                IsPopularProduct = 1,
                Price = 800,
                Keywords = "Phone,Xiomi,Note"
            };
            Product product4 = new Product
            {
                Id = "4",
                Name = "Samsung Phone",
                Category = "PHONE",
                Description = "Samsung Note 9",
                IsPopularProduct = 1,
                Price = 900,
                Keywords = "Phone,Xiomi,Note"
            };

            //Connecting to ES
            ElasticClient elasticClient = ElasticsearchHelper.GetESClient();

            //Creating Document
            ElasticsearchHelper.CreateDocument(elasticClient, INDEX_NAME, product, product.Id);
            ElasticsearchHelper.CreateDocument(elasticClient, INDEX_NAME, product2, product2.Id);
            ElasticsearchHelper.CreateDocument(elasticClient, INDEX_NAME, product3, product3.Id);
            ElasticsearchHelper.CreateDocument(elasticClient, INDEX_NAME, product4, product4.Id);

            //Getting document by id
            ElasticsearchHelper.GetDocument(elasticClient, INDEX_NAME, product.Id);
            Console.WriteLine("**********************");

            //Get all products  by category
            ElasticsearchHelper.GetProductByCategory(elasticClient, INDEX_NAME, "PHONE");
            Console.WriteLine("**********************");

            //Get all products by price range
            ElasticsearchHelper.GetProductByPriceRange(elasticClient, INDEX_NAME, 500, 1000);
            Console.WriteLine("**********************");

            //Updating Documents
            product.Description = "Samsung SMART LCD TV";
            ElasticsearchHelper.UpdateDocument(elasticClient, INDEX_NAME, product, product.Id);
            ElasticsearchHelper.GetDocument(elasticClient, INDEX_NAME, product.Id);
            Console.WriteLine("**********************");

            //Deleting Documents by id
            ElasticsearchHelper.DeleteDocument(elasticClient, INDEX_NAME, product.Id);

            // Getting all documents
            ElasticsearchHelper.GetAllDocument(elasticClient, INDEX_NAME);
            Console.WriteLine("**********************");

            //Gett all products by category and price range
            ElasticsearchHelper.GetProductByCategoryPriceRange(elasticClient, INDEX_NAME, "PHONE", 700, 1000);

            Console.ReadLine();
        }
    }
}

