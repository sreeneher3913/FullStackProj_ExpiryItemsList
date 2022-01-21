using FINALP.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FINALP.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext pc;
        private readonly IJwtAuthenticationManager jwtAuthenticationManager;
        public ProductsController(IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            pc = new ProductContext();
        }
        
        //private object jwtAuthenticationManager;

        //public ProductsController()
        //{
           
        //}
        //private readonly ProductContext _context;
        ////private readonly IConfiguration _configuration;

        //public ProductsController(ProductContext context)
        //{
        //    _context = context;
        //   // _configuration = configuration;
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            return await pc.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await pc.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return new JsonResult(product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            pc.Entry(product).State = EntityState.Modified;

            try
            {
                await pc.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return new JsonResult(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            pc.Products.Add(product);
            await pc.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = product.ProductId }, product);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await pc.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            pc.Products.Remove(product);
            await pc.SaveChangesAsync();

            return product;
        }
        private Boolean ProductExists(int id)
        {
            return pc.Products.Any(e => e.ProductId == id);
        }

        //private bool Product(int id)
        //{
        //    throw new NotImplementedException();
        //}
        
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserCred userCred)
        {
            var token = jwtAuthenticationManager.Authenticate(userCred.Username, userCred.Password);
            if (token == null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(token);
            }
        }


      
    }
}

