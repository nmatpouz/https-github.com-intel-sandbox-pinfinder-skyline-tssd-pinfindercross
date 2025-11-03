using System.Data.Odbc;

namespace PinFinder.Database;

public class ApplicationDbContext
{
    private readonly OdbcConnectionStringBuilder _odbcConnectionStringBuilder;  
    public ApplicationDbContext(OdbcConnectionStringBuilder odbcConnectionStringBuilder)
    {
        _odbcConnectionStringBuilder = odbcConnectionStringBuilder;
    }

    public string QueryHeader(string lotId, string operCode, string summaryName)
    {
        var headerquery = $@"
    SELECT
        stpl, soc, env, lotId, packg, prgnm, prdct, testertype, sspec, lcode, sysid, tempr, ldbid, smrynam, prdctName, uniqueFolderName, overflow_flag, iudesignid, mudesignid
    FROM
        [pinfinder_production].[dbo].[ituff_header]
    WHERE
        lcode = {operCode} AND lotId = {lotId} AND smrynam = {summaryName}
    ";
        using var connection = new OdbcConnection(_odbcConnectionStringBuilder.ConnectionString);

        connection.Open();
        using var command = new OdbcCommand(headerquery, connection);
        using var reader = command.ExecuteReader();
        List<Json>
        while (reader.Read())
        {
            
        }

        return null;
    }
    public string QueryData(string lotId, string operCode, string summaryName)
    {
        var headerquery = $@"
    SELECT
        stpl, soc, env, lotId, packg, prgnm, prdct, testertype, sspec, lcode, sysid, tempr, ldbid, smrynam, prdctName, uniqueFolderName, overflow_flag, iudesignid, mudesignid
    FROM
        [pinfinder_production].[dbo].[ituff_header]
    WHERE
        lcode = {operCode} AND lotId = {lotId} AND smrynam = {summaryName}
    ";
        using var connection = new OdbcConnection(_odbcConnectionStringBuilder.ConnectionString);

        connection.Open();
        using var command = new OdbcCommand(headerquery, connection);
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return reader.GetString(0);
        }
        return null;
    }
}
