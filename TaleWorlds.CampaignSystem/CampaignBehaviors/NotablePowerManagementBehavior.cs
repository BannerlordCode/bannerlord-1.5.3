using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200043E RID: 1086
	public class NotablePowerManagementBehavior : CampaignBehaviorBase
	{
		// Token: 0x060045E8 RID: 17896 RVA: 0x00153934 File Offset: 0x00151B34
		public override void RegisterEvents()
		{
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.RaidCompletedEvent.AddNonSerializedListener(this, new Action<BattleSideEnum, RaidEventComponent>(this.OnRaidCompleted));
		}

		// Token: 0x060045E9 RID: 17897 RVA: 0x00153986 File Offset: 0x00151B86
		private void OnHeroCreated(Hero hero, bool isMaternal)
		{
			if (hero.IsNotable)
			{
				hero.AddPower((float)Campaign.Current.Models.NotablePowerModel.GetInitialPower(hero));
			}
		}

		// Token: 0x060045EA RID: 17898 RVA: 0x001539AC File Offset: 0x00151BAC
		private void DailyTickHero(Hero hero)
		{
			if (hero.IsAlive && hero.IsNotable)
			{
				hero.AddPower(Campaign.Current.Models.NotablePowerModel.CalculateDailyPowerChangeForHero(hero, false).ResultNumber);
				this.BalanceGoldAndPowerOfNotable(hero);
			}
		}

		// Token: 0x060045EB RID: 17899 RVA: 0x001539F4 File Offset: 0x00151BF4
		private void OnRaidCompleted(BattleSideEnum winnerSide, RaidEventComponent mapEvent)
		{
			foreach (Hero hero in mapEvent.MapEventSettlement.Notables)
			{
				hero.AddPower(-5f);
			}
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x00153A50 File Offset: 0x00151C50
		private void BalanceGoldAndPowerOfNotable(Hero notable)
		{
			if (notable.Gold > 10500)
			{
				int num = (notable.Gold - 10000) / 500;
				GiveGoldAction.ApplyBetweenCharacters(notable, null, num * 500, true);
				notable.AddPower((float)num);
				return;
			}
			if (notable.Gold < 4500 && notable.Power > 0f)
			{
				int num2 = (5000 - notable.Gold) / 500;
				GiveGoldAction.ApplyBetweenCharacters(null, notable, num2 * 500, true);
				notable.AddPower((float)(-(float)num2));
			}
		}

		// Token: 0x060045ED RID: 17901 RVA: 0x00153ADA File Offset: 0x00151CDA
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x04001421 RID: 5153
		private const int GoldLimitForNotablesToStartGainingPower = 10000;

		// Token: 0x04001422 RID: 5154
		private const int GoldLimitForNotablesToStartLosingPower = 5000;

		// Token: 0x04001423 RID: 5155
		private const int GoldNeededToGainOnePower = 500;
	}
}
