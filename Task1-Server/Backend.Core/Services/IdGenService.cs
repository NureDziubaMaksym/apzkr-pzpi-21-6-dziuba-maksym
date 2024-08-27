using Backend.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Core.Services
{
    public class IdGenService
    {
        private readonly Random _random;
        private readonly DataContext _dataContext;

        public IdGenService(DataContext dataContext)
        {
            _random = new Random();
            _dataContext = dataContext;
        }

        public int GenerateNewId<T>() where T : class
        {
            int minId = 1;
            int maxId = CalculateMaxId<T>();
            HashSet<int> existingIds = new HashSet<int>(GetExistingIds<T>());

            int newId;
            do
            {
                newId = _random.Next(minId, maxId + 1);
            } while (existingIds.Contains(newId));

            return newId;
        }

        private int CalculateMaxId<T>() where T : class
        {
            int currentMaxId = GetExistingIds<T>().Max();

            if (currentMaxId == 0)
            {
                return 99; // Если нет существующих ID, начнем с двухзначных
            }

            int numDigits = currentMaxId.ToString().Length;

            // Вычисляем максимум для текущего количества цифр
            int maxForCurrentDigits = (int)Math.Pow(10, numDigits) - 1;

            if (currentMaxId < maxForCurrentDigits)
            {
                return maxForCurrentDigits;
            }

            // Если все двухзначные заняты, увеличиваем диапазон
            return (int)Math.Pow(10, numDigits + 1) - 1;
        }

        private IEnumerable<int> GetExistingIds<T>() where T : class
        {
            var property = typeof(T).GetProperties().FirstOrDefault(p => p.Name.EndsWith("Id"));
            if (property == null)
            {
                throw new InvalidOperationException("Сущность не имеет поля с именем, заканчивающимся на 'Id'");
            }

            return _dataContext.Set<T>().Select(e => (int)property.GetValue(e));
        }
    }
}

