using Library.Application.Common;
using Library.Domain.Entities;

namespace Library.Application.Interfaces;

public interface ITokenService
{
    TokenResult GenerateToken(SystemUser user, IEnumerable<string> roles);
}
