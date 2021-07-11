using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ALTRGRPC.Models;
using Microsoft.EntityFrameworkCore;
using ALTRGRPC.Interfaces;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace ALTRgRPCTests
{
    [TestClass]
    public class PackageServiceTests
    {
        [TestMethod]
        public void TestGetRequestById()
        {
            var dbContextMock = new Mock<RepositoryContext>();
            var dbSetMock = new Mock<DbSet<Request>>();

            dbSetMock.Setup(s => s.FindAsync(It.IsAny<int>())).Returns(new ValueTask<Request>());
            dbContextMock.Setup(s => s.Set<Request>()).Returns(dbSetMock.Object);

            var requestRepository = new RequestRepository(dbContextMock.Object);
            var request = requestRepository.GetRequestByIdAsync(1).Result;

            Assert.IsNotNull(request);
        }
    }
}
