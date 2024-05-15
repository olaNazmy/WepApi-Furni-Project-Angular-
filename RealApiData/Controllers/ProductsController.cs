using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealApiData.Models;
using RealApiData.Repository;
using Microsoft.Extensions.Configuration;
using RealApiData.Repository.Abstract;
using RealApiData.DTO;


namespace RealApiData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //get  repos from generic repo
        public ProductRepository productRepository;
        private readonly IFileService fileService;

        public ProductsController(ProductRepository _prepo, IFileService _fileService)
        {
            productRepository = _prepo;
            fileService = _fileService;

        }

        // 1- GET: api/products 
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await productRepository.GetProducts();
                return Ok(products);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Status
                {
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult Add( Product model)
        {
            var status = new Status();
            Console.WriteLine("Adding new product: ", model); // Log the product object

            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass the valid data";
                return BadRequest(status);
            }


            var productResult = productRepository.Add(model);
            if (productResult)
            {
                status.StatusCode = 1;
                status.Message = "Added successfully";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error on adding product";
            }

            Console.WriteLine("Product added: ", status); // Log the status object

            return Ok(status);
        }


        //3- eg. PUT: api/product/3 
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,  Product productToUpdate)
        {
            try
            {
                if (id != productToUpdate.id)
                    return StatusCode(StatusCodes.Status400BadRequest,
                    new Status
                    {
                        StatusCode = 400,
                        Message = "Id in url and form body does not match."
                    });
                await productRepository.UpdateAsync(productToUpdate);
                return Ok(new Status
                {
                    StatusCode = 200,
                    Message = "Updated successfully"
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Status
                {
                    StatusCode = 500,
                    Message = ex.Message
                });
            }

        }

        // 4- eg. DELETE: api/product/3 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await productRepository.FindByIdAsync(id);
                if (product == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Status
                    {
                        StatusCode = 404,
                        Message = $"product with id: {id} does not exists"
                    });
                }
                await productRepository.DeleteAsync(product);
                await fileService.DeleteImage(product.ImageUrl);

                return Ok(new Status
                {
                    StatusCode = 200,
                    Message = $"Product with id: {id} is deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Status
                {
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }

        //5- get by id


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await productRepository.FindByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new Status
                    {
                        StatusCode = 404,
                        Message = $"Product with id: {id} not found"
                    });
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Status
                {
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }



    }
}
