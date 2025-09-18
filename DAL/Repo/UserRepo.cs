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
    public interface IUserRepo
    {
        Task<int> AddUserAsync(tblInfoUserReport user);
        Task<bool> UpdateUserAsync(tblInfoUserReport user);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<tblInfoUserReport>> GetAllUsersAsync();
        Task<tblInfoUserReport?> GetUserByIdAsync(int id);
        Task<bool> GetUserByNoStaffAsync(string nostaff);
        Task<bool> ResetPasswordAsync(int id, string plainPassword);
        Task<bool> KemaskiniPasswordBaru(string id, string password);
    }
    public class UserRepo(ServerProd serverProd, ServerDev serverDev, ServerEHR serverEhr) :IUserRepo
    {
        private readonly ServerProd _serverProd = serverProd;
        private readonly ServerDev _serverDev = serverDev;
        private readonly ServerEHR _serverEhr = serverEhr;

        

    // INSERT
    public async Task<int> AddUserAsync(tblInfoUserReport user)
        {
            var sql = @"INSERT INTO tblInfoUserReport (NoStaf, Nama, Password, Unit, Status)
                    VALUES (@NoStaf, @Nama, @Password, @Unit, @Status);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

            return await _serverProd.Connections().QuerySingleAsync<int>(sql, user);
        }

        // UPDATE
        public async Task<bool> UpdateUserAsync(tblInfoUserReport user)
        {
            var sql = @"UPDATE tblInfoUserReport
                    SET NoStaf = @NoStaf,
                        Nama = @Nama,
                        Password = @Password,
                        Unit = @Unit,
                        Status = @Status
                    WHERE Id = @Id";

            var rows = await _serverProd.Connections().ExecuteAsync(sql, user);
            return rows > 0;
        }

        // DELETE
        public async Task<bool> DeleteUserAsync(int id)
        {
            var sql = "DELETE FROM tblInfoUserReport WHERE Id = @Id";
            var rows = await _serverProd.Connections().ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        // LOAD ALL
        public async Task<IEnumerable<tblInfoUserReport>> GetAllUsersAsync()
        {
            var sql = "SELECT * FROM tblInfoUserReport ORDER BY Nama";
            return await _serverProd.Connections().QueryAsync<tblInfoUserReport>(sql);
        }

        // LOAD BY ID
        public async Task<tblInfoUserReport?> GetUserByIdAsync(int id)
        {
            var sql = "SELECT * FROM tblInfoUserReport WHERE Id = @Id";
            return await _serverProd.Connections().QueryFirstOrDefaultAsync<tblInfoUserReport>(sql, new { Id = id });
        }

        public async Task<bool> GetUserByNoStaffAsync(string nostaff)
        {
            var sql = "SELECT count(nostaf) FROM tblInfoUserReport WHERE nostaf = @nostaf";
            var ada = await _serverProd.Connections().ExecuteScalarAsync<int>(sql, new { nostaf = nostaff });
            return ada > 0;
        }

        public async Task<bool> ResetPasswordAsync(int id, string plainPassword)
        {
            const string sql = @" UPDATE tblInfoUserReport SET Password = @Password, status = 1 WHERE Id = @Id";

            var rowsAffected = await _serverProd
                .Connections()
                .ExecuteAsync(sql, new { Id = id, Password = plainPassword });

            return rowsAffected > 0;
        }

        public async Task<bool> KemaskiniPasswordBaru(string id, string password)
        {
            string sql = @"update tblInfoUserReport set password = @password, status = 2 where id = @id";
            var res = await _serverProd.Connections().ExecuteAsync(sql, new { id = id, password = password });
            return res > 0;
        }





    }
}
