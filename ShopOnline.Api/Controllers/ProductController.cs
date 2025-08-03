using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;
using System.IO;
using System.Net.Mail;
using System.Net;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Data; // Adjust namespace if your DbContext is elsewhere

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ShopOnlineDbContext _context; // Add this line

        public ProductController(IProductRepository productRepository, ShopOnlineDbContext context) // Add context to constructor
        {
            this.productRepository = productRepository;
            this._context = context; // Assign context
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await productRepository.GetItems();


                if (products == null)
                { 
                   return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto();

                    return Ok(productDtos);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
               
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await this.productRepository.GetItem(id);
               
                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    
                    var productDto = product.ConvertToDto();

                    return Ok(productDto);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }

        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await productRepository.GetCategories();
                
                var productCategoryDtos = productCategories.ConvertToDto();

                return Ok(productCategoryDtos);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }

        }

        [HttpGet]
        [Route("{categoryId}/GetItemsByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var products = await productRepository.GetItemsByCategory(categoryId);

                var productDtos = products.ConvertToDto();

                return Ok(productDtos);
            
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }

        [HttpPost("UploadSchilderij")]
        public async Task<IActionResult> UploadSchilderij([FromForm] IFormFile image, [FromForm] string name, [FromForm] string description, [FromForm] decimal price, [FromForm] int categoryId)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Geen afbeelding ontvangen.");

            var fileName = Path.GetFileName(image.FileName);
            var savePath = Path.Combine("images", "schilderijen", fileName);

            // Zorg dat de map bestaat
            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Voeg product toe aan database
            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price,
				ImageURL = $"/images/schilderijen/{fileName}",
                CategoryId = categoryId
            };

                _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("UploadProduct")]
        public async Task<IActionResult> UploadProduct([FromForm] IFormFile image, [FromForm] string name, [FromForm] string description, [FromForm] decimal price, [FromForm] int categoryId)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Geen afbeelding ontvangen.");

            var fileName = Path.GetFileName(image.FileName);
            var savePath = Path.Combine("images", "producten", fileName);

            // Zorg dat de map bestaat
            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Voeg product toe aan database
            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                ImageURL = $"/images/schilderijen/{fileName}",
                CategoryId = categoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
