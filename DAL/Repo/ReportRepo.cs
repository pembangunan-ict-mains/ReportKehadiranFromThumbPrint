using DAL.BaseConn;
using DAL.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public interface IReportRepo
    {
        Task<IEnumerable<InfoReport>> GetInfoSummary_KurangJamBekerja(int Bulan, int Tahun);
        Task<IEnumerable<InfoReportDetail>> GetInfoDetails_KurangJamBekerja(int Bulan, int Tahun);
    }
    public class ReportRepo(ServerProd serverProd, ServerDev serverDev, ServerEHR serverEhr) : IReportRepo
    {
        private readonly ServerProd _serverProd = serverProd;
        private readonly ServerDev _serverDev = serverDev;
        private readonly ServerEHR _serverEhr = serverEhr;

        public async Task<IEnumerable<InfoReport>> GetInfoSummary_KurangJamBekerja(int Bulan, int Tahun)
        {
            try
            {
                string sql = @"SELECT MONTH(Date) AS BULAN, 
                                YEAR(Date) AS TAHUN,    
                                Employee, 
                                CONCAT(First_Name, ' ', Last_Name) AS NAMA,
	                            COUNT(Employee) AS KEKERAPAN    
                            FROM tblMainKehadiran
                            WHERE 
                                Work < 8.59
                                AND [In] IS NOT NULL 
                                AND [Out] IS NOT NULL
                                AND MONTH(Date) = @myBulan
                                AND YEAR(Date) = @myTahun
                                AND Day_Type = 'Workday' 
                                AND (LeaveType IS NULL OR LeaveType = '')
                                AND TRY_CAST(Leave AS DECIMAL(10,2)) = 0.00
                            GROUP BY Employee, MONTH(Date), YEAR(Date), First_Name, Last_Name
                            HAVING COUNT(Employee) >= 3
                            ORDER BY BULAN, KEKERAPAN DESC";
                return await _serverProd.Connections().QueryAsync<InfoReport>(sql, new {myBulan = Bulan,myTahun = Tahun });
            }
            catch(System.Exception err)
            {
                throw new Exception(err.Message);
            }           
        }

        public async Task<IEnumerable<InfoReportDetail>> GetInfoDetails_KurangJamBekerja(int Bulan, int Tahun)
        {
            try
            {
                string sql = @"SELECT 
                                k.Employee, 
                                CONCAT(k.First_Name, ' ', k.Last_Name) AS NAMA, 
                                MONTH(k.Date) AS BULAN, 
                                YEAR(k.Date) AS TAHUN, 
                                k.[In], 
                                k.[Out], 
                                k.Remark, 
                                k.LeaveType, 
                                k.Leave, 
                                k.Work
                            FROM tblMainKehadiran AS k
                            JOIN (
                                SELECT Employee, MONTH(Date) AS BULAN, YEAR(Date) AS TAHUN
                                FROM tblMainKehadiran
                                WHERE 
                                    Work < 8.59
                                    AND MONTH(Date) = @myBulan
                                    AND YEAR(Date) = @myTahun
                                    AND Day_Type = 'Workday'
                                    AND [In] IS NOT NULL 
                                    AND [Out] IS NOT NULL
                                    AND (LeaveType IS NULL OR LeaveType = '') 
                                    AND TRY_CAST(Leave AS DECIMAL(10,2)) = 0.00
                                GROUP BY Employee, MONTH(Date), YEAR(Date)
                                HAVING COUNT(Employee) >= 3
                            ) AS filtered ON k.Employee = filtered.Employee 
                                AND MONTH(k.Date) = filtered.BULAN 
                                AND YEAR(k.Date) = filtered.TAHUN
                            WHERE 
                                k.Work < 8.59 
                                AND k.Day_Type = 'Workday' 
                                AND k.[In] IS NOT NULL 
                                AND k.[Out] IS NOT NULL 
                                AND (k.LeaveType IS NULL OR k.LeaveType = '')
                                AND TRY_CAST(k.Leave AS DECIMAL(10,2)) = 0.00
                            ORDER BY BULAN, k.Employee, k.Date";
                return await _serverProd.Connections().QueryAsync<InfoReportDetail>(sql, new { myBulan = Bulan, myTahun = Tahun });
            }
            catch (System.Exception err)
            {
                throw new Exception(err.Message);
            }
        }

    }
}
