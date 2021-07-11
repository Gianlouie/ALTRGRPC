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

                    while (true)
                    {
                        Console.WriteLine("Press 1 to get a new package or press anything else to exit");
                        var input = Console.ReadLine();
                        if (input == "1")
                        {
                            Func<Task<Package>> action = async () => await client.GetPackageAsync(new Empty { });
                            Func<Package, bool> validate = x => x != null;

                            reply = await Retry.DoAsync(action, validate);

                            if (reply != null)
                            {
                                Console.WriteLine(reply);
                            }
                            else
                            {
                                Console.WriteLine("Unable to get package");
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    shouldStop = true;
                }
                catch (RpcException re)
                {
                    Console.WriteLine("Unable to get package " + re.ToString());
                    shouldStop = false;
                }
                catch (AggregateException ae)
                {
                    Console.WriteLine(ae + "\nRetried to get package 10 times and still unable to retrieve it. \n ");
                    shouldStop = false;
                }
            }
        }
    }
}
