using Grpc.Net.Client;
//using PinFinderSkyPF.Protos;
using System.Threading.Tasks;

//namespace PinFinderSkyPF.Client.Services
//{
    // Creating a Shared Service for gRPC call
    //public class GrpcService
    //{
    //    private readonly GrpcService.GrpcServiceClient client_;

    //    public GrpcService()
    //    {
    //        // gRPC client is created from a channel, so use the channel to create a gRPC client
    //        var channel = GrpcChannel.ForAddress("https://localhost:5001"); // Port number must match the port of the gRPC server Albert create.
    //        
    //        // service called need to change and follow the gRPC service from .proto file
    //        client_ = new Service.ServiceClient(channel);
    //    }

    //    public async Task<PinFinderResponse> GetDataAsync(PinFinderRequest request)
    //    {
    //        return await _client.PinFinderMethodAsync(request);
    //    }

    //}
//}
