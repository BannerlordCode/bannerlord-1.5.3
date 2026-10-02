using System;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000065 RID: 101
	public class CampaignSiegeStateHandler : MissionLogic
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x000176C8 File Offset: 0x000158C8
		public bool IsSiege
		{
			get
			{
				return this._mapEvent.IsSiegeAssault;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x000176D5 File Offset: 0x000158D5
		public bool IsSallyOut
		{
			get
			{
				return this._mapEvent.IsSallyOut;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x000176E2 File Offset: 0x000158E2
		public Settlement Settlement
		{
			get
			{
				return this._mapEvent.MapEventSettlement;
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x000176EF File Offset: 0x000158EF
		public CampaignSiegeStateHandler()
		{
			this._mapEvent = PlayerEncounter.Battle;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00017702 File Offset: 0x00015902
		public override void OnRetreatMission()
		{
			this._isRetreat = true;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001770B File Offset: 0x0001590B
		public override void OnMissionResultReady(MissionResult missionResult)
		{
			this._defenderVictory = missionResult.BattleState == BattleState.DefenderVictory;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001771C File Offset: 0x0001591C
		public override void OnSurrenderMission()
		{
			PlayerEncounter.PlayerSurrender = true;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00017724 File Offset: 0x00015924
		protected override void OnEndMission()
		{
			if (this.IsSiege && this._mapEvent.PlayerSide == BattleSideEnum.Attacker && !this._isRetreat && !this._defenderVictory)
			{
				this.Settlement.SetNextSiegeState();
			}
		}

		// Token: 0x04000211 RID: 529
		private readonly MapEvent _mapEvent;

		// Token: 0x04000212 RID: 530
		private bool _isRetreat;

		// Token: 0x04000213 RID: 531
		private bool _defenderVictory;
	}
}
