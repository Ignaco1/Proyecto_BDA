using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
