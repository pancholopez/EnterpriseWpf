using FriendOrganizer.Core.Models;

namespace FriendOrganizer.UI.Wrapper
{
    public class ProgrammingLanguageWrapper : ModelWrapper<ProgrammingLanguage>
    {
        public int Id => Model.Id;

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ProgrammingLanguageWrapper(ProgrammingLanguage model) : base(model)
        {
        }
    }
}