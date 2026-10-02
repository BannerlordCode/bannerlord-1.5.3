using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000123 RID: 291
	public class DefaultHeroCreationModel : HeroCreationModel
	{
		// Token: 0x060018AB RID: 6315 RVA: 0x00077834 File Offset: 0x00075A34
		public override ValueTuple<CampaignTime, CampaignTime> GetBirthAndDeathDay(CharacterObject character, bool createAlive, int age)
		{
			if (!createAlive)
			{
				CampaignTime campaignTime;
				CampaignTime campaignTime2;
				HeroHelper.GetRandomDeathDayAndBirthDay((int)character.Age, out campaignTime, out campaignTime2);
				return new ValueTuple<CampaignTime, CampaignTime>(campaignTime, campaignTime2);
			}
			if (age == -1)
			{
				CampaignTime randomBirthDayForAge = HeroHelper.GetRandomBirthDayForAge((float)(Campaign.Current.Models.AgeModel.HeroComesOfAge + MBRandom.RandomInt(30)));
				CampaignTime never = CampaignTime.Never;
				return new ValueTuple<CampaignTime, CampaignTime>(randomBirthDayForAge, never);
			}
			if (age == 0)
			{
				CampaignTime now = CampaignTime.Now;
				CampaignTime never2 = CampaignTime.Never;
				return new ValueTuple<CampaignTime, CampaignTime>(now, never2);
			}
			if (character.Occupation == Occupation.Wanderer)
			{
				age = (int)character.Age + MBRandom.RandomInt(5);
				if (age < 20)
				{
					foreach (TraitObject traitObject in TraitObject.All)
					{
						int num = 12 + 4 * character.GetTraitLevel(traitObject);
						if (age < num)
						{
							age = num;
						}
					}
				}
				CampaignTime randomBirthDayForAge2 = HeroHelper.GetRandomBirthDayForAge((float)age);
				CampaignTime never3 = CampaignTime.Never;
				return new ValueTuple<CampaignTime, CampaignTime>(randomBirthDayForAge2, never3);
			}
			CampaignTime randomBirthDayForAge3 = HeroHelper.GetRandomBirthDayForAge((float)age);
			CampaignTime never4 = CampaignTime.Never;
			return new ValueTuple<CampaignTime, CampaignTime>(randomBirthDayForAge3, never4);
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x00077948 File Offset: 0x00075B48
		public override Settlement GetBornSettlement(Hero hero)
		{
			if (hero.Mother != null)
			{
				Settlement settlement;
				if (hero.Mother.CurrentSettlement != null && (hero.Mother.CurrentSettlement.IsTown || hero.Mother.CurrentSettlement.IsVillage))
				{
					settlement = hero.Mother.CurrentSettlement;
				}
				else if (hero.Mother.PartyBelongedTo != null || hero.Mother.PartyBelongedToAsPrisoner != null)
				{
					IMapPoint mapPoint3;
					if (hero.Mother.PartyBelongedToAsPrisoner != null)
					{
						IMapPoint mapPoint2;
						if (!hero.Mother.PartyBelongedToAsPrisoner.IsMobile)
						{
							IMapPoint mapPoint = hero.Mother.PartyBelongedToAsPrisoner.Settlement;
							mapPoint2 = mapPoint;
						}
						else
						{
							IMapPoint mapPoint = hero.Mother.PartyBelongedToAsPrisoner.MobileParty;
							mapPoint2 = mapPoint;
						}
						mapPoint3 = mapPoint2;
					}
					else
					{
						mapPoint3 = hero.Mother.PartyBelongedTo;
					}
					Settlement settlement2;
					MobileParty mobileParty;
					if ((settlement2 = mapPoint3 as Settlement) != null)
					{
						settlement = settlement2;
					}
					else if ((mobileParty = mapPoint3 as MobileParty) != null)
					{
						Town town = SettlementHelper.FindNearestTownToMobileParty(mobileParty, MobileParty.NavigationType.All, null);
						settlement = ((town != null) ? town.Settlement : hero.Mother.HomeSettlement);
					}
					else
					{
						settlement = hero.Mother.HomeSettlement;
					}
				}
				else
				{
					settlement = hero.Mother.HomeSettlement;
				}
				if (settlement == null)
				{
					settlement = ((hero.Mother.Clan.Settlements.Count > 0) ? hero.Mother.Clan.Settlements.GetRandomElement<Settlement>() : Town.AllTowns.GetRandomElement<Town>().Settlement);
				}
				return settlement;
			}
			Settlement settlement3 = SettlementHelper.FindRandomSettlement((Settlement x) => x.IsTown && (hero.Culture.StringId == "neutral_culture" || x.Culture == hero.Culture));
			if (settlement3 == null)
			{
				settlement3 = SettlementHelper.FindRandomSettlement((Settlement x) => x.IsTown);
			}
			return settlement3;
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x00077B54 File Offset: 0x00075D54
		public override StaticBodyProperties GetStaticBodyProperties(Hero hero, bool isOffspring, float variationAmount = 0.2f)
		{
			if (isOffspring)
			{
				string text = hero.CharacterObject.BodyPropertyRange.HairTags;
				string text2 = hero.CharacterObject.BodyPropertyRange.BeardTags;
				string text3 = hero.CharacterObject.BodyPropertyRange.TattooTags;
				bool flag = string.IsNullOrEmpty(text);
				bool flag2 = string.IsNullOrEmpty(text2);
				bool flag3 = string.IsNullOrEmpty(text3);
				if (!flag || !flag2 || !flag3)
				{
					Hero hero2;
					if (hero.IsFemale)
					{
						hero2 = hero.Mother;
					}
					else
					{
						hero2 = hero.Father;
					}
					if (hero2 != null)
					{
						if (!flag)
						{
							text = hero2.CharacterObject.BodyPropertyRange.HairTags;
						}
						if (!flag2)
						{
							text2 = hero2.CharacterObject.BodyPropertyRange.BeardTags;
						}
						if (!flag3)
						{
							text3 = hero2.CharacterObject.BodyPropertyRange.TattooTags;
						}
					}
				}
				BodyProperties bodyProperties = hero.Mother.BodyProperties;
				BodyProperties bodyProperties2 = hero.Father.BodyProperties;
				int num = MBRandom.RandomInt();
				BodyProperties randomBodyProperties = BodyProperties.GetRandomBodyProperties(hero.Mother.CharacterObject.Race, hero.IsFemale, bodyProperties, bodyProperties2, 1, num, text, text2, text3, variationAmount);
				int num2 = -1;
				int num3 = -1;
				int num4 = -1;
				if (string.IsNullOrEmpty(text))
				{
					int[] hairIndicesForCulture = Campaign.Current.Models.BodyPropertiesModel.GetHairIndicesForCulture(hero.CharacterObject.Race, hero.IsFemale ? 1 : 0, hero.Age, hero.Culture);
					num2 = ((hairIndicesForCulture.Length != 0) ? hairIndicesForCulture.GetRandomElement<int>() : (-1));
				}
				if (string.IsNullOrEmpty(text2))
				{
					int[] beardIndicesForCulture = Campaign.Current.Models.BodyPropertiesModel.GetBeardIndicesForCulture(hero.CharacterObject.Race, hero.IsFemale ? 1 : 0, hero.Age, hero.Culture);
					num3 = ((beardIndicesForCulture.Length != 0) ? beardIndicesForCulture.GetRandomElement<int>() : (-1));
				}
				if (string.IsNullOrEmpty(text3))
				{
					int[] tattooIndicesForCulture = Campaign.Current.Models.BodyPropertiesModel.GetTattooIndicesForCulture(hero.CharacterObject.Race, hero.IsFemale ? 1 : 0, hero.Age, hero.Culture);
					num4 = ((tattooIndicesForCulture.Length != 0) ? tattooIndicesForCulture.GetRandomElement<int>() : (-1));
					float tattooZeroProbability = FaceGen.GetTattooZeroProbability(hero.CharacterObject.Race, hero.IsFemale ? 1 : 0, hero.Age);
					if (MBRandom.RandomFloat < tattooZeroProbability)
					{
						num4 = 0;
					}
				}
				FaceGen.SetHair(ref randomBodyProperties, num2, num3, num4);
				return randomBodyProperties.StaticProperties;
			}
			if (hero.CharacterObject.IsOriginalCharacter)
			{
				return hero.CharacterObject.GetBodyPropertiesMin(true).StaticProperties;
			}
			CharacterObject originalCharacter = hero.CharacterObject.OriginalCharacter;
			return BodyProperties.GetRandomBodyProperties(originalCharacter.Race, originalCharacter.IsFemale, originalCharacter.GetBodyPropertiesMin(true), originalCharacter.GetBodyPropertiesMax(true), 0, MBRandom.RandomInt(), originalCharacter.BodyPropertyRange.HairTags, originalCharacter.BodyPropertyRange.BeardTags, originalCharacter.BodyPropertyRange.TattooTags, 0f).StaticProperties;
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x00077E30 File Offset: 0x00076030
		public override FormationClass GetPreferredUpgradeFormation(Hero hero)
		{
			int num = MBRandom.RandomInt(10);
			if (num < 4)
			{
				return (FormationClass)num;
			}
			return FormationClass.NumberOfAllFormations;
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x00077E4D File Offset: 0x0007604D
		public override Clan GetClan(Hero hero)
		{
			if (hero.Mother == null)
			{
				return null;
			}
			if (hero.Father == Hero.MainHero || hero.Mother == Hero.MainHero)
			{
				return Clan.PlayerClan;
			}
			return hero.Father.Clan;
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x00077E84 File Offset: 0x00076084
		public override CultureObject GetCulture(Hero hero, Settlement bornSettlement, Clan clan)
		{
			if (hero.Mother != null)
			{
				if (hero.Father == Hero.MainHero || hero.Mother == Hero.MainHero)
				{
					return Hero.MainHero.Culture;
				}
				if (MBRandom.RandomFloat >= 0.5f)
				{
					return hero.Mother.Culture;
				}
				return hero.Father.Culture;
			}
			else
			{
				if (!hero.CharacterObject.IsOriginalCharacter)
				{
					return hero.CharacterObject.OriginalCharacter.Culture;
				}
				return hero.CharacterObject.Culture;
			}
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x00077F0C File Offset: 0x0007610C
		public override CharacterObject GetRandomTemplateByOccupation(Occupation occupation, Settlement settlement = null)
		{
			Settlement settlement2 = settlement ?? SettlementHelper.GetRandomTown(null);
			List<CharacterObject> list = settlement2.Culture.NotableTemplates.Where<CharacterObject>((CharacterObject x) => x.Occupation == occupation).ToList<CharacterObject>();
			int num = 0;
			foreach (CharacterObject characterObject in list)
			{
				int num2 = characterObject.GetTraitLevel(DefaultTraits.Frequency) * 10;
				num += ((num2 > 0) ? num2 : 100);
			}
			if (!list.Any<CharacterObject>())
			{
				return null;
			}
			int num3 = settlement2.RandomIntWithSeed((uint)settlement2.Notables.Count, 1, num);
			foreach (CharacterObject characterObject2 in list)
			{
				int num4 = characterObject2.GetTraitLevel(DefaultTraits.Frequency) * 10;
				num3 -= ((num4 > 0) ? num4 : 100);
				if (num3 < 0)
				{
					return characterObject2;
				}
			}
			Debug.FailedAssert("Couldn't find template for given occupation!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultHeroCreationModel.cs", "GetRandomTemplateByOccupation", 311);
			return null;
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x00078050 File Offset: 0x00076250
		[return: TupleElementNames(new string[] { "trait", "level" })]
		public override List<ValueTuple<TraitObject, int>> GetTraitsForHero(Hero hero)
		{
			List<ValueTuple<TraitObject, int>> list = new List<ValueTuple<TraitObject, int>>();
			if (hero.Mother != null)
			{
				float randomFloat = MBRandom.RandomFloat;
				int num;
				if (randomFloat < 0.1f)
				{
					num = 0;
				}
				else if (randomFloat < 0.5f)
				{
					num = 1;
				}
				else if (randomFloat < 0.9f)
				{
					num = 2;
				}
				else
				{
					num = 3;
				}
				List<TraitObject> list2 = DefaultTraits.Personality.ToList<TraitObject>();
				list2.Shuffle<TraitObject>();
				for (int i = 0; i < Math.Min(list2.Count, num); i++)
				{
					int num2 = ((MBRandom.RandomFloat < 0.5f) ? MBRandom.RandomInt(list2[i].MinValue, 0) : MBRandom.RandomInt(1, list2[i].MaxValue + 1));
					list.Add(new ValueTuple<TraitObject, int>(list2[i], num2));
				}
				foreach (TraitObject traitObject in TraitObject.All.Except<TraitObject>(DefaultTraits.Personality))
				{
					list.Add(new ValueTuple<TraitObject, int>(traitObject, (MBRandom.RandomFloat < 0.5f) ? hero.Mother.GetTraitLevel(traitObject) : hero.Father.GetTraitLevel(traitObject)));
				}
			}
			if (hero.Occupation == Occupation.GangLeader || hero.Occupation == Occupation.Artisan || hero.Occupation == Occupation.RuralNotable || hero.Occupation == Occupation.Merchant || hero.Occupation == Occupation.Headman)
			{
				list.Add(new ValueTuple<TraitObject, int>(DefaultTraits.Honor, DefaultHeroCreationModel.CalculateTraitValueForHero(hero, DefaultTraits.Honor)));
				list.Add(new ValueTuple<TraitObject, int>(DefaultTraits.Mercy, DefaultHeroCreationModel.CalculateTraitValueForHero(hero, DefaultTraits.Mercy)));
				list.Add(new ValueTuple<TraitObject, int>(DefaultTraits.Generosity, DefaultHeroCreationModel.CalculateTraitValueForHero(hero, DefaultTraits.Generosity)));
				list.Add(new ValueTuple<TraitObject, int>(DefaultTraits.Valor, DefaultHeroCreationModel.CalculateTraitValueForHero(hero, DefaultTraits.Valor)));
				list.Add(new ValueTuple<TraitObject, int>(DefaultTraits.Calculating, DefaultHeroCreationModel.CalculateTraitValueForHero(hero, DefaultTraits.Calculating)));
			}
			return list;
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x00078250 File Offset: 0x00076450
		private static int CalculateTraitValueForHero(Hero hero, TraitObject trait)
		{
			int num = hero.CharacterObject.GetTraitLevel(trait);
			float num2 = (((hero.IsPreacher && trait == DefaultTraits.Generosity) || (hero.IsPreacher && trait == DefaultTraits.Calculating)) ? 0.5f : MBRandom.RandomFloat);
			if (num2 < 0.25f)
			{
				num--;
			}
			else if (num2 > 0.75f)
			{
				num++;
			}
			if (hero.IsGangLeader && (trait == DefaultTraits.Mercy || trait == DefaultTraits.Honor) && num > 0)
			{
				num = 0;
			}
			return MBMath.ClampInt(num, trait.MinValue, trait.MaxValue);
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x000782E3 File Offset: 0x000764E3
		public override Equipment GetCivilianEquipment(Hero hero)
		{
			if (hero.Age < (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
			{
				return Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentForDeliveredOffspring(hero);
			}
			return hero.CivilianEquipment;
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x0007831E File Offset: 0x0007651E
		public override Equipment GetBattleEquipment(Hero hero)
		{
			if (hero.Age < (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
			{
				Equipment equipment = new Equipment(Equipment.EquipmentType.Battle);
				equipment.FillFrom(hero.CivilianEquipment, false);
				return equipment;
			}
			return hero.BattleEquipment;
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x00078357 File Offset: 0x00076557
		public override CharacterObject GetCharacterTemplateForOffspring(Hero mother, Hero father, bool isOffspringFemale)
		{
			if (!isOffspringFemale)
			{
				return father.CharacterObject;
			}
			return mother.CharacterObject;
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x0007836C File Offset: 0x0007656C
		[return: TupleElementNames(new string[] { "firstName", "name" })]
		public override ValueTuple<TextObject, TextObject> GenerateFirstAndFullName(Hero hero)
		{
			TextObject textObject;
			TextObject textObject2;
			NameGenerator.Current.GenerateHeroNameAndHeroFullName(hero, out textObject, out textObject2, false);
			return new ValueTuple<TextObject, TextObject>(textObject, textObject2);
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x00078390 File Offset: 0x00076590
		public override List<ValueTuple<SkillObject, int>> GetDefaultSkillsForHero(Hero hero)
		{
			List<ValueTuple<SkillObject, int>> list = new List<ValueTuple<SkillObject, int>>();
			if (hero.Age < (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
			{
				return list;
			}
			MBCharacterSkills defaultCharacterSkills = hero.CharacterObject.GetDefaultCharacterSkills();
			foreach (SkillObject skillObject in Skills.All)
			{
				int num = defaultCharacterSkills.Skills.GetPropertyValue(skillObject);
				if (num > 0)
				{
					num = DefaultHeroCreationModel.AddNoiseToSkillValue(num);
				}
				list.Add(new ValueTuple<SkillObject, int>(skillObject, num));
			}
			return list;
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x00078438 File Offset: 0x00076638
		private static int GetInheritedSkillValue(Hero hero, SkillObject skillObject)
		{
			Hero father = hero.Father;
			int num = ((father != null) ? father.GetSkillValue(skillObject) : 0);
			Hero mother = hero.Mother;
			int num2 = ((mother != null) ? mother.GetSkillValue(skillObject) : 0);
			int num3 = num + num2;
			return DefaultHeroCreationModel.AddNoiseToSkillValue((MBRandom.RandomInt(0, num3) < num) ? num : num2);
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x00078484 File Offset: 0x00076684
		public override List<ValueTuple<SkillObject, int>> GetInheritedSkillsForHero(Hero hero)
		{
			if (hero.Father == null && hero.Mother == null)
			{
				MBCharacterSkills defaultSkills = hero.CharacterObject.GetDefaultCharacterSkills();
				return Skills.All.Select<SkillObject, ValueTuple<SkillObject, int>>((SkillObject skill) => new ValueTuple<SkillObject, int>(skill, defaultSkills.Skills.GetPropertyValue(skill))).ToList<ValueTuple<SkillObject, int>>();
			}
			List<ValueTuple<SkillObject, int>> list = new List<ValueTuple<SkillObject, int>>();
			SkillObject skillObject = null;
			foreach (SkillObject skillObject2 in Skills.All)
			{
				list.Add(new ValueTuple<SkillObject, int>(skillObject2, DefaultHeroCreationModel.GetInheritedSkillValue(hero, skillObject2)));
			}
			list = list.OrderByDescending<ValueTuple<SkillObject, int>, int>((ValueTuple<SkillObject, int> x) => x.Item2).ToList<ValueTuple<SkillObject, int>>();
			int num = (int)Math.Round((double)((float)list.Count * 0.2777778f));
			int num2 = -1;
			for (int i = 0; i < list.Count; i++)
			{
				ValueTuple<SkillObject, int> valueTuple = list[i];
				if (DefaultHeroCreationModel.IsSkillCombatant(valueTuple.Item1))
				{
					num2 = i;
					skillObject = valueTuple.Item1;
					break;
				}
			}
			list = list.Take<ValueTuple<SkillObject, int>>(num).ToList<ValueTuple<SkillObject, int>>();
			bool flag = !hero.IsFemale || hero.Mother == null || !hero.Mother.IsNoncombatant || MBRandom.RandomFloat < 0.6f;
			if (flag && num2 >= list.Count)
			{
				list[list.Count - 1] = new ValueTuple<SkillObject, int>(skillObject, list[list.Count - 1].Item2);
				num2 = list.Count - 1;
			}
			int num3 = list.Sum<ValueTuple<SkillObject, int>>((ValueTuple<SkillObject, int> x) => x.Item2);
			if (num3 == 0)
			{
				Debug.FailedAssert("Neither parent has any skills!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultHeroCreationModel.cs", "GetInheritedSkillsForHero", 512);
				return new List<ValueTuple<SkillObject, int>>();
			}
			float num4 = (float)(112 * num) / (float)num3;
			if (flag)
			{
				if (MathF.Round((float)list[num2].Item2 * num4) < 100)
				{
					num3 -= list[num2].Item2;
					num4 = (float)(112 * (num - 1)) / (float)num3;
					list[num2] = new ValueTuple<SkillObject, int>(list[num2].Item1, 100);
				}
				else
				{
					list[num2] = new ValueTuple<SkillObject, int>(list[num2].Item1, MathF.Round((float)list[num2].Item2 * num4));
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (!flag || j != num2)
				{
					ValueTuple<SkillObject, int> valueTuple2 = list[j];
					list[j] = new ValueTuple<SkillObject, int>(valueTuple2.Item1, MathF.Round((float)valueTuple2.Item2 * num4));
				}
			}
			return list;
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x00078754 File Offset: 0x00076954
		private static bool IsSkillCombatant(SkillObject skillObject)
		{
			return skillObject == DefaultSkills.OneHanded || skillObject == DefaultSkills.TwoHanded || skillObject == DefaultSkills.Polearm || skillObject == DefaultSkills.Throwing || skillObject == DefaultSkills.Crossbow || skillObject == DefaultSkills.Bow;
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x00078788 File Offset: 0x00076988
		private static int AddNoiseToSkillValue(int skillValue)
		{
			skillValue += MBRandom.RandomInt(5, 10);
			return MathF.Max(skillValue, 1);
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x000787A0 File Offset: 0x000769A0
		public override bool IsHeroCombatant(Hero hero)
		{
			return hero.GetSkillValue(DefaultSkills.OneHanded) >= 100 || hero.GetSkillValue(DefaultSkills.TwoHanded) >= 100 || hero.GetSkillValue(DefaultSkills.Polearm) >= 100 || hero.GetSkillValue(DefaultSkills.Throwing) >= 100 || hero.GetSkillValue(DefaultSkills.Crossbow) >= 100 || hero.GetSkillValue(DefaultSkills.Bow) >= 100;
		}

		// Token: 0x04000817 RID: 2071
		private const int AverageSkillValueForHeroComesOfAge = 112;

		// Token: 0x04000818 RID: 2072
		private const int NonCombatantSkillThresholdValue = 100;

		// Token: 0x04000819 RID: 2073
		private const float FemaleCombatantChance = 0.6f;

		// Token: 0x0400081A RID: 2074
		private const int NoiseValueToAddSkill = 5;
	}
}
