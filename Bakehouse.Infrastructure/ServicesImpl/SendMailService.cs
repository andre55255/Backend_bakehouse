using Bakehouse.Communication.ViewObjects.Email;
using Bakehouse.Core.Entities;
using Bakehouse.Helpers;
using FluentResults;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Bakehouse.Core.ServicesInterface;
using Bakehouse.Core.RepositoriesInterface;

namespace Bakehouse.Infrastructure.ServicesImpl
{
    public class SendMailService : ISendMailService
    {
        private readonly IApplicationUserRepository _userRepo;
        private readonly IConfigurationRepository _configRepo;
        private readonly ILogService _logService;

        public SendMailService(IApplicationUserRepository userRepo, ILogService logService, IConfigurationRepository configRepo)
        {
            _userRepo = userRepo;
            _logService = logService;
            _configRepo = configRepo;
        }

        public async Task<Result> SendMailForgotPassword(EmailDataVO dataVO)
        {
            try
            {
                ApplicationUser user = await _userRepo.FindByEmailAsync(dataVO.Email);
                SendMailVO active = new SendMailVO();
                active.Body = LoadTemplate("ResetPassword.html");
                active.Destination = dataVO.Email;
                if (user == null || string.IsNullOrWhiteSpace(active.Body) || string.IsNullOrWhiteSpace(active.Destination))
                    return Result.Fail("Erro ao enviar email de recuperação de senha");
                active.Subject = string.Concat("Email de recuperação de senha: ", user.Name, " ", user.LastName);
                active.Body = active.Body.Replace("[[NAME]]", user.Name);
                active.Body = active.Body.Replace("[[LINK]]", dataVO.Link);
                return await SendMail(active);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsEmail.ErrorSendMail, this.GetType().ToString());
                return Result.Fail(ConstantsEmail.ErrorSendMail);
            }
        }

        public async Task<Result> SendMailUserConfirmation(EmailDataVO dataVO)
        {
            try
            {
                ApplicationUser user = await _userRepo.FindByEmailAsync(dataVO.Email);
                SendMailVO active = new SendMailVO();
                active.Body = LoadTemplate("ConfirmAccount.html");
                active.Destination = dataVO.Email;
                if (user == null || string.IsNullOrWhiteSpace(active.Body) || string.IsNullOrWhiteSpace(active.Destination))
                    return Result.Fail("Erro ao enviar email de confirmação de conta");
                active.Subject = string.Concat("Email de confirmação: ", user.Name, " ", user.LastName);
                active.Body = active.Body.Replace("[[NAME]]", user.Name);
                active.Body = active.Body.Replace("[[LINK]]", dataVO.Path);
                return await SendMail(active);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsEmail.ErrorSendMail, this.GetType().ToString());
                return Result.Fail(ConstantsEmail.ErrorSendMail);
            }
        }

        private async Task<Result> SendMail(SendMailVO em)
        {
            MailMessage mail = new MailMessage();
            try
            {
                List<Configuration> configs = await _configRepo.FindAllAsync();
                string sourceEmail = configs.Where(x => x.Token == "SMTP_Login").Select(x => x.Value).FirstOrDefault();
                string passEmail = configs.Where(x => x.Token == "SMTP_Password").Select(x => x.Value).FirstOrDefault();
                string portSmtp = configs.Where(x => x.Token == "SMTP_Port").Select(x => x.Value).FirstOrDefault();
                string hostSmtp = configs.Where(x => x.Token == "SMTP_Server").Select(x => x.Value).FirstOrDefault();

                mail.From = new MailAddress(sourceEmail);
                mail.To.Add(em.Destination);
                mail.Subject = em.Subject;
                mail.Body = em.Body;
                mail.IsBodyHtml = false;
                mail.Priority = MailPriority.Normal;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential(sourceEmail, passEmail);
                smtp.Host = hostSmtp;
                smtp.Port = int.Parse(portSmtp);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsEmail.ErrorSendMail, this.GetType().ToString());
                return Result.Fail(ConstantsEmail.ErrorSendMail);
            }
            finally
            {
                mail.Dispose();
            }
        }

        private string LoadTemplate(string path)
        {
            try
            {
                string Local = Path.Combine(Path.GetFullPath("Content/Template"), path);
                using (StreamReader Reader = new StreamReader(Local))
                {
                    return Reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsEmail.ErrorLoadTemplate, this.GetType().ToString());
                return null;
            }
        }
    }
}