namespace InventoryApi.Data;

public class SqlConnectionFactory
{
    private readonly string _cs;
    public SqlConnectionFactory(string cs) => _cs = cs;
    public System.Data.IDbConnection Create()
        => new Microsoft.Data.SqlClient.SqlConnection(_cs);
}
