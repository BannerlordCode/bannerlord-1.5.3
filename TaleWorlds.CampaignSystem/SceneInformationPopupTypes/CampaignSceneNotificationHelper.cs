using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B8 RID: 184
	public static class CampaignSceneNotificationHelper
	{
		// Token: 0x06001421 RID: 5153 RVA: 0x0005E8AC File Offset: 0x0005CAAC
		public static SceneNotificationData.SceneNotificationCharacter GetBodyguardOfCulture(CultureObject culture)
		{
			string stringId = culture.StringId;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(stringId);
			string text;
			if (num <= 2848701557U)
			{
				if (num != 744444005U)
				{
					if (num != 1759932477U)
					{
						if (num == 2848701557U)
						{
							if (stringId == "khuzait")
							{
								text = "khuzait_khans_guard";
								goto IL_0118;
							}
						}
					}
					else if (stringId == "battania")
					{
						text = "battanian_fian_champion";
						goto IL_0118;
					}
				}
				else if (stringId == "empire")
				{
					text = "imperial_legionary";
					goto IL_0118;
				}
			}
			else if (num <= 3015521580U)
			{
				if (num != 2894801972U)
				{
					if (num == 3015521580U)
					{
						if (stringId == "aserai")
						{
							text = "mamluke_palace_guard";
							goto IL_0118;
						}
					}
				}
				else if (stringId == "nord")
				{
					text = "nord_huscarl";
					goto IL_0118;
				}
			}
			else if (num != 3311783860U)
			{
				if (num == 4214512470U)
				{
					if (stringId == "vlandia")
					{
						text = "vlandian_banner_knight";
						goto IL_0118;
					}
				}
			}
			else if (stringId == "sturgia")
			{
				text = "druzhinnik_champion";
				goto IL_0118;
			}
			text = "fighter_sturgia";
			IL_0118:
			return new SceneNotificationData.SceneNotificationCharacter(MBObjectManager.Instance.GetObject<CharacterObject>(text), null, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false);
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0005E9F0 File Offset: 0x0005CBF0
		public static void RemoveWeaponsFromEquipment(ref Equipment equipment, bool removeHelmet = false, bool removeShoulder = false)
		{
			for (int i = 0; i < 5; i++)
			{
				equipment[i] = EquipmentElement.Invalid;
			}
			if (removeHelmet)
			{
				equipment[5] = EquipmentElement.Invalid;
			}
			if (removeShoulder)
			{
				equipment[9] = EquipmentElement.Invalid;
			}
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0005EA38 File Offset: 0x0005CC38
		public static string GetChildStageEquipmentIDFromCulture(CultureObject childCulture)
		{
			string stringId = childCulture.StringId;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(stringId);
			if (num <= 2848701557U)
			{
				if (num != 744444005U)
				{
					if (num != 1759932477U)
					{
						if (num == 2848701557U)
						{
							if (stringId == "khuzait")
							{
								return "comingofage_kid_khu_cutscene_template";
							}
						}
					}
					else if (stringId == "battania")
					{
						return "comingofage_kid_bat_cutscene_template";
					}
				}
				else if (stringId == "empire")
				{
					return "comingofage_kid_emp_cutscene_template";
				}
			}
			else if (num <= 3015521580U)
			{
				if (num != 2894801972U)
				{
					if (num == 3015521580U)
					{
						if (stringId == "aserai")
						{
							return "comingofage_kid_ase_cutscene_template";
						}
					}
				}
				else if (stringId == "nord")
				{
					return "comingofage_kid_nord_cutscene_template";
				}
			}
			else if (num != 3311783860U)
			{
				if (num == 4214512470U)
				{
					if (stringId == "vlandia")
					{
						return "comingofage_kid_vla_cutscene_template";
					}
				}
			}
			else if (stringId == "sturgia")
			{
				return "comingofage_kid_stu_cutscene_template";
			}
			return "comingofage_kid_emp_cutscene_template";
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0005EB48 File Offset: 0x0005CD48
		public static CharacterObject GetRandomTroopForCulture(CultureObject culture)
		{
			string text = "imperial_recruit";
			if (culture != null)
			{
				List<CharacterObject> list = new List<CharacterObject>();
				if (culture.BasicTroop != null)
				{
					list.Add(culture.BasicTroop);
				}
				if (culture.EliteBasicTroop != null)
				{
					list.Add(culture.EliteBasicTroop);
				}
				if (culture.MeleeMilitiaTroop != null)
				{
					list.Add(culture.MeleeMilitiaTroop);
				}
				if (culture.MeleeEliteMilitiaTroop != null)
				{
					list.Add(culture.MeleeEliteMilitiaTroop);
				}
				if (culture.RangedMilitiaTroop != null)
				{
					list.Add(culture.RangedMilitiaTroop);
				}
				if (culture.RangedEliteMilitiaTroop != null)
				{
					list.Add(culture.RangedEliteMilitiaTroop);
				}
				if (list.Count > 0)
				{
					return list[MBRandom.RandomInt(list.Count)];
				}
			}
			return Game.Current.ObjectManager.GetObject<CharacterObject>(text);
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0005EC0A File Offset: 0x0005CE0A
		public static IEnumerable<Hero> GetMilitaryAudienceForHero(Hero hero, bool includeClanLeader = true, bool onlyClanMembers = false)
		{
			if (hero.Clan != null)
			{
				if (includeClanLeader)
				{
					Hero leader = hero.Clan.Leader;
					if (leader != null && leader.IsAlive && hero != hero.Clan.Leader)
					{
						yield return hero.Clan.Leader;
					}
				}
				IOrderedEnumerable<Hero> orderedEnumerable = hero.Clan.Heroes.OrderBy<Hero, int>((Hero h) => h.Level);
				foreach (Hero hero2 in orderedEnumerable)
				{
					if (hero2 != hero.Clan.Leader && hero2.IsAlive && !hero2.IsChild && hero2 != hero)
					{
						yield return hero2;
					}
				}
				IEnumerator<Hero> enumerator = null;
			}
			if (!onlyClanMembers)
			{
				IOrderedEnumerable<Hero> orderedEnumerable2 = Hero.AllAliveHeroes.OrderBy<Hero, int>((Hero h) => CharacterRelationManager.GetHeroRelation(hero, h));
				foreach (Hero hero3 in orderedEnumerable2)
				{
					bool flag = hero3 != null && hero3.Clan != hero.Clan;
					if (hero3.IsFriend(hero3) && hero3.IsLord && !hero3.IsChild && hero3 != hero && !flag)
					{
						yield return hero3;
					}
				}
				IEnumerator<Hero> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0005EC28 File Offset: 0x0005CE28
		public static IEnumerable<Hero> GetMilitaryAudienceForKingdom(Kingdom kingdom, bool includeKingdomLeader = true)
		{
			if (includeKingdomLeader)
			{
				Hero leader = kingdom.Leader;
				if (leader != null && leader.IsAlive)
				{
					yield return kingdom.Leader;
				}
			}
			Hero leader2 = kingdom.Leader;
			IOrderedEnumerable<Hero> orderedEnumerable;
			if (leader2 == null)
			{
				orderedEnumerable = null;
			}
			else
			{
				orderedEnumerable = from h in leader2.Clan.Heroes.WhereQ<Hero>((Hero h) => h != h.Clan.Kingdom.Leader)
					orderby h.GetRelationWithPlayer()
					select h;
			}
			IOrderedEnumerable<Hero> orderedEnumerable2 = orderedEnumerable;
			foreach (Hero hero in orderedEnumerable2)
			{
				if (!hero.IsChild && hero != Hero.MainHero && hero.IsAlive)
				{
					yield return hero;
				}
			}
			IEnumerator<Hero> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0005EC40 File Offset: 0x0005CE40
		public static TextObject GetFormalDayAndSeasonText(CampaignTime time)
		{
			TextObject textObject = new TextObject("{=CpsPq6WD}the {DAY_ORDINAL} day of {SEASON_NAME}", null);
			TextObject textObject2 = GameTexts.FindText("str_season_" + time.GetSeasonOfYear, null);
			TextObject textObject3 = GameTexts.FindText("str_ordinal_number", (time.GetDayOfSeason + 1).ToString());
			textObject.SetTextVariable("SEASON_NAME", textObject2);
			textObject.SetTextVariable("DAY_ORDINAL", textObject3);
			return textObject;
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0005ECAC File Offset: 0x0005CEAC
		public static SceneNotificationData.SceneNotificationCharacter CreateNotificationCharacterFromHero(Hero hero, Equipment overridenEquipment = null, bool useCivilian = false, BodyProperties overriddenBodyProperties = default(BodyProperties), uint overriddenColor1 = 4294967295U, uint overriddenColor2 = 4294967295U, bool useHorse = false)
		{
			if (overriddenColor1 == 4294967295U)
			{
				IFaction mapFaction = hero.MapFaction;
				overriddenColor1 = ((mapFaction != null) ? mapFaction.Color : hero.CharacterObject.Culture.Color);
			}
			if (overriddenColor2 == 4294967295U)
			{
				IFaction mapFaction2 = hero.MapFaction;
				overriddenColor2 = ((mapFaction2 != null) ? mapFaction2.Color2 : hero.CharacterObject.Culture.Color2);
			}
			if (overridenEquipment == null)
			{
				overridenEquipment = (useCivilian ? hero.CivilianEquipment : hero.BattleEquipment);
			}
			return new SceneNotificationData.SceneNotificationCharacter(hero.CharacterObject, overridenEquipment, overriddenBodyProperties, useCivilian, overriddenColor1, overriddenColor2, useHorse);
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0005ED34 File Offset: 0x0005CF34
		public static SceneNotificationData.SceneNotificationShip CreateNotificationShipFromShip(Ship ship)
		{
			List<ShipVisualSlotInfo> shipVisualSlotInfos = ship.GetShipVisualSlotInfos();
			PartyBase owner = ship.Owner;
			uint? num;
			if (owner == null)
			{
				num = null;
			}
			else
			{
				IFaction mapFaction = owner.MapFaction;
				num = ((mapFaction != null) ? new uint?(mapFaction.Color) : null);
			}
			uint num2 = num ?? uint.MaxValue;
			PartyBase owner2 = ship.Owner;
			uint? num3;
			if (owner2 == null)
			{
				num3 = null;
			}
			else
			{
				IFaction mapFaction2 = owner2.MapFaction;
				num3 = ((mapFaction2 != null) ? new uint?(mapFaction2.Color2) : null);
			}
			uint num4 = num3 ?? uint.MaxValue;
			int randomValue = ship.RandomValue;
			float num5 = (ship.MaxHitPoints.ApproximatelyEqualsTo(0f, 1E-05f) ? 0f : (ship.HitPoints / ship.MaxHitPoints));
			MissionShipObject @object = MBObjectManager.Instance.GetObject<MissionShipObject>(ship.ShipHull.MissionShipObjectId);
			if (@object != null)
			{
				Debug.FailedAssert(string.Format("Ship ({0}) does not have a valid mission ship object id", ship), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\SceneInformationPopupTypes\\CampaignSceneNotificationHelper.cs", "CreateNotificationShipFromShip", 248);
				return new SceneNotificationData.SceneNotificationShip("", shipVisualSlotInfos, num5, num2, num4, randomValue);
			}
			return new SceneNotificationData.SceneNotificationShip(@object.Prefab, shipVisualSlotInfos, num5, num2, num4, randomValue);
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0005EE70 File Offset: 0x0005D070
		public static SceneNotificationData.SceneNotificationShip CreateNotificationShipFromShip(Ship ship, float hitPointRatio)
		{
			List<ShipVisualSlotInfo> shipVisualSlotInfos = ship.GetShipVisualSlotInfos();
			PartyBase owner = ship.Owner;
			uint? num;
			if (owner == null)
			{
				num = null;
			}
			else
			{
				IFaction mapFaction = owner.MapFaction;
				num = ((mapFaction != null) ? new uint?(mapFaction.Color) : null);
			}
			uint num2 = num ?? uint.MaxValue;
			PartyBase owner2 = ship.Owner;
			uint? num3;
			if (owner2 == null)
			{
				num3 = null;
			}
			else
			{
				IFaction mapFaction2 = owner2.MapFaction;
				num3 = ((mapFaction2 != null) ? new uint?(mapFaction2.Color2) : null);
			}
			uint num4 = num3 ?? uint.MaxValue;
			int randomValue = ship.RandomValue;
			hitPointRatio = MathF.Clamp(hitPointRatio, 0f, 1f);
			MissionShipObject @object = MBObjectManager.Instance.GetObject<MissionShipObject>(ship.ShipHull.MissionShipObjectId);
			if (@object != null)
			{
				Debug.FailedAssert(string.Format("Ship ({0}) does not have a valid mission ship object id", ship), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\SceneInformationPopupTypes\\CampaignSceneNotificationHelper.cs", "CreateNotificationShipFromShip", 266);
				return new SceneNotificationData.SceneNotificationShip("", shipVisualSlotInfos, hitPointRatio, num2, num4, randomValue);
			}
			return new SceneNotificationData.SceneNotificationShip(@object.Prefab, shipVisualSlotInfos, hitPointRatio, num2, num4, randomValue);
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0005EF8E File Offset: 0x0005D18E
		public static ItemObject GetDefaultHorseItem()
		{
			return Game.Current.ObjectManager.GetObjectTypeList<ItemObject>().First<ItemObject>((ItemObject i) => i.ItemType == ItemObject.ItemTypeEnum.Horse && i.HasHorseComponent && i.HorseComponent.IsMount && i.HorseComponent.Monster.StringId == "horse");
		}
	}
}
