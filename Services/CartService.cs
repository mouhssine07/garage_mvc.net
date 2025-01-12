using System.Collections.Generic;
using System.Threading.Tasks;

public class CartService : ICartService
{
    private readonly List<garage.Models.Cart> _cartItems = new List<garage.Models.Cart>();

    public Task<List<garage.Models.Cart>> GetCartItemsAsync()
    {
        return Task.FromResult(_cartItems.ToList());
    }

    public Task RemoveFromCartAsync(int productId)
    {
        var item = _cartItems.FirstOrDefault(c => c.ProductId == productId);
        if (item != null)
        {
            _cartItems.Remove(item);
        }
        return Task.CompletedTask;
    }

    public Task SaveCartAsync()
    {
        // Implementation here
        return Task.CompletedTask;
    }
}
