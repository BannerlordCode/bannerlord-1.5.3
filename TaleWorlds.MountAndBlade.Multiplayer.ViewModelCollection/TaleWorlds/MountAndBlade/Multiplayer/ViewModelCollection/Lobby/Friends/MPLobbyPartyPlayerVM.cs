using System;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x02000058 RID: 88
	public class MPLobbyPartyPlayerVM : MPLobbyPlayerBaseVM
	{
		// Token: 0x060007D9 RID: 2009 RVA: 0x000196ED File Offset: 0x000178ED
		public MPLobbyPartyPlayerVM(PlayerId id, Action<MPLobbyPartyPlayerVM> onActivatePlayerActions)
			: base(id, "", null, null)
		{
			this._onActivatePlayerActions = onActivatePlayerActions;
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00019704 File Offset: 0x00017904
		private void ExecuteActivatePlayerActions()
		{
			this._onActivatePlayerActions(this);
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x00019712 File Offset: 0x00017912
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x0001971A File Offset: 0x0001791A
		[DataSourceProperty]
		public bool IsWaitingConfirmation
		{
			get
			{
				return this._isWaitingConfirmation;
			}
			set
			{
				if (value != this._isWaitingConfirmation)
				{
					this._isWaitingConfirmation = value;
					base.OnPropertyChangedWithValue(value, "IsWaitingConfirmation");
				}
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00019738 File Offset: 0x00017938
		// (set) Token: 0x060007DE RID: 2014 RVA: 0x00019740 File Offset: 0x00017940
		[DataSourceProperty]
		public bool IsPartyLeader
		{
			get
			{
				return this._isPartyLeader;
			}
			set
			{
				if (value != this._isPartyLeader)
				{
					this._isPartyLeader = value;
					base.OnPropertyChangedWithValue(value, "IsPartyLeader");
				}
			}
		}

		// Token: 0x04000397 RID: 919
		private Action<MPLobbyPartyPlayerVM> _onActivatePlayerActions;

		// Token: 0x04000398 RID: 920
		private bool _isWaitingConfirmation;

		// Token: 0x04000399 RID: 921
		private bool _isPartyLeader;
	}
}
