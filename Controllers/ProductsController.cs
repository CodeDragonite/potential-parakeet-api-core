using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Models;

namespace WebApiCore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly ShopContext _context;

    public ProductsController(ILogger<ProductsController> logger, ShopContext context)
    {
        _logger = logger;
        _context = context;
        _context.Database.EnsureCreated();
    }

    
    /*
    public IEnumerable<Product> GetAllProducts()
    {
        return _context.Products.ToList();
        
    }*/

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        var products = await _context.Products.ToArrayAsync();
        return Ok(products);
    }
    [HttpGet, Route("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();
        return Ok(product);
    }

    [HttpGet, Route("/products/{id}")]
    public async Task<ActionResult<Product>> GetProductShiel(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();
        return Ok(product);
    }


    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        if(!ModelState.IsValid)
            return BadRequest();
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(
            "GetProduct",
            new { id = product.Id },
            product);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> PutProduct(int id, Product product)
    {
        if(id != product.Id)
            return BadRequest();


        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {

            if (!_context.Products.Any(p => p.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {

        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return product;
    }


    [HttpPost]
    [Route("Delete")]
    public async Task<ActionResult> DeleteBatchProduct([FromBody] int[] ids)
    {

        var products = await _context.Products.FindAsync(ids);

        if (products == null)
            return NotFound();
        _context.Products.RemoveRange(products);
        await _context.SaveChangesAsync();
        return Ok(products);
    }

}
