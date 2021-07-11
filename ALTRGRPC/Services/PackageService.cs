using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Altr;
using static Altr.PackageService;
using System;
using ALTRGRPC.Models;
using System.Net;
using ALTRGRPC.Interfaces;
using System.Linq;
using System.Collections.Generic;

namespace ALTRGRPC
{
    public class PackageService : PackageServiceBase
    {
        private readonly ILogger<PackageService> _logger;
        private readonly IRequestRepository _requestRepository;

        public PackageService(ILogger<PackageService> logger, IRequestRepository requestRepository)
        {
            _logger = logger;
            _requestRepository = requestRepository;
        }

        public override async Task<Package> GetPackage(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Getting Package");

            var serviceRequest = new Request()
            {
                IpAddress = GetIpAddress(),
                RequestTime = DateTime.Now,
                Serviced = false
            };

            bool proceed = CheckIpAddressAttempts(serviceRequest);

            if (proceed)
            {
                serviceRequest.Serviced = true;

                await AddRequestAsync(serviceRequest);

                return await Task.FromResult(new Package
                {
                    Name = "package",
                    Version = "current version",
                    License = "current license",
                    Repository = "my repository"
                });
            }
            else
            {
                serviceRequest.Serviced = false;

                await AddRequestAsync(serviceRequest);

                return await Task.FromResult<Package>(null);
            }

        }

        /// <summary>
        /// Determine if the requesting client has exceeded the request limit of 3 requests
        /// per IP Address per 30 seconds. If the limit has exceeded, the gRPC service is expected
        /// to reject the request. If the limit is not exceeded, the gRPC service is expected to
        /// successfully service the request. 
        /// </summary>
        private bool CheckIpAddressAttempts(Request serviceRequest)
        {
            var results = GetAllServicedRequestsWithinLast30SecondsByIpAddressAsync(serviceRequest.IpAddress).Result;

            if (results.Count < 3)
                return true;

            return false;
        }

        private string GetIpAddress()
        {
            string hostName = Dns.GetHostName();
            string IpAddress = Dns.GetHostEntry(hostName).AddressList[0].ToString();
            
            return IpAddress;
        }

        private async Task<List<Request>> GetAllServicedRequestsWithinLast30SecondsByIpAddressAsync(string ipAddress)
        {
            var now = DateTime.Now;
            var halfMinute = now.AddSeconds(30);

            var results = await _requestRepository.GetAllRequestsAsync();

            return results.Where(r => r.Serviced && r.RequestTime < halfMinute && r.IpAddress == ipAddress).ToList();
        }

        private async Task<Request> AddRequestAsync(Request request)
        {
            return await _requestRepository.AddRequestAsync(request);
        }
    }
}
