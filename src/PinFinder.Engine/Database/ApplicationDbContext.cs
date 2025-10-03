using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Internal;

using PinFinder.Core.Domain.Commands.Replies;

using System.Data.Odbc;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;

using PinFinder.Core.Extension;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.ComponentModel;
using static MudBlazor.FilterOperator;
using System.Text;

namespace PinFinder.Database;

public class ApplicationDbContext : DbContext
{
    private readonly OdbcConnectionStringBuilder _odbcConnectionStringBuilder;
    public ApplicationDbContext(OdbcConnectionStringBuilder odbcConnectionStringBuilder)
    {
        _odbcConnectionStringBuilder = odbcConnectionStringBuilder;
    }
    public SummaryReply QuerySummary(string lotId, string operCode)
    {
        var Summaryquery = @"
        SELECT DISTINCT
            smrynam
        FROM
            [pinfinder_production].[dbo].[ituff_header]
        WHERE
            lcode = ? AND lotId = ?
        ";

        using var connection = new OdbcConnection(_odbcConnectionStringBuilder.ConnectionString);

        connection.Open();

        // Define command
        using var command = new OdbcCommand(Summaryquery, connection);

        // Add parameters to the command
        command.Parameters.AddWithValue("@lcode", operCode);
        command.Parameters.AddWithValue("@lotId", lotId);

        using var reader = command.ExecuteReader();
        List<SummaryData> jsonRequest = new List<SummaryData>();
        while (reader.Read())
        {
            jsonRequest.Add(new SummaryData
            {
                Summary = reader.IsDBNull(0) ? null : reader.GetString(0),
            });
        }

        return new SummaryReply { SummaryList = jsonRequest };
    }
    public SiteReply QuerySite(string lotId, string operCode, string site)
    {
        if (site == "PG12" || site == "IDC" || site == "JF")
            site = "LABS";

        // Construct the table name using the site parameter
        string unitDataTableName = $"{site}_unit_data";

        using var connection = new OdbcConnection(_odbcConnectionStringBuilder.ConnectionString);
        connection.Open();

        // Check if the table exists in the database
        var checkTableQuery = $@"
        SELECT COUNT(*)
        FROM INFORMATION_SCHEMA.TABLES
        WHERE TABLE_NAME = '{unitDataTableName}'
        ";

        using var checkCommand = new OdbcCommand(checkTableQuery, connection);
        int tableExists = (int)checkCommand.ExecuteScalar();

        if (tableExists == 0)
        {
            // Table does not exist, return with LotList as null
            return new SiteReply { LotList = null };
        }

        // Define the query to get the lotId
        var siteQuery = $@"
        SELECT DISTINCT
            lotId
        FROM
            [pinfinder_production].[dbo].[{unitDataTableName}]
        WHERE
            lcode = ? AND lotId = ?
        ";

        using var command = new OdbcCommand(siteQuery, connection);

        // Add parameters to the command
        command.Parameters.AddWithValue("@lcode", operCode);
        command.Parameters.AddWithValue("@lotId", lotId);

        using var reader = command.ExecuteReader();
        List<SiteData> jsonRequest = new List<SiteData>();
        while (reader.Read())
        {
            jsonRequest.Add(new SiteData
            {
                LotID = reader.IsDBNull(0) ? null : reader.GetString(0),
            });
        }

        return new SiteReply { LotList = jsonRequest };
    }

