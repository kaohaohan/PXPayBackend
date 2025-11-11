# CI/CD 實作說明

## 🎯 什麼是 CI/CD？

### CI（Continuous Integration - 持續整合）
每次開發者推送程式碼時，自動執行：
- 編譯（Build）
- 測試（Test）
- 程式碼品質檢查（Code Quality）

**目的**：確保新程式碼不會破壞現有功能

### CD（Continuous Deployment - 持續部署）
測試通過後，自動部署到：
- 測試環境（Staging）
- 正式環境（Production）

**目的**：加快交付速度，減少人為錯誤

---

## 📋 本專案的 CI/CD 實作

### 已實作功能

#### 1. GitHub Actions（`.github/workflows/dotnet.yml`）

**觸發時機**：
- 推送到 `main` 或 `master` 分支
- 建立 Pull Request

**執行步驟**：
1. ✅ 檢出程式碼
2. ✅ 設定 .NET 8.0 環境
3. ✅ 還原 NuGet 套件（`dotnet restore`）
4. ✅ 編譯專案（`dotnet build`）
5. ✅ 執行測試（`dotnet test`）

**查看方式**：
- 前往 GitHub 專案頁面
- 點擊上方的 **Actions** 頁籤
- 可以看到每次 push 的建置狀態（✅ 成功 / ❌ 失敗）

---

#### 2. GitLab CI（`.gitlab-ci.yml`）

**觸發時機**：
- 推送到 `main` 分支
- 建立 Merge Request
- 建立 Tag

**執行階段**：
1. **Build 階段**：
   - 還原套件
   - 編譯專案
   - 保留編譯產物（1 小時）

2. **Test 階段**：
   - 執行測試
   - 程式碼格式檢查（允許失敗）

**特色**：
- 使用 Docker 映像檔（`mcr.microsoft.com/dotnet/sdk:8.0`）
- 快取 NuGet 套件（加速建置）
- 保留建置產物供後續階段使用

---

## 🎤 面試回答範本

### 問題 1：你有 CI/CD 經驗嗎？

**✅ 好的回答：**

> "我在這個專案實作了 CI/CD，使用 **GitHub Actions** 和 **GitLab CI**。"
>
> "每次 push 程式碼時，會自動執行 `dotnet restore`、`dotnet build`、`dotnet test`，確保程式碼品質。如果編譯失敗或測試不通過，GitHub 會立刻通知我。"
>
> "這樣做的好處是：**提早發現問題**、**確保程式碼品質**、**團隊協作更順暢**。"

---

### 問題 2：CI/CD 的好處是什麼？

**✅ 好的回答：**

> "CI/CD 有三個主要好處："
>
> "1. **提早發現問題**：每次 commit 都會自動測試，不用等到上線才發現 bug"
>
> "2. **加快交付速度**：自動化部署，不需要手動操作，減少人為錯誤"
>
> "3. **提升程式碼品質**：強制執行測試和程式碼檢查，確保每次提交都符合標準"
>
> "在全支付這種金流系統，CI/CD 特別重要，因為任何一個 bug 都可能造成金額錯誤。"

---

### 問題 3：GitHub Actions 和 GitLab CI 有什麼差別？

**✅ 好的回答：**

> "兩者的核心概念是一樣的，都是用 YAML 檔案定義工作流程。"
>
> "**GitHub Actions** 使用 `workflow` 和 `jobs`，設定檔在 `.github/workflows/`"
>
> "**GitLab CI** 使用 `stages` 和 `jobs`，設定檔是根目錄的 `.gitlab-ci.yml`"
>
> "我的專案兩個都實作了，所以不管公司用哪一個，我都能快速上手。"

---

### 問題 4：你的 CI/CD Pipeline 包含哪些步驟？

**✅ 好的回答：**

> "我的 Pipeline 包含三個主要步驟："
>
> "1. **Build（編譯）**：`dotnet restore` 還原套件，`dotnet build` 編譯專案"
>
> "2. **Test（測試）**：`dotnet test` 執行單元測試（目前專案還沒加測試，但架構已經準備好了）"
>
> "3. **Code Quality（程式碼品質）**：`dotnet format` 檢查程式碼格式"
>
> "如果任何一個步驟失敗，整個 Pipeline 就會停止，並通知開發者。"

---

### 問題 5：如果 CI/CD 失敗了，你會怎麼處理？

**✅ 好的回答：**

> "我會按照這個流程處理："
>
> "1. **查看 Log**：先看 GitHub Actions 或 GitLab CI 的執行日誌，找出是哪一步失敗"
>
> "2. **本地重現**：在本地執行相同的指令（例如 `dotnet build`），確認問題"
>
> "3. **修正問題**：可能是編譯錯誤、測試失敗、或套件版本衝突"
>
> "4. **重新推送**：修正後 commit 並 push，CI/CD 會自動重新執行"
>
> "5. **預防再發生**：如果是常見問題，可以在 CI/CD 加入檢查，提早發現"

---

## 🔧 如何查看 CI/CD 執行結果

### GitHub Actions

1. 前往 https://github.com/kaohaohan/PXPayBackend
2. 點擊上方的 **Actions** 頁籤
3. 可以看到每次 push 的建置記錄
4. 點擊任一記錄，可以看到詳細的執行步驟和 Log

### GitLab CI

1. 如果專案在 GitLab 上，前往專案頁面
2. 點擊左側的 **CI/CD** → **Pipelines**
3. 可以看到每次 push 的 Pipeline 狀態
4. 點擊任一 Pipeline，可以看到各個 Stage 的執行結果

---

## 📚 延伸學習

如果面試官問到更進階的 CI/CD 主題，可以這樣回答：

### CD（持續部署）

> "目前我的專案只做到 CI（持續整合），還沒做 CD（持續部署）。"
>
> "如果要做 CD，我會在 Pipeline 加入部署步驟，例如：部署到 Azure App Service 或 AWS，使用 Docker 容器化，並設定環境變數和資料庫連線。"

### 環境分離

> "企業級的 CI/CD 通常會有多個環境：Dev（開發）、Staging（測試）、Production（正式）。"
>
> "可以設定不同的分支觸發不同的部署，例如：`develop` 分支部署到 Dev 環境，`main` 分支部署到 Production。"

### 安全性

> "CI/CD 的敏感資訊（例如資料庫密碼、API Key）不應該寫在程式碼裡，應該使用 GitHub Secrets 或 GitLab CI Variables 來管理。"

---

## ✅ 總結

**你已經實作的 CI/CD 功能：**
- ✅ GitHub Actions 自動建置和測試
- ✅ GitLab CI 自動建置和測試
- ✅ 程式碼格式檢查
- ✅ 完整的 YAML 設定檔

**面試時可以展示：**
- 打開 GitHub Actions 頁面，展示建置記錄
- 說明 `.github/workflows/dotnet.yml` 的內容
- 說明 CI/CD 的好處和重要性

**你的優勢：**
- 2 天學會 .NET，並且實作了 CI/CD
- 展現了快速學習能力和主動性
- 理解 CI/CD 的核心概念和實務應用

---

**浩瀚，你現在已經有 CI/CD 經驗了！明天面試就大膽說出來！** 🚀

