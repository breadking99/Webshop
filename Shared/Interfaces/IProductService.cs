using Shared.Models;
using Shared.Queries;
using Shared.Responses;

namespace Shared.Interfaces;

//! products
public interface IProductService
{
     //!
     Task<Response<List<Product>>> GetProductsAsync(PagerQuery? pager);
     //! {id}
     Task<Response<Product>> GetProductByIdAsync(int id);
}