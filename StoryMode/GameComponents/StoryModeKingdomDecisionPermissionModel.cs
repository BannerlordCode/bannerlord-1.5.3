using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace StoryMode.GameComponents
{
	// Token: 0x02000045 RID: 69
	public class StoryModeKingdomDecisionPermissionModel : KingdomDecisionPermissionModel
	{
		// Token: 0x06000455 RID: 1109 RVA: 0x000194D6 File Offset: 0x000176D6
		public override bool IsPolicyDecisionAllowed(PolicyObject policy)
		{
			return base.BaseModel.IsPolicyDecisionAllowed(policy);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x000194E4 File Offset: 0x000176E4
		public override bool IsAnnexationDecisionAllowed(Settlement annexedSettlement)
		{
			return base.BaseModel.IsAnnexationDecisionAllowed(annexedSettlement);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x000194F2 File Offset: 0x000176F2
		public override bool IsExpulsionDecisionAllowed(Clan expelledClan)
		{
			return base.BaseModel.IsExpulsionDecisionAllowed(expelledClan);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00019500 File Offset: 0x00017700
		public override bool IsKingSelectionDecisionAllowed(Kingdom kingdom)
		{
			return base.BaseModel.IsKingSelectionDecisionAllowed(kingdom);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00019510 File Offset: 0x00017710
		public override bool IsWarDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
		{
			if (StoryModeManager.Current.MainStoryLine.ThirdPhase != null)
			{
				MBReadOnlyList<Kingdom> oppositionKingdoms = StoryModeManager.Current.MainStoryLine.ThirdPhase.OppositionKingdoms;
				if (oppositionKingdoms.IndexOf(kingdom1) >= 0 && oppositionKingdoms.IndexOf(kingdom2) >= 0)
				{
					reason = GameTexts.FindText("str_kingdom_diplomacy_war_truce_disabled_reason_story", null);
					return false;
				}
			}
			return base.BaseModel.IsWarDecisionAllowedBetweenKingdoms(kingdom1, kingdom2, out reason);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00019574 File Offset: 0x00017774
		public override bool IsPeaceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
		{
			if (StoryModeManager.Current.MainStoryLine.ThirdPhase != null)
			{
				MBReadOnlyList<Kingdom> oppositionKingdoms = StoryModeManager.Current.MainStoryLine.ThirdPhase.OppositionKingdoms;
				MBReadOnlyList<Kingdom> allyKingdoms = StoryModeManager.Current.MainStoryLine.ThirdPhase.AllyKingdoms;
				if ((oppositionKingdoms.IndexOf(kingdom1) >= 0 && allyKingdoms.IndexOf(kingdom2) >= 0) || (oppositionKingdoms.IndexOf(kingdom2) >= 0 && allyKingdoms.IndexOf(kingdom1) >= 0))
				{
					reason = GameTexts.FindText("str_kingdom_diplomacy_war_truce_disabled_reason_story", null);
					return false;
				}
			}
			return base.BaseModel.IsPeaceDecisionAllowedBetweenKingdoms(kingdom1, kingdom2, out reason);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00019601 File Offset: 0x00017801
		public override bool IsStartAllianceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
		{
			return base.BaseModel.IsStartAllianceDecisionAllowedBetweenKingdoms(kingdom1, kingdom2, out reason);
		}
	}
}
