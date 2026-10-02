using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Incidents
{
	// Token: 0x02000235 RID: 565
	public static class CampaignIncidentEffects
	{
		// Token: 0x06002200 RID: 8704 RVA: 0x00097194 File Offset: 0x00095394
		public static IncidentEffect ChangePrisonerAmount(Func<CharacterObject> characterGetter, int amount)
		{
			return new IncidentEffect(delegate
			{
				Func<CharacterObject> characterGetter2 = characterGetter;
				CharacterObject characterObject = ((characterGetter2 != null) ? characterGetter2() : null);
				if (characterObject == null)
				{
					return false;
				}
				if (amount >= 0)
				{
					return true;
				}
				int num = MobileParty.MainParty.PrisonRoster.FindIndexOfTroop(characterObject);
				return num != -1 && MobileParty.MainParty.PrisonRoster.GetElementNumber(num) >= Math.Abs(amount);
			}, delegate
			{
				CharacterObject characterObject2 = characterGetter();
				MobileParty.MainParty.PrisonRoster.AddToCounts(characterObject2, amount, false, 0, 0, true, -1);
				TextObject textObject = new TextObject("{=QbIJIMth}{ABS(AMOUNT)} {CHARACTER.NAME} {?AMOUNT > 0}taken prisoner{?}released{\\?}.", null);
				textObject.SetTextVariable("AMOUNT", amount);
				textObject.SetCharacterProperties("CHARACTER", characterObject2, false);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				CharacterObject characterObject3 = characterGetter();
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=X3nxRojo}{?AMOUNT > 0}Take{?}Release{\\?} {ABS(AMOUNT)} {CHARACTER.NAME} {?AMOUNT > 0}as {?AMOUNT > 1}prisoners{?}prisoner{\\?}{?}{\\?}", null);
				}
				else
				{
					textObject2 = new TextObject("{=EgivK0mt}{CHANCE}% chance of {?AMOUNT > 0}taking{?}releasing{\\?} {ABS(AMOUNT)} {CHARACTER.NAME}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				textObject2.SetCharacterProperties("CHARACTER", characterObject3, false);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x000971E0 File Offset: 0x000953E0
		public static IncidentEffect GoldChange(Func<int> amountGetter)
		{
			return new IncidentEffect(delegate
			{
				int num = amountGetter();
				return Hero.MainHero.Gold >= -num;
			}, delegate
			{
				int num2 = amountGetter();
				if (num2 > 0)
				{
					GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, num2, false);
				}
				else
				{
					GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, MathF.Abs(num2), false);
				}
				return new List<TextObject>();
			}, delegate(IncidentEffect effect)
			{
				int num3 = amountGetter();
				TextObject textObject;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject = new TextObject("{=YGgPUH3r}{?AMOUNT > 0}Earn{?}Lose{\\?} {ABS(AMOUNT)}{GOLD_ICON}", null);
				}
				else
				{
					textObject = new TextObject("{=Lo1o8fqp}{CHANCE}% chance to {?AMOUNT > 0}earn{?}lose{\\?} {ABS(AMOUNT)}{GOLD_ICON}", null);
					textObject.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject.SetTextVariable("AMOUNT", num3);
				return new IncidentHint(textObject, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x00097224 File Offset: 0x00095424
		public static IncidentEffect TraitChange(TraitObject trait, int amount)
		{
			return new IncidentEffect(null, delegate
			{
				TraitLevelingHelper.OnIncidentResolved(trait, amount);
				TextObject textObject = new TextObject("{=UM8ZOtar}Increased reputation for being {TRAIT}.", null);
				textObject.SetTextVariable("TRAIT", HeroHelper.GetPersonalityTraitChangeName(trait, Hero.MainHero, amount >= 0));
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=fnkkMRl0}Increase reputation for being {TRAIT}", null);
				}
				else
				{
					textObject2 = new TextObject("{=9mGtDERC}{CHANCE}% chance of increasing reputation for being {TRAIT}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("TRAIT", HeroHelper.GetPersonalityTraitChangeName(trait, Hero.MainHero, amount >= 0));
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x00097264 File Offset: 0x00095464
		public static IncidentEffect BuildingLevelChange(Func<Building> buildingGetter, Func<int> amountGetter)
		{
			return new IncidentEffect(delegate
			{
				Func<int> amountGetter2 = amountGetter;
				int? num = ((amountGetter2 != null) ? new int?(amountGetter2()) : null);
				Func<Building> buildingGetter2 = buildingGetter;
				Building building = ((buildingGetter2 != null) ? buildingGetter2() : null);
				if (building != null)
				{
					int? num2 = num;
					int num3 = 0;
					if (!((num2.GetValueOrDefault() == num3) & (num2 != null)))
					{
						num2 = num;
						num3 = 0;
						if ((num2.GetValueOrDefault() > num3) & (num2 != null))
						{
							num2 = building.CurrentLevel + num;
							num3 = 3;
							if ((num2.GetValueOrDefault() <= num3) & (num2 != null))
							{
								return true;
							}
						}
						num2 = num;
						num3 = 0;
						if ((num2.GetValueOrDefault() < num3) & (num2 != null))
						{
							num2 = building.CurrentLevel + num;
							num3 = building.BuildingType.StartLevel;
							return (num2.GetValueOrDefault() >= num3) & (num2 != null);
						}
						return false;
					}
				}
				return false;
			}, delegate
			{
				int num4 = amountGetter();
				Building building2 = buildingGetter();
				if (num4 > 0)
				{
					for (int i = 0; i < num4; i++)
					{
						int constructionCost = building2.GetConstructionCost();
						building2.CurrentLevel++;
						building2.BuildingProgress -= (float)constructionCost;
					}
				}
				else
				{
					building2.CurrentLevel += num4;
					building2.BuildingProgress = 0f;
				}
				CampaignEventDispatcher.Instance.OnBuildingLevelChanged(building2.Town, building2, num4);
				TextObject textObject = new TextObject("{=bdfoDUM0}{?AMOUNT > 0}Increased{?}Decreased{\\?} {BUILDING} level by {ABS(AMOUNT)}.", null);
				textObject.SetTextVariable("BUILDING", building2.Name);
				textObject.SetTextVariable("AMOUNT", num4);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				int num5 = amountGetter();
				Building building3 = buildingGetter();
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=nAft4SaD}{?AMOUNT > 0}Increase{?}Decrease{\\?} {BUILDING} level by {ABS(AMOUNT)}.", null);
				}
				else
				{
					textObject2 = new TextObject("{=BaffFLtX}{CHANCE}% chance of {=*}{?AMOUNT > 0}increasing{?}decreasing{\\?} {BUILDING} level by {ABS(AMOUNT)}.", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("BUILDING", building3.Name);
				textObject2.SetTextVariable("AMOUNT", num5);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x000972B0 File Offset: 0x000954B0
		public static IncidentEffect SiegeProgressChange(Func<float> amountGetter)
		{
			return new IncidentEffect(delegate
			{
				SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
				bool flag;
				if (playerSiegeEvent == null)
				{
					flag = null != null;
				}
				else
				{
					BesiegerCamp besiegerCamp = playerSiegeEvent.BesiegerCamp;
					if (besiegerCamp == null)
					{
						flag = null != null;
					}
					else
					{
						SiegeEvent.SiegeEnginesContainer siegeEngines = besiegerCamp.SiegeEngines;
						flag = ((siegeEngines != null) ? siegeEngines.SiegePreparations : null) != null;
					}
				}
				return flag && !PlayerSiege.PlayerSiegeEvent.BesiegerCamp.IsPreparationComplete;
			}, delegate
			{
				float num = amountGetter();
				PlayerSiege.PlayerSiegeEvent.BesiegerCamp.SiegeEngines.SiegePreparations.SetProgress(PlayerSiege.PlayerSiegeEvent.BesiegerCamp.SiegeEngines.SiegePreparations.Progress + num);
				TextObject textObject = new TextObject("{=C0kUpB48}{?AMOUNT > 0}Increased{?}Decreased{\\?} siege progress by {ABS(AMOUNT)}%.", null);
				textObject.SetTextVariable("AMOUNT", MathF.Round(num * 100f));
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				float num2 = amountGetter();
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=aqE0C4s8}{?AMOUNT > 0}Increase{?}Decrease{\\?} siege progress by {ABS(AMOUNT)}%", null);
				}
				else
				{
					textObject2 = new TextObject("{=BaffFLtX}{CHANCE}% chance of {?AMOUNT > 0}increasing{?}decreasing{\\?} siege progress by {ABS(AMOUNT)}%", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", MathF.Round(num2 * 100f));
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x00097308 File Offset: 0x00095508
		public static IncidentEffect WorkshopProfitabilityChange(Func<Workshop> workshopGetter, float percentage)
		{
			return new IncidentEffect(delegate
			{
				Func<Workshop> workshopGetter2 = workshopGetter;
				return ((workshopGetter2 != null) ? workshopGetter2() : null) != null;
			}, delegate
			{
				Workshop workshop = workshopGetter();
				int num = MathF.Round((float)workshop.ProfitMade * (1f / Campaign.Current.Models.ClanFinanceModel.RevenueSmoothenFraction()) * percentage);
				workshop.ChangeGold(num);
				TextObject textObject = new TextObject("{=9bIi78RK}{WORKSHOP} {?PERCENTAGE > 0}gained{?}lost{\\?} {AMOUNT} gold.", null);
				textObject.SetTextVariable("WORKSHOP", workshop.Name);
				textObject.SetTextVariable("PERCENTAGE", percentage, 2);
				textObject.SetTextVariable("AMOUNT", Math.Abs(num));
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=rMWw9ieF}Workshop {?PERCENTAGE > 0}gains{?}loses{\\?} {ABS(PERCENTAGE)}% of its revenue.", null);
				}
				else
				{
					textObject2 = new TextObject("{=s8DBEakZ}{CHANCE}% chance of workshop {?PERCENTAGE > 0}gaining{?}losing{\\?} {ABS(PERCENTAGE)}% of its revenue.", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("PERCENTAGE", percentage * 100f, 2);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x00097354 File Offset: 0x00095554
		public static IncidentEffect SkillChange(SkillObject skill, float amount)
		{
			return new IncidentEffect(() => true, delegate
			{
				ExplainedNumber explainedNumber = Campaign.Current.Models.CharacterDevelopmentModel.CalculateLearningRate(Hero.MainHero.CharacterAttributes, Hero.MainHero.HeroDeveloper.GetFocus(skill), Hero.MainHero.GetSkillValue(skill), skill, false);
				bool flag = explainedNumber.ResultNumber >= 1f;
				Hero.MainHero.HeroDeveloper.AddSkillXp(skill, amount, flag, true);
				float num = amount;
				if (flag)
				{
					num *= explainedNumber.ResultNumber;
				}
				TextObject textObject = new TextObject("{=ySoK6FLl}{?AMOUNT > 0}Gained{?}Lost{\\?} {ABS(AMOUNT)} {SKILL} XP.", null);
				textObject.SetTextVariable("AMOUNT", MathF.Round(num));
				textObject.SetTextVariable("SKILL", skill.Name);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=7aKxuBVH}+{AMOUNT} XP {SKILL}", null);
				}
				else
				{
					textObject2 = new TextObject("{=ZucFKCqy}{CHANCE}% chance of +{AMOUNT} XP {SKILL}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				ExplainedNumber explainedNumber2 = Campaign.Current.Models.CharacterDevelopmentModel.CalculateLearningRate(Hero.MainHero.CharacterAttributes, Hero.MainHero.HeroDeveloper.GetFocus(skill), Hero.MainHero.GetSkillValue(skill), skill, false);
				bool flag2 = explainedNumber2.ResultNumber >= 1f;
				float num2 = amount;
				if (flag2)
				{
					num2 *= explainedNumber2.ResultNumber;
				}
				textObject2.SetTextVariable("AMOUNT", MathF.Round(num2));
				textObject2.SetTextVariable("SKILL", skill.Name);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x000973B4 File Offset: 0x000955B4
		public static IncidentEffect MoraleChange(float amount)
		{
			return new IncidentEffect(null, delegate
			{
				MobileParty.MainParty.RecentEventsMorale += amount;
				TextObject textObject = new TextObject("{=QG50JVu8}{?AMOUNT > 0}Increased{?}Decreased{\\?} morale by {ABS(AMOUNT)}", null);
				textObject.SetTextVariable("AMOUNT", amount, 2);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=YAyISGjV}{?AMOUNT > 0}Increase{?}Decrease{\\?} morale by {ABS(AMOUNT)}", null);
				}
				else
				{
					textObject2 = new TextObject("{=I8WRkX2a}{CHANCE}% chance of {?AMOUNT > 0}increasing{?}decreasing{\\?} morale by {ABS(AMOUNT)}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount, 2);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x000973EC File Offset: 0x000955EC
		public static IncidentEffect HealthChance(int amount)
		{
			return new IncidentEffect(null, delegate
			{
				Hero.MainHero.HitPoints += amount;
				TextObject textObject = new TextObject("{=sPa4O70I}{?AMOUNT > 0}Healed{?}Lost{\\?} {ABS(AMOUNT)} hit points.", null);
				textObject.SetTextVariable("AMOUNT", amount);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=94UTHJCl}{?AMOUNT > 0}Heal{?}Lose{\\?} {ABS(AMOUNT)} hit points", null);
				}
				else
				{
					textObject2 = new TextObject("{=t7YszJ1w}{CHANCE}% chance of {?AMOUNT > 0}healing{?}losing{\\?} {ABS(AMOUNT)} hit points", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x00097424 File Offset: 0x00095624
		public static IncidentEffect RenownChange(float amount)
		{
			return new IncidentEffect(() => true, delegate
			{
				GainRenownAction.Apply(Hero.MainHero, amount, false);
				TextObject textObject = new TextObject("{=NHzq8L83}Gained {AMOUNT} renown.", null);
				textObject.SetTextVariable("AMOUNT", amount, 2);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=noOOudW8}Gain {AMOUNT} renown", null);
				}
				else
				{
					textObject2 = new TextObject("{=6a8nEuqX}{CHANCE}% chance of gaining {AMOUNT} renown", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount, 2);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x0009747C File Offset: 0x0009567C
		public static IncidentEffect CrimeRatingChange(Func<IFaction> factionGetter, float amount)
		{
			return new IncidentEffect(delegate
			{
				Func<IFaction> factionGetter2 = factionGetter;
				return ((factionGetter2 != null) ? factionGetter2() : null) != null;
			}, delegate
			{
				ChangeCrimeRatingAction.Apply(factionGetter(), amount, true);
				TextObject textObject = new TextObject("{=V2CGB9Sw}{?AMOUNT > 0}Increased{?}Decreased{\\?} crime rating by {ABS(AMOUNT)} in {FACTION}.", null);
				textObject.SetTextVariable("AMOUNT", amount, 2);
				textObject.SetTextVariable("FACTION", factionGetter().Name);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=t01zXDvG}{?AMOUNT > 0}Increase{?}Decrease{\\?} crime rating by {ABS(AMOUNT)} in {FACTION}", null);
				}
				else
				{
					textObject2 = new TextObject("{=b029BWMC}{CHANCE}% chance of {?AMOUNT > 0}increasing{?}decreasing{\\?} crime rating by {ABS(AMOUNT)} in {FACTION}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount, 2);
				textObject2.SetTextVariable("FACTION", factionGetter().Name);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x000974C8 File Offset: 0x000956C8
		public static IncidentEffect InfluenceChange(float amount)
		{
			return new IncidentEffect(() => Hero.MainHero.Clan.Kingdom != null, delegate
			{
				float num = ((Hero.MainHero.Clan.Influence + amount > 0f) ? amount : (-Hero.MainHero.Clan.Influence));
				GainKingdomInfluenceAction.ApplyForDefault(Hero.MainHero, num);
				TextObject textObject = new TextObject("{=MW0ah7pi}{?AMOUNT > 0}Gained{?}Lost{\\?} {ABS(AMOUNT)} influence.", null);
				textObject.SetTextVariable("AMOUNT", num, 2);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				float num2 = ((Hero.MainHero.Clan.Influence + amount > 0f) ? amount : (-Hero.MainHero.Clan.Influence));
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=3a3D6aAt}{?AMOUNT > 0}Gain{?}Lose{\\?} {ABS(AMOUNT)} influence", null);
				}
				else
				{
					textObject2 = new TextObject("{=3K85XqUs}{CHANCE}% chance of {?AMOUNT > 0}gaining{?}losing{\\?} {ABS(AMOUNT)} influence", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", num2, 2);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x00097520 File Offset: 0x00095720
		public static IncidentEffect SettlementRelationChange(Func<Settlement> settlementGetter, int amount, Func<Hero, bool> notablePredicate = null)
		{
			return new IncidentEffect(delegate
			{
				Func<Settlement> settlementGetter2 = settlementGetter;
				return ((settlementGetter2 != null) ? settlementGetter2() : null) != null;
			}, delegate
			{
				Settlement settlement = settlementGetter();
				List<TextObject> list = new List<TextObject>();
				foreach (Hero hero in settlement.Notables)
				{
					if (notablePredicate == null || notablePredicate(hero))
					{
						ChangeRelationAction.ApplyPlayerRelation(hero, amount, true, true);
						TextObject textObject = new TextObject("{=8IzNumMa}{?AMOUNT > 0}Increased{?}Decreased{\\?} relationship with {SETTLEMENT} by {ABS(AMOUNT)}.", null);
						textObject.SetTextVariable("AMOUNT", amount);
						textObject.SetTextVariable("SETTLEMENT", settlement.Name);
						list.Add(textObject);
					}
				}
				return list;
			}, delegate(IncidentEffect effect)
			{
				Settlement settlement2 = settlementGetter();
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=PaKstm1Q}{?AMOUNT > 0}Increase{?}Decrease{\\?} relationship with notables in {SETTLEMENT} by {ABS(AMOUNT)}", null);
				}
				else
				{
					textObject2 = new TextObject("{=8yruI4lJ}{CHANCE}% chance of {?AMOUNT > 0}increasing{?}decreasing{\\?} relationship with notables in {SETTLEMENT} by {ABS(AMOUNT)}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				textObject2.SetTextVariable("SETTLEMENT", settlement2.Name);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x00097574 File Offset: 0x00095774
		public static IncidentEffect TownBoundVillageRelationChange(Func<Town> townGetter, int amount)
		{
			return new IncidentEffect(delegate
			{
				Func<Town> townGetter2 = townGetter;
				return ((townGetter2 != null) ? townGetter2() : null) != null && townGetter().Villages.Count > 0;
			}, delegate
			{
				Town town = townGetter();
				List<TextObject> list = new List<TextObject>();
				foreach (Village village in town.Villages)
				{
					foreach (Hero hero in village.Settlement.Notables)
					{
						ChangeRelationAction.ApplyPlayerRelation(hero, amount, true, true);
					}
					TextObject textObject = new TextObject("{=8IzNumMa}{?AMOUNT > 0}Increased{?}Decreased{\\?} relationship with {SETTLEMENT} by {ABS(AMOUNT)}.", null);
					textObject.SetTextVariable("AMOUNT", amount);
					textObject.SetTextVariable("SETTLEMENT", village.Name);
					list.Add(textObject);
				}
				return list;
			}, delegate(IncidentEffect effect)
			{
				Town town2 = townGetter();
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=SwEQJC4s}{?AMOUNT > 0}Increase{?}Decrease{\\?} relationship with each bound village of {SETTLEMENT} by {ABS(AMOUNT)}", null);
				}
				else
				{
					textObject2 = new TextObject("{=bSMXHl3A}{CHANCE}% chance of {?AMOUNT > 0}increasing{?}decreasing{\\?} relationship with each bound village of {SETTLEMENT} by {ABS(AMOUNT)}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				textObject2.SetTextVariable("SETTLEMENT", town2.Name);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x000975C0 File Offset: 0x000957C0
		public static IncidentEffect TownBoundVillageHearthChange(Func<Town> townGetter, int amount)
		{
			return new IncidentEffect(delegate
			{
				Func<Town> townGetter2 = townGetter;
				return ((townGetter2 != null) ? townGetter2() : null) != null && townGetter().Villages.Count > 0;
			}, delegate
			{
				Town town = townGetter();
				List<TextObject> list = new List<TextObject>();
				foreach (Village village in town.Villages)
				{
					village.Hearth += (float)amount;
					TextObject textObject = new TextObject("{=qMCaLtKm}{?AMOUNT > 0}Increased{?}Decreased{\\?} hearth of {SETTLEMENT} by {ABS(AMOUNT)}.", null);
					textObject.SetTextVariable("AMOUNT", amount);
					textObject.SetTextVariable("SETTLEMENT", village.Name);
					list.Add(textObject);
				}
				return list;
			}, delegate(IncidentEffect effect)
			{
				Town town2 = townGetter();
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=lWOTWQdF}{?AMOUNT > 0}Increase{?}Decrease{\\?} hearth of each bound village of {SETTLEMENT} by {ABS(AMOUNT)}", null);
				}
				else
				{
					textObject2 = new TextObject("{=pBNxbnBk}{CHANCE}% chance of {?AMOUNT > 0}increasing{?}decreasing{\\?} hearth of each bound village of {SETTLEMENT} by {ABS(AMOUNT)}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				textObject2.SetTextVariable("SETTLEMENT", town2.Name);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x0009760C File Offset: 0x0009580C
		public static IncidentEffect VillageHearthChange(Func<Village> villageGetter, int amount)
		{
			return new IncidentEffect(delegate
			{
				Func<Village> villageGetter2 = villageGetter;
				return ((villageGetter2 != null) ? villageGetter2() : null) != null;
			}, delegate
			{
				Village village = villageGetter();
				village.Hearth += (float)amount;
				TextObject textObject = new TextObject("{=qMCaLtKm}{?AMOUNT > 0}Increased{?}Decreased{\\?} hearth of {SETTLEMENT} by {ABS(AMOUNT)}.", null);
				textObject.SetTextVariable("AMOUNT", amount);
				textObject.SetTextVariable("SETTLEMENT", village.Name);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				Village village2 = villageGetter();
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=ZpE5eWj9}{?AMOUNT > 0}Increase{?}Decrease{\\?} {VILLAGE} hearth by {ABS(AMOUNT)}", null);
				}
				else
				{
					textObject2 = new TextObject("{=bSTaPVG7}{CHANCE}% chance of {?AMOUNT > 0}increasing{?}decreasing{\\?} {VILLAGE} hearth by {ABS(AMOUNT)}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				textObject2.SetTextVariable("VILLAGE", village2.Name);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x00097658 File Offset: 0x00095858
		public static IncidentEffect TownSecurityChange(Func<Town> townGetter, int amount)
		{
			return new IncidentEffect(delegate
			{
				Func<Town> townGetter2 = townGetter;
				return ((townGetter2 != null) ? townGetter2() : null) != null;
			}, delegate
			{
				Town town = townGetter();
				town.Security += (float)amount;
				TextObject textObject = new TextObject("{=ShfAk7aC}{?AMOUNT > 0}Increased{?}Decreased{\\?} {TOWN} security by {ABS(AMOUNT)}.", null);
				textObject.SetTextVariable("AMOUNT", amount);
				textObject.SetTextVariable("TOWN", town.Name);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				Town town2 = townGetter();
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=sJ9OpLKp}{?AMOUNT > 0}Increase{?}Decrease{\\?} {TOWN} security by {ABS(AMOUNT)}", null);
				}
				else
				{
					textObject2 = new TextObject("{=6E7sQIfW}{CHANCE}% chance of {?AMOUNT > 0}increasing{?}decreasing{\\?} {TOWN} security by {ABS(AMOUNT)}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				textObject2.SetTextVariable("TOWN", town2.Name);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x000976A4 File Offset: 0x000958A4
		public static IncidentEffect HeroRelationChange(Func<Hero> heroGetter, int amount)
		{
			return new IncidentEffect(delegate
			{
				Func<Hero> heroGetter2 = heroGetter;
				return ((heroGetter2 != null) ? heroGetter2() : null) != null;
			}, delegate
			{
				ChangeRelationAction.ApplyPlayerRelation(heroGetter(), amount, true, true);
				return new List<TextObject>();
			}, delegate(IncidentEffect effect)
			{
				Hero hero = heroGetter();
				TextObject textObject;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject = new TextObject("{=T8GYg3tv}{?AMOUNT > 0}Increase{?}Decrease{\\?} relationship with {HERO.NAME} by {ABS(AMOUNT)}", null);
				}
				else
				{
					textObject = new TextObject("{=6tnegb19}{CHANCE}% chance of {?AMOUNT > 0}increasing{?}decreasing{\\?} relationship with {HERO.NAME} by {ABS(AMOUNT)}", null);
					textObject.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject.SetTextVariable("AMOUNT", amount);
				textObject.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return new IncidentHint(textObject, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x000976F0 File Offset: 0x000958F0
		public static IncidentEffect TownProsperityChange(Func<Town> townGetter, int amount)
		{
			return new IncidentEffect(delegate
			{
				Func<Town> townGetter2 = townGetter;
				return ((townGetter2 != null) ? townGetter2() : null) != null;
			}, delegate
			{
				Town town = townGetter();
				town.Prosperity += (float)amount;
				TextObject textObject = new TextObject("{=gd2Ppaae}{?AMOUNT > 0}Increased{?}Decreased{\\?} {TOWN}'s prosperity by {ABS(AMOUNT)}.", null);
				textObject.SetTextVariable("AMOUNT", amount);
				textObject.SetTextVariable("TOWN", town.Name);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				Town town2 = townGetter();
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=sBoVFny0}{?AMOUNT > 0}Increase{?}Decrease{\\?} {TOWN}'s prosperity by {ABS(AMOUNT)}", null);
				}
				else
				{
					textObject2 = new TextObject("{=CirbWpGB}{CHANCE}% chance of {?AMOUNT > 0}increasing{?}decreasing{\\?} {TOWN}'s prosperity by {ABS(AMOUNT)}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("TOWN", town2.Name);
				textObject2.SetTextVariable("AMOUNT", amount);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x0009773C File Offset: 0x0009593C
		public static IncidentEffect SettlementMilitiaChange(Func<Settlement> settlementGetter, int amount)
		{
			return new IncidentEffect(delegate
			{
				Func<Settlement> settlementGetter2 = settlementGetter;
				Settlement settlement = ((settlementGetter2 != null) ? settlementGetter2() : null);
				return settlement != null && settlement.Militia > (float)(-(float)amount);
			}, delegate
			{
				Settlement settlement2 = settlementGetter();
				settlement2.Militia += (float)amount;
				TextObject textObject = new TextObject("{=Zu4loCJR}{?AMOUNT > 0}Increased{?}Decreased{\\?} {SETTLEMENT}'s militia by {ABS(AMOUNT)}.", null);
				textObject.SetTextVariable("SETTLEMENT", settlement2.Name);
				textObject.SetTextVariable("AMOUNT", amount);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				Settlement settlement3 = settlementGetter();
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=UUXAl3un}{?AMOUNT > 0}Increase{?}Decrease{\\?} {SETTLEMENT}'s militia by {ABS(AMOUNT)}", null);
				}
				else
				{
					textObject2 = new TextObject("{=b2Iu3WsA}{CHANCE}% chance of {?AMOUNT > 0}increasing{?}decreasing{\\?} {SETTLEMENT}'s militia by {ABS(AMOUNT)}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("SETTLEMENT", settlement3.Name);
				textObject2.SetTextVariable("AMOUNT", amount);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x00097788 File Offset: 0x00095988
		public static IncidentEffect InfestNearbyHideout(Func<Settlement> settlementGetter)
		{
			return new IncidentEffect(delegate
			{
				Func<Settlement> settlementGetter2 = settlementGetter;
				return ((settlementGetter2 != null) ? settlementGetter2() : null) != null;
			}, delegate
			{
				Hideout hideout = SettlementHelper.FindNearestHideoutToSettlement(settlementGetter(), MobileParty.NavigationType.Default, (Settlement settlement) => settlement.IsHideout && !settlement.IsVisible);
				BanditSpawnCampaignBehavior behavior = Campaign.Current.CampaignBehaviorManager.GetBehavior<BanditSpawnCampaignBehavior>();
				if (hideout == null)
				{
					return new List<TextObject>();
				}
				if (!hideout.IsInfested)
				{
					int num = Campaign.Current.Models.BanditDensityModel.NumberOfMinimumBanditPartiesInAHideoutToInfestIt - hideout.Settlement.Parties.Count<MobileParty>((MobileParty x) => x.IsBandit);
					for (int i = 0; i < num; i++)
					{
						behavior.AddBanditToHideout(hideout, null, false);
					}
				}
				hideout.Settlement.IsVisible = false;
				CampaignEventDispatcher.Instance.OnHideoutSpotted(MobileParty.MainParty.Party, hideout.Settlement.Party);
				return new List<TextObject>();
			}, delegate(IncidentEffect effect)
			{
				Settlement settlement = settlementGetter();
				TextObject textObject;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject = new TextObject("{=VIMgmfp8}Infest a hideout nearby {SETTLEMENT}", null);
				}
				else
				{
					textObject = new TextObject("{=5HYYbGDe}{CHANCE}% chance of infesting a hideout nearby {SETTLEMENT}", null);
					textObject.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject.SetTextVariable("SETTLEMENT", settlement.Name);
				return new IncidentHint(textObject, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x000977CC File Offset: 0x000959CC
		public static IncidentEffect WoundTroopsRandomly(float percentage)
		{
			return new IncidentEffect(() => (float)(MobileParty.MainParty.MemberRoster.TotalRegulars - MobileParty.MainParty.MemberRoster.TotalWoundedRegulars) >= (float)MobileParty.MainParty.MemberRoster.TotalRegulars * percentage, delegate
			{
				int num = MathF.Round((float)MobileParty.MainParty.MemberRoster.TotalRegulars * percentage);
				MobileParty.MainParty.MemberRoster.WoundNumberOfNonHeroTroopsRandomly(num);
				TextObject textObject = new TextObject("{=GMF8G6IQ}{AMOUNT} of your troops got wounded.", null);
				textObject.SetTextVariable("AMOUNT", num);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=5opu0SNu}{AMOUNT}% of your troops get wounded", null);
				}
				else
				{
					textObject2 = new TextObject("{=lQa4geng}{CHANCE}% chance of {AMOUNT}% of your troops getting wounded", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", percentage * 100f, 2);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x00097810 File Offset: 0x00095A10
		public static IncidentEffect WoundTroopsRandomly(Func<TroopRosterElement, bool> predicate, Func<int> amountGetter, bool specifyUnitTypeOnHint = true)
		{
			return new IncidentEffect(delegate
			{
				IEnumerable<TroopRosterElement> enumerable = MobileParty.MainParty.MemberRoster.GetTroopRoster().Where<TroopRosterElement>(predicate).ToList<TroopRosterElement>();
				int num = amountGetter();
				return enumerable.Sum<TroopRosterElement>((TroopRosterElement x) => x.Number - x.WoundedNumber) > num;
			}, delegate
			{
				List<TroopRosterElement> list = MobileParty.MainParty.MemberRoster.GetTroopRoster().Where<TroopRosterElement>(predicate).ToList<TroopRosterElement>();
				int i = amountGetter();
				TextObject textObject = new TextObject("{=GMF8G6IQ}{AMOUNT} of your troops got wounded.", null);
				textObject.SetTextVariable("AMOUNT", i);
				while (i > 0)
				{
					if (!list.Any<TroopRosterElement>((TroopRosterElement x) => x.Number > x.WoundedNumber))
					{
						break;
					}
					TroopRosterElement randomElementWithPredicate = list.GetRandomElementWithPredicate<TroopRosterElement>((TroopRosterElement x) => x.Number > x.WoundedNumber);
					int num2 = randomElementWithPredicate.Number - randomElementWithPredicate.WoundedNumber;
					int num3 = ((i > num2) ? num2 : MBRandom.RandomInt(1, i + 1));
					MobileParty.MainParty.MemberRoster.WoundTroop(randomElementWithPredicate.Character, num3, default(UniqueTroopDescriptor));
					i -= num3;
				}
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (specifyUnitTypeOnHint)
				{
					if (effect.ChanceToOccur >= 1f)
					{
						textObject2 = new TextObject("{=Q4j44aOp}{AMOUNT} {UNIT_TYPE} get wounded", null);
					}
					else
					{
						textObject2 = new TextObject("{=oXtob6vV}{CHANCE}% chance of {AMOUNT} {UNIT_TYPE} getting wounded", null);
						textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
					}
					TroopRosterElement randomElement = MobileParty.MainParty.MemberRoster.GetTroopRoster().Where<TroopRosterElement>(predicate).ToList<TroopRosterElement>()
						.GetRandomElement<TroopRosterElement>();
					textObject2.SetTextVariable("UNIT_TYPE", randomElement.Character.DefaultFormationClass.GetName());
				}
				else if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=3SDefWu5}{AMOUNT} random troops get wounded", null);
				}
				else
				{
					textObject2 = new TextObject("{=Owc4NdbL}{CHANCE}% chance of {AMOUNT} random troops getting wounded", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amountGetter());
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x00097864 File Offset: 0x00095A64
		public static IncidentEffect WoundTroopsRandomlyWithChanceOfDeath(float percentage, float chanceOfDeathPerUnit)
		{
			return new IncidentEffect(() => (float)(MobileParty.MainParty.MemberRoster.TotalRegulars - MobileParty.MainParty.MemberRoster.TotalWoundedRegulars) >= (float)MobileParty.MainParty.MemberRoster.TotalRegulars * percentage, delegate
			{
				int num = MathF.Round((float)MobileParty.MainParty.MemberRoster.TotalRegulars * percentage);
				int num2;
				MobilePartyHelper.WoundNumberOfNonHeroTroopsRandomlyWithChanceOfDeath(MobileParty.MainParty.MemberRoster, num, chanceOfDeathPerUnit, out num2);
				int num3 = num - num2;
				List<TextObject> list = new List<TextObject>();
				if (num2 > 0)
				{
					if (num3 == 0)
					{
						TextObject textObject = new TextObject("{=ni1m6VDh}{AMOUNT} of your troops died.", null);
						textObject.SetTextVariable("AMOUNT", num2);
						list.Add(textObject);
					}
					else
					{
						TextObject textObject2 = new TextObject("{=zXmhbszd}{AMOUNT} of your troops got wounded, and {AMOUNT_DEAD} died.", null);
						textObject2.SetTextVariable("AMOUNT", num3);
						textObject2.SetTextVariable("AMOUNT_DEAD", num2);
						list.Add(textObject2);
					}
				}
				else
				{
					TextObject textObject3 = new TextObject("{=GMF8G6IQ}{AMOUNT} of your troops got wounded.", null);
					textObject3.SetTextVariable("AMOUNT", num3);
					list.Add(textObject3);
				}
				return list;
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject4;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject4 = new TextObject("{=R170aDUL}{AMOUNT}% of your troops get wounded and each unit has {DEATH_CHANCE}% chance of dying", null);
				}
				else
				{
					textObject4 = new TextObject("{=9Mjatr99}{CHANCE}% chance of {AMOUNT}% of your troops getting wounded and each unit has {DEATH_CHANCE}% chance of dying", null);
					textObject4.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject4.SetTextVariable("DEATH_CHANCE", MathF.Round(chanceOfDeathPerUnit * 100f));
				textObject4.SetTextVariable("AMOUNT", MathF.Round(percentage * 100f));
				return new IncidentHint(textObject4, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x000978B0 File Offset: 0x00095AB0
		public static IncidentEffect BreachSiegeWall(int amount)
		{
			return new IncidentEffect(() => MobileParty.MainParty.SiegeEvent != null, delegate
			{
				MBReadOnlyList<float> hitPointRatioList = PlayerSiege.BesiegedSettlement.SettlementWallSectionHitPointsRatioList;
				int num = amount;
				List<int> list = (from x in Enumerable.Range(0, hitPointRatioList.Count)
					where hitPointRatioList[x] > 0f
					select x).ToList<int>();
				while (num > 0 && list.Count > 0)
				{
					int randomElement = list.GetRandomElement<int>();
					PlayerSiege.BesiegedSettlement.SetWallSectionHitPointsRatioAtIndex(randomElement, 0f);
					num--;
				}
				PlayerSiege.BesiegedSettlement.Party.SetVisualAsDirty();
				TextObject textObject = new TextObject("{=WXCl7J36}{?AMOUNT > 1}Walls{?}Wall{\\?} breached successfully.", null);
				textObject.SetTextVariable("AMOUNT", amount - num);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=zDqwzpYf}Breach {AMOUNT} siege {?AMOUNT > 1}walls{?}wall{\\?}", null);
				}
				else
				{
					textObject2 = new TextObject("{=KCLDqhsV}{CHANCE}% chance of breaching {AMOUNT} siege {?AMOUNT > 1}walls{?}wall{\\?}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x00097908 File Offset: 0x00095B08
		public static IncidentEffect WoundTroopsRandomly(int amount)
		{
			return new IncidentEffect(() => MobileParty.MainParty.MemberRoster.TotalRegulars - MobileParty.MainParty.MemberRoster.TotalWoundedRegulars >= amount, delegate
			{
				MobileParty.MainParty.MemberRoster.WoundNumberOfNonHeroTroopsRandomly(amount);
				TextObject textObject = new TextObject("{=GMF8G6IQ}{AMOUNT} of your troops got wounded.", null);
				textObject.SetTextVariable("AMOUNT", amount);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=UufjeVy5}{AMOUNT} of your troops {?AMOUNT > 1}get{?}gets{\\?} wounded", null);
				}
				else
				{
					textObject2 = new TextObject("{=2DID1Cju}{CHANCE}% chance of {AMOUNT} of your troops getting wounded", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x0009794C File Offset: 0x00095B4C
		public static IncidentEffect WoundTroopsRandomlyWithChanceOfDeath(int amount, float chanceOfDeathPerUnit)
		{
			return new IncidentEffect(() => MobileParty.MainParty.MemberRoster.TotalRegulars - MobileParty.MainParty.MemberRoster.TotalWoundedRegulars >= amount, delegate
			{
				int num;
				MobilePartyHelper.WoundNumberOfNonHeroTroopsRandomlyWithChanceOfDeath(MobileParty.MainParty.MemberRoster, amount, chanceOfDeathPerUnit, out num);
				int num2 = amount - num;
				List<TextObject> list = new List<TextObject>();
				if (num > 0)
				{
					if (num2 == 0)
					{
						TextObject textObject = new TextObject("{=ni1m6VDh}{AMOUNT} of your troops died.", null);
						textObject.SetTextVariable("AMOUNT", num);
						list.Add(textObject);
					}
					else
					{
						TextObject textObject2 = new TextObject("{=zXmhbszd}{AMOUNT} of your troops got wounded, and {AMOUNT_DEAD} died.", null);
						textObject2.SetTextVariable("AMOUNT", num2);
						textObject2.SetTextVariable("AMOUNT_DEAD", num);
						list.Add(textObject2);
					}
				}
				else
				{
					TextObject textObject3 = new TextObject("{=GMF8G6IQ}{AMOUNT} of your troops got wounded.", null);
					textObject3.SetTextVariable("AMOUNT", num2);
					list.Add(textObject3);
				}
				return list;
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject4;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject4 = new TextObject("{=n5lPsPKq}{AMOUNT} of your troops {?AMOUNT > 1}get{?}gets{\\?} wounded and each unit has {DEATH_CHANCE}% chance of dying", null);
				}
				else
				{
					textObject4 = new TextObject("{=v679SFPt}{CHANCE}% chance of {AMOUNT} of your troops getting wounded and each unit has {DEATH_CHANCE}% chance of dying", null);
					textObject4.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject4.SetTextVariable("DEATH_CHANCE", MathF.Round(chanceOfDeathPerUnit * 100f));
				textObject4.SetTextVariable("AMOUNT", amount);
				return new IncidentHint(textObject4, IncidentHintType.Effect);
			});
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x00097998 File Offset: 0x00095B98
		public static IncidentEffect WoundTroop(Func<CharacterObject> characterGetter, int amount)
		{
			return new IncidentEffect(delegate
			{
				Func<CharacterObject> characterGetter2 = characterGetter;
				CharacterObject characterObject = ((characterGetter2 != null) ? characterGetter2() : null);
				if (characterObject == null)
				{
					return false;
				}
				int num = MobileParty.MainParty.MemberRoster.FindIndexOfTroop(characterObject);
				if (num == -1)
				{
					return false;
				}
				TroopRosterElement troopRosterElement = MobileParty.MainParty.MemberRoster.GetTroopRoster()[num];
				return troopRosterElement.Number - troopRosterElement.WoundedNumber >= amount;
			}, delegate
			{
				CharacterObject characterObject2 = characterGetter();
				MobileParty.MainParty.MemberRoster.WoundTroop(characterObject2, amount, default(UniqueTroopDescriptor));
				TextObject textObject = new TextObject("{=Cep8OD72}{AMOUNT} {TROOP.NAME} got wounded.", null);
				textObject.SetTextVariable("AMOUNT", amount);
				textObject.SetCharacterProperties("TROOP", characterObject2, false);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				CharacterObject characterObject3 = characterGetter();
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=wFtO2y5R}{AMOUNT} {TROOP.NAME} gets wounded", null);
				}
				else
				{
					textObject2 = new TextObject("{=PMgz8Dah}{CHANCE}% chance of {AMOUNT} {TROOP.NAME} getting wounded", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				textObject2.SetCharacterProperties("TROOP", characterObject3, false);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x000979E4 File Offset: 0x00095BE4
		public static IncidentEffect WoundTroopsRandomlyByChance(float chancePerUnit)
		{
			return new IncidentEffect(() => MobileParty.MainParty.MemberRoster.TotalRegulars - MobileParty.MainParty.MemberRoster.TotalWoundedRegulars > 0, delegate
			{
				List<TroopRosterElement> list = (from x in MobileParty.MainParty.MemberRoster.GetTroopRoster()
					where !x.Character.IsHero
					select x).ToList<TroopRosterElement>();
				TextObject textObject = new TextObject("{=GMF8G6IQ}{AMOUNT} of your troops got wounded.", null);
				int num = 0;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					TroopRosterElement troopRosterElement = list[i];
					int num2 = 0;
					while (num2 < troopRosterElement.Number - troopRosterElement.WoundedNumber && MBRandom.RandomFloat < chancePerUnit)
					{
						num2++;
					}
					num += num2;
					MobileParty.MainParty.MemberRoster.WoundTroop(troopRosterElement.Character, num2, default(UniqueTroopDescriptor));
				}
				textObject.SetTextVariable("AMOUNT", num);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=bsRI2wUz}Wound troops with {CHANCE_PER_UNIT}% chance each", null);
				}
				else
				{
					textObject2 = new TextObject("{=8k6NCb2S}{CHANCE}% chance of wounding troops with {CHANCE_PER_UNIT}% chance each", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("CHANCE_PER_UNIT", chancePerUnit * 100f, 2);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x00097A3C File Offset: 0x00095C3C
		public static IncidentEffect KillTroopsRandomlyOrderedByTier(Func<TroopRosterElement, bool> predicate, Func<int> amountGetter)
		{
			Func<TroopRosterElement, bool> <>9__3;
			return new IncidentEffect(() => CampaignIncidentEffects.KillTroopsRandomly(predicate, amountGetter, false).Condition(), delegate
			{
				int num = amountGetter();
				int i = num;
				IEnumerable<TroopRosterElement> troopRoster = MobileParty.MainParty.MemberRoster.GetTroopRoster();
				Func<TroopRosterElement, bool> func;
				if ((func = <>9__3) == null)
				{
					func = (<>9__3 = (TroopRosterElement x) => predicate(x) && !x.Character.IsHero);
				}
				List<TroopRosterElement> list = (from x in troopRoster.Where<TroopRosterElement>(func)
					orderby x.Character.Tier
					select x).ToList<TroopRosterElement>();
				while (i > 0)
				{
					TroopRosterElement randomElementInefficiently = (from x in list
						group x by x.Character.Tier).First<IGrouping<int, TroopRosterElement>>().GetRandomElementInefficiently<TroopRosterElement>();
					int num2 = Math.Min(MBRandom.RandomInt(1, randomElementInefficiently.Number + 1), i);
					MobileParty.MainParty.MemberRoster.AddToCounts(randomElementInefficiently.Character, -num2, false, 0, 0, true, -1);
					i -= num2;
					if (num2 == randomElementInefficiently.Number)
					{
						list.Remove(randomElementInefficiently);
					}
					else
					{
						randomElementInefficiently.Number -= num2;
						list[list.IndexOf(randomElementInefficiently)] = randomElementInefficiently;
					}
				}
				TextObject textObject = new TextObject("{=ni1m6VDh}{AMOUNT} of your troops died.", null);
				textObject.SetTextVariable("AMOUNT", num);
				return new List<TextObject> { textObject };
			}, (IncidentEffect effect) => CampaignIncidentEffects.KillTroopsRandomly(predicate, amountGetter, false).GetHint());
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x00097A88 File Offset: 0x00095C88
		public static IncidentEffect KillTroopsRandomly(Func<TroopRosterElement, bool> predicate, Func<int> amountGetter, bool useLostText = false)
		{
			Func<TroopRosterElement, bool> <>9__3;
			Func<TroopRosterElement, bool> <>9__5;
			return new IncidentEffect(delegate
			{
				IEnumerable<TroopRosterElement> troopRoster = MobileParty.MainParty.MemberRoster.GetTroopRoster();
				Func<TroopRosterElement, bool> func;
				if ((func = <>9__3) == null)
				{
					func = (<>9__3 = (TroopRosterElement x) => predicate(x) && !x.Character.IsHero);
				}
				return troopRoster.Where<TroopRosterElement>(func).Sum<TroopRosterElement>((TroopRosterElement x) => x.Number) >= amountGetter();
			}, delegate
			{
				int num = amountGetter();
				int i = num;
				IEnumerable<TroopRosterElement> troopRoster2 = MobileParty.MainParty.MemberRoster.GetTroopRoster();
				Func<TroopRosterElement, bool> func2;
				if ((func2 = <>9__5) == null)
				{
					func2 = (<>9__5 = (TroopRosterElement x) => predicate(x) && !x.Character.IsHero);
				}
				List<TroopRosterElement> list = troopRoster2.Where<TroopRosterElement>(func2).ToList<TroopRosterElement>();
				while (i > 0)
				{
					int num2 = MBRandom.RandomInt(list.Count);
					TroopRosterElement troopRosterElement = list[num2];
					int num3 = MBRandom.RandomInt(1, Math.Min(troopRosterElement.Number + 1, i));
					MobileParty.MainParty.MemberRoster.AddToCounts(troopRosterElement.Character, -num3, false, 0, 0, true, -1);
					i -= num3;
					if (num3 == troopRosterElement.Number)
					{
						list.RemoveAt(num2);
					}
					else
					{
						troopRosterElement.Number -= num3;
						list[num2] = troopRosterElement;
					}
				}
				TextObject textObject = (useLostText ? new TextObject("{=UWH1KUIO}Lost {AMOUNT} of your troops", null) : new TextObject("{=ni1m6VDh}{AMOUNT} of your troops died.", null));
				textObject.SetTextVariable("AMOUNT", num);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=UOEOCf69}Lose {AMOUNT} of your troops", null);
				}
				else
				{
					textObject2 = new TextObject("{=AaiqI22d}{CHANCE}% chance of losing {AMOUNT} of your troops", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amountGetter());
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x00097ADC File Offset: 0x00095CDC
		public static IncidentEffect KillTroopsRandomlyByChance(float chancePerUnit)
		{
			return new IncidentEffect(() => MobileParty.MainParty.MemberRoster.TotalRegulars > 0, delegate
			{
				List<TroopRosterElement> list = (from x in MobileParty.MainParty.MemberRoster.GetTroopRoster()
					where !x.Character.IsHero
					select x).ToList<TroopRosterElement>();
				TextObject textObject = new TextObject("{=ni1m6VDh}{AMOUNT} of your troops died.", null);
				for (int i = list.Count - 1; i >= 0; i--)
				{
					TroopRosterElement troopRosterElement = list[i];
					int num = 0;
					while (num < troopRosterElement.Number && MBRandom.RandomFloat < chancePerUnit)
					{
						num++;
					}
					textObject.SetTextVariable("AMOUNT", num);
					MobileParty.MainParty.MemberRoster.AddToCounts(troopRosterElement.Character, -num, false, 0, 0, true, -1);
				}
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=hXentcdy}Lose troops with {CHANCE_PER_UNIT}% chance each", null);
				}
				else
				{
					textObject2 = new TextObject("{=Zl6LASZq}{CHANCE}% chance of losing troops with {CHANCE_PER_UNIT}% chance each", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("CHANCE_PER_UNIT", chancePerUnit * 100f, 2);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x00097B34 File Offset: 0x00095D34
		public static IncidentEffect KillTroop(Func<CharacterObject> characterGetter, int amount)
		{
			return CampaignIncidentEffects.ChangeTroopAmount(characterGetter, -amount, delegate(IncidentEffect effect)
			{
				CharacterObject characterObject = characterGetter();
				TextObject textObject;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject = new TextObject("{=kHooobeJ}{AMOUNT} {TROOP.NAME} gets killed", null);
				}
				else
				{
					textObject = new TextObject("{=Y6s7AxWu}{CHANCE}% chance of {AMOUNT} {TROOP.NAME} getting killed", null);
					textObject.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject.SetTextVariable("AMOUNT", amount);
				textObject.SetCharacterProperties("TROOP", characterObject, false);
				return new IncidentHint(textObject, IncidentHintType.Effect);
			}, delegate
			{
				TextObject textObject2 = new TextObject("{=ni1m6VDh}{AMOUNT} of your troops died.", null);
				textObject2.SetTextVariable("AMOUNT", amount);
				return new List<TextObject> { textObject2 };
			});
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x00097B80 File Offset: 0x00095D80
		public static IncidentEffect ChangeTroopAmount(Func<CharacterObject> characterGetter, int amount, Func<IncidentEffect, IncidentHint> customHint = null, Func<List<TextObject>> customInformation = null)
		{
			return new IncidentEffect(delegate
			{
				Func<CharacterObject> characterGetter2 = characterGetter;
				CharacterObject characterObject = ((characterGetter2 != null) ? characterGetter2() : null);
				if (characterObject == null)
				{
					return false;
				}
				if (amount >= 0)
				{
					return true;
				}
				int num = MobileParty.MainParty.MemberRoster.FindIndexOfTroop(characterObject);
				return num != -1 && MobileParty.MainParty.MemberRoster.GetElementNumber(num) >= Math.Abs(amount);
			}, delegate
			{
				CharacterObject characterObject2 = characterGetter();
				MobileParty.MainParty.MemberRoster.AddToCounts(characterObject2, amount, false, 0, 0, true, -1);
				TextObject textObject = new TextObject("{=Ckj7L2Sz}{ABS(AMOUNT)} {CHARACTER.NAME} {?AMOUNT > 0}joined{?}left{\\?} your party", null);
				textObject.SetTextVariable("AMOUNT", amount);
				textObject.SetCharacterProperties("CHARACTER", characterObject2, false);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				CharacterObject characterObject3 = characterGetter();
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=AlgTsbSu}{ABS(AMOUNT)} {CHARACTER.NAME} {?AMOUNT > 0}{?ABS(AMOUNT) > 1}join{?}joins{\\?}{?}{?ABS(AMOUNT) > 1}leave{?}leaves{\\?}{\\?} your party", null);
				}
				else
				{
					textObject2 = new TextObject("{=eQv4dYQz}{CHANCE}% chance of {ABS(AMOUNT)} {CHARACTER.NAME} {?AMOUNT > 0}joining{?}leaving{\\?} your party", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				textObject2.SetCharacterProperties("CHARACTER", characterObject3, false);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x00097BCC File Offset: 0x00095DCC
		public static IncidentEffect UpgradeTroop(Func<CharacterObject> characterGetter, Func<CharacterObject, bool> upgradePredicate, int amount, Func<long> incidentSeedGetter)
		{
			return new IncidentEffect(delegate
			{
				Func<CharacterObject> characterGetter2 = characterGetter;
				CharacterObject characterObject = ((characterGetter2 != null) ? characterGetter2() : null);
				if (characterObject == null)
				{
					return false;
				}
				List<CharacterObject> list = CharacterHelper.GetTroopTree(characterObject, -1f, float.MaxValue).Where<CharacterObject>(upgradePredicate).ToList<CharacterObject>();
				return MobileParty.MainParty.MemberRoster.GetElementNumber(characterObject) >= amount && list.Count != 0;
			}, delegate
			{
				CharacterObject characterObject2 = characterGetter();
				CharacterObject seededRandomElement = IncidentHelper.GetSeededRandomElement<CharacterObject>(CharacterHelper.GetTroopTree(characterObject2, -1f, float.MaxValue).Where<CharacterObject>(upgradePredicate).ToList<CharacterObject>(), incidentSeedGetter());
				TroopRoster memberRoster = MobileParty.MainParty.MemberRoster;
				memberRoster.AddToCounts(characterObject2, -amount, false, 0, 0, true, -1);
				memberRoster.AddToCounts(seededRandomElement, amount, false, 0, 0, true, -1);
				TextObject textObject = new TextObject("{=0yugQtfB}Upgraded {AMOUNT} of your {TROOP.NAME} to {UPGRADED_TROOP.NAME}", null);
				textObject.SetTextVariable("AMOUNT", amount);
				textObject.SetCharacterProperties("TROOP", characterObject2, false);
				textObject.SetCharacterProperties("UPGRADED_TROOP", seededRandomElement, false);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				CharacterObject characterObject3 = characterGetter();
				CharacterObject seededRandomElement2 = IncidentHelper.GetSeededRandomElement<CharacterObject>(CharacterHelper.GetTroopTree(characterObject3, -1f, float.MaxValue).Where<CharacterObject>(upgradePredicate).ToList<CharacterObject>(), incidentSeedGetter());
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=nm2XK1pt}Upgrade {AMOUNT} of your {TROOP.NAME} to {UPGRADED_TROOP.NAME}", null);
				}
				else
				{
					textObject2 = new TextObject("{=tRipzFIP}{CHANCE}% chance of upgrading {AMOUNT} of your {TROOP.NAME} to {UPGRADED_TROOP.NAME}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				textObject2.SetCharacterProperties("TROOP", characterObject3, false);
				textObject2.SetCharacterProperties("UPGRADED_TROOP", seededRandomElement2, false);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x00097C24 File Offset: 0x00095E24
		public static IncidentEffect UpgradeTroop(Func<CharacterObject> characterGetter, Func<CharacterObject> upgradedCharacterGetter, int amount, Func<long> incidentSeedGetter)
		{
			return new IncidentEffect(delegate
			{
				Func<CharacterObject> characterGetter2 = characterGetter;
				CharacterObject characterObject = ((characterGetter2 != null) ? characterGetter2() : null);
				if (characterObject == null)
				{
					return false;
				}
				Func<CharacterObject> upgradedCharacterGetter2 = upgradedCharacterGetter;
				return ((upgradedCharacterGetter2 != null) ? upgradedCharacterGetter2() : null) != null && MobileParty.MainParty.MemberRoster.GetElementNumber(characterObject) >= amount;
			}, delegate
			{
				CharacterObject characterObject2 = characterGetter();
				CharacterObject characterObject3 = upgradedCharacterGetter();
				TroopRoster memberRoster = MobileParty.MainParty.MemberRoster;
				memberRoster.AddToCounts(characterObject2, -amount, false, 0, 0, true, -1);
				memberRoster.AddToCounts(characterObject3, amount, false, 0, 0, true, -1);
				TextObject textObject = new TextObject("{=0yugQtfB}Upgraded {AMOUNT} of your {TROOP.NAME} to {UPGRADED_TROOP.NAME}", null);
				textObject.SetTextVariable("AMOUNT", amount);
				textObject.SetCharacterProperties("TROOP", characterObject2, false);
				textObject.SetCharacterProperties("UPGRADED_TROOP", characterObject3, false);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				CharacterObject characterObject4 = characterGetter();
				CharacterObject characterObject5 = upgradedCharacterGetter();
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=nm2XK1pt}Upgrade {AMOUNT} of your {TROOP.NAME} to {UPGRADED_TROOP.NAME}", null);
				}
				else
				{
					textObject2 = new TextObject("{=tRipzFIP}{CHANCE}% chance of upgrading {AMOUNT} of your {TROOP.NAME} to {UPGRADED_TROOP.NAME}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				textObject2.SetCharacterProperties("TROOP", characterObject4, false);
				textObject2.SetCharacterProperties("UPGRADED_TROOP", characterObject5, false);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x00097C78 File Offset: 0x00095E78
		public static IncidentEffect RemovePrisonersRandomlyWithPredicate(Func<TroopRosterElement, bool> predicate, int amount)
		{
			return new IncidentEffect(() => MobileParty.MainParty.PrisonRoster.GetTroopRoster().Where<TroopRosterElement>(predicate).Sum<TroopRosterElement>((TroopRosterElement x) => x.Number) >= amount, delegate
			{
				int i = amount;
				MBList<TroopRosterElement> troopRoster = MobileParty.MainParty.PrisonRoster.GetTroopRoster();
				while (i > 0)
				{
					List<TroopRosterElement> list = troopRoster.Where<TroopRosterElement>(predicate).ToList<TroopRosterElement>();
					TroopRosterElement troopRosterElement = list[MobileParty.MainParty.RandomInt(list.Count)];
					int num = Math.Min(MBRandom.RandomInt(1, troopRosterElement.Number), i);
					MobileParty.MainParty.PrisonRoster.AddToCounts(troopRosterElement.Character, -num, false, 0, 0, true, -1);
					i -= num;
				}
				TextObject textObject = new TextObject("{=tvshVXKT}Lost {AMOUNT} {?AMOUNT > 1}prisoners{?}prisoner{\\?}.", null);
				textObject.SetTextVariable("AMOUNT", amount);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=6XW3fKhU}Lose {AMOUNT} {?AMOUNT > 1}prisoners{?}prisoner{\\?}", null);
				}
				else
				{
					textObject2 = new TextObject("{=F3AEpszA}{CHANCE}% chance of losing {AMOUNT} {?AMOUNT > 1}prisoners{?}prisoner{\\?}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x00097CC4 File Offset: 0x00095EC4
		public static IncidentEffect ChangeItemsAmount(Func<List<ItemObject>> itemsGetter, int amount)
		{
			return new IncidentEffect(delegate
			{
				CampaignIncidentEffects.<>c__DisplayClass37_1 CS$<>8__locals2 = new CampaignIncidentEffects.<>c__DisplayClass37_1();
				CampaignIncidentEffects.<>c__DisplayClass37_1 CS$<>8__locals3 = CS$<>8__locals2;
				Func<List<ItemObject>> itemsGetter2 = itemsGetter;
				CS$<>8__locals3.items = ((itemsGetter2 != null) ? itemsGetter2() : null);
				if (CS$<>8__locals2.items == null || CS$<>8__locals2.items.Count < 0)
				{
					return false;
				}
				if (amount >= 0)
				{
					return true;
				}
				return MobileParty.MainParty.ItemRoster.Where<ItemRosterElement>((ItemRosterElement x) => CS$<>8__locals2.items.Contains(x.EquipmentElement.Item)).Sum<ItemRosterElement>((ItemRosterElement x) => x.Amount) >= amount;
			}, delegate
			{
				List<ItemObject> list = itemsGetter();
				ItemObject itemObject = list.First<ItemObject>();
				int i = Math.Abs(amount);
				while (i > 0)
				{
					ItemObject randomElement = list.GetRandomElement<ItemObject>();
					int num = MobileParty.MainParty.ItemRoster.FindIndexOfItem(randomElement);
					int elementNumber = MobileParty.MainParty.ItemRoster.GetElementNumber(num);
					int num2 = Math.Min(MBRandom.RandomInt(1, Math.Min(elementNumber, i)), i);
					MobileParty.MainParty.ItemRoster.AddToCounts(MobileParty.MainParty.ItemRoster[num].EquipmentElement, num2 * Math.Sign(amount));
					i -= num2;
					if (elementNumber - num2 == 0)
					{
						list.Remove(randomElement);
					}
				}
				TextObject textObject = ((list.Count == 1) ? list.First<ItemObject>().Name : itemObject.ItemCategory.GetName());
				TextObject textObject2 = new TextObject("{=shZVdlRQ}{?AMOUNT > 1}Received{?}Lost{\\?} {ABS(AMOUNT)} {?AMOUNT > 1}{PLURAL(ITEM)}{?}{ITEM}{\\?}.", null);
				textObject2.SetTextVariable("AMOUNT", amount);
				textObject2.SetTextVariable("ITEM", textObject);
				return new List<TextObject> { textObject2 };
			}, delegate(IncidentEffect effect)
			{
				List<ItemObject> list2 = itemsGetter();
				TextObject textObject3 = ((list2.Count == 1) ? list2.First<ItemObject>().Name : list2.First<ItemObject>().ItemCategory.GetName());
				TextObject textObject4;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject4 = new TextObject("{=OZAKqzln}{?AMOUNT > 1}Get{?}Lose{\\?} {ABS(AMOUNT)} {?AMOUNT > 1}{PLURAL(ITEM)}{?}{ITEM}{\\?}", null);
				}
				else
				{
					textObject4 = new TextObject("{=aVtM937J}{CHANCE}% chance of {?AMOUNT > 1}getting{?}losing{\\?} {ABS(AMOUNT)} {?AMOUNT > 1}{PLURAL(ITEM)}{?}{ITEM}{\\?}", null);
					textObject4.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject4.SetTextVariable("AMOUNT", amount);
				textObject4.SetTextVariable("ITEM", textObject3);
				return new IncidentHint(textObject4, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x00097D10 File Offset: 0x00095F10
		public static IncidentEffect ChangeItemAmount(Func<ItemObject> itemGetter, Func<int> amountGetter)
		{
			return new IncidentEffect(delegate
			{
				Func<ItemObject> itemGetter2 = itemGetter;
				return ((itemGetter2 != null) ? itemGetter2() : null) != null && amountGetter != null && (amountGetter() > 0 || MobileParty.MainParty.ItemRoster.GetItemNumber(itemGetter()) >= Math.Abs(amountGetter()));
			}, delegate
			{
				ItemObject itemObject = itemGetter();
				int num = amountGetter();
				MobileParty.MainParty.ItemRoster.AddToCounts(itemObject, num);
				TextObject textObject = new TextObject("{=0utzQGvE}{?AMOUNT >= 1}Received{?}Lost{\\?} {ABS(AMOUNT)} {?AMOUNT > 1}{PLURAL(ITEM)}{?}{ITEM}{\\?}.", null);
				textObject.SetTextVariable("AMOUNT", num);
				textObject.SetTextVariable("ITEM", itemObject.Name);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				ItemObject itemObject2 = itemGetter();
				int num2 = amountGetter();
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=GQWArgN4}{?AMOUNT >= 1}Get{?}Lose{\\?} {ABS(AMOUNT)} {?AMOUNT > 1}{PLURAL(ITEM)}{?}{ITEM}{\\?}", null);
				}
				else
				{
					textObject2 = new TextObject("{=ZCIsZTe2}{CHANCE}% chance of {?AMOUNT >= 1}getting{?}losing{\\?} {ABS(AMOUNT)} {?AMOUNT >= 1}{PLURAL(ITEM)}{?}{ITEM}{\\?}", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", num2);
				textObject2.SetTextVariable("ITEM", itemObject2.Name);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x00097D5C File Offset: 0x00095F5C
		public static IncidentEffect PartyExperienceChance(int amount)
		{
			return new IncidentEffect(null, delegate
			{
				MobilePartyHelper.PartyAddSharedXp(MobileParty.MainParty, (float)amount);
				TextObject textObject = new TextObject("{=LgzX3fDk}Party gained {AMOUNT} shared experience.", null);
				textObject.SetTextVariable("AMOUNT", amount);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=CCfVud1f}Party gains {AMOUNT} shared experience", null);
				}
				else
				{
					textObject2 = new TextObject("{=aFUVF8VO}{CHANCE}% chance of party gaining {AMOUNT} shared experience", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x00097D94 File Offset: 0x00095F94
		public static IncidentEffect DisorganizeParty()
		{
			return new IncidentEffect(null, delegate
			{
				MobileParty.MainParty.SetDisorganized(true);
				TextObject textObject = new TextObject("{=ylqcMuBF}Your party got disorganized.", null);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=02DbEPC1}Party becomes disorganized", null);
				}
				else
				{
					textObject2 = new TextObject("{=aXXF3aJE}{CHANCE}% chance of party becoming disorganized", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x00097DE8 File Offset: 0x00095FE8
		public static IncidentEffect HealTroopsRandomly(int amount)
		{
			return new IncidentEffect(null, delegate
			{
				TroopRoster memberRoster = MobileParty.MainParty.MemberRoster;
				int num = MBRandom.RandomInt(memberRoster.Count);
				int num2 = 0;
				while (num2 < memberRoster.Count && amount > 0)
				{
					int num3 = (num + num2) % memberRoster.Count;
					if (memberRoster.GetCharacterAtIndex(num3).IsRegular)
					{
						int num4 = MathF.Min(amount, memberRoster.GetElementWoundedNumber(num3));
						if (num4 > 0)
						{
							memberRoster.AddToCountsAtIndex(num3, 0, -num4, 0, true);
							amount -= num4;
						}
					}
					num2++;
				}
				TextObject textObject = new TextObject("{=pawoTBr8}Healed {AMOUNT} wounded troops.", null);
				textObject.SetTextVariable("AMOUNT", amount);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				TextObject textObject2;
				if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=EbBazlbZ}Heal {AMOUNT} wounded troops", null);
				}
				else
				{
					textObject2 = new TextObject("{=riVJZgSU}{CHANCE}% chance of healing {AMOUNT} wounded troops", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x00097E20 File Offset: 0x00096020
		public static IncidentEffect DemoteTroopsRandomlyWithPredicate(Func<TroopRosterElement, bool> predicate, Func<CharacterObject, bool> demotionPredicate, int amount, bool specifyUnitTypeOnHint = true)
		{
			Func<TroopRosterElement, bool> <>9__3;
			Func<TroopRosterElement, bool> <>9__5;
			Func<TroopRosterElement, bool> <>9__6;
			return new IncidentEffect(delegate
			{
				IEnumerable<TroopRosterElement> troopRoster = MobileParty.MainParty.MemberRoster.GetTroopRoster();
				Func<TroopRosterElement, bool> func;
				if ((func = <>9__3) == null)
				{
					func = (<>9__3 = delegate(TroopRosterElement x)
					{
						CharacterObject characterObject;
						return predicate != null && predicate(x) && x.Character != CharacterObject.PlayerCharacter && CampaignIncidentEffects.FindTroopToDemoteTo(x.Character, demotionPredicate, out characterObject);
					});
				}
				return troopRoster.Where<TroopRosterElement>(func).Sum<TroopRosterElement>((TroopRosterElement x) => x.Number) > amount;
			}, delegate
			{
				int i = amount;
				while (i > 0)
				{
					MBList<TroopRosterElement> troopRoster2 = MobileParty.MainParty.MemberRoster.GetTroopRoster();
					Func<TroopRosterElement, bool> func2;
					if ((func2 = <>9__5) == null)
					{
						func2 = (<>9__5 = delegate(TroopRosterElement x)
						{
							CharacterObject characterObject3;
							return predicate(x) && x.Character != CharacterObject.PlayerCharacter && CampaignIncidentEffects.FindTroopToDemoteTo(x.Character, demotionPredicate, out characterObject3);
						});
					}
					TroopRosterElement randomElementWithPredicate = troopRoster2.GetRandomElementWithPredicate<TroopRosterElement>(func2);
					CharacterObject characterObject2;
					CampaignIncidentEffects.FindTroopToDemoteTo(randomElementWithPredicate.Character, demotionPredicate, out characterObject2);
					if (randomElementWithPredicate.WoundedNumber > 0)
					{
						int num = Math.Min(MBRandom.RandomInt(1, Math.Min(randomElementWithPredicate.WoundedNumber, i)), i);
						MobileParty.MainParty.MemberRoster.AddToCounts(randomElementWithPredicate.Character, -num, false, num, 0, true, -1);
						MobileParty.MainParty.MemberRoster.AddToCounts(characterObject2, num, false, num, 0, true, -1);
						i -= num;
					}
					else
					{
						int num2 = Math.Min(MBRandom.RandomInt(1, Math.Min(randomElementWithPredicate.Number, i)), i);
						MobileParty.MainParty.MemberRoster.AddToCounts(randomElementWithPredicate.Character, -num2, false, 0, 0, true, -1);
						MobileParty.MainParty.MemberRoster.AddToCounts(characterObject2, num2, false, 0, 0, true, -1);
						i -= num2;
					}
				}
				TextObject textObject = new TextObject("{=211WkLlN}{AMOUNT} of your troops got demoted.", null);
				textObject.SetTextVariable("AMOUNT", amount);
				return new List<TextObject> { textObject };
			}, delegate(IncidentEffect effect)
			{
				IEnumerable<TroopRosterElement> troopRoster3 = MobileParty.MainParty.MemberRoster.GetTroopRoster();
				Func<TroopRosterElement, bool> func3;
				if ((func3 = <>9__6) == null)
				{
					func3 = (<>9__6 = (TroopRosterElement x) => predicate(x) && CharacterHelper.GetTroopTree(x.Character.Culture.BasicTroop, -1f, (float)(x.Character.Tier - 1)).FirstOrDefault<CharacterObject>(demotionPredicate) != null);
				}
				TroopRosterElement troopRosterElement = troopRoster3.FirstOrDefault<TroopRosterElement>(func3);
				TextObject textObject2;
				if (specifyUnitTypeOnHint)
				{
					if (effect.ChanceToOccur >= 1f)
					{
						textObject2 = new TextObject("{=64YgbSH8}{AMOUNT} {UNIT_TYPE} get demoted", null);
					}
					else
					{
						textObject2 = new TextObject("{=CC1tYZMa}{CHANCE}% chance of {AMOUNT} {UNIT_TYPE} getting demoted", null);
						textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
					}
					textObject2.SetTextVariable("UNIT_TYPE", troopRosterElement.Character.DefaultFormationClass.GetName());
				}
				else if (effect.ChanceToOccur >= 1f)
				{
					textObject2 = new TextObject("{=fMCKvvCa}{AMOUNT} random troops get demoted", null);
				}
				else
				{
					textObject2 = new TextObject("{=47MFRfCt}{CHANCE}% chance of {AMOUNT} random troops getting demoted", null);
					textObject2.SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f));
				}
				textObject2.SetTextVariable("AMOUNT", amount);
				return new IncidentHint(textObject2, IncidentHintType.Effect);
			});
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x00097E78 File Offset: 0x00096078
		private static bool FindTroopToDemoteTo(CharacterObject troop, Func<CharacterObject, bool> demotionPredicate, out CharacterObject troopToDemoteTo)
		{
			List<CharacterObject> list = CharacterHelper.GetTroopTree(troop.Culture.BasicTroop, -1f, (float)(troop.Tier - 1)).Where<CharacterObject>(demotionPredicate).ToList<CharacterObject>();
			if (list.Any<CharacterObject>())
			{
				IGrouping<int, CharacterObject> grouping = (from x in list
					group x by CampaignIncidentEffects.FindUpgradeDistanceBfs(x, troop) into x
					orderby x.Key
					select x).FirstOrDefault<IGrouping<int, CharacterObject>>((IGrouping<int, CharacterObject> x) => x.Key != -1);
				if (grouping != null)
				{
					CharacterObject characterObject = grouping.FirstOrDefault<CharacterObject>((CharacterObject x) => x.UpgradeTargets.Contains(troop));
					if (characterObject != null)
					{
						troopToDemoteTo = characterObject;
						return true;
					}
					troopToDemoteTo = grouping.ToList<CharacterObject>().GetRandomElement<CharacterObject>();
					return true;
				}
			}
			List<CharacterObject> list2 = CharacterHelper.GetTroopTree(troop.Culture.EliteBasicTroop, -1f, (float)(troop.Tier - 1)).Where<CharacterObject>(demotionPredicate).ToList<CharacterObject>();
			if (list2.Any<CharacterObject>())
			{
				IGrouping<int, CharacterObject> grouping2 = (from x in list2
					group x by CampaignIncidentEffects.FindUpgradeDistanceBfs(x, troop) into x
					orderby x.Key
					select x).FirstOrDefault<IGrouping<int, CharacterObject>>((IGrouping<int, CharacterObject> x) => x.Key != -1);
				if (grouping2 != null)
				{
					CharacterObject characterObject2 = grouping2.FirstOrDefault<CharacterObject>((CharacterObject x) => x.UpgradeTargets.Contains(troop));
					if (characterObject2 != null)
					{
						troopToDemoteTo = characterObject2;
						return true;
					}
					troopToDemoteTo = grouping2.ToList<CharacterObject>().GetRandomElement<CharacterObject>();
					return true;
				}
			}
			troopToDemoteTo = null;
			return false;
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x00098030 File Offset: 0x00096230
		private static int FindUpgradeDistanceBfs(CharacterObject start, CharacterObject target)
		{
			if (start == target)
			{
				return 0;
			}
			HashSet<CharacterObject> hashSet = new HashSet<CharacterObject>();
			Queue<ValueTuple<CharacterObject, int>> queue = new Queue<ValueTuple<CharacterObject, int>>();
			queue.Enqueue(new ValueTuple<CharacterObject, int>(start, 0));
			hashSet.Add(start);
			while (queue.Count > 0)
			{
				ValueTuple<CharacterObject, int> valueTuple = queue.Dequeue();
				CharacterObject item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				foreach (CharacterObject characterObject in item.UpgradeTargets)
				{
					if (!hashSet.Contains(characterObject))
					{
						if (characterObject == target)
						{
							return item2 + 1;
						}
						hashSet.Add(characterObject);
						queue.Enqueue(new ValueTuple<CharacterObject, int>(characterObject, item2 + 1));
					}
				}
			}
			return -1;
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x000980D4 File Offset: 0x000962D4
		public static IncidentEffect Group(params IncidentEffect[] effects)
		{
			return new IncidentEffect(() => effects.All<IncidentEffect>((IncidentEffect x) => x.Condition()), delegate
			{
				List<TextObject> list = new List<TextObject>();
				foreach (IncidentEffect incidentEffect in effects)
				{
					list.AddRange(incidentEffect.Consequence());
				}
				return list;
			}, (IncidentEffect effect) => new IncidentHint((effect.ChanceToOccur < 1f) ? new TextObject("{=haovqFEg}{CHANCE}% chance of:", null).SetTextVariable("CHANCE", MathF.Round(effect.ChanceToOccur * 100f)) : null, IncidentHintType.Group, effects.SelectQ<IncidentEffect, IncidentHint>((IncidentEffect x) => x.GetHint()).ToArrayQ<IncidentHint>()).WithChance(effect.ChanceToOccur));
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x00098118 File Offset: 0x00096318
		private static int[] GetDisplayedChancePercentages(float[] chances)
		{
			int[] array = new int[chances.Length];
			float[] array2 = new float[chances.Length];
			int num = 0;
			for (int i = 0; i < chances.Length; i++)
			{
				float num2 = chances[i] * 100f;
				array[i] = MathF.Floor(num2);
				array2[i] = num2 - (float)array[i];
				num += array[i];
			}
			for (int j = 100 - num; j > 0; j--)
			{
				int num3 = -1;
				float num4 = -1f;
				for (int k = 0; k < array2.Length; k++)
				{
					if (array2[k] > num4)
					{
						num4 = array2[k];
						num3 = k;
					}
				}
				if (num3 < 0)
				{
					break;
				}
				array[num3]++;
				array2[num3] = float.MinValue;
			}
			return array;
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x000981CC File Offset: 0x000963CC
		public static IncidentEffect Select(IncidentEffect effectOne, IncidentEffect effectTwo, float chanceOfFirstOne)
		{
			return new IncidentEffect(() => effectOne.Condition() && effectTwo.Condition(), delegate
			{
				List<TextObject> list = new List<TextObject>();
				if (MBRandom.RandomFloat < chanceOfFirstOne)
				{
					list.AddRange(effectOne.Consequence());
				}
				else
				{
					list.AddRange(effectTwo.Consequence());
				}
				return list;
			}, delegate(IncidentEffect effect)
			{
				int[] displayedChancePercentages = CampaignIncidentEffects.GetDisplayedChancePercentages(new float[]
				{
					chanceOfFirstOne,
					1f - chanceOfFirstOne
				});
				IncidentHint incidentHint = new IncidentHint(new TextObject("{=haovqFEg}{CHANCE}% chance of:", null).SetTextVariable("CHANCE", displayedChancePercentages[0]), IncidentHintType.SelectBranch, new IncidentHint[] { effectOne.GetHint() }).WithChance(chanceOfFirstOne);
				IncidentHint incidentHint2 = new IncidentHint(new TextObject("{=dQqR2DXD}or {CHANCE}% chance of:", null).SetTextVariable("CHANCE", displayedChancePercentages[1]), IncidentHintType.SelectBranch, new IncidentHint[] { effectTwo.GetHint() }).WithChance(1f - chanceOfFirstOne);
				return new IncidentHint(null, IncidentHintType.Select, new IncidentHint[] { incidentHint, incidentHint2 });
			});
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x00098220 File Offset: 0x00096420
		public static IncidentEffect Select([TupleElementNames(new string[] { "Effect", "Chance" })] params ValueTuple<IncidentEffect, float>[] effectAndChance)
		{
			return new IncidentEffect(delegate
			{
				ValueTuple<IncidentEffect, float>[] effectAndChance2 = effectAndChance;
				for (int i = 0; i < effectAndChance2.Length; i++)
				{
					if (!effectAndChance2[i].Item1.Condition())
					{
						return false;
					}
				}
				return true;
			}, delegate
			{
				List<TextObject> list = new List<TextObject>();
				float num = MBRandom.RandomFloat;
				for (int j = 0; j < effectAndChance.Length; j++)
				{
					num -= effectAndChance[j].Item2;
					if (num <= 0f)
					{
						list.AddRange(effectAndChance[j].Item1.Consequence());
						break;
					}
				}
				return list;
			}, delegate(IncidentEffect effect)
			{
				IncidentHint[] array = new IncidentHint[effectAndChance.Length];
				int[] displayedChancePercentages = CampaignIncidentEffects.GetDisplayedChancePercentages(effectAndChance.SelectQ<ValueTuple<IncidentEffect, float>, float>(([TupleElementNames(new string[] { "Effect", "Chance" })] ValueTuple<IncidentEffect, float> x) => x.Item2).ToArrayQ<float>());
				for (int k = 0; k < effectAndChance.Length; k++)
				{
					TextObject textObject = ((k == 0) ? new TextObject("{=haovqFEg}{CHANCE}% chance of:", null).SetTextVariable("CHANCE", displayedChancePercentages[k]) : new TextObject("{=dQqR2DXD}or {CHANCE}% chance of:", null).SetTextVariable("CHANCE", displayedChancePercentages[k]));
					array[k] = new IncidentHint(textObject, IncidentHintType.SelectBranch, new IncidentHint[] { effectAndChance[k].Item1.GetHint() }).WithChance(effectAndChance[k].Item2);
				}
				return new IncidentHint(null, IncidentHintType.Select, array);
			});
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x00098263 File Offset: 0x00096463
		public static IncidentEffect Custom(Func<bool> condition, Func<List<TextObject>> consequence, Func<IncidentEffect, IncidentHint> hint)
		{
			return new IncidentEffect(condition, consequence, hint);
		}
	}
}
