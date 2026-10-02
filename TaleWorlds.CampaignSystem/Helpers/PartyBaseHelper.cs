using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x0200000E RID: 14
	public static class PartyBaseHelper
	{
		// Token: 0x06000072 RID: 114 RVA: 0x0000714C File Offset: 0x0000534C
		public static void SortRoster(MobileParty mobileParty)
		{
			CharacterObject characterObject = null;
			foreach (TroopRosterElement troopRosterElement in mobileParty.MemberRoster.GetTroopRoster())
			{
				if (characterObject == null || characterObject.Tier < troopRosterElement.Character.Tier)
				{
					characterObject = troopRosterElement.Character;
					if (characterObject.Tier == Campaign.Current.Models.CharacterStatsModel.MaxCharacterTier)
					{
						break;
					}
				}
			}
			if (characterObject != null)
			{
				mobileParty.MemberRoster.SwapTroopsAtIndices(mobileParty.MemberRoster.FindIndexOfTroop(characterObject), 0);
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000071F8 File Offset: 0x000053F8
		public static TextObject GetPartySizeText(PartyBase party)
		{
			TextObject textObject;
			if (party.NumberOfHealthyMembers == party.NumberOfAllMembers)
			{
				textObject = new TextObject(party.NumberOfHealthyMembers.ToString(), null);
			}
			else
			{
				MBTextManager.SetTextVariable("HEALTHY_NUM", party.NumberOfHealthyMembers);
				MBTextManager.SetTextVariable("WOUNDED_NUM", party.NumberOfAllMembers - party.NumberOfHealthyMembers);
				textObject = GameTexts.FindText("str_party_health", null);
			}
			return textObject;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00007260 File Offset: 0x00005460
		public static TextObject GetPartySizeText(int healtyNumber, int woundedNumber, bool isInspected)
		{
			string text = "";
			if (!isInspected)
			{
				string text2 = new int[] { 0, 10, 100, 1000 }.Where<int>((int t) => t < healtyNumber + woundedNumber).Aggregate(text, (string current, int t) => current + "?");
				return new TextObject("{=!}" + text2, null);
			}
			if (woundedNumber == 0)
			{
				return new TextObject(healtyNumber, null);
			}
			TextObject textObject = GameTexts.FindText("str_party_health", null);
			textObject.SetTextVariable("HEALTHY_NUM", healtyNumber);
			textObject.SetTextVariable("WOUNDED_NUM", woundedNumber);
			return textObject;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00007328 File Offset: 0x00005528
		public static string GetShipSizeText(int shipCount, bool isInspected)
		{
			if (isInspected)
			{
				return shipCount.ToString();
			}
			return "?";
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000733C File Offset: 0x0000553C
		public static float FindPartySizeNormalLimit(MobileParty mobileParty)
		{
			int num;
			if (mobileParty.LeaderHero != null)
			{
				Hero owner = mobileParty.Party.Owner;
				if (((owner != null) ? owner.Clan : null) != null && mobileParty.LeaderHero != mobileParty.Party.Owner.Clan.Leader)
				{
					num = mobileParty.LeaderHero.CharacterObject.TroopWage;
					goto IL_0051;
				}
			}
			num = 0;
			IL_0051:
			int num2 = Math.Min(num, mobileParty.TotalWage);
			int num3 = (int)((float)(mobileParty.PaymentLimit - num2) / Campaign.Current.AverageWage) + 1;
			int num4 = MathF.Max(1, MathF.Min(num3, mobileParty.Party.PartySizeLimit));
			return MathF.Max(0.1f, (float)num4 / (float)mobileParty.Party.PartySizeLimit);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000073F0 File Offset: 0x000055F0
		public static Hero GetCaptainOfTroop(PartyBase affectorParty, CharacterObject affectorCharacter)
		{
			foreach (TroopRosterElement troopRosterElement in affectorParty.MemberRoster.GetTroopRoster())
			{
				if (troopRosterElement.Character.IsHero && !troopRosterElement.Character.HeroObject.IsWounded && MBRandom.RandomFloat < 0.2f)
				{
					return troopRosterElement.Character.HeroObject;
				}
			}
			return affectorParty.LeaderHero;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00007484 File Offset: 0x00005684
		public static string PrintRosterContents(TroopRoster roster)
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "PrintRosterContents");
			for (int i = 0; i < roster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = roster.GetElementCopyAtIndex(i);
				TextObject textObject;
				if (elementCopyAtIndex.Character.IsHero)
				{
					textObject = elementCopyAtIndex.Character.Name;
				}
				else
				{
					TextObject textObject2 = new TextObject("{=fW0XS9JC}{ELEMENT_NUMBER} {ELEMENT_CHAR_NAME}", null);
					textObject2.SetTextVariable("ELEMENT_NUMBER", elementCopyAtIndex.Number);
					textObject2.SetTextVariable("ELEMENT_CHAR_NAME", elementCopyAtIndex.Character.Name);
					textObject = textObject2;
				}
				mbstringBuilder.Append<TextObject>(textObject);
				if (i < roster.Count - 1)
				{
					mbstringBuilder.Append<string>(", ");
				}
			}
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000753C File Offset: 0x0000573C
		public static TextObject PrintSummarisedItemRoster(ItemRoster items)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			ItemObject itemObject = null;
			ItemObject itemObject2 = null;
			ItemObject itemObject3 = null;
			ItemObject itemObject4 = null;
			for (int i = 0; i < items.Count; i++)
			{
				ItemRosterElement elementCopyAtIndex = items.GetElementCopyAtIndex(i);
				ItemObject item = elementCopyAtIndex.EquipmentElement.Item;
				if (item.IsTradeGood)
				{
					bool flag;
					if (itemObject3 == null)
					{
						flag = true;
					}
					else
					{
						int value = itemObject3.Value;
						flag = false;
					}
					int? num9 = (flag ? new int?(-1) : ((itemObject3 != null) ? new int?(itemObject3.Value) : null));
					int num10 = item.Value;
					if ((num9.GetValueOrDefault() < num10) & (num9 != null))
					{
						num6 = elementCopyAtIndex.Amount;
						itemObject3 = item;
					}
					num5 += elementCopyAtIndex.Amount;
				}
				else if (item.HasArmorComponent)
				{
					bool flag2;
					if (itemObject2 == null)
					{
						flag2 = true;
					}
					else
					{
						int value2 = itemObject2.Value;
						flag2 = false;
					}
					int? num9 = (flag2 ? new int?(-1) : ((itemObject2 != null) ? new int?(itemObject2.Value) : null));
					int num10 = item.Value;
					if ((num9.GetValueOrDefault() < num10) & (num9 != null))
					{
						num4 = elementCopyAtIndex.Amount;
						itemObject2 = item;
					}
					num3 += elementCopyAtIndex.Amount;
				}
				else if (item.WeaponComponent != null)
				{
					bool flag3;
					if (itemObject == null)
					{
						flag3 = true;
					}
					else
					{
						int value3 = itemObject.Value;
						flag3 = false;
					}
					int? num9 = (flag3 ? new int?(-1) : ((itemObject != null) ? new int?(itemObject.Value) : null));
					int num10 = item.Value;
					if ((num9.GetValueOrDefault() < num10) & (num9 != null))
					{
						num2 = elementCopyAtIndex.Amount;
						itemObject = item;
					}
					num += elementCopyAtIndex.Amount;
				}
				else
				{
					bool flag4;
					if (itemObject4 == null)
					{
						flag4 = true;
					}
					else
					{
						int value4 = itemObject4.Value;
						flag4 = false;
					}
					int? num9 = (flag4 ? new int?(-1) : ((itemObject4 != null) ? new int?(itemObject4.Value) : null));
					int num10 = item.Value;
					if ((num9.GetValueOrDefault() < num10) & (num9 != null))
					{
						num8 = elementCopyAtIndex.Amount;
						itemObject4 = item;
					}
					num7 += elementCopyAtIndex.Amount;
				}
			}
			num5 -= num6;
			num3 -= num4;
			num -= num2;
			num7 -= num8;
			int[] array = new int[] { num6, num4, num2, num8 };
			int[] array2 = new int[] { num5, num3, num, num7 };
			ItemObject[] array3 = new ItemObject[] { itemObject3, itemObject2, itemObject, itemObject4 };
			TextObject[,] array4 = new TextObject[4, 2];
			array4[0, 0] = new TextObject("{=nc9KELFA}trade goods", null);
			array4[0, 1] = new TextObject("{=eVcvaxz6}trade good", null);
			array4[1, 0] = new TextObject("{=YJJwR5PB}pieces of armour", null);
			array4[1, 1] = new TextObject("{=pF47ldtJ}piece of armour", null);
			array4[2, 0] = new TextObject("{=ADabRUeh}weapons", null);
			array4[2, 1] = new TextObject("{=Rs8xhY46}weapon", null);
			array4[3, 0] = new TextObject("{=Py5jvZWL}type of items", null);
			array4[3, 1] = new TextObject("{=2HmzaFVK}type of item", null);
			TextObject[,] array5 = array4;
			List<TextObject> list = new List<TextObject>();
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] != 0)
				{
					TextObject textObject = new TextObject("{=eBea9Ext}{VALUABLE_ITEM_COUNT} {VALUABLE_ITEM_NAME}{?IS_THERE_OTHER_ITEMS} and {?PLURAL}{OTHER_ITEMS_COUNT}other {OTHER_ITEMS_CATEGORY_PLURAL}{?}an other {OTHER_ITEMS_CATEGORY_SINGULAR}{\\?}{?}{\\?}", null);
					textObject.SetTextVariable("OTHER_ITEMS_COUNT", array2[j]);
					textObject.SetTextVariable("OTHER_ITEMS_CATEGORY_PLURAL", array5[j, 0]);
					textObject.SetTextVariable("OTHER_ITEMS_CATEGORY_SINGULAR", array5[j, 1]);
					textObject.SetTextVariable("VALUABLE_ITEM_COUNT", array[j]);
					textObject.SetTextVariable("VALUABLE_ITEM_NAME", array3[j].Name);
					textObject.SetTextVariable("IS_THERE_OTHER_ITEMS", (array2[j] > 0) ? 1 : 0);
					textObject.SetTextVariable("PLURAL", (array2[j] == 1) ? 0 : 1);
					list.Add(textObject);
				}
			}
			if (list.Count <= 0)
			{
				return TextObject.GetEmpty();
			}
			return GameTexts.GameTextHelper.MergeTextObjectsWithComma(list, false);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00007978 File Offset: 0x00005B78
		public static TextObject PrintRegularTroopCategories(TroopRoster roster)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < roster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = roster.GetElementCopyAtIndex(i);
				CharacterObject character = elementCopyAtIndex.Character;
				if (!character.IsHero && elementCopyAtIndex.Number != 0)
				{
					if (character.IsInfantry)
					{
						num += elementCopyAtIndex.Number;
					}
					else if (character.IsRanged)
					{
						if (character.IsMounted)
						{
							num4 += elementCopyAtIndex.Number;
						}
						else
						{
							num2 += elementCopyAtIndex.Number;
						}
					}
					else if (character.IsMounted)
					{
						num3 += elementCopyAtIndex.Number;
					}
				}
			}
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			if (num != 0)
			{
				dictionary.Add("Infantry", num);
			}
			if (num2 != 0)
			{
				dictionary.Add("Ranged", num2);
			}
			if (num3 != 0)
			{
				dictionary.Add("Cavalry", num3);
			}
			if (num4 != 0)
			{
				dictionary.Add("HorseArcher", num4);
			}
			List<TextObject> list = new List<TextObject>();
			foreach (KeyValuePair<string, int> keyValuePair in dictionary)
			{
				TextObject textObject = new TextObject("{=ksTDGuXs}{TROOP_TYPE_COUNT} {TROOP_TYPE} {?TROOP_TYPE_COUNT>1}troops{?}troop{\\?}", null);
				textObject.SetTextVariable("TROOP_TYPE_COUNT", keyValuePair.Value);
				textObject.SetTextVariable("TROOP_TYPE", GameTexts.FindText("str_troop_type_name", keyValuePair.Key));
				list.Add(textObject);
			}
			return GameTexts.GameTextHelper.MergeTextObjectsWithComma(list, true);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00007AF8 File Offset: 0x00005CF8
		public static CharacterObject GetVisualPartyLeader(PartyBase party)
		{
			if (party == null)
			{
				return null;
			}
			if (party.LeaderHero != null)
			{
				return party.LeaderHero.CharacterObject;
			}
			TroopRoster memberRoster = party.MemberRoster;
			if (memberRoster == null || memberRoster.TotalManCount <= 0)
			{
				return null;
			}
			return party.MemberRoster.GetCharacterAtIndex(0);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00007B38 File Offset: 0x00005D38
		public static int GetSpeedLimitation(ItemRoster partyItemRoster, out ItemObject speedLimitationItem)
		{
			speedLimitationItem = null;
			int num = 100;
			foreach (ItemRosterElement itemRosterElement in partyItemRoster)
			{
				if (itemRosterElement.EquipmentElement.Item != null && itemRosterElement.EquipmentElement.Item.IsAnimal && num > itemRosterElement.EquipmentElement.GetModifiedMountSpeed(in EquipmentElement.Invalid))
				{
					num = itemRosterElement.EquipmentElement.GetModifiedMountSpeed(in EquipmentElement.Invalid);
					speedLimitationItem = itemRosterElement.EquipmentElement.Item;
				}
			}
			return num;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00007BE4 File Offset: 0x00005DE4
		public static bool HasFeat(PartyBase party, FeatObject feat)
		{
			if (party == null)
			{
				return false;
			}
			if (party.LeaderHero != null)
			{
				return party.LeaderHero.Culture.HasFeat(feat);
			}
			if (party.Culture != null)
			{
				return party.Culture.HasFeat(feat);
			}
			if (party.Owner != null)
			{
				return party.Owner.Culture.HasFeat(feat);
			}
			return party.Settlement != null && party.Settlement.Culture.HasFeat(feat);
		}
	}
}
