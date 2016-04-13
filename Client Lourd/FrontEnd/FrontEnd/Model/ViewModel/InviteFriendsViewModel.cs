using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FrontEnd.Game;
using FrontEnd.Helper;
using FrontEnd.ViewModel.Base;

namespace FrontEnd.Model.ViewModel
{
    /// <summary>
    /// View model pass to the Invite Friends pop-up
    /// </summary>
    public class InviteFriendsViewModel : ObservableObject
    {
        public ObservableCollection<FriendInvitationItem> FriendsInvitation { get; set; }

        public InviteFriendsViewModel(List<FriendInvitationItem> friendsInvitation)
        {
            FriendsInvitation = new ObservableCollection<FriendInvitationItem>(friendsInvitation);
        }

    }
}