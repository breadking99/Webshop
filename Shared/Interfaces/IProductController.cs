using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Queries;

namespace Shared.Interfaces;

//! products
public interface IProductController
{
     //!
     Task<ActionResult<List<Product>>> GetProductsAsync(PagerQuery? pager);
     //! {id}
     Task<ActionResult<Product>> GetProductByIdAsync(int id);
}