using System;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000149 RID: 329
	public class DefaultPrisonBreakModel : PrisonBreakModel
	{
		// Token: 0x06001A3B RID: 6715 RVA: 0x00084400 File Offset: 0x00082600
		public override int GetNumberOfGuardsToSpawn(Settlement settlement)
		{
			int num = (int)Math.Ceiling((double)(2f + settlement.Town.Security / 30f));
			int num2 = settlement.Town.GetWallLevel() - 1;
			return num + num2;
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x0008443C File Offset: 0x0008263C
		public override bool CanPlayerStagePrisonBreak(Settlement settlement)
		{
			bool flag = false;
			if (settlement.IsFortification)
			{
				MobileParty garrisonParty = settlement.Town.GarrisonParty;
				bool flag2 = (garrisonParty != null && garrisonParty.PrisonRoster.TotalHeroes > 0) || settlement.Party.PrisonRoster.TotalHeroes > 0;
				flag = settlement.MapFaction != Clan.PlayerClan.MapFaction && !DiplomacyHelper.IsSameFactionAndNotEliminated(settlement.MapFaction, Clan.PlayerClan.MapFaction) && flag2;
			}
			return flag;
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x000844BC File Offset: 0x000826BC
		public override int GetPrisonBreakStartCost(Hero prisonerHero)
		{
			int num = MathF.Ceiling((float)Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(prisonerHero.CharacterObject, null) / 2000f * prisonerHero.CurrentSettlement.Town.Security * 40f - (float)(Hero.MainHero.GetSkillValue(DefaultSkills.Roguery) * 10));
			num = ((num < 100) ? 0 : (num / 100 * 100));
			return num + 1000;
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x00084533 File Offset: 0x00082733
		public override int GetRelationRewardOnPrisonBreak(Hero prisonerHero)
		{
			return 15;
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x00084537 File Offset: 0x00082737
		public override float GetRogueryRewardOnPrisonBreak(Hero prisonerHero, bool isSuccess)
		{
			return (float)(isSuccess ? MBRandom.RandomInt(2000, 4500) : MBRandom.RandomInt(500, 1000));
		}

		// Token: 0x040008B1 RID: 2225
		private const int BasePrisonBreakCost = 1000;
	}
}
