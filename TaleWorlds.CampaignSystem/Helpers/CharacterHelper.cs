using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace Helpers
{
	// Token: 0x02000017 RID: 23
	public static class CharacterHelper
	{
		// Token: 0x060000AE RID: 174 RVA: 0x00009BC4 File Offset: 0x00007DC4
		public static TextObject GetDeathNotification(Hero victimHero, Hero killer, KillCharacterAction.KillCharacterActionDetail detail)
		{
			TextObject textObject = TextObject.GetEmpty();
			if (detail == KillCharacterAction.KillCharacterActionDetail.DiedInLabor || detail == KillCharacterAction.KillCharacterActionDetail.Murdered || detail == KillCharacterAction.KillCharacterActionDetail.DiedInBattle || detail == KillCharacterAction.KillCharacterActionDetail.DiedOfOldAge)
			{
				textObject = GameTexts.FindText("str_on_hero_killed", detail.ToString());
			}
			else if (detail == KillCharacterAction.KillCharacterActionDetail.Executed && killer != null)
			{
				textObject = GameTexts.FindText("str_on_hero_killed", detail.ToString());
				StringHelpers.SetCharacterProperties("KILLER", killer.CharacterObject, textObject, false);
			}
			else if (detail == KillCharacterAction.KillCharacterActionDetail.Lost)
			{
				textObject = GameTexts.FindText("str_on_hero_killed", detail.ToString());
				StringHelpers.SetCharacterProperties("VICTIM", victimHero.CharacterObject, textObject, false);
			}
			else
			{
				textObject = GameTexts.FindText("str_on_hero_killed", "Default");
			}
			StringHelpers.SetCharacterProperties("HERO", victimHero.CharacterObject, textObject, false);
			return textObject;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00009C8C File Offset: 0x00007E8C
		public static DynamicBodyProperties GetDynamicBodyPropertiesBetweenMinMaxRange(CharacterObject character)
		{
			BodyProperties bodyPropertyMin = character.BodyPropertyRange.BodyPropertyMin;
			BodyProperties bodyPropertyMax = character.BodyPropertyRange.BodyPropertyMax;
			float num = ((bodyPropertyMin.Age < bodyPropertyMax.Age) ? bodyPropertyMin.Age : bodyPropertyMax.Age);
			float num2 = ((bodyPropertyMin.Age > bodyPropertyMax.Age) ? bodyPropertyMin.Age : bodyPropertyMax.Age);
			float num3 = ((bodyPropertyMin.Weight < bodyPropertyMax.Weight) ? bodyPropertyMin.Weight : bodyPropertyMax.Weight);
			float num4 = ((bodyPropertyMin.Weight > bodyPropertyMax.Weight) ? bodyPropertyMin.Weight : bodyPropertyMax.Weight);
			float num5 = ((bodyPropertyMin.Build < bodyPropertyMax.Build) ? bodyPropertyMin.Build : bodyPropertyMax.Build);
			float num6 = ((bodyPropertyMin.Build > bodyPropertyMax.Build) ? bodyPropertyMin.Build : bodyPropertyMax.Build);
			float num7 = MBRandom.RandomFloatRanged(num, num2);
			float num8 = MBRandom.RandomFloatRanged(num3, num4);
			float num9 = MBRandom.RandomFloatRanged(num5, num6);
			return new DynamicBodyProperties(num7, num8, num9);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00009DA0 File Offset: 0x00007FA0
		public static TextObject GetReputationDescription(CharacterObject character)
		{
			TextObject textObject = new TextObject("{=!}{REPUTATION_SUMMARY}", null);
			TextObject textObject2 = Campaign.Current.ConversationManager.FindMatchingTextOrNull("reputation", character);
			StringHelpers.SetCharacterProperties("NOTABLE", character, textObject2, false);
			textObject.SetTextVariable("REPUTATION_SUMMARY", textObject2);
			return textObject;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00009DEC File Offset: 0x00007FEC
		[return: TupleElementNames(new string[] { "color1", "color2" })]
		public static ValueTuple<uint, uint> GetDeterministicColorsForCharacter(CharacterObject character, PartyBase partyBelongsTo)
		{
			if (!character.IsHero)
			{
				uint? num;
				if (partyBelongsTo == null)
				{
					num = null;
				}
				else
				{
					IFaction mapFaction = partyBelongsTo.MapFaction;
					num = ((mapFaction != null) ? new uint?(mapFaction.Color) : null);
				}
				uint num2 = num ?? 4291609515U;
				uint? num3;
				if (partyBelongsTo == null)
				{
					num3 = null;
				}
				else
				{
					IFaction mapFaction2 = partyBelongsTo.MapFaction;
					num3 = ((mapFaction2 != null) ? new uint?(mapFaction2.Color2) : null);
				}
				uint num4 = num3 ?? 4291609515U;
				return new ValueTuple<uint, uint>(num2, num4);
			}
			if (character.Occupation == Occupation.Lord)
			{
				IFaction mapFaction3 = character.HeroObject.MapFaction;
				uint num5 = ((mapFaction3 != null) ? mapFaction3.Color : 4291609515U);
				IFaction mapFaction4 = character.HeroObject.MapFaction;
				return new ValueTuple<uint, uint>(num5, (mapFaction4 != null) ? mapFaction4.Color2 : 4291609515U);
			}
			if (character.HeroObject.MapFaction == null)
			{
				return new ValueTuple<uint, uint>(4291609515U, 4291609515U);
			}
			string stringId = character.HeroObject.MapFaction.Culture.StringId;
			uint[] array;
			if (!(stringId == "empire"))
			{
				if (!(stringId == "sturgia"))
				{
					if (!(stringId == "aserai"))
					{
						if (!(stringId == "vlandia"))
						{
							if (!(stringId == "battania"))
							{
								if (!(stringId == "khuzait"))
								{
									array = CampaignData.EmpireHeroClothColors;
								}
								else
								{
									array = CampaignData.KhuzaitHeroClothColors;
								}
							}
							else
							{
								array = CampaignData.BattaniaHeroClothColors;
							}
						}
						else
						{
							array = CampaignData.VlandiaHeroClothColors;
						}
					}
					else
					{
						array = CampaignData.AseraiHeroClothColors;
					}
				}
				else
				{
					array = CampaignData.SturgiaHeroClothColors;
				}
			}
			else
			{
				array = CampaignData.EmpireHeroClothColors;
			}
			return new ValueTuple<uint, uint>(character.HeroObject.MapFaction.Color, CharacterHelper.GetDeterministicColorFromListForHero(character.HeroObject, array));
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00009FB9 File Offset: 0x000081B9
		private static uint GetDeterministicColorFromListForHero(Hero hero, uint[] colors)
		{
			return colors.ElementAt<uint>(hero.RandomIntWithSeed(39U) % colors.Length);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00009FCD File Offset: 0x000081CD
		public static IFaceGeneratorCustomFilter GetFaceGeneratorFilter()
		{
			IFacegenCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IFacegenCampaignBehavior>();
			if (campaignBehavior == null)
			{
				return null;
			}
			return campaignBehavior.GetFaceGenFilter();
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00009FE4 File Offset: 0x000081E4
		public static string GetNonconversationPose(CharacterObject character)
		{
			if (character.HeroObject.IsGangLeader)
			{
				return "aggressive";
			}
			if (!character.HeroObject.IsNoncombatant && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) <= 0 && character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) < 0)
			{
				return "aggressive2";
			}
			if (!character.HeroObject.IsNoncombatant && character.HeroObject.IsLord && character.GetPersona() == DefaultTraits.PersonaCurt && character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0)
			{
				return "warrior2";
			}
			if (character.HeroObject.Clan != null && character.HeroObject.Clan.IsNoble && character.GetPersona() == DefaultTraits.PersonaEarnest && character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) >= 0 && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) >= 0)
			{
				return "hip2";
			}
			if (character.IsFemale && character.GetPersona() == DefaultTraits.PersonaSoftspoken)
			{
				return "demure";
			}
			if (character.IsFemale && character.GetPersona() == DefaultTraits.PersonaIronic)
			{
				return "confident3";
			}
			if (character.GetPersona() == DefaultTraits.PersonaCurt)
			{
				return "closed2";
			}
			if (character.GetPersona() == DefaultTraits.PersonaSoftspoken)
			{
				return "demure2";
			}
			if (character.GetPersona() == DefaultTraits.PersonaIronic)
			{
				return "confident";
			}
			if (character.GetPersona() == DefaultTraits.PersonaEarnest)
			{
				return "normal2";
			}
			return "normal";
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000A15C File Offset: 0x0000835C
		public static string GetNonconversationFacialIdle(CharacterObject character)
		{
			string text = "convo_normal";
			string text2 = "convo_bemused";
			string text3 = "convo_mocking_teasing";
			string text4 = "convo_mocking_revenge";
			string text5 = "convo_delighted";
			string text6 = "convo_approving";
			string text7 = "convo_thinking";
			string text8 = "convo_focused_happy";
			string text9 = "convo_calm_friendly";
			string text10 = "convo_annoyed";
			string text11 = "convo_undecided_closed";
			string text12 = "convo_bored";
			string text13 = "convo_grave";
			string text14 = "convo_predatory";
			string text15 = "convo_confused_annoyed";
			if (character.HeroObject.IsGangLeader)
			{
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) <= 0 && character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) < 0)
				{
					return text14;
				}
				return text15;
			}
			else if (character.GetPersona() == DefaultTraits.PersonaCurt)
			{
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) < 0)
				{
					return text12;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0)
				{
					return text11;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0)
				{
					return text10;
				}
				return text13;
			}
			else if (character.GetPersona() == DefaultTraits.PersonaEarnest)
			{
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) > 0)
				{
					return text8;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) < 0)
				{
					return text12;
				}
				return text5;
			}
			else if (character.IsFemale && character.GetPersona() == DefaultTraits.PersonaSoftspoken)
			{
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) > 0)
				{
					return text9;
				}
				if (!character.HeroObject.IsNoncombatant)
				{
					return text7;
				}
				return text6;
			}
			else
			{
				if (character.GetPersona() != DefaultTraits.PersonaIronic)
				{
					return text;
				}
				if (!character.HeroObject.IsNoncombatant && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0)
				{
					return text4;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) < 0)
				{
					return text3;
				}
				return text2;
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000A31C File Offset: 0x0000851C
		public static string GetStandingBodyIdle(CharacterObject character, PartyBase party)
		{
			HeroHelper.WillLordAttack();
			string text = "normal";
			TraitObject persona = character.GetPersona();
			bool flag = Settlement.CurrentSettlement != null;
			if (character.IsHero)
			{
				if (character.HeroObject.IsWounded)
				{
					return (MBRandom.RandomFloat <= 0.7f) ? "weary" : "weary2";
				}
				bool flag2 = !character.HeroObject.IsHumanPlayerCharacter;
				int superiorityState = CharacterHelper.GetSuperiorityState(character);
				if (flag2)
				{
					int relation = Hero.MainHero.GetRelation(character.HeroObject);
					bool flag3 = CharacterHelper.MorePowerThanPlayer(character);
					if (character.IsFemale && character.HeroObject.IsNoncombatant)
					{
						if (relation < 0)
						{
							text = "closed";
						}
						else if (persona == DefaultTraits.PersonaIronic)
						{
							text = ((MBRandom.RandomFloat <= 0.5f) ? "confident" : "confident2");
						}
						else if (persona == DefaultTraits.PersonaCurt)
						{
							text = ((MBRandom.RandomFloat <= 0.5f) ? "closed" : "confident");
						}
						else if (persona == DefaultTraits.PersonaEarnest || persona == DefaultTraits.PersonaSoftspoken)
						{
							text = ((MBRandom.RandomFloat <= 0.7f) ? "demure" : "confident");
						}
					}
					else if (relation <= -20)
					{
						if (superiorityState >= 0)
						{
							if (persona == DefaultTraits.PersonaSoftspoken)
							{
								text = (character.IsFemale ? "closed" : "warrior2");
							}
							else if (persona == DefaultTraits.PersonaIronic)
							{
								text = (character.IsFemale ? "confident2" : "aggressive");
							}
							else
							{
								text = (character.IsFemale ? "confident2" : "warrior");
							}
						}
						else if (superiorityState == -1)
						{
							if (persona == DefaultTraits.PersonaSoftspoken)
							{
								if (flag3)
								{
									text = "closed";
								}
								else
								{
									text = (character.IsFemale ? "closed" : "normal");
								}
							}
							else if (persona == DefaultTraits.PersonaIronic)
							{
								if (flag3)
								{
									text = ((MBRandom.RandomFloat <= 0.5f) ? "closed" : "warrior");
								}
								else
								{
									text = "closed";
								}
							}
							else
							{
								text = (character.IsFemale ? "closed" : "warrior2");
							}
						}
					}
					else if (superiorityState >= 0)
					{
						if (persona == DefaultTraits.PersonaIronic)
						{
							if (flag)
							{
								if (flag3)
								{
									text = ((MBRandom.RandomFloat <= 0.7f) ? "confident2" : "normal");
								}
								else
								{
									text = ((MBRandom.RandomFloat <= 0.5f) ? "hip" : "normal");
								}
							}
							else
							{
								text = "confident2";
							}
						}
						else if (persona == DefaultTraits.PersonaSoftspoken)
						{
							if (flag)
							{
								if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) + character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0)
								{
									text = ((MBRandom.RandomFloat <= 0.5f) ? "normal2" : "demure2");
								}
								else if (flag3)
								{
									text = ((MBRandom.RandomFloat <= 0.5f) ? "normal" : "closed");
								}
								else
								{
									text = ((MBRandom.RandomFloat <= 0.5f) ? "normal" : "demure");
								}
							}
							else
							{
								text = "normal";
							}
						}
						else if (persona == DefaultTraits.PersonaCurt)
						{
							if (flag)
							{
								if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) + character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0)
								{
									text = "demure2";
								}
								else if (flag3)
								{
									text = ((MBRandom.RandomFloat <= 0.6f) ? "normal" : "closed2");
								}
								else
								{
									text = ((MBRandom.RandomFloat <= 0.4f) ? "warrior2" : "closed");
								}
							}
							else
							{
								text = "normal";
							}
						}
						else if (persona == DefaultTraits.PersonaEarnest)
						{
							if (flag)
							{
								if (flag3)
								{
									text = ((MBRandom.RandomFloat <= 0.6f) ? "normal" : "confident");
								}
								else
								{
									text = ((MBRandom.RandomFloat <= 0.2f) ? "normal" : "confident");
								}
							}
							else
							{
								text = "normal";
							}
						}
					}
				}
			}
			if (party != null)
			{
				MobileParty mobileParty = party.MobileParty;
				if (mobileParty != null && mobileParty.IsCurrentlyAtSea && party != PartyBase.MainParty)
				{
					return "naval";
				}
			}
			if (character.Occupation == Occupation.Bandit || character.Occupation == Occupation.Gangster)
			{
				text = ((MBRandom.RandomFloat <= 0.7f) ? "aggressive" : "hip");
			}
			if (character.Occupation == Occupation.Guard || character.Occupation == Occupation.PrisonGuard || character.Occupation == Occupation.Soldier)
			{
				text = "normal";
			}
			return text;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000A784 File Offset: 0x00008984
		public static string GetDefaultFaceIdle(CharacterObject character)
		{
			string text = "convo_normal";
			string text2 = "convo_bemused";
			string text3 = "convo_mocking_aristocratic";
			string text4 = "convo_mocking_teasing";
			string text5 = "convo_mocking_revenge";
			string text6 = "convo_contemptuous";
			string text7 = "convo_delighted";
			string text8 = "convo_approving";
			string text9 = "convo_relaxed_happy";
			string text10 = "convo_nonchalant";
			string text11 = "convo_thinking";
			string text12 = "convo_undecided_closed";
			string text13 = "convo_bored";
			string text14 = "convo_bored2";
			string text15 = "convo_grave";
			string text16 = "convo_stern";
			string text17 = "convo_very_stern";
			string text18 = "convo_beaten";
			string text19 = "convo_predatory";
			string text20 = "convo_confused_annoyed";
			bool flag = false;
			bool flag2 = false;
			if (character.IsHero)
			{
				flag = character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) + character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) > 0;
				flag2 = character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) + character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) < 0;
			}
			bool flag3 = Hero.MainHero.Clan.Renown < 0f;
			bool flag4 = false;
			if (PlayerEncounter.Current != null && PlayerEncounter.Current.PlayerSide == BattleSideEnum.Defender && (PlayerEncounter.EncounteredMobileParty == null || PlayerEncounter.EncounteredMobileParty.Ai.DoNotAttackMainPartyUntil.IsPast) && PlayerEncounter.EncounteredParty.Owner != null && FactionManager.IsAtWarAgainstFaction(PlayerEncounter.EncounteredParty.MapFaction, Hero.MainHero.MapFaction))
			{
				flag4 = true;
			}
			if (Campaign.Current.CurrentConversationContext == ConversationContext.CapturedLord && character.IsHero && character.HeroObject.MapFaction == PlayerEncounter.EncounteredParty.MapFaction)
			{
				return text16;
			}
			if (character.HeroObject != null)
			{
				int relation = character.HeroObject.GetRelation(Hero.MainHero);
				if (character.HeroObject != null && character.GetPersona() == DefaultTraits.PersonaIronic)
				{
					if (relation > 4)
					{
						return text4;
					}
					if (relation < -10)
					{
						return text5;
					}
					if (character.Occupation == Occupation.GangLeader && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0)
					{
						return text10;
					}
					if (character.Occupation == Occupation.GangLeader && flag3)
					{
						return text10;
					}
					Clan clan = character.HeroObject.Clan;
					if (clan == null || !clan.IsNoble)
					{
						return text3;
					}
					if (character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) + character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0)
					{
						return text13;
					}
					return text2;
				}
				else if (character.HeroObject != null && character.GetPersona() == DefaultTraits.PersonaCurt)
				{
					if (relation > 4)
					{
						return text7;
					}
					if (relation < -20)
					{
						return text4;
					}
					if (character.Occupation == Occupation.GangLeader && flag3)
					{
						return text19;
					}
					if (flag2)
					{
						return text15;
					}
					return text14;
				}
				else if (character.HeroObject != null && character.GetPersona() == DefaultTraits.PersonaSoftspoken)
				{
					if (relation > 4)
					{
						return text7;
					}
					if (relation < -20)
					{
						return text20;
					}
					Clan clan2 = character.HeroObject.Clan;
					if ((clan2 == null || !clan2.IsNoble) && flag3 && !character.IsFemale && flag2)
					{
						return text6;
					}
					Clan clan3 = character.HeroObject.Clan;
					if (clan3 != null && clan3.IsNoble && flag3 && !character.IsFemale && flag2)
					{
						return text12;
					}
					if (flag)
					{
						return text8;
					}
					return text11;
				}
				else if (character.HeroObject != null && character.GetPersona() == DefaultTraits.PersonaEarnest)
				{
					if (relation > 4)
					{
						return text7;
					}
					if (relation < -40)
					{
						return text17;
					}
					if (relation < -20)
					{
						return text16;
					}
					Clan clan4 = character.HeroObject.Clan;
					if (clan4 != null && clan4.IsNoble && flag2)
					{
						return text10;
					}
					if (flag)
					{
						return text8;
					}
					return text;
				}
			}
			else if (character.Occupation == Occupation.Villager || character.Occupation == Occupation.Townsfolk)
			{
				int deterministicHashCode = character.StringId.GetDeterministicHashCode();
				if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.Town != null && Settlement.CurrentSettlement.Town.Prosperity < (float)(200 * (Settlement.CurrentSettlement.IsTown ? 5 : 1)) && deterministicHashCode % 2 == 0)
				{
					return text18;
				}
				if (deterministicHashCode % 2 == 1)
				{
					return text9;
				}
			}
			else if (flag4 && character.Occupation == Occupation.Bandit)
			{
				return text16;
			}
			return text;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000AB94 File Offset: 0x00008D94
		private static int GetSuperiorityState(CharacterObject character)
		{
			if (Hero.MainHero.MapFaction != null && Hero.MainHero.MapFaction.Leader == Hero.MainHero && character.HeroObject.MapFaction == Hero.MainHero.MapFaction)
			{
				return -1;
			}
			if (character.IsHero && character.HeroObject.MapFaction != null && character.HeroObject.MapFaction.IsKingdomFaction)
			{
				Clan clan = character.HeroObject.Clan;
				if (clan != null && clan.IsNoble)
				{
					return 1;
				}
			}
			if (character.Occupation == Occupation.Villager || character.Occupation == Occupation.Townsfolk || character.Occupation == Occupation.Bandit || character.Occupation == Occupation.Gangster || character.Occupation == Occupation.Wanderer)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000AC54 File Offset: 0x00008E54
		private static bool MorePowerThanPlayer(CharacterObject otherCharacter)
		{
			float num;
			if (otherCharacter.HeroObject.PartyBelongedTo != null)
			{
				num = otherCharacter.HeroObject.PartyBelongedTo.Party.CalculateCurrentStrength();
			}
			else
			{
				num = otherCharacter.HeroObject.Power;
			}
			float num2 = MobileParty.MainParty.Party.CalculateCurrentStrength();
			return num > num2;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000ACAC File Offset: 0x00008EAC
		public static CharacterObject FindUpgradeRootOf(CharacterObject character)
		{
			foreach (CharacterObject characterObject in CharacterObject.All)
			{
				if (characterObject.IsBasicTroop && CharacterHelper.UpgradeTreeContains(characterObject, characterObject, character))
				{
					return characterObject;
				}
			}
			return character;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000AD10 File Offset: 0x00008F10
		private static bool UpgradeTreeContains(CharacterObject rootTroop, CharacterObject baseTroop, CharacterObject character)
		{
			if (baseTroop == character)
			{
				return true;
			}
			for (int i = 0; i < baseTroop.UpgradeTargets.Length; i++)
			{
				if (baseTroop.UpgradeTargets[i] == rootTroop)
				{
					return false;
				}
				if (CharacterHelper.UpgradeTreeContains(rootTroop, baseTroop.UpgradeTargets[i], character))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000AD58 File Offset: 0x00008F58
		public static ItemObject GetDefaultWeapon(CharacterObject affectorCharacter)
		{
			for (int i = 0; i <= 4; i++)
			{
				EquipmentElement equipmentFromSlot = affectorCharacter.Equipment.GetEquipmentFromSlot((EquipmentIndex)i);
				ItemObject item = equipmentFromSlot.Item;
				if (((item != null) ? item.PrimaryWeapon : null) != null && equipmentFromSlot.Item.PrimaryWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.WeaponMask))
				{
					return equipmentFromSlot.Item;
				}
			}
			return null;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000ADB8 File Offset: 0x00008FB8
		public static bool CanUseItem(BasicCharacterObject currentCharacter, EquipmentElement itemRosterElement)
		{
			TextObject textObject;
			return CharacterHelper.CanUseItem(currentCharacter, itemRosterElement, out textObject);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000ADD0 File Offset: 0x00008FD0
		public static bool CanUseItem(BasicCharacterObject currentCharacter, EquipmentElement itemRosterElement, out TextObject reason)
		{
			bool flag = true;
			reason = null;
			ItemObject item = itemRosterElement.Item;
			SkillObject relevantSkill = item.RelevantSkill;
			if (relevantSkill != null && currentCharacter.GetSkillValue(relevantSkill) < item.Difficulty)
			{
				reason = new TextObject("{=rgqA29b8}You don't have enough {SKILL_NAME} skill to equip this item", null);
				reason.SetTextVariable("SKILL_NAME", itemRosterElement.Item.RelevantSkill.Name);
				flag = false;
			}
			int num = (((!currentCharacter.IsFemale || !item.ItemFlags.HasAnyFlag(ItemFlags.NotUsableByFemale)) && (currentCharacter.IsFemale || !item.ItemFlags.HasAnyFlag(ItemFlags.NotUsableByMale))) ? 1 : 0);
			bool flag2 = item.StringId.Equals("dragon_banner_center") || item.StringId.Equals("dragon_banner_dragonhead") || item.StringId.Equals("dragon_banner_handle");
			if (num == 0 || flag2 || (itemRosterElement.Item.HasHorseComponent && !itemRosterElement.Item.HorseComponent.IsRideable))
			{
				reason = new TextObject("{=ITKb4cKv}{ITEM_NAME} is not equippable.", null);
				reason.SetTextVariable("ITEM_NAME", itemRosterElement.GetModifiedItemName());
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000AEF0 File Offset: 0x000090F0
		public static int GetPartyMemberFaceSeed(PartyBase party, BasicCharacterObject character, int rank)
		{
			int num = party.Index * 171 + character.StringId.GetDeterministicHashCode() * 6791 + rank * 197;
			return ((num >= 0) ? num : (-num)) % 2000;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000AF33 File Offset: 0x00009133
		public static int GetDefaultFaceSeed(BasicCharacterObject character, int rank)
		{
			return character.GetDefaultFaceSeed(rank);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000AF3C File Offset: 0x0000913C
		public static bool SearchForFormationInTroopTree(CharacterObject baseTroop, FormationClass formation)
		{
			if (baseTroop.UpgradeTargets.Length == 0 && baseTroop.DefaultFormationClass == formation)
			{
				return true;
			}
			foreach (CharacterObject characterObject in baseTroop.UpgradeTargets)
			{
				if (characterObject.Level > baseTroop.Level && CharacterHelper.SearchForFormationInTroopTree(characterObject, formation))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000AF90 File Offset: 0x00009190
		public static IEnumerable<CharacterObject> GetTroopTree(CharacterObject baseTroop, float minTier = -1f, float maxTier = 3.4028235E+38f)
		{
			MBQueue<CharacterObject> queue = new MBQueue<CharacterObject>();
			queue.Enqueue(baseTroop);
			while (queue.Count > 0)
			{
				CharacterObject character = queue.Dequeue();
				if ((float)character.Tier >= minTier && (float)character.Tier <= maxTier)
				{
					yield return character;
				}
				foreach (CharacterObject characterObject in character.UpgradeTargets)
				{
					queue.Enqueue(characterObject);
				}
				character = null;
			}
			yield break;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000AFB0 File Offset: 0x000091B0
		public static void DeleteQuestCharacter(CharacterObject character, Settlement questSettlement)
		{
			if (questSettlement != null)
			{
				IList<LocationCharacter> listOfCharacters = questSettlement.LocationComplex.GetListOfCharacters();
				if (listOfCharacters.Any<LocationCharacter>((LocationCharacter x) => x.Character == character))
				{
					LocationCharacter locationCharacter = listOfCharacters.First<LocationCharacter>((LocationCharacter x) => x.Character == character);
					questSettlement.LocationComplex.RemoveCharacterIfExists(locationCharacter);
				}
			}
			Game.Current.ObjectManager.UnregisterObject(character);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000B024 File Offset: 0x00009224
		public static CharacterObject GetRandomCompanionTemplateWithPredicate(Func<CharacterObject, bool> predicate = null)
		{
			if (predicate == null)
			{
				return MBObjectManager.Instance.GetObjectTypeList<CharacterObject>().GetRandomElementWithPredicate<CharacterObject>((CharacterObject x) => x.IsTemplate && x.Occupation == Occupation.Wanderer);
			}
			return MBObjectManager.Instance.GetObjectTypeList<CharacterObject>().GetRandomElementWithPredicate<CharacterObject>((CharacterObject x) => x.IsTemplate && x.Occupation == Occupation.Wanderer && predicate(x));
		}
	}
}
