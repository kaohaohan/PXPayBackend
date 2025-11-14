# E-Commerce Backend API

ASP.NET Core Web API 專案，展示高併發 API 優化技術。

## 技術棧

- .NET 8.0 + ASP.NET Core Web API
- SQL Server + Entity Framework Core
- Redis 分散式快取
- Docker

## 快速開始

### 1. 啟動 SQL Server

```bash
docker start sqlserver
```

如果沒有容器，執行：
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" \
  -p 1433:1433 --name sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. 啟動應用

```bash
dotnet restore
dotnet ef database update
dotnet run
```

### 3. 測試

打開 Swagger：http://localhost:5000/swagger

或打開 demo.html 查看效能測試 Demo

## 專案結構

```
├── Controllers/        # API 控制器
├── Services/           # 業務邏輯層
├── Models/             # 資料模型
├── Data/               # DbContext
├── Migrations/         # 資料庫遷移
└── demo.html          # 效能測試前端
```

## 核心功能

### Database Indexing 優化

使用 B-Tree 索引優化查詢效能：
- Contains()：全表掃描，300ms
- StartsWith() + Index：前綴搜尋，3ms（快 100 倍）

實測 100,000 筆資料，100 併發測試，錯誤率 0%。

### Redis Cache

實作 Cache-Aside Pattern：
- 第一次：查詢資料庫
- 之後：從 Redis 讀取
- TTL：5 分鐘

### Connection Pooling

配置資料庫連線池，避免重複建立連線。

## API 端點

**Products API**
- `GET /api/products/search/{keyword}` - Contains 搜尋（無優化）
- `GET /api/products/search-starts-with/{keyword}` - StartsWith 搜尋（有索引）
- `GET /api/products/search-cached/{keyword}` - Redis 快取版本
- `POST /api/products/init` - 初始化測試資料

**TodoItems API**
- `GET /api/todoitems` - 查詢所有
- `GET /api/todoitems/{id}` - 查詢單筆
- `POST /api/todoitems` - 新增
- `PUT /api/todoitems/{id}` - 更新
- `DELETE /api/todoitems/{id}` - 刪除
- `DELETE /api/todoitems/batch` - 批次刪除（Transaction）

## 效能測試

使用 JMeter 壓測結果（100 併發）：

| 版本 | 回應時間 | 錯誤率 | 吞吐量 |
|------|---------|--------|--------|
| 無索引 | 200-300ms | 80% | 低 |
| 有索引 | 1-5ms | 0% | 高 300 倍 |

## 技術重點

- RESTful API 設計
- MVC + Service 分層架構
- Entity Framework Core ORM
- Async/Await 非同步程式設計
- LINQ 資料查詢
- ACID Transaction

## 資料庫配置

編輯 `appsettings.json`：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=TodoDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;",
    "Redis": "localhost:6379"
  }
}
```

## License

MIT
