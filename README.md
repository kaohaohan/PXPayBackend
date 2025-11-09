# PXPayBackend - Todo API

這是我用 ASP.NET Core 寫的待辦事項 API，用來練習和展示完整的 CRUD 操作，並整合了 SQL Server 資料庫。

## 環境需求

- .NET 8.0 SDK
- Docker Desktop（用來跑 SQL Server）
- 任何能跑 .NET 的作業系統（Windows / macOS / Linux）

## 怎麼跑起來

### 1. 先啟動 SQL Server（用 Docker）

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" \
  -p 1433:1433 --name sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Clone 專案並還原套件

```bash
git clone https://github.com/kaohaohan/PXPayBackend.git
cd PXPayBackend
dotnet restore
```

### 3. 執行資料庫遷移（建立資料表）

```bash
dotnet ef database update
```

### 4. 啟動 API

```bash
dotnet run --urls "http://localhost:5000"
```

### 5. 打開 Swagger 測試頁面

```
http://localhost:5000/swagger
```

## API 功能

基本的增刪改查都有：

- `GET /api/todoitems` - 拿所有待辦事項
- `GET /api/todoitems/{id}` - 拿單一筆資料
- `POST /api/todoitems` - 新增待辦事項
- `PUT /api/todoitems/{id}` - 更新待辦事項
- `DELETE /api/todoitems/{id}` - 刪除待辦事項

## 專案結構

```
PXPayBackend/
├── Controllers/
│   └── TodoItemsController.cs    # API 邏輯（使用 async/await + DI）
├── Models/
│   └── TodoItem.cs                # 資料結構定義
├── Data/
│   └── TodoContext.cs             # EF Core DbContext（資料庫連線）
├── Migrations/                    # 資料庫遷移檔案
├── Program.cs                     # 程式進入點（IOC/DI 設定）
├── appsettings.json               # 資料庫連線字串設定
└── PXPayBackend.csproj           # 專案設定檔
```

## 用到的技術

- **ASP.NET Core Web API** - 主要框架
- **Entity Framework Core** - ORM（物件關聯映射）
- **SQL Server** - 關聯式資料庫
- **IOC/DI（依賴注入）** - 將 DbContext 注入到 Controller
- **async/await** - 非同步程式設計，提升效能
- **Swagger** - API 文件跟測試界面
- **LINQ & Lambda** - 資料查詢（例如 `await _context.TodoItems.FindAsync(id)`）
- **Migration（資料庫遷移）** - 自動管理資料庫結構變更

## 資料存儲

使用 **SQL Server** 作為資料庫，透過 **Entity Framework Core** 進行資料存取。資料會持久化儲存，伺服器重啟後資料不會消失。

## 測試方式

啟動後打開 Swagger 頁面，可以直接在瀏覽器上測試所有 API。

試試看：
1. 先 GET 看看有哪些資料
2. POST 新增一筆
3. 再 GET 一次確認有新增成功
4. 用 PUT 更新資料
5. 用 DELETE 刪掉

## 技術亮點

這個專案展示了企業級 ASP.NET Core 開發的核心技術：

1. **RESTful API 設計** - 標準的 HTTP 方法和狀態碼
2. **Entity Framework Core ORM** - Code First 方式管理資料庫
3. **IOC/DI 架構模式** - 鬆耦合、可測試的程式碼設計
4. **非同步程式設計** - 所有資料庫操作都使用 async/await
5. **資料庫遷移** - 使用 EF Core Migrations 管理資料表結構
6. **LINQ & Lambda 表達式** - 優雅的資料查詢語法
7. **錯誤處理** - 404 Not Found、400 Bad Request、並發處理
8. **ACID Transaction** - 批次刪除 API 展示交易處理（原子性、一致性）
9. **CI/CD 自動化** - GitHub Actions 自動建置和測試

## CI/CD 自動化

本專案已整合 **GitHub Actions** 和 **GitLab CI**，每次 push 或 PR 時會自動：

1. ✅ **還原套件** - `dotnet restore`
2. ✅ **編譯專案** - `dotnet build`
3. ✅ **執行測試** - `dotnet test`（如果有測試專案）
4. ✅ **程式碼品質檢查** - `dotnet format`

### GitHub Actions

查看建置狀態：前往專案的 **Actions** 頁籤

設定檔位置：`.github/workflows/dotnet.yml`

### GitLab CI

如果你想在 GitLab 上使用，專案已包含 `.gitlab-ci.yml` 設定檔，直接推送到 GitLab 即可自動觸發 CI/CD Pipeline。

## 注意事項

- 如果要修改資料庫連線字串，請編輯 `appsettings.json` 的 `ConnectionStrings` 區塊
- 執行專案前請確保 Docker 中的 SQL Server 容器正在運行
- 若要查看資料庫內容，可使用 Azure Data Studio 或 SQL Server Management Studio

有任何問題歡迎聯絡我！

