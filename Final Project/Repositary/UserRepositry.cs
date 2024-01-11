using Final_Project.Models.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace Final_Project.Repositary
{
    public class UserRepositry:Repository<IdentityUser>
    {
        private readonly DataContext db;

        public UserRepositry(DataContext _db):base(_db)
        {
            db = _db;
        }

        public string UploadFile(IFormFile image)
        {

            string uploadsFolder = Path.Combine("wwwroot", "Images");
            string ImageName = image.FileName; //Guid.NewGuid().ToString() + "_" +
            string filePath = Path.Combine(uploadsFolder, ImageName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }
            return ImageName;

        }

        public void SendMessage(string to, string subject, string body)
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

             //   ViewBag.Message = "Email sent successfully!";
            }
            catch (Exception ex)
            {
               // ViewBag.Error = $"Error sending email: {ex.Message}";
            }

           // Redirect to the appropriate view
        }





    }
}
