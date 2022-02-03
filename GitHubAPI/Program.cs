using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace GitHubAPI
{
    class Program
    {
        private static IConfiguration configuration;

        static async Task Main(string[] args)
        {
            int counter = 1;
            var names = await Program.ApiResult();
            JArray result = JArray.Parse(names);

            Console.Write("Api calling");
            for (int i=0; i<3; i++)
            {
                Console.Write(".");
                System.Threading.Thread.Sleep(1200);
            }

            Console.WriteLine("\n");
           
            foreach (JObject item in result)
            {
                // getting repository names 
                string name = item.GetValue("name").ToString();
                Console.WriteLine(counter + "." + name);
                counter++;
            }
            Console.Read();
        }

        public static IConfiguration config()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration;
        }

        public static async Task<string> ApiResult()
        {
            configuration = Program.config();
            string url = "https://api.github.com/user/repos?affiliation=owner";
            var userName = configuration.GetSection("userName").Value;
            var password = configuration.GetSection("password").Value;

            var client = new RestClient();
            client.Authenticator = new HttpBasicAuthenticator(userName, password);
            var request = new RestRequest(url, Method.Get);
            var response = await client.ExecuteAsync(request);

            var names = response.Content;
            return names;
        }
    }
}
