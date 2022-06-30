using DOC_RASCH.Data.Entities;
using DOC_RASCH.Models;
using System;
using System.Threading.Tasks;

namespace DOC_RASCH.Helpers
{
    public interface IConverterHelper
    {
        Task<User> ToUserAsync(UserViewModel model, Guid imageId, bool isNew);

        UserViewModel ToUserViewModel(User user);

    }
}