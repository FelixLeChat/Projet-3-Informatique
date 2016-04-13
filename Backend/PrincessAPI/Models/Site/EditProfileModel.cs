using Models;
using Models.Database;

namespace PrincessAPI.Models.Site
{
    public class EditProfileModel
    {
        public string Description { get; set; }
        public EnumsModel.PrincessAvatar Avatar { get; set; }
        public bool IsPrivate { get; set; }
        public string ErrorMessage { get; set; }

        public EditProfileModel() { }

        public EditProfileModel(ProfileModel model)
        {
            Description = model.Description;
            Avatar = model.Picture;
            IsPrivate = model.IsPrivate;
        }

        public ProfileModel updatedProfile(ProfileModel currentProfile)
        {
            currentProfile.Description = Description;
            currentProfile.Picture = Avatar;
            currentProfile.IsPrivate = IsPrivate;
            return currentProfile;
        }
    }


}