    public HeaderReply QueryHeader(string lotId, string operCode, string summaryName)
    {
        var headerquery = @"
    SELECT
        stpl, soc, env, lotId, packg, prgnm, prdct, testertype, sspec, lcode, sysid, tempr, ldbid, smrynam, prdctName, uniqueFolderName, overflow_flag, iudesignid, mudesignid
    FROM
        [pinfinder_production].[dbo].[ituff_header]
    WHERE
        lcode = ? AND lotId = ? AND smrynam = ?
    ";

        using var connection = new OdbcConnection(_odbcConnectionStringBuilder.ConnectionString);

        connection.Open();

        // Define command
        using var command = new OdbcCommand(headerquery, connection);

        // Add parameters to the command
        command.Parameters.AddWithValue("@lcode", operCode);
        command.Parameters.AddWithValue("@lotId", lotId);
        command.Parameters.AddWithValue("@smrynam", summaryName);

        using var reader = command.ExecuteReader();
        List<HeaderData> jsonRequest = new List<HeaderData>();
        while (reader.Read())
        {
            jsonRequest.Add(new HeaderData
            {
                Stpl = reader.IsDBNull(0) ? null : reader.GetString(0),
                Soc = reader.IsDBNull(1) ? null : reader.GetString(1),
                Env = reader.IsDBNull(2) ? null : reader.GetString(2),
                LotId = reader.IsDBNull(3) ? null : reader.GetString(3),
                Packg = reader.IsDBNull(4) ? null : reader.GetString(4),
                Prgnm = reader.IsDBNull(5) ? null : reader.GetString(5),
                Prdct = reader.IsDBNull(6) ? null : reader.GetString(6),
                TesterType = reader.IsDBNull(7) ? null : reader.GetString(7),
                Sspec = reader.IsDBNull(8) ? null : reader.GetString(8),
                Lcode = reader.IsDBNull(9) ? null : reader.GetString(9),
                SysId = reader.IsDBNull(10) ? null : reader.GetString(10),
                Tempr = reader.IsDBNull(11) ? null : reader.GetString(11),
                Ldbid = reader.IsDBNull(12) ? null : reader.GetString(12),
                Smrynam = reader.IsDBNull(13) ? null : reader.GetString(13),
                PrdctName = reader.IsDBNull(14) ? null : reader.GetString(14),
                UniqueFolderName = reader.IsDBNull(15) ? null : reader.GetString(15),
                OverflowFlag = reader.IsDBNull(16) ? null : reader.GetString(16),
                IuDesignId = reader.IsDBNull(17) ? null : reader.GetString(17),
                MuDesignId = reader.IsDBNull(18) ? null : reader.GetString(18)
            });
        }

        return new HeaderReply { HeaderList = jsonRequest };
    }
    public UnitReply QueryUnit(string lotId, string operCode, string summaryName, string site)
    {
        if (site == "PG12" || site == "IDC" || site == "JF")
            site = "LABS";
        // Construct the table name using the site parameter
        string unitDataTableName = $"{site}_unit_data";

        var headerquery = $@"
            SELECT
            [uh].[site], [uh].[testNameAccuracy], [uh].[start], [uh].[tuiId], [uh].[end], [uh].[sum], [uh].[productNum], [uh].[ult], [uh].[fbin], [uh].[visualId], [uh].[eightdigitbin], [uh].[index], [uh].[smrynam],
            [ud].[Tname], [ud].[Die], [ud].[Connector], [ud].[FailCycle], [ud].[PinName], [ud].[TiuPinName], [ud].[MEASURE], [ud].[FailPattern], [ud].[CHAN], [ud].[FailVector]
        FROM
            [pinfinder_production].[dbo].[unit_header] AS [uh]
        JOIN
            [pinfinder_production].[dbo].[{unitDataTableName}] AS [ud]
        ON
            [uh].[lcode] = [ud].[lcode] AND [uh].[lotId] = [ud].[lotId] AND [uh].[productNum] = [ud].[productNum] AND [uh].[smrynam] = [ud].[smrynam] AND [uh].[tuiId] = [ud].[tuiId]
        WHERE
            [uh].[lcode] = ? AND [uh].[lotId] = ? AND [uh].[smrynam] = ?
        ";

        using var connection = new OdbcConnection(_odbcConnectionStringBuilder.ConnectionString);
        connection.Open();

        // Define command
        using var command = new OdbcCommand(headerquery, connection);

        // Add parameters to the command
        command.Parameters.AddWithValue("@lcode", operCode);
        command.Parameters.AddWithValue("@lotId", lotId);
        command.Parameters.AddWithValue("@smrynam", summaryName);

        using var reader = command.ExecuteReader();

        // Store the results in a list of dictionaries
        var results = new List<Dictionary<string, object>>();

        Dictionary<string, UnitHeader> Units = new Dictionary<string, UnitHeader>();
        while (reader.Read())
        {
            var product = reader.IsDBNull(6) ? null : reader.GetString(6);
            var tname = reader.IsDBNull(13) ? null : reader.GetString(13);

            if (product == null || tname == null)
            {
                continue;
            }

            if (!Units.TryGetValue(product, out var unitHeader))
            {
                unitHeader = new UnitHeader
                {
                    Tests = new List<TestNameInfo>()  // Change from Dictionary to List
                };
                Units[product] = unitHeader;
            }

            unitHeader.Site = reader.IsDBNull(0) ? null : reader.GetString(0);
            unitHeader.TestNameAccuracy = reader.IsDBNull(1) ? null : reader.GetString(1);
            unitHeader.Start = reader.IsDBNull(2) ? null : reader.GetString(2);
            unitHeader.TiuId = reader.IsDBNull(3) ? null : reader.GetString(3);
            unitHeader.End = reader.IsDBNull(4) ? null : reader.GetString(4);
            unitHeader.Sum = reader.IsDBNull(5) ? null : reader.GetString(5);
            unitHeader.ProductNum = product;
            unitHeader.Ult = reader.IsDBNull(7) ? null : reader.GetString(7);
            unitHeader.Fbin = reader.IsDBNull(8) ? null : reader.GetString(8);
            unitHeader.VisualId = reader.IsDBNull(9) ? null : reader.GetString(9);
            unitHeader.EBin = reader.IsDBNull(10) ? null : reader.GetString(10);

            // Create a new TestNameInfo for each row
            var testName = new TestNameInfo
            {
                Tname = tname,
                Die = reader.IsDBNull(14) ? null : reader.GetString(14),
                Connector = reader.IsDBNull(15) ? null : reader.GetString(15),
                FailCycle = reader.IsDBNull(16) ? null : reader.GetString(16),
                PinName = reader.IsDBNull(17) ? null : reader.GetString(17),
                TiuPinName = reader.IsDBNull(18) ? null : reader.GetString(18),
                Measure = reader.IsDBNull(19) ? null : reader.GetString(19),
                FailPattern = reader.IsDBNull(20) ? null : reader.GetString(20),
                Channel = reader.IsDBNull(21) ? null : reader.GetString(21),
                FailVector = reader.IsDBNull(22) ? null : reader.GetString(22)
            };

            // Add the TestNameInfo to the list
            unitHeader.Tests.Add(testName);
        }
        var units = Units;
        return new UnitReply { Units = Units };
    }
    public UnitReply QueryTiuIdUnitData(string tiuId, string site)
    {
        if (site == "PG12" || site == "IDC" || site == "JF")
            site = "LABS";
        // Construct the table name using the site parameter
        string unitDataTableName = $"{site}_unit_data";

        var headerquery = $@"
            SELECT
            [uh].[site], [uh].[testNameAccuracy], [uh].[start], [uh].[tuiId], [uh].[end], [uh].[sum], [uh].[productNum], [uh].[ult], [uh].[fbin], [uh].[visualId], [uh].[eightdigitbin], [uh].[index], [uh].[smrynam],
            [ud].[Tname], [ud].[Die], [ud].[Connector], [ud].[FailCycle], [ud].[PinName], [ud].[TiuPinName], [ud].[MEASURE], [ud].[FailPattern], [ud].[CHAN], [ud].[FailVector]
        FROM
            [pinfinder_production].[dbo].[unit_header] AS [uh]
        JOIN
            [pinfinder_production].[dbo].[{unitDataTableName}] AS [ud]
        ON
            [uh].[tuiId] = [ud].[tuiId]
        WHERE
            [uh].[tuiId] = ?
        ";

        using var connection = new OdbcConnection(_odbcConnectionStringBuilder.ConnectionString);
        connection.Open();

        // Define command
        using var command = new OdbcCommand(headerquery, connection);

        // Add parameters to the command
        command.Parameters.AddWithValue("@tuiId", tiuId);

        using var reader = command.ExecuteReader();

        // Store the results in a list of dictionaries
        var results = new List<Dictionary<string, object>>();
        int count = 0;

        Dictionary<string, UnitHeader> Units = new Dictionary<string, UnitHeader>();
        while (reader.Read())
        {
            count++;
            var product = reader.IsDBNull(6) ? null : reader.GetString(6);
            var tname = reader.IsDBNull(13) ? null : reader.GetString(13);

            if (product == null || tname == null)
            {
                continue;
            }

            if (!Units.TryGetValue(product, out var unitHeader))
            {
                unitHeader = new UnitHeader
                {
                    Tests = new List<TestNameInfo>()  // Change from Dictionary to List
                };
                Units[product] = unitHeader;
            }

            unitHeader.Site = reader.IsDBNull(0) ? null : reader.GetString(0);
            unitHeader.TestNameAccuracy = reader.IsDBNull(1) ? null : reader.GetString(1);
            unitHeader.Start = reader.IsDBNull(2) ? null : reader.GetString(2);
            unitHeader.TiuId = reader.IsDBNull(3) ? null : reader.GetString(3);
            unitHeader.End = reader.IsDBNull(4) ? null : reader.GetString(4);
            unitHeader.Sum = reader.IsDBNull(5) ? null : reader.GetString(5);
            unitHeader.ProductNum = product;
            unitHeader.Ult = reader.IsDBNull(7) ? null : reader.GetString(7);
            unitHeader.Fbin = reader.IsDBNull(8) ? null : reader.GetString(8);
            unitHeader.VisualId = reader.IsDBNull(9) ? null : reader.GetString(9);
            unitHeader.EBin = reader.IsDBNull(10) ? null : reader.GetString(10);

            // Create a new TestNameInfo for each row
            var testName = new TestNameInfo
            {
                Tname = tname,
                Die = reader.IsDBNull(14) ? null : reader.GetString(14),
                Connector = reader.IsDBNull(15) ? null : reader.GetString(15),
                FailCycle = reader.IsDBNull(16) ? null : reader.GetString(16),
                PinName = reader.IsDBNull(17) ? null : reader.GetString(17),
                TiuPinName = reader.IsDBNull(18) ? null : reader.GetString(18),
                Measure = reader.IsDBNull(19) ? null : reader.GetString(19),
                FailPattern = reader.IsDBNull(20) ? null : reader.GetString(20),
                Channel = reader.IsDBNull(21) ? null : reader.GetString(21),
                FailVector = reader.IsDBNull(22) ? null : reader.GetString(22)
            };

            // Add the TestNameInfo to the list
            unitHeader.Tests.Add(testName);
        }
        var units = Units;
        return new UnitReply { Units = Units };
    }
    public ProductConfigReply QueryProductConfig(bool request)
    {
        if (request)
        {
            var configquery = @"
            SELECT
            *
            FROM
                [pinfinder_production].[dbo].[pinfinder_config]
            ";
            using var connection = new OdbcConnection(_odbcConnectionStringBuilder.ConnectionString);

            connection.Open();

            // Define command
            using var command = new OdbcCommand(configquery, connection);

            using var reader = command.ExecuteReader();

            List<ProductConfig> configRequest = new List<ProductConfig>();
            while (reader.Read())
            {
                configRequest.Add(new ProductConfig
                {
                    Name = reader.IsDBNull(0) ? null : reader.GetString(0),
                    PackageType = reader.IsDBNull(1) ? null : reader.GetString(1),
                    PackageId = reader.IsDBNull(2) ? null : reader.GetString(2),
                    TestProgram = reader.IsDBNull(3) ? null : reader.GetString(3),
                    TIU = reader.IsDBNull(4) ? null : reader.GetString(4),
                    IUDesignId = reader.IsDBNull(9) ? null : reader.GetString(9),
                    MUDesignId = reader.IsDBNull(10) ? null : reader.GetString(10),
                    AdditionalAccessGroup = reader.IsDBNull(15) ? null : reader.GetString(15),
                });
            }

            return new ProductConfigReply { ProductConfigList = configRequest };
        }
        var productConfigReply = new ProductConfigReply
        {
            ProductConfigList = new List<ProductConfig>
            {
                new ProductConfig
                {
                    Name = "NA",
                    PackageType = "NA",
                    PackageId = "NA",
                    TestProgram = "NA",
                    TIU = "NA",
                    IUDesignId = "NA",
                    MUDesignId = "NA",
                    AdditionalAccessGroup = "NA",
                    PPMR = "NA",
                    PPMO = "NA"
                }
            }
        };
        return productConfigReply;
    }
    public SvgReply QuerySvg(string tiuDesignID, string mUDesignID, string pkg, string prgnm, string tiuID)
    {
        string siteRoot = "D:\\SkyPF_repo\\pinfinder-sttd_skyline_pinfinder\\pinfinder_classic\\pinfinder_classic_web";
        string uniqueFolder = $"{pkg}_{prgnm}_{tiuID}";
        uniqueFolder = FileManagement.SanitizeFileName(uniqueFolder);
        string svgBottomPath = Path.Combine(siteRoot, "avacado", "static", "avacado", "data", "bottom", uniqueFolder);

        if (!Directory.Exists(svgBottomPath))
        {
            Directory.CreateDirectory(svgBottomPath);
        }
        else
        {
            FileManagement.EmptyDirectory(svgBottomPath);
        }

        string timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        string svgFileName = Path.Combine(svgBottomPath, $"0_{timeStamp}.svg");

        return SvgCreator(tiuDesignID, mUDesignID, svgFileName);
    }
    private SvgReply SvgCreator(string iuDesignID, string muDesignID, string svgFileName)
    {
        string context = string.Empty;

        var svgQuery = @"SELECT CONNECT_NUMBER, X, Y FROM [pinfinder_production].[dbo].[svg_location] WHERE IU_Design_ID = ? AND MU_Design_ID = ?";

        using var connection = new OdbcConnection(_odbcConnectionStringBuilder.ConnectionString);

        connection.Open();

        // Define command
        using var command = new OdbcCommand(svgQuery, connection);

        // Add parameters to the command
        command.Parameters.AddWithValue("@IU_Design_ID", iuDesignID);
        command.Parameters.AddWithValue("@MU_Design_ID", muDesignID);

        using var reader = command.ExecuteReader();

        List<SvgData> svgs = new List<SvgData>();
        while (reader.Read())
        {
            svgs.Add(new SvgData
            {
                CONNECT_NUMBER = reader.IsDBNull(0) ? null : reader.GetString(0),
                X = reader.IsDBNull(1) ? null : reader.GetDecimal(1).ToString(),
                Y = reader.IsDBNull(2) ? null : reader.GetDecimal(2).ToString(),
            });
        }

        // Finally create the svg file
        SvgGenerator(svgFileName, reader);

        return new SvgReply { SvgList = svgs, Context = context };
    }
    private void SvgGenerator(string svgFileName, OdbcDataReader reader)
    {
        // Default square dot color and size
        string squareColor = "black";
        int squareSize = 4;

        // Get the first character of the file name
        char firstChar = Path.GetFileName(svgFileName)[0];

        // Build the SVG ID
        StringBuilder svgIdBuilder = new StringBuilder("site");
        svgIdBuilder.Append(firstChar).Append("_tiu_bottom");
        string svgId = svgIdBuilder.ToString();
        string svgIdChild = svgId + "_child";

        // Create a StringBuilder to build the SVG content
        StringBuilder svgContent = new StringBuilder();

        // Write the SVG header
        svgContent.AppendLine("<svg version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" height=\"500\" width=\"650\">");
        svgContent.AppendLine($"<g id=\"{svgId}\" style=\"fill-opacity:1.0; stroke:black; stroke-width:1;\">");
        svgContent.AppendLine($"<g id=\"{svgIdChild}\">");
        svgContent.AppendLine("<rect id=\"nostrike\" x=\"0\" y=\"0\" height=\"500\" width=\"650\" fill=\"#fff\" style=\"fill-opacity:1.0; stroke:none;\"/>");

        // Loop through the result set and draw square dots for each coordinate
        while (reader.Read())
        {
            float x = reader.GetFloat(reader.GetOrdinal("X"));
            float y = reader.GetFloat(reader.GetOrdinal("Y"));
            string connectorId = reader.GetString(reader.GetOrdinal("CONNECT_NUMBER")).Trim();

            StringBuilder pinIdBuilder = new StringBuilder("pin_site");
            pinIdBuilder.Append(firstChar).Append("_");
            string pinId = pinIdBuilder.ToString() + connectorId;

            svgContent.AppendLine(string.Format("<rect id=\"{0}\" x=\"{1}\" y=\"{2}\" width=\"{3}\" height=\"{4}\" fill=\"{5}\"/>",
                pinId, x - squareSize / 2, y - squareSize / 2, squareSize, squareSize, squareColor));
        }

        // Close the SVG tags
        svgContent.AppendLine("</g></g></svg>");

        // Write the SVG content to a file
        File.WriteAllText(svgFileName, svgContent.ToString());
    }

