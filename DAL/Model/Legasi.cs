using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{

    public class ReportSummary
    {
        public int Month { get; set; }
        public int Nama { get; set; }
        public int TotalKekerapan { get; set; }
    }
    public class InfoCuti
    {
        public DateTime TarikhMula { get; set; }
        public DateTime TarikhAkhir { get; set; }
    }

    public class DetailReportSummaryInfo
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? WeekDay { get; set; } = string.Empty;
        public string? Employee { get; set; } = string.Empty;
        public string? NamaPekerja { get; set; } = string.Empty;
        public string? EmployeeName { get; set; } = string.Empty;
        public string? LeaveType { get; set; } = string.Empty;
        public string? Department { get; set; } = string.Empty;
        public TimeSpan In { get; set; }
        public TimeSpan Out { get; set; }
        public string? Remark { get; set; } = string.Empty;
        public decimal Work { get; set; }
    }


    public class ReportSummaryInfo
    {
        public int Id { get; set; }
        public string NamaPekerja { get; set; } = string.Empty;
        public string NoPekerja { get; set; } = string.Empty;
        public int Tahun { get; set; }
        public int Bulan { get; set; }
        public string? BulanText { get; set; } = string.Empty;
        public int MissingInAction { get; set; }
        public int LewatThumb { get; set; }
        public int TiadaTimeOut { get; set; }
        public int KurangJamBekerja { get; set; }
        public int Work { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Employee { get; set; } = string.Empty;
        public string First_Name { get; set; } = string.Empty;
        public string Last_Name { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

    public class ReportMeragukan
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? First_Name { get; set; } = string.Empty;
        public string? Last_name { get; set; } = string.Empty;
        public string? Employee { get; set; } = string.Empty;
        public TimeSpan In { get; set; }
        public TimeSpan Out { get; set; }
        public decimal Work { get; set; }
        public string? Department { get; set; } = string.Empty;
        public string? EditOleh { get; set; } = string.Empty;

        public DateTime TarikhEdit { get; set; }
        public string? Remark { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
    }

    public class AllReportShared
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? NAMA { get; set; } = string.Empty;
        public string? WeekDay { get; set; } = string.Empty;
        public int User_Id { get; set; }
        public string? Employee { get; set; } = string.Empty;
        public string? In { get; set; } = string.Empty;
        public string? Out { get; set; } = string.Empty;
        public string? LeaveType { get; set; } = string.Empty;
        public string? Remark { get; set; } = string.Empty;

    }
    public class DataV3
    {
        public DateTime DATE { get; set; }
        public string? EMPLOYEE { get; set; } = string.Empty;
        public string? LEAVETYPE { get; set; } = string.Empty;
    }

    public class Dashboard1
    {
        public int Jumlah { get; set; }
        public string? Department { get; set; } = string.Empty;
    }
    public class DepartmentInfo
    {
        public string? Nama { get; set; } = string.Empty;
    }

    public class ReportByMonth
    {
        public int BULAN { get; set; }
        public string BULANNAME { get; set; } = string.Empty;
        public int JUMLAHREKOD { get; set; }
    }

    public class ReportByPercent
    {
        public string Type { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public int Jumlah { get; set; }
    }

    public class ReportTimeOutNull
    {
        public int Id { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Bulan { get; set; } = string.Empty;
        public int Jumlah { get; set; }

        public string JumlahTotal { get; set; } = string.Empty;
        public string TotalHari { get; set; } = string.Empty;
    }


    public class ReportDetailsbyMonthKeseluruhan_New
    {
        public int Id { get; set; }
        public string Bulan { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public double Kerja { get; set; }
        public double Cuti { get; set; }
        public double MIA { get; set; }
    }

    public class ReportDetailsbyMonthKeseluruhan
    {
        public int Id { get; set; }
        public string Bulan { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public int Jumlah { get; set; }
        public double JumlahP { get; set; }
    }
    public class ReportDetailsbyMonth
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Day_Type { get; set; } = string.Empty;
        public string Weekday { get; set; } = string.Empty;
        public int User_Id { get; set; }
        public string First_Name { get; set; } = string.Empty;
        public string Last_Name { get; set; } = string.Empty;
        public string Employee { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string In { get; set; } = string.Empty;
        public string Out { get; set; } = string.Empty;
        public string NamaPekerja { get; set; } = string.Empty;
        public string LeaveType { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public string Work { get; set; } = string.Empty;
    }

    public class ReportByJabatan
    {
        public int BULAN { get; set; }
        public int JUMLAHREKOD { get; set; }
        public string LEAVETYPE { get; set; }
    }

    public class ReportbyJabatanOverall
    {
        public int BULAN { get; set; }
        public int JUMLAHREKOD { get; set; }
        public string DEPARTMENT { get; set; } = string.Empty;
    }

    public class TahunSemasa
    {
        public int Tahun { get; set; }
    }
    public class ListOfStaff
    {
        public int UID { get; set; }
        public string FULLNAME { get; set; } = string.Empty;
    }

    public class Department
    {
        public string ListDepartment { get; set; } = string.Empty;
    }

    public class ReportIndividuAbsent
    {
        public string NAMABULAN { get; set; } = string.Empty;
        public int BULAN { get; set; }
        public int UID { get; set; }
        public int JUMLAH { get; set; }
        public string LEAVE { get; set; } = string.Empty;
    }

    public class RekodModel
    {
        public DateTime Date { get; set; }
        public string DayType { get; set; } = string.Empty;
        public string Weekday { get; set; } = string.Empty;
        public string UserID { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Employee { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public TimeSpan? In { get; set; }
        public TimeSpan? Out { get; set; }
        public decimal? Work { get; set; }
        public string OT { get; set; } = string.Empty;
        public decimal Work2 { get; set; }
        public TimeSpan? Overt { get; set; }
        public int? GroupRoster { get; set; }
        public string IC { get; set; } = string.Empty;
        public string CardNo { get; set; } = string.Empty;
        public int? ScheduleID { get; set; }
        public int? S { get; set; }
        public string Break { get; set; } = string.Empty;
        public string Resume { get; set; } = string.Empty;
        public string Done { get; set; } = string.Empty;
        public TimeSpan? Diff { get; set; }
        public TimeSpan? Short { get; set; }
        public TimeSpan? TotalW { get; set; }
        public TimeSpan? TotalO { get; set; }
        public TimeSpan? SumSh { get; set; }
        public string LeaveType { get; set; } = string.Empty;
        public string Leave { get; set; } = string.Empty;
        public string Leave2 { get; set; } = string.Empty;
        public string WorkCode { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
    }

    //public class RekodModel
    //{
    //    public DateTime Date { get; set; }
    //    public string DayType { get; set; } = string.Empty;
    //    public string Weekday { get; set; } = string.Empty;
    //    public string UserID { get; set; } = string.Empty;
    //    public string FirstName { get; set; } = string.Empty;
    //    public string LastName { get; set; } = string.Empty;
    //    public string Employee { get; set; } = string.Empty;
    //    public string Designation { get; set; } = string.Empty;
    //    public string Department { get; set; } = string.Empty;
    //    public string In { get; set; } = string.Empty;
    //    public string Out { get; set; } = string.Empty;
    //    public string Work { get; set; } = string.Empty;
    //    public string OT { get; set; } = string.Empty;
    //    public string Work2 { get; set; } = string.Empty;
    //    public string Overt { get; set; } = string.Empty;
    //    public string GroupRoster { get; set; } = string.Empty;
    //    public string IC { get; set; } = string.Empty;
    //    public string CardNo { get; set; } = string.Empty;
    //    public string ScheduleID { get; set; } = string.Empty;
    //    public string S { get; set; } = string.Empty;
    //    public string Break { get; set; } = string.Empty;
    //    public string Resume { get; set; } = string.Empty;
    //    public string Done { get; set; } = string.Empty;
    //    public string Diff { get; set; } = string.Empty;
    //    public string Short { get; set; } = string.Empty;
    //    public string TotalW { get; set; } = string.Empty;
    //    public string TotalO { get; set; } = string.Empty;
    //    public string SumSh { get; set; } = string.Empty;
    //    public string LeaveType { get; set; } = string.Empty;
    //    public string Leave { get; set; } = string.Empty;
    //    public string Leave2 { get; set; } = string.Empty;
    //    public string WorkCode { get; set; } = string.Empty;
    //    public string Remark { get; set; } = string.Empty;
    //}

    public class CleanDB
    {
        public int User_Id { get; set; }
        public string last_name { get; set; } = string.Empty;
    }

    public class DataPB
    {
        public string NAMA { get; set; } = string.Empty;
        public string Employee { get; set; } = string.Empty;
        public int User_Id { get; set; }
    }

    public class DataNOMA
    {
        public int user_id { get; set; }
        public string Nama { get; set; } = string.Empty;
        public string NoMa { get; set; } = string.Empty;
    }

    public class RekodRujukan
    {
        public string NoMa { get; set; } = string.Empty;
        public int User_Id { get; set; }
    }

    public class Data
    {
        public DateTime? Date { get; set; }
        public string? Employee { get; set; } = string.Empty;
        public string? LeaveType { get; set; } = string.Empty;

        public string? Remark { get; set; } = string.Empty;
        public string? Id { get; set; } = string.Empty;
        public string? User_Id { get; set; } = string.Empty;
    }

    public class LaporanCutiIndividu
    {
        public int myID { get; set; }
        public string JenisCuti { get; set; } = string.Empty;
        public int JumlahCuti { get; set; }
        public string KeteranganCuti { get; set; } = string.Empty;
        public string Ringkasan { get; set; } = string.Empty;
        public string UID { get; set; } = string.Empty;
        public string Tempat { get; set; } = string.Empty;
        public string Ulasan { get; set; } = string.Empty;
        public DateTime Dari { get; set; }
        public DateTime Hingga { get; set; }

        public int JumlahHari { get; set; }
    }

    public class LoginModelInfo
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedDate { get; set; } = string.Empty;

        public string CreatedDate2 { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string NamaPenuh { get; set; } = string.Empty;
        [Required]
        public string Roles { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
        public int Flag { get; set; } = 0;
    }

    public class LogEntry
    {
        public DateTime Date { get; set; }
        public string Logs { get; set; }
        public string Modules { get; set; }
    }


    public class ReportKESELURUHAN
    {
        public int Id { get; set; }
        public string Department { get; set; } = string.Empty;
        public int Jumlah { get; set; }
        public int JumlahTotal { get; set; }
        public int Year { get; set; }

    }

    //-------------------

    public class REPORTLEWATTHUMBPRINTSUKN9
    {
        public int Id { get; set; }
        public string? NAMA { get; set; } = string.Empty;
        public string? EMPLOYEE { get; set; } = string.Empty;
        public int KEKERAPAN { get; set; }
        public int BULAN { get; set; }
        public int TAHUN { get; set; }
        public decimal PERCENTAGE { get; set; }

    }

    public class SUMMARYSUMMARY
    {
        public int Id { get; set; }
        public string? EMPLOYEE { get; set; } = string.Empty;
        public string? NAMA { get; set; } = string.Empty;
        public int BULAN { get; set; }
        public int TAHUN { get; set; }
        public TimeSpan? In { get; set; }
        public TimeSpan? Out { get; set; }

        public decimal Leave { get; set; }
        public decimal Work { get; set; }
        public string Remark { get; set; } = string.Empty;
        public string LeaveType { get; set; } = string.Empty;

    }

    public class REPORTTIADALOGKELUARSUKN9
    {
        public int Id { get; set; }
        public string? NAMA { get; set; } = string.Empty;
        public string? EMPLOYEE { get; set; } = string.Empty;
        public int KEKERAPAN { get; set; }
        public int BULAN { get; set; }
        public int TAHUN { get; set; }
        public decimal PERCENTAGE { get; set; }
    }

    public class REPORTTIDAKCUKUP9JAMSUKN9
    {
        public int Id { get; set; }
        public string? NAMA { get; set; } = string.Empty;
        public string? EMPLOYEE { get; set; } = string.Empty;
        public int KEKERAPAN { get; set; }
        public int BULAN { get; set; }
        public int TAHUN { get; set; }
        public decimal PERCENTAGE { get; set; }
    }

    public class REPORTTIADAINFORMASISUKN9
    {
        public int Id { get; set; }
        public string? NAMA { get; set; } = string.Empty;
        public string? EMPLOYEE { get; set; } = string.Empty;
        public int KEKERAPAN { get; set; }
        public int BULAN { get; set; }
        public int TAHUN { get; set; }
        public decimal PERCENTAGE { get; set; }
    }


    public class UNITORGANISASI
    {
        public int Idx { get; set; }
        public string? Unit { get; set; } = string.Empty;
        public int JumlahPekerja { get; set; }
    }
}
