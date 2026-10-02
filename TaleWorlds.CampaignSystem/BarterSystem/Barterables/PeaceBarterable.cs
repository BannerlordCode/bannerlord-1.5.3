using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x020004AC RID: 1196
	public class PeaceBarterable : Barterable
	{
		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x06004C66 RID: 19558 RVA: 0x001834BE File Offset: 0x001816BE
		public CampaignTime Duration { get; }

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x06004C67 RID: 19559 RVA: 0x001834C6 File Offset: 0x001816C6
		public override string StringID
		{
			get
			{
				return "peace_barterable";
			}
		}

		// Token: 0x06004C68 RID: 19560 RVA: 0x001834CD File Offset: 0x001816CD
		public PeaceBarterable(Hero owner, IFaction peaceOfferingFaction, IFaction offeredFaction, CampaignTime duration)
		{
			MobileParty partyBelongedTo = owner.PartyBelongedTo;
			base..ctor(owner, (partyBelongedTo != null) ? partyBelongedTo.Party : null);
			this.Duration = duration;
			this.PeaceOfferingFaction = peaceOfferingFaction;
			this.OfferedFaction = offeredFaction;
		}

		// Token: 0x06004C69 RID: 19561 RVA: 0x00183500 File Offset: 0x00181700
		public PeaceBarterable(IFaction peaceOfferingFaction, IFaction offeredFaction, CampaignTime duration)
		{
			Hero leader = peaceOfferingFaction.Leader;
			Hero leader2 = peaceOfferingFaction.Leader;
			PartyBase partyBase;
			if (leader2 == null)
			{
				partyBase = null;
			}
			else
			{
				MobileParty partyBelongedTo = leader2.PartyBelongedTo;
				partyBase = ((partyBelongedTo != null) ? partyBelongedTo.Party : null);
			}
			base..ctor(leader, partyBase);
			this.Duration = duration;
			this.PeaceOfferingFaction = peaceOfferingFaction;
			this.OfferedFaction = offeredFaction;
		}

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x06004C6A RID: 19562 RVA: 0x0018354C File Offset: 0x0018174C
		public override TextObject Name
		{
			get
			{
				TextObject textObject = new TextObject("{=R0bJS0pn}Make peace with the {OTHER_FACTION}", null);
				textObject.SetTextVariable("OTHER_FACTION", this.OfferedFaction.InformalName);
				return textObject;
			}
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x00183570 File Offset: 0x00181770
		public override int GetUnitValueForFaction(IFaction factionToEvaluateFor)
		{
			float num = 0f;
			IFaction faction = this.OfferedFaction;
			IFaction faction2 = this.PeaceOfferingFaction;
			if (factionToEvaluateFor.MapFaction == faction)
			{
				IFaction faction3 = faction2;
				IFaction faction4 = faction;
				faction = faction3;
				faction2 = faction4;
			}
			if (faction == null || faction2 == null)
			{
				return 0;
			}
			TextObject textObject;
			num = (float)((int)Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringPeaceForClan(faction2, faction, factionToEvaluateFor.Leader.Clan, out textObject, false));
			if (factionToEvaluateFor.IsKingdomFaction)
			{
				float num2 = 0f;
				int num3 = 0;
				foreach (Clan clan in ((Kingdom)factionToEvaluateFor).Clans)
				{
					float num4 = ((clan.Leader != null) ? ((clan.Leader.Gold < 50000) ? (1f + 0.5f * ((50000f - (float)clan.Leader.Gold) / 50000f)) : ((clan.Leader.Gold > 200000) ? MathF.Max(0.66f, MathF.Pow(200000f / (float)clan.Leader.Gold, 0.4f)) : 1f)) : 1f);
					num2 += num4;
					num3++;
				}
				float num5 = (num2 + 1f) / ((float)num3 + 1f);
				num /= num5;
			}
			return (int)num;
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x001836E8 File Offset: 0x001818E8
		public override bool IsCompatible(Barterable barterable)
		{
			PeaceBarterable peaceBarterable = barterable as PeaceBarterable;
			return peaceBarterable == null || peaceBarterable.OfferedFaction != base.OriginalOwner.MapFaction;
		}

		// Token: 0x06004C6D RID: 19565 RVA: 0x00183717 File Offset: 0x00181917
		public override ImageIdentifier GetVisualIdentifier()
		{
			return null;
		}

		// Token: 0x06004C6E RID: 19566 RVA: 0x0018371A File Offset: 0x0018191A
		public override string GetEncyclopediaLink()
		{
			return base.OriginalOwner.MapFaction.EncyclopediaLink;
		}

		// Token: 0x06004C6F RID: 19567 RVA: 0x0018372C File Offset: 0x0018192C
		public override void Apply()
		{
			if (this.PeaceOfferingFaction.MapFaction.IsAtWarWith(this.OfferedFaction))
			{
				MakePeaceAction.Apply(this.PeaceOfferingFaction.MapFaction, this.OfferedFaction);
				if (PlayerEncounter.Current != null && Hero.OneToOneConversationHero == base.OriginalOwner)
				{
					PlayerEncounter.LeaveEncounter = true;
					PartyBase originalParty = base.OriginalParty;
					bool flag;
					if (originalParty == null)
					{
						flag = null != null;
					}
					else
					{
						MobileParty mobileParty = originalParty.MobileParty;
						flag = ((mobileParty != null) ? mobileParty.Ai.AiBehaviorPartyBase : null) != null;
					}
					if (flag)
					{
						LocatableSearchData<MobileParty> locatableSearchData = Campaign.Current.MobilePartyLocator.StartFindingLocatablesAroundPosition(MobileParty.MainParty.Position.ToVec2(), 5f);
						for (MobileParty mobileParty2 = Campaign.Current.MobilePartyLocator.FindNextLocatable(ref locatableSearchData); mobileParty2 != null; mobileParty2 = Campaign.Current.MobilePartyLocator.FindNextLocatable(ref locatableSearchData))
						{
							if (!mobileParty2.IsMainParty && mobileParty2.MapFaction == base.OriginalOwner.MapFaction && (mobileParty2.TargetParty == MobileParty.MainParty || mobileParty2.Ai.AiBehaviorPartyBase == PartyBase.MainParty || mobileParty2.TargetSettlement == MobileParty.MainParty.TargetSettlement))
							{
								mobileParty2.SetMoveModeHold();
							}
						}
						if (base.OriginalParty.MobileParty.Army != null && MobileParty.MainParty.Army != base.OriginalParty.MobileParty.Army)
						{
							base.OriginalParty.MobileParty.Army.LeaderParty.SetMoveModeHold();
						}
					}
				}
			}
		}

		// Token: 0x06004C70 RID: 19568 RVA: 0x0018389C File Offset: 0x00181A9C
		internal static void AutoGeneratedStaticCollectObjectsPeaceBarterable(object o, List<object> collectedObjects)
		{
			((PeaceBarterable)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06004C71 RID: 19569 RVA: 0x001838AA File Offset: 0x00181AAA
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0400153F RID: 5439
		public readonly IFaction PeaceOfferingFaction;

		// Token: 0x04001540 RID: 5440
		public readonly IFaction OfferedFaction;
	}
}
