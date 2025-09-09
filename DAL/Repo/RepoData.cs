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




    }
}
