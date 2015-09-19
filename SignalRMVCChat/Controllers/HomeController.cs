using Microsoft.AspNet.SignalR;
using SignalRMVCChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRMVCChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadPicture(string message, string name)
        {
            // Save picture to server
            // Get Signal R Context
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            var file = Request.Files["picture"];

            if (file != null)
            {
                var filename = new Random().Next() + file.FileName;
                var path = "~/Content/UploadedFiles/" + filename;
                file.SaveAs(Server.MapPath(path));

                // Send to all Clients
                context.Clients.All.addNewMessageToPage(name, message, "/Content/UploadedFiles/" + filename);
                return Content("success");
            }

            if (message != null && message.Length > 0)
            {
                // Send to all Clients
                context.Clients.All.addNewMessageToPageNoPic(name, message);
                return Content("success");
            }
            
            return Content("");
        }
    }
}