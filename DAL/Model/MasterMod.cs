using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{

    public class LoginModel
    {
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }

    public class tblMainKehadiran
    {
        public DateTime? Date { get; set; }
        public string Day_Type { get; set; } = string.Empty;
        public string Weekday { get; set; } = string.Empty;
        public short? User_ID { get; set; }
        public string First_Name { get; set; } = string.Empty;
        public string Last_Name { get; set; } = string.Empty;
        public string Employee { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public TimeSpan? In { get; set; }
        public TimeSpan? Out { get; set; }
        public decimal? Work { get; set; }
        public string OT { get; set; } = string.Empty;
        public double? Work2 { get; set; }
        public TimeSpan? Overt { get; set; }
        public byte? Group_Roster { get; set; }
        public string IC { get; set; } = string.Empty;
        public string Card_No { get; set; }
    }

    public class InfoDashboard
    {
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public int AttendanceCount { get; set; }
    }

    public class view_Butiran_Staf
    {
        public string NoStaf { get; set; } = string.Empty;
        public string NoStafBaca { get; set; } = string.Empty;
        public string NoStafPenuh { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public string Ic { get; set; } = string.Empty;
        public int JabatanId { get; set; }
        public string Jabatan { get; set; } = string.Empty;
        public int BahagianId { get; set; }
        public string Bahagian { get; set; } = string.Empty;
        public int UnitId { get; set; }
        public string Unit { get; set; } = string.Empty;
        public int PenempatanId { get; set; }
        public string Penempatan { get; set; } = string.Empty;
        public int JawatanId { get; set; }
        public string Jawatan { get; set; } = string.Empty;
        public int GredId { get; set; }
        public string Gred { get; set; } = string.Empty;
        public string Emel { get; set; } = string.Empty;
        public string NoTelefon { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }


    public class InfoReport
    {
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string Employee { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public int Kekerapan { get; set; }
    }

    public class InfoReportDetail
    {
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string Employee { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public int Kekerapan { get; set; }

        public TimeSpan In { get; set; }
        public TimeSpan Out { get; set; }

        public string Remark { get; set; } = string.Empty;
        public string LeaveType { get; set; } = string.Empty;
        public decimal Leave { get; set; }
        public decimal Work { get; set; }
    }

    public class tblInfoUserReport
    {
        public int Id { get; set; }
        public string NoStaf { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public int Status { get; set; }
    }

    public class tblInfoAuditLog
    {
        public int Id { get; set; } = 0;

        public DateTime? CreatedDate { get; set; } = null;

        public string NoStaf { get; set; } = string.Empty;

        public string Event { get; set; } = string.Empty;
    }

}
