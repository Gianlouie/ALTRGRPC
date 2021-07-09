using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Altr;
using static Altr.PackageService;

namespace ALTRGRPC
{
    public class PackageService : PackageServiceBase
    {
        private readonly ILogger<PackageService> _logger;
        public PackageService(ILogger<PackageService> logger)
        {
            _logger = logger;
        }

        /*public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }*/

        public override Task<Package> GetPackage(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Getting Package");
            return Task.FromResult(new Package
            { 
                Name = "package",
                Version = "current version",
                License = "current license",
                Repository = "my repository"
            });

        }
    }
}
