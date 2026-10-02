using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200044B RID: 1099
	public class PartyRolesCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600466F RID: 18031 RVA: 0x0015773C File Offset: 0x0015593C
		public override void RegisterEvents()
		{
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.OnGovernorChangedEvent.AddNonSerializedListener(this, new Action<Town, Hero, Hero>(this.OnGovernorChanged));
			CampaignEvents.MobilePartyCreated.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartySpawned));
			CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.OnCompanionRemoved));
			CampaignEvents.OnHeroGetsBusyEvent.AddNonSerializedListener(this, new Action<Hero, HeroGetsBusyReasons>(this.OnHeroGetsBusy));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroPrisonerTaken));
			CampaignEvents.OnHeroChangedClanEvent.AddNonSerializedListener(this, new Action<Hero, Clan>(this.OnHeroChangedClan));
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x001577EA File Offset: 0x001559EA
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004671 RID: 18033 RVA: 0x001577EC File Offset: 0x001559EC
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			if (victim.Clan == Clan.PlayerClan)
			{
				this.RemoveAllPartyRolesOfHeroIfExist(victim);
			}
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x00157802 File Offset: 0x00155A02
		private void OnHeroPrisonerTaken(PartyBase party, Hero prisoner)
		{
			if (prisoner.Clan == Clan.PlayerClan)
			{
				this.RemoveAllPartyRolesOfHeroIfExist(prisoner);
			}
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x00157818 File Offset: 0x00155A18
		private void OnGovernorChanged(Town fortification, Hero oldGovernor, Hero newGovernor)
		{
			if (((newGovernor != null) ? newGovernor.Clan : null) == Clan.PlayerClan)
			{
				this.RemoveAllPartyRolesOfHeroIfExist(newGovernor);
			}
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x00157834 File Offset: 0x00155A34
		private void OnPartySpawned(MobileParty spawnedParty)
		{
			if (spawnedParty.IsLordParty && spawnedParty.ActualClan == Clan.PlayerClan)
			{
				foreach (TroopRosterElement troopRosterElement in spawnedParty.MemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character.IsHero)
					{
						this.RemoveAllPartyRolesOfHeroIfExist(troopRosterElement.Character.HeroObject);
					}
				}
			}
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x001578B8 File Offset: 0x00155AB8
		private void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			this.RemoveAllPartyRolesOfHeroIfExist(companion);
		}

		// Token: 0x06004676 RID: 18038 RVA: 0x001578C1 File Offset: 0x00155AC1
		private void OnHeroGetsBusy(Hero hero, HeroGetsBusyReasons heroGetsBusyReason)
		{
			if (hero.Clan == Clan.PlayerClan)
			{
				this.RemoveAllPartyRolesOfHeroIfExist(hero);
			}
		}

		// Token: 0x06004677 RID: 18039 RVA: 0x001578D7 File Offset: 0x00155AD7
		private void OnHeroChangedClan(Hero hero, Clan oldClan)
		{
			if (oldClan == Clan.PlayerClan)
			{
				this.RemoveAllPartyRolesOfHeroIfExist(hero);
			}
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x001578E8 File Offset: 0x00155AE8
		private void RemoveAllPartyRolesOfHeroIfExist(Hero hero)
		{
			foreach (WarPartyComponent warPartyComponent in Clan.PlayerClan.WarPartyComponents)
			{
				warPartyComponent.MobileParty.RemoveAllPartyRolesOfHero(hero);
			}
		}
	}
}
