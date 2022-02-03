using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace GitHubAPI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create a service collection for DI
            //var serviceCollection = new ServiceCollection();

            //build configuration
            IConfiguration configuration;
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json")
                .Build();

            string url = "https://api.github.com/user/repos?affiliation=owner";
            var userName = configuration.GetSection("userName").Value;
            var password = configuration.GetSection("password").Value;
            int counter = 1;

            var client = new RestClient();
            client.Authenticator = new HttpBasicAuthenticator(userName, password);

            var request = new RestRequest(url);
            var response = await client.ExecuteAsync(request);

            var names = response.Content;
            JArray result = JArray.Parse(names);
            foreach (JObject item in result)
            {
                string name = item.GetValue("name").
                    ToString();
                Console.WriteLine(counter + "." + name);
                counter++;
            }
            Console.Read();
        }
    }
}
