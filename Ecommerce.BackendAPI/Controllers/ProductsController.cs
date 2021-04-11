using Ecommerce.Application.Catalog.Products;
using Ecommerce.ViewModel.Catalog.Product;
using Ecommerce.ViewModel.Catalog.ProductImage;
using Ecommerce.ViewModel.Common;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController( IProductService ProductService)
        {
            _productService = ProductService;
        }
        [HttpGet("paging/{languageId}")]
        public async Task<IActionResult> GetAllByCategoryId(string languageId, [FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _productService.GetAllByCategoryId(languageId, request);
            return Ok(products);
        }

        [HttpGet("Get_Product_By_Id/{productId}/{languageId}")]
        public async Task<IActionResult> ProductGetById(int productId, string languageId)
        {
            var result = await _productService.GetById(productId, languageId);
            if (result == null)
            {
                return BadRequest("Cannot find product");
            }
            return Ok(result);
        }

        [HttpPost("Create_Product")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.Create(request);
            if (result == 0)
            {
                return BadRequest();
            }
            var product = await _productService.GetById(result, request.LanguageId);
            return CreatedAtAction(nameof(ProductGetById), new { productId = result }, product);
        }

        [HttpPut("Update_Product")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateRequest request)
        {
            var affectResult = await _productService.Update(request);
            if (affectResult == 0)
            {
                return BadRequest();
            }
            return Ok(affectResult);
        }

        [HttpDelete("Delete_Product/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var affectResult = await _productService.Delete(productId);
            if (affectResult == 0)
            {
                return BadRequest();
            }
            return Ok(affectResult);
        }

        [HttpPatch("Update_Price/{productId}/{newPrice}")]
        public async Task<IActionResult> Update_Price_Product(int productId, decimal newPrice)
        {
            var affectResult = await _productService.UpdatePrice(productId, newPrice);
            if (!affectResult)
            {
                return BadRequest();
            }
            return Ok(affectResult);
        }

        //Images
        [HttpPost("{productId}/Create_Image")]
        public async Task<IActionResult> Create_Image(int productId,[FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.AddImage(productId, request);
            if (result == 0)
            {
                return BadRequest();
            }
            var image = await _productService.GetImageById(result);
            return CreatedAtAction(nameof(Get_Image_By_Id),new { imageId = result } ,image);
        }

        [HttpPost("{productId}/Update_Image")]
        public async Task<IActionResult> Update_Image(int productId,[FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.UpdateImage(productId, request);
            if (result == 0)
            {
                return BadRequest();
            }
            var image = await _productService.GetImageById(result);
            return CreatedAtAction(nameof(Get_Image_By_Id), new { imageId = result }, image);
        }

        [HttpDelete("{imageId}/Detele_Image")]
        public async Task<IActionResult> Detele_Image(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.RemoveImage(imageId);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("{imageId}/Get_Image_By_Id")]
        public async Task<IActionResult> Get_Image_By_Id(int imageId)
        {
            var image = await _productService.GetImageById(imageId);
            if (image==null)
            {
                return BadRequest();
            }
            return Ok(image);
        }
    }
}
