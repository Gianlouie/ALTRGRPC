using ALTRGRPC.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALTRGRPC.Interfaces
{
    public interface IRequestRepository : IRepository<Request>
    {
        Task<Request> GetRequestByIdAsync(int id);

        Task<Request> AddRequestAsync(Request request);

        Task<List<Request>> GetAllRequestsAsync();

        Task<Request> UpdateRequestAsync(Request request);
    }
}
