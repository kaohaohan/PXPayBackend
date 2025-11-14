using Microsoft.EntityFrameworkCore;

namespace PXPayBackend.Models;

/// <summary>
/// 商品 Model - 高併發效能優化版本
/// 
/// 【效能優化重點】
/// 1. Name 欄位建立 B-Tree 索引 (Index Attribute)
/// 2. 支援 StartsWith 查詢時可利用索引 (O(log n))
/// 3. Contains 查詢無法使用索引 (O(n) 全表掃描)
/// </summary>
[Index(nameof(Name))]  // ← 關鍵優化：在 Name 欄位建立索引
public class Product
{
    public long Id { get; set; }
    
    /// <summary>
    /// 商品名稱 - 已建立索引，支援高效能前綴搜尋
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    public int Stock { get; set; }  
    
    [System.ComponentModel.DataAnnotations.Schema.Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
}

