using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000471 RID: 1137
	public class VillageHealCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060049C0 RID: 18880 RVA: 0x001718B4 File Offset: 0x0016FAB4
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
		}

		// Token: 0x060049C1 RID: 18881 RVA: 0x001718D0 File Offset: 0x0016FAD0
		private void DailyTickSettlement(Settlement settlement)
		{
			if ((settlement.IsVillage || settlement.IsTown) && settlement.SettlementHitPoints < 1f && settlement.Party.MapEvent == null && settlement.Party.SiegeEvent == null)
			{
				float num = (7000f - MathF.Min(7000f, MathF.Max(1000f, settlement.MapFaction.CurrentTotalStrength))) / 100000f;
				ExplainedNumber explainedNumber = new ExplainedNumber(0.06f + num, false, null);
				if (settlement.IsVillage && settlement.Village.TradeBound != null)
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Medicine.CleanInfrastructure, settlement.Village.TradeBound.Town, false, ref explainedNumber);
				}
				if (settlement.OwnerClan.Leader.GetPerkValue(DefaultPerks.Roguery.InBestLight))
				{
					explainedNumber.AddFactor(DefaultPerks.Roguery.InBestLight.SecondaryBonus, DefaultPerks.Roguery.InBestLight.Name);
				}
				IncreaseSettlementHealthAction.Apply(settlement, explainedNumber.ResultNumber);
			}
		}

		// Token: 0x060049C2 RID: 18882 RVA: 0x001719CE File Offset: 0x0016FBCE
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
