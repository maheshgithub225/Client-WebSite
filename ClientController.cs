using System;
using System.Collections.Generic;
using System.Web.Mvc;

using HelloWorld.Models;
using Newtonsoft.Json;

namespace HelloWorld.Controllers
{
    public class ClientController : Controller
    {
        public ActionResult Index()
        {
            var clients = Client.GetClients();

            return View(clients);
        }
        public ActionResult Create()
        {
            ViewBag.Submitted = false;
            var created = false;

            if(HttpContext.Request.RequestType == "POST")
            {
                ViewBag.Submitted = true;

                var id = Request.Form["id"];
                var name = Request.Form["name"];
                var address = Request.Form["address"];

                bool trusted = false;

                if(Request.Form["trusted"] == "on")
                {
                    trusted = true;
                }

                Client client = new Client()
                {
                    ID = Convert.ToInt16(id),
                    Name = name,
                    Address = address,
                    Trusted = trusted                    
                };

                var ClientFile = Client.ClientData;
                var ClientData = System.IO.File.ReadAllText(ClientFile);

                List<Client> ClientList = new List<Client>();
                ClientList = JsonConvert.DeserializeObject<List<Client>>(ClientData);
                if(ClientList == null)
                {
                    ClientList = new List<Client>();
                }
                ClientList.Add(client);

                System.IO.File.WriteAllText(ClientFile, JsonConvert.SerializeObject(ClientList));

                created = true;

               
            }
            if (created)
            {
                ViewBag.Message = "Client Created Successfully";
            }
            else
            {
                ViewBag.Message = "Client Creation Failed";
            }

            return View();
        }

        public ActionResult Update(int id)
        {
            if(HttpContext.Request.RequestType == "POST")
            {
                var name = Request.Form["name"];
                var address = Request.Form["address"];
                var trusted = Request.Form["trusted"];

                var existingclients = Client.GetClients();

                foreach(Client client in existingclients)
                {
                    if(client.ID == id)
                    {
                        client.Name = name;
                        client.Address = address;
                        client.Trusted = Convert.ToBoolean(trusted);
                        break;
                    }
                }

                System.IO.File.WriteAllText(Client.ClientData, JsonConvert.SerializeObject(existingclients));

                Response.Redirect("~/Client/Index?Message = Client_Updated");

            }

            var clientinfo = new Client();
            var existingClients = Client.GetClients();
            
            foreach(Client client in existingClients)
            {
                if(client.ID == id)
                {
                    clientinfo = client;
                    break;
                }
            }

            if(clientinfo == null)
            {
                ViewBag.Message = "No client found with given details";
            }
            return View(clientinfo);
        }

        public ActionResult Delete(int id)
        {
            var Clients = Client.GetClients();
            var deleted = false;
            foreach(Client client in Clients)
            {
                if(client.ID == id)
                {
                    var clientIndex = Clients.IndexOf(client);
                    Clients.RemoveAt(clientIndex);

                    System.IO.File.WriteAllText(Client.ClientData, JsonConvert.SerializeObject(Clients));
                    deleted = true;
                    break;
                }
            }

            if(deleted)
            {
                ViewBag.Message = "Client Deleted Successfully";
            }
            else
            {
                ViewBag.Message = "Error Occured";
            }

            return View();
        }
    }
}