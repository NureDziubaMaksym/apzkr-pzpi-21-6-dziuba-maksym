using Backend.Core.DtoModels;
using Backend.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Interfaces
{
    public interface IFocusGroupRepository
    {
        ICollection<FocusGroup> GetFocusGroups();
        FocusGroup GetFocusGroupById(int id);
        bool FocusGroupExists(int id);
        void DeleteFocusGroup(int id);
        bool Save();
        ICollection<User> GetGroupUsers(int id);
        bool AddMemberToGroup(int userId, int groupId);
        bool DeleteFromGroup(int groupId, int userId);
        bool AddFocusGroup(FocusGroup focusGroup);
        bool UpdateFocusGroup(int id, UpdateFocusGroupDto updateFocusGroupDto);

    }
}
