using System.Collections.Generic;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        public IEnumerable<Friend> GetAll()
        {
            //todo: load from database
            yield return new Friend { FirstName = "Francisco", LastName = "Lopez" };
            yield return new Friend { FirstName = "Irma", LastName = "Estrada" };
            yield return new Friend { FirstName = "Christopher", LastName = "Lopez" };
            yield return new Friend { FirstName = "Fidel", LastName = "Razo" };
        }
    }
}