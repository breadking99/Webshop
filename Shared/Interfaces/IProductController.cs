using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Queries;

namespace Shared.Interfaces;

//! products
public interface IProductController
{
     //!
     Task<ActionResult<List<Product>>> GetProductsAsync(ProductFilter? pager);
     //! {id}
     Task<ActionResult<Product>> GetProductByIdAsync(int id);
}