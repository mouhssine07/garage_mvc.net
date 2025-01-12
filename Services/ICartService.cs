using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICartService
{
    Task<List<garage.Models.Cart>> GetCartItemsAsync();
    Task RemoveFromCartAsync(int productId);
    Task SaveCartAsync();
}
