using DAL.BaseConn;
using DAL.Model;
using Dapper;
using Microsoft.Data.SqlClient;
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

        Task<IEnumerable<InfoReport>> GetInfoSummary_LewatThumbprint(int Bulan, int Tahun);
        Task<IEnumerable<InfoReportDetail>> GetInfoDetails_LewatThumbprint(int Bulan, int Tahun);

        Task<IEnumerable<InfoReport>> GetInfoSummary_TiadaLogkeluar(int Bulan, int Tahun);
        Task<IEnumerable<InfoReportDetail>> GetInfoDetails_TiadaLogkeluar(int Bulan, int Tahun);
        Task<IEnumerable<InfoReport>> GetInfoSummary_TiadaInfoMIA(int Bulan, int Tahun);
        Task<IEnumerable<InfoReportDetail>> GetInfoDetail_TiadaInfoMIA(int Bulan, int Tahun);
        Task<List<string>> GetDistinctUnitsAsync();

    }
    public class ReportRepo(ServerProd serverProd, ServerDev serverDev, ServerEHR serverEhr) : IReportRepo
    {
        private readonly ServerProd _serverProd = serverProd;
        private readonly ServerDev _serverDev = serverDev;
        private readonly ServerEHR _serverEhr = serverEhr;

        //----Kurang Jam Bekerja
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

        //-----lewat thumbprint
        public async Task<IEnumerable<InfoReport>> GetInfoSummary_LewatThumbprint(int Bulan, int Tahun)
        {
            try
            {
                string sql = @"
                        select count(employee) as KEKERAPAN, Employee, CONCAT(first_name, ' ', last_name) as NAMA, MONTH(date) as BULAN, YEAR(date) as Tahun 
                                from tblMainKehadiran
                                where 
                                    [In] > '09:00:00' and MONTH(date)= @myBulan
                                    and Year(Date) = @myTahun
                                    and Day_Type='Workday' 
                                    and tblMainKehadiran.[In] IS NOT NULL 
                                    AND tblMainKehadiran.[Out] IS NOT NULL 
                                    ---and (Remark is null or remark ='')
                                    and ( LeaveType is null or LeaveType = '')
                                    group by Employee, month(date), year(date), first_name, Last_name
                                    having count(employee) >= 3
                                    order by month(date),KEKERAPAN desc";

                return await _serverProd.Connections().QueryAsync<InfoReport>(sql, new {myBulan = Bulan,myTahun = Tahun});
            }
            catch (System.Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<IEnumerable<InfoReportDetail>> GetInfoDetails_LewatThumbprint(int Bulan, int Tahun)
        {
            try
            {
                string sql = @" WITH LateSummary AS (
                                            SELECT 
                                                Employee, 
                                                CONCAT(first_name, ' ', last_name) AS NAMA, 
                                                MONTH(date) AS BULAN, 
                                                YEAR(date) AS Tahun,
                                                COUNT(*) AS KEKERAPAN
                                            FROM tblMainKehadiran
                                            WHERE 
                                                [In] > '09:00:00'
                                                AND MONTH(date) = @myBulan
                                                AND YEAR(date) = @myTahun
                                                AND Day_Type = 'Workday'
                                                AND tblMainKehadiran.[In] IS NOT NULL 
                                                AND tblMainKehadiran.[Out] IS NOT NULL
                                                --AND (Remark IS NULL OR Remark = '') 
                                                AND (LeaveType IS NULL OR LeaveType = '')
                                            GROUP BY Employee, MONTH(date), YEAR(date), first_name, last_name
                                            HAVING COUNT(*) >= 3
                                        )
                                        SELECT 
                                            k.Employee, 
                                            CONCAT(k.first_name, ' ', k.last_name) AS NAMA, 
                                            MONTH(k.date) AS BULAN, 
                                            YEAR(k.date) AS Tahun, 
                                            k.[In], 
                                            k.[Out], 
                                            k.Remark, 
                                            k.LeaveType
                                        FROM tblMainKehadiran AS k
                                        JOIN LateSummary AS ls
                                            ON k.Employee = ls.Employee 
                                            AND MONTH(k.date) = ls.BULAN
                                            AND YEAR(k.date) = ls.Tahun
                                        WHERE 
                                            k.[In] > '09:00:00' 
                                            AND MONTH(k.date) = @myBulan
                                            AND YEAR(k.date) = @myTahun 
                                            AND Day_Type = 'Workday' 
                                            AND k.[In] IS NOT NULL 
                                            AND k.[Out] IS NOT NULL
                                            --AND (k.Remark IS NULL OR k.Remark = '') 
                                            AND (k.LeaveType IS NULL OR k.LeaveType = '')
                                        ORDER BY k.Employee, k.date; ";

                return await _serverProd.Connections().QueryAsync<InfoReportDetail>(sql, new { myBulan = Bulan, myTahun = Tahun });
            }
            catch (System.Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        //----- tiada log keluar
        public async Task<IEnumerable<InfoReport>> GetInfoSummary_TiadaLogkeluar(int Bulan, int Tahun)
        {
            try
            {
                string sql = @"select MONTH(date) as BULAN, YEAR(date) as TAHUN,Employee, CONCAT(first_name, ' ', last_name) as NAMA,  count(employee) as KEKERAPAN
                            from tblMainKehadiran
                            where 
                                tblMainKehadiran.[In] IS NOT NULL 
                                AND tblMainKehadiran.[Out] IS NULL
                                and MONTH(date) = @myBulan
                                and YEAR(date) = @myTahun
                                and Day_Type='Workday' 
                                and (Remark is null or remark ='')
                                and ( LeaveType is null or LeaveType = '')
                                group by Employee, month(date), year(date), first_name, Last_name
                                having count(employee) >= 3
                                order by EMPLOYEE, month(date),KEKERAPAN";

                return await _serverProd.Connections().QueryAsync<InfoReport>(sql, new { myBulan = Bulan, myTahun = Tahun });
            }
            catch(SystemException err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<IEnumerable<InfoReportDetail>> GetInfoDetails_TiadaLogkeluar(int Bulan, int Tahun)
        {
            try
            {
                string sql = @"SELECT 
                                k.Employee, 
                                CONCAT(k.first_name, ' ', k.last_name) AS NAMA, 
                                MONTH(k.date) AS BULAN, 
                                YEAR(k.date) AS TAHUN, 
                                k.[In], 
                                k.[Out], 
                                k.Remark, 
                                k.LeaveType
                            FROM tblMainKehadiran AS k
                            WHERE 
                                k.[In] IS NOT NULL 
                                AND k.[Out] IS NULL 
                                AND MONTH(k.date) = @myBulan
                                AND YEAR(k.date) = @myTahun
                                AND k.Day_Type = 'Workday' 
                                AND (k.Remark IS NULL OR k.Remark = '') 
                                AND (k.LeaveType IS NULL OR k.LeaveType = '')
                                AND k.Employee IN (
                                    SELECT Employee
                                    FROM tblMainKehadiran
                                    WHERE 
                                        [In] IS NOT NULL 
                                        AND [Out] IS NULL
                                        AND MONTH(date) = @myBulan
                                        AND YEAR(date) = @myTahun
                                        AND Day_Type = 'Workday' 
                                        AND (Remark IS NULL OR Remark = '') 
                                        AND (LeaveType IS NULL OR LeaveType = '')
                                    GROUP BY Employee, MONTH(date), YEAR(date)
                                    HAVING COUNT(Employee) >= 3
                                )
                            ORDER BY BULAN DESC, k.Employee, k.date
                        ";

                return await _serverProd.Connections().QueryAsync<InfoReportDetail>(sql, new { myBulan = Bulan, myTahun = Tahun });
            }
            catch (SystemException err)
            {
                throw new Exception(err.Message);
            }
        }

        //---- miA

        public async Task<IEnumerable<InfoReport>> GetInfoSummary_TiadaInfoMIA(int Bulan, int Tahun)
        {
            try
            {
                string sql = @"
                                select  MONTH(date) as BULAN, YEAR(date) as TAHUN,Employee, CONCAT(first_name, ' ', last_name) as NAMA, count(employee) as KEKERAPAN
                                        from tblMainKehadiran
                                        where LeaveType='Absent' 
                                            and MONTH(date) =  @myBulan
                                            and YEAR(date) = @myTahun
                                            and Day_Type='Workday' 
                                            and (Remark is null or remark ='')
                                            group by Employee, month(date), year(date), first_name, Last_name
                                            having count(employee) >= 3
                                            order by month(date),KEKERAPAN desc";
                return await _serverProd.Connections().QueryAsync<InfoReport>(sql, new { myBulan = Bulan, myTahun = Tahun });
            }
            catch(System.Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<IEnumerable<InfoReportDetail>> GetInfoDetail_TiadaInfoMIA(int Bulan, int Tahun)
        {
            try
            {
                string sql = @"SELECT 
	                                k.date,
                                    k.Employee, 
                                    CONCAT(k.first_name, ' ', k.last_name) AS NAMA, 
                                    MONTH(k.date) AS BULAN, 
                                    YEAR(k.date) AS TAHUN, 
                                    k.[In], 
                                    k.[Out], 
                                    k.Remark, 
                                    k.LeaveType
                                FROM tblMainKehadiran AS k
                                WHERE 
                                    k.LeaveType = 'Absent' 
                                    AND MONTH(k.date) = @myBulan
                                    AND YEAR(k.date) = @myTahun
                                    AND k.Day_Type = 'Workday' 
                                    AND (k.Remark IS NULL OR k.Remark = '') 
                                    AND k.Employee IN (
                                        SELECT Employee
                                        FROM tblMainKehadiran
                                        WHERE 
                                            LeaveType = 'Absent' 
                                            AND MONTH(date) = @myBulan
                                            AND YEAR(date) = @myTahun
                                            AND Day_Type = 'Workday' 
                                            AND (Remark IS NULL OR Remark = '')
                                        GROUP BY Employee, MONTH(date), YEAR(date)
                                        HAVING COUNT(Employee) >= 3
                                    )
                                ORDER BY BULAN DESC, k.Employee, k.date";
                return await _serverProd.Connections().QueryAsync<InfoReportDetail>(sql, new { myBulan = Bulan, myTahun = Tahun });
            }
            catch (System.Exception err)
            {
                throw new Exception(err.Message);
            }
        }


        public async Task<List<string>> GetDistinctUnitsAsync()
        {
           const string query = @"SELECT DISTINCT(unit) as Unit FROM view_butiran_staf ORDER BY unit ASC";

            var units = await _serverEhr.Connections().QueryAsync<string>(query);
            return units.ToList();
        }



    }
}
