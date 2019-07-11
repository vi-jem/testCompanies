using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using myrestful.Infrastructure;
using myrestful.Models;
using Newtonsoft.Json;
using Xunit;

namespace myrestful.tests
{
    public class IntegrationTest
    {
        private readonly HttpClient _client;
        private readonly DBContextEmployees _context;
        public IntegrationTest()
        {
            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Path.GetFullPath(@"../../../../src/"))
                                    .Build();
            

            var builder = new WebHostBuilder()
                            .UseEnvironment("Debug")
                            .UseStartup<Startup>()
                            .UseConfiguration(configuration);
            var server = new TestServer(builder);
            this._context = server.Host.Services.GetService(typeof(DBContextEmployees)) as DBContextEmployees;
            this._client = server.CreateClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Basic YWxhZGRpbjpvcGVuc2VzYW1l");
        }

        [Fact]
        public async Task CreateAndGetCompany()
        {
            Company companyToCreate = new Company{Name = "zzz", EstablishmentYear = 1212};
            var createContent = new StringContent(JsonConvert.SerializeObject(companyToCreate), Encoding.UTF8, "application/json");
            HttpResponseMessage createResponse = await _client.PostAsync($"/company/create", createContent);

            var newId = JsonConvert.DeserializeObject<Entity>(await createResponse.Content.ReadAsStringAsync());

            long id = newId.ID;

            var response = await _client.GetAsync($"/company/{id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var valueResponse = JsonConvert.DeserializeObject<Company>(jsonResponse);

            Assert.Equal(id, valueResponse.ID);
        }

        [Fact]
        public async Task SearchUpdateAndDeleteCompany()
        {
            SearchQuery query = new SearchQuery{Keyword = "zzz"};
            

        }
    }
}
