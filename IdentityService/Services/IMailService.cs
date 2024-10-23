using IdentityService.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace IdentityService.Services
{
    public interface IMailService
    {
        Task Send(MailDto mailDto);
    }
}