    public ProductDetailReply QueryProductDetail(string pkg, string prgnm, string tiuID)
    {
        var headerquery = @"
    SELECT
        Tester_Channel_Num, Socket_ID, Socket_Signal_Name, Manufactured_Unit_Signal_Name, Connector_ID
    FROM
        [pinfinder_production].[dbo].[tiu_substrate]
    WHERE
        Package_ID_Search_Text = ? AND Test_Program_Search_Text = ? AND Interface_Unit_ID_Search_Text = ?
    ";

        using var connection = new OdbcConnection(_odbcConnectionStringBuilder.ConnectionString);

        connection.Open();

        // Define command
        using var command = new OdbcCommand(headerquery, connection);

        // Add parameters to the command
        command.Parameters.AddWithValue("@Package_ID_Search_Text", pkg);
        command.Parameters.AddWithValue("@Test_Program_Search_Text", prgnm);
        command.Parameters.AddWithValue("@Interface_Unit_ID_Search_Text", tiuID);

        using var reader = command.ExecuteReader();
        List<ProductDetail> productDetailRequest = new List<ProductDetail>();
        while (reader.Read())
        {
            productDetailRequest.Add(new ProductDetail
            {
                ChannelNumber = reader.IsDBNull(0) ? null : reader.GetInt32(0).ToString(),
                SocketID = reader.IsDBNull(1) ? null : reader.GetInt32(0).ToString(),
                IUNetName = reader.IsDBNull(2) ? null : reader.GetString(2),
                MUNetName = reader.IsDBNull(3) ? null : reader.GetString(3),
                ConnectorID = reader.IsDBNull(4) ? null : reader.GetString(4)
            });
        }

        return new ProductDetailReply { ProductDetailList = productDetailRequest };
    }

