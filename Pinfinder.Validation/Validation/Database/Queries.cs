using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using PinFinder.Core.Domain.Commands.Calls;
using PinFinder.Core.Domain.Commands.Replies;
using PinFinder.Core.Queries.Get;
using PinFinder.Database;
using MediatR;
using PinFinder.Core.Controllers;
using System.Diagnostics;
using System.Data.Odbc;

namespace PinFinder.Validation.Unit.Queries
{
    public static class DbConnectionInfo
    {
        public static OdbcConnectionStringBuilder ConnectionString()
        {
            var _odbc_rw = new OdbcConnectionStringBuilder()
            {
                Driver = "ODBC Driver 18 for SQL Server"
            };
            _odbc_rw.Add("SERVER", "sql2101-fm1-in.amr.corp.intel.com,3181");
            _odbc_rw.Add("DATABASE", "pinfinder_production");
            _odbc_rw.Add("UID", "pinfinder_pr_rw");
            _odbc_rw.Add("PWD", "6OsW32m4x3x68Qx");

            return _odbc_rw;
        }
    }
    public class GetItuffDataQueryHandlerTests
    {
        [Fact]
        public async Task ApplicationDbContext_QuerySite()
        {
            // Arrange
            var connection = DbConnectionInfo.ConnectionString();
            var dbContext = new ApplicationDbContext(connection);
            // Act
            var SiteReply = dbContext.QuerySite("J515G002", "6197", "KM");

            // Assert
            foreach (var Site in SiteReply.LotList)
            {
                Debug.WriteLine($"This is a lot: {Site.LotID}");
            }
            Assert.NotEmpty(SiteReply.LotList);
        }
        [Fact]
        public async Task ApplicationDbContext_QuerySummary()
        {
            // Arrange
            var connection = DbConnectionInfo.ConnectionString();
            var dbContext = new ApplicationDbContext(connection);
            // Act
            var SummaryReply = dbContext.QuerySummary("4440B199", "6262");

            // Assert
            foreach (var summary in SummaryReply.SummaryList)
            {
                Debug.WriteLine($"This is a summary: {summary.Summary}");
            }
            Assert.NotEmpty(SummaryReply.SummaryList);
        }
        [Fact]
        public async Task ApplicationDbContext_QueryHeader()
        {
            // Arrange
            var connection = DbConnectionInfo.ConnectionString();
            var dbContext = new ApplicationDbContext(connection);
            // Act
            var headerReply = dbContext.QueryHeader("4440B199", "6262", "1A");

            // Assert
            Debug.WriteLine($"this is TP: {headerReply.HeaderList[0].Prgnm}");
            Assert.NotEmpty(headerReply.HeaderList);
        }

        [Fact]
        public async Task ApplicationDbContext_QueryUnit()
        {
            // Arrange
            var connection = DbConnectionInfo.ConnectionString();
            var dbContext = new ApplicationDbContext(connection);
            // Act
            var unitReply = dbContext.QueryUnit("J515F846", "6208", "1A", "KM");

            // Assert
            Debug.WriteLine($"this is TIU ID: {unitReply.Units.Values.First().TiuId}");
            Assert.NotEmpty(unitReply.Units);

            Debug.WriteLine($"this is Channel: {unitReply.Units.Values.First().Tests[0].Channel}");
            Assert.NotEmpty(unitReply.Units);
        }
        [Fact]
        public async Task ApplicationDbContext_QueryTiuIdUnitData()
        {
            // Arrange
            var connection = DbConnectionInfo.ConnectionString();
            var dbContext = new ApplicationDbContext(connection);
            // Act
            var unitReply = dbContext.QueryTiuIdUnitData("EMRXCC10T4858", "KM", 14);

            // Assert
            Debug.WriteLine($"Number of rows in unitReply: {unitReply.Units.Count}");
            Assert.True(unitReply.Units.Count > 0, "unitReply.Units count should be greater than zero");

            Debug.WriteLine($"this is TIU ID: {unitReply.Units.Values.First().TiuId}");
            Assert.NotEmpty(unitReply.Units);

            Debug.WriteLine($"this is Channel: {unitReply.Units.Values.First().Tests[0].Channel}");
            Assert.NotEmpty(unitReply.Units);
        }
    }

