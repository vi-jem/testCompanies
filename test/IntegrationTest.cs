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
            IConfigurationRoot configuration = new ConfigurationBuilder()
                                    .SetBasePath(Path.GetFullPath(@"../../../../src/"))
                                    .Build();
            

            IWebHostBuilder builder = new WebHostBuilder()
                            .UseEnvironment("Debug")
                            .UseStartup<Startup>()
                            .UseConfiguration(configuration);
            TestServer server = new TestServer(builder);
            this._context = server.Host.Services.GetService(typeof(DBContextEmployees)) as DBContextEmployees;
            this._client = server.CreateClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Basic YWxhZGRpbjpvcGVuc2VzYW1l");
        }

        [Fact]
        public async Task CreateGetSearchUpdateDelete()
        {
            //create company
            Company companyToCreate = new Company{Name = "zzz", EstablishmentYear = 1212};
            HttpContent createContent = new StringContent(JsonConvert.SerializeObject(companyToCreate), Encoding.UTF8, "application/json");
            HttpResponseMessage createResponse = await _client.PostAsync($"/company/create", createContent);
            string jsonResponse = await createResponse.Content.ReadAsStringAsync();
            long id = JsonConvert.DeserializeObject<Entity>(jsonResponse).ID;

            //get company by id
            HttpResponseMessage response = await _client.GetAsync($"/company/{id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            jsonResponse = await response.Content.ReadAsStringAsync();
            Company valueResponse = JsonConvert.DeserializeObject<Company>(jsonResponse);

            Assert.Equal(id, valueResponse.ID);

            //search company
            SearchQuery searchQuery = new SearchQuery{Keyword = "zzz"};
            HttpContent searchContent = new StringContent(JsonConvert.SerializeObject(searchQuery), Encoding.UTF8, "application/json");
            HttpResponseMessage searchResponse = await _client.PostAsync($"/company/search", searchContent);
            jsonResponse = await searchResponse.Content.ReadAsStringAsync();
            SearchResult resultResponse = JsonConvert.DeserializeObject<SearchResult>(jsonResponse);
            Assert.NotEmpty(resultResponse.Results);

            //update company
            companyToCreate.Name = "aaa";
            companyToCreate.ID = id;
            HttpContent updateContent = new StringContent(JsonConvert.SerializeObject(companyToCreate), Encoding.UTF8, "application/json");
            HttpResponseMessage updateResponse = await _client.PutAsync($"/company/update/{id}", updateContent);
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            //get company and verify namechange
            HttpResponseMessage getResponse = await _client.GetAsync($"/company/{id}");
            jsonResponse = await getResponse.Content.ReadAsStringAsync();
            valueResponse = JsonConvert.DeserializeObject<Company>(jsonResponse);
            Assert.Equal(companyToCreate.Name, valueResponse.Name);

            //delete company
            HttpResponseMessage deleteResponse = await _client.DeleteAsync($"/company/delete/{id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            //get company
            getResponse = await _client.GetAsync($"/company/{id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
