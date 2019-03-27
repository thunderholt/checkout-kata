namespace Checkout
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        Product GetProduct(string sku);
    }

    public interface ICheckout
    {
        void Scan(string sku);
        decimal GetTotalPrice();
    }
}
