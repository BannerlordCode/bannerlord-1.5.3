using System;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200012C RID: 300
	public class DefaultKingdomDecisionPermissionModel : KingdomDecisionPermissionModel
	{
		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x060018F2 RID: 6386 RVA: 0x000795E7 File Offset: 0x000777E7
		private IAllianceCampaignBehavior AllianceCampaignBehavior
		{
			get
			{
				if (this._allianceCampaignBehavior == null)
				{
					this._allianceCampaignBehavior = Campaign.Current.GetCampaignBehavior<IAllianceCampaignBehavior>();
				}
				return this._allianceCampaignBehavior;
			}
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x00079607 File Offset: 0x00077807
		public override bool IsPolicyDecisionAllowed(PolicyObject policy)
		{
			return true;
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x0007960A File Offset: 0x0007780A
		public override bool IsWarDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
		{
			reason = null;
			return true;
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x00079610 File Offset: 0x00077810
		public override bool IsPeaceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
		{
			reason = null;
			Kingdom kingdom3 = null;
			if (Campaign.Current.Models.DiplomacyModel.IsAtConstantWar(kingdom1, kingdom2))
			{
				reason = new TextObject("{=eNPupZOp}These kingdoms can not declare peace at this time.", null);
				return false;
			}
			IAllianceCampaignBehavior allianceCampaignBehavior = this.AllianceCampaignBehavior;
			if (allianceCampaignBehavior != null && allianceCampaignBehavior.IsAtWarByCallToWarAgreement(kingdom1, kingdom2, out kingdom3))
			{
				reason = this.GetExplanationForPeaceOfferWithCallToWar(kingdom3, kingdom1, kingdom2);
				return false;
			}
			IAllianceCampaignBehavior allianceCampaignBehavior2 = this.AllianceCampaignBehavior;
			if (allianceCampaignBehavior2 != null && allianceCampaignBehavior2.IsAtWarByCallToWarAgreement(kingdom2, kingdom1, out kingdom3))
			{
				reason = this.GetExplanationForPeaceOfferWithCallToWar(kingdom3, kingdom2, kingdom1);
				return false;
			}
			if (!Campaign.Current.Models.DiplomacyModel.IsPeaceSuitable(kingdom1, kingdom2))
			{
				reason = new TextObject("{=JkQ7fmcX}The enemy is not open to negotiations.", null);
				return false;
			}
			return true;
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x000796BB File Offset: 0x000778BB
		public override bool IsAnnexationDecisionAllowed(Settlement annexedSettlement)
		{
			return true;
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x000796BE File Offset: 0x000778BE
		public override bool IsExpulsionDecisionAllowed(Clan expelledClan)
		{
			return true;
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x000796C1 File Offset: 0x000778C1
		public override bool IsKingSelectionDecisionAllowed(Kingdom kingdom)
		{
			return true;
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x000796C4 File Offset: 0x000778C4
		public override bool IsStartAllianceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
		{
			reason = null;
			return true;
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x000796CC File Offset: 0x000778CC
		private TextObject GetExplanationForPeaceOfferWithCallToWar(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			TextObject textObject = TextObject.GetEmpty();
			if (calledKingdom == Clan.PlayerClan.Kingdom)
			{
				textObject = new TextObject("{=M6wsjpNN}Your realm is not allowed to negotiate peace with {KINGDOM_TO_CALL_TO_WAR_AGAINST} due to your Call to War Agreement with {CALLING_KINGDOM}.", null);
				textObject.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", kingdomToCallToWarAgainst.Name);
				textObject.SetTextVariable("CALLING_KINGDOM", callingKingdom.Name);
			}
			else if (kingdomToCallToWarAgainst == Clan.PlayerClan.Kingdom)
			{
				textObject = new TextObject("{=CiFKYMKb}Your realm is not allowed to negotiate peace with {CALLED_KINGDOM} due to their Call to War Agreement with {CALLING_KINGDOM}.", null);
				textObject.SetTextVariable("CALLED_KINGDOM", calledKingdom.Name);
				textObject.SetTextVariable("CALLING_KINGDOM", callingKingdom.Name);
			}
			else
			{
				textObject = new TextObject("{=mc0wmdkb}{KINGDOM_NAME} is not allowed to negotiate peace with {CALLED_KINGDOM} due to their Call to War Agreement with {CALLING_KINGDOM}.", null);
				textObject.SetTextVariable("KINGDOM_NAME", kingdomToCallToWarAgainst.Name);
				textObject.SetTextVariable("CALLED_KINGDOM", calledKingdom.Name);
				textObject.SetTextVariable("CALLING_KINGDOM", callingKingdom.Name);
			}
			return textObject;
		}

		// Token: 0x0400082B RID: 2091
		private IAllianceCampaignBehavior _allianceCampaignBehavior;
	}
}
