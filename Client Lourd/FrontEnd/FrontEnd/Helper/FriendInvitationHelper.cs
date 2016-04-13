using System;
using System.Collections.Generic;
using System.Linq;
using FrontEnd.Model.ViewModel;
using FrontEndAccess.APIAccess;

namespace FrontEnd.Helper
{
    public static class FriendInvitationHelper
    {
        /// <summary>
        /// Will return all friends the player can invite
        /// </summary>
        /// <returns></returns>
        public static List<FriendInvitationItem> GetFriendInvitation()
        {
            var invitations = new List<FriendInvitationItem>();
            var friendsBasicInfo = ProfileAccess.Instance.GetAllPublicUserInfos().Where(x => x.AreFriend);
            foreach (var info in friendsBasicInfo)
            {
                try
                {
                    var model = ProfileAccess.Instance.GetUserInfoPlease(info.HashId);
                    var invitation = ModelConverter.ProfileModelConverter.ConvertToInvitation(model);
                    invitations.Add(invitation);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error creating invitation: ", e.Message);
                }

            }

            return invitations;
        }
    }
}