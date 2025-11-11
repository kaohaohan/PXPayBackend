using Microsoft.AspNetCore.Mvc;
using PXPayBackend.Services;

namespace PXPayBackend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProductsController : ControllerBase
{
   private readonly IProductService _productService;

       public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("stock")]
    public async Task<IActionResult> GetStock()
    {
        var stock = await _productService.GetStockAsync();
        return Ok(new { stock = stock }); 
    }

    [HttpGet("stock/no-cache")]
    public async Task<IActionResult> GetStockNoCache()
    {
        // 呼叫 GetStockNoCacheAsync()
         var stock = await _productService.GetStockNoCacheAsync();
        return Ok(new { stock = stock }); 
    }   

    [HttpPost("init")]
    public async Task<IActionResult> InitTestData()
    {
        // Hint 1: 呼叫 InitTestDataAsync()，回傳 bool記得加 await
        var success = await _productService.InitTestDataAsync();

        // Hint 2: 如果 true，回傳「測試資料已建立」
        if(success){
            return Ok(new { message = "成功" });
        }
        return Ok(new { message = "測試資料已存在" });
        // Hint 3: 如果 false，回傳「測試資料已存在」
    }
    
}