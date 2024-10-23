using OrderService.Dtos;
using System.Threading.Tasks;

namespace OrderService.Services
{
    public interface IMailService
    {
        Task Send(MailDto mailDto);
    }
}
