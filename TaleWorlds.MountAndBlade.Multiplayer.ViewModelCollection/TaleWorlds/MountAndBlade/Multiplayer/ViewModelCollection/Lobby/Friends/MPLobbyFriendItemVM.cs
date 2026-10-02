using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x02000054 RID: 84
	public class MPLobbyFriendItemVM : MPLobbyPlayerBaseVM
	{
		// Token: 0x06000737 RID: 1847 RVA: 0x00016DFD File Offset: 0x00014FFD
		public MPLobbyFriendItemVM(PlayerId ID, Action<MPLobbyPlayerBaseVM> onActivatePlayerActions, Action<PlayerId> onInviteToClan = null, Action<PlayerId> onFriendRequestAnswered = null)
			: base(ID, string.Empty, onInviteToClan, onFriendRequestAnswered)
		{
			this._onActivatePlayerActions = onActivatePlayerActions;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00016E15 File Offset: 0x00015015
		private void ExecuteActivatePlayerActions()
		{
			this._onActivatePlayerActions(this);
		}

		// Token: 0x0400035C RID: 860
		private Action<MPLobbyPlayerBaseVM> _onActivatePlayerActions;
	}
}
