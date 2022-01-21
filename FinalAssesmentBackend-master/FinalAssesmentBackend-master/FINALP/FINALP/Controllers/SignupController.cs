using FINALP.SignUp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FINALP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private readonly ProductContext pc;
        public SignupController()
        { 
            pc = new ProductContext();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Userdetails>>> Get()
        {
            return await pc.Userdetails.ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult<Userdetails>> Post(Userdetails udetails)
        {
            pc.Userdetails.Add(udetails);

            await pc.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = udetails.Username }, udetails);

        }
    }
}
