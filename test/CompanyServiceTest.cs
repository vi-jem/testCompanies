using Xunit;
using myrestful.Models;
using myrestful.Repositories;
using Moq;
using myrestful.Services;
using System.Threading.Tasks;

namespace myrestful.tests
{
    public class CompanyServiceTest
    {
        private readonly ICompanyService _service;

        private readonly Company aCompany;
        private readonly Company aCompanyWithoutData;
        private readonly SearchQuery searchQuery;

        public CompanyServiceTest()
        {
            aCompany = new Company{Name = "aaa", EstablishmentYear = 111};
            aCompanyWithoutData = new Company();
            searchQuery = new SearchQuery();

            Mock<ICompanyRepository> mockRepo = new Mock<ICompanyRepository>();
            mockRepo.Setup(repo => repo.GetById(1))
                .ReturnsAsync(new Company{ID = 1, Name = "zzz", EstablishmentYear = 2011});
            
            mockRepo.Setup(repo => repo.GetById(2))
                .ReturnsAsync(default(IEntity));

            mockRepo.Setup(repo => repo.Create(aCompany))
                .ReturnsAsync(new Entity{ID = 3});

            mockRepo.Setup(repo => repo.Create(aCompanyWithoutData))
                .ReturnsAsync(default(IEntity));                

            _service = new CompanyService(mockRepo.Object);
        }

        [Fact]
        public async Task GetCompany_success()
        {
            IEntity company = await _service.GetById(1);
            Assert.NotNull(company);
        }

        [Fact]
        public async Task GetCompany_non_existing()
        {
            IEntity company = await _service.GetById(2);
            Assert.Null(company);
        }

        //TODO: add more tests if services become more complex
    }
}
