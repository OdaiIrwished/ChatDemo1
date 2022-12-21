using System.Threading.Tasks;

namespace ChatDemo1.Services
{
    public interface IChatHub
    {
       public Task SendMessage(string message, string UserId);
    }
}