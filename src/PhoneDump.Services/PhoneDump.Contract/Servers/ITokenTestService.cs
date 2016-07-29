using System.Threading.Tasks;
using PhoneDump.Entity.Auth;

namespace PhoneDump.Contract.Servers
{
    public interface ITokenTestService
    {
        Task<TokenResult> TestToken(string token);
    }
}