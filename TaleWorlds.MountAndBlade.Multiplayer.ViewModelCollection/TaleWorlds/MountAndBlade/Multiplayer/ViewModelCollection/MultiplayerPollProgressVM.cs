using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000015 RID: 21
	public class MultiplayerPollProgressVM : ViewModel
	{
		// Token: 0x06000116 RID: 278 RVA: 0x000059EE File Offset: 0x00003BEE
		public MultiplayerPollProgressVM()
		{
			this.Keys = new MBBindingList<InputKeyItemVM>();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005A04 File Offset: 0x00003C04
		public void OnKickPollOpened(MissionPeer initiatorPeer, MissionPeer targetPeer, bool isBanRequested)
		{
			this.TargetPlayer = new MPPlayerVM(targetPeer);
			this.PollInitiatorName = initiatorPeer.DisplayedName;
			GameTexts.SetVariable("ACTION", isBanRequested ? MultiplayerPollProgressVM._banText : MultiplayerPollProgressVM._kickText);
			this.PollDescription = new TextObject("{=qyuhC21P}wants to {ACTION}", null).ToString();
			this.VotesAccepted = 0;
			this.VotesRejected = 0;
			this.AreKeysEnabled = NetworkMain.GameClient.PlayerID != targetPeer.Peer.Id;
			this.HasOngoingPoll = true;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005A8D File Offset: 0x00003C8D
		public void OnPollUpdated(int votesAccepted, int votesRejected)
		{
			this.VotesAccepted = votesAccepted;
			this.VotesRejected = votesRejected;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005A9D File Offset: 0x00003C9D
		public void OnPollClosed()
		{
			this.HasOngoingPoll = false;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005AA6 File Offset: 0x00003CA6
		public void OnPollOptionPicked()
		{
			this.AreKeysEnabled = false;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005AAF File Offset: 0x00003CAF
		public void AddKey(GameKey key)
		{
			this.Keys.Add(InputKeyItemVM.CreateFromGameKey(key, false));
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00005AC3 File Offset: 0x00003CC3
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00005ACB File Offset: 0x00003CCB
		[DataSourceProperty]
		public bool HasOngoingPoll
		{
			get
			{
				return this._hasOngoingPoll;
			}
			set
			{
				if (value != this._hasOngoingPoll)
				{
					this._hasOngoingPoll = value;
					base.OnPropertyChangedWithValue(value, "HasOngoingPoll");
				}
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00005AE9 File Offset: 0x00003CE9
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00005AF1 File Offset: 0x00003CF1
		[DataSourceProperty]
		public bool AreKeysEnabled
		{
			get
			{
				return this._areKeysEnabled;
			}
			set
			{
				if (value != this._areKeysEnabled)
				{
					this._areKeysEnabled = value;
					base.OnPropertyChangedWithValue(value, "AreKeysEnabled");
				}
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00005B0F File Offset: 0x00003D0F
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00005B17 File Offset: 0x00003D17
		[DataSourceProperty]
		public int VotesAccepted
		{
			get
			{
				return this._votesAccepted;
			}
			set
			{
				if (this._votesAccepted != value)
				{
					this._votesAccepted = value;
					base.OnPropertyChangedWithValue(value, "VotesAccepted");
				}
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00005B35 File Offset: 0x00003D35
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00005B3D File Offset: 0x00003D3D
		[DataSourceProperty]
		public int VotesRejected
		{
			get
			{
				return this._votesRejected;
			}
			set
			{
				if (this._votesRejected != value)
				{
					this._votesRejected = value;
					base.OnPropertyChangedWithValue(value, "VotesRejected");
				}
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00005B5B File Offset: 0x00003D5B
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00005B63 File Offset: 0x00003D63
		[DataSourceProperty]
		public string PollInitiatorName
		{
			get
			{
				return this._pollInitiatorName;
			}
			set
			{
				if (this._pollInitiatorName != value)
				{
					this._pollInitiatorName = value;
					base.OnPropertyChangedWithValue<string>(value, "PollInitiatorName");
				}
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00005B86 File Offset: 0x00003D86
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00005B8E File Offset: 0x00003D8E
		[DataSourceProperty]
		public string PollDescription
		{
			get
			{
				return this._pollDescription;
			}
			set
			{
				if (this._pollDescription != value)
				{
					this._pollDescription = value;
					base.OnPropertyChangedWithValue<string>(value, "PollDescription");
				}
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00005BB1 File Offset: 0x00003DB1
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00005BB9 File Offset: 0x00003DB9
		[DataSourceProperty]
		public MPPlayerVM TargetPlayer
		{
			get
			{
				return this._targetPlayer;
			}
			set
			{
				if (value != this._targetPlayer)
				{
					this._targetPlayer = value;
					base.OnPropertyChangedWithValue<MPPlayerVM>(value, "TargetPlayer");
				}
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00005BD7 File Offset: 0x00003DD7
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00005BDF File Offset: 0x00003DDF
		[DataSourceProperty]
		public MBBindingList<InputKeyItemVM> Keys
		{
			get
			{
				return this._keys;
			}
			set
			{
				if (this._keys != value)
				{
					this._keys = value;
					base.OnPropertyChangedWithValue<MBBindingList<InputKeyItemVM>>(value, "Keys");
				}
			}
		}

		// Token: 0x04000097 RID: 151
		private static readonly TextObject _kickText = new TextObject("{=gk5dCG1j}kick", null);

		// Token: 0x04000098 RID: 152
		private static readonly TextObject _banText = new TextObject("{=sFDrUfNR}ban", null);

		// Token: 0x04000099 RID: 153
		private bool _hasOngoingPoll;

		// Token: 0x0400009A RID: 154
		private bool _areKeysEnabled;

		// Token: 0x0400009B RID: 155
		private int _votesAccepted;

		// Token: 0x0400009C RID: 156
		private int _votesRejected;

		// Token: 0x0400009D RID: 157
		private string _pollInitiatorName;

		// Token: 0x0400009E RID: 158
		private string _pollDescription;

		// Token: 0x0400009F RID: 159
		private MPPlayerVM _targetPlayer;

		// Token: 0x040000A0 RID: 160
		private MBBindingList<InputKeyItemVM> _keys;
	}
}
