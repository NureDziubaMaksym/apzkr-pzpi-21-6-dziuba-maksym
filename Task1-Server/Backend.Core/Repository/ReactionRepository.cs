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

namespace Backend.Core.Repository
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly DataContext _dataContext;
        private readonly IdGenService _idGenService;

        public ReactionRepository(DataContext dataContext, IdGenService idGenService)
        {
            _dataContext = dataContext;
            _idGenService = idGenService;
        }

        public bool AddReaction(Reaction reaction)
        {
            if (reaction == null)
            {
                return false;
            }

            reaction.ReactionId = _idGenService.GenerateNewId<Reaction>();

            _dataContext.Reactions.Add(reaction);
            return Save();
        }

        public Reaction GetReactionById(int id)
        {
            return _dataContext.Reactions.FirstOrDefault(r => r.ReactionId == id);
        }

        public ICollection<Reaction> GetReactions()
        {
            return _dataContext.Reactions.OrderBy(r => r.ReactionId).ToList();
        }

        public void ReactionDelete(int id)
        {
            var reaction = _dataContext.Reactions.FirstOrDefault(r => r.ReactionId == id);
            if (reaction != null)
            {
                _dataContext.Reactions.Remove(reaction);
            }
        }

        public bool ReactionExsists(int id)
        {
            return _dataContext.Reactions.Any(r => r.ReactionId == id);
        }

        public ICollection<Reaction> ReactionsByContentId(int id)
        {
            return _dataContext.Reactions
            .Where(r => r.ContentId == id)
            .OrderBy(r => r.ReactionId)
            .ToList();
        }

        public ICollection<Reaction> ReactionsByGroup(int groupId, int contentId)
        {
            // Получаем пользователей фокус-группы
            var userIds = _dataContext.FillingGroups
                .Where(fg => fg.FocusId == groupId)
                .Select(fg => fg.UserId)
                .ToList();

            // Получаем реакции для указанных пользователей и контента
            return _dataContext.Reactions
                .Where(r => userIds.Contains(r.UserId) && r.ContentId == contentId)
                .OrderBy(r => r.ReactionId)
                .ToList();
        }

        public ICollection<Reaction> ReactionsByUserId(int id)
        {
            return _dataContext.Reactions
            .Where(r => r.UserId == id)
            .OrderBy(r => r.ReactionId)
            .ToList();
        }

        public bool Save()
        {
            return _dataContext.SaveChanges() >= 0;
        }

        public bool UpdateReaction(int id, UpdateReactionDto updateReactionDto)
        {
            var reaction = _dataContext.Reactions.FirstOrDefault(r => r.ReactionId == id);

            if (reaction == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(updateReactionDto.Comment))
            {
                reaction.Comment = updateReactionDto.Comment;
            }

            if (updateReactionDto.Grade.HasValue)
            {
                reaction.Grade = updateReactionDto.Grade.Value;
            }

            if (updateReactionDto.InterestRate.HasValue)
            {
                reaction.InterestRate = updateReactionDto.InterestRate.Value;
            }

            return Save();
        }
    }
}
