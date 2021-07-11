using System;
using System.Threading.Tasks;
using Altr;
using Grpc.Net.Client;

namespace ALTRgRPCClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var client = new PackageService.PackageServiceClient(channel);
            var reply = await client.GetPackageAsync(new Empty { });

            Console.WriteLine(reply);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
