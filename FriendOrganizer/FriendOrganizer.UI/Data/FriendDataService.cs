using System.Collections.Generic;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        public IEnumerable<Friend> GetAll()
        {
            //todo: load from database
            yield return new Friend { FirstName = "Francisco", LastName = "Lopez", Email = "pancho@mail.com" };
            yield return new Friend { FirstName = "Irma", LastName = "Estrada", Email = "irma@mail.com" };
            yield return new Friend { FirstName = "Christopher", LastName = "Lopez", Email = "chris@email.com" };
            yield return new Friend { FirstName = "Fidel", LastName = "Razo", Email = "fidel@mail.com" };
        }
    }
}