    public PostProcessingReply QueryPostProcessing(string config, string lotID, string opCdode, string summaryName)
    {
        var postProcessingQuery = @"
    SELECT
        Unit, [Test Name], [Failing Pin], [Output Path]
    FROM
        [pinfinder_production].[dbo].[post_processing]
    WHERE
        Config = ? AND Lot = ? AND Operation = ? AND Summary = ?
    ";

        using var connection = new OdbcConnection(_odbcConnectionStringBuilder.ConnectionString);

        connection.Open();

        // Define command
        using var command = new OdbcCommand(postProcessingQuery, connection);

        // Add parameters to the command
        command.Parameters.AddWithValue("@Config", config);
        command.Parameters.AddWithValue("@Lot", lotID);
        command.Parameters.AddWithValue("@Operation", opCdode);
        command.Parameters.AddWithValue("@Summary", summaryName);

        using var reader = command.ExecuteReader();
        List<PostProcessingData> productDetailRequest = new List<PostProcessingData>();
        while (reader.Read())
        {
            productDetailRequest.Add(new PostProcessingData
            {
                Unit = reader.IsDBNull(0) ? null : reader.GetString(0),
                TestName = reader.IsDBNull(1) ? null : reader.GetString(1),
                FailingPin = reader.IsDBNull(2) ? null : reader.GetString(2),
                OutputPath = reader.IsDBNull(3) ? null : reader.GetString(3),
            });
        }

        return new PostProcessingReply { PostProcessingList = productDetailRequest };
    }

