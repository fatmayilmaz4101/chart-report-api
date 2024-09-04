using chart_report_api.BusinessLogic.Entity;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace chart_report_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DbConnectionController : ControllerBase
    {
        [HttpPost("connect")]
        public async Task<IActionResult> Post([FromBody] DbConnectionEntity request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest("Invalid input");
            var connectionString = $"Host=localhost;Port=5433;Username={request.Username};Password={request.Password};Database={request.Database}";
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    // Bağlantı başarılı olduysa diğer işlemlere geç
                    if (connectionString is not null)
                        return Ok(connectionString); // Başarılı bir sonuç döner
                    else
                        return BadRequest("Failed to connect to the database");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
