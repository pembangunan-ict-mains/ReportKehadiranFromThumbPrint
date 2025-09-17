using DAL.Model;
using DAL.Repo;
using Microsoft.AspNetCore.Components.Forms;
using System.Data;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;

namespace SAL
{
    public interface IServices
    {
        Task<bool> CheckLoginAsync(string uid, string password);
        Task<IEnumerable<InfoDashboard>> GetInfoForDashboard();
        Task<int> KiraJumlah(); Task<tblMainKehadiran?> GetLatestRecord();
        Task<IEnumerable<view_Butiran_Staf>> GetInfoUser(string userid);
        Task<IEnumerable<InfoDashboard>> GetReportForChart();
        Task<IEnumerable<InfoReport>> GetInfoSummary_KurangJamBekerja(int Bulan, int Tahun);
        Task<IEnumerable<InfoReportDetail>> GetInfoDetails_KurangJamBekerja(int Bulan, int Tahun);
        Task<IEnumerable<InfoReport>> GetInfoSummary_LewatThumbprint(int Bulan, int Tahun);
        Task<IEnumerable<InfoReportDetail>> GetInfoDetails_LewatThumbprint(int Bulan, int Tahun);

        Task<IEnumerable<InfoReport>> GetInfoSummary_TiadaLogkeluar(int Bulan, int Tahun);
        Task<IEnumerable<InfoReportDetail>> GetInfoDetails_TiadaLogkeluar(int Bulan, int Tahun);

        Task<IEnumerable<InfoReport>> GetInfoSummary_TiadaInfoMIA(int Bulan, int Tahun);
        Task<IEnumerable<InfoReportDetail>> GetInfoDetail_TiadaInfoMIA(int Bulan, int Tahun);
        Task<IEnumerable<tblInfoUserReport>> GetloginInfo(string nostaff);
        Task InsertAuditLogAsync(string noStaf, string eventDescription);
        List<RekodModel> ProcessRAW2Final(List<string> records);

        //for migrate
        Task CleanDatabase1();
        Task CleanDatabase2();
        Task CleanDatabase_NOMA();
        Task CleanDatabasePBNo();
        Task CleanDatabasePBNo2();
        Task CleanDatabaseNoStaff();

        bool AddLog(LogEntry log);
        Task<List<string>?> ProsesFile(IBrowserFile _files); Task<bool> CheckDuplicateResult(string UID, string Employee, string xDate);
        Task<bool> InsertIntoDBFinal(IEnumerable<RekodModel> _rm);

        Task CrossCheckUpdateRekod();

        Task<int> AddUserAsync(tblInfoUserReport user);
        Task<bool> UpdateUserAsync(tblInfoUserReport user);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<tblInfoUserReport>> GetAllUsersAsync();
        Task<tblInfoUserReport?> GetUserByIdAsync(int id);
        Task<bool> GetUserByNoStaffAsync(string nostaff);
        Task<bool> ResetPasswordAsync(int id, string plainPassword);

    }
    public class Services(IRepoData repo, IReportRepo report, IUserRepo user) : IServices
    {
        private readonly IRepoData _repo = repo;
        private readonly IReportRepo _report = report;
        private readonly IUserRepo _user = user;

