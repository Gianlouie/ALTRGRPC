using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALTRGRPC.Interfaces;
using ALTRGRPC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ALTRGRPC.Models
{
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        public RequestRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<Request> GetRequestByIdAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Request>> GetAllRequestsAsync()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<Request> AddRequestAsync(Request request)
        {
            return await AddAsync(request);
        }

        public async Task<Request> UpdateRequestAsync(Request request)
        {
            return await UpdateAsync(request);
        }
    }
}
