using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using PXPayBackend.Data;
using PXPayBackend.Models;

namespace PXPayBackend.Services;

/// <summary>
/// 商品服務實作
/// </summary>
public class ProductService : IProductService
{
    private readonly IMemoryCache _cache;
    private readonly TodoContext _context;
    
    // TODO 任務 1：完成 Constructor（注入 Cache 和 DbContext）
    public ProductService(IMemoryCache cache, TodoContext context)
    {
        _cache = cache;
        _context = context;
    }
    
    // TODO 任務 2：
    // 提示：
    // 1. 使用 await Task.Delay(500) 模擬慢速查詢
    // 2. 使用 _context.Products.Where().FirstOrDefaultAsync()
    // 3. 回傳 product?.Stock ?? 0
    private async Task<int> GetStockFromDatabaseAsync()
    {

        // 模擬資料庫查詢需要時間（InMemory 太快，用 Task.Delay 讓它變慢）
        await Task.Delay(500);
        
        var product = await _context.Products
            .Where(p => p.Name == "福利熊玩偶")
            .FirstOrDefaultAsync();
        
        // 如果是 null 回傳 0
        return product?.Stock ?? 0;
       
    }
    
    // TODO 任務 3：查庫存（有 Cache 的版本）
    public async Task<int> GetStockAsync()
    {
        // 1. 先檢查 Cache 有沒有資料
        // 用key識別資料 
        var cacheKey = "product_stock";
        
        // 2.檢查cache 用 TryGetValue()取值 回傳bool裡有沒有這筆cacheKey 
        // out這個變數會被方法賦值  
        ////沒有cacheKey->執行查找資料庫, 存到cache ->回傳
        if (!_cache.TryGetValue(cacheKey, out int stock))
         {
             // Cache Miss：沒有快取，需要查詢資料庫
            stock = await GetStockFromDatabaseAsync();
        
            // 把資料存入 Cache（設定5 秒後過期）
            //因為cache不會自動更新 
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
            };
            _cache.Set(cacheKey, stock, cacheOptions);
         }
        
        
        return stock;
    }
    
    // TODO 任務 4：查庫存 沒有 Cache 的版本）
    // 提示：
    // 1. 直接呼叫 GetStockFromDatabaseAsync()
    // 2. 回傳結果
    public async Task<int> GetStockNoCacheAsync()
    {
        
        return await GetStockFromDatabaseAsync();
    }




    public async Task<bool> InitTestDataAsync()
    {
        var exists = await _context.Products.AnyAsync(p => p.Name == "福利熊玩偶");
        if (exists)
        {
            return false;
        }

        var product = new Product
        {
            Name = "福利熊玩偶",
            Stock = 1000,
            Price = 299m
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return true;
    }
}