        public async Task<bool> CheckLoginAsync(string uid, string password)
        {
            try
            {
                await Task.CompletedTask;
                return uid == "sysadmin" && password == "P@wd2025";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<InfoDashboard>> GetInfoForDashboard()
        {
            // return await _repo.GetInfoForDashboard();
            var result = await _repo.GetInfoForDashboard();
            return result ?? Enumerable.Empty<InfoDashboard>();

        }

        public async Task<int> KiraJumlah()
        {
            return await _repo.KiraJumlah();
        }

        public async Task<tblMainKehadiran?> GetLatestRecord()
        {
            return await _repo.GetLatestRecord();
        }

        public async Task<IEnumerable<view_Butiran_Staf>> GetInfoUser(string userid)
        {
            return await _repo.GetInfoUser(userid);
        }

        public async Task<IEnumerable<InfoDashboard>> GetReportForChart()
        {
            return await _repo.GetReportForChart();
        }

        //----------------------

        public async Task<IEnumerable<InfoReport>> GetInfoSummary_KurangJamBekerja(int Bulan, int Tahun)
        {
            return await _report.GetInfoSummary_KurangJamBekerja(Bulan, Tahun);
        }

        public async Task<IEnumerable<InfoReportDetail>> GetInfoDetails_KurangJamBekerja(int Bulan, int Tahun)
        {
            return await _report.GetInfoDetails_KurangJamBekerja(Bulan, Tahun);
        }

        public async Task<IEnumerable<InfoReport>> GetInfoSummary_LewatThumbprint(int Bulan, int Tahun)
        {
            return await _report.GetInfoSummary_LewatThumbprint(Bulan, Tahun);
        }

        public async Task<IEnumerable<InfoReportDetail>> GetInfoDetails_LewatThumbprint(int Bulan, int Tahun)
        {
            return await _report.GetInfoDetails_LewatThumbprint(Bulan, Tahun);
        }

        public async Task<IEnumerable<InfoReport>> GetInfoSummary_TiadaLogkeluar(int Bulan, int Tahun)
        {
            return await _report.GetInfoSummary_TiadaLogkeluar(Bulan, Tahun);
        }
        public async Task<IEnumerable<InfoReportDetail>> GetInfoDetails_TiadaLogkeluar(int Bulan, int Tahun)
        {
            return await _report.GetInfoDetails_TiadaLogkeluar(Bulan, Tahun);
        }


        public async Task<IEnumerable<InfoReport>> GetInfoSummary_TiadaInfoMIA(int Bulan, int Tahun)
        {
            return await _report.GetInfoSummary_TiadaInfoMIA(Bulan, Tahun);
        }
        public async Task<IEnumerable<InfoReportDetail>> GetInfoDetail_TiadaInfoMIA(int Bulan, int Tahun)
        {
            return await _report.GetInfoDetail_TiadaInfoMIA(Bulan, Tahun);
        }

        public async Task<IEnumerable<tblInfoUserReport>> GetloginInfo(string nostaff)
        {
            return await _repo.GetloginInfo(nostaff);
        }

        public async Task InsertAuditLogAsync(string noStaf, string eventDescription)
        {
            await _repo.InsertAuditLogAsync(noStaf, eventDescription);
        }

        //for migration

        public async Task CleanDatabase1()
        {
            await _repo.CleanDatabase1();
        }
        public async Task CleanDatabase2()
        {
            await _repo.CleanDatabase2();
        }
        public async Task CleanDatabase_NOMA()
        {
            await _repo.CleanDatabase_NOMA();
        }
        public async Task CleanDatabasePBNo()
        {
            await _repo.CleanDatabasePBNo();
        }
        public async Task CleanDatabasePBNo2()
        {
            await _repo.CleanDatabasePBNo2();
        }
        public async Task CleanDatabaseNoStaff()
        {
            await _repo.CleanDatabaseNoStaff();
        }

        public bool AddLog(LogEntry log)
        {
            return _repo.AddLog(log);
        }
        private RekodModel ParseRekodModel(string record)
        {
            string[] fields = record.Split(",");
            if (fields.Length < 33)
            {
                throw new ArgumentException("Invalid record format: Insufficient fields");
            }

            string dateString = fields[0].Trim();
            RekodModel rm = new RekodModel
            {
                Date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture), //DateTime.Parse(fields[0].Trim()),
                DayType = fields[1].Trim(),
                Weekday = fields[2].Trim(),
                UserID = fields[3].Trim(),
                FirstName = fields[4].Trim(),
                LastName = fields[5].Trim(),
                Employee = fields[6].Trim(),
                Designation = fields[7].Trim(),
                Department = fields[8].Trim(),
                In = ParseTimeString(fields[9].Trim()),
                Out = ParseTimeString(fields[10].Trim()),
                Work = decimal.Parse(fields[11].Trim()),
                OT = fields[12].Trim(),
                Work2 = decimal.Parse(fields[13].Trim()),
                Overt = ParseTimeString(fields[14].Trim()),
                GroupRoster = Convert.ToInt32(fields[15].Trim()),
                IC = fields[16].Trim(),
                CardNo = fields[17].Trim(),
                ScheduleID = Convert.ToInt32(fields[18].Trim()),
                S = Convert.ToInt32(fields[19].Trim()),
                Break = fields[20].Trim(),
                Resume = fields[21].Trim(),
                Done = fields[22].Trim(),
                Diff = ParseTimeString(fields[23].Trim()),
                Short = ParseTimeString(fields[24].Trim()),
                TotalW = ParseTimeString(fields[25].Trim()),
                TotalO = ParseTimeString(fields[26].Trim()),
                SumSh = ParseTimeString(fields[27].Trim()),
                LeaveType = fields[28].Trim(),
                Leave = fields[29].Trim(),
                Leave2 = fields[30].Trim(),
                WorkCode = fields[31].Trim(),
                Remark = fields[32].Trim()
            };

            return rm;
        }


        public static TimeSpan? ParseTimeString(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            // Assuming the format is like '7:56:00 AM'
            if (DateTime.TryParseExact(text, new[] { "h:mm:ss tt", "h:mm tt" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
            {
                return dateTime.TimeOfDay;
            }

            return null; // Or throw an exception if you prefer
        }

        public string _processedText = string.Empty;
        public async Task<List<string>?> ProsesFile(IBrowserFile _files)
        {
            long maxFileSize = 1024L * 1024L * 1024L * 2L;
            try
            {
                var stream = _files.OpenReadStream(maxFileSize);
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    _processedText = await reader.ReadToEndAsync();
                    string[] records = _processedText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    return records.ToList();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                // Handle exception appropriately
                return new List<string>();
            }
        }

        List<RekodModel> _rmodel = new List<RekodModel>();
        public List<RekodModel> ProcessRAW2Final(List<string> records)
        {
            try
            {
                foreach (var record in records.Skip(1))
                {
                    RekodModel rm = ParseRekodModel(record);
                    _rmodel.Add(rm);
                }
                return _rmodel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<RekodModel>();
            }
        }

        public async Task<bool> CheckDuplicateResult(string UID, string Employee, string xDate)
        {
            return await _repo.CheckDuplicateResult(UID, Employee, xDate);
        }

        DataTable CreateDataTable()
        {
            DataTable tblMainKehadiran = new DataTable("tblMainKehadiran");

            // Add columns to the DataTable
            tblMainKehadiran.Columns.Add("Date", typeof(DateTime));
            tblMainKehadiran.Columns.Add("Day_Type", typeof(string));
            tblMainKehadiran.Columns.Add("Weekday", typeof(string));
            tblMainKehadiran.Columns.Add("User_ID", typeof(short));
            tblMainKehadiran.Columns.Add("First_Name", typeof(string));
            tblMainKehadiran.Columns.Add("Last_Name", typeof(string));
            tblMainKehadiran.Columns.Add("Employee", typeof(string));
            tblMainKehadiran.Columns.Add("Designation", typeof(string));
            tblMainKehadiran.Columns.Add("Department", typeof(string));
            tblMainKehadiran.Columns.Add("In", typeof(TimeSpan));
            tblMainKehadiran.Columns.Add("Out", typeof(TimeSpan));
            tblMainKehadiran.Columns.Add("Work", typeof(decimal));
            tblMainKehadiran.Columns.Add("OT", typeof(string));
            tblMainKehadiran.Columns.Add("Work2", typeof(float));
            tblMainKehadiran.Columns.Add("Overt", typeof(TimeSpan));
            tblMainKehadiran.Columns.Add("Group_Roster", typeof(byte));
            tblMainKehadiran.Columns.Add("IC", typeof(string));
            tblMainKehadiran.Columns.Add("Card_No", typeof(string));
            tblMainKehadiran.Columns.Add("Schedule_ID", typeof(byte));
            tblMainKehadiran.Columns.Add("S", typeof(byte));
            tblMainKehadiran.Columns.Add("Break", typeof(string));
            tblMainKehadiran.Columns.Add("Resume", typeof(string));
            tblMainKehadiran.Columns.Add("Done", typeof(string));
            tblMainKehadiran.Columns.Add("Diff", typeof(TimeSpan));
            tblMainKehadiran.Columns.Add("Short", typeof(TimeSpan));
            tblMainKehadiran.Columns.Add("Total_W", typeof(TimeSpan));
            tblMainKehadiran.Columns.Add("Total_O", typeof(TimeSpan));
            tblMainKehadiran.Columns.Add("Sum_Sh", typeof(TimeSpan));
            tblMainKehadiran.Columns.Add("LeaveType", typeof(string));
            tblMainKehadiran.Columns.Add("Leave", typeof(string));
            tblMainKehadiran.Columns.Add("Leave2", typeof(string));
            tblMainKehadiran.Columns.Add("Work_Code", typeof(string));
            tblMainKehadiran.Columns.Add("Remark", typeof(string));


            return tblMainKehadiran;
        }
        DataTable AssignDataTble(IEnumerable<RekodModel> records, DataTable dt)
        {

            foreach (var record in records)
            {
                DataRow row = dt.NewRow();
                row["Date"] = record.Date;
                row["Day_Type"] = record.DayType.Trim();
                row["Weekday"] = record.Weekday.Trim();
                row["User_ID"] = record.UserID.Trim();
                row["First_Name"] = record.FirstName.Trim();
                row["Last_Name"] = record.LastName.Trim();
                row["Employee"] = record.Employee.Trim();
                row["Designation"] = record.Designation.Trim();
                row["Department"] = record.Department.Trim();
                row["In"] = record.In != null ? record.In : DBNull.Value;
                row["Out"] = record.Out != null ? record.Out : DBNull.Value;
                row["Work"] = record.Work != null ? record.Work : DBNull.Value;
                row["OT"] = record.OT != null ? record.OT : DBNull.Value;
                row["Work2"] = record.Work2;
                row["Overt"] = record.Overt != null ? record.Overt : DBNull.Value;
                row["Group_Roster"] = record.GroupRoster != null ? record.GroupRoster : DBNull.Value;
                row["IC"] = record.IC;
                row["Card_No"] = record.CardNo;
                row["Schedule_ID"] = record.ScheduleID != null ? record.ScheduleID : DBNull.Value;
                row["S"] = record.S != null ? record.S : DBNull.Value;
                row["Break"] = record.Break != null ? record.Break : DBNull.Value;
                row["Resume"] = record.Resume != null ? record.Resume : DBNull.Value;
                row["Done"] = record.Done != null ? record.Done : DBNull.Value;
                row["Diff"] = record.Diff != null ? record.Diff : DBNull.Value;
                row["Short"] = record.Short != null ? record.Short : DBNull.Value;
                row["Total_W"] = record.TotalW != null ? record.TotalW : DBNull.Value;
                row["Total_O"] = record.TotalW != null ? record.TotalO : DBNull.Value;
                row["Sum_Sh"] = record.SumSh != null ? record.SumSh : DBNull.Value;
                row["LeaveType"] = record.LeaveType;
                row["Leave"] = record.Leave != null ? record.Leave : DBNull.Value;
                row["Leave2"] = record.Leave2 != null ? record.Leave2 : DBNull.Value;
                row["Work_Code"] = record.WorkCode.Trim();
                row["Remark"] = record.Remark.Trim();

                dt.Rows.Add(row);
            }


            return dt;
        }

        public async Task<bool> InsertIntoDBFinal(IEnumerable<RekodModel> _rm)
        {
            DataTable dataTable = new DataTable();

            dataTable = CreateDataTable();
            dataTable = AssignDataTble(_rm, dataTable);
            return await _repo.InsertIntoDBFinal_BULK(dataTable);
        }

        public async Task CrossCheckUpdateRekod()
        {
            await _repo.CrossCheckUpdateRekod();
        }

        //---- user operasi
        public async Task<int> AddUserAsync(tblInfoUserReport user)
        {
            return await _user.AddUserAsync(user);
        }

        public async Task<bool> UpdateUserAsync(tblInfoUserReport user)
        {
            return await _user.UpdateUserAsync(user);
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _user.DeleteUserAsync(id);
        }
        public async Task<IEnumerable<tblInfoUserReport>> GetAllUsersAsync()
        {
            return await _user.GetAllUsersAsync();
        }
        public async Task<tblInfoUserReport?> GetUserByIdAsync(int id)
        {
            return await _user.GetUserByIdAsync(id);
        }

        public async Task<bool> GetUserByNoStaffAsync(string nostaff)
        {
            return await _user.GetUserByNoStaffAsync(nostaff);
        }

        public async Task<bool> ResetPasswordAsync(int id, string plainPassword)
        {
            return await _user.ResetPasswordAsync(id, plainPassword);
        }
    }
}
