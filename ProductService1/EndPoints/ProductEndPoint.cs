using System.Net;

namespace ProductService1.EndPoints
{
    public class ProductEndPoint : IEndPoint
    {
        public void MapEndpoint(WebApplication app)
        {
            var group = app.MapGroup("api/products");

            group.MapGet("/list", GetProducts)
                .Produces<APIResponse>(200)
                .AddEndpointFilter<AuthorizeEndPointFilter>()
                .Produces(400)
                .WithName("GetProducts")
                .WithOpenApi();
        }

        private async static Task<IResult> GetProducts()
        {
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

            // Create two dummy products
            var product1 = new ProductDTO
            {
                ID = 1,
                Name = "Product 3",
                Description = "Description for Product 1",
                Price = 10.00m
            };

            var product2 = new ProductDTO
            {
                ID = 2,
                Name = "Product 4",
                Description = "Description for Product 2",
                Price = 20.00m
            };

            // Add products to the response
            var products = new List<ProductDTO> { product1, product2 };

            response.Result = products;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Results.Ok(response);
        }

        // Sample ProductDTO class
        public class ProductDTO
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
        }

        // Sample APIResponse class
        public class APIResponse
        {
            public bool IsSuccess { get; set; }
            public object Result { get; set; }
            public HttpStatusCode StatusCode { get; set; }
            public List<string> ErrorMessages { get; set; } = new();
        }
    }
}
