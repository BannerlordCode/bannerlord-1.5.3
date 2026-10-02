using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F8 RID: 248
	public class DefaultBanditDensityModel : BanditDensityModel
	{
		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x060016DC RID: 5852 RVA: 0x0006A583 File Offset: 0x00068783
		public override int NumberOfMinimumBanditPartiesInAHideoutToInfestIt
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060016DD RID: 5853 RVA: 0x0006A586 File Offset: 0x00068786
		public override int NumberOfMaximumBanditPartiesInEachHideout
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060016DE RID: 5854 RVA: 0x0006A589 File Offset: 0x00068789
		public override int NumberOfMaximumBanditPartiesAroundEachHideout
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060016DF RID: 5855 RVA: 0x0006A58C File Offset: 0x0006878C
		public override int NumberOfMaximumHideoutsAtEachBanditFaction
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060016E0 RID: 5856 RVA: 0x0006A590 File Offset: 0x00068790
		public override int NumberOfInitialHideoutsAtEachBanditFaction
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x0006A593 File Offset: 0x00068793
		public override int NumberOfMinimumBanditTroopsInHideoutMission
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060016E2 RID: 5858 RVA: 0x0006A597 File Offset: 0x00068797
		public override int NumberOfMaximumTroopCountForFirstFightInHideout
		{
			get
			{
				return MathF.Floor(11f * (2f + Campaign.Current.PlayerProgress));
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060016E3 RID: 5859 RVA: 0x0006A5B4 File Offset: 0x000687B4
		public override int NumberOfMaximumTroopCountForBossFightInHideout
		{
			get
			{
				return MathF.Floor(1f + 5f * (1f + Campaign.Current.PlayerProgress));
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060016E4 RID: 5860 RVA: 0x0006A5D7 File Offset: 0x000687D7
		public override float SpawnPercentageForFirstFightInHideoutMission
		{
			get
			{
				return 0.8f;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060016E5 RID: 5861 RVA: 0x0006A5DE File Offset: 0x000687DE
		private Clan DeserterClan
		{
			get
			{
				if (this._deserterClan == null)
				{
					this._deserterClan = Clan.FindFirst((Clan x) => x.StringId == "deserters");
				}
				return this._deserterClan;
			}
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x0006A618 File Offset: 0x00068818
		public override int GetMinimumTroopCountForHideoutMission(MobileParty party, bool isAssault)
		{
			if (!isAssault)
			{
				return 25;
			}
			return 8;
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x0006A624 File Offset: 0x00068824
		public override int GetMaxSupportedNumberOfLootersForClan(Clan clan)
		{
			if (clan == this.DeserterClan)
			{
				return 50;
			}
			if (clan.StringId == "looters" && this.DeserterClan != null)
			{
				return 270 - this.DeserterClan.WarPartyComponents.Count;
			}
			return 270;
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x0006A674 File Offset: 0x00068874
		public override int GetMaximumTroopCountForHideoutMission(MobileParty party, bool isAssault)
		{
			int num = (isAssault ? 15 : 40);
			Hero hero = null;
			if (party.HasPerk(DefaultPerks.Tactics.SmallUnitTactics, out hero, false))
			{
				int num2 = (int)DefaultPerks.Tactics.SmallUnitTactics.PrimaryBonus;
				num += num2;
			}
			return num;
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x0006A6AE File Offset: 0x000688AE
		public override bool IsPositionInsideNavalSafeZone(CampaignVec2 position)
		{
			return false;
		}

		// Token: 0x040007A2 RID: 1954
		private Clan _deserterClan;
	}
}
