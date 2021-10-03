using System.Collections.Generic;
using System.Linq;
using WebApi.Data.Entities.Users;
using WebApi.Entities;

namespace WebApi.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users) 
        {
            return users?.Select(x => x.WithoutPassword());
        }

        public static User WithoutPassword(this User user) 
        {
            if (user == null) return null;

            user.Password = null;
            user.PasswordHash = null;
            user.PasswordSalt = null;
            return user;
        }
    }
}