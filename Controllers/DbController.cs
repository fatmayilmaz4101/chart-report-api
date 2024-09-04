using chart_report_api.BusinessLogic.Entity;
using chart_report_api.Helper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace chart_report_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DbController : ControllerBase
    {

        [HttpGet("views")]
        public async Task<IActionResult> Get(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null.");
            }
            var viewColumnInfos = new List<ViewColumnInfo>();
            using (var connection = new NpgsqlConnection(connectionString.ToString()))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(Queries.ViewAndColumnNames, connection))
                {

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var viewColumnInfo = new ViewColumnInfo
                            {
                                View = reader["view_name"].ToString(),
                                Columns = reader["columns"] as string[] ?? new string[0]
                            };
                            viewColumnInfos.Add(viewColumnInfo);
                        }
                    }
                }
            }
            if (viewColumnInfos.Count == 0)
            {
                return NotFound("No views found in the database.");
            }

            return Ok(viewColumnInfos);
        }
        [HttpGet("view-data")]
        public async Task<IActionResult> Get(
            [FromQuery] string connectionString,
            [FromQuery] string viewName,
            [FromQuery] string[] yColumns,
            [FromQuery] string xColumn)
        {
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(viewName) || xColumn == null || yColumns.Length == 0)
            {
                return BadRequest("Invalid parameters.");
            }


            var columnsList = string.Join(",", yColumns.Select(y => $"{y} as y_{y}"));
            var query = $"SELECT {columnsList}, {xColumn} as x_{xColumn} FROM {viewName}";

            var viewDatas = new ViewXAndYColumnsInfo
            {
                XColumn =
                    new XColumn
                    {
                        Name = xColumn,
                        Values = new List<string>(),
                    },
                YColumn = yColumns.Select(y => new YColumns { Column = new List<YColumn>() { new YColumn { Name = y, Values = new List<string>() } } })
                .ToList()
            };

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var xCol = viewDatas.XColumn;
                            xCol.Values.Add(reader[xCol.NameAlias].ToString());

                            viewDatas.YColumn.ForEach((yCol) =>
                            {
                                yCol.Column.ForEach(yCol2 =>
                                {
                                    yCol2.Values.Add(reader[yCol2.NameAlias].ToString());
                                });
                            });
                        }
                    }
                }
            }
            return Ok(viewDatas);
        }
        [HttpGet("functions")]

        public async Task<IActionResult> GetFunctions(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null.");
            }
            var funcColumnInfos = new List<FuncColumnInfo>();

            using (var connection = new NpgsqlConnection(connectionString.ToString()))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(Queries.FuncAndColumnNames, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var funcColumnInfo = new FuncColumnInfo
                            {
                                Func = reader["function_name"].ToString(),
                                Columns = reader["columns"] as string[] ?? new string[0]
                            };
                            funcColumnInfos.Add(funcColumnInfo);
                        }
                    }
                }
            }
            if (funcColumnInfos.Count == 0)
            {
                return NotFound("No function found in the database.");
            }
            return Ok(funcColumnInfos);
        }

        [HttpGet("func-data")]
        public async Task<IActionResult> GetFuncData(
        [FromQuery] string connectionString,
        [FromQuery] string funcName,
        [FromQuery] string[] yColumns,
        [FromQuery] string xColumn)
        {
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(funcName) || xColumn == null || yColumns.Length == 0)
            {
                return BadRequest("Invalid parameters.");
            }


            var columnsList = string.Join(",", yColumns.Select(y => $"{y} as y_{y}"));
            var query = $"SELECT {columnsList}, {xColumn} as x_{xColumn} FROM {funcName}()";

            var funcDatas = new ViewXAndYColumnsInfo
            {
                XColumn =
                    new XColumn
                    {
                        Name = xColumn,
                        Values = new List<string>(),
                    },
                YColumn = yColumns.Select(y => new YColumns { Column = new List<YColumn>() { new YColumn { Name = y, Values = new List<string>() } } })
                .ToList()
            };

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var xCol = funcDatas.XColumn;
                            xCol.Values.Add(reader[xCol.NameAlias].ToString());

                            funcDatas.YColumn.ForEach((yCol) =>
                            {
                                yCol.Column.ForEach(yCol2 =>
                                {
                                    yCol2.Values.Add(reader[yCol2.NameAlias].ToString());
                                });
                            });
                        }
                    }
                }
            }
            return Ok(funcDatas);
        }


    }
}