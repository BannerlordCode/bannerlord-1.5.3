using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D3 RID: 723
	public abstract class MissionRepresentativeBase : PeerComponent
	{
		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06002A06 RID: 10758 RVA: 0x0009E85D File Offset: 0x0009CA5D
		protected MissionRepresentativeBase.PlayerTypes PlayerType
		{
			get
			{
				if (!base.Peer.Communicator.IsNetworkActive)
				{
					return MissionRepresentativeBase.PlayerTypes.Bot;
				}
				if (!base.Peer.Communicator.IsServerPeer)
				{
					return MissionRepresentativeBase.PlayerTypes.Client;
				}
				return MissionRepresentativeBase.PlayerTypes.Server;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06002A07 RID: 10759 RVA: 0x0009E888 File Offset: 0x0009CA88
		// (set) Token: 0x06002A08 RID: 10760 RVA: 0x0009E890 File Offset: 0x0009CA90
		public Agent ControlledAgent { get; private set; }

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06002A09 RID: 10761 RVA: 0x0009E89C File Offset: 0x0009CA9C
		// (set) Token: 0x06002A0A RID: 10762 RVA: 0x0009E8DC File Offset: 0x0009CADC
		public int Gold
		{
			get
			{
				if (this._gold < 0)
				{
					return this._gold;
				}
				bool flag;
				MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.UnlimitedGold, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out flag);
				if (!flag)
				{
					return this._gold;
				}
				return 2000;
			}
			private set
			{
				if (value < 0)
				{
					this._gold = value;
					return;
				}
				bool flag;
				MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.UnlimitedGold, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out flag);
				this._gold = ((!flag) ? value : 2000);
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06002A0B RID: 10763 RVA: 0x0009E91A File Offset: 0x0009CB1A
		public MissionPeer MissionPeer
		{
			get
			{
				if (this._missionPeer == null)
				{
					this._missionPeer = base.GetComponent<MissionPeer>();
				}
				return this._missionPeer;
			}
		}

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06002A0C RID: 10764 RVA: 0x0009E938 File Offset: 0x0009CB38
		// (remove) Token: 0x06002A0D RID: 10765 RVA: 0x0009E970 File Offset: 0x0009CB70
		public event Action OnGoldUpdated;

		// Token: 0x06002A0F RID: 10767 RVA: 0x0009E9AD File Offset: 0x0009CBAD
		public void SetAgent(Agent agent)
		{
			this.ControlledAgent = agent;
			if (this.ControlledAgent != null)
			{
				this.ControlledAgent.SetMissionRepresentative(this);
				this.OnAgentSpawned();
			}
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x0009E9D0 File Offset: 0x0009CBD0
		public virtual void OnAgentSpawned()
		{
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x0009E9D2 File Offset: 0x0009CBD2
		public virtual void Tick(float dt)
		{
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x0009E9D4 File Offset: 0x0009CBD4
		public void UpdateGold(int gold)
		{
			this.Gold = gold;
			Action onGoldUpdated = this.OnGoldUpdated;
			if (onGoldUpdated == null)
			{
				return;
			}
			onGoldUpdated();
		}

		// Token: 0x0400101B RID: 4123
		private int _gold;

		// Token: 0x0400101C RID: 4124
		private MissionPeer _missionPeer;

		// Token: 0x020005BC RID: 1468
		protected enum PlayerTypes
		{
			// Token: 0x04001F6E RID: 8046
			Bot,
			// Token: 0x04001F6F RID: 8047
			Client,
			// Token: 0x04001F70 RID: 8048
			Server
		}
	}
}
