# WaterDeliveryInfo

一個使用 ASP.NET Core MVC 製作的桶裝水單據管理系統，支援使用者登入、個人資料隔離，以及每一次的送水單據紀錄。

---

## Demo

👉 [Live Demo][https://waterdelieveryinfo.onrender.com]

---

## Screenshots

![Record](test/ScreenShots/record.jpg)
![Upload](test/ScreenShots/upload.jpg)
![Edit](test/ScreenShots/edit.jpg)
![AI](test/ScreenShots/ai.jpg)
![Delete](test/ScreenShots/delete.jpg)
![Login](test/ScreenShots/login.jpg)
![Register](test/ScreenShots/register.jpg)


## 部署注意事項 (Render Free Plan)

本專案部署於 Render 免費方案，由於平台限制，請留意以下行為：

*   **自動休眠**：若服務超過 **15 分鐘**未收到任何請求，Render 將會自動讓應用程式進入「休眠」狀態。
*   **冷啟動延遲**：當您在休眠後首次造訪網頁，會觸發**冷啟動（Cold Start）**。這可能導致約 **30 - 60 秒** 的連線延遲。

---

## 功能

- 🔐 使用者註冊 / 登入
- 👤 每位使用者只能看到自己的送水紀錄
- 📷 上傳送水單據圖片
- 🤖 使用 Gemini 分析單據
- 📅 自動辨識送水日期
- 💧 自動辨識本次送水數量
- 💧 自動辨識剩餘水量
- 💾 將分析結果儲存到 PostgreSQL
- ✏️ 編輯送水紀錄
- 🗑️ 刪除送水紀錄
- 📊 Dashboard 查看歷史送水紀錄
- 📆 計算兩次送水之間相隔幾天

---

## Tech Stack

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- PostgreSQL (Neon)
- ASP.NET Identity
- Razor View
- Docker
- Google Gemini API
- Render（部署）

---

## Project Structure

```
WaterDeliveryInfo
│
├── wwwroot
├── Controllers
├── Data
├── Migrations
├── Models
├── Views
├── Services
├── Dockerfile
└── Program.cs
```

---

## Getting Started

### 1️⃣ Clone 專案
```bash
git clone https://github.com/Peter-Harker-Starling/WaterDelieveryInfo.git
cd WaterDelieveryInfo
```
### 2️⃣ 設定資料庫連線
```JSON
"ConnectionStrings": {
  "DefaultConnection": "資料庫連線字串"
},
"Gemini": {
  "ApiKey": "Google AI Studio ApiKey"
}
```
3️⃣ 套用資料庫 Migration
```bash
dotnet ef database update
```
4️⃣ 執行專案
```bash
dotnet run
```

---

## Note

透過此專案我學到：

- ASP.NET Core MVC 
- Entity Framework Core Migration
- ASP.NET Identity 
- PostgreSQL / Neon 雲端資料庫整合
- Docker 容器化部署
- Render 雲端部署
- Google Gemini 圖片辨識與資料解析
- 使用者資料隔離與授權控制
