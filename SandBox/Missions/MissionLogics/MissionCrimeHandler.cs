using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000078 RID: 120
	public class MissionCrimeHandler : MissionLogic
	{
		// Token: 0x060004D5 RID: 1237 RVA: 0x0001F06C File Offset: 0x0001D26C
		protected override void OnEndMission()
		{
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.IsFortification)
			{
				IFaction mapFaction = Settlement.CurrentSettlement.MapFaction;
				if (!Hero.MainHero.IsPrisoner && !Campaign.Current.IsMainHeroDisguised && !mapFaction.IsBanditFaction && Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingSevere(mapFaction.MapFaction))
				{
					Campaign.Current.GameMenuManager.SetNextMenu("fortification_crime_rating");
				}
			}
		}
	}
}
