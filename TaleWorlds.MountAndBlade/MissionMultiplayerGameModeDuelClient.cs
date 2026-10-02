using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.MissionRepresentatives;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B7 RID: 695
	public class MissionMultiplayerGameModeDuelClient : MissionMultiplayerGameModeBaseClient
	{
		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06002744 RID: 10052 RVA: 0x0009160A File Offset: 0x0008F80A
		public override bool IsGameModeUsingGold
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06002745 RID: 10053 RVA: 0x0009160D File Offset: 0x0008F80D
		public override bool IsGameModeTactical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06002746 RID: 10054 RVA: 0x00091610 File Offset: 0x0008F810
		public override bool IsGameModeUsingRoundCountdown
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002747 RID: 10055 RVA: 0x00091613 File Offset: 0x0008F813
		public override bool IsGameModeUsingAllowCultureChange
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06002748 RID: 10056 RVA: 0x00091616 File Offset: 0x0008F816
		public override bool IsGameModeUsingAllowTroopChange
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06002749 RID: 10057 RVA: 0x00091619 File Offset: 0x0008F819
		public override MultiplayerGameType GameType
		{
			get
			{
				return MultiplayerGameType.Duel;
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x0600274A RID: 10058 RVA: 0x0009161C File Offset: 0x0008F81C
		public bool IsInDuel
		{
			get
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				bool? flag;
				if (component == null)
				{
					flag = null;
				}
				else
				{
					Team team = component.Team;
					flag = ((team != null) ? new bool?(team.IsDefender) : null);
				}
				return flag ?? false;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x0600274B RID: 10059 RVA: 0x00091673 File Offset: 0x0008F873
		// (set) Token: 0x0600274C RID: 10060 RVA: 0x0009167B File Offset: 0x0008F87B
		public DuelMissionRepresentative MyRepresentative { get; private set; }

		// Token: 0x0600274D RID: 10061 RVA: 0x00091684 File Offset: 0x0008F884
		private void OnMyClientSynchronized()
		{
			this.MyRepresentative = GameNetwork.MyPeer.GetComponent<DuelMissionRepresentative>();
			Action onMyRepresentativeAssigned = this.OnMyRepresentativeAssigned;
			if (onMyRepresentativeAssigned != null)
			{
				onMyRepresentativeAssigned();
			}
			this.MyRepresentative.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x000916B3 File Offset: 0x0008F8B3
		public override int GetGoldAmount()
		{
			return 0;
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x000916B6 File Offset: 0x0008F8B6
		public override void OnGoldAmountChangedForRepresentative(MissionRepresentativeBase representative, int goldAmount)
		{
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x000916B8 File Offset: 0x0008F8B8
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			base.MissionNetworkComponent.OnMyClientSynchronized += this.OnMyClientSynchronized;
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x000916D7 File Offset: 0x0008F8D7
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			base.MissionNetworkComponent.OnMyClientSynchronized -= this.OnMyClientSynchronized;
			if (this.MyRepresentative != null)
			{
				this.MyRepresentative.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
			}
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x0009170A File Offset: 0x0008F90A
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
			if (this.MyRepresentative != null)
			{
				this.MyRepresentative.CheckHasRequestFromAndRemoveRequestIfNeeded(affectedAgent.MissionPeer);
			}
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x00091734 File Offset: 0x0008F934
		public override bool CanRequestCultureChange()
		{
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			return ((missionPeer != null) ? missionPeer.Team : null) != null && missionPeer.Team.IsAttacker;
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x00091770 File Offset: 0x0008F970
		public override bool CanRequestTroopChange()
		{
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			return ((missionPeer != null) ? missionPeer.Team : null) != null && missionPeer.Team.IsAttacker;
		}

		// Token: 0x04000EE2 RID: 3810
		public Action OnMyRepresentativeAssigned;
	}
}
