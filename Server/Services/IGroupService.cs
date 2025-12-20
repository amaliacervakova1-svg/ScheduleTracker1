using Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IGroupService
    {
        Task<List<Group>> GetAllGroupsAsync();
        Task<Group?> GetGroupByIdAsync(int id);
        Task<Group> AddGroupAsync(Group group);
        Task<bool> DeleteGroupAsync(int id);
        Task<bool> GroupExistsAsync(int id);
        Task<bool> GroupHasLessonsAsync(int id);
        Task<List<Lesson>> GetGroupLessonsAsync(int id);
        Task<Group?> UpdateGroupAsync(int id, Group updatedGroup);
    }
}