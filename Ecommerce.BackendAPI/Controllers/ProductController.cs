using Ecommerce.Application.Catalog.Products;
using Ecommerce.ViewModel.Catalog.Product;
using Ecommerce.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;
        public ProductController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }

        [HttpGet("{languageId}")]
        public async Task<IActionResult> Get(string languageId)
        {
            var products = await _publicProductService.GetAll(languageId);
            return Ok(products);
        }

        [HttpGet("public_paging")]
        public async Task<IActionResult> GetAllByCategoryId([FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _publicProductService.GetAllByCategoryId(request); 
            return Ok(products);
        }

        [HttpGet("Get_Product_By_Id/{productId}/{languageId}")]
        public async Task<IActionResult> ProductGetById(int productId,string languageId)
        {
            var result = await _manageProductService.GetById(productId, languageId);
            if (result == null)
            {
                return BadRequest("Cannot find product");
            }
            return Ok(result);
        }

        [HttpPost("Create_Product")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateRequest request)
        {
            var result = await _manageProductService.Create(request);
            if (result == 0)
            {
                return BadRequest();
            }
            var product = await _manageProductService.GetById(result, request.LanguageId);
            return CreatedAtAction(nameof(ProductGetById), new { productId = result }, product);
        }

        [HttpPut("Update_Product")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateRequest request)
        {
            var affectResult = await _manageProductService.Update(request);
            if (affectResult == 0)
            {
                return BadRequest();
            }
            return Ok(affectResult);
        }

        [HttpDelete("Delete_Product/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var affectResult = await _manageProductService.Delete(productId);
            if (affectResult == 0)
            {
                return BadRequest();
            }
            return Ok(affectResult);
        }

        [HttpPut("Update_Price/{productId}/{newPrice}")]
        public async Task<IActionResult> Update_Price_Product(int productId,decimal newPrice)
        {
            var affectResult = await _manageProductService.UpdatePrice(productId, newPrice);
            if (!affectResult)
            {
                return BadRequest();
            }
            return Ok(affectResult);
        }
    }
}
