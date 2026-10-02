using System;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker.Targets
{
	// Token: 0x020000A0 RID: 160
	public class MissionPeerMarkerTargetVM : MissionMarkerTargetVM
	{
		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x0003105B File Offset: 0x0002F25B
		// (set) Token: 0x06000FB3 RID: 4019 RVA: 0x00031063 File Offset: 0x0002F263
		public MissionPeer TargetPeer { get; private set; }

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0003106C File Offset: 0x0002F26C
		public override Vec3 WorldPosition
		{
			get
			{
				MissionPeer targetPeer = this.TargetPeer;
				if (((targetPeer != null) ? targetPeer.ControlledAgent : null) != null)
				{
					return this.TargetPeer.ControlledAgent.Position + new Vec3(0f, 0f, this.TargetPeer.ControlledAgent.GetEyeGlobalHeight(), -1f);
				}
				Debug.FailedAssert("No target found!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\FlagMarker\\Targets\\MissionPeerMarkerTargetVM.cs", "WorldPosition", 27);
				return Vec3.One;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x000310E2 File Offset: 0x0002F2E2
		protected override float HeightOffset
		{
			get
			{
				return 0.75f;
			}
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x000310E9 File Offset: 0x0002F2E9
		public MissionPeerMarkerTargetVM(MissionPeer peer, bool isFriend)
			: base(MissionMarkerType.Peer)
		{
			this.TargetPeer = peer;
			this._isFriend = isFriend;
			base.Name = peer.DisplayedName;
			this.SetVisual();
			this.RefreshHealth();
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00031118 File Offset: 0x0002F318
		private void RefreshHealth()
		{
			MissionPeer targetPeer = this.TargetPeer;
			Agent agent = ((targetPeer != null) ? targetPeer.ControlledAgent : null);
			bool flag = MultiplayerSpectatorHelper.ShouldShowBothTeamsData() && agent != null && agent.IsActive();
			this.ShowHealth = flag;
			if (flag)
			{
				this.HealthLimit = agent.HealthLimit;
				this.CurrentHealth = agent.Health;
			}
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00031170 File Offset: 0x0002F370
		private void SetVisual()
		{
			string text = "#FFFFFFFF";
			if (NetworkMain.GameClient.IsInParty && NetworkMain.GameClient.PlayersInParty.Any<PartyPlayerInLobbyClient>((PartyPlayerInLobbyClient p) => p.PlayerId.Equals(this.TargetPeer.Peer.Id)))
			{
				text = "#00FF00FF";
			}
			else if (this._isFriend)
			{
				text = "#FFFF00FF";
			}
			else if (NetworkMain.GameClient.IsInClan && NetworkMain.GameClient.PlayersInClan.Any<ClanPlayer>((ClanPlayer p) => p.PlayerId.Equals(this.TargetPeer.Peer.Id)))
			{
				text = "#00FFFFFF";
			}
			uint num = TaleWorlds.Library.Color.ConvertStringToColor("#FFFFFFFF").ToUnsignedInteger();
			uint num2 = TaleWorlds.Library.Color.ConvertStringToColor(text).ToUnsignedInteger();
			base.RefreshColor(num, num2);
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0003121D File Offset: 0x0002F41D
		public override void UpdateScreenPosition(Camera missionCamera)
		{
			MissionPeer targetPeer = this.TargetPeer;
			if (((targetPeer != null) ? targetPeer.ControlledAgent : null) == null)
			{
				return;
			}
			this.RefreshHealth();
			base.UpdateScreenPosition(missionCamera);
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06000FBA RID: 4026 RVA: 0x00031241 File Offset: 0x0002F441
		// (set) Token: 0x06000FBB RID: 4027 RVA: 0x00031249 File Offset: 0x0002F449
		[DataSourceProperty]
		public bool ShowHealth
		{
			get
			{
				return this._showHealth;
			}
			set
			{
				if (value != this._showHealth)
				{
					this._showHealth = value;
					base.OnPropertyChangedWithValue(value, "ShowHealth");
				}
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x00031267 File Offset: 0x0002F467
		// (set) Token: 0x06000FBD RID: 4029 RVA: 0x0003126F File Offset: 0x0002F46F
		[DataSourceProperty]
		public float HealthLimit
		{
			get
			{
				return this._healthLimit;
			}
			set
			{
				if (value != this._healthLimit)
				{
					this._healthLimit = value;
					base.OnPropertyChangedWithValue(value, "HealthLimit");
				}
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x0003128D File Offset: 0x0002F48D
		// (set) Token: 0x06000FBF RID: 4031 RVA: 0x00031295 File Offset: 0x0002F495
		[DataSourceProperty]
		public float CurrentHealth
		{
			get
			{
				return this._currentHealth;
			}
			set
			{
				if (value != this._currentHealth)
				{
					this._currentHealth = value;
					base.OnPropertyChangedWithValue(value, "CurrentHealth");
				}
			}
		}

		// Token: 0x04000751 RID: 1873
		private const string _partyMemberColor = "#00FF00FF";

		// Token: 0x04000752 RID: 1874
		private const string _friendColor = "#FFFF00FF";

		// Token: 0x04000753 RID: 1875
		private const string _clanMemberColor = "#00FFFFFF";

		// Token: 0x04000754 RID: 1876
		private bool _isFriend;

		// Token: 0x04000755 RID: 1877
		private bool _showHealth;

		// Token: 0x04000756 RID: 1878
		private float _healthLimit;

		// Token: 0x04000757 RID: 1879
		private float _currentHealth;
	}
}
