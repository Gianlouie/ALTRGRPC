using System;
using System.Threading.Tasks;
using Altr;
using Grpc.Core;
using Grpc.Net.Client;

namespace ALTRgRPCClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            bool shouldStop = false;
            while (!shouldStop)
            {
                try
                {
                   // The port number(5001) must match the port of the gRPC server.
                   using var channel = GrpcChannel.ForAddress("https://localhost:5001");

                    var client = new PackageService.PackageServiceClient(channel);
                    Package reply;

                    do
                    {
                        Console.WriteLine("Press 1 to get a new package or press 2 to exit");
                        if (Console.ReadLine() == "1")
                        {
                            reply = await client.GetPackageAsync(new Empty { });
                            if (reply != null)
                            {
                                Console.WriteLine(reply);
                            }
                            else
                            {
                                Console.WriteLine("Unable to get package");
                            }
                        }
                    }
                    while (Console.ReadLine() != "2");

                    shouldStop = true;
                }
                catch (RpcException re)
                {
                    Console.WriteLine("Unable to get package " + re.ToString());
                    shouldStop = false;
                }
            }
        }
    }
}
