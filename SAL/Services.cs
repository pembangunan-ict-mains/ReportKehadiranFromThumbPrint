using DAL.Model;
using DAL.Repo;

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
    }
    public class Services(IRepoData repo, IReportRepo report) : IServices
    {
        private readonly IRepoData _repo = repo;
        private readonly IReportRepo _report = report;

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
            return await _report.GetInfoSummary_KurangJamBekerja(Bulan,Tahun);
        }

        public async Task<IEnumerable<InfoReportDetail>> GetInfoDetails_KurangJamBekerja(int Bulan, int Tahun)
        {
            return await _report.GetInfoDetails_KurangJamBekerja(Bulan, Tahun);
        }
    }
}
