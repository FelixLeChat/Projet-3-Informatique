using System;
using FrontEnd.Game;
using FrontEnd.Model.ViewModel;
using Models.Database;

namespace FrontEnd.ModelConverter
{
    public static class ProfileModelConverter
    {
        public static SessionParticipant ConvertToParticipant(ProfileModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var converted = new SessionParticipant()
            {
                Player = ConvertToPlayer(model),
                CurrentState = SessionParticipant.ParticipantState.Setuping
            };

            return converted;
        }

        public static FriendInvitationItem ConvertToInvitation(ProfileModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var converted = new FriendInvitationItem()
            {
                Player = ConvertToPlayer(model)
            };

            return converted;
        }

        public static Model.Player ConvertToPlayer(ProfileModel model)
        {
            var converted = new Model.Player()
            {
                UserName = model.Username,
                HashId = model.UserHashId,
                Picture = model.Picture,
                PrincessTitle = model.PrincessTitle,
            };
            return converted;
        }

        
    }
}