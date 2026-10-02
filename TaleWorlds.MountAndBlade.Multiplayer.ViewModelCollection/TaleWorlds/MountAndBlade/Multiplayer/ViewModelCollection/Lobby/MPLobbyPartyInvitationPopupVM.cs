using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x0200002E RID: 46
	public class MPLobbyPartyInvitationPopupVM : ViewModel
	{
		// Token: 0x06000368 RID: 872 RVA: 0x0000C940 File Offset: 0x0000AB40
		public MPLobbyPartyInvitationPopupVM()
		{
			this.RefreshValues();
			this.MaxAnswerDuration = 60f;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000C959 File Offset: 0x0000AB59
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Title = new TextObject("{=QDNcl3DH}Party Invitation", null).ToString();
			this.Message = new TextObject("{=AaAcmalE}You've been invited to join a party by", null).ToString();
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000C98D File Offset: 0x0000AB8D
		public void OpenWith(PlayerId invitingPlayerID)
		{
			this.RemainingAnswerDuration = this.MaxAnswerDuration;
			this.InvitingPlayer = new MPLobbyPlayerBaseVM(invitingPlayerID, "", null, null);
			this.IsEnabled = true;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000C9B5 File Offset: 0x0000ABB5
		public void Close()
		{
			if (this.IsEnabled)
			{
				this.ExecuteDecline();
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000C9C5 File Offset: 0x0000ABC5
		public void OnTick(float dt)
		{
			if (this.IsEnabled)
			{
				this.RemainingAnswerDuration -= dt;
				if (this.RemainingAnswerDuration <= 0f)
				{
					this.ExecuteDecline();
				}
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000C9F0 File Offset: 0x0000ABF0
		private void ExecuteAccept()
		{
			this.IsEnabled = false;
			NetworkMain.GameClient.AcceptPartyInvitation();
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000CA03 File Offset: 0x0000AC03
		private void ExecuteDecline()
		{
			this.IsEnabled = false;
			NetworkMain.GameClient.DeclinePartyInvitation();
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0000CA16 File Offset: 0x0000AC16
		// (set) Token: 0x06000370 RID: 880 RVA: 0x0000CA1E File Offset: 0x0000AC1E
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000371 RID: 881 RVA: 0x0000CA3C File Offset: 0x0000AC3C
		// (set) Token: 0x06000372 RID: 882 RVA: 0x0000CA44 File Offset: 0x0000AC44
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000373 RID: 883 RVA: 0x0000CA67 File Offset: 0x0000AC67
		// (set) Token: 0x06000374 RID: 884 RVA: 0x0000CA6F File Offset: 0x0000AC6F
		[DataSourceProperty]
		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				if (value != this._message)
				{
					this._message = value;
					base.OnPropertyChangedWithValue<string>(value, "Message");
				}
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000375 RID: 885 RVA: 0x0000CA92 File Offset: 0x0000AC92
		// (set) Token: 0x06000376 RID: 886 RVA: 0x0000CA9A File Offset: 0x0000AC9A
		[DataSourceProperty]
		public MPLobbyPlayerBaseVM InvitingPlayer
		{
			get
			{
				return this._invitingPlayer;
			}
			set
			{
				if (value != this._invitingPlayer)
				{
					this._invitingPlayer = value;
					base.OnPropertyChangedWithValue<MPLobbyPlayerBaseVM>(value, "InvitingPlayer");
				}
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000377 RID: 887 RVA: 0x0000CAB8 File Offset: 0x0000ACB8
		// (set) Token: 0x06000378 RID: 888 RVA: 0x0000CAC0 File Offset: 0x0000ACC0
		[DataSourceProperty]
		public float RemainingAnswerDuration
		{
			get
			{
				return this._remainingAnswerDuration;
			}
			set
			{
				if (value != this._remainingAnswerDuration)
				{
					this._remainingAnswerDuration = value;
					base.OnPropertyChangedWithValue(value, "RemainingAnswerDuration");
				}
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000379 RID: 889 RVA: 0x0000CADE File Offset: 0x0000ACDE
		// (set) Token: 0x0600037A RID: 890 RVA: 0x0000CAE6 File Offset: 0x0000ACE6
		[DataSourceProperty]
		public float MaxAnswerDuration
		{
			get
			{
				return this._maxAnswerDuration;
			}
			set
			{
				if (value != this._maxAnswerDuration)
				{
					this._maxAnswerDuration = value;
					base.OnPropertyChangedWithValue(value, "MaxAnswerDuration");
				}
			}
		}

		// Token: 0x040001BD RID: 445
		private bool _isEnabled;

		// Token: 0x040001BE RID: 446
		private float _remainingAnswerDuration;

		// Token: 0x040001BF RID: 447
		private float _maxAnswerDuration;

		// Token: 0x040001C0 RID: 448
		private string _title;

		// Token: 0x040001C1 RID: 449
		private string _message;

		// Token: 0x040001C2 RID: 450
		private MPLobbyPlayerBaseVM _invitingPlayer;
	}
}
