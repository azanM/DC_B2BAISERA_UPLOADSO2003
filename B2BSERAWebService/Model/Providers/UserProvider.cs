using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using B2BSERAWebService.Model.DataAccess;

namespace B2BSERAWebService.Model.Providers
{
    public class UserProvider : BaseProvider
    {
        private Repository repository;

        public UserProvider(Repository repository)
        {
            this.repository = repository;
        }

        public User GetUser(int userID)
        {
            return repository.GetByKey<User>(userID);
        }

        public int GetUserID(string userName)
        {
            return repository.Single<User>(user => user.UserName == userName).ID;
        }
    }
}