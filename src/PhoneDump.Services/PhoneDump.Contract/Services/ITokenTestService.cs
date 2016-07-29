using System.Threading.Tasks;
using PhoneDump.Entity.Auth;

namespace PhoneDump.Contract.Services
{
    public interface ITokenTestService
    {
        Task<TokenResult> TestToken(string token);
    }
}