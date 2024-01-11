using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace Final_Project.Controllers
{
    public class EmailController : Controller
    {
        [HttpGet]
        public IActionResult SendMessage()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SendMessage(string to, string subject, string body)
        {
            try
            {
                
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("A.Zakarya19.870@gmail.com", "xfjvazpjmsqjpjhi"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("A.Zakarya19.870@gmail.com"),//base Email
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(to);

                smtpClient.Send(mailMessage);

                ViewBag.Message = "Email sent successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error sending email: {ex.Message}";
            }

            return View(); // Redirect to the appropriate view
        }





    }
}
