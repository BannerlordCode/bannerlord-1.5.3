using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.BattleWreckages;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Information.RundownTooltip;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection
{
	// Token: 0x0200001F RID: 31
	public static class TooltipRefresherCollection
	{
		// Token: 0x060001D9 RID: 473 RVA: 0x0000CBD0 File Offset: 0x0000ADD0
		public static void RefreshExplainedNumberTooltip(RundownTooltipVM explainedNumberTooltip, object[] args)
		{
			explainedNumberTooltip.IsActive = explainedNumberTooltip.IsInitializedProperly;
			if (explainedNumberTooltip.IsActive)
			{
				Func<ExplainedNumber> func = args[0] as Func<ExplainedNumber>;
				Func<ExplainedNumber> func2 = args[1] as Func<ExplainedNumber>;
				explainedNumberTooltip.Lines.Clear();
				Func<ExplainedNumber> func3 = ((explainedNumberTooltip.IsExtended && func2 != null) ? func2 : func);
				if (func3 != null)
				{
					ExplainedNumber explainedNumber = func3();
					explainedNumberTooltip.CurrentExpectedChange = explainedNumber.ResultNumber;
					foreach (ValueTuple<string, float> valueTuple in explainedNumber.GetLines())
					{
						explainedNumberTooltip.Lines.Add(new RundownLineVM(valueTuple.Item1, valueTuple.Item2));
					}
				}
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000CC98 File Offset: 0x0000AE98
		public static void RefreshTrackTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			Track track = args[0] as Track;
			propertyBasedTooltipVM.Mode = 1;
			MapTrackModel mapTrackModel = Campaign.Current.Models.MapTrackModel;
			if (mapTrackModel != null)
			{
				TextObject textObject = mapTrackModel.TrackTitle(track);
				propertyBasedTooltipVM.AddProperty("", textObject.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
				foreach (ValueTuple<TextObject, string> valueTuple in mapTrackModel.GetTrackDescription(track))
				{
					TextObject item = valueTuple.Item1;
					propertyBasedTooltipVM.AddProperty((item != null) ? item.ToString() : null, valueTuple.Item2, 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000CD44 File Offset: 0x0000AF44
		public static void RefreshHeroTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			Hero hero = args[0] as Hero;
			bool flag = (bool)args[1];
			StringHelpers.SetCharacterProperties("NPC", hero.CharacterObject, null, false);
			TextObject textObject;
			bool flag2 = CampaignUIHelper.IsHeroInformationHidden(hero, out textObject);
			if (hero.IsEnemy(Hero.MainHero))
			{
				propertyBasedTooltipVM.Mode = 3;
			}
			else if (hero == Hero.MainHero || hero.IsFriend(Hero.MainHero))
			{
				propertyBasedTooltipVM.Mode = 2;
			}
			else
			{
				propertyBasedTooltipVM.Mode = 1;
			}
			propertyBasedTooltipVM.AddProperty("", hero.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
			if (!hero.IsNotable && !hero.IsWanderer)
			{
				Clan clan = hero.Clan;
				if (((clan != null) ? clan.Kingdom : null) != null)
				{
					propertyBasedTooltipVM.AddProperty("", CampaignUIHelper.GetHeroKingdomRank(hero), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (Game.Current.IsDevelopmentMode)
				{
					Clan clan2 = hero.Clan;
					if (((clan2 != null) ? clan2.Leader : null) == hero)
					{
						propertyBasedTooltipVM.AddProperty("DEBUG Clan Leader", "", 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
				if (Game.Current.IsDevelopmentMode)
				{
					Clan clan3 = hero.Clan;
					if (((clan3 != null) ? clan3.Kingdom : null) != null)
					{
						Hero hero2 = hero;
						IFaction mapFaction = hero.MapFaction;
						if (hero2 == ((mapFaction != null) ? mapFaction.Leader : null))
						{
							Clan clan4 = hero.Clan;
							if (((clan4 != null) ? clan4.Kingdom : null) != null)
							{
								propertyBasedTooltipVM.AddProperty("DEBUG Kingdom Gold", hero.Clan.Kingdom.KingdomBudgetWallet.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
							}
						}
						propertyBasedTooltipVM.AddProperty("DEBUG Gold", hero.Gold.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
						if (Game.Current.IsDevelopmentMode && hero.Clan != null && hero.Clan.IsUnderMercenaryService)
						{
							propertyBasedTooltipVM.AddProperty("DEBUG Mercenary Award", hero.Clan.MercenaryAwardMultiplier.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
						}
						if (Game.Current.IsDevelopmentMode)
						{
							Clan clan5 = hero.Clan;
							if (((clan5 != null) ? clan5.Leader : null) == hero)
							{
								propertyBasedTooltipVM.AddProperty("DEBUG Debt To Kingdom", hero.Clan.DebtToKingdom.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
							}
						}
					}
				}
				if (Game.Current.IsDevelopmentMode && hero.PartyBelongedTo != null && !hero.IsSpecial)
				{
					propertyBasedTooltipVM.AddProperty("DEBUG Party Size", hero.PartyBelongedTo.MemberRoster.TotalManCount.ToString() + "/" + hero.PartyBelongedTo.Party.PartySizeLimit.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty("DEBUG Party Position", ((int)hero.PartyBelongedTo.Position.X).ToString() + "," + ((int)hero.PartyBelongedTo.Position.Y).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty("DEBUG Party Wage", hero.PartyBelongedTo.TotalWage.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (Game.Current.IsDevelopmentMode && hero.PartyBelongedTo != null)
				{
					propertyBasedTooltipVM.AddProperty("DEBUG Party Morale", hero.PartyBelongedTo.Morale.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (Game.Current.IsDevelopmentMode && hero.PartyBelongedTo != null)
				{
					propertyBasedTooltipVM.AddProperty("DEBUG Starving", hero.PartyBelongedTo.Party.IsStarving.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (Game.Current.IsDevelopmentMode)
				{
					IFaction mapFaction2 = hero.MapFaction;
					if (((mapFaction2 != null) ? mapFaction2.Leader : null) != null && hero != hero.MapFaction.Leader)
					{
						propertyBasedTooltipVM.AddProperty("DEBUG King Relation", hero.GetRelation(hero.MapFaction.Leader).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
				if (Game.Current.IsDevelopmentMode && hero.PartyBelongedToAsPrisoner != null)
				{
					propertyBasedTooltipVM.AddProperty("DEBUG Prisoner at", hero.PartyBelongedToAsPrisoner.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			if (hero.Clan != null)
			{
				propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_clan", null).ToString(), hero.Clan.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			Clan clan6 = hero.Clan;
			if (clan6 != null && clan6.HasBloodFeudWithPlayer)
			{
				propertyBasedTooltipVM.AddProperty("", new TextObject("{=AZaCxlzZ}{BLOOD_ICON} In a blood feud with the {CLAN_NAME}", null).SetTextVariable("BLOOD_ICON", "{=!}<img src=\"SPGeneral\\blood_feud_icon\" extend=\"3\">").SetTextVariable("CLAN_NAME", Clan.PlayerClan.Name).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
			if (!flag2)
			{
				List<TextObject> list = new List<TextObject>();
				Func<Workshop, bool> <>9__2;
				foreach (Settlement settlement in Settlement.All)
				{
					if (settlement.IsTown)
					{
						Town town = settlement.Town;
						List<TextObject> list2 = list;
						IEnumerable<Workshop> workshops = town.Workshops;
						Func<Workshop, bool> func;
						if ((func = <>9__2) == null)
						{
							func = (<>9__2 = (Workshop x) => x.Owner == hero && !x.WorkshopType.IsHidden);
						}
						list2.AddRange(from x in workshops.Where<Workshop>(func)
							select x.WorkshopType.Name);
					}
					if (settlement.IsTown || settlement.IsVillage)
					{
						foreach (Alley alley in settlement.Alleys)
						{
							if (alley.Owner == hero)
							{
								TextObject textObject2 = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null);
								textObject2.SetTextVariable("RANK", alley.Name);
								textObject2.SetTextVariable("NUMBER", Campaign.Current.Models.AlleyModel.GetTroopsOfAIOwnedAlley(alley).TotalManCount);
								list.Add(textObject2);
							}
						}
					}
				}
				MBTextManager.SetTextVariable("PROPERTIES", CampaignUIHelper.GetCommaSeparatedText(new TextObject("{=VZjxs5Dt}Owner of ", null), list), false);
				string text = new TextObject("{=j8uZBakZ}{PROPERTIES}", null).ToString();
				if (list.Count > 0)
				{
					propertyBasedTooltipVM.AddProperty("", text, 0, TooltipProperty.TooltipPropertyFlags.MultiLine);
				}
				TextObject textObject3 = new TextObject("{=C2qpwFq5}Owner of {SETTLEMENTS}", null);
				IEnumerable<TextObject> enumerable = from x in Settlement.FindAll((Settlement x) => x.IsFortification && x.OwnerClan != null && x.OwnerClan.Leader == hero)
					select x.Name;
				MBTextManager.SetTextVariable("SETTLEMENTS", CampaignUIHelper.GetCommaSeparatedText(null, enumerable), false);
				if (enumerable.Count<TextObject>() > 0)
				{
					propertyBasedTooltipVM.AddProperty("", textObject3.ToString(), 0, TooltipProperty.TooltipPropertyFlags.MultiLine);
				}
				if (hero.OwnedCaravans.Count > 0)
				{
					TextObject textObject4 = TextObject.GetEmpty();
					Settlement currentSettlement = hero.CurrentSettlement;
					if (currentSettlement != null && currentSettlement.HasPort)
					{
						textObject4 = new TextObject("{=03G5GPec}Owned Convoys: {CARAVAN_COUNT}", null);
					}
					else
					{
						textObject4 = new TextObject("{=TEkWkzbH}Owned Caravans: {CARAVAN_COUNT}", null);
					}
					textObject4.SetTextVariable("CARAVAN_COUNT", hero.OwnedCaravans.Count);
					propertyBasedTooltipVM.AddProperty("", textObject4.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (hero.GovernorOf != null)
				{
					MBTextManager.SetTextVariable("STR1", new TextObject("{=jQdBl4hf}Governor of ", null), false);
					MBTextManager.SetTextVariable("STR2", hero.GovernorOf.Name, false);
					TextObject textObject5 = GameTexts.FindText("str_STR1_STR2", null);
					propertyBasedTooltipVM.AddProperty("", new Func<string>(textObject5.ToString), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (hero != Hero.MainHero)
				{
					MBTextManager.SetTextVariable("LEFT", GameTexts.FindText("str_tooltip_label_relation", null), false);
					string text2 = GameTexts.FindText("str_LEFT_ONLY", null).ToString();
					propertyBasedTooltipVM.AddProperty(text2, ((int)hero.GetRelationWithPlayer()).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			if (hero.HomeSettlement != null)
			{
				MBTextManager.SetTextVariable("LEFT", GameTexts.FindText("str_tooltip_label_home", null), false);
				string text3 = GameTexts.FindText("str_LEFT_ONLY", null).ToString();
				propertyBasedTooltipVM.AddProperty(text3, hero.HomeSettlement.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (hero.IsNotable)
			{
				MBTextManager.SetTextVariable("LEFT", GameTexts.FindText("str_notable_power", null), false);
				string text4 = GameTexts.FindText("str_LEFT_colon", null).ToString();
				MBTextManager.SetTextVariable("RANK", Campaign.Current.Models.NotablePowerModel.GetPowerRankName(hero).ToString(), false);
				MBTextManager.SetTextVariable("NUMBER", ((int)hero.Power).ToString(), false);
				string text5 = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null).ToString();
				propertyBasedTooltipVM.AddProperty(text4, text5, 0, TooltipProperty.TooltipPropertyFlags.None);
				if (Game.Current.IsDevelopmentMode)
				{
					propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.None);
					ExplainedNumber explainedNumber = Campaign.Current.Models.NotablePowerModel.CalculateDailyPowerChangeForHero(hero, true);
					propertyBasedTooltipVM.AddProperty("[DEV] Daily Power Change", explainedNumber.ResultNumber.ToString("+0.##;-0.##;0"), 0, TooltipProperty.TooltipPropertyFlags.RundownResult);
					propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
					foreach (ValueTuple<string, float> valueTuple in explainedNumber.GetLines())
					{
						string item = valueTuple.Item1;
						float item2 = valueTuple.Item2;
						propertyBasedTooltipVM.AddProperty("[DEV] " + item, item2.ToString("+0.##;-0.##;0"), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
					propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			MBTextManager.SetTextVariable("LEFT", GameTexts.FindText("str_tooltip_label_type", null), false);
			string text6 = GameTexts.FindText("str_LEFT_ONLY", null).ToString();
			propertyBasedTooltipVM.AddProperty(text6, HeroHelper.GetCharacterTypeName(hero).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			if (hero.CurrentSettlement != null && LocationComplex.Current != null && hero.CurrentSettlement == Hero.MainHero.CurrentSettlement && LocationComplex.Current.GetLocationOfCharacter(hero) != null)
			{
				MBTextManager.SetTextVariable("LEFT", GameTexts.FindText("str_tooltip_label_location", null), false);
				string text7 = GameTexts.FindText("str_LEFT_ONLY", null).ToString();
				propertyBasedTooltipVM.AddProperty(text7, LocationComplex.Current.GetLocationOfCharacter(hero).DoorName.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (hero.CurrentSettlement != null && hero.IsNotable && hero.SupporterOf != null)
			{
				MBTextManager.SetTextVariable("LEFT", GameTexts.FindText("str_tooltip_label_supporter_of", null), false);
				string text8 = GameTexts.FindText("str_LEFT_ONLY", null).ToString();
				propertyBasedTooltipVM.AddProperty(text8, hero.SupporterOf.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (flag)
			{
				List<ValueTuple<CampaignUIHelper.IssueQuestFlags, TextObject, TextObject>> questStateOfHero = CampaignUIHelper.GetQuestStateOfHero(hero);
				for (int i = 0; i < questStateOfHero.Count; i++)
				{
					string questExplanationOfHero = CampaignUIHelper.GetQuestExplanationOfHero(questStateOfHero[i].Item1);
					if (!string.IsNullOrEmpty(questExplanationOfHero))
					{
						propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
						propertyBasedTooltipVM.AddProperty(questExplanationOfHero, questStateOfHero[i].Item2.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
			}
			if (!hero.IsAlive)
			{
				propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_dead", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000DA00 File Offset: 0x0000BC00
		public static void RefreshInventoryTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			InventoryLogic inventoryLogic = args[0] as InventoryLogic;
			propertyBasedTooltipVM.Mode = 0;
			List<ValueTuple<ItemRosterElement, int>> soldItems = inventoryLogic.GetSoldItems();
			List<ValueTuple<ItemRosterElement, int>> boughtItems = inventoryLogic.GetBoughtItems();
			TextObject textObject = new TextObject("{=bPFjmYCI}{SHOP_NAME} x {SHOP_DIFFERENCE_COUNT}", null);
			TextObject textObject2 = new TextObject("{=lxwGbRwu}x {SHOP_DIFFERENCE_COUNT}", null);
			TextObject textObject3 = (inventoryLogic.IsTrading ? textObject : textObject2);
			int num = 0;
			int num2 = 40;
			foreach (ValueTuple<ItemRosterElement, int> valueTuple in soldItems)
			{
				if (num == num2)
				{
					break;
				}
				TextObject textObject4 = textObject3;
				string text = "SHOP_NAME";
				ItemRosterElement itemRosterElement = valueTuple.Item1;
				textObject4.SetTextVariable(text, itemRosterElement.EquipmentElement.GetModifiedItemName());
				TextObject textObject5 = textObject3;
				string text2 = "SHOP_DIFFERENCE_COUNT";
				itemRosterElement = valueTuple.Item1;
				textObject5.SetTextVariable(text2, itemRosterElement.Amount);
				if (inventoryLogic.IsTrading)
				{
					string text3 = textObject3.ToString();
					string text4 = "+";
					int item = valueTuple.Item2;
					propertyBasedTooltipVM.AddColoredProperty(text3, text4 + item.ToString(), UIColors.PositiveIndicator, 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				else
				{
					itemRosterElement = valueTuple.Item1;
					propertyBasedTooltipVM.AddColoredProperty(itemRosterElement.EquipmentElement.GetModifiedItemName().ToString(), textObject3.ToString(), UIColors.NegativeIndicator, 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				num++;
			}
			foreach (ValueTuple<ItemRosterElement, int> valueTuple2 in boughtItems)
			{
				if (num == num2)
				{
					break;
				}
				TextObject textObject6 = textObject3;
				string text5 = "SHOP_NAME";
				ItemRosterElement itemRosterElement = valueTuple2.Item1;
				textObject6.SetTextVariable(text5, itemRosterElement.EquipmentElement.GetModifiedItemName());
				TextObject textObject7 = textObject3;
				string text6 = "SHOP_DIFFERENCE_COUNT";
				itemRosterElement = valueTuple2.Item1;
				textObject7.SetTextVariable(text6, itemRosterElement.Amount);
				if (inventoryLogic.IsTrading)
				{
					propertyBasedTooltipVM.AddColoredProperty(textObject3.ToString(), (-valueTuple2.Item2).ToString(), UIColors.NegativeIndicator, 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				else
				{
					itemRosterElement = valueTuple2.Item1;
					propertyBasedTooltipVM.AddColoredProperty(itemRosterElement.EquipmentElement.GetModifiedItemName().ToString(), textObject3.ToString(), UIColors.PositiveIndicator, 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				num++;
			}
			if (num == num2)
			{
				int num3 = soldItems.Count + boughtItems.Count - num;
				if (num3 > 0)
				{
					TextObject textObject8 = new TextObject("{=OpsiBFCu}... and {COUNT} more items.", null);
					textObject8.SetTextVariable("COUNT", num3);
					propertyBasedTooltipVM.AddProperty("", textObject8.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000DC9C File Offset: 0x0000BE9C
		public static void RefreshCraftingPartTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			WeaponDesignElement weaponDesignElement = args[0] as WeaponDesignElement;
			propertyBasedTooltipVM.Mode = 0;
			propertyBasedTooltipVM.AddProperty("", weaponDesignElement.CraftingPiece.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
			TextObject textObject = GameTexts.FindText("str_crafting_piece_type", weaponDesignElement.CraftingPiece.PieceType.ToString());
			propertyBasedTooltipVM.AddProperty("", textObject.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty(new TextObject("{=Oo3fkeab}Difficulty: ", null).ToString(), Campaign.Current.Models.SmithingModel.GetCraftingPartDifficulty(weaponDesignElement.CraftingPiece).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty(new TextObject("{=XUtiwiYP}Length: ", null).ToString(), MathF.Round(weaponDesignElement.CraftingPiece.Length * 100f, 2).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_weight_text", null).ToString(), MathF.Round(weaponDesignElement.CraftingPiece.Weight, 2).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			if (weaponDesignElement.CraftingPiece.PieceType == CraftingPiece.PieceTypes.Blade)
			{
				if (weaponDesignElement.CraftingPiece.BladeData.SwingDamageType != DamageTypes.Invalid)
				{
					DamageTypes swingDamageType = weaponDesignElement.CraftingPiece.BladeData.SwingDamageType;
					MBTextManager.SetTextVariable("SWING_DAMAGE_FACTOR", MathF.Round(weaponDesignElement.CraftingPiece.BladeData.SwingDamageFactor, 2) + " " + GameTexts.FindText("str_damage_types", swingDamageType.ToString()).ToString()[0].ToString(), false);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=nYYUQQm0}Swing Damage Factor ", null).ToString(), new TextObject("{=aTdrjrEh}{SWING_DAMAGE_FACTOR}", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (weaponDesignElement.CraftingPiece.BladeData.ThrustDamageType != DamageTypes.Invalid)
				{
					DamageTypes thrustDamageType = weaponDesignElement.CraftingPiece.BladeData.ThrustDamageType;
					MBTextManager.SetTextVariable("THRUST_DAMAGE_FACTOR", MathF.Round(weaponDesignElement.CraftingPiece.BladeData.ThrustDamageFactor, 2) + " " + GameTexts.FindText("str_damage_types", thrustDamageType.ToString()).ToString()[0].ToString(), false);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=KTKBKmvp}Thrust Damage Factor ", null).ToString(), new TextObject("{=DNq9bdvV}{THRUST_DAMAGE_FACTOR}", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			if (weaponDesignElement.CraftingPiece.ArmorBonus != 0)
			{
				propertyBasedTooltipVM.AddModifierProperty(new TextObject("{=7Xynf4IA}Hand Armor", null).ToString(), weaponDesignElement.CraftingPiece.ArmorBonus, 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (weaponDesignElement.CraftingPiece.SwingDamageBonus != 0)
			{
				propertyBasedTooltipVM.AddModifierProperty(new TextObject("{=QeToaiLt}Swing Damage", null).ToString(), weaponDesignElement.CraftingPiece.SwingDamageBonus, 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (weaponDesignElement.CraftingPiece.SwingSpeedBonus != 0)
			{
				propertyBasedTooltipVM.AddModifierProperty(new TextObject("{=sVZaIPoQ}Swing Speed", null).ToString(), weaponDesignElement.CraftingPiece.SwingSpeedBonus, 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (weaponDesignElement.CraftingPiece.ThrustDamageBonus != 0)
			{
				propertyBasedTooltipVM.AddModifierProperty(new TextObject("{=dO95yR9b}Thrust Damage", null).ToString(), weaponDesignElement.CraftingPiece.ThrustDamageBonus, 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (weaponDesignElement.CraftingPiece.ThrustSpeedBonus != 0)
			{
				propertyBasedTooltipVM.AddModifierProperty(new TextObject("{=4uMWNDoi}Thrust Speed", null).ToString(), weaponDesignElement.CraftingPiece.ThrustSpeedBonus, 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (weaponDesignElement.CraftingPiece.HandlingBonus != 0)
			{
				propertyBasedTooltipVM.AddModifierProperty(new TextObject("{=oibdTnXP}Handling", null).ToString(), weaponDesignElement.CraftingPiece.HandlingBonus, 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (weaponDesignElement.CraftingPiece.AccuracyBonus != 0)
			{
				propertyBasedTooltipVM.AddModifierProperty(new TextObject("{=TAnabTdy}Accuracy", null).ToString(), weaponDesignElement.CraftingPiece.AccuracyBonus, 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty(new TextObject("{=hr4MuPnt}Required Materials", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty("", string.Empty, -1, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
			foreach (ValueTuple<CraftingMaterials, int> valueTuple in weaponDesignElement.CraftingPiece.MaterialsUsed)
			{
				ItemObject craftingMaterialItem = Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem(valueTuple.Item1);
				if (craftingMaterialItem != null)
				{
					string text = craftingMaterialItem.Name.ToString();
					int item = valueTuple.Item2;
					propertyBasedTooltipVM.AddProperty(text, item.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000E13C File Offset: 0x0000C33C
		public static void RefreshCharacterTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			CharacterObject characterObject = args[0] as CharacterObject;
			propertyBasedTooltipVM.Mode = 1;
			propertyBasedTooltipVM.AddProperty("", characterObject.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
			TextObject textObject = GameTexts.FindText("str_party_troop_tier", null);
			textObject.SetTextVariable("TIER_LEVEL", characterObject.Tier);
			propertyBasedTooltipVM.AddProperty("", textObject.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			if (characterObject.UpgradeTargets.Length != 0)
			{
				GameTexts.SetVariable("XP_AMOUNT", characterObject.GetUpgradeXpCost(PartyBase.MainParty, 0));
				propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_required_xp_to_upgrade", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (characterObject.TroopWage > 0)
			{
				GameTexts.SetVariable("LEFT", GameTexts.FindText("str_wage", null));
				GameTexts.SetVariable("STR1", characterObject.TroopWage);
				GameTexts.SetVariable("STR2", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
				GameTexts.SetVariable("RIGHT", GameTexts.FindText("str_STR1_space_STR2", null));
				propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_skills", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
			foreach (SkillObject skillObject in Skills.All)
			{
				if (characterObject.GetSkillValue(skillObject) > 0)
				{
					propertyBasedTooltipVM.AddProperty(skillObject.Name.ToString(), characterObject.GetSkillValue(skillObject).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000E304 File Offset: 0x0000C504
		public static void RefreshItemTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			EquipmentElement? equipmentElement = args[0] as EquipmentElement?;
			ItemObject item = equipmentElement.Value.Item;
			propertyBasedTooltipVM.Mode = 1;
			propertyBasedTooltipVM.AddProperty("", item.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
			propertyBasedTooltipVM.AddProperty(new TextObject("{=zMMqgxb1}Type", null).ToString(), GameTexts.FindText("str_inventory_type_" + (int)item.Type, null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty(" ", " ", 0, TooltipProperty.TooltipPropertyFlags.None);
			if (Game.Current.IsDevelopmentMode)
			{
				if (item.Culture != null)
				{
					propertyBasedTooltipVM.AddProperty("Culture: ", item.Culture.StringId, 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				else
				{
					propertyBasedTooltipVM.AddProperty("Culture: ", "No Culture", 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				propertyBasedTooltipVM.AddProperty("ID: ", item.StringId, 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (item.RelevantSkill != null && item.Difficulty > 0)
			{
				propertyBasedTooltipVM.AddProperty(new TextObject("{=dWYm9GsC}Requires", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty(" ", " ", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
				propertyBasedTooltipVM.AddProperty(item.RelevantSkill.Name.ToString(), item.Difficulty.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty(" ", " ", 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			propertyBasedTooltipVM.AddProperty(new TextObject("{=4Dd2xgPm}Weight", null).ToString(), item.Weight.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			string text = "";
			if (item.IsUniqueItem)
			{
				if (text != string.Empty)
				{
					TextObject textObject = GameTexts.FindText("str_STR1_space_STR2", null);
					textObject.SetTextVariable("STR1", text);
					textObject.SetTextVariable("STR2", GameTexts.FindText("str_inventory_flag_unique", null).ToString());
					text = textObject.ToString();
				}
				else
				{
					text = GameTexts.FindText("str_inventory_flag_unique", null).ToString();
				}
			}
			if (item.ItemFlags.HasAnyFlag(ItemFlags.NotUsableByFemale))
			{
				if (text != string.Empty)
				{
					TextObject textObject2 = GameTexts.FindText("str_STR1_space_STR2", null);
					textObject2.SetTextVariable("STR1", text);
					textObject2.SetTextVariable("STR2", GameTexts.FindText("str_inventory_flag_male_only", null).ToString());
					text = textObject2.ToString();
				}
				else
				{
					text = GameTexts.FindText("str_inventory_flag_male_only", null).ToString();
				}
			}
			if (item.ItemFlags.HasAnyFlag(ItemFlags.NotUsableByMale))
			{
				if (text != string.Empty)
				{
					TextObject textObject3 = GameTexts.FindText("str_STR1_space_STR2", null);
					textObject3.SetTextVariable("STR1", text);
					textObject3.SetTextVariable("STR2", GameTexts.FindText("str_inventory_flag_female_only", null).ToString());
					text = textObject3.ToString();
				}
				else
				{
					text = GameTexts.FindText("str_inventory_flag_female_only", null).ToString();
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				propertyBasedTooltipVM.AddProperty(new TextObject("{=eHVq6yDa}Item Properties", null).ToString(), text, 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (item.HasArmorComponent)
			{
				if (Campaign.Current != null)
				{
					propertyBasedTooltipVM.AddProperty(new TextObject("{=US7UmBbt}Armor Tier", null).ToString(), ((int)(item.Tier + 1)).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (item.ArmorComponent.HeadArmor != 0)
				{
					propertyBasedTooltipVM.AddProperty(new TextObject("{=O3dhjtOS}Head Armor", null).ToString(), equipmentElement.Value.GetModifiedHeadArmor().ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (item.ArmorComponent.BodyArmor != 0)
				{
					if (item.Type == ItemObject.ItemTypeEnum.HorseHarness)
					{
						propertyBasedTooltipVM.AddProperty(new TextObject("{=kftE5nvv}Horse Armor", null).ToString(), equipmentElement.Value.GetModifiedMountBodyArmor().ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
					else
					{
						propertyBasedTooltipVM.AddProperty(new TextObject("{=HkfY3Ds5}Body Armor", null).ToString(), equipmentElement.Value.GetModifiedBodyArmor().ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
				if (item.ArmorComponent.ArmArmor != 0)
				{
					propertyBasedTooltipVM.AddProperty(new TextObject("{=kx7q8ybD}Arm Armor", null).ToString(), equipmentElement.Value.GetModifiedArmArmor().ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (item.ArmorComponent.LegArmor != 0)
				{
					propertyBasedTooltipVM.AddProperty(new TextObject("{=eIws123Z}Leg Armor", null).ToString(), equipmentElement.Value.GetModifiedLegArmor().ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			else if (item.WeaponComponent != null && item.Weapons.Count > 0)
			{
				int num = ((item.Weapons.Count > 1 && propertyBasedTooltipVM.IsExtended) ? 1 : 0);
				WeaponComponentData weaponComponentData = item.Weapons[num];
				propertyBasedTooltipVM.AddProperty(new TextObject("{=sqdzHOPe}Class", null).ToString(), GameTexts.FindText("str_inventory_weapon", weaponComponentData.WeaponClass.ToString()).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				if (Campaign.Current != null)
				{
					propertyBasedTooltipVM.AddProperty(new TextObject("{=hn9TPqhK}Weapon Tier", null).ToString(), ((int)(item.Tier + 1)).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				ItemObject.ItemTypeEnum itemTypeFromWeaponClass = WeaponComponentData.GetItemTypeFromWeaponClass(weaponComponentData.WeaponClass);
				if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.OneHandedWeapon || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.TwoHandedWeapon || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Polearm)
				{
					if (weaponComponentData.SwingDamageType != DamageTypes.Invalid)
					{
						propertyBasedTooltipVM.AddProperty(new TextObject("{=sVZaIPoQ}Swing Speed", null).ToString(), equipmentElement.Value.GetModifiedSwingSpeedForUsage(num).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
						propertyBasedTooltipVM.AddProperty(new TextObject("{=QeToaiLt}Swing Damage", null).ToString(), equipmentElement.Value.GetModifiedSwingDamageForUsage(num).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
					if (weaponComponentData.ThrustDamageType != DamageTypes.Invalid)
					{
						propertyBasedTooltipVM.AddProperty(new TextObject("{=4uMWNDoi}Thrust Speed", null).ToString(), equipmentElement.Value.GetModifiedThrustSpeedForUsage(num).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
						propertyBasedTooltipVM.AddProperty(new TextObject("{=dO95yR9b}Thrust Damage", null).ToString(), equipmentElement.Value.GetModifiedThrustDamageForUsage(num).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
					propertyBasedTooltipVM.AddProperty(new TextObject("{=ZcybPatO}Weapon Length", null).ToString(), weaponComponentData.WeaponLength.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=oibdTnXP}Handling", null).ToString(), weaponComponentData.Handling.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Thrown)
				{
					propertyBasedTooltipVM.AddProperty(new TextObject("{=ZcybPatO}Weapon Length", null).ToString(), weaponComponentData.WeaponLength.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=s31DnnAf}Damage", null).ToString(), ItemHelper.GetMissileDamageText(weaponComponentData, equipmentElement.Value.ItemModifier).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=bAqDnkaT}Missile Speed", null).ToString(), equipmentElement.Value.GetModifiedMissileSpeedForUsage(num).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=TAnabTdy}Accuracy", null).ToString(), weaponComponentData.Accuracy.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=twtbH1zv}Stack Amount", null).ToString(), equipmentElement.Value.GetModifiedStackCountForUsage(num).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Shield)
				{
					propertyBasedTooltipVM.AddProperty(new TextObject("{=6GSXsdeX}Speed", null).ToString(), equipmentElement.Value.GetModifiedSwingSpeedForUsage(num).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=oBbiVeKE}Hit Points", null).ToString(), equipmentElement.Value.GetModifiedMaximumHitPointsForUsage(num).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Bow || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Crossbow || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Sling)
				{
					propertyBasedTooltipVM.AddProperty(new TextObject("{=6GSXsdeX}Speed", null).ToString(), equipmentElement.Value.GetModifiedSwingSpeedForUsage(num).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=s31DnnAf}Damage", null).ToString(), ItemHelper.GetThrustDamageText(weaponComponentData, equipmentElement.Value.ItemModifier).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=TAnabTdy}Accuracy", null).ToString(), weaponComponentData.Accuracy.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=bAqDnkaT}Missile Speed", null).ToString(), equipmentElement.Value.GetModifiedMissileSpeedForUsage(num).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Crossbow)
					{
						propertyBasedTooltipVM.AddProperty(new TextObject("{=cnmRwV4s}Ammo Limit", null).ToString(), weaponComponentData.MaxDataValue.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
				if (weaponComponentData.IsAmmo)
				{
					propertyBasedTooltipVM.AddProperty(new TextObject("{=TAnabTdy}Accuracy", null).ToString(), weaponComponentData.Accuracy.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=s31DnnAf}Damage", null).ToString(), ItemHelper.GetThrustDamageText(weaponComponentData, equipmentElement.Value.ItemModifier).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=twtbH1zv}Stack Amount", null).ToString(), equipmentElement.Value.GetModifiedStackCountForUsage(num).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (item.Weapons.Any<WeaponComponentData>(delegate(WeaponComponentData x)
				{
					string weaponDescriptionId = x.WeaponDescriptionId;
					return weaponDescriptionId != null && weaponDescriptionId.IndexOf("couch", StringComparison.OrdinalIgnoreCase) >= 0;
				}))
				{
					propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_inventory_flag_couchable", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (item.Weapons.Any<WeaponComponentData>(delegate(WeaponComponentData x)
				{
					string weaponDescriptionId2 = x.WeaponDescriptionId;
					return weaponDescriptionId2 != null && weaponDescriptionId2.IndexOf("bracing", StringComparison.OrdinalIgnoreCase) >= 0;
				}))
				{
					propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_inventory_flag_braceable", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			else if (item.HasHorseComponent)
			{
				if (item.HorseComponent.IsMount)
				{
					propertyBasedTooltipVM.AddProperty(new TextObject("{=8BlMRMiR}Horse Tier", null).ToString(), ((int)(item.Tier + 1)).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=Mfbc4rQR}Charge Damage", null).ToString(), equipmentElement.Value.GetModifiedMountCharge(in EquipmentElement.Invalid).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=6GSXsdeX}Speed", null).ToString(), equipmentElement.Value.GetModifiedMountSpeed(in EquipmentElement.Invalid).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=rg7OuWS2}Maneuver", null).ToString(), equipmentElement.Value.GetModifiedMountManeuver(in EquipmentElement.Invalid).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=oBbiVeKE}Hit Points", null).ToString(), equipmentElement.Value.GetModifiedMountHitPoints().ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=ZUgoQ1Ws}Horse Type", null).ToString(), item.ItemCategory.GetName().ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			else if (item.HasFoodComponent)
			{
				if (item.FoodComponent.MoraleBonus > 0)
				{
					propertyBasedTooltipVM.AddProperty(new TextObject("{=myMbtwXi}Morale Bonus", null).ToString(), item.FoodComponent.MoraleBonus.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (item.IsFood)
				{
					propertyBasedTooltipVM.AddProperty(new TextObject("{=qSi4DlT4}Food", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			if (item != null && item.HasBannerComponent)
			{
				bool flag;
				if (item == null)
				{
					flag = null != null;
				}
				else
				{
					BannerComponent bannerComponent = item.BannerComponent;
					flag = ((bannerComponent != null) ? bannerComponent.BannerEffect : null) != null;
				}
				TextObject textObject4;
				if (flag)
				{
					GameTexts.SetVariable("RANK", item.BannerComponent.BannerEffect.Name);
					string text2 = string.Empty;
					if (item.BannerComponent.BannerEffect.IncrementType == EffectIncrementType.AddFactor)
					{
						GameTexts.FindText("str_NUMBER_percent", null).SetTextVariable("NUMBER", ((int)Math.Abs(item.BannerComponent.GetBannerEffectBonus() * 100f)).ToString());
						object obj;
						text2 = obj.ToString();
					}
					else if (item.BannerComponent.BannerEffect.IncrementType == EffectIncrementType.Add)
					{
						text2 = item.BannerComponent.GetBannerEffectBonus().ToString();
					}
					GameTexts.SetVariable("NUMBER", text2);
					textObject4 = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null);
				}
				else
				{
					textObject4 = new TextObject("{=koX9okuG}None", null);
				}
				propertyBasedTooltipVM.AddProperty(new TextObject("{=DbXZjPdf}Banner Effect: ", null).ToString(), textObject4.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000EFAC File Offset: 0x0000D1AC
		public static void RefreshBuildingTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			Building building = args[0] as Building;
			propertyBasedTooltipVM.Mode = 1;
			propertyBasedTooltipVM.AddProperty("", building.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
			if (building.BuildingType.IsDailyProject)
			{
				propertyBasedTooltipVM.AddProperty("", new TextObject("{=bd7oAQq6}Daily", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			else
			{
				propertyBasedTooltipVM.AddProperty(new TextObject("{=IJdjwXvn}Current Level: ", null).ToString(), building.CurrentLevel.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			propertyBasedTooltipVM.AddProperty("", building.Explanation.ToString(), 0, TooltipProperty.TooltipPropertyFlags.MultiLine);
			propertyBasedTooltipVM.AddProperty("", building.GetBonusExplanation().ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000F068 File Offset: 0x0000D268
		public static void RefreshAnchorTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			AnchorPoint anchorPoint = args[0] as AnchorPoint;
			propertyBasedTooltipVM.Mode = 1;
			propertyBasedTooltipVM.AddProperty("", anchorPoint.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000F0A4 File Offset: 0x0000D2A4
		public static void RefreshWorkshopTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			Workshop workshop = args[0] as Workshop;
			propertyBasedTooltipVM.Mode = 1;
			propertyBasedTooltipVM.AddProperty("", workshop.WorkshopType.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
			propertyBasedTooltipVM.AddProperty(new TextObject("{=qRqnrtdX}Owner", null).ToString(), workshop.Owner.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, 0, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty(new TextObject("{=xtt9Oxer}Productions", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
			IEnumerable<ValueTuple<ItemCategory, int>> enumerable = workshop.WorkshopType.Productions.SelectMany<WorkshopType.Production, ValueTuple<ItemCategory, int>>((WorkshopType.Production p) => p.Inputs).Distinct<ValueTuple<ItemCategory, int>>(TooltipRefresherCollection.itemCategoryDistinctComparer);
			IEnumerable<ValueTuple<ItemCategory, int>> enumerable2 = workshop.WorkshopType.Productions.SelectMany<WorkshopType.Production, ValueTuple<ItemCategory, int>>((WorkshopType.Production p) => p.Outputs).Distinct<ValueTuple<ItemCategory, int>>(TooltipRefresherCollection.itemCategoryDistinctComparer);
			if (enumerable.Any<ValueTuple<ItemCategory, int>>())
			{
				propertyBasedTooltipVM.AddProperty(new TextObject("{=omXaC1am}Material", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.DefaultSeperator);
				foreach (ValueTuple<ItemCategory, int> valueTuple in enumerable)
				{
					propertyBasedTooltipVM.AddProperty(" ", valueTuple.Item1.GetName().ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (enumerable2.Any<ValueTuple<ItemCategory, int>>())
			{
				propertyBasedTooltipVM.AddProperty(new TextObject("{=bGyrPe8c}Production", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.DefaultSeperator);
				foreach (ValueTuple<ItemCategory, int> valueTuple2 in enumerable2)
				{
					propertyBasedTooltipVM.AddProperty(" ", valueTuple2.Item1.GetName().ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000F2EC File Offset: 0x0000D4EC
		public static void RefreshEncounterTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			bool flag = (int)args[0] != 0;
			List<MobileParty> list = new List<MobileParty> { MobileParty.MainParty };
			List<MobileParty> list2 = new List<MobileParty> { PlayerEncounter.EncounteredParty.MobileParty };
			PlayerEncounter.Current.FindAllNpcPartiesWhoWillJoinEvent(list, list2);
			List<MobileParty> parties = null;
			if (!flag)
			{
				parties = list;
				propertyBasedTooltipVM.Mode = 2;
			}
			else
			{
				parties = list2;
				propertyBasedTooltipVM.Mode = 3;
			}
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRoster2 = TroopRoster.CreateDummyTroopRoster();
			foreach (MobileParty mobileParty in parties)
			{
				for (int i = 0; i < mobileParty.MemberRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = mobileParty.MemberRoster.GetElementCopyAtIndex(i);
					troopRoster.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, elementCopyAtIndex.WoundedNumber, 0, true, -1);
				}
				for (int j = 0; j < mobileParty.PrisonRoster.Count; j++)
				{
					TroopRosterElement elementCopyAtIndex2 = mobileParty.PrisonRoster.GetElementCopyAtIndex(j);
					troopRoster2.AddToCounts(elementCopyAtIndex2.Character, elementCopyAtIndex2.Number, false, elementCopyAtIndex2.WoundedNumber, 0, true, -1);
				}
			}
			Func<TroopRoster> func = delegate
			{
				TroopRoster troopRoster3 = TroopRoster.CreateDummyTroopRoster();
				foreach (MobileParty mobileParty3 in parties)
				{
					for (int k = 0; k < mobileParty3.MemberRoster.Count; k++)
					{
						TroopRosterElement elementCopyAtIndex3 = mobileParty3.MemberRoster.GetElementCopyAtIndex(k);
						troopRoster3.AddToCounts(elementCopyAtIndex3.Character, elementCopyAtIndex3.Number, false, elementCopyAtIndex3.WoundedNumber, 0, true, -1);
					}
				}
				return troopRoster3;
			};
			Func<TroopRoster> func2 = delegate
			{
				TroopRoster troopRoster4 = TroopRoster.CreateDummyTroopRoster();
				foreach (MobileParty mobileParty4 in parties)
				{
					for (int l = 0; l < mobileParty4.PrisonRoster.Count; l++)
					{
						TroopRosterElement elementCopyAtIndex4 = mobileParty4.PrisonRoster.GetElementCopyAtIndex(l);
						troopRoster4.AddToCounts(elementCopyAtIndex4.Character, elementCopyAtIndex4.Number, false, elementCopyAtIndex4.WoundedNumber, 0, true, -1);
					}
				}
				return troopRoster4;
			};
			bool flag2 = false;
			foreach (MobileParty mobileParty2 in parties)
			{
				flag2 = flag2 || mobileParty2.IsInspected;
				propertyBasedTooltipVM.AddProperty("", mobileParty2.Name.ToString(), 1, TooltipProperty.TooltipPropertyFlags.None);
				string text = mobileParty2.Name.ToString();
				IFaction mapFaction = mobileParty2.MapFaction;
				if (text != ((mapFaction != null) ? mapFaction.Name.ToString() : null))
				{
					string text2 = "";
					IFaction mapFaction2 = mobileParty2.MapFaction;
					propertyBasedTooltipVM.AddProperty(text2, ((mapFaction2 != null) ? mapFaction2.Name.ToString() : null) ?? "", 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			if (troopRoster.Count > 0)
			{
				TooltipRefresherCollection.AddPartyTroopProperties(propertyBasedTooltipVM, troopRoster, GameTexts.FindText("str_map_tooltip_troops", null), flag2, func);
			}
			if (troopRoster2.Count > 0)
			{
				TooltipRefresherCollection.AddPartyTroopProperties(propertyBasedTooltipVM, troopRoster2, GameTexts.FindText("str_map_tooltip_prisoners", null), flag2, func2);
			}
			if (!Campaign.Current.IsMapTooltipLongForm && !propertyBasedTooltipVM.IsExtended)
			{
				propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
				GameTexts.SetVariable("EXTEND_KEY", propertyBasedTooltipVM.GetKeyText(TooltipRefresherCollection.ExtendKeyId));
				propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_map_tooltip_info", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000F5C8 File Offset: 0x0000D7C8
		public static void RefreshSiegeEventTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			SiegeEvent siegeEvent = args[0] as SiegeEvent;
			if (!siegeEvent.ReadyToBeRemoved)
			{
				propertyBasedTooltipVM.Mode = 4;
				TooltipProperty.TooltipPropertyFlags tooltipPropertyFlags;
				if (FactionManager.IsAtWarAgainstFaction(siegeEvent.BesiegerCamp.MapFaction, PartyBase.MainParty.MapFaction))
				{
					tooltipPropertyFlags = TooltipProperty.TooltipPropertyFlags.WarFirstEnemy;
				}
				else if (siegeEvent.BesiegerCamp.MapFaction == PartyBase.MainParty.MapFaction || DiplomacyHelper.IsSameFactionAndNotEliminated(siegeEvent.BesiegerCamp.MapFaction, PartyBase.MainParty.MapFaction))
				{
					tooltipPropertyFlags = TooltipProperty.TooltipPropertyFlags.WarFirstAlly;
				}
				else
				{
					tooltipPropertyFlags = TooltipProperty.TooltipPropertyFlags.WarFirstNeutral;
				}
				TooltipProperty.TooltipPropertyFlags tooltipPropertyFlags2;
				if (FactionManager.IsAtWarAgainstFaction(siegeEvent.BesiegedSettlement.MapFaction, PartyBase.MainParty.MapFaction))
				{
					tooltipPropertyFlags2 = TooltipProperty.TooltipPropertyFlags.WarSecondEnemy;
				}
				else if (siegeEvent.BesiegedSettlement.MapFaction == PartyBase.MainParty.MapFaction || DiplomacyHelper.IsSameFactionAndNotEliminated(siegeEvent.BesiegedSettlement.MapFaction, PartyBase.MainParty.MapFaction))
				{
					tooltipPropertyFlags2 = TooltipProperty.TooltipPropertyFlags.WarSecondAlly;
				}
				else
				{
					tooltipPropertyFlags2 = TooltipProperty.TooltipPropertyFlags.WarSecondNeutral;
				}
				propertyBasedTooltipVM.AddProperty("", "", 1, tooltipPropertyFlags | tooltipPropertyFlags2);
				if (siegeEvent.GetCurrentBattleType() == MapEvent.BattleTypes.Siege)
				{
					TextObject textObject = new TextObject("{=43HYUImy}{SETTLEMENT}'s Siege", null);
					textObject.SetTextVariable("SETTLEMENT", siegeEvent.BesiegedSettlement.Name);
					propertyBasedTooltipVM.AddProperty("", textObject.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
				}
				propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
				MBList<PartyBase> mblist = new MBReadOnlyList<PartyBase>(siegeEvent.BesiegerCamp.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege)).Where<PartyBase>((PartyBase x) => !x.IsSettlement).ToMBList<PartyBase>();
				MBList<PartyBase> mblist2 = new MBReadOnlyList<PartyBase>(siegeEvent.BesiegedSettlement.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege)).Where<PartyBase>((PartyBase x) => !x.IsSettlement).ToMBList<PartyBase>();
				TooltipRefresherCollection.AddEncounterParties(propertyBasedTooltipVM, mblist, mblist2, propertyBasedTooltipVM.IsExtended);
				if (!propertyBasedTooltipVM.IsExtended)
				{
					propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
					GameTexts.SetVariable("EXTEND_KEY", propertyBasedTooltipVM.GetKeyText(TooltipRefresherCollection.ExtendKeyId));
					propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_map_tooltip_info", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000F7F0 File Offset: 0x0000D9F0
		public static void RefreshMapEventTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			MapEvent mapEvent = args[0] as MapEvent;
			propertyBasedTooltipVM.Mode = 4;
			TooltipProperty.TooltipPropertyFlags tooltipPropertyFlags;
			if (FactionManager.IsAtWarAgainstFaction(mapEvent.AttackerSide.LeaderParty.MapFaction, PartyBase.MainParty.MapFaction))
			{
				tooltipPropertyFlags = TooltipProperty.TooltipPropertyFlags.WarFirstEnemy;
			}
			else if (mapEvent.AttackerSide.LeaderParty.MapFaction == PartyBase.MainParty.MapFaction || DiplomacyHelper.IsSameFactionAndNotEliminated(mapEvent.AttackerSide.LeaderParty.MapFaction, PartyBase.MainParty.MapFaction))
			{
				tooltipPropertyFlags = TooltipProperty.TooltipPropertyFlags.WarFirstAlly;
			}
			else
			{
				tooltipPropertyFlags = TooltipProperty.TooltipPropertyFlags.WarFirstNeutral;
			}
			TooltipProperty.TooltipPropertyFlags tooltipPropertyFlags2;
			if (FactionManager.IsAtWarAgainstFaction(mapEvent.DefenderSide.LeaderParty.MapFaction, PartyBase.MainParty.MapFaction))
			{
				tooltipPropertyFlags2 = TooltipProperty.TooltipPropertyFlags.WarSecondEnemy;
			}
			else if (mapEvent.DefenderSide.LeaderParty.MapFaction == PartyBase.MainParty.MapFaction || DiplomacyHelper.IsSameFactionAndNotEliminated(mapEvent.DefenderSide.LeaderParty.MapFaction, PartyBase.MainParty.MapFaction))
			{
				tooltipPropertyFlags2 = TooltipProperty.TooltipPropertyFlags.WarSecondAlly;
			}
			else
			{
				tooltipPropertyFlags2 = TooltipProperty.TooltipPropertyFlags.WarSecondNeutral;
			}
			propertyBasedTooltipVM.AddProperty("", "", 1, tooltipPropertyFlags | tooltipPropertyFlags2);
			if (mapEvent.IsSiegeAssault)
			{
				TextObject textObject = new TextObject("{=43HYUImy}{SETTLEMENT}'s Siege", null);
				textObject.SetTextVariable("SETTLEMENT", mapEvent.MapEventSettlement.Name);
				propertyBasedTooltipVM.AddProperty("", textObject.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
			}
			else if (mapEvent.IsRaid)
			{
				TextObject textObject2 = new TextObject("{=T9bndUYP}{SETTLEMENT}'s Raid", null);
				textObject2.SetTextVariable("SETTLEMENT", mapEvent.MapEventSettlement.Name);
				propertyBasedTooltipVM.AddProperty("", textObject2.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
			}
			else if (mapEvent.IsNavalMapEvent)
			{
				TextObject textObject3 = new TextObject("{=lr2UaD9m}Naval Battle", null);
				propertyBasedTooltipVM.AddProperty("", textObject3.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
			}
			else
			{
				TextObject textObject4 = new TextObject("{=CnsIzaWo}Field Battle", null);
				propertyBasedTooltipVM.AddProperty("", textObject4.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
			}
			propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
			MBList<MapEventParty> mblist = (from x in mapEvent.PartiesOnSide(BattleSideEnum.Attacker)
				where !x.Party.IsSettlement
				select x).ToMBList<MapEventParty>();
			MBList<MapEventParty> mblist2 = (from x in mapEvent.PartiesOnSide(BattleSideEnum.Defender)
				where !x.Party.IsSettlement
				select x).ToMBList<MapEventParty>();
			TooltipRefresherCollection.AddEncounterParties(propertyBasedTooltipVM, mblist, mblist2, propertyBasedTooltipVM.IsExtended);
			if (!propertyBasedTooltipVM.IsExtended)
			{
				propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
				GameTexts.SetVariable("EXTEND_KEY", propertyBasedTooltipVM.GetKeyText(TooltipRefresherCollection.ExtendKeyId));
				propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_map_tooltip_info", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000FAB8 File Offset: 0x0000DCB8
		public static void RefreshSettlementTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			Settlement settlement = args[0] as Settlement;
			PartyBase settlementAsParty = settlement.Party;
			if (settlementAsParty != null)
			{
				if (FactionManager.IsAtWarAgainstFaction(settlementAsParty.MapFaction, PartyBase.MainParty.MapFaction))
				{
					propertyBasedTooltipVM.Mode = 3;
				}
				else if (settlementAsParty.MapFaction == PartyBase.MainParty.MapFaction || DiplomacyHelper.IsSameFactionAndNotEliminated(settlementAsParty.MapFaction, PartyBase.MainParty.MapFaction))
				{
					propertyBasedTooltipVM.Mode = 2;
				}
				else
				{
					propertyBasedTooltipVM.Mode = 1;
				}
				if (Game.Current.IsDevelopmentMode)
				{
					string text = settlement.Name.ToString();
					int num = 1;
					string text2 = "";
					if (settlement.IsHideout)
					{
						text2 = settlement.LocationComplex.GetScene("hideout_center", num);
						propertyBasedTooltipVM.AddProperty("", string.Concat(new string[] { text, "( id: ", settlementAsParty.Id, ")\n(Scene: ", text2, ")" }), 1, TooltipProperty.TooltipPropertyFlags.None);
					}
					else
					{
						if (settlement.IsFortification)
						{
							num = settlement.Town.GetWallLevel();
							text2 = settlement.LocationComplex.GetScene("center", num);
						}
						else if (settlement.IsVillage)
						{
							text2 = settlement.LocationComplex.GetScene("village_center", num);
						}
						propertyBasedTooltipVM.AddProperty("", text + " (" + text2 + ")", 0, TooltipProperty.TooltipPropertyFlags.Title);
					}
					if (settlement.IsFortification)
					{
						propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
						string text3 = "[DEBUG WALL DATA]\n";
						text3 = string.Concat(new object[]
						{
							text3,
							"Current wall level: ",
							settlement.Town.GetWallLevel(),
							"\n"
						});
						text3 = string.Concat(new object[] { text3, "Current wall hp: ", settlement.SettlementTotalWallHitPoints, "\n" });
						text3 = string.Concat(new object[] { text3, "Max wall hp: ", settlement.MaxWallHitPoints, "\n" });
						propertyBasedTooltipVM.AddProperty("", text3, 0, TooltipProperty.TooltipPropertyFlags.Title);
					}
				}
				else
				{
					propertyBasedTooltipVM.AddProperty("", settlement.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
				}
				TextObject textObject;
				bool flag = !CampaignUIHelper.IsSettlementInformationHidden(settlement, out textObject);
				propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_owner", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
				TextObject textObject2 = new TextObject("{=!}{PARTY_OWNERS_FACTION}", null);
				TextObject textObject3 = ((settlement.OwnerClan == null) ? new TextObject("{=3PzgpFGq}Neutral", null) : settlement.OwnerClan.Name);
				textObject2.SetTextVariable("PARTY_OWNERS_FACTION", textObject3);
				propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_clan", null).ToString(), textObject2.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				if (settlementAsParty.MapFaction != null)
				{
					TextObject textObject4 = new TextObject("{=!}{MAP_FACTION}", null);
					TextObject textObject5 = textObject4;
					string text4 = "MAP_FACTION";
					IFaction mapFaction = settlementAsParty.MapFaction;
					textObject5.SetTextVariable(text4, ((mapFaction != null) ? mapFaction.Name : null) ?? new TextObject("{=!}ERROR", null));
					propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_faction", null).ToString(), textObject4.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (settlement.Culture != null && !TextObject.IsNullOrEmpty(settlement.Culture.Name))
				{
					TextObject textObject6 = new TextObject("{=!}{CULTURE}", null);
					textObject6.SetTextVariable("CULTURE", settlement.Culture.Name);
					propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_culture", null).ToString(), textObject6.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (flag)
				{
					if (settlementAsParty.IsSettlement && (settlementAsParty.Settlement.IsVillage || settlementAsParty.Settlement.IsTown || settlementAsParty.Settlement.IsCastle))
					{
						propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
						propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_information", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
						propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
					}
					if (settlement.IsFortification)
					{
						int wallLevel = settlementAsParty.Settlement.Town.GetWallLevel();
						propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_map_tooltip_wall_level", null).ToString(), wallLevel.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
					if (settlement.IsFortification)
					{
						Func<string> func = delegate
						{
							int num5 = (int)settlementAsParty.Settlement.Town.FoodChange;
							int num6 = (int)settlementAsParty.Settlement.Town.FoodStocks;
							TextObject textObject9 = new TextObject("{=Jyfkahka}{VALUE} ({?POSITIVE}+{?}{\\?}{DELTA_VALUE})", null);
							textObject9.SetTextVariable("VALUE", num6);
							textObject9.SetTextVariable("POSITIVE", (num5 > 0) ? 1 : 0);
							textObject9.SetTextVariable("DELTA_VALUE", num5);
							return textObject9.ToString();
						};
						propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_map_tooltip_food_stocks", null).ToString(), func, 0, TooltipProperty.TooltipPropertyFlags.None);
					}
					if (settlement.IsVillage || settlement.IsFortification)
					{
						Func<string> func2 = delegate
						{
							float num7 = float.Parse(string.Format("{0:0.00}", settlementAsParty.Settlement.IsFortification ? settlementAsParty.Settlement.Town.ProsperityChange : settlementAsParty.Settlement.Village.HearthChange));
							int num8 = (int)(settlementAsParty.Settlement.IsFortification ? settlementAsParty.Settlement.Town.Prosperity : settlementAsParty.Settlement.Village.Hearth);
							TextObject textObject10 = new TextObject("{=Jyfkahka}{VALUE} ({?POSITIVE}+{?}{\\?}{DELTA_VALUE})", null);
							textObject10.SetTextVariable("VALUE", num8);
							textObject10.SetTextVariable("POSITIVE", (num7 > 0f) ? 1 : 0);
							textObject10.SetTextVariable("DELTA_VALUE", num7, 2);
							return textObject10.ToString();
						};
						propertyBasedTooltipVM.AddProperty(settlementAsParty.Settlement.IsFortification ? GameTexts.FindText("str_map_tooltip_prosperity", null).ToString() : GameTexts.FindText("str_map_tooltip_hearths", null).ToString(), func2, 0, TooltipProperty.TooltipPropertyFlags.None);
					}
					if (settlement.IsFortification)
					{
						Func<string> func3 = delegate
						{
							TextObject textObject11 = new TextObject("{=Jyfkahka}{VALUE} ({?POSITIVE}+{?}{\\?}{DELTA_VALUE})", null);
							textObject11.SetTextVariable("VALUE", settlement.Town.Loyalty, 2);
							textObject11.SetTextVariable("POSITIVE", (settlement.Town.LoyaltyChange > 0f) ? 1 : 0);
							textObject11.SetTextVariable("DELTA_VALUE", settlement.Town.LoyaltyChange, 2);
							return textObject11.ToString();
						};
						propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_loyalty", null).ToString(), func3, 0, TooltipProperty.TooltipPropertyFlags.None);
						Func<string> func4 = delegate
						{
							TextObject textObject12 = new TextObject("{=Jyfkahka}{VALUE} ({?POSITIVE}+{?}{\\?}{DELTA_VALUE})", null);
							textObject12.SetTextVariable("VALUE", settlement.Town.Security, 2);
							textObject12.SetTextVariable("POSITIVE", (settlement.Town.SecurityChange > 0f) ? 1 : 0);
							textObject12.SetTextVariable("DELTA_VALUE", settlement.Town.SecurityChange, 2);
							return textObject12.ToString();
						};
						propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_security", null).ToString(), func4, 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
				if (settlement.IsVillage)
				{
					string text5 = GameTexts.FindText("str_bound_settlement", null).ToString();
					string text6 = settlementAsParty.Settlement.Village.Bound.Name.ToString();
					propertyBasedTooltipVM.AddProperty(text5, text6, 0, TooltipProperty.TooltipPropertyFlags.None);
					if (settlementAsParty.Settlement.Village.TradeBound != null)
					{
						string text7 = GameTexts.FindText("str_trade_bound_settlement", null).ToString();
						string text8 = settlementAsParty.Settlement.Village.TradeBound.Name.ToString();
						propertyBasedTooltipVM.AddProperty(text7, text8, 0, TooltipProperty.TooltipPropertyFlags.None);
					}
					ItemObject primaryProduction = settlementAsParty.Settlement.Village.VillageType.PrimaryProduction;
					propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_primary_production", null).ToString(), primaryProduction.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (settlement.BoundVillages.Count > 0)
				{
					string text9 = GameTexts.FindText("str_bound_village", null).ToString();
					IEnumerable<TextObject> enumerable = settlementAsParty.Settlement.BoundVillages.Select<Village, TextObject>((Village x) => x.Name);
					propertyBasedTooltipVM.AddProperty(text9, CampaignUIHelper.GetCommaNewlineSeparatedText(TextObject.GetEmpty(), enumerable).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					if (propertyBasedTooltipVM.IsExtended && settlement.IsTown && settlement.Town.TradeBoundVillages.Count > 0)
					{
						string text10 = GameTexts.FindText("str_trade_bound_village", null).ToString();
						IEnumerable<TextObject> enumerable2 = settlement.Town.TradeBoundVillages.Select<Village, TextObject>((Village x) => x.Name);
						propertyBasedTooltipVM.AddProperty(text10, CampaignUIHelper.GetCommaNewlineSeparatedText(TextObject.GetEmpty(), enumerable2).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
				if (Game.Current.IsDevelopmentMode && settlement.IsTown)
				{
					propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty("[DEV] " + GameTexts.FindText("str_shops", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
					int num2 = 1;
					foreach (Workshop workshop in settlementAsParty.Settlement.Town.Workshops)
					{
						if (workshop.WorkshopType != null)
						{
							propertyBasedTooltipVM.AddProperty("[DEV] Shop " + num2.ToString(), workshop.WorkshopType.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
							num2++;
						}
					}
				}
				TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
				TroopRoster troopRoster2 = TroopRoster.CreateDummyTroopRoster();
				TroopRoster.CreateDummyTroopRoster();
				Func<TroopRoster> func5 = delegate
				{
					TroopRoster troopRoster3 = TroopRoster.CreateDummyTroopRoster();
					foreach (MobileParty mobileParty4 in settlement.Parties)
					{
						if (!FactionManager.IsAtWarAgainstFaction(mobileParty4.MapFaction, settlementAsParty.MapFaction) && (mobileParty4.Aggressiveness >= 0.01f || mobileParty4.IsGarrison || mobileParty4.IsMilitia) && !mobileParty4.IsMainParty)
						{
							for (int k = 0; k < mobileParty4.MemberRoster.Count; k++)
							{
								TroopRosterElement elementCopyAtIndex = mobileParty4.MemberRoster.GetElementCopyAtIndex(k);
								troopRoster3.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, elementCopyAtIndex.WoundedNumber, 0, true, -1);
							}
						}
					}
					return troopRoster3;
				};
				Func<TroopRoster> func6 = delegate
				{
					TroopRoster troopRoster4 = TroopRoster.CreateDummyTroopRoster();
					foreach (MobileParty mobileParty5 in settlement.Parties)
					{
						if (!mobileParty5.IsMainParty && !FactionManager.IsAtWarAgainstFaction(mobileParty5.MapFaction, settlementAsParty.MapFaction))
						{
							for (int l = 0; l < mobileParty5.PrisonRoster.Count; l++)
							{
								TroopRosterElement elementCopyAtIndex2 = mobileParty5.PrisonRoster.GetElementCopyAtIndex(l);
								troopRoster4.AddToCounts(elementCopyAtIndex2.Character, elementCopyAtIndex2.Number, false, elementCopyAtIndex2.WoundedNumber, 0, true, -1);
							}
						}
					}
					for (int m = 0; m < settlementAsParty.PrisonRoster.Count; m++)
					{
						TroopRosterElement elementCopyAtIndex3 = settlementAsParty.PrisonRoster.GetElementCopyAtIndex(m);
						troopRoster4.AddToCounts(elementCopyAtIndex3.Character, elementCopyAtIndex3.Number, false, elementCopyAtIndex3.WoundedNumber, 0, true, -1);
					}
					return troopRoster4;
				};
				troopRoster2 = func6();
				if (propertyBasedTooltipVM.IsExtended)
				{
					troopRoster = func5();
					if (troopRoster.Count > 0)
					{
						TooltipRefresherCollection.AddPartyTroopProperties(propertyBasedTooltipVM, troopRoster, GameTexts.FindText("str_map_tooltip_troops", null), flag, func5);
					}
				}
				else
				{
					propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
					if (!settlement.IsHideout && flag)
					{
						List<MobileParty> list = new List<MobileParty>();
						Town town = settlement.Town;
						bool flag2 = town == null || !town.InRebelliousState;
						for (int j = 0; j < settlement.Parties.Count; j++)
						{
							MobileParty mobileParty = settlement.Parties[j];
							bool flag3 = flag2 && mobileParty.IsMilitia;
							if (DiplomacyHelper.IsSameFactionAndNotEliminated(settlementAsParty.MapFaction, mobileParty.MapFaction) && (mobileParty.IsLordParty || flag3 || mobileParty.IsGarrison))
							{
								list.Add(mobileParty);
							}
						}
						list.Sort(CampaignUIHelper.MobilePartyPrecedenceComparerInstance);
						List<MobileParty> list2 = settlement.Parties.Where<MobileParty>((MobileParty p) => !p.IsLordParty && !p.IsMilitia && !p.IsGarrison).ToList<MobileParty>();
						list2.Sort(CampaignUIHelper.MobilePartyPrecedenceComparerInstance);
						if (list.Count > 0)
						{
							int num3 = list.Sum<MobileParty>((MobileParty p) => p.Party.NumberOfHealthyMembers);
							int num4 = list.Sum<MobileParty>((MobileParty p) => p.Party.NumberOfWoundedTotalMembers);
							string text11 = num3 + ((num4 > 0) ? ("+" + num4 + GameTexts.FindText("str_party_nameplate_wounded_abbr", null).ToString()) : "");
							propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_map_tooltip_defenders", null).ToString(), text11, 0, TooltipProperty.TooltipPropertyFlags.None);
							propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
							foreach (MobileParty mobileParty2 in list)
							{
								propertyBasedTooltipVM.AddProperty(mobileParty2.Name.ToString(), CampaignUIHelper.GetPartyNameplateText(mobileParty2, false), 0, TooltipProperty.TooltipPropertyFlags.None);
							}
							propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
						}
						if (list2.Count <= 0)
						{
							goto IL_0C22;
						}
						propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.DefaultSeperator);
						using (List<MobileParty>.Enumerator enumerator = list2.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								MobileParty mobileParty3 = enumerator.Current;
								propertyBasedTooltipVM.AddProperty(mobileParty3.Name.ToString(), CampaignUIHelper.GetPartyNameplateText(mobileParty3, false), 0, TooltipProperty.TooltipPropertyFlags.None);
							}
							goto IL_0C22;
						}
					}
					string text12 = GameTexts.FindText("str_missing_info_indicator", null).ToString();
					propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_map_tooltip_parties", null).ToString(), text12, 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				IL_0C22:
				if (!settlement.IsHideout && troopRoster2.Count > 0 && flag)
				{
					TooltipRefresherCollection.AddPartyTroopProperties(propertyBasedTooltipVM, troopRoster2, GameTexts.FindText("str_map_tooltip_prisoners", null), flag, func6);
				}
				if (settlement.IsFortification && settlement.Town.InRebelliousState)
				{
					propertyBasedTooltipVM.AddProperty(string.Empty, GameTexts.FindText("str_settlement_rebellious_state", null).ToString(), -1, TooltipProperty.TooltipPropertyFlags.None);
				}
				propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
				if (!settlement.IsHideout && !propertyBasedTooltipVM.IsExtended && flag)
				{
					TextObject textObject7 = GameTexts.FindText("str_map_tooltip_info", null);
					textObject7.SetTextVariable("EXTEND_KEY", propertyBasedTooltipVM.GetKeyText(TooltipRefresherCollection.ExtendKeyId));
					propertyBasedTooltipVM.AddProperty(string.Empty, textObject7.ToString(), -1, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (Campaign.Current.Models.EncounterModel.CanMainHeroDoParleyWithParty(settlementAsParty, out textObject))
				{
					TextObject textObject8 = new TextObject("{=uEeLvYXT}Press '{MODIFIER_KEY}' + '{CLICK_KEY}' to parley.", null);
					textObject8.SetTextVariable("MODIFIER_KEY", propertyBasedTooltipVM.GetKeyText(TooltipRefresherCollection.FollowModifierKeyId));
					textObject8.SetTextVariable("CLICK_KEY", propertyBasedTooltipVM.GetKeyText(TooltipRefresherCollection.MapClickKeyId));
					propertyBasedTooltipVM.AddProperty(string.Empty, textObject8.ToString(), -1, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0001084C File Offset: 0x0000EA4C
		public static void RefreshMobilePartyTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			MobileParty mobileParty = args[0] as MobileParty;
			bool flag = (bool)args[1];
			bool flag2 = (bool)args[2];
			if (mobileParty != null)
			{
				if (FactionManager.IsAtWarAgainstFaction(mobileParty.MapFaction, PartyBase.MainParty.MapFaction))
				{
					propertyBasedTooltipVM.Mode = 3;
				}
				else if (mobileParty.MapFaction == PartyBase.MainParty.MapFaction || DiplomacyHelper.IsSameFactionAndNotEliminated(mobileParty.MapFaction, PartyBase.MainParty.MapFaction))
				{
					propertyBasedTooltipVM.Mode = 2;
				}
				else
				{
					propertyBasedTooltipVM.Mode = 1;
				}
				if (Game.Current.IsDevelopmentMode)
				{
					propertyBasedTooltipVM.AddProperty("", string.Concat(new object[] { mobileParty.Name, " (", mobileParty.Id, ")" }), 1, TooltipProperty.TooltipPropertyFlags.Title);
				}
				else
				{
					propertyBasedTooltipVM.AddProperty("", mobileParty.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
				}
				bool isInspected = mobileParty.IsInspected;
				if (mobileParty.IsDisorganized)
				{
					TextObject hoursAndDaysTextFromHourValue = CampaignUIHelper.GetHoursAndDaysTextFromHourValue(MathF.Ceiling(mobileParty.DisorganizedUntilTime.RemainingHoursFromNow));
					TextObject textObject = new TextObject("{=BbLTwhsA}Disorganized for {REMAINING_TIME}", null);
					textObject.SetTextVariable("REMAINING_TIME", hoursAndDaysTextFromHourValue.ToString());
					propertyBasedTooltipVM.AddProperty("", textObject.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (isInspected)
				{
					propertyBasedTooltipVM.AddProperty("", CampaignUIHelper.GetMobilePartyBehaviorText(mobileParty), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (propertyBasedTooltipVM.IsExtended)
				{
					Clan actualClan = mobileParty.ActualClan;
					if (actualClan != null && actualClan.HasBloodFeudWithPlayer)
					{
						if (isInspected)
						{
							propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
						}
						propertyBasedTooltipVM.AddProperty("", new TextObject("{=AZaCxlzZ}{BLOOD_ICON} In a blood feud with the {CLAN_NAME}", null).SetTextVariable("BLOOD_ICON", "{=!}<img src=\"SPGeneral\\blood_feud_icon\" extend=\"3\">").SetTextVariable("CLAN_NAME", Clan.PlayerClan.Name).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
				propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_owner", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
				if (mobileParty.LeaderHero != null && mobileParty.LeaderHero.Clan != null && mobileParty.LeaderHero.Clan != mobileParty.MapFaction)
				{
					TextObject textObject2 = new TextObject("{=oUhd9YhP}{PARTY_LEADERS_FACTION}", null);
					textObject2.SetTextVariable("PARTY_LEADERS_FACTION", mobileParty.LeaderHero.Clan.Name);
					propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_clan", null).ToString(), textObject2.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (mobileParty.MapFaction != null)
				{
					TextObject textObject3 = new TextObject("{=!}{MAP_FACTION}", null);
					TextObject textObject4 = textObject3;
					string text = "MAP_FACTION";
					IFaction mapFaction = mobileParty.MapFaction;
					textObject4.SetTextVariable(text, ((mapFaction != null) ? mapFaction.Name : null) ?? new TextObject("{=!}ERROR", null));
					propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_faction", null).ToString(), textObject3.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (isInspected)
				{
					propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_information", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
					propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_map_tooltip_speed", null).ToString(), CampaignUIHelper.FloatToString(mobileParty.Speed), 0, TooltipProperty.TooltipPropertyFlags.None);
					if (propertyBasedTooltipVM.IsExtended)
					{
						TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(mobileParty.CurrentNavigationFace);
						propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_terrain", null).ToString(), GameTexts.FindText("str_terrain_types", faceTerrainType.ToString()).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
				TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
				TroopRoster troopRoster2 = TroopRoster.CreateDummyTroopRoster();
				TroopRoster.CreateDummyTroopRoster();
				Func<TroopRoster> func = delegate
				{
					TroopRoster troopRoster3 = TroopRoster.CreateDummyTroopRoster();
					for (int i = 0; i < mobileParty.MemberRoster.Count; i++)
					{
						TroopRosterElement elementCopyAtIndex = mobileParty.MemberRoster.GetElementCopyAtIndex(i);
						troopRoster3.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, elementCopyAtIndex.WoundedNumber, 0, true, -1);
					}
					return troopRoster3;
				};
				Func<TroopRoster> func2 = delegate
				{
					TroopRoster troopRoster4 = TroopRoster.CreateDummyTroopRoster();
					for (int j = 0; j < mobileParty.PrisonRoster.Count; j++)
					{
						TroopRosterElement elementCopyAtIndex2 = mobileParty.PrisonRoster.GetElementCopyAtIndex(j);
						troopRoster4.AddToCounts(elementCopyAtIndex2.Character, elementCopyAtIndex2.Number, false, elementCopyAtIndex2.WoundedNumber, 0, true, -1);
					}
					return troopRoster4;
				};
				troopRoster = func();
				troopRoster2 = func2();
				if (troopRoster.Count > 0 && !mobileParty.IsInfoHidden)
				{
					TooltipRefresherCollection.AddPartyTroopProperties(propertyBasedTooltipVM, troopRoster, GameTexts.FindText("str_map_tooltip_troops", null), flag || isInspected || !flag2, func);
				}
				if (troopRoster2.Count > 0 && !mobileParty.IsInfoHidden && (isInspected || flag))
				{
					TooltipRefresherCollection.AddPartyTroopProperties(propertyBasedTooltipVM, troopRoster2, GameTexts.FindText("str_map_tooltip_prisoners", null), isInspected || !flag2, func2);
				}
				if (mobileParty.Ships.Count > 0 && !mobileParty.IsInfoHidden)
				{
					TooltipRefresherCollection.AddPartyShipProperties(propertyBasedTooltipVM, new MBList<MobileParty> { mobileParty }, flag, flag2);
				}
				if (!propertyBasedTooltipVM.IsExtended)
				{
					propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
					GameTexts.SetVariable("EXTEND_KEY", propertyBasedTooltipVM.GetKeyText(TooltipRefresherCollection.ExtendKeyId));
					propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_map_tooltip_info", null).ToString(), -1, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00010DC0 File Offset: 0x0000EFC0
		public static void RefreshArmyTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			TooltipRefresherCollection.<>c__DisplayClass19_0 CS$<>8__locals1 = new TooltipRefresherCollection.<>c__DisplayClass19_0();
			CS$<>8__locals1.army = args[0] as Army;
			bool flag = (bool)args[1];
			bool flag2 = (bool)args[2];
			MobileParty leaderParty = CS$<>8__locals1.army.LeaderParty;
			if (leaderParty != null)
			{
				if (FactionManager.IsAtWarAgainstFaction(leaderParty.MapFaction, PartyBase.MainParty.MapFaction))
				{
					propertyBasedTooltipVM.Mode = 3;
				}
				else if (CS$<>8__locals1.army.Kingdom == PartyBase.MainParty.MapFaction || DiplomacyHelper.IsSameFactionAndNotEliminated(leaderParty.MapFaction, PartyBase.MainParty.MapFaction))
				{
					propertyBasedTooltipVM.Mode = 2;
				}
				else
				{
					propertyBasedTooltipVM.Mode = 1;
				}
				if (Game.Current.IsDevelopmentMode)
				{
					propertyBasedTooltipVM.AddProperty("", string.Concat(new object[]
					{
						CS$<>8__locals1.army.Name,
						" (",
						CS$<>8__locals1.army.LeaderParty.Id,
						")"
					}), 1, TooltipProperty.TooltipPropertyFlags.Title);
				}
				else
				{
					propertyBasedTooltipVM.AddProperty("", CS$<>8__locals1.army.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
				}
				if (leaderParty.IsInspected || !flag2)
				{
					propertyBasedTooltipVM.AddProperty("", CampaignUIHelper.GetMobilePartyBehaviorText(leaderParty), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (propertyBasedTooltipVM.IsExtended)
				{
					if (CS$<>8__locals1.army.Parties.Any<MobileParty>(delegate(MobileParty x)
					{
						Clan actualClan = x.ActualClan;
						return actualClan != null && actualClan.HasBloodFeudWithPlayer;
					}))
					{
						if (leaderParty.IsInspected || !flag2)
						{
							propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
						}
						propertyBasedTooltipVM.AddProperty("", new TextObject("{=a5EFOx2U}{BLOOD_ICON} Some members are in a blood feud with the {CLAN_NAME}", null).SetTextVariable("BLOOD_ICON", "{=!}<img src=\"SPGeneral\\blood_feud_icon\" extend=\"3\">").SetTextVariable("CLAN_NAME", Clan.PlayerClan.Name).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
				propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_owner", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
				if (CS$<>8__locals1.army.Kingdom != null)
				{
					TextObject textObject = new TextObject("{=!}{MAP_FACTION}", null);
					TextObject textObject2 = textObject;
					string text = "MAP_FACTION";
					Kingdom kingdom = CS$<>8__locals1.army.Kingdom;
					textObject2.SetTextVariable(text, ((kingdom != null) ? kingdom.Name : null) ?? new TextObject("{=!}ERROR", null));
					propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_faction", null).ToString(), textObject.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (leaderParty.IsInspected || !flag2)
				{
					propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_information", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
					propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_map_tooltip_speed", null).ToString(), CampaignUIHelper.FloatToString(leaderParty.Speed), 0, TooltipProperty.TooltipPropertyFlags.None);
					if (propertyBasedTooltipVM.IsExtended)
					{
						TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(leaderParty.CurrentNavigationFace);
						propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_terrain", null).ToString(), GameTexts.FindText("str_terrain_types", faceTerrainType.ToString()).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
				TroopRoster troopRoster = CS$<>8__locals1.<RefreshArmyTooltip>g__GetTempRoster|1();
				TroopRoster troopRoster2 = CS$<>8__locals1.<RefreshArmyTooltip>g__GetTempPrisonerRoster|2();
				if (troopRoster.Count > 0)
				{
					TooltipRefresherCollection.AddPartyTroopProperties(propertyBasedTooltipVM, troopRoster, GameTexts.FindText("str_map_tooltip_troops", null), flag || leaderParty.IsInspected, new Func<TroopRoster>(CS$<>8__locals1.<RefreshArmyTooltip>g__GetTempRoster|1));
				}
				if (troopRoster2.Count > 0 && (leaderParty.IsInspected || flag || !flag2))
				{
					TooltipRefresherCollection.AddPartyTroopProperties(propertyBasedTooltipVM, troopRoster2, GameTexts.FindText("str_map_tooltip_prisoners", null), leaderParty.IsInspected || !flag2, new Func<TroopRoster>(CS$<>8__locals1.<RefreshArmyTooltip>g__GetTempPrisonerRoster|2));
				}
				MBList<MobileParty> mblist = new MBList<MobileParty>();
				mblist.Add(leaderParty);
				mblist.AddRange(leaderParty.AttachedParties);
				if (mblist.Any<MobileParty>((MobileParty x) => x.Ships.Count > 0))
				{
					TooltipRefresherCollection.AddPartyShipProperties(propertyBasedTooltipVM, mblist, flag, flag2);
				}
				if (!propertyBasedTooltipVM.IsExtended)
				{
					propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
					GameTexts.SetVariable("EXTEND_KEY", propertyBasedTooltipVM.GetKeyText(TooltipRefresherCollection.ExtendKeyId));
					propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_map_tooltip_info", null).ToString(), -1, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0001123C File Offset: 0x0000F43C
		public static void RefreshClanTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			Clan clan = args[0] as Clan;
			propertyBasedTooltipVM.AddProperty("", clan.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
			if (FactionManager.IsAtWarAgainstFaction(clan.MapFaction, Hero.MainHero.MapFaction))
			{
				propertyBasedTooltipVM.Mode = 3;
				propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_kingdom_at_war", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
			}
			else if (DiplomacyHelper.IsSameFactionAndNotEliminated(clan.MapFaction, Hero.MainHero.MapFaction))
			{
				propertyBasedTooltipVM.Mode = 2;
			}
			else
			{
				propertyBasedTooltipVM.Mode = 1;
			}
			if (clan.IsEliminated)
			{
				propertyBasedTooltipVM.AddProperty("", new TextObject("{=SlubkZ1A}Eliminated", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				return;
			}
			propertyBasedTooltipVM.AddProperty(new TextObject("{=SrfYbg3x}Leader", null).ToString(), clan.Leader.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
			propertyBasedTooltipVM.AddProperty(new TextObject("{=tTLvo8sM}Clan Tier", null).ToString(), clan.Tier.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty(new TextObject("{=ODEnkg0o}Clan Strength", null).ToString(), MathF.Round(clan.CurrentTotalStrength).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_wealth", null).ToString(), CampaignUIHelper.GetClanWealthStatusText(clan), 0, TooltipProperty.TooltipPropertyFlags.None);
			int count = clan.Fiefs.Count;
			List<TextObject> list = new List<TextObject>();
			foreach (IFaction faction in Campaign.Current.Factions.OrderBy<IFaction, bool>((IFaction x) => !x.IsKingdomFaction).ThenBy<IFaction, string>((IFaction f) => f.Name.ToString()))
			{
				if (FactionManager.IsAtWarAgainstFaction(clan.MapFaction, faction.MapFaction) && !faction.MapFaction.IsBanditFaction && !list.Contains(faction.MapFaction.Name))
				{
					list.Add(faction.MapFaction.Name);
				}
			}
			if (propertyBasedTooltipVM.IsExtended)
			{
				if (count > 0)
				{
					propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
					IEnumerable<TextObject> enumerable = from s in clan.Fiefs
						orderby s.IsCastle, s.IsTown
						select s into x
						select x.Name;
					propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_settlements", null).ToString(), CampaignUIHelper.GetCommaNewlineSeparatedText(TextObject.GetEmpty(), enumerable).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (list.Count > 0)
				{
					propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=zZlWRZjO}Wars", null).ToString(), CampaignUIHelper.GetCommaNewlineSeparatedText(TextObject.GetEmpty(), list).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			if (!propertyBasedTooltipVM.IsExtended && (count > 0 || list.Count > 0))
			{
				propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
				GameTexts.SetVariable("EXTEND_KEY", propertyBasedTooltipVM.GetKeyText(TooltipRefresherCollection.ExtendKeyId));
				propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_map_tooltip_info", null).ToString(), -1, TooltipProperty.TooltipPropertyFlags.None);
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000115FC File Offset: 0x0000F7FC
		public static void RefreshKingdomTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			Kingdom kingdom = args[0] as Kingdom;
			propertyBasedTooltipVM.AddProperty("", kingdom.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
			if (FactionManager.IsAtWarAgainstFaction(kingdom.MapFaction, Hero.MainHero.MapFaction))
			{
				propertyBasedTooltipVM.Mode = 3;
				propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_kingdom_at_war", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
			}
			else if (DiplomacyHelper.IsSameFactionAndNotEliminated(kingdom.MapFaction, Hero.MainHero.MapFaction))
			{
				propertyBasedTooltipVM.Mode = 2;
			}
			else
			{
				propertyBasedTooltipVM.Mode = 1;
			}
			if (kingdom.IsEliminated)
			{
				propertyBasedTooltipVM.AddProperty("", new TextObject("{=SlubkZ1A}Eliminated", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				return;
			}
			propertyBasedTooltipVM.AddProperty(new TextObject("{=SrfYbg3x}Leader", null).ToString(), kingdom.Leader.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			int count = kingdom.Clans.Count;
			List<TextObject> list = new List<TextObject>();
			foreach (IFaction faction in Campaign.Current.Factions.OrderBy<IFaction, bool>((IFaction x) => !x.IsKingdomFaction).ThenBy<IFaction, string>((IFaction f) => f.Name.ToString()))
			{
				if (FactionManager.IsAtWarAgainstFaction(kingdom.MapFaction, faction.MapFaction) && !faction.MapFaction.IsBanditFaction && !list.Contains(faction.MapFaction.Name))
				{
					list.Add(faction.MapFaction.Name);
				}
			}
			if (propertyBasedTooltipVM.IsExtended)
			{
				if (count > 0)
				{
					propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
					IEnumerable<TextObject> enumerable = kingdom.Clans.Select<Clan, TextObject>((Clan x) => x.Name);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=bfQLwMUp}Clans", null).ToString(), CampaignUIHelper.GetCommaNewlineSeparatedText(TextObject.GetEmpty(), enumerable).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (list.Count > 0)
				{
					propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(new TextObject("{=zZlWRZjO}Wars", null).ToString(), CampaignUIHelper.GetCommaNewlineSeparatedText(TextObject.GetEmpty(), list).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			if (!propertyBasedTooltipVM.IsExtended && (count > 0 || list.Count > 0))
			{
				propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
				GameTexts.SetVariable("EXTEND_KEY", propertyBasedTooltipVM.GetKeyText(TooltipRefresherCollection.ExtendKeyId));
				propertyBasedTooltipVM.AddProperty("", GameTexts.FindText("str_map_tooltip_info", null).ToString(), -1, TooltipProperty.TooltipPropertyFlags.None);
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000118E4 File Offset: 0x0000FAE4
		private static void AddEncounterParties(PropertyBasedTooltipVM propertyBasedTooltipVM, MBReadOnlyList<PartyBase> parties1, MBReadOnlyList<PartyBase> parties2, bool isExtended)
		{
			propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.BattleMode);
			int num = 0;
			while (num < parties1.Count || num < parties2.Count)
			{
				MBTextManager.SetTextVariable("PARTY_1S_MEMBERS", "", false);
				MBTextManager.SetTextVariable("PARTY_2S_MEMBERS", "", false);
				if (num < parties1.Count)
				{
					MBTextManager.SetTextVariable("PARTY_1S_MEMBERS", parties1[num].Name, false);
				}
				if (num < parties2.Count)
				{
					MBTextManager.SetTextVariable("PARTY_2S_MEMBERS", parties2[num].Name, false);
				}
				propertyBasedTooltipVM.AddProperty(new TextObject("{=CExQ40Ux}{PARTY_1S_MEMBERS}   ", null).ToString(), new TextObject("{=OTaPfaJl}{PARTY_2S_MEMBERS}   ", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				num++;
			}
			if (parties1.Count > 0 && parties2.Count > 0)
			{
				PartyBase partyBase = parties1[0];
				if (((partyBase != null) ? partyBase.MapFaction : null) != null)
				{
					PartyBase partyBase2 = parties2[0];
					if (((partyBase2 != null) ? partyBase2.MapFaction : null) != null)
					{
						propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.DefaultSeperator);
						MBTextManager.SetTextVariable("PARTY_1S_MEMBERS", parties1[0].MapFaction.Name, false);
						MBTextManager.SetTextVariable("PARTY_2S_MEMBERS", parties2[0].MapFaction.Name, false);
						propertyBasedTooltipVM.AddProperty(new TextObject("{=CExQ40Ux}{PARTY_1S_MEMBERS}   ", null).ToString(), new TextObject("{=OTaPfaJl}{PARTY_2S_MEMBERS}   ", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
			}
			int lastHeroIndex = 0;
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (PartyBase partyBase3 in parties1)
			{
				for (int i = 0; i < partyBase3.MemberRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = partyBase3.MemberRoster.GetElementCopyAtIndex(i);
					if (elementCopyAtIndex.Character.IsHero)
					{
						troopRoster.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, elementCopyAtIndex.WoundedNumber, 0, true, lastHeroIndex);
						int num2 = lastHeroIndex;
						lastHeroIndex = num2 + 1;
					}
					else
					{
						troopRoster.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, elementCopyAtIndex.WoundedNumber, 0, true, -1);
					}
				}
			}
			lastHeroIndex = 0;
			TroopRoster troopRoster2 = TroopRoster.CreateDummyTroopRoster();
			foreach (PartyBase partyBase4 in parties2)
			{
				for (int j = 0; j < partyBase4.MemberRoster.Count; j++)
				{
					TroopRosterElement elementCopyAtIndex2 = partyBase4.MemberRoster.GetElementCopyAtIndex(j);
					if (elementCopyAtIndex2.Character.IsHero)
					{
						troopRoster2.AddToCounts(elementCopyAtIndex2.Character, elementCopyAtIndex2.Number, false, elementCopyAtIndex2.WoundedNumber, 0, true, lastHeroIndex);
						int num2 = lastHeroIndex;
						lastHeroIndex = num2 + 1;
					}
					else
					{
						troopRoster2.AddToCounts(elementCopyAtIndex2.Character, elementCopyAtIndex2.Number, false, elementCopyAtIndex2.WoundedNumber, 0, true, -1);
					}
				}
			}
			Func<string> func = () => "";
			Func<string> func2 = () => "";
			if (troopRoster.Count > 0)
			{
				func = delegate
				{
					TroopRoster troopRoster3 = TroopRoster.CreateDummyTroopRoster();
					lastHeroIndex = 0;
					foreach (PartyBase partyBase5 in parties1)
					{
						for (int k = 0; k < partyBase5.MemberRoster.Count; k++)
						{
							TroopRosterElement elementCopyAtIndex3 = partyBase5.MemberRoster.GetElementCopyAtIndex(k);
							if (elementCopyAtIndex3.Character.IsHero)
							{
								troopRoster3.AddToCounts(elementCopyAtIndex3.Character, elementCopyAtIndex3.Number, false, elementCopyAtIndex3.WoundedNumber, 0, true, lastHeroIndex);
								int lastHeroIndex3 = lastHeroIndex;
								lastHeroIndex = lastHeroIndex3 + 1;
							}
							else
							{
								troopRoster3.AddToCounts(elementCopyAtIndex3.Character, elementCopyAtIndex3.Number, false, elementCopyAtIndex3.WoundedNumber, 0, true, -1);
							}
						}
					}
					TextObject textObject = new TextObject("{=QlbkxoSp} {TOOLTIP_TROOPS} ({PARTY_SIZE})", null);
					textObject.SetTextVariable("TOOLTIP_TROOPS", GameTexts.FindText("str_map_tooltip_troops", null));
					textObject.SetTextVariable("PARTY_SIZE", PartyBaseHelper.GetPartySizeText(troopRoster3.TotalManCount - troopRoster3.TotalWounded, troopRoster3.TotalWounded, true));
					return textObject.ToString();
				};
			}
			if (troopRoster2.Count > 0)
			{
				func2 = delegate
				{
					TroopRoster troopRoster4 = TroopRoster.CreateDummyTroopRoster();
					lastHeroIndex = 0;
					foreach (PartyBase partyBase6 in parties2)
					{
						for (int l = 0; l < partyBase6.MemberRoster.Count; l++)
						{
							TroopRosterElement elementCopyAtIndex4 = partyBase6.MemberRoster.GetElementCopyAtIndex(l);
							if (elementCopyAtIndex4.Character.IsHero)
							{
								troopRoster4.AddToCounts(elementCopyAtIndex4.Character, elementCopyAtIndex4.Number, false, elementCopyAtIndex4.WoundedNumber, 0, true, lastHeroIndex);
								int lastHeroIndex2 = lastHeroIndex;
								lastHeroIndex = lastHeroIndex2 + 1;
							}
							else
							{
								troopRoster4.AddToCounts(elementCopyAtIndex4.Character, elementCopyAtIndex4.Number, false, elementCopyAtIndex4.WoundedNumber, 0, true, -1);
							}
						}
					}
					TextObject textObject2 = new TextObject("{=QlbkxoSp} {TOOLTIP_TROOPS} ({PARTY_SIZE})", null);
					textObject2.SetTextVariable("TOOLTIP_TROOPS", GameTexts.FindText("str_map_tooltip_troops", null));
					textObject2.SetTextVariable("PARTY_SIZE", PartyBaseHelper.GetPartySizeText(troopRoster4.TotalManCount - troopRoster4.TotalWounded, troopRoster4.TotalWounded, true));
					return textObject2.ToString();
				};
			}
			if (func().Length != 0 && func2().Length != 0)
			{
				propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty(func, func2, 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (isExtended)
			{
				propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.DefaultSeperator);
				int num3 = 0;
				while (num3 < troopRoster.Count || num3 < troopRoster2.Count)
				{
					string blankString = new TextObject("{=!} ", null).ToString();
					Func<string> func3 = () => blankString;
					Func<string> func4 = () => blankString;
					if (num3 < troopRoster.Count)
					{
						CharacterObject character2 = troopRoster.GetElementCopyAtIndex(num3).Character;
						func3 = delegate
						{
							lastHeroIndex = 0;
							foreach (PartyBase partyBase7 in parties1)
							{
								for (int m = 0; m < partyBase7.MemberRoster.Count; m++)
								{
									TroopRosterElement elementCopyAtIndex5 = partyBase7.MemberRoster.GetElementCopyAtIndex(m);
									if (elementCopyAtIndex5.Character == character2)
									{
										TextObject textObject3;
										if (elementCopyAtIndex5.Character.IsHero)
										{
											textObject3 = new TextObject("{=W1tsTWZv} {PARTY_MEMBER.LINK} ({MEMBER_HEALTH}%)", null);
											textObject3.SetTextVariable("MEMBER_HEALTH", elementCopyAtIndex5.Character.HeroObject.HitPoints * 100 / elementCopyAtIndex5.Character.MaxHitPoints());
										}
										else
										{
											textObject3 = new TextObject("{=vLaBJFGy} {PARTY_MEMBER.LINK} ({PARTY_SIZE})", null);
											textObject3.SetTextVariable("PARTY_SIZE", PartyBaseHelper.GetPartySizeText(elementCopyAtIndex5.Number - elementCopyAtIndex5.WoundedNumber, elementCopyAtIndex5.WoundedNumber, true));
										}
										StringHelpers.SetCharacterProperties("PARTY_MEMBER", elementCopyAtIndex5.Character, textObject3, false);
										return textObject3.ToString();
									}
								}
							}
							return blankString;
						};
					}
					if (num3 < troopRoster2.Count)
					{
						CharacterObject character = troopRoster2.GetElementCopyAtIndex(num3).Character;
						func4 = delegate
						{
							lastHeroIndex = 0;
							foreach (PartyBase partyBase8 in parties2)
							{
								for (int n = 0; n < partyBase8.MemberRoster.Count; n++)
								{
									TroopRosterElement elementCopyAtIndex6 = partyBase8.MemberRoster.GetElementCopyAtIndex(n);
									if (character == elementCopyAtIndex6.Character)
									{
										TextObject textObject4;
										if (character.IsHero)
										{
											textObject4 = new TextObject("{=PS02CqPu} {PARTY_MEMBER.LINK} (Health: {MEMBER_HEALTH}%)", null);
											textObject4.SetTextVariable("MEMBER_HEALTH", elementCopyAtIndex6.Character.HeroObject.HitPoints * 100 / elementCopyAtIndex6.Character.MaxHitPoints());
										}
										else
										{
											textObject4 = new TextObject("{=vLaBJFGy} {PARTY_MEMBER.LINK} ({PARTY_SIZE})", null);
											textObject4.SetTextVariable("PARTY_SIZE", PartyBaseHelper.GetPartySizeText(elementCopyAtIndex6.Number - elementCopyAtIndex6.WoundedNumber, elementCopyAtIndex6.WoundedNumber, true));
										}
										StringHelpers.SetCharacterProperties("PARTY_MEMBER", elementCopyAtIndex6.Character, textObject4, false);
										return textObject4.ToString();
									}
								}
							}
							return blankString;
						};
					}
					propertyBasedTooltipVM.AddProperty(func3, func4, 0, TooltipProperty.TooltipPropertyFlags.None);
					num3++;
				}
			}
			propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.BattleModeOver);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00011E50 File Offset: 0x00010050
		private static void AddEncounterParties(PropertyBasedTooltipVM propertyBasedTooltipVM, MBReadOnlyList<MapEventParty> parties1, MBReadOnlyList<MapEventParty> parties2, bool isExtended)
		{
			propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.BattleMode);
			int num = 0;
			while (num < parties1.Count || num < parties2.Count)
			{
				MBTextManager.SetTextVariable("PARTY_1S_MEMBERS", "", false);
				MBTextManager.SetTextVariable("PARTY_2S_MEMBERS", "", false);
				if (num < parties1.Count)
				{
					MBTextManager.SetTextVariable("PARTY_1S_MEMBERS", parties1[num].Party.Name, false);
				}
				if (num < parties2.Count)
				{
					MBTextManager.SetTextVariable("PARTY_2S_MEMBERS", parties2[num].Party.Name, false);
				}
				propertyBasedTooltipVM.AddProperty(new TextObject("{=CExQ40Ux}{PARTY_1S_MEMBERS}   ", null).ToString(), new TextObject("{=OTaPfaJl}{PARTY_2S_MEMBERS}   ", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				num++;
			}
			if (parties1.Count > 0 && parties2.Count > 0)
			{
				MapEventParty mapEventParty = parties1[0];
				bool flag;
				if (mapEventParty == null)
				{
					flag = null != null;
				}
				else
				{
					PartyBase party = mapEventParty.Party;
					flag = ((party != null) ? party.MapFaction : null) != null;
				}
				if (flag)
				{
					MapEventParty mapEventParty2 = parties2[0];
					bool flag2;
					if (mapEventParty2 == null)
					{
						flag2 = null != null;
					}
					else
					{
						PartyBase party2 = mapEventParty2.Party;
						flag2 = ((party2 != null) ? party2.MapFaction : null) != null;
					}
					if (flag2)
					{
						propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.DefaultSeperator);
						MBTextManager.SetTextVariable("PARTY_1S_MEMBERS", parties1[0].Party.MapFaction.Name, false);
						MBTextManager.SetTextVariable("PARTY_2S_MEMBERS", parties2[0].Party.MapFaction.Name, false);
						propertyBasedTooltipVM.AddProperty(new TextObject("{=CExQ40Ux}{PARTY_1S_MEMBERS}   ", null).ToString(), new TextObject("{=OTaPfaJl}{PARTY_2S_MEMBERS}   ", null).ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
			}
			bool isInfoHiddenLeft = false;
			bool isInfoHiddenRight = false;
			int lastHeroIndex = 0;
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (MapEventParty mapEventParty3 in parties1)
			{
				MobileParty mobileParty = mapEventParty3.Party.MobileParty;
				if (mobileParty == null || !mobileParty.IsInfoHidden)
				{
					for (int i = 0; i < mapEventParty3.Party.MemberRoster.Count; i++)
					{
						TroopRosterElement elementCopyAtIndex = mapEventParty3.Party.MemberRoster.GetElementCopyAtIndex(i);
						if (elementCopyAtIndex.Character.IsHero)
						{
							troopRoster.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, elementCopyAtIndex.WoundedNumber, 0, true, lastHeroIndex);
							int num2 = lastHeroIndex;
							lastHeroIndex = num2 + 1;
						}
						else
						{
							troopRoster.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, elementCopyAtIndex.WoundedNumber, 0, true, -1);
						}
					}
				}
				else
				{
					isInfoHiddenLeft = true;
				}
			}
			lastHeroIndex = 0;
			TroopRoster troopRoster2 = TroopRoster.CreateDummyTroopRoster();
			foreach (MapEventParty mapEventParty4 in parties2)
			{
				MobileParty mobileParty2 = mapEventParty4.Party.MobileParty;
				if (mobileParty2 == null || !mobileParty2.IsInfoHidden)
				{
					for (int j = 0; j < mapEventParty4.Party.MemberRoster.Count; j++)
					{
						TroopRosterElement elementCopyAtIndex2 = mapEventParty4.Party.MemberRoster.GetElementCopyAtIndex(j);
						if (elementCopyAtIndex2.Character.IsHero)
						{
							troopRoster2.AddToCounts(elementCopyAtIndex2.Character, elementCopyAtIndex2.Number, false, elementCopyAtIndex2.WoundedNumber, 0, true, lastHeroIndex);
							int num2 = lastHeroIndex;
							lastHeroIndex = num2 + 1;
						}
						else
						{
							troopRoster2.AddToCounts(elementCopyAtIndex2.Character, elementCopyAtIndex2.Number, false, elementCopyAtIndex2.WoundedNumber, 0, true, -1);
						}
					}
				}
				else
				{
					isInfoHiddenRight = true;
				}
			}
			Func<string> func = () => "";
			Func<string> func2 = () => "";
			if (troopRoster.Count > 0)
			{
				func = delegate
				{
					TroopRoster troopRoster3 = TroopRoster.CreateDummyTroopRoster();
					lastHeroIndex = 0;
					foreach (MapEventParty mapEventParty5 in parties1)
					{
						MobileParty mobileParty3 = mapEventParty5.Party.MobileParty;
						if (mobileParty3 == null || !mobileParty3.IsInfoHidden)
						{
							for (int k = 0; k < mapEventParty5.Party.MemberRoster.Count; k++)
							{
								TroopRosterElement elementCopyAtIndex3 = mapEventParty5.Party.MemberRoster.GetElementCopyAtIndex(k);
								if (elementCopyAtIndex3.Character.IsHero)
								{
									troopRoster3.AddToCounts(elementCopyAtIndex3.Character, elementCopyAtIndex3.Number, false, elementCopyAtIndex3.WoundedNumber, 0, true, lastHeroIndex);
									int lastHeroIndex3 = lastHeroIndex;
									lastHeroIndex = lastHeroIndex3 + 1;
								}
								else
								{
									troopRoster3.AddToCounts(elementCopyAtIndex3.Character, elementCopyAtIndex3.Number, false, elementCopyAtIndex3.WoundedNumber, 0, true, -1);
								}
							}
						}
					}
					TextObject textObject = new TextObject("{=QlbkxoSp} {TOOLTIP_TROOPS} ({PARTY_SIZE})", null);
					if (isInfoHiddenLeft)
					{
						if (troopRoster3.TotalManCount > 0)
						{
							textObject = new TextObject("{=ZGjnsItg} {TOOLTIP_TROOPS} ({PARTY_SIZE}) + ?", null);
						}
						else
						{
							textObject = new TextObject("{=CLvFTdTn} {TOOLTIP_TROOPS} (?)", null);
						}
					}
					textObject.SetTextVariable("TOOLTIP_TROOPS", GameTexts.FindText("str_map_tooltip_troops", null));
					textObject.SetTextVariable("PARTY_SIZE", PartyBaseHelper.GetPartySizeText(troopRoster3.TotalManCount - troopRoster3.TotalWounded, troopRoster3.TotalWounded, true));
					return textObject.ToString();
				};
			}
			if (troopRoster2.Count > 0)
			{
				func2 = delegate
				{
					TroopRoster troopRoster4 = TroopRoster.CreateDummyTroopRoster();
					lastHeroIndex = 0;
					foreach (MapEventParty mapEventParty6 in parties2)
					{
						MobileParty mobileParty4 = mapEventParty6.Party.MobileParty;
						if (mobileParty4 == null || !mobileParty4.IsInfoHidden)
						{
							for (int l = 0; l < mapEventParty6.Party.MemberRoster.Count; l++)
							{
								TroopRosterElement elementCopyAtIndex4 = mapEventParty6.Party.MemberRoster.GetElementCopyAtIndex(l);
								if (elementCopyAtIndex4.Character.IsHero)
								{
									troopRoster4.AddToCounts(elementCopyAtIndex4.Character, elementCopyAtIndex4.Number, false, elementCopyAtIndex4.WoundedNumber, 0, true, lastHeroIndex);
									int lastHeroIndex2 = lastHeroIndex;
									lastHeroIndex = lastHeroIndex2 + 1;
								}
								else
								{
									troopRoster4.AddToCounts(elementCopyAtIndex4.Character, elementCopyAtIndex4.Number, false, elementCopyAtIndex4.WoundedNumber, 0, true, -1);
								}
							}
						}
					}
					TextObject textObject2 = new TextObject("{=QlbkxoSp} {TOOLTIP_TROOPS} ({PARTY_SIZE})", null);
					if (isInfoHiddenRight)
					{
						if (troopRoster4.TotalManCount > 0)
						{
							textObject2 = new TextObject("{=ZGjnsItg} {TOOLTIP_TROOPS} ({PARTY_SIZE}) + ?", null);
						}
						else
						{
							textObject2 = new TextObject("{=CLvFTdTn} {TOOLTIP_TROOPS} (?)", null);
						}
					}
					textObject2.SetTextVariable("TOOLTIP_TROOPS", GameTexts.FindText("str_map_tooltip_troops", null));
					textObject2.SetTextVariable("PARTY_SIZE", PartyBaseHelper.GetPartySizeText(troopRoster4.TotalManCount - troopRoster4.TotalWounded, troopRoster4.TotalWounded, true));
					return textObject2.ToString();
				};
			}
			if (func().Length != 0 && func2().Length != 0)
			{
				propertyBasedTooltipVM.AddProperty("", "", -1, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty(func, func2, 0, TooltipProperty.TooltipPropertyFlags.None);
			}
			if (isExtended)
			{
				string blankString = new TextObject("{=!} ", null).ToString();
				propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.DefaultSeperator);
				int num3 = 0;
				Func<string> <>9__4;
				Func<string> <>9__5;
				while (num3 < troopRoster.Count || num3 < troopRoster2.Count)
				{
					Func<string> func3;
					if ((func3 = <>9__4) == null)
					{
						func3 = (<>9__4 = () => blankString);
					}
					Func<string> func4 = func3;
					Func<string> func5;
					if ((func5 = <>9__5) == null)
					{
						func5 = (<>9__5 = () => blankString);
					}
					Func<string> func6 = func5;
					if (num3 < troopRoster.Count)
					{
						CharacterObject character2 = troopRoster.GetElementCopyAtIndex(num3).Character;
						func4 = delegate
						{
							lastHeroIndex = 0;
							foreach (MapEventParty mapEventParty7 in parties1)
							{
								for (int m = 0; m < mapEventParty7.Party.MemberRoster.Count; m++)
								{
									TroopRosterElement elementCopyAtIndex5 = mapEventParty7.Party.MemberRoster.GetElementCopyAtIndex(m);
									if (elementCopyAtIndex5.Character == character2)
									{
										TextObject textObject3;
										if (elementCopyAtIndex5.Character.IsHero)
										{
											textObject3 = new TextObject("{=W1tsTWZv} {PARTY_MEMBER.LINK} ({MEMBER_HEALTH}%)", null);
											textObject3.SetTextVariable("MEMBER_HEALTH", elementCopyAtIndex5.Character.HeroObject.HitPoints * 100 / elementCopyAtIndex5.Character.MaxHitPoints());
										}
										else
										{
											textObject3 = new TextObject("{=vLaBJFGy} {PARTY_MEMBER.LINK} ({PARTY_SIZE})", null);
											textObject3.SetTextVariable("PARTY_SIZE", PartyBaseHelper.GetPartySizeText(elementCopyAtIndex5.Number - elementCopyAtIndex5.WoundedNumber, elementCopyAtIndex5.WoundedNumber, true));
										}
										StringHelpers.SetCharacterProperties("PARTY_MEMBER", elementCopyAtIndex5.Character, textObject3, false);
										return textObject3.ToString();
									}
								}
							}
							return blankString;
						};
					}
					else if ((num3 == troopRoster.Count) & isInfoHiddenLeft)
					{
						func4 = () => "?";
					}
					if (num3 < troopRoster2.Count)
					{
						CharacterObject character = troopRoster2.GetElementCopyAtIndex(num3).Character;
						func6 = delegate
						{
							lastHeroIndex = 0;
							foreach (MapEventParty mapEventParty8 in parties2)
							{
								for (int n = 0; n < mapEventParty8.Party.MemberRoster.Count; n++)
								{
									TroopRosterElement elementCopyAtIndex6 = mapEventParty8.Party.MemberRoster.GetElementCopyAtIndex(n);
									if (character == elementCopyAtIndex6.Character)
									{
										TextObject textObject4;
										if (character.IsHero)
										{
											textObject4 = new TextObject("{=PS02CqPu} {PARTY_MEMBER.LINK} (Health: {MEMBER_HEALTH}%)", null);
											textObject4.SetTextVariable("MEMBER_HEALTH", elementCopyAtIndex6.Character.HeroObject.HitPoints * 100 / elementCopyAtIndex6.Character.MaxHitPoints());
										}
										else
										{
											textObject4 = new TextObject("{=vLaBJFGy} {PARTY_MEMBER.LINK} ({PARTY_SIZE})", null);
											textObject4.SetTextVariable("PARTY_SIZE", PartyBaseHelper.GetPartySizeText(elementCopyAtIndex6.Number - elementCopyAtIndex6.WoundedNumber, elementCopyAtIndex6.WoundedNumber, true));
										}
										StringHelpers.SetCharacterProperties("PARTY_MEMBER", elementCopyAtIndex6.Character, textObject4, false);
										return textObject4.ToString();
									}
								}
							}
							return blankString;
						};
					}
					else if ((num3 == troopRoster2.Count) & isInfoHiddenRight)
					{
						func6 = () => "?";
					}
					propertyBasedTooltipVM.AddProperty(func4, func6, 0, TooltipProperty.TooltipPropertyFlags.None);
					num3++;
				}
				if (((troopRoster.Count >= troopRoster2.Count) & isInfoHiddenLeft) || ((troopRoster2.Count >= troopRoster.Count) & isInfoHiddenRight))
				{
					Func<string> func7 = () => blankString;
					Func<string> func8 = () => blankString;
					if ((troopRoster.Count >= troopRoster2.Count) & isInfoHiddenLeft)
					{
						func7 = () => "?";
					}
					if ((troopRoster2.Count >= troopRoster.Count) & isInfoHiddenRight)
					{
						func8 = () => "?";
					}
					propertyBasedTooltipVM.AddProperty(func7, func8, 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.BattleModeOver);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000125F4 File Offset: 0x000107F4
		private static void AddPartyTroopProperties(PropertyBasedTooltipVM propertyBasedTooltipVM, TroopRoster troopRoster, TextObject title, bool isInspected, Func<TroopRoster> funcToDoBeforeLambda = null)
		{
			propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
			propertyBasedTooltipVM.AddProperty(title.ToString(), delegate
			{
				TroopRoster troopRoster2 = ((funcToDoBeforeLambda != null) ? funcToDoBeforeLambda() : troopRoster);
				int num2 = 0;
				int num3 = 0;
				for (int l = 0; l < troopRoster2.Count; l++)
				{
					TroopRosterElement elementCopyAtIndex3 = troopRoster2.GetElementCopyAtIndex(l);
					num2 += elementCopyAtIndex3.Number - elementCopyAtIndex3.WoundedNumber;
					num3 += elementCopyAtIndex3.WoundedNumber;
				}
				TextObject textObject3 = new TextObject("{=iXXTONWb} ({PARTY_SIZE})", null);
				textObject3.SetTextVariable("PARTY_SIZE", PartyBaseHelper.GetPartySizeText(num2, num3, isInspected));
				return textObject3.ToString();
			}, 0, TooltipProperty.TooltipPropertyFlags.None);
			if (isInspected)
			{
				propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
			}
			if (isInspected)
			{
				Dictionary<FormationClass, Tuple<int, int>> dictionary = new Dictionary<FormationClass, Tuple<int, int>>();
				for (int i = 0; i < troopRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = troopRoster.GetElementCopyAtIndex(i);
					if (dictionary.ContainsKey(elementCopyAtIndex.Character.DefaultFormationClass))
					{
						Tuple<int, int> tuple = dictionary[elementCopyAtIndex.Character.DefaultFormationClass];
						dictionary[elementCopyAtIndex.Character.DefaultFormationClass] = new Tuple<int, int>(tuple.Item1 + elementCopyAtIndex.Number - elementCopyAtIndex.WoundedNumber, tuple.Item2 + elementCopyAtIndex.WoundedNumber);
					}
					else
					{
						dictionary.Add(elementCopyAtIndex.Character.DefaultFormationClass, new Tuple<int, int>(elementCopyAtIndex.Number - elementCopyAtIndex.WoundedNumber, elementCopyAtIndex.WoundedNumber));
					}
				}
				foreach (KeyValuePair<FormationClass, Tuple<int, int>> keyValuePair in dictionary.OrderBy<KeyValuePair<FormationClass, Tuple<int, int>>, FormationClass>((KeyValuePair<FormationClass, Tuple<int, int>> x) => x.Key))
				{
					TextObject textObject = new TextObject("{=Dqydb21E} {PARTY_SIZE}", null);
					textObject.SetTextVariable("PARTY_SIZE", PartyBaseHelper.GetPartySizeText(keyValuePair.Value.Item1, keyValuePair.Value.Item2, true));
					TextObject textObject2 = GameTexts.FindText("str_troop_type_name", keyValuePair.Key.GetName());
					propertyBasedTooltipVM.AddProperty(textObject2.ToString(), textObject.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			if (propertyBasedTooltipVM.IsExtended & isInspected)
			{
				propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_troop_types", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
				propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.DefaultSeperator);
				for (int j = 0; j < troopRoster.Count; j++)
				{
					TroopRosterElement elementCopyAtIndex2 = troopRoster.GetElementCopyAtIndex(j);
					if (elementCopyAtIndex2.Character.IsHero)
					{
						CharacterObject hero = elementCopyAtIndex2.Character;
						TextObject name = hero.Name;
						Clan clan = hero.HeroObject.Clan;
						string text = ((clan != null && clan.HasBloodFeudWithPlayer) ? GameTexts.FindText("str_STR1_STR2", null).SetTextVariable("STR1", name).SetTextVariable("STR2", "{=!}<img src=\"SPGeneral\\blood_feud_icon\" extend=\"3\">")
							.ToString() : name.ToString());
						propertyBasedTooltipVM.AddProperty(text, delegate
						{
							TroopRoster troopRoster3 = ((funcToDoBeforeLambda != null) ? funcToDoBeforeLambda() : troopRoster);
							int num4 = troopRoster3.FindIndexOfTroop(hero);
							if (num4 == -1)
							{
								return string.Empty;
							}
							TroopRosterElement elementCopyAtIndex4 = troopRoster3.GetElementCopyAtIndex(num4);
							TextObject textObject4 = GameTexts.FindText("str_NUMBER_percent", null);
							textObject4.SetTextVariable("NUMBER", elementCopyAtIndex4.Character.HeroObject.HitPoints * 100 / elementCopyAtIndex4.Character.MaxHitPoints());
							return textObject4.ToString();
						}, 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
				for (int k = 0; k < troopRoster.Count; k++)
				{
					int num = k;
					CharacterObject character = troopRoster.GetElementCopyAtIndex(num).Character;
					if (!character.IsHero)
					{
						propertyBasedTooltipVM.AddProperty(character.Name.ToString(), delegate
						{
							TroopRoster troopRoster4 = ((funcToDoBeforeLambda != null) ? funcToDoBeforeLambda() : troopRoster);
							int num5 = troopRoster4.FindIndexOfTroop(character);
							if (num5 != -1)
							{
								if (num5 > troopRoster4.Count)
								{
									return string.Empty;
								}
								TroopRosterElement elementCopyAtIndex5 = troopRoster4.GetElementCopyAtIndex(num5);
								if (elementCopyAtIndex5.Character == null)
								{
									return string.Empty;
								}
								CharacterObject character2 = elementCopyAtIndex5.Character;
								if (character2 != null && !character2.IsHero)
								{
									TextObject textObject5 = new TextObject("{=!}{PARTY_SIZE}", null);
									textObject5.SetTextVariable("PARTY_SIZE", PartyBaseHelper.GetPartySizeText(elementCopyAtIndex5.Number - elementCopyAtIndex5.WoundedNumber, elementCopyAtIndex5.WoundedNumber, true));
									return textObject5.ToString();
								}
							}
							return string.Empty;
						}, 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000129A4 File Offset: 0x00010BA4
		private static void AddPartyShipProperties(PropertyBasedTooltipVM propertyBasedTooltipVM, MBList<MobileParty> parties, bool openedFromMenuLayout, bool checkForMapVisibility)
		{
			propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
			Dictionary<ShipHull.ShipType, int> dictionary = new Dictionary<ShipHull.ShipType, int>();
			Dictionary<ShipHull, int> dictionary2 = new Dictionary<ShipHull, int>();
			int num = 0;
			bool flag = parties.Any<MobileParty>((MobileParty x) => x.IsInspected);
			foreach (MobileParty mobileParty in parties)
			{
				if (mobileParty.Ships.Count > 0)
				{
					num += mobileParty.Ships.Count;
					if (openedFromMenuLayout || flag || !checkForMapVisibility)
					{
						for (int i = 0; i < mobileParty.Ships.Count; i++)
						{
							Ship ship = mobileParty.Ships[i];
							ShipHull shipHull = ship.ShipHull;
							ShipHull.ShipType type = ship.ShipHull.Type;
							if (dictionary.ContainsKey(type))
							{
								Dictionary<ShipHull.ShipType, int> dictionary3 = dictionary;
								ShipHull.ShipType shipType = type;
								int num2 = dictionary3[shipType];
								dictionary3[shipType] = num2 + 1;
							}
							else
							{
								dictionary[type] = 1;
							}
							if (dictionary2.ContainsKey(shipHull))
							{
								Dictionary<ShipHull, int> dictionary4 = dictionary2;
								ShipHull shipHull2 = shipHull;
								int num2 = dictionary4[shipHull2];
								dictionary4[shipHull2] = num2 + 1;
							}
							else
							{
								dictionary2[shipHull] = 1;
							}
						}
					}
				}
			}
			string shipSizeText = PartyBaseHelper.GetShipSizeText(num, openedFromMenuLayout || flag || !checkForMapVisibility);
			TextObject textObject = GameTexts.FindText("str_men_count_in_paranthesis_wo_wounded", null);
			textObject.SetTextVariable("NUMBER_OF_MEN", shipSizeText);
			propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_map_tooltip_ships", null).ToString(), textObject.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
			if (openedFromMenuLayout || flag || !checkForMapVisibility)
			{
				propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.RundownSeperator);
				foreach (KeyValuePair<ShipHull.ShipType, int> keyValuePair in dictionary.OrderBy<KeyValuePair<ShipHull.ShipType, int>, ShipHull.ShipType>((KeyValuePair<ShipHull.ShipType, int> x) => x.Key))
				{
					TextObject textObject2 = new TextObject("{=Dqydb21E} {PARTY_SIZE}", null);
					textObject2.SetTextVariable("PARTY_SIZE", keyValuePair.Value);
					TextObject textObject3 = GameTexts.FindText("str_ship_type", keyValuePair.Key.ToString().ToLower());
					propertyBasedTooltipVM.AddProperty(textObject3.ToString(), textObject2.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
				}
				if (propertyBasedTooltipVM.IsExtended)
				{
					propertyBasedTooltipVM.AddProperty(string.Empty, string.Empty, -1, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty(GameTexts.FindText("str_ship_types", null).ToString(), " ", 0, TooltipProperty.TooltipPropertyFlags.None);
					propertyBasedTooltipVM.AddProperty("", "", 0, TooltipProperty.TooltipPropertyFlags.DefaultSeperator);
					foreach (KeyValuePair<ShipHull, int> keyValuePair2 in dictionary2)
					{
						ShipHull key = keyValuePair2.Key;
						int value = keyValuePair2.Value;
						propertyBasedTooltipVM.AddProperty(key.Name.ToString(), value.ToString(), 0, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00012CE4 File Offset: 0x00010EE4
		public static void RefreshMapMarkerTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			MapMarker mapMarker = args[0] as MapMarker;
			propertyBasedTooltipVM.Mode = 1;
			propertyBasedTooltipVM.AddProperty("", new Func<string>(mapMarker.Name.ToString), 0, TooltipProperty.TooltipPropertyFlags.Title);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00012D24 File Offset: 0x00010F24
		public static void RefreshBattleWreckageTooltip(PropertyBasedTooltipVM propertyBasedTooltipVM, object[] args)
		{
			BattleWreckage battleWreckage = args[0] as BattleWreckage;
			propertyBasedTooltipVM.Mode = 1;
			if (battleWreckage.IsInvestigated)
			{
				TextObject textObject = new TextObject("{=NFavDfWG}Battle Remains", null);
				propertyBasedTooltipVM.AddProperty("", textObject.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
				return;
			}
			propertyBasedTooltipVM.AddProperty("", battleWreckage.Name.ToString(), 0, TooltipProperty.TooltipPropertyFlags.Title);
		}

		// Token: 0x040000DB RID: 219
		private static readonly IEqualityComparer<ValueTuple<ItemCategory, int>> itemCategoryDistinctComparer = new CampaignUIHelper.ProductInputOutputEqualityComparer();

		// Token: 0x040000DC RID: 220
		private static string ExtendKeyId = "ExtendModifier";

		// Token: 0x040000DD RID: 221
		private static string FollowModifierKeyId = "FollowModifier";

		// Token: 0x040000DE RID: 222
		private static string MapClickKeyId = "MapClick";
	}
}
