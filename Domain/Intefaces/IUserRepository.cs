using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Intefaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);

        Task<IEnumerable<User>> GetAllAsync();

        Task<User> CreateAsync(User user);

        Task<User> UpdateAsync(User user);

        Task<User> GetUserByEmailAsync(string email);
    }
}
