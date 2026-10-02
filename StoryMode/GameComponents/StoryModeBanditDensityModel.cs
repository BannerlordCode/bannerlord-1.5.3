using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;

namespace StoryMode.GameComponents
{
	// Token: 0x0200003C RID: 60
	public class StoryModeBanditDensityModel : BanditDensityModel
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x00018DB1 File Offset: 0x00016FB1
		public override int NumberOfMaximumBanditPartiesAroundEachHideout
		{
			get
			{
				if (StoryModeManager.Current.MainStoryLine.IsPlayerInteractionRestricted)
				{
					return 0;
				}
				return base.BaseModel.NumberOfMaximumBanditPartiesAroundEachHideout;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00018DD1 File Offset: 0x00016FD1
		public override int NumberOfMaximumBanditPartiesInEachHideout
		{
			get
			{
				if (StoryModeManager.Current.MainStoryLine.IsPlayerInteractionRestricted)
				{
					return 0;
				}
				return base.BaseModel.NumberOfMaximumBanditPartiesInEachHideout;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x00018DF1 File Offset: 0x00016FF1
		public override int NumberOfMaximumHideoutsAtEachBanditFaction
		{
			get
			{
				if (StoryModeManager.Current.MainStoryLine.IsPlayerInteractionRestricted)
				{
					return 0;
				}
				return base.BaseModel.NumberOfMaximumHideoutsAtEachBanditFaction;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00018E11 File Offset: 0x00017011
		public override int NumberOfInitialHideoutsAtEachBanditFaction
		{
			get
			{
				if (StoryModeManager.Current.MainStoryLine.IsPlayerInteractionRestricted)
				{
					return 0;
				}
				return base.BaseModel.NumberOfInitialHideoutsAtEachBanditFaction;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x00018E31 File Offset: 0x00017031
		public override int NumberOfMinimumBanditPartiesInAHideoutToInfestIt
		{
			get
			{
				return base.BaseModel.NumberOfMinimumBanditPartiesInAHideoutToInfestIt;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00018E3E File Offset: 0x0001703E
		public override int NumberOfMinimumBanditTroopsInHideoutMission
		{
			get
			{
				return base.BaseModel.NumberOfMinimumBanditTroopsInHideoutMission;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x00018E4B File Offset: 0x0001704B
		public override int NumberOfMaximumTroopCountForFirstFightInHideout
		{
			get
			{
				return base.BaseModel.NumberOfMaximumTroopCountForFirstFightInHideout;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00018E58 File Offset: 0x00017058
		public override int NumberOfMaximumTroopCountForBossFightInHideout
		{
			get
			{
				return base.BaseModel.NumberOfMaximumTroopCountForBossFightInHideout;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x00018E65 File Offset: 0x00017065
		public override float SpawnPercentageForFirstFightInHideoutMission
		{
			get
			{
				return base.BaseModel.SpawnPercentageForFirstFightInHideoutMission;
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00018E72 File Offset: 0x00017072
		public override int GetMaximumTroopCountForHideoutMission(MobileParty party, bool isAssault)
		{
			return base.BaseModel.GetMaximumTroopCountForHideoutMission(party, isAssault);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00018E81 File Offset: 0x00017081
		public override bool IsPositionInsideNavalSafeZone(CampaignVec2 position)
		{
			return base.BaseModel.IsPositionInsideNavalSafeZone(position);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00018E8F File Offset: 0x0001708F
		public override int GetMaxSupportedNumberOfLootersForClan(Clan clan)
		{
			if (StoryModeManager.Current.MainStoryLine.IsPlayerInteractionRestricted)
			{
				return 0;
			}
			return base.BaseModel.GetMaxSupportedNumberOfLootersForClan(clan);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00018EB0 File Offset: 0x000170B0
		public override int GetMinimumTroopCountForHideoutMission(MobileParty party, bool isAssault)
		{
			return base.BaseModel.GetMinimumTroopCountForHideoutMission(party, isAssault);
		}
	}
}
