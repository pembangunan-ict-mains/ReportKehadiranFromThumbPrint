using DAL.BaseConn;
using DAL.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DAL.Repo
{
    public interface IRepoData
    {
        Task<bool> GetStatusRecordAsync(int bulan, int tahun);
        Task<IEnumerable<InfoDashboard>> GetInfoForDashboard();
        Task<int> KiraJumlah(); Task<tblMainKehadiran?> GetLatestRecord();
        Task<IEnumerable<view_Butiran_Staf>> GetInfoUser(string userid);
        Task<IEnumerable<InfoDashboard>> GetReportForChart();
        Task<IEnumerable<tblInfoUserReport>> GetloginInfo(string nostaff);
        Task InsertAuditLogAsync(string noStaf, string eventDescription);
    }
    public class RepoData(ServerProd serverProd, ServerDev serverDev, ServerEHR serverEhr) : IRepoData
    {
        private readonly ServerProd _serverProd = serverProd;
        private readonly ServerDev _serverDev = serverDev;
        private readonly ServerEHR _serverEhr = serverEhr;

        public async Task<bool> GetStatusRecordAsync(int bulan, int tahun)
        {
            string sql = @"SELECT COUNT(*) FROM tblMainKehadiran WHERE MONTH([Date]) = @bulan AND YEAR([Date]) = @tahun";

            int count = await _serverProd.Connections().QuerySingleAsync<int>(sql, new { bulan, tahun });

            return count > 0;
        }

        public async Task<IEnumerable<InfoDashboard>> GetReportForChart()
        {
            string sql = @"
                            SELECT MONTH([date]) AS Month, COUNT(*) AS AttendanceCount
                            FROM tblMainKehadiran
                             WHERE YEAR([date]) = YEAR(GETDATE())
                            GROUP BY MONTH([date])";
            try
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var result = await _serverProd.Connections().QueryAsync<InfoDashboard>(sql);
                sw.Stop();
                Console.WriteLine($"DB Query took {sw.ElapsedMilliseconds} ms");
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); return new List<InfoDashboard>();
            }
        }

        public async Task<IEnumerable<InfoDashboard>> GetInfoForDashboard()
        {
            string sql = @"
                            SELECT 
                            m.MonthNumber AS [Month],
                            DATENAME(MONTH, DATEFROMPARTS(YEAR(GETDATE()), m.MonthNumber, 1)) AS [MonthName],
                            ISNULL(a.AttendanceCount, 0) AS [AttendanceCount]
                        FROM (
                            VALUES (1), (2), (3), (4), (5), (6), (7), (8), (9), (10), (11), (12)
                        ) AS m(MonthNumber)
                        LEFT JOIN (
                            SELECT MONTH([date]) AS Month, COUNT(*) AS AttendanceCount
                            FROM tblMainKehadiran
                             WHERE YEAR([date]) = YEAR(GETDATE())
                            GROUP BY MONTH([date])
                        ) AS a
                        ON m.MonthNumber = a.Month
                        ORDER BY m.MonthNumber";
            try
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var result = await _serverProd.Connections().QueryAsync<InfoDashboard>(sql);
                sw.Stop();
                Console.WriteLine($"DB Query took {sw.ElapsedMilliseconds} ms");
                return result;
              
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message); return new List<InfoDashboard>();
                // throw new Exception("Error fetching dashboard info", ex);
            }
        }


        public async Task<int> KiraJumlah()
        {           
            try
            {
               
                string sql = @"select top 1 * from tblmainkehadiran";
                var res = await _serverProd.Connections().ExecuteScalarAsync<int>(sql);
                return res;
            }
            catch (SqlException sx)
            {
                Console.WriteLine($"Sql exp: {sx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception("Error in KiraJumlah", ex);
            }

            return 0;
        }


        public async Task<tblMainKehadiran?> GetLatestRecord()
        {
            try
            {
                const string sql = @"SELECT TOP 1 date,first_name FROM tblMainKehadiran order by [date] asc";
                            

                var record = await _serverProd.Connections()
                                 .QueryFirstOrDefaultAsync<tblMainKehadiran>(sql);

                return record;
            }
            catch (SqlException sx)
            {
                Console.WriteLine($"Sql exp: {sx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetLatestRecord", ex);
            }
        }


        public async Task<IEnumerable<view_Butiran_Staf>> GetInfoUser(string userid)
        {
            try
            {
                string sql = @"select * from view_butiran_staf where nostafbaca = @nostafbaca";
                return await _serverEhr.Connections().QueryAsync<view_Butiran_Staf>(sql, new { nostafbaca = userid });
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error in GetInfoUser: " + ex.Message);
            }
        }

        public async Task<IEnumerable<tblInfoUserReport>> GetloginInfo(string nostaff)
        {
            try
            {
                string sql = @"select * from tblinfouserreport where nostaf=@nostaf";
                return await _serverProd.Connections().QueryAsync<tblInfoUserReport>(sql, new {nostaf = nostaff});
            }
            catch (SqlException err)
            {
                Console.WriteLine(err.Message);
                throw new Exception("Error in GetInfoUser: " + err.Message);
            }
        }


        public async Task InsertAuditLogAsync(string noStaf, string eventDescription)
        {
            const string sql = @"
                                INSERT INTO tblInfoAuditLog (CreatedDate, NoStaf, Event)
                                VALUES (@CreatedDate, @NoStaf, @Event)";

            var parameters = new
            {
                CreatedDate = DateTime.Now,
                NoStaf = noStaf ?? string.Empty,
                Event = eventDescription ?? string.Empty
            };

            using var conn = _serverProd.Connections();
            await conn.ExecuteAsync(sql, parameters);
        }


        //-------------------------
        string newString;
        public async Task CleanDatabase1()
        {
            using var conn = _serverProd.Connections();
            string sql = @"select distinct(last_name), user_id 
                           from 
                           tblmainkehadiran 
                           where (last_name like 'B %' or last_name like 'B. %')";
            try
            {
                var _result = await conn.QueryAsync<CleanDB>(sql);
                if (_result.Count() > 0)
                {
                    foreach (var xx in _result)
                    {

                        //string val = xx.last_name.Trim().Replace(".", "");
                        //string newString = val.StartsWith("BT") ? val.Replace("BT", "BINTI") : val.Replace("B", "BIN");
                        string val = xx.last_name.Trim().Replace(".", "");
                        if (val.StartsWith("B") || val.StartsWith("B."))
                        {
                            newString = val.Substring(1).StartsWith("T") ? "BINTI" + val.Substring(2) : "BIN" + val.Substring(1);
                        }
                        else
                        {
                            newString = val;
                        }

                        //---sql to update
                        string sql2 = @"update tblmainkehadiran set last_name = @last where user_id=@uid";
                        var _res2 = conn.Execute(sql2, new { last = newString, uid = xx.User_Id });
                    }
                }
            }
            catch (System.Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task CleanDatabase2()
        {
            using var conn = _serverProd.Connections();
            string sql = @"select distinct(last_name), user_id 
                           from 
                           tblmainkehadiran 
                           where (last_name like 'BT %' or last_name like 'BT. %')";
            try
            {
                var _result = await conn.QueryAsync<CleanDB>(sql);
                if (_result.Count() > 0)
                {
                    foreach (var xx in _result)
                    {

                        //string val = xx.last_name.Trim().Replace(".", "");
                        //string newString = val.StartsWith("BT") ? val.Replace("BT", "BINTI") : val.Replace("B", "BIN");
                        string val = xx.last_name.Trim().Replace(".", "");
                        if (val.StartsWith("BT") || val.StartsWith("BT."))
                        {
                            newString = val.Substring(1).StartsWith("T") ? "BINTI" + val.Substring(2) : "BIN" + val.Substring(1);
                        }
                        else
                        {
                            newString = val;
                        }

                        //---sql to update
                        string sql2 = @"update tblmainkehadiran set last_name = @last where user_id=@uid";
                        var _res2 = conn.Execute(sql2, new { last = newString, uid = xx.User_Id });
                    }
                }
            }
            catch (System.Exception err)
            {
                throw new Exception(err.Message);
            }
        }


        public async Task CleanDatabase_NOMA()
        {
            using var conn = _serverProd.Connections();
            string sql = @"select distinct(trim(a.First_Name)) + ' ' + trim(a.Last_Name) as Nama, b.NoMa, a.User_id
                                    from tblMainKehadiran a
                                    inner join [tblInfoStaff] b on 
                                    trim(a.first_name) +  ' ' + trim(a.Last_Name) = b.Nama";
            try
            {
                var _res = await conn.QueryAsync<DataNOMA>(sql);

                if (_res.Count() > 0)
                {
                    foreach (var xx in _res)
                    {
                        string sql2 = @"update tblmainkehadiran set Employee = @emp where user_id = @user_id ";
                        var _res2 = conn.Execute(sql2, new { emp = xx.NoMa, user_id = xx.user_id });
                    }
                }
            }
            catch (System.Exception err)
            {
                throw new Exception(err.Message);
            }

        }

        public async Task CleanDatabasePBNo()
        {
            using var conn = _serverProd.Connections();
            string sql = @"SELECT distinct(trim(b.first_Name)) + ' ' + trim(b.last_name) as NAMA, b.Employee, b.User_Id
                                  FROM tblMainKehadiran b
                                  where Employee like 'PB%' order by NAMA asc";
            try
            {
                var _result = await conn.QueryAsync<DataPB>(sql);
                if (_result != null)
                {
                    foreach (var xx in _result)
                    {
                        string sql1 = @"select NoMA from tblRujukanRekod where user_id = @nama";
                        var _result2 = await conn.QueryFirstOrDefaultAsync<string>(sql1, new { nama = xx.User_Id });
                        if (_result2 != null)
                        {
                            string sql3 = @"update tblmainkehadiran set employee = @employee where user_id = @userid ";
                            var _result3 = conn.Execute(sql3, new { employee = _result2.ToString(), userid = xx.User_Id });
                        }
                    }
                }
            }
            catch (System.Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task CleanDatabasePBNo2()
        {
            using var conn = _serverProd.Connections();
            string sql = @"SELECT distinct(trim(b.first_Name)) + ' ' + trim(b.last_name) as NAMA, b.Employee, b.User_Id
                                  FROM tblMainKehadiran b
                                  where Employee like 'PB%' order by NAMA asc";
            try
            {
                var _result = await conn.QueryAsync<DataPB>(sql);
                if (_result != null)
                {
                    foreach (var xx in _result)
                    {
                        var noma = xx.Employee.Substring(2);

                        string sql3 = @"update tblmainkehadiran set employee = @employee where user_id = @userid ";
                        var _result3 = conn.Execute(sql3, new { employee = "MA" + noma.ToString(), userid = xx.User_Id });
                    }
                }
            }
            catch (System.Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public async Task CleanDatabaseNoStaff()
        {
            using var conn = _serverProd.Connections();
            string sql = @"select user_id, noma from tblrujukanrekod order by user_id asc";

            try
            {
                var _result = await conn.QueryAsync<RekodRujukan>(sql);
                if (_result != null)
                {
                    ;
                    foreach (var item in _result)
                    {
                        string sql1 = @"update tblMainKehadiran set employee = @employee where user_id = @user_id";
                        var _update = conn.Execute(sql1, new { employee = item.NoMa, user_id = item.User_Id });
                    }
                }
            }
            catch (System.Exception err)
            {
                throw new Exception(err.Message);
            }
        }



    }
}
