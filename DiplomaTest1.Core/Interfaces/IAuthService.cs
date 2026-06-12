using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiplomaTest1.Core.DTOs.Auth;

namespace DiplomaTest1.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAcync(LoginDto dto);
    }
}
