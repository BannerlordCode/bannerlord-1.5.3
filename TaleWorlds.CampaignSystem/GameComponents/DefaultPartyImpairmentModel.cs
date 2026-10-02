using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200013B RID: 315
	public class DefaultPartyImpairmentModel : PartyImpairmentModel
	{
		// Token: 0x060019BB RID: 6587 RVA: 0x0008059C File Offset: 0x0007E79C
		public override float GetSiegeExpectedVulnerabilityTime()
		{
			float num = ((float)CampaignTime.SunRise + MBRandom.RandomFloatNormal + (float)CampaignTime.HoursInDay - CampaignTime.Now.CurrentHourInDay) % (float)CampaignTime.HoursInDay;
			float num2 = MathF.Pow(MBRandom.RandomFloat, 6f);
			return (((MBRandom.RandomFloatNormal > 0f) ? num2 : (1f - num2)) * (float)CampaignTime.HoursInDay + num) % (float)CampaignTime.HoursInDay;
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x00080608 File Offset: 0x0007E808
		public override ExplainedNumber GetDisorganizedStateDuration(MobileParty party)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(6f, false, null);
			if (party.MapEvent != null && (party.MapEvent.IsRaid || party.MapEvent.IsSiegeAssault))
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.SwiftRegroup, party, true, ref explainedNumber);
			}
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Foragers, party, false, ref explainedNumber);
			Army army = party.Army;
			Hero hero;
			if (army == null)
			{
				hero = null;
			}
			else
			{
				MobileParty leaderParty = army.LeaderParty;
				hero = ((leaderParty != null) ? leaderParty.LeaderHero : null);
			}
			Hero hero2 = hero ?? party.LeaderHero;
			if (hero2 != null)
			{
				TraitEffectHelper.ApplyTraitEffect(hero2, DefaultPersonalityTraitEffects.CalculatingLongerDisorganizeEffect, ref explainedNumber);
			}
			return explainedNumber;
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x000806A4 File Offset: 0x0007E8A4
		public override bool CanGetDisorganized(PartyBase party)
		{
			return party.IsActive && party.IsMobile && party.MobileParty.MemberRoster.TotalManCount >= 10 && (party.MobileParty.Army == null || party.MobileParty == party.MobileParty.Army.LeaderParty || party.MobileParty.AttachedTo != null);
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x0008070C File Offset: 0x0007E90C
		public override float GetVulnerabilityStateDuration(PartyBase party)
		{
			return MBRandom.RandomFloatNormal + 4f;
		}

		// Token: 0x04000862 RID: 2146
		private const float BaseDisorganizedStateDuration = 6f;

		// Token: 0x04000863 RID: 2147
		private static readonly TextObject _settlementInvolvedMapEvent = new TextObject("{=KVlPhPSD}Settlement involved map event", null);
	}
}