    public LookupB8ConfigReply QueryLookupB8Config(bool request)
    {
        if (request)
        {
            var configquery = @"
            SELECT
            *
            FROM
                [pinfinder_production].[dbo].[Bin8_99_Lookup_Config]
            ";
            using var connection = new OdbcConnection(_odbcConnectionStringBuilder.ConnectionString);

            connection.Open();

            // Define command
            using var command = new OdbcCommand(configquery, connection);

            using var reader = command.ExecuteReader();

            List<LookupB8Config> configRequest = new List<LookupB8Config>();
            while (reader.Read())
            {
                configRequest.Add(new LookupB8Config
                {
                    Binning = reader.IsDBNull(0) ? null : reader.GetString(0),
                    PackageId = reader.IsDBNull(1) ? null : reader.GetString(1),
                    TestProgram = reader.IsDBNull(2) ? null : reader.GetString(2),
                    TIU = reader.IsDBNull(3) ? null : reader.GetString(3),
                    IUDesignId = reader.IsDBNull(4) ? null : reader.GetString(4),
                    MUDesignId = reader.IsDBNull(5) ? null : reader.GetString(5),
                });
            }

            return new LookupB8ConfigReply { LookupB8ConfigList = configRequest };
        }
        var productConfigReply = new LookupB8ConfigReply
        {
            LookupB8ConfigList = new List<LookupB8Config>
            {
                new LookupB8Config
                {
                    Binning = "NA",
                    PackageId = "NA",
                    TestProgram = "NA",
                    TIU = "NA",
                    IUDesignId = "NA",
                    MUDesignId = "NA",
                }
            }
        };
        return productConfigReply;
    }
}
