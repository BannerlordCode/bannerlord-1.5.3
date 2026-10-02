using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200007C RID: 124
	public class MissionSettlementPrepareLogic : MissionLogic
	{
		// Token: 0x06000520 RID: 1312 RVA: 0x00022936 File Offset: 0x00020B36
		public override void AfterStart()
		{
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign && Settlement.CurrentSettlement != null && (Settlement.CurrentSettlement.IsTown || Settlement.CurrentSettlement.IsCastle))
			{
				this.OpenGates();
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0002296C File Offset: 0x00020B6C
		private void OpenGates()
		{
			foreach (CastleGate castleGate in Mission.Current.ActiveMissionObjects.FindAllWithType<CastleGate>().ToList<CastleGate>())
			{
				castleGate.OpenDoorAndDisableGateForCivilianMission();
			}
		}
	}
}
