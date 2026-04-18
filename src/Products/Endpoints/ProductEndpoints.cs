using DataEntities;
using Microsoft.EntityFrameworkCore;
using Products.Data;

namespace Products.Endpoints;

public static class ProductEndpoints
{
    /// <summary>
    /// Extension method to define product route mappings.
    /// </summary>
    /// <param name="routes">The endpoint route builder used to register product endpoints.</param>
    public static void MapProductEndpoints (this IEndpointRouteBuilder routes)
    {
        // MapGroup で共通プレフィックスを作る
        var group = routes.MapGroup("/api/Product");

        // グループ内に GET エンドポイントを追加（DI で Db コンテキストを受け取る）
        group.MapGet("/", async (ProductDataContext db) =>
        {
            return await db.Product.ToListAsync(); // これがそのまま JSON レスポンスになる
        })
        .WithName("GetAllProducts")
        .Produces<List<Product>>(StatusCodes.Status200OK);

        group.MapGet("/{productId}", async (int productId, ProductDataContext db) =>
        {
            var product = await db.Product.FindAsync(productId);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProductById")
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (Product product, ProductDataContext db) =>
        {
            db.Product.Add(product);
            await db.SaveChangesAsync();

            return Results.Created($"/api/Product/{product.Id}", product);
        })
        .WithName("CreateProduct")
        .Produces<Product>(StatusCodes.Status201Created);

        group.MapPut("/{id}", async (int id, Product input, ProductDataContext db) =>
        {
            var product = await db.Product.FindAsync(id);
            if (product is null)
                return Results.NotFound();

            product.Name = input.Name;
            product.Description = input.Description;
            product.Price = input.Price;
            product.ImageUrl = input.ImageUrl;

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateProduct")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id}", async (int id, ProductDataContext db) =>
        {
            var product = await db.Product.FindAsync(id);
            if (product is null)
                return Results.NotFound();

            db.Product.Remove(product);
            await db.SaveChangesAsync();

            return Results.Ok(product);
        })
        .WithName("DeleteProduct")
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
