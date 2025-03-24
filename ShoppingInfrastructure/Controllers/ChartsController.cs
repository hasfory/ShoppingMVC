using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace ShoppingInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private record CountByBrandResponseItem(string BrandName, int Count);
        private record CountByUserResponseItem(string UserEmail, int Count);

        private readonly ShoppingDbContext _context;

        public ChartsController(ShoppingDbContext context)
        {
            _context = context;
        }

        // 1) Кількість товарів за брендами
        [HttpGet("brandProductCount")]
        public async Task<JsonResult> GetBrandProductCountAsync(CancellationToken cancellationToken)
        {
            var responseItems = await _context.Products
                .GroupBy(p => p.Brand.BrandName)  
                .Select(g => new CountByBrandResponseItem(
                    g.Key,       
                    g.Count()    
                ))
                .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }

        // 2) Кількість платежів, зроблених кожним користувачем
        [HttpGet("userPaymentCount")]
        public async Task<JsonResult> GetUserPaymentCountAsync(CancellationToken cancellationToken)
        {
            var responseItems = await _context.Payments
                .GroupBy(p => p.User.PhoneOrEmail) 
                .Select(g => new CountByUserResponseItem(
                    g.Key,
                    g.Count()
                ))
                .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }
    }
}

