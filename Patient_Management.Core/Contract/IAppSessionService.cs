using System.Threading.Tasks;

namespace Patient_Management.Core.Contract
{
    public interface IAppSessionService
    {
        Task<bool> CreateSession(string sessionId, string username);
        void DeleteSession(string username);
        Task<bool> ValidateSession(string sessionId, string username);
    }
}
