using DataEntities;
using System.Text.Json;
using System.Net.Http.Json;

namespace Store.Services;

public class ProductService
{
    HttpClient httpClient;
    public ProductService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<Product?> GetProductById(int id)
    {
        var response = await httpClient.GetAsync($"/api/Product/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync(ProductSerializerContext.Default.Product);
    }

    public async Task<Product?> CreateProduct(Product product)
    {
        var response = await httpClient.PostAsJsonAsync("/api/Product", product);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync(ProductSerializerContext.Default.Product);
    }

    public async Task<bool> UpdateProduct(int id, Product product)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/Product/{id}", product);
        return response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.NoContent;
    }

    public async Task<Product?> DeleteProduct(int id)
    {
        var response = await httpClient.DeleteAsync($"/api/Product/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync(ProductSerializerContext.Default.Product);
    }
    public async Task<List<Product>> GetProducts()
    {
        List<Product>? products = null;
        var response = await httpClient.GetAsync("/api/Product");
        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            products = await response.Content.ReadFromJsonAsync(ProductSerializerContext.Default.ListProduct);
        }

        return products ?? new List<Product>();
    }
    
}