using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000162 RID: 354
	public class DefaultSmithingModel : SmithingModel
	{
		// Token: 0x06001B3C RID: 6972 RVA: 0x0008AFA5 File Offset: 0x000891A5
		public override int GetCraftingPartDifficulty(CraftingPiece craftingPiece)
		{
			if (!craftingPiece.IsEmptyPiece)
			{
				return craftingPiece.PieceTier * 50;
			}
			return 0;
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x0008AFBC File Offset: 0x000891BC
		public override int CalculateWeaponDesignDifficulty(WeaponDesign weaponDesign)
		{
			float num = 0f;
			float num2 = 0f;
			foreach (WeaponDesignElement weaponDesignElement in weaponDesign.UsedPieces)
			{
				if (weaponDesignElement.IsValid && !weaponDesignElement.CraftingPiece.IsEmptyPiece)
				{
					if (weaponDesignElement.CraftingPiece.PieceType == CraftingPiece.PieceTypes.Blade)
					{
						num += 100f;
						num2 += (float)(this.GetCraftingPartDifficulty(weaponDesignElement.CraftingPiece) * 100);
					}
					else if (weaponDesignElement.CraftingPiece.PieceType == CraftingPiece.PieceTypes.Guard)
					{
						num += 20f;
						num2 += (float)(this.GetCraftingPartDifficulty(weaponDesignElement.CraftingPiece) * 20);
					}
					else if (weaponDesignElement.CraftingPiece.PieceType == CraftingPiece.PieceTypes.Handle)
					{
						num += 60f;
						num2 += (float)(this.GetCraftingPartDifficulty(weaponDesignElement.CraftingPiece) * 60);
					}
					else if (weaponDesignElement.CraftingPiece.PieceType == CraftingPiece.PieceTypes.Pommel)
					{
						num += 20f;
						num2 += (float)(this.GetCraftingPartDifficulty(weaponDesignElement.CraftingPiece) * 20);
					}
				}
			}
			return MathF.Round(num2 / num);
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x0008B0D0 File Offset: 0x000892D0
		public override ItemModifier GetCraftedWeaponModifier(WeaponDesign weaponDesign, Hero hero)
		{
			List<ValueTuple<ItemQuality, float>> modifierQualityProbabilities = this.GetModifierQualityProbabilities(weaponDesign, hero);
			ItemQuality itemQuality = modifierQualityProbabilities.Last<ValueTuple<ItemQuality, float>>().Item1;
			float num = MBRandom.RandomFloat;
			foreach (ValueTuple<ItemQuality, float> valueTuple in modifierQualityProbabilities)
			{
				if (num <= valueTuple.Item2)
				{
					itemQuality = valueTuple.Item1;
					break;
				}
				num -= valueTuple.Item2;
			}
			itemQuality = this.AdjustQualityRegardingDesignTier(itemQuality, weaponDesign);
			List<ItemModifier> modifiersBasedOnQuality = weaponDesign.Template.ItemModifierGroup.GetModifiersBasedOnQuality(itemQuality);
			if (modifiersBasedOnQuality.IsEmpty<ItemModifier>())
			{
				return null;
			}
			if (modifiersBasedOnQuality.Count == 1)
			{
				return modifiersBasedOnQuality[0];
			}
			int num2 = MBRandom.RandomInt(0, modifiersBasedOnQuality.Count);
			return modifiersBasedOnQuality[num2];
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x0008B19C File Offset: 0x0008939C
		public override IEnumerable<Crafting.RefiningFormula> GetRefiningFormulas(Hero weaponsmith)
		{
			if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.CharcoalMaker))
			{
				yield return new Crafting.RefiningFormula(CraftingMaterials.Wood, 2, CraftingMaterials.Iron1, 0, CraftingMaterials.Charcoal, 3, CraftingMaterials.IronOre, 0);
			}
			else
			{
				yield return new Crafting.RefiningFormula(CraftingMaterials.Wood, 2, CraftingMaterials.Iron1, 0, CraftingMaterials.Charcoal, 1, CraftingMaterials.IronOre, 0);
			}
			bool perkValue = weaponsmith.GetPerkValue(DefaultPerks.Crafting.IronMaker);
			yield return new Crafting.RefiningFormula(CraftingMaterials.IronOre, 1, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron1, perkValue ? 3 : 2, CraftingMaterials.IronOre, 0);
			yield return new Crafting.RefiningFormula(CraftingMaterials.Iron1, 1, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron2, 1, CraftingMaterials.IronOre, 0);
			yield return new Crafting.RefiningFormula(CraftingMaterials.Iron2, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron3, 1, CraftingMaterials.Iron1, 1);
			if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker))
			{
				yield return new Crafting.RefiningFormula(CraftingMaterials.Iron3, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron4, 1, CraftingMaterials.Iron1, 1);
			}
			if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker2))
			{
				yield return new Crafting.RefiningFormula(CraftingMaterials.Iron4, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron5, 1, CraftingMaterials.Iron1, 1);
			}
			if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker3))
			{
				yield return new Crafting.RefiningFormula(CraftingMaterials.Iron5, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron6, 1, CraftingMaterials.Iron1, 1);
			}
			yield break;
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x0008B1AC File Offset: 0x000893AC
		public override int GetSkillXpForRefining(ref Crafting.RefiningFormula refineFormula)
		{
			return MathF.Round(0.3f * (float)(this.GetCraftingMaterialItem(refineFormula.Output).Value * refineFormula.OutputCount));
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x0008B1D4 File Offset: 0x000893D4
		public override int GetSkillXpForSmelting(ItemObject item)
		{
			return MathF.Round(0.02f * (float)item.Value);
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x0008B1E8 File Offset: 0x000893E8
		public override int GetSkillXpForSmithingInFreeBuildMode(ItemObject item)
		{
			return MathF.Round(0.02f * (float)item.Value);
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x0008B1FC File Offset: 0x000893FC
		public override int GetSkillXpForSmithingInCraftingOrderMode(ItemObject item)
		{
			return MathF.Round(0.1f * (float)item.Value);
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x0008B210 File Offset: 0x00089410
		public override int GetEnergyCostForRefining(ref Crafting.RefiningFormula refineFormula, Hero hero)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(6f, false, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crafting.PracticalRefiner, BattleEnvironment.Any, hero.CharacterObject, true, ref explainedNumber);
			return (int)explainedNumber.ResultNumber;
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x0008B248 File Offset: 0x00089448
		public override int GetEnergyCostForSmithing(ItemObject item, Hero hero)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber((float)(10 + ItemObject.ItemTiers.Tier6 * item.Tier), false, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crafting.PracticalSmith, BattleEnvironment.Any, hero.CharacterObject, true, ref explainedNumber);
			return (int)explainedNumber.ResultNumber;
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x0008B288 File Offset: 0x00089488
		public override int GetEnergyCostForSmelting(ItemObject item, Hero hero)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(10f, false, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crafting.PracticalSmelter, BattleEnvironment.Any, hero.CharacterObject, true, ref explainedNumber);
			return (int)explainedNumber.ResultNumber;
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x0008B2C0 File Offset: 0x000894C0
		public override ItemObject GetCraftingMaterialItem(CraftingMaterials craftingMaterial)
		{
			switch (craftingMaterial)
			{
			case CraftingMaterials.IronOre:
				return DefaultItems.IronOre;
			case CraftingMaterials.Iron1:
				return DefaultItems.IronIngot1;
			case CraftingMaterials.Iron2:
				return DefaultItems.IronIngot2;
			case CraftingMaterials.Iron3:
				return DefaultItems.IronIngot3;
			case CraftingMaterials.Iron4:
				return DefaultItems.IronIngot4;
			case CraftingMaterials.Iron5:
				return DefaultItems.IronIngot5;
			case CraftingMaterials.Iron6:
				return DefaultItems.IronIngot6;
			case CraftingMaterials.Wood:
				return DefaultItems.HardWood;
			case CraftingMaterials.Charcoal:
				return DefaultItems.Charcoal;
			default:
				return DefaultItems.IronIngot1;
			}
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x0008B334 File Offset: 0x00089534
		public override int[] GetSmeltingOutputForItem(ItemObject item)
		{
			int[] array = new int[9];
			if (item.WeaponDesign != null)
			{
				foreach (WeaponDesignElement weaponDesignElement in item.WeaponDesign.UsedPieces)
				{
					if (weaponDesignElement != null && weaponDesignElement.IsValid)
					{
						foreach (ValueTuple<CraftingMaterials, int> valueTuple in weaponDesignElement.CraftingPiece.MaterialsUsed)
						{
							array[(int)valueTuple.Item1] += valueTuple.Item2;
						}
					}
				}
				this.AddSmeltingReductions(array);
			}
			return array;
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x0008B3E8 File Offset: 0x000895E8
		[return: TupleElementNames(new string[] { "quality", "probability" })]
		private List<ValueTuple<ItemQuality, float>> GetModifierQualityProbabilities(WeaponDesign weaponDesign, Hero hero)
		{
			int num = this.CalculateWeaponDesignDifficulty(weaponDesign);
			int skillValue = hero.CharacterObject.GetSkillValue(DefaultSkills.Crafting);
			List<ValueTuple<ItemQuality, float>> list = new List<ValueTuple<ItemQuality, float>>();
			ExplainedNumber explainedNumber = new ExplainedNumber((float)(-(float)num), false, null);
			SkillHelper.AddSkillBonusForCharacter(DefaultSkillEffects.SmithingLevel, hero.CharacterObject, ref explainedNumber);
			explainedNumber.LimitMin(-300f);
			explainedNumber.LimitMax(300f, null);
			list.Add(new ValueTuple<ItemQuality, float>(ItemQuality.Poor, 0.36f * (1f - this.CalculateSigmoidFunction(explainedNumber.ResultNumber, -70f, 0.018f))));
			list.Add(new ValueTuple<ItemQuality, float>(ItemQuality.Inferior, 0.45f * (1f - this.CalculateSigmoidFunction(explainedNumber.ResultNumber, -55f, 0.018f))));
			list.Add(new ValueTuple<ItemQuality, float>(ItemQuality.Common, this.CalculateSigmoidFunction(explainedNumber.ResultNumber, 25f, 0.018f)));
			list.Add(new ValueTuple<ItemQuality, float>(ItemQuality.Fine, 0.36f * this.CalculateSigmoidFunction(explainedNumber.ResultNumber, 40f, 0.018f)));
			list.Add(new ValueTuple<ItemQuality, float>(ItemQuality.Masterwork, 0.27f * this.CalculateSigmoidFunction(explainedNumber.ResultNumber, 70f, 0.018f)));
			list.Add(new ValueTuple<ItemQuality, float>(ItemQuality.Legendary, 0.18f * this.CalculateSigmoidFunction(explainedNumber.ResultNumber, 115f, 0.018f)));
			float num2 = list.Sum<ValueTuple<ItemQuality, float>>(([TupleElementNames(new string[] { "quality", "probability" })] ValueTuple<ItemQuality, float> tuple) => tuple.Item2);
			for (int i = 0; i < list.Count; i++)
			{
				ValueTuple<ItemQuality, float> valueTuple = list[i];
				list[i] = new ValueTuple<ItemQuality, float>(valueTuple.Item1, valueTuple.Item2 / num2);
			}
			List<ItemQuality> list2 = new List<ItemQuality>();
			bool perkValue = hero.CharacterObject.GetPerkValue(DefaultPerks.Crafting.ExperiencedSmith);
			if (perkValue)
			{
				list2.Add(ItemQuality.Masterwork);
				list2.Add(ItemQuality.Legendary);
				DefaultSmithingModel.AdjustModifierProbabilities(list, ItemQuality.Fine, DefaultPerks.Crafting.ExperiencedSmith.PrimaryBonus, list2);
			}
			bool perkValue2 = hero.CharacterObject.GetPerkValue(DefaultPerks.Crafting.MasterSmith);
			if (perkValue2)
			{
				list2.Clear();
				list2.Add(ItemQuality.Legendary);
				if (perkValue)
				{
					list2.Add(ItemQuality.Fine);
				}
				DefaultSmithingModel.AdjustModifierProbabilities(list, ItemQuality.Masterwork, DefaultPerks.Crafting.MasterSmith.PrimaryBonus, list2);
			}
			if (hero.CharacterObject.GetPerkValue(DefaultPerks.Crafting.LegendarySmith))
			{
				list2.Clear();
				if (perkValue)
				{
					list2.Add(ItemQuality.Fine);
				}
				if (perkValue2)
				{
					list2.Add(ItemQuality.Masterwork);
				}
				float num3 = DefaultPerks.Crafting.LegendarySmith.PrimaryBonus + Math.Max((float)(skillValue - 275), 0f) / 5f * 0.01f;
				DefaultSmithingModel.AdjustModifierProbabilities(list, ItemQuality.Legendary, num3, list2);
			}
			return list;
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x0008B698 File Offset: 0x00089898
		private static void AdjustModifierProbabilities([TupleElementNames(new string[] { "quality", "probability" })] List<ValueTuple<ItemQuality, float>> modifierProbabilities, ItemQuality qualityToAdjust, float amount, List<ItemQuality> qualitiesToIgnore)
		{
			int num = modifierProbabilities.Count - (qualitiesToIgnore.Count + 1);
			float num2 = amount / (float)num;
			float num3 = 0f;
			for (int i = 0; i < modifierProbabilities.Count; i++)
			{
				ValueTuple<ItemQuality, float> valueTuple = modifierProbabilities[i];
				if (valueTuple.Item1 == qualityToAdjust)
				{
					modifierProbabilities[i] = new ValueTuple<ItemQuality, float>(valueTuple.Item1, valueTuple.Item2 + amount);
				}
				else if (!qualitiesToIgnore.Contains(valueTuple.Item1))
				{
					float num4 = valueTuple.Item2 - (num2 + num3);
					if (num4 < 0f)
					{
						num3 = -num4;
						num4 = 0f;
					}
					else
					{
						num3 = 0f;
					}
					modifierProbabilities[i] = new ValueTuple<ItemQuality, float>(valueTuple.Item1, num4);
				}
			}
			float num5 = modifierProbabilities.Sum<ValueTuple<ItemQuality, float>>(([TupleElementNames(new string[] { "quality", "probability" })] ValueTuple<ItemQuality, float> tuple) => tuple.Item2);
			for (int j = 0; j < modifierProbabilities.Count; j++)
			{
				ValueTuple<ItemQuality, float> valueTuple2 = modifierProbabilities[j];
				modifierProbabilities[j] = new ValueTuple<ItemQuality, float>(valueTuple2.Item1, valueTuple2.Item2 / num5);
			}
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x0008B7C0 File Offset: 0x000899C0
		private ItemQuality AdjustQualityRegardingDesignTier(ItemQuality weaponQuality, WeaponDesign weaponDesign)
		{
			int num = 0;
			float num2 = 0f;
			foreach (WeaponDesignElement weaponDesignElement in weaponDesign.UsedPieces)
			{
				if (weaponDesignElement.IsValid)
				{
					num2 += (float)weaponDesignElement.CraftingPiece.PieceTier;
					num++;
				}
			}
			num2 /= (float)num;
			if (num2 >= 4.5f)
			{
				return weaponQuality;
			}
			if (num2 >= 3.5f)
			{
				if (weaponQuality < ItemQuality.Legendary)
				{
					return weaponQuality;
				}
				return ItemQuality.Masterwork;
			}
			else
			{
				if (weaponQuality < ItemQuality.Masterwork)
				{
					return weaponQuality;
				}
				return ItemQuality.Fine;
			}
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x0008B834 File Offset: 0x00089A34
		private float CalculateSigmoidFunction(float x, float mean, float curvature)
		{
			double num = Math.Exp((double)(curvature * (x - mean)));
			return (float)(num / (1.0 + num));
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x0008B85B File Offset: 0x00089A5B
		private float GetDifficultyForElement(WeaponDesignElement weaponDesignElement)
		{
			return (float)weaponDesignElement.CraftingPiece.PieceTier * (1f + 0.5f * weaponDesignElement.ScaleFactor);
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x0008B87C File Offset: 0x00089A7C
		private void AddSmeltingReductions(int[] quantities)
		{
			if (quantities[6] > 0)
			{
				quantities[6]--;
				quantities[5]++;
			}
			else if (quantities[5] > 0)
			{
				quantities[5]--;
				quantities[4]++;
			}
			else if (quantities[4] > 0)
			{
				quantities[4]--;
				quantities[3]++;
			}
			else if (quantities[3] > 0)
			{
				quantities[3]--;
				quantities[2]++;
			}
			else if (quantities[2] > 0)
			{
				quantities[2]--;
				quantities[1]++;
			}
			quantities[8]--;
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x0008B934 File Offset: 0x00089B34
		public override int[] GetSmithingCostsForWeaponDesign(WeaponDesign weaponDesign)
		{
			int[] array = new int[9];
			foreach (WeaponDesignElement weaponDesignElement in weaponDesign.UsedPieces)
			{
				if (weaponDesignElement != null && weaponDesignElement.IsValid)
				{
					foreach (ValueTuple<CraftingMaterials, int> valueTuple in weaponDesignElement.CraftingPiece.MaterialsUsed)
					{
						array[(int)valueTuple.Item1] -= valueTuple.Item2;
					}
				}
			}
			array[8]--;
			return array;
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x0008B9D8 File Offset: 0x00089BD8
		public override float ResearchPointsNeedForNewPart(int totalPartCount, int openedPartCount)
		{
			return MathF.Sqrt(100f / (float)totalPartCount) * ((float)openedPartCount * 9f + 10f);
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x0008B9F8 File Offset: 0x00089BF8
		public override int GetPartResearchGainForSmeltingItem(ItemObject item, Hero hero)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(1f + (float)MathF.Round(0.02f * (float)item.Value), false, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crafting.CuriousSmelter, BattleEnvironment.Any, hero.CharacterObject, true, ref explainedNumber);
			return (int)explainedNumber.ResultNumber;
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x0008BA44 File Offset: 0x00089C44
		public override int GetPartResearchGainForSmithingItem(ItemObject item, Hero hero, bool isFreeBuild)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crafting.CuriousSmith, BattleEnvironment.Any, hero.CharacterObject, true, ref explainedNumber);
			if (isFreeBuild)
			{
				explainedNumber.AddFactor(0.1f, null);
			}
			return 1 + MathF.Floor(0.1f * (float)item.Value * explainedNumber.ResultNumber);
		}

		// Token: 0x0400090E RID: 2318
		private const int BladeDifficultyCalculationWeight = 100;

		// Token: 0x0400090F RID: 2319
		private const int GuardDifficultyCalculationWeight = 20;

		// Token: 0x04000910 RID: 2320
		private const int HandleDifficultyCalculationWeight = 60;

		// Token: 0x04000911 RID: 2321
		private const int PommelDifficultyCalculationWeight = 20;
	}
}
