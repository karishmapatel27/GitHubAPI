using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace GitHubAPI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // base url
            string url = "https://api.github.com/users/karishmapatel27/repos";
   
            var client = new RestClient();
            var request = new RestRequest(url, Method.Get);
            var response = await client.ExecuteAsync(request);
            var names = response.Content; // was using toSting() here 
            JArray o = JArray.Parse(names);
      

            foreach (JObject item in o)
            {
                string name = item.GetValue("name").ToString();
                Console.WriteLine(name);
            }
            Console.ReadLine();
        }


    }
}
