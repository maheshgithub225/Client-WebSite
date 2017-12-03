using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace HelloWorld.Models
{
    public class Client
    {
        public static string ClientData = HttpContext.Current.Server.MapPath("~/App_Data/Client.json");

        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool Trusted { get; set; }

        public static List<Client> GetClients()
        {
            List<Client> clients = new List<Client>();

            if(File.Exists(ClientData))
            {
                string content = File.ReadAllText(ClientData);
                clients = JsonConvert.DeserializeObject<List<Client>>(content);
                return clients;
            }
            else
            {
                File.Create(ClientData).Close();
                File.WriteAllText(ClientData, "[]");

                GetClients();
            }

            return clients;
        }
    }
}