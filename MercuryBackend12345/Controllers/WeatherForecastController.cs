using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper; // 如果沒有安裝 Dapper，請在 Package Manager Console 裡輸入: Install-Package Dapper
using Newtonsoft.Json.Linq; // 如果沒有安裝，請安裝: Install-Package Newtonsoft.Json
using Microsoft.Extensions.Configuration;

[ApiController]
[Route("api/[controller]")]
public class MyofficeController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public MyofficeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // 讀取資料
    [HttpGet("GetData")]
    public async Task<IActionResult> GetData()
    {
        using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();
            // 呼叫我們在資料庫的 usp_GetMyoffice_ACPD
            var sql = "EXEC usp_GetMyoffice_ACPD";
            var result = await conn.QueryAsync<string>(sql);

            // result 是 JSON 格式的字串
            return Ok(result);
        }
    }

    // 新增資料
    [HttpPost("Create")]
    public async Task<IActionResult> CreateData([FromBody] JObject jsonData)
    {
        using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();
            // 呼叫我們在資料庫的 usp_InsertMyoffice_ACPD
            var sql = "EXEC usp_InsertMyoffice_ACPD @JsonData";
            await conn.ExecuteAsync(sql, new { JsonData = jsonData.ToString() });

            return Ok("新增資料成功");
        }
    }

    // 更新資料
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateData([FromBody] JObject jsonData)
    {
        using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();
            // 呼叫 usp_UpdateMyoffice_ACPD
            var sql = "EXEC usp_UpdateMyoffice_ACPD @JsonData";
            await conn.ExecuteAsync(sql, new { JsonData = jsonData.ToString() });

            return Ok("更新資料成功");
        }
    }

    // 刪除資料
    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteData([FromBody] JObject jsonData)
    {
        using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();
            // 呼叫 usp_DeleteMyoffice_ACPD
            var sql = "EXEC usp_DeleteMyoffice_ACPD @JsonData";
            await conn.ExecuteAsync(sql, new { JsonData = jsonData.ToString() });

            return Ok("刪除資料成功");
        }
    }
}

