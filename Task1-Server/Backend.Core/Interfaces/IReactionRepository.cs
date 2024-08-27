using Backend.Core.DtoModels;
using Backend.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Interfaces
{
    public interface IReactionRepository
    {
        ICollection<Reaction> GetReactions();
        void ReactionDelete(int id);
        bool ReactionExsists(int id);
        bool Save();
        ICollection<Reaction> ReactionsByUserId(int id);
        ICollection<Reaction> ReactionsByContentId(int id);
        ICollection<Reaction> ReactionsByGroup(int groupId, int contentId);
        bool AddReaction(Reaction reaction);
        Reaction GetReactionById(int id);
        bool UpdateReaction(int id, UpdateReactionDto updateReactionDto);
    }
}
