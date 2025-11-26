using System;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BBURGERClone.Models;

namespace BBURGERClone.Controllers
{
    public class ContactController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _siteContactTo;

        public ContactController(IConfiguration config)
        {
            _config = config;
            // email address to receive contact messages (set in appsettings)
            _siteContactTo = _config["Contact:To"] ?? "support@bburgerclone.com";
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ContactForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                // Validation failed, return view with errors
                return View(model);
            }

            // Try sending email if SMTP configured
            var smtpHost = _config["Smtp:Host"];
            if (!string.IsNullOrEmpty(smtpHost))
            {
                try
                {
                    var smtpPort = int.TryParse(_config["Smtp:Port"], out var p) ? p : 587;
                    var smtpUser = _config["Smtp:User"];
                    var smtpPass = _config["Smtp:Pass"];
                    var enableSsl = bool.TryParse(_config["Smtp:EnableSsl"], out var s) ? s : true;
                    var fromAddress = _config["Smtp:From"] ?? smtpUser ?? "noreply@bburgerclone.com";

                    var mail = new MailMessage();
                    mail.From = new MailAddress(fromAddress, "BBURGER Clone");
                    mail.To.Add(new MailAddress(_siteContactTo));
                    mail.ReplyToList.Add(new MailAddress(model.Email));
                    mail.Subject = $"Contact form: {model.Name}";
                    mail.IsBodyHtml = false;
                    mail.Body = $"Name: {model.Name}\nEmail: {model.Email}\n\nMessage:\n{model.Message}\n\nSent: {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC";

                    using var client = new SmtpClient(smtpHost, smtpPort)
                    {
                        EnableSsl = enableSsl
                    };

                    if (!string.IsNullOrEmpty(smtpUser))
                    {
                        client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    }

                    client.Send(mail);

                    TempData["ContactSuccess"] = "Thanks! Your message was sent. We'll contact you soon.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log exception (for now, write to TempData so you can see it during dev)
                    // In production send to logging infrastructure (Serilog, ILogger etc.)
                    TempData["ContactError"] = "There was an error sending your message. Please try again later.";
                    // Optionally store ex.Message in logs, but don't display stack traces to users.
                    Console.Error.WriteLine("Contact email send error: " + ex.Message);
                    return View(model);
                }
            }

            // If SMTP not configured, just acknowledge (you may store the message to DB instead)
            TempData["ContactSuccess"] = "Thanks! Your message has been received. We'll contact you soon.";
            return RedirectToAction("Index");
        }
    }
}
