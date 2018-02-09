using System.Collections.Generic;
using FriendOrganizer.Model;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository
    {
        Task<Friend> GetByIdAsync(int friendId);
        Task SaveAsync();
        bool HasChanges();
    }
}