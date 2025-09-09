using Microsoft.Data.SqlClient;
using System.Data;

namespace DAL.BaseConn;

public abstract class BaseConnectionSqlServer(string connectionString)
{
    public IDbConnection Connections() => new SqlConnection(connectionString);
}

public class ServerDev(string connStr) : BaseConnectionSqlServer(connStr) { }

public class ServerEHR(string connStr) : BaseConnectionSqlServer(connStr) { }

public class ServerProd(string connStr) : BaseConnectionSqlServer(connStr) { }

