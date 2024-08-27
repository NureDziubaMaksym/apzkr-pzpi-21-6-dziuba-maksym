using Backend.Core.DtoModels;
using Backend.Core.Interfaces;
using Backend.Core.Services;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Backend.Core.Repository
{
    public class FocusGroupRepository : IFocusGroupRepository
    {
        private readonly DataContext _dataContext;
        private readonly IdGenService _idGenService;

        public FocusGroupRepository(DataContext dataContext, IdGenService idGenService)
        {
            _dataContext = dataContext;
            _idGenService = idGenService;
        }

        public bool AddFocusGroup(FocusGroup focusGroup)
        {
            if (focusGroup == null)
            {
                return false;
            }

            focusGroup.FocGrId = _idGenService.GenerateNewId<FocusGroup>();

            _dataContext.FocusGroups.Add(focusGroup);
            return Save();
            
        }

        public bool AddMemberToGroup(int userId, int groupId)
        {
            var user = _dataContext.Users.FirstOrDefault(u => u.UserId == userId);
            var group = _dataContext.FocusGroups.FirstOrDefault(fg => fg.FocGrId == groupId);

            if (user == null || group == null)
            {
                return false;
            }

            if (group.Race != "none" && group.Race != user.Race)
            {
                throw new InvalidOperationException("User does not meet the racial requirements of the group.");
            }

            var userAgeCategory = GetAgeCategory(user.Age);

            if (group.Age != "none" && group.Age != userAgeCategory)
            {
                throw new InvalidOperationException("User does not meet the age requirements of the group.");
            }

            if (group.Gender != "none" && group.Gender != user.Gender)
            {
                throw new InvalidOperationException("User does not meet the gender requirements of the group.");
            }

            var alreadyInGroup = _dataContext.FillingGroups
                .Any(fg => fg.UserId == userId && fg.FocusId == groupId);

            if (alreadyInGroup)
            {
                return false;
            }

            var fillingGroup = new FillingGroup
            {
                UserId = userId,
                FocusId = groupId
            };

            _dataContext.FillingGroups.Add(fillingGroup);
            return Save();
        }

        public void DeleteFocusGroup(int id)
        {
            var focusGroup = _dataContext.FocusGroups.FirstOrDefault(fg => fg.FocGrId == id);
            if (focusGroup != null)
            {
                _dataContext.FocusGroups.Remove(focusGroup);
            }
            else
            {
                throw new InvalidOperationException("FocusGroup has not found");
            }
        }

        public bool DeleteFromGroup(int groupId, int userId)
        {
            var fillingGroup = _dataContext.FillingGroups
                .FirstOrDefault(fg => fg.FocusId == groupId && fg.UserId == userId);

            if (fillingGroup == null)
            {
                return false;
            }

            _dataContext.FillingGroups.Remove(fillingGroup);
            return Save();
        }

        public bool FocusGroupExists(int id)
        {
            return _dataContext.FocusGroups.Any(fg => fg.FocGrId == id);
        }

        public FocusGroup GetFocusGroupById(int id)
        {
            return _dataContext.FocusGroups.FirstOrDefault(fg => fg.FocGrId == id);
        }

        public ICollection<FocusGroup> GetFocusGroups()
        {
            return _dataContext.FocusGroups.OrderBy(fg => fg.FocGrId).ToList();
        }

        public ICollection<User> GetGroupUsers(int id)
        {
            return _dataContext.FillingGroups
                .Where(fg => fg.FocusId == id)
                .Select(fg => fg.User)
                .ToList();
        }

        public bool Save()
        {
            return _dataContext.SaveChanges() >= 0;
        }

        public bool UpdateFocusGroup(int id, UpdateFocusGroupDto updateFocusGroupDto)
        {
            var focusGroup = _dataContext.FocusGroups.FirstOrDefault(fg => fg.FocGrId == id);

            if (focusGroup == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(updateFocusGroupDto.Race))
            {
                focusGroup.Race = updateFocusGroupDto.Race;
            }

            if (!string.IsNullOrEmpty(updateFocusGroupDto.Age))
            {
                focusGroup.Age = updateFocusGroupDto.Age;
            }

            if (!string.IsNullOrEmpty(updateFocusGroupDto.Gender))
            {
                focusGroup.Gender = updateFocusGroupDto.Gender;
            }


            return Save();
        }

        private string GetAgeCategory(int age)
        {
            return age switch
            {
                < 13 => "U13",
                >= 14 and <= 19 => "teens",
                >= 20 and <= 29 => "young adults",
                >= 30 and <= 59 => "middle-aged",
                >= 60 and <= 69 => "old",
                _ => "elderly"
            };
        }

    }
}
