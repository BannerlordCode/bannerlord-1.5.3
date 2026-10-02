using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000458 RID: 1112
	public class PrisonerCaptureCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060047BA RID: 18362 RVA: 0x001607C4 File Offset: 0x0015E9C4
		public override void RegisterEvents()
		{
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
		}

		// Token: 0x060047BB RID: 18363 RVA: 0x00160816 File Offset: 0x0015EA16
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060047BC RID: 18364 RVA: 0x00160818 File Offset: 0x0015EA18
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification)
		{
			for (int i = 0; i < clan.Settlements.Count; i++)
			{
				Settlement settlement = clan.Settlements[i];
				if (settlement.IsFortification)
				{
					this.HandleSettlementHeroes(settlement);
				}
			}
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x00160858 File Offset: 0x0015EA58
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
		{
			for (int i = 0; i < faction1.Settlements.Count; i++)
			{
				Settlement settlement = faction1.Settlements[i];
				if (settlement.IsFortification)
				{
					this.HandleSettlementHeroes(settlement);
				}
			}
			for (int j = 0; j < faction2.Settlements.Count; j++)
			{
				Settlement settlement2 = faction2.Settlements[j];
				if (settlement2.IsFortification)
				{
					this.HandleSettlementHeroes(settlement2);
				}
			}
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x001608C9 File Offset: 0x0015EAC9
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (settlement.IsFortification)
			{
				this.HandleSettlementHeroes(settlement);
			}
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x001608DC File Offset: 0x0015EADC
		private void HandleSettlementHeroes(Settlement settlement)
		{
			for (int i = settlement.HeroesWithoutParty.Count - 1; i >= 0; i--)
			{
				Hero hero = settlement.HeroesWithoutParty[i];
				if (this.SettlementHeroCaptureCommonCondition(hero))
				{
					TakePrisonerAction.Apply(hero.CurrentSettlement.Party, hero);
				}
			}
			for (int j = settlement.Parties.Count - 1; j >= 0; j--)
			{
				MobileParty mobileParty = settlement.Parties[j];
				if (mobileParty.IsLordParty && (mobileParty.Army == null || (mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty && !mobileParty.Army.Parties.Contains(MobileParty.MainParty))) && mobileParty.MapEvent == null && this.SettlementHeroCaptureCommonCondition(mobileParty.LeaderHero))
				{
					LeaveSettlementAction.ApplyForParty(mobileParty);
				}
			}
		}

		// Token: 0x060047C0 RID: 18368 RVA: 0x001609A8 File Offset: 0x0015EBA8
		private bool SettlementHeroCaptureCommonCondition(Hero hero)
		{
			return hero != null && hero != Hero.MainHero && !hero.IsWanderer && !hero.IsNotable && hero.HeroState != Hero.CharacterStates.Prisoner && hero.HeroState != Hero.CharacterStates.Dead && hero.MapFaction != null && hero.CurrentSettlement != null && hero.MapFaction.IsAtWarWith(hero.CurrentSettlement.MapFaction);
		}
	}
}
