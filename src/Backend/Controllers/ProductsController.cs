using Backend.Data;
using Backend.Dtos;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize is < 1 or > 100) pageSize = 10;

            var result = await _productService.LoadProductsAsync(page, pageSize);
            return Ok(new
            {
                currentPage = result.page,
                pageSize = result.size,
                totalItems = result.totalCount,
                totalPages = result.totalPage,
                data = result.items
            });

        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "An error occurred while fetching products.", details = e.Message });
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the product.", details = e.Message });
        }
    }


    [HttpPost("upsert")]
    public async Task<IActionResult> UpsertProduct([FromBody] ProductUpsertDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _productService.UpsertProductAsync(dto);
            
            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "An error occurred while saving the product.", details = e.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the product.", details = e.Message });
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string query, int page = 1, int pageSize = 10)
    {
        try
        {
            var result = await _productService.SearchProductsAsync(query, page, pageSize);
            
            return Ok(new
            {
                currentPage = result.page,
                pageSize = result.size,
                totalItems = result.totalCount,
                totalPages = result.totalPage,
                data = result.items
            });

        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "An error occurred while searching for products.", details = e.Message });
        }
    }



    
}