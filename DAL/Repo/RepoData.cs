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

        //for migrate
        Task CleanDatabase1();
        Task CleanDatabase2();
        Task CleanDatabase_NOMA();
        Task CleanDatabasePBNo();
        Task CleanDatabasePBNo2();
        Task CleanDatabaseNoStaff();
        bool AddLog(LogEntry log);
        Task<bool> CheckDuplicateResult(string UID, string Employee, string xDate);
        Task<bool> InsertIntoDBFinal_BULK(DataTable dt); Task CrossCheckUpdateRekod();
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
        string newString = string.Empty;

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

        public bool AddLog(LogEntry log)
        {
            using var conn = _serverProd.Connections();
            string sql = @"insert into tbllogger(Date, Logs, Modules) values (@date, @logs, @modules)";
            try
            {
                int rec = conn.Execute(sql, log);
                return rec > 0;
            }
            catch (System.Exception e)
            {
                throw new Exception($"{e.Message}");
            }
        }

        public async Task<bool> CheckDuplicateResult(string UID, string Employee, string xDate)
        {
            using var conn = _serverProd.Connections();
            string sql = @"select count(*) from tblmainkehadiran where
                            date = @date and employee = @employee and user_id = @userid";

            try
            {
                var count = await conn.ExecuteScalarAsync<int>(sql, new { date = xDate, employee = Employee, userid = UID });
                return count > 0;
            }
            catch (System.Exception ex)
            {
                return false;
                throw new Exception(sql, ex);
            }
        }

        //public async Task<bool> InsertIntoDBFinal_BULK(DataTable dt)
        //{
        //    var connstr = _serverProd.Connections();
        //    var conn = new SqlConnection(connstr);
        //    conn.Open();
        //    var trans = conn.BeginTransaction();

        //    try
        //    {
        //        using var copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans);
        //        copy.DestinationTableName = "tblMainKehadiran";

        //        await copy.WriteToServerAsync(dt);
        //        trans.Commit();

        //        Console.WriteLine($"Saved record: {dt.Rows.Count}");
        //        Console.WriteLine("Data has been saved to database!");

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine("Failed to save data to database!");
        //        throw new Exception($"Error save to database! {ex.Message}");
        //    }
        //}
        //public async Task<bool> InsertIntoDBFinal_BULK(DataTable dt)
        //{
        //    // await using var conn = new SqlConnection(_serverProd.Connections());
        //    string connStr = _serverProd.Connections().ToString();
        //    await using var conn = new SqlConnection(connStr);
        //    await conn.OpenAsync().ConfigureAwait(false);
        //    await using var trans = await conn.BeginTransactionAsync().ConfigureAwait(false);

        //    try
        //    {
        //        using var copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, (SqlTransaction)trans)
        //        {
        //            DestinationTableName = "tblMainKehadiran"
        //        };

        //        await copy.WriteToServerAsync(dt).ConfigureAwait(false);

        //        await trans.CommitAsync().ConfigureAwait(false);

        //        Console.WriteLine($"Saved record: {dt.Rows.Count}");
        //        Console.WriteLine("Data has been saved to database!");

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        await trans.RollbackAsync().ConfigureAwait(false);
        //        Console.WriteLine("Failed to save data to database!");
        //        throw new Exception($"Error save to database! {ex.Message}", ex);
        //    }
        //}

        //        public async Task<bool> InsertIntoDBFinal_BULK(DataTable dt)
        //{
        //    // If Connections() already returns string, no need for .ToString()
        //        var connStr = _serverProd.Connections();  

        //    await using var conn = new SqlConnection(connStr);
        //    await conn.OpenAsync().ConfigureAwait(false);
        //    await using var trans = await conn.BeginTransactionAsync().ConfigureAwait(false);

        //    try
        //    {
        //        using var copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, (SqlTransaction)trans)
        //        {
        //            DestinationTableName = "tblMainKehadiran"
        //        };

        //        await copy.WriteToServerAsync(dt).ConfigureAwait(false);

        //        await trans.CommitAsync().ConfigureAwait(false);

        //        Console.WriteLine($"Saved record: {dt.Rows.Count}");
        //        Console.WriteLine("Data has been saved to database!");

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        await trans.RollbackAsync().ConfigureAwait(false);
        //        Console.WriteLine("Failed to save data to database!");
        //        throw new Exception($"Error save to database! {ex.Message}", ex);
        //    }
        //}

        public async Task<bool> InsertIntoDBFinal_BULK(DataTable dt)
        {
            // If Connections() already returns string, no need for .ToString()
            string connStr = _serverProd.Connections().ConnectionString;

            await using var conn = new SqlConnection(connStr);
            await conn.OpenAsync().ConfigureAwait(false);
            await using var trans = await conn.BeginTransactionAsync().ConfigureAwait(false);

            try
            {
                using var copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, (SqlTransaction)trans)
                {
                    DestinationTableName = "tblMainKehadiran"
                };

                await copy.WriteToServerAsync(dt).ConfigureAwait(false);

                await trans.CommitAsync().ConfigureAwait(false);

                Console.WriteLine($"Saved record: {dt.Rows.Count}");
                Console.WriteLine("Data has been saved to database!");

                return true;
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync().ConfigureAwait(false);
                Console.WriteLine("Failed to save data to database!");
                throw new Exception($"Error save to database! {ex.Message}", ex);
            }
        }


        string? mDate;
        string? myDate, myId, myIdd;
        int ii = 0;
        string? StartT = string.Empty; string xx1 = string.Empty;
        string? EndT, xx2 = string.Empty;
        DateTime startTime;
        string sql2 = string.Empty;
        string? reason, JENISCUTI = string.Empty;
        int typeLeave, checker, count;

        public async Task CrossCheckUpdateRekod()
        {
            using var conn = _serverProd.Connections();
            string sql = @"SELECT tmk.[Date] , tmk.Employee, tmk.LeaveType, mu.id
                                from tblMainKehadiran tmk 
                                inner join ehr.dbo.main_users mu on REPLACE(mu.employeeId, '0000', '') COLLATE SQL_Latin1_General_CP1_CI_AS = tmk.Employee
                                where (tmk.LeaveType='Absent')";

            //where(tmk.LeaveType is not NULL or tmk.LeaveType = 'Absent')";
            try
            {
                var result = await conn.QueryAsync<Data>(sql);
                startTime = DateTime.Now;
                foreach (var _res in result)
                {

                    ii = ii + 1;

                    mDate = Convert.ToDateTime(_res.Date.ToString()).ToString("yyyy-MM-dd");
                    myId = _res.Employee;
                    myIdd = _res.Id;

                    //enhance kod
                    sql2 = @"select type_leave, reason, checker, count from ehr.dbo.main_leaves 
                                     where user_id=@user_id 
                                     and 
                                     @ddate between from_date and to_date";

                    var result2 = await conn.QueryAsync(sql2, new { ddate = mDate, user_id = myIdd });

                    if (result2.Count() > 0 || result2 != null)
                    {

                        foreach (var row in result2)
                        {
                            typeLeave = row.type_leave;
                            reason = row.reason;
                            checker = row.checker;
                            count = row.count;


                            switch (typeLeave)
                            {
                                case 1:
                                    JENISCUTI = "Cuti Tahunan / Rehat (Klausa 2.2.) ";
                                    break;
                                case 2:
                                    JENISCUTI = "Cuti Kecemasan - Hanya 1 Hari Setiap Kali Permohonan (Klausa 2.7.) ";
                                    break;
                                case 3:
                                    JENISCUTI = "Cuti Sakit - Biasa (Pesakit Luar) Di Klinik Swasta (Klausa 3.1.)";
                                    break;
                                case 5:
                                    JENISCUTI = "Cuti Bencana - Menyebabkan Terhalangnya Perjalanan Ke Pejabat/Tempat Kerja Mengikut Tempoh Sebenar Bencana (Klausa 6.7.)";
                                    break;
                                case 6:
                                    JENISCUTI = "Cuti Bagi Urusan Kematian Keluarga Terdekat (Klausa 6.9.)";
                                    break;
                                case 7:
                                    JENISCUTI = "Cuti Ihsan (Klausa 14.6-)";
                                    break;
                                case 8:
                                    JENISCUTI = "Cuti Bersalin (Klausa 3.10.)";
                                    break;
                                case 9:
                                    JENISCUTI = "Cuti Tanpa Gaji - Sebab Urusan Peribadi Yang Mustahak (Klausa 2.9.)";
                                    break;
                                case 10:
                                    JENISCUTI = "Cuti Menghadiri Peperiksaan - Hari Yang Diperlukan Bagi Peperiksaan Sahaja (Klausa 6.4.)";
                                    break;
                                case 12:
                                    JENISCUTI = "Cuti Khas Bergaji Penuh (Klausa 14.8-)";
                                    break;
                                case 13:
                                    JENISCUTI = "Cuti Haji - Telah Berkhidmat Tidak Kurang Daripada 3 Tahun (Klausa 4.1.)";
                                    break;
                                case 14:
                                    JENISCUTI = "Cuti Gantian - Hendaklah Diambil Dalam Tempoh 6 Bulan Daripada Tarikh Kerja Lebih Masa (Klausa 5.1.)";
                                    break;
                                case 15:
                                    JENISCUTI = "Cuti Kuarantin - Pegawai Yang Dalam Perjalanan Balik Ke Malaysia Dari Luar Negara (Klausa 3.9.)";
                                    break;
                                case 16:
                                    JENISCUTI = "Cuti Tanpa Rekod - Bertugas Untuk Pilihanraya Umum (Klausa 14.13-)";
                                    break;
                                case 17:
                                    JENISCUTI = "Cuti Kerantina - Kerana Berlakunya Wabak Penyakit Merebak (Covid-19) (Klausa 14.12-)";
                                    break;
                                case 18:
                                    JENISCUTI = "Cuti Rehat Di Luar Negara (Klausa 2.3.)";
                                    break;
                                case 19:
                                    JENISCUTI = "Cuti Rehat Pegawai Yang Akan Bersara (Klausa 2.4.)";
                                    break;
                                case 20:
                                    JENISCUTI = "Cuti Separuh Gaji - Sebab-sebab Kesihatan Keluarga Terdekat Bagi Tujuan Penjagaan (Klausa 2.8.)";
                                    break;
                                case 21:
                                    JENISCUTI = "Cuti Sakit - Biasa/Warded (Pesakit Luar/Dalam) Daripada Hospital/Klinik Kerajaan (Klausa 3.1.) ";
                                    break;
                                case 22:
                                    JENISCUTI = "Cuti Sakit Lanjutan - Cuti Separuh Gaji (Klausa 3.7.)";
                                    break;
                                case 24:
                                    JENISCUTI = "Cuti Isteri Bersalin - 5 Kali Sepanjang Tempoh Perkhidmatan (Klausa 3.11.)";
                                    break;
                                case 25:
                                    JENISCUTI = "Cuti Menjaga Anak (Cuti Tanpa Gaji - Bersambung Dengan Cuti Bersalin) (Klausa 3.12.)";
                                    break;
                                case 26:
                                    JENISCUTI = "Cuti Menjaga Anak Yang Dikuarantin Atau Memerlukan Pengasingan (Klausa 3.13.)";
                                    break;
                                case 27:
                                    JENISCUTI = "Cuti Kusta Atau Barah (Klausa 3.15.)";
                                    break;
                                case 28:
                                    JENISCUTI = "Cuti Umrah - 1 Kali Sepanjang Tempoh Perkhidmatan (Klausa 4.2.)";
                                    break;
                                case 29:
                                    JENISCUTI = "Cuti Menjaga Anak (Cuti Tanpa Gaji - Tidak Bersambung Dengan Cuti Bersalin) (Klausa 3.12.)";
                                    break;
                                case 30:
                                    JENISCUTI = "Cuti Kecederaan (Cuti Sakit Lanjutan - Semasa Menjalankan Tugas-tugas Rasminya) (Klausa 3.14.)";
                                    break;
                                case 31:
                                    JENISCUTI = "Cuti Tibi (Klausa 3.15.)";
                                    break;
                                case 32:
                                    JENISCUTI = "Cuti Latihan Pasukan Sukarela (Klausa 6.1.)";
                                    break;
                                case 33:
                                    JENISCUTI = "Cuti Menghadiri Latihan Atau Khemah Tahunan Pertubuhan Atau Persatuan (Klausa 6.2.)";
                                    break;
                                case 34:
                                    JENISCUTI = "Cuti Menghadiri Mesyuarat, Khemah Latihan, Seminar Dan Aktiviti Sukan (Klausa 6.3.)";
                                    break;
                                case 35:
                                    JENISCUTI = "Cuti Mengambil Bahagian Dalam Olahraga";
                                    break;
                                case 36:
                                    JENISCUTI = "Ahli Majlis atau JawatanKuasa Pertandingan";
                                    break;
                                case 37:
                                    JENISCUTI = "Cuti Gantian Rehat";
                                    break;
                                case 38:
                                    JENISCUTI = "Cuti Sakit - Penyakit berjangkit";
                                    break;
                                case 39:
                                    JENISCUTI = "Cuti Sakit - Warded";
                                    break;
                                case 40:
                                    JENISCUTI = "Cuti Kebakaran";
                                    break;
                                case 45:
                                    JENISCUTI = "Cuti Bawa Kehadapan";
                                    break;
                                case 46:
                                    JENISCUTI = "Bekerja Dari Rumah";
                                    break;
                                case 1041:
                                    JENISCUTI = "Cuti Tanpa Rekod - Bertugas Untuk Istana";
                                    break;
                                case 1042:
                                    JENISCUTI = "Cuti Tanpa Rekod";
                                    break;

                            }

                            if (!string.IsNullOrEmpty(typeLeave.ToString()) && !string.IsNullOrEmpty(reason) && !string.IsNullOrEmpty(checker.ToString()))
                            {

                                string sql3 = @"
                                        UPDATE [tblMainKehadiran] 
                                        SET LeaveType = @JENISCUTII, Leave = @leave, Remark = @remark
                                        WHERE Employee = @id AND [date] = @date";

                                var _result3 = conn.Execute(sql3, new
                                {
                                    leave = typeLeave,
                                    remark = reason,
                                    id = myId,
                                    date = mDate,
                                    JENISCUTII = JENISCUTI
                                });

                                typeLeave = 0; reason = null; myId = null; mDate = null; JENISCUTI = null;
                            }



                        }
                    }
                }
            }
            catch (System.Exception err)
            {
                throw new Exception(sql, err);

            }
        }
    }
}
