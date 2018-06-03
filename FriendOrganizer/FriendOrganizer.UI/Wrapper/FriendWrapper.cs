using System;
using System.CodeDom;
using System.Collections.Generic;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Wrapper
{
    public class FriendWrapper : ModelWrapper<Friend>
    {
        public int Id => Model.Id;
        public string FirstName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string LastName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int? FavoriteLanguageId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public FriendWrapper(Friend model) : base(model)
        {
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, "robot", StringComparison.InvariantCultureIgnoreCase))
                    {
                        yield return "Robot is not a valid friend.";
                    }
                    break;
            }
        }
    }
}