    public class GetProductConfigDataQueryHandlerTests
    {
        [Fact]
        public async Task ApplicationDbContext_QueryProductConfig()
        {
            // Arrange
            var connection = DbConnectionInfo.ConnectionString();
            var dbContext = new ApplicationDbContext(connection);
            // Act
            var productConfigReply = dbContext.QueryProductConfig(true);

            // Assert
            Debug.WriteLine($"this is TIU: {productConfigReply.ProductConfigList[0].TIU}");
            Debug.WriteLine($"this is IU: {productConfigReply.ProductConfigList[0].IUDesignId}");
            Debug.WriteLine($"this is MU: {productConfigReply.ProductConfigList[0].MUDesignId}");
            Debug.WriteLine($"this is Additional Access Group: {productConfigReply.ProductConfigList[0].AdditionalAccessGroup}");
            Assert.NotEmpty(productConfigReply.ProductConfigList);
        }

        [Fact]
        public async Task ApplicationDbContext_QueryLookupB8Config()
        {
            // Arrange
            var connection = DbConnectionInfo.ConnectionString();
            var dbContext = new ApplicationDbContext(connection);
            // Act
            var productConfigReply = dbContext.QueryLookupB8Config(true);

            // Assert
            Debug.WriteLine($"this is TIU: {productConfigReply.LookupB8ConfigList[0].TIU}");
            Debug.WriteLine($"this is IU: {productConfigReply.LookupB8ConfigList[0].IUDesignId}");
            Debug.WriteLine($"this is MU: {productConfigReply.LookupB8ConfigList[0].MUDesignId}");
            Assert.NotEmpty(productConfigReply.LookupB8ConfigList);
        }
    }
    public class GetSvgDataQueryHandlerTests
    {
        [Fact]
        public async Task ApplicationDbContext_QuerySvg()
        {
            // Arrange
            var connection = DbConnectionInfo.ConnectionString();
            var dbContext = new ApplicationDbContext(connection);

            //var tiuDesignID = "640-0280R0";
            //var mUDesignID = "N47859-002";
            //var pkg = "28";
            //var prgnm = "SM4";
            //var tiuID = "BB2T_SM43C1";

            var tiuDesignID = "630-0065R0";
            var mUDesignID = "M40800-005";
            var pkg = "M8";
            var prgnm = "SPR____E";
            var tiuID = "SPRXCC";
            // Act
            var svgReply = dbContext.QuerySvg(tiuDesignID, mUDesignID, pkg, prgnm, tiuID);

            // Assert
            Debug.WriteLine($"this is one of X location: {svgReply.SvgList[0].X}");
            Debug.WriteLine($"this is one of Connect No: {svgReply.SvgList[0].CONNECT_NUMBER}");
            Assert.NotEmpty(svgReply.SvgList);
            Assert.DoesNotContain("error", svgReply.Context, StringComparison.OrdinalIgnoreCase);
        }
    }
    public class GetProductDetailDataQueryHandlerTests
    {
        [Fact]
        public async Task ApplicationDbContext_QueryProductDetail()
        {
            // Arrange
            var connection = DbConnectionInfo.ConnectionString();
            var dbContext = new ApplicationDbContext(connection);
            var pkg = "28";
            var prgnm = "SM4%";
            var tiuID = "BB2T%:SM43C1%";
            // Act
            var productDetailReply = dbContext.QueryProductDetail(pkg, prgnm, tiuID);

            // Assert
            Debug.WriteLine($"this is Channel: {productDetailReply.ProductDetailList[1].ChannelNumber}");
            Debug.WriteLine($"this is IU: {productDetailReply.ProductDetailList[2].IUNetName}");
            Debug.WriteLine($"this is MU: {productDetailReply.ProductDetailList[2].MUNetName}");
            Debug.WriteLine($"this is ConnectorID: {productDetailReply.ProductDetailList[4].ConnectorID}");
            Assert.NotEmpty(productDetailReply.ProductDetailList);
        }
    }
    public class GetPostProcessingDataQueryHandlerTests
    {
        [Fact]
        public async Task ApplicationDbContext_QueryProductDetail()
        {
            // Arrange
            var connection = DbConnectionInfo.ConnectionString();
            var dbContext = new ApplicationDbContext(connection);
            var config = "Z9_LNLM_BB2T_LNL";
            var lotID = "AX442T242";
            var operation = "6197";
            var summaryName = "1A";
            // Act
            var postProcessingReply = dbContext.QueryPostProcessing(config, lotID, operation, summaryName);

            // Assert
            Debug.WriteLine($"this is Channel: {postProcessingReply.PostProcessingList[0].Unit}");
            Debug.WriteLine($"this is IU: {postProcessingReply.PostProcessingList[0].TestName}");
            Debug.WriteLine($"this is MU: {postProcessingReply.PostProcessingList[0].FailingPin}");
            Debug.WriteLine($"this is ConnectorID: {postProcessingReply.PostProcessingList[0].OutputPath}");
            Assert.NotEmpty(postProcessingReply.PostProcessingList);
        }
    }
}