using System.Collections.Generic;

public class ProductStateService
{
    public string Package { get; set; }
    public string ProgramName { get; set; }
    public string TIUID { get; set; }

    public List<ProductDetailsModel> ProductDetails { get; set; } = new List<ProductDetailsModel>();

    public class ProductDetailsModel
    {
        public string ChannelNum { get; set; } = string.Empty;
        public string SocketId { get; set; } = string.Empty;
        public string TIUName { get; set; } = string.Empty;
        public string MUName { get; set; } = string.Empty;
        public string Connector { get; set; } = string.Empty;
    }
}