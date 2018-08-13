using FriendOrganizer.Core.Models;

namespace FriendOrganizer.UI.Wrapper
{
    public class FriendPhoneNumberWrapper:ModelWrapper<FriendPhoneNumber>
    {
        public string Number
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public FriendPhoneNumberWrapper(FriendPhoneNumber model) : base(model)
        {
        }
    }
}