using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using Umbraco.Core.Models;

namespace AarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {
        //
        // GET: /ContactFormSurface/

        public ActionResult Index()
        {
            return PartialView("ContactForm", new AarhusWebDevCoop.Models.ContactFormViewModel());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(AarhusWebDevCoop.Models.ContactFormViewModel model)
        {

            if (!ModelState.IsValid) { return CurrentUmbracoPage(); }


            IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Comment");

            comment.SetValue("name", model.Name);
            comment.SetValue("email", model.Email);
            comment.SetValue("subject", model.Subject);
            comment.SetValue("message", model.Message);

            //Save
            Services.ContentService.Save(comment);


            MailMessage message = new MailMessage();
            message.To.Add("nakedtest2010@gmail.com");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message + "\n my email is: " + model.Email;

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("nakedtest2010@gmail.com", "naked2010");
                smtp.EnableSsl = true;

                smtp.Send(message);
                TempData["success"] = true;

            }



            return RedirectToCurrentUmbracoPage();

        }





    }
}
