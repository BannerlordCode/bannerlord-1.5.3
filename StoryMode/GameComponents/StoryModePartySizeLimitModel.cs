using System;
using System.Collections.Generic;
using System.Linq;
using StoryMode.Quests.SecondPhase.ConspiracyQuests;
using StoryMode.Quests.ThirdPhase;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;

namespace StoryMode.GameComponents
{
	// Token: 0x02000047 RID: 71
	public class StoryModePartySizeLimitModel : PartySizeLimitModel
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x0001965A File Offset: 0x0001785A
		public override int MinimumNumberOfVillagersAtVillagerParty
		{
			get
			{
				return base.BaseModel.MinimumNumberOfVillagersAtVillagerParty;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00019667 File Offset: 0x00017867
		private DefeatTheConspiracyQuestBehavior DefeatTheConspiracyQuestBehavior
		{
			get
			{
				if (this._defeatTheConspiracyQuestBehavior != null)
				{
					return this._defeatTheConspiracyQuestBehavior;
				}
				this._defeatTheConspiracyQuestBehavior = Campaign.Current.GetCampaignBehavior<DefeatTheConspiracyQuestBehavior>();
				return this._defeatTheConspiracyQuestBehavior;
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001968E File Offset: 0x0001788E
		public override ExplainedNumber CalculateGarrisonPartySizeLimit(Settlement settlement, bool includeDescriptions = false)
		{
			return base.BaseModel.CalculateGarrisonPartySizeLimit(settlement, includeDescriptions);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001969D File Offset: 0x0001789D
		public override TroopRoster FindAppropriateInitialRosterForMobileParty(MobileParty party, PartyTemplateObject partyTemplate)
		{
			return base.BaseModel.FindAppropriateInitialRosterForMobileParty(party, partyTemplate);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000196AC File Offset: 0x000178AC
		public override List<Ship> FindAppropriateInitialShipsForMobileParty(MobileParty party, PartyTemplateObject partyTemplate)
		{
			return base.BaseModel.FindAppropriateInitialShipsForMobileParty(party, partyTemplate);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000196BB File Offset: 0x000178BB
		public override int GetAssumedPartySizeForLordParty(Hero leaderHero, IFaction partyMapFaction, Clan actualClan)
		{
			return base.BaseModel.GetAssumedPartySizeForLordParty(leaderHero, partyMapFaction, actualClan);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000196CB File Offset: 0x000178CB
		public override int GetClanTierPartySizeEffectForHero(Hero hero)
		{
			return base.BaseModel.GetClanTierPartySizeEffectForHero(hero);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x000196D9 File Offset: 0x000178D9
		public override int GetIdealVillagerPartySize(Village village)
		{
			return base.BaseModel.GetIdealVillagerPartySize(village);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000196E7 File Offset: 0x000178E7
		public override int GetNextClanTierPartySizeEffectChangeForHero(Hero hero)
		{
			return base.BaseModel.GetNextClanTierPartySizeEffectChangeForHero(hero);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000196F8 File Offset: 0x000178F8
		public override ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false)
		{
			if (party.IsMobile)
			{
				QuestBase questBase = Campaign.Current.QuestManager.Quests.FirstOrDefault<QuestBase>((QuestBase q) => !q.IsFinalized && q.GetType() == typeof(DisruptSupplyLinesConspiracyQuest));
				if (questBase != null)
				{
					MobileParty conspiracyCaravan = ((DisruptSupplyLinesConspiracyQuest)questBase).ConspiracyCaravan;
					if (((conspiracyCaravan != null) ? conspiracyCaravan.Party : null) == party)
					{
						return new ExplainedNumber((float)((DisruptSupplyLinesConspiracyQuest)questBase).CaravanPartySize, false, null);
					}
				}
				if (this.DefeatTheConspiracyQuestBehavior != null && this.DefeatTheConspiracyQuestBehavior.IsMobilePartyCreatedForQuest(party.MobileParty))
				{
					return new ExplainedNumber(600f, false, null);
				}
			}
			return base.BaseModel.GetPartyMemberSizeLimit(party, includeDescriptions);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000197AA File Offset: 0x000179AA
		public override ExplainedNumber GetPartyPrisonerSizeLimit(PartyBase party, bool includeDescriptions = false)
		{
			return base.BaseModel.GetPartyPrisonerSizeLimit(party, includeDescriptions);
		}

		// Token: 0x04000194 RID: 404
		private DefeatTheConspiracyQuestBehavior _defeatTheConspiracyQuestBehavior;
	}
}
