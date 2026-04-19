using DataEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TinyShop.Tests;

[TestClass]
public class ProductTests
{
    [TestMethod]
    // Verify a newly created Product has the expected default values
    public void Product_NewInstance_HasDefaultValues()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        Assert.AreEqual(0, product.Id, "Default Id should be 0");
        Assert.IsNull(product.Name, "Default Name should be null");
        Assert.IsNull(product.Description, "Default Description should be null");
        Assert.AreEqual(0m, product.Price, "Default Price should be 0m");
        Assert.IsNull(product.ImageUrl, "Default ImageUrl should be null");
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(12345)]
    // Test that the Id property preserves assigned integer values
    public void Id_SetAndGet_ReturnsExpectedValue(int id)
    {
        // Arrange
        var product = new Product();

        // Act
        product.Id = id;

        // Assert
        Assert.AreEqual(id, product.Id, $"Product.Id should return the value that was set. Expected: {id}");
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("Simple name")]
    [DataRow("This is a reasonably long product name used for testing purposes to exercise property behavior")]
    // Test multiple name values using DataRow to ensure Name round-trips correctly
    public void Name_SetAndGet_ReturnsExpectedValue(string name)
    {
        // Arrange
        var product = new Product();

        // Act
        product.Name = name;

        // Assert
        Assert.AreEqual(name, product.Name, "Product.Name did not return the expected value after set.");
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("Short description")]
    [DataRow("Detailed description with multiple sentences. It should be preserved exactly as assigned.")]
    // Verify Description property preserves various string values
    public void Description_SetAndGet_ReturnsExpectedValue(string description)
    {
        var product = new Product();

        product.Description = description;

        Assert.AreEqual(description, product.Description, "Product.Description did not round-trip the assigned value.");
    }

    [DataTestMethod]
    // Prices are provided as strings because attribute parameters do not support decimal literals.
    [DataRow("0")]
    [DataRow("0.01")]
    [DataRow("99.99")]
    [DataRow("-1")]
    [DataRow("79228162514264337593543950335")] // decimal.MaxValue as string
    // Test that Price preserves assigned decimal values, including edge and negative values
    public void Price_SetAndGet_ReturnsExpectedValue(string priceString)
    {
        // Arrange
        var product = new Product();
        var price = decimal.Parse(priceString, System.Globalization.CultureInfo.InvariantCulture);

        // Act
        product.Price = price;

        // Assert
        Assert.AreEqual(price, product.Price, $"Product.Price did not return the expected value. Expected: {price}");
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("/images/product.png")]
    [DataRow("https://example.com/assets/image.jpg")]
    // Verify ImageUrl property preserves various URL and path values
    public void ImageUrl_SetAndGet_ReturnsExpectedValue(string imageUrl)
    {
        var product = new Product();

        product.ImageUrl = imageUrl;

        Assert.AreEqual(imageUrl, product.ImageUrl, "Product.ImageUrl did not round-trip the assigned value.");
    }
}
