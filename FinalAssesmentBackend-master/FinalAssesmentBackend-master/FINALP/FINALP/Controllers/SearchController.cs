using FINALP.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FINALP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        ProductContext pcx = new ProductContext();
        //Search API
        [HttpGet("{name}/{id}")]
        public IActionResult Search(string name,int id)
        {
            
            //Product p = new Product();
            
            var pdata = from a in pcx.Products
                        select a;
            if (!String.IsNullOrEmpty(name) )
            {
                if (id == 1)    // Search by ProductName
                {
                    pdata = pdata.Where(i => i.ProductName.Contains(name) || i.ProductName.StartsWith(name));
                }
                else if(id == 2)      // Search by ProductDesc
                {
                    pdata = pdata.Where(i => i.ProductDescription.Contains(name) || i.ProductDescription.StartsWith(name));
                }
                //else if (id == 3)
                //{
                //    pdata = pdata.Where(i => (i.ProductManufacturingDate).ToString().Contains(name) || i.ProductManufacturingDate.ToString().StartsWith(name));
                //}
                //else if (id == 4)
                //{
                //    pdata = pdata.Where(i => (i.ProductExpiryDate).ToString().Contains(name) || i.ProductExpiryDate.ToString().StartsWith(name));
                //}

                return new JsonResult(pdata);
            }

            return BadRequest();

        }
    }
}
