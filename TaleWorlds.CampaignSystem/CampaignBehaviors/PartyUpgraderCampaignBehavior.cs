using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200044C RID: 1100
	public class PartyUpgraderCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600467A RID: 18042 RVA: 0x0015794C File Offset: 0x00155B4C
		public override void RegisterEvents()
		{
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.MapEventEnded));
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickParty));
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x0015797C File Offset: 0x00155B7C
		private void MapEventEnded(MapEvent mapEvent)
		{
			foreach (PartyBase partyBase in mapEvent.InvolvedParties)
			{
				this.UpgradeReadyTroops(partyBase);
			}
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x001579CC File Offset: 0x00155BCC
		public void DailyTickParty(MobileParty party)
		{
			if (party.MapEvent == null)
			{
				this.UpgradeReadyTroops(party.Party);
			}
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x001579E2 File Offset: 0x00155BE2
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x001579E4 File Offset: 0x00155BE4
		private PartyUpgraderCampaignBehavior.TroopUpgradeArgs SelectPossibleUpgrade(List<PartyUpgraderCampaignBehavior.TroopUpgradeArgs> possibleUpgrades)
		{
			PartyUpgraderCampaignBehavior.TroopUpgradeArgs troopUpgradeArgs = possibleUpgrades[0];
			if (possibleUpgrades.Count > 1)
			{
				float num = 0f;
				foreach (PartyUpgraderCampaignBehavior.TroopUpgradeArgs troopUpgradeArgs2 in possibleUpgrades)
				{
					num += troopUpgradeArgs2.UpgradeChance;
				}
				float num2 = num * MBRandom.RandomFloat;
				foreach (PartyUpgraderCampaignBehavior.TroopUpgradeArgs troopUpgradeArgs3 in possibleUpgrades)
				{
					num2 -= troopUpgradeArgs3.UpgradeChance;
					if (num2 <= 0f)
					{
						troopUpgradeArgs = troopUpgradeArgs3;
						break;
					}
				}
			}
			return troopUpgradeArgs;
		}

		// Token: 0x0600467F RID: 18047 RVA: 0x00157AA8 File Offset: 0x00155CA8
		private List<PartyUpgraderCampaignBehavior.TroopUpgradeArgs> GetPossibleUpgradeTargets(PartyBase party, TroopRosterElement element)
		{
			PartyWageModel partyWageModel = Campaign.Current.Models.PartyWageModel;
			List<PartyUpgraderCampaignBehavior.TroopUpgradeArgs> list = new List<PartyUpgraderCampaignBehavior.TroopUpgradeArgs>();
			CharacterObject character = element.Character;
			int num = element.Number - element.WoundedNumber;
			if (num > 0)
			{
				PartyTroopUpgradeModel partyTroopUpgradeModel = Campaign.Current.Models.PartyTroopUpgradeModel;
				int i = 0;
				while (i < character.UpgradeTargets.Length)
				{
					num = element.Number - element.WoundedNumber;
					CharacterObject characterObject = character.UpgradeTargets[i];
					int upgradeXpCost = character.GetUpgradeXpCost(party, i);
					if (upgradeXpCost <= 0)
					{
						goto IL_008F;
					}
					num = MathF.Min(num, element.Xp / upgradeXpCost);
					if (num != 0)
					{
						goto IL_008F;
					}
					IL_01AC:
					i++;
					continue;
					IL_008F:
					if (characterObject.Tier > character.Tier && party.MobileParty.HasLimitedWage() && party.MobileParty.TotalWage + num * (partyWageModel.GetCharacterWage(characterObject) - partyWageModel.GetCharacterWage(character)) > party.MobileParty.PaymentLimit)
					{
						num = MathF.Max(0, MathF.Min(num, (party.MobileParty.PaymentLimit - party.MobileParty.TotalWage) / (partyWageModel.GetCharacterWage(characterObject) - partyWageModel.GetCharacterWage(character))));
						if (num == 0)
						{
							goto IL_01AC;
						}
					}
					int upgradeGoldCost = character.GetUpgradeGoldCost(party, i);
					if (party.LeaderHero != null && upgradeGoldCost != 0 && num * upgradeGoldCost > party.MobileParty.PartyTradeGold)
					{
						num = party.MobileParty.PartyTradeGold / upgradeGoldCost;
						if (num == 0)
						{
							goto IL_01AC;
						}
					}
					if ((!party.Culture.IsBandit || characterObject.Culture.IsBandit) && (character.Occupation != Occupation.Bandit || partyTroopUpgradeModel.CanPartyUpgradeTroopToTarget(party, character, characterObject)))
					{
						float upgradeChanceForTroopUpgrade = Campaign.Current.Models.PartyTroopUpgradeModel.GetUpgradeChanceForTroopUpgrade(party, character, i);
						list.Add(new PartyUpgraderCampaignBehavior.TroopUpgradeArgs(character, characterObject, num, upgradeGoldCost, upgradeXpCost, upgradeChanceForTroopUpgrade));
						goto IL_01AC;
					}
					goto IL_01AC;
				}
			}
			return list;
		}

		// Token: 0x06004680 RID: 18048 RVA: 0x00157C78 File Offset: 0x00155E78
		private void ApplyEffects(PartyBase party, PartyUpgraderCampaignBehavior.TroopUpgradeArgs upgradeArgs)
		{
			if (party.Owner != null && party.Owner.IsAlive)
			{
				SkillLevelingManager.OnUpgradeTroops(party, upgradeArgs.Target, upgradeArgs.UpgradeTarget, upgradeArgs.PossibleUpgradeCount);
				GiveGoldAction.ApplyBetweenCharacters(party.Owner, null, upgradeArgs.UpgradeGoldCost * upgradeArgs.PossibleUpgradeCount, true);
				return;
			}
			if (party.LeaderHero != null && party.LeaderHero.IsAlive)
			{
				SkillLevelingManager.OnUpgradeTroops(party, upgradeArgs.Target, upgradeArgs.UpgradeTarget, upgradeArgs.PossibleUpgradeCount);
				GiveGoldAction.ApplyBetweenCharacters(party.LeaderHero, null, upgradeArgs.UpgradeGoldCost * upgradeArgs.PossibleUpgradeCount, true);
			}
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x00157D14 File Offset: 0x00155F14
		private void UpgradeTroop(PartyBase party, int rosterIndex, PartyUpgraderCampaignBehavior.TroopUpgradeArgs upgradeArgs)
		{
			TroopRoster memberRoster = party.MemberRoster;
			CharacterObject upgradeTarget = upgradeArgs.UpgradeTarget;
			int possibleUpgradeCount = upgradeArgs.PossibleUpgradeCount;
			int num = upgradeArgs.UpgradeXpCost * possibleUpgradeCount;
			memberRoster.SetElementXp(rosterIndex, memberRoster.GetElementXp(rosterIndex) - num);
			memberRoster.AddToCounts(upgradeArgs.Target, -possibleUpgradeCount, false, 0, 0, true, -1);
			memberRoster.AddToCounts(upgradeTarget, possibleUpgradeCount, false, 0, 0, true, -1);
			if (possibleUpgradeCount > 0)
			{
				this.ApplyEffects(party, upgradeArgs);
			}
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x00157D80 File Offset: 0x00155F80
		public void UpgradeReadyTroops(PartyBase party)
		{
			if (party != PartyBase.MainParty && party.IsActive)
			{
				TroopRoster memberRoster = party.MemberRoster;
				PartyTroopUpgradeModel partyTroopUpgradeModel = Campaign.Current.Models.PartyTroopUpgradeModel;
				for (int i = 0; i < memberRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = memberRoster.GetElementCopyAtIndex(i);
					if (partyTroopUpgradeModel.IsTroopUpgradeable(party, elementCopyAtIndex.Character))
					{
						List<PartyUpgraderCampaignBehavior.TroopUpgradeArgs> possibleUpgradeTargets = this.GetPossibleUpgradeTargets(party, elementCopyAtIndex);
						if (possibleUpgradeTargets.Count > 0)
						{
							PartyUpgraderCampaignBehavior.TroopUpgradeArgs troopUpgradeArgs = this.SelectPossibleUpgrade(possibleUpgradeTargets);
							this.UpgradeTroop(party, i, troopUpgradeArgs);
						}
					}
				}
			}
		}

		// Token: 0x0200087F RID: 2175
		private readonly struct TroopUpgradeArgs
		{
			// Token: 0x06006B28 RID: 27432 RVA: 0x001DA8A3 File Offset: 0x001D8AA3
			public TroopUpgradeArgs(CharacterObject target, CharacterObject upgradeTarget, int possibleUpgradeCount, int upgradeGoldCost, int upgradeXpCost, float upgradeChance)
			{
				this.Target = target;
				this.UpgradeTarget = upgradeTarget;
				this.PossibleUpgradeCount = possibleUpgradeCount;
				this.UpgradeGoldCost = upgradeGoldCost;
				this.UpgradeXpCost = upgradeXpCost;
				this.UpgradeChance = upgradeChance;
			}

			// Token: 0x040024F0 RID: 9456
			public readonly CharacterObject Target;

			// Token: 0x040024F1 RID: 9457
			public readonly CharacterObject UpgradeTarget;

			// Token: 0x040024F2 RID: 9458
			public readonly int PossibleUpgradeCount;

			// Token: 0x040024F3 RID: 9459
			public readonly int UpgradeGoldCost;

			// Token: 0x040024F4 RID: 9460
			public readonly int UpgradeXpCost;

			// Token: 0x040024F5 RID: 9461
			public readonly float UpgradeChance;
		}
	}
}
