using Google.GenAI;
using Google.GenAI.Types;
using water.Models;


namespace water.Services
{
    public class WaterDeliveryService
    {
        private readonly Client _client;

        public WaterDeliveryService(IConfiguration configuration)
        {
            var apiKey = configuration["Gemini:ApiKey"] ?? throw new InvalidOperationException("找不到給咪咪的 APIKey");
            this._client = new Client(apiKey: apiKey);
        }
        public async Task<WaterDeliveryInfo> AnalyzeImageAsync(byte[] imageBytes)
        {
            var imagePart = new Part
            {
                InlineData = new Blob
                {
                    MimeType = "image/jpeg",
                    Data = imageBytes
                }
            };

            var textPart = new Part
            {
                Text = @"請讀取這張訂水單中的資訊，並以 JSON 格式回答，不要有額外的說明文字。
                格式如下：
                {
                  ""Product_Id"": ""字串"",
                  ""Product_Name"": ""字串"",
                  ""DeliveryDate"": ""YYYY-MM-DD"",
                  ""Quantity"": 數字,
                  ""RemainingQuantity"": 數字,
                  ""Sheet_Id"": ""字串""
                }"
            };

            var content = new Content
            {
                Parts = new List<Part> { imagePart, textPart }
            };

            var config = new GenerateContentConfig
            {
                ResponseMimeType = "application/json",
                ResponseSchema = new Schema
                {
                    Type = Google.GenAI.Types.Type.Object,
                    Properties = new Dictionary<string, Schema>
                    {
                        ["Product_Id"] = new Schema { Type = Google.GenAI.Types.Type.String, Description = "商品代號" },
                        ["Product_Name"] = new Schema { Type = Google.GenAI.Types.Type.String, Description = "品名" },
                        ["DeliveryDate"] = new Schema { Type = Google.GenAI.Types.Type.String, Description = "送水日期，格式 YYYY-MM-DD" },
                        ["Quantity"] = new Schema { Type = Google.GenAI.Types.Type.Integer, Description = "數量" },
                        ["RemainingQuantity"] = new Schema { Type = Google.GenAI.Types.Type.Integer, Description = "剩餘桶數" },
                        ["Sheet_Id"] = new Schema { Type = Google.GenAI.Types.Type.String, Description = "送水單號" }
                    },
                    Required = new List<string> { "Product_Id", "Product_Name", "DeliveryDate", "Quantity", "RemainingQuantity", "Sheet_Id" }
                }
            };

            var response = await _client.Models.GenerateContentAsync(
                model: "gemini-3.1-flash-lite",
                contents: new List<Content> { content },
                config: config
            );

            var info = System.Text.Json.JsonSerializer.Deserialize<WaterDeliveryInfo>(response.Text!);

            return info!;
        }
    }
}
