using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003F2 RID: 1010
	public class CampaignAdvancedStartingPlayerOptionsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003D6C RID: 15724 RVA: 0x000FF29C File Offset: 0x000FD49C
		public override void RegisterEvents()
		{
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action<int>(this.OnCharacterCreationIsOver));
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x000FF2B5 File Offset: 0x000FD4B5
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x000FF2B8 File Offset: 0x000FD4B8
		private void OnCharacterCreationIsOver(int index)
		{
			if (index == 8)
			{
				string startType = Campaign.Current.AdvancedStartData.GetStartType();
				if (!(startType == "king"))
				{
					if (!(startType == "mercenary"))
					{
						if (!(startType == "vassal"))
						{
							if (!(startType == "trader"))
							{
								if (!(startType == "outlaw"))
								{
									if (startType == "beggar")
									{
										this.StartGameAsBeggar();
									}
								}
								else
								{
									this.StartGameAsOutlaw();
								}
							}
							else
							{
								this.StartGameAsTrader();
							}
						}
						else
						{
							this.StartGameAsVassal(Campaign.Current.AdvancedStartData.GetKingdomId());
						}
					}
					else
					{
						this.StartGameAsMercenary(Campaign.Current.AdvancedStartData.GetKingdomId());
					}
				}
				else
				{
					this.StartGameAsRuler(Campaign.Current.AdvancedStartData.GetKingdomId());
				}
				MapState mapState;
				if ((mapState = GameStateManager.Current.ActiveState as MapState) != null)
				{
					mapState.Handler.ResetCamera(true, true);
					mapState.Handler.TeleportCameraToMainParty();
				}
				MobileParty.MainParty.MemberRoster.UpdateVersion();
			}
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x000FF3C4 File Offset: 0x000FD5C4
		private void StartGameAsRuler(string kingdomId)
		{
			MobileParty.MainParty.ItemRoster.Clear();
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.EnsureMinimumClanTier(5);
			Kingdom kingdom = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.ResolveKingdom(kingdomId);
			Clan.PlayerClan.ShouldStayInKingdomUntil = CampaignTime.Zero;
			kingdom.RulingClan = Clan.PlayerClan;
			Clan.PlayerClan.Kingdom = kingdom;
			MBFastRandom mbfastRandom = new MBFastRandom(Campaign.Current.Options.Seed ^ 3324857891U);
			Settlement settlement2;
			if (Campaign.Current.AdvancedStartData.GetScenario() == "LastStand")
			{
				settlement2 = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GiveStartingFiefs(1, mbfastRandom, (Settlement settlement) => (settlement.IsTown || settlement.IsCastle) && settlement.OwnerClan.MapFaction == kingdom);
			}
			else
			{
				settlement2 = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GiveStartingFiefs(2, mbfastRandom, (Settlement settlement) => settlement.IsTown && settlement.OwnerClan.MapFaction == kingdom);
				if (Clan.PlayerClan.Settlements.Count<Settlement>((Settlement s) => s.IsTown) < 2)
				{
					settlement2 = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GiveStartingFiefs(2 - Clan.PlayerClan.Settlements.Count, mbfastRandom, (Settlement settlement) => settlement.IsTown && settlement.Culture == kingdom.Culture);
				}
			}
			if (settlement2 == null)
			{
				settlement2 = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.FindFallbackStartingTown(mbfastRandom, false);
			}
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.UpdateMainHeroHomeSettlement(settlement2);
			MobileParty.MainParty.Position = Hero.MainHero.HomeSettlement.GatePosition;
			int num = mbfastRandom.Next(100000, 150001);
			Hero.MainHero.Gold = num;
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.AdjustStartingFood(mbfastRandom, 100, 200, kingdom.Culture);
			Equipment suitableEquipmentSet = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GetSuitableEquipmentSet(Hero.MainHero, kingdom.Culture, EquipmentCategories.IsKingdomRulerTemplate, Equipment.EquipmentType.Battle, mbfastRandom);
			Equipment suitableEquipmentSet2 = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GetSuitableEquipmentSet(Hero.MainHero, kingdom.Culture, EquipmentCategories.IsKingdomRulerTemplate, Equipment.EquipmentType.Civilian, mbfastRandom);
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.AssignMainHeroEquipmentKeepingHorse(suitableEquipmentSet);
			EquipmentHelper.AssignHeroEquipmentFromEquipment(Hero.MainHero, suitableEquipmentSet2);
			this.GiveTroopsFromTree(new List<CharacterObject> { kingdom.Culture.EliteBasicTroop }, CampaignAdvancedStartingPlayerOptionsCampaignBehavior.RulerEliteTroopCounts, mbfastRandom);
			this.GiveTroopsFromTree(new List<CharacterObject> { kingdom.Culture.BasicTroop }, CampaignAdvancedStartingPlayerOptionsCampaignBehavior.RulerNormalTroopCounts, mbfastRandom);
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GiveStartingCompanions(4, mbfastRandom);
			Hero.MainHero.AddInfluenceWithKingdom((float)mbfastRandom.Next(75, 151));
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x000FF5F0 File Offset: 0x000FD7F0
		private void StartGameAsVassal(string kingdomId)
		{
			MobileParty.MainParty.ItemRoster.Clear();
			MBFastRandom mbfastRandom = new MBFastRandom(Campaign.Current.Options.Seed ^ 753149491U);
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.EnsureMinimumClanTier(2);
			Kingdom kingdom = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.ResolveKingdom(kingdomId);
			Clan.PlayerClan.DebtToKingdom = 0;
			FactionHelper.AdjustFactionStancesForClanJoiningKingdom(Clan.PlayerClan, kingdom);
			Clan.PlayerClan.ShouldStayInKingdomUntil = default(CampaignTime);
			Clan.PlayerClan.Kingdom = kingdom;
			int num = mbfastRandom.Next(25000, 75001);
			Hero.MainHero.Gold = num;
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.AdjustStartingFood(mbfastRandom, 75, 100, kingdom.Culture);
			Equipment suitableEquipmentSet = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GetSuitableEquipmentSet(Hero.MainHero, kingdom.Culture, EquipmentCategories.IsLordTemplate, Equipment.EquipmentType.Battle, mbfastRandom);
			Equipment suitableEquipmentSet2 = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GetSuitableEquipmentSet(Hero.MainHero, kingdom.Culture, EquipmentCategories.IsLordTemplate, Equipment.EquipmentType.Civilian, mbfastRandom);
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.AssignMainHeroEquipmentKeepingHorse(suitableEquipmentSet);
			EquipmentHelper.AssignHeroEquipmentFromEquipment(Hero.MainHero, suitableEquipmentSet2);
			this.GiveTroopsFromTree(new List<CharacterObject> { kingdom.Culture.EliteBasicTroop }, CampaignAdvancedStartingPlayerOptionsCampaignBehavior.VassalEliteTroopCounts, mbfastRandom);
			this.GiveTroopsFromTree(new List<CharacterObject> { kingdom.Culture.BasicTroop }, CampaignAdvancedStartingPlayerOptionsCampaignBehavior.VassalNormalTroopCounts, mbfastRandom);
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GiveStartingCompanions(2, mbfastRandom);
			Settlement settlement2 = null;
			if (Campaign.Current.AdvancedStartData.GetScenario() != "LastStand")
			{
				settlement2 = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GiveStartingFiefs(1, mbfastRandom, (Settlement settlement) => settlement.IsCastle && settlement.OwnerClan.MapFaction == kingdom);
			}
			if (settlement2 == null)
			{
				settlement2 = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.FindFallbackStartingTown(mbfastRandom, false);
			}
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.UpdateMainHeroHomeSettlement(settlement2);
			MobileParty.MainParty.Position = settlement2.GatePosition;
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x000FF7A0 File Offset: 0x000FD9A0
		private void StartGameAsMercenary(string kingdomId)
		{
			MobileParty.MainParty.ItemRoster.Clear();
			MBFastRandom mbfastRandom = new MBFastRandom(Campaign.Current.Options.Seed ^ 1160783149U);
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.EnsureMinimumClanTier(1);
			Kingdom kingdom = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.ResolveKingdom(kingdomId);
			FactionHelper.AdjustFactionStancesForClanJoiningKingdom(Clan.PlayerClan, kingdom);
			Clan.PlayerClan.MercenaryAwardMultiplier = 50;
			Clan.PlayerClan.Kingdom = kingdom;
			Clan.PlayerClan.StartMercenaryService();
			Campaign.Current.KingdomManager.PlayerMercenaryServiceNextRenewalDay = Campaign.CurrentTime + 30f * (float)CampaignTime.HoursInDay;
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.UpdateMainHeroHomeSettlement(CampaignAdvancedStartingPlayerOptionsCampaignBehavior.FindFallbackStartingTown(mbfastRandom, false));
			Kingdom kingdom2 = this.FindEnemyKingdomOrDeclareWar(kingdom);
			MobileParty.MainParty.Position = this.GetStartingPositionForMercenary(kingdom, kingdom2, mbfastRandom);
			int num = mbfastRandom.Next(10000, 15001);
			Hero.MainHero.Gold = num;
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.AdjustStartingFood(mbfastRandom, 30, 50, kingdom.Culture);
			this.GiveTroopsFromTree(new List<CharacterObject> { kingdom.BasicTroop, kingdom2.BasicTroop }, CampaignAdvancedStartingPlayerOptionsCampaignBehavior.MercenaryTroopCounts, mbfastRandom);
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GiveStartingCompanions(1, mbfastRandom);
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x000FF8B4 File Offset: 0x000FDAB4
		private void StartGameAsTrader()
		{
			MobileParty.MainParty.ItemRoster.Clear();
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.EnsureMinimumClanTier(1);
			MBFastRandom mbfastRandom = new MBFastRandom(Campaign.Current.Options.Seed ^ 1374241951U);
			Town town = Town.AllTowns[mbfastRandom.Next(0, Town.AllTowns.Count)];
			Kingdom kingdom = town.Settlement.OwnerClan.Kingdom;
			MobileParty.MainParty.Position = town.Settlement.GatePosition;
			Hero.MainHero.BornSettlement = town.Settlement;
			Hero.MainHero.UpdateHomeSettlement();
			int num = mbfastRandom.Next(5000, 10001);
			Hero.MainHero.Gold = num;
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.AdjustStartingFood(mbfastRandom, 30, 50, kingdom.Culture);
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GiveStartingCompanions(2, mbfastRandom);
			List<Workshop> list = town.Workshops.Where<Workshop>((Workshop w) => !w.WorkshopType.IsHidden).ToList<Workshop>();
			ChangeOwnerOfWorkshopAction.ApplyByFree(list[mbfastRandom.Next(0, list.Count)], Hero.MainHero);
			Hero hero = Clan.PlayerClan.Companions[mbfastRandom.Next(0, Clan.PlayerClan.Companions.Count)];
			PartyTemplateObject randomCaravanTemplate = CaravanHelper.GetRandomCaravanTemplate(kingdom.Culture, false, true);
			CaravanPartyComponent.CreateCaravanParty(Hero.MainHero, town.Settlement, randomCaravanTemplate, true, hero, null, false);
			List<CharacterObject> list2 = kingdom.Culture.NotableTemplates.Where<CharacterObject>((CharacterObject t) => t.IsFemale == Hero.MainHero.IsFemale).ToList<CharacterObject>();
			CharacterObject characterObject = list2[mbfastRandom.Next(0, list2.Count)];
			List<Equipment> list3 = characterObject.BattleEquipments.ToList<Equipment>();
			List<Equipment> list4 = characterObject.CivilianEquipments.ToList<Equipment>();
			EquipmentHelper.AssignHeroEquipmentFromEquipment(Hero.MainHero, list3[mbfastRandom.Next(0, list3.Count)].Clone(false));
			EquipmentHelper.AssignHeroEquipmentFromEquipment(Hero.MainHero, list4[mbfastRandom.Next(0, list4.Count)].Clone(false));
			ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>("mule");
			int num2 = mbfastRandom.Next(3, 6);
			MobileParty.MainParty.ItemRoster.AddToCounts(new EquipmentElement(@object, null, null, false), num2);
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GiveTroopsToMainPartyFromCaravanTemplate(kingdom.Culture, mbfastRandom);
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GiveTradeGoods(kingdom.Culture, 1500, 5000, 1, mbfastRandom);
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x000FFB28 File Offset: 0x000FDD28
		private void StartGameAsOutlaw()
		{
			MobileParty.MainParty.ItemRoster.Clear();
			MBFastRandom mbfastRandom = new MBFastRandom(Campaign.Current.Options.Seed ^ 4060451693U);
			Town town = Town.AllTowns[mbfastRandom.Next(0, Town.AllTowns.Count)];
			MobileParty.MainParty.Position = town.Settlement.GatePosition;
			int num = mbfastRandom.Next(2000, 5001);
			Hero.MainHero.Gold = num;
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.AdjustStartingFood(mbfastRandom, 3, 7, town.Culture);
			Settlement settlement = null;
			float num2 = float.MaxValue;
			foreach (Hideout hideout in Hideout.All)
			{
				float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(town.Settlement, hideout.Settlement, false, false, MobileParty.NavigationType.Default);
				if (distance < num2)
				{
					num2 = distance;
					settlement = hideout.Settlement;
				}
			}
			CultureObject culture = settlement.Culture;
			List<CharacterObject> list = new List<CharacterObject>();
			list.Add(culture.BanditBandit);
			list.Add(culture.BanditRaider);
			int num3 = mbfastRandom.Next(10, 16);
			for (int i = 0; i < num3; i++)
			{
				CharacterObject characterObject = list[mbfastRandom.Next(0, list.Count)];
				MobileParty.MainParty.MemberRoster.AddToCounts(characterObject, 1, false, 0, 0, true, -1);
			}
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GiveStartingCompanions(1, mbfastRandom);
			foreach (Kingdom kingdom in Campaign.Current.Kingdoms)
			{
				if (!kingdom.IsEliminated)
				{
					float num4 = MBMath.ClampFloat(kingdom.MainHeroCrimeRating + 45f, 0f, Campaign.Current.Models.CrimeModel.GetMaxCrimeRating());
					kingdom.MainHeroCrimeRating = num4;
				}
			}
			foreach (Clan clan in Campaign.Current.Clans)
			{
				if (!clan.IsEliminated && !clan.MapFaction.IsKingdomFaction && clan != Clan.PlayerClan)
				{
					float num5 = MBMath.ClampFloat(clan.MainHeroCrimeRating + 45f, 0f, Campaign.Current.Models.CrimeModel.GetMaxCrimeRating());
					clan.MainHeroCrimeRating = num5;
				}
			}
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GiveTradeGoods(Hero.MainHero.Culture, 1500, 5000, mbfastRandom.Next(3, 6), mbfastRandom);
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x000FFDF4 File Offset: 0x000FDFF4
		private void StartGameAsBeggar()
		{
			MobileParty.MainParty.ItemRoster.Clear();
			MBFastRandom mbfastRandom = new MBFastRandom(Campaign.Current.Options.Seed ^ 2508482539U);
			List<Town> list = Town.AllTowns.Where<Town>((Town t) => t.Culture == Hero.MainHero.Culture).ToList<Town>();
			Town town = list[mbfastRandom.Next(0, list.Count)];
			MobileParty.MainParty.Position = town.Settlement.GatePosition;
			Hero.MainHero.Gold = 0;
			if (!Hero.MainHero.IsFemale)
			{
				CharacterObject beggar = Hero.MainHero.Culture.Beggar;
			}
			else
			{
				CharacterObject femaleBeggar = Hero.MainHero.Culture.FemaleBeggar;
			}
			for (int i = 0; i < 12; i++)
			{
				Hero.MainHero.BattleEquipment[i] = EquipmentElement.Invalid;
				Hero.MainHero.CivilianEquipment[i] = EquipmentElement.Invalid;
				Hero.MainHero.StealthEquipment[i] = EquipmentElement.Invalid;
			}
			ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>("aso_beggar_robe");
			Hero.MainHero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(@object, null, null, false);
			Hero.MainHero.CivilianEquipment[EquipmentIndex.Body] = new EquipmentElement(@object, null, null, false);
			Hero.MainHero.StealthEquipment[EquipmentIndex.Body] = new EquipmentElement(@object, null, null, false);
			Hero.MainHero.BattleEquipment[EquipmentIndex.WeaponItemBeginSlot] = new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("aso_beggar_sword"), null, null, false);
			Hero.MainHero.StealthEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("stealth_throwing_stone"), null, null, false);
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x000FFFB8 File Offset: 0x000FE1B8
		public static void AssignMainHeroEquipmentKeepingHorse(Equipment sourceEquipment)
		{
			Equipment equipment = (sourceEquipment.IsCivilian ? Hero.MainHero.CivilianEquipment : Hero.MainHero.BattleEquipment);
			EquipmentElement equipmentElement = equipment[EquipmentIndex.ArmorItemEndSlot];
			EquipmentElement equipmentElement2 = equipment[EquipmentIndex.HorseHarness];
			EquipmentElement equipmentElement3 = sourceEquipment[EquipmentIndex.ArmorItemEndSlot];
			EquipmentElement equipmentElement4 = sourceEquipment[EquipmentIndex.HorseHarness];
			if (!equipmentElement3.IsEmpty)
			{
				MobileParty.MainParty.ItemRoster.AddToCounts(equipmentElement3, 1);
			}
			if (!equipmentElement4.IsEmpty)
			{
				MobileParty.MainParty.ItemRoster.AddToCounts(equipmentElement4, 1);
			}
			EquipmentHelper.AssignHeroEquipmentFromEquipment(Hero.MainHero, sourceEquipment);
			equipment[EquipmentIndex.ArmorItemEndSlot] = equipmentElement;
			equipment[EquipmentIndex.HorseHarness] = equipmentElement2;
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x0010005C File Offset: 0x000FE25C
		private static void GiveTroopsToMainPartyFromCaravanTemplate(CultureObject culture, MBFastRandom random)
		{
			List<PartyTemplateObject> list = new List<PartyTemplateObject>();
			foreach (PartyTemplateObject partyTemplateObject in culture.CaravanPartyTemplates)
			{
				if (partyTemplateObject.ShipHulls.Count == 0)
				{
					list.Add(partyTemplateObject);
				}
			}
			PartyTemplateObject partyTemplateObject2 = list[random.Next(0, list.Count)];
			List<ValueTuple<CharacterObject, int>> list2 = new List<ValueTuple<CharacterObject, int>>();
			int num = 0;
			for (int i = 0; i < partyTemplateObject2.Stacks.Count; i++)
			{
				CharacterObject character = partyTemplateObject2.Stacks[i].Character;
				int num2 = random.Next(partyTemplateObject2.Stacks[i].MinValue, partyTemplateObject2.Stacks[i].MaxValue + 1);
				list2.Add(new ValueTuple<CharacterObject, int>(character, num2));
				num += num2;
			}
			int num3 = (int)MobileParty.MainParty.Party.PartySizeLimitExplainer.ResultNumber - MobileParty.MainParty.Party.NumberOfAllMembers;
			if (num > num3 && num > 0 && num3 > 0)
			{
				int num4 = 0;
				for (int j = 0; j < partyTemplateObject2.Stacks.Count; j++)
				{
					num4 += partyTemplateObject2.Stacks[j].MinValue;
				}
				if (num3 <= num4)
				{
					float num5 = ((num4 > 0) ? ((float)num3 / (float)num4) : 0f);
					for (int k = 0; k < list2.Count; k++)
					{
						list2[k] = new ValueTuple<CharacterObject, int>(list2[k].Item1, (int)((float)partyTemplateObject2.Stacks[k].MinValue * num5));
					}
				}
				else
				{
					float num6 = (float)(num3 - num4);
					int num7 = num - num4;
					float num8 = num6 / (float)num7;
					for (int l = 0; l < list2.Count; l++)
					{
						int num9 = list2[l].Item2 - partyTemplateObject2.Stacks[l].MinValue;
						list2[l] = new ValueTuple<CharacterObject, int>(list2[l].Item1, partyTemplateObject2.Stacks[l].MinValue + (int)((float)num9 * num8));
					}
				}
			}
			foreach (ValueTuple<CharacterObject, int> valueTuple in list2)
			{
				CharacterObject item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				if (item2 > 0)
				{
					MobileParty.MainParty.MemberRoster.AddToCounts(item, item2, false, 0, 0, true, -1);
				}
			}
		}

		// Token: 0x06003D77 RID: 15735 RVA: 0x0010030C File Offset: 0x000FE50C
		public static Equipment GetSuitableEquipmentSet(Hero hero, CultureObject culture, EquipmentCategories customFlags, Equipment.EquipmentType equipmentType, MBFastRandom random)
		{
			MBList<Equipment> mblist = new MBList<Equipment>();
			if (hero.IsFemale)
			{
				customFlags |= EquipmentCategories.IsFemaleTemplate;
			}
			foreach (MBEquipmentRoster mbequipmentRoster in MBEquipmentRosterExtensions.All)
			{
				if (mbequipmentRoster.EquipmentCulture == culture && mbequipmentRoster.EquipmentCategories == customFlags)
				{
					foreach (Equipment equipment in mbequipmentRoster.AllEquipments)
					{
						if (equipment.ItemEquipmentType == equipmentType)
						{
							mblist.Add(equipment);
						}
					}
				}
			}
			return mblist[random.Next(0, mblist.Count)];
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x001003E0 File Offset: 0x000FE5E0
		public static Settlement GiveStartingFiefs(int count, MBFastRandom random, Func<Settlement, bool> condition = null)
		{
			List<Settlement> list = new List<Settlement>();
			foreach (Town town in Town.AllFiefs)
			{
				if (condition == null || condition(town.Settlement))
				{
					list.Add(town.Settlement);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			Settlement settlement = list[random.Next(0, list.Count)];
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GiveSettlementToMainHero(settlement);
			List<Settlement> list2 = new List<Settlement>();
			HashSet<Settlement> hashSet = new HashSet<Settlement>();
			list2.Add(settlement);
			hashSet.Add(settlement);
			for (int i = 0; i < count - 1; i++)
			{
				Settlement nextFief = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GetNextFief(list2, hashSet, condition, random, 2);
				if (nextFief == null)
				{
					break;
				}
				CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GiveSettlementToMainHero(nextFief);
				list2.Add(nextFief);
				hashSet.Add(nextFief);
			}
			return settlement;
		}

		// Token: 0x06003D79 RID: 15737 RVA: 0x001004CC File Offset: 0x000FE6CC
		private static Settlement GetNextFief(List<Settlement> givenSettlements, HashSet<Settlement> excludedSettlements, Func<Settlement, bool> condition, MBFastRandom random, int searchDepth)
		{
			Dictionary<Settlement, int> dictionary = new Dictionary<Settlement, int>();
			foreach (Settlement settlement in givenSettlements)
			{
				foreach (Settlement settlement2 in settlement.Town.GetNeighborFortifications(MobileParty.NavigationType.All))
				{
					if (!excludedSettlements.Contains(settlement2) && (condition == null || condition(settlement2)))
					{
						if (!dictionary.ContainsKey(settlement2))
						{
							dictionary[settlement2] = 0;
						}
						Dictionary<Settlement, int> dictionary2 = dictionary;
						Settlement settlement3 = settlement2;
						int num = dictionary2[settlement3];
						dictionary2[settlement3] = num + 1;
					}
				}
			}
			if (dictionary.Count > 0)
			{
				int maxScore = dictionary.Values.Max();
				List<Settlement> list = (from kv in dictionary
					where kv.Value == maxScore
					select kv.Key).ToList<Settlement>();
				return list[random.Next(0, list.Count)];
			}
			if (searchDepth <= 0)
			{
				return null;
			}
			List<Settlement> list2 = new List<Settlement>();
			foreach (Settlement settlement4 in givenSettlements)
			{
				foreach (Settlement settlement5 in settlement4.Town.GetNeighborFortifications(MobileParty.NavigationType.All))
				{
					if (!excludedSettlements.Contains(settlement5))
					{
						list2.Add(settlement5);
					}
				}
			}
			if (list2.Count == 0)
			{
				return null;
			}
			return CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GetNextFief(list2, excludedSettlements, condition, random, searchDepth - 1);
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x001006C8 File Offset: 0x000FE8C8
		public static void UpdateMainHeroHomeSettlement(Settlement settlement)
		{
			Hero.MainHero.BornSettlement = settlement;
			Clan.PlayerClan.SetInitialHomeSettlement(settlement);
			Hero.MainHero.UpdateHomeSettlement();
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x001006EC File Offset: 0x000FE8EC
		public static Settlement FindFallbackStartingTown(MBFastRandom random, bool preferPort = false)
		{
			Settlement settlement = null;
			if (preferPort)
			{
				if (Clan.PlayerClan.Kingdom != null)
				{
					List<Town> list = Town.AllTowns.Where<Town>((Town t) => t.Settlement.OwnerClan.Kingdom == Clan.PlayerClan.Kingdom && t.Settlement.HasPort).ToList<Town>();
					if (list.Count > 0)
					{
						settlement = list[random.Next(0, list.Count)].Settlement;
					}
					if (settlement == null)
					{
						List<Town> list2 = Town.AllTowns.Where<Town>((Town t) => t.Settlement.Culture == Clan.PlayerClan.Kingdom.Culture && t.Settlement.HasPort).ToList<Town>();
						if (list2.Count > 0)
						{
							settlement = list2[random.Next(0, list2.Count)].Settlement;
						}
					}
				}
				if (settlement == null)
				{
					List<Town> list3 = Town.AllTowns.Where<Town>((Town t) => t.Settlement.Culture == Hero.MainHero.Culture && t.Settlement.HasPort).ToList<Town>();
					if (list3.Count > 0)
					{
						settlement = list3[random.Next(0, list3.Count)].Settlement;
					}
				}
				if (settlement == null)
				{
					List<Town> list4 = Town.AllTowns.Where<Town>((Town t) => t.Settlement.HasPort).ToList<Town>();
					settlement = list4[random.Next(0, list4.Count)].Settlement;
				}
			}
			else
			{
				if (Clan.PlayerClan.Kingdom != null)
				{
					List<Town> list5 = Town.AllTowns.Where<Town>((Town t) => t.Settlement.OwnerClan.Kingdom == Clan.PlayerClan.Kingdom).ToList<Town>();
					if (list5.Count > 0)
					{
						settlement = list5[random.Next(0, list5.Count)].Settlement;
					}
					if (settlement == null)
					{
						List<Town> list6 = Town.AllTowns.Where<Town>((Town t) => t.Settlement.Culture == Clan.PlayerClan.Kingdom.Culture).ToList<Town>();
						if (list6.Count > 0)
						{
							settlement = list6[random.Next(0, list6.Count)].Settlement;
						}
					}
				}
				if (settlement == null)
				{
					List<Town> list7 = Town.AllTowns.Where<Town>((Town t) => t.Settlement.Culture == Hero.MainHero.Culture).ToList<Town>();
					if (list7.Count > 0)
					{
						settlement = list7[random.Next(0, list7.Count)].Settlement;
					}
				}
				if (settlement == null)
				{
					settlement = Town.AllTowns[random.Next(0, Town.AllTowns.Count)].Settlement;
				}
			}
			return settlement;
		}

		// Token: 0x06003D7C RID: 15740 RVA: 0x00100998 File Offset: 0x000FEB98
		private static void GiveSettlementToMainHero(Settlement settlement)
		{
			if (settlement.IsFortification)
			{
				settlement.Town.OwnerClan = Clan.PlayerClan;
				if (settlement.Town.Governor != null)
				{
					settlement.Town.Governor.GovernorOf.Governor = null;
					settlement.Town.Governor.GovernorOf = null;
				}
			}
			settlement.Party.SetVisualAsDirty();
			foreach (Village village in settlement.BoundVillages)
			{
				village.Settlement.Party.SetVisualAsDirty();
				if (village.VillagerPartyComponent != null)
				{
					foreach (MobileParty mobileParty in MobileParty.All)
					{
						if (mobileParty.MapEvent == null && mobileParty != MobileParty.MainParty && mobileParty.ShortTermTargetParty == village.VillagerPartyComponent.MobileParty && !mobileParty.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
						{
							mobileParty.SetMoveModeHold();
						}
					}
				}
			}
		}

		// Token: 0x06003D7D RID: 15741 RVA: 0x00100AD4 File Offset: 0x000FECD4
		private void GiveTroopsFromTree(List<CharacterObject> baseTroops, Dictionary<int, ValueTuple<int, int>> tierList, MBFastRandom random)
		{
			foreach (KeyValuePair<int, ValueTuple<int, int>> keyValuePair in tierList)
			{
				List<CharacterObject> list = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CollectTroops(baseTroops, keyValuePair.Key);
				if (list.Count == 0)
				{
					list = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CollectTroops(baseTroops, keyValuePair.Key + 1);
				}
				if (list.Count == 0 && keyValuePair.Key > 1)
				{
					list = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CollectTroops(baseTroops, keyValuePair.Key - 1);
				}
				if (list.Count == 0)
				{
					Debug.FailedAssert("Check the troop tree for double check if troop tiers are correct.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\CampaignAdvancedStartingPlayerOptionsCampaignBehavior.cs", "GiveTroopsFromTree", 871);
				}
				else
				{
					int num = random.Next(keyValuePair.Value.Item1, keyValuePair.Value.Item2 + 1);
					for (int i = 0; i < num; i++)
					{
						CharacterObject characterObject = list[random.Next(0, list.Count)];
						MobileParty.MainParty.MemberRoster.AddToCounts(characterObject, 1, false, 0, 0, true, -1);
					}
				}
			}
		}

		// Token: 0x06003D7E RID: 15742 RVA: 0x00100BEC File Offset: 0x000FEDEC
		private static List<CharacterObject> CollectTroops(List<CharacterObject> baseTroops, int tier)
		{
			List<CharacterObject> list = new List<CharacterObject>();
			foreach (CharacterObject characterObject in baseTroops)
			{
				list.AddRange(CharacterHelper.GetTroopTree(characterObject, (float)tier, (float)tier));
			}
			return list;
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x00100C4C File Offset: 0x000FEE4C
		private Kingdom FindEnemyKingdomOrDeclareWar(Kingdom kingdom)
		{
			foreach (Kingdom kingdom2 in Campaign.Current.Kingdoms)
			{
				if (!kingdom2.IsEliminated && kingdom2 != kingdom && FactionManager.IsAtWarAgainstFaction(kingdom, kingdom2))
				{
					return kingdom2;
				}
			}
			Kingdom kingdom3 = null;
			Dictionary<Kingdom, float> dictionary = new Dictionary<Kingdom, float>();
			foreach (Settlement settlement in kingdom.Settlements)
			{
				if (settlement.IsTown || settlement.IsCastle)
				{
					using (List<Settlement>.Enumerator enumerator3 = settlement.Town.GetNeighborFortifications(MobileParty.NavigationType.All).GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							Kingdom kingdom4;
							if ((kingdom4 = enumerator3.Current.MapFaction as Kingdom) != null && kingdom4 != kingdom && !kingdom4.IsEliminated && !dictionary.ContainsKey(kingdom4))
							{
								TextObject textObject;
								float scoreOfDeclaringWar = Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringWar(kingdom, kingdom4, Clan.PlayerClan, out textObject, false);
								dictionary.Add(kingdom4, scoreOfDeclaringWar);
								if (kingdom3 == null || scoreOfDeclaringWar > dictionary[kingdom3])
								{
									kingdom3 = kingdom4;
								}
							}
						}
					}
				}
			}
			FactionManager.DeclareWar(kingdom, kingdom3);
			return kingdom3;
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x00100DCC File Offset: 0x000FEFCC
		private CampaignVec2 GetStartingPositionForMercenary(Kingdom kingdom, Kingdom enemyKingdom, MBFastRandom random)
		{
			Settlement settlement = null;
			Settlement settlement2 = null;
			float num = float.MaxValue;
			foreach (Town town in kingdom.Fiefs)
			{
				foreach (Town town2 in enemyKingdom.Fiefs)
				{
					float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(town.Settlement, town2.Settlement, false, false, MobileParty.NavigationType.Default);
					if (distance < num)
					{
						num = distance;
						settlement = town.Settlement;
						settlement2 = town2.Settlement;
					}
				}
			}
			Vec2 vec = (settlement2.GetPosition2D - settlement.GetPosition2D).Normalized();
			return NavigationHelper.FindPointAroundPosition(new CampaignVec2(settlement.GatePosition.ToVec2() + vec * 2f, true), MobileParty.NavigationType.All, 2f, 0f, false, false);
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x00100EF8 File Offset: 0x000FF0F8
		public static void GiveTradeGoods(CultureObject culture, int goldValueMin, int goldValueMax, int typeCount, MBFastRandom random)
		{
			List<ItemObject> list = new List<ItemObject>();
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsVillage && settlement.Culture == culture)
				{
					foreach (ValueTuple<ItemObject, float> valueTuple in settlement.Village.VillageType.Productions)
					{
						ItemObject item = valueTuple.Item1;
						if (item != null && !item.IsFood && !item.IsMountable && !item.IsAnimal && !item.HasHorseComponent && item.Value > 0 && !list.Contains(item))
						{
							list.Add(item);
						}
					}
				}
			}
			ItemObject[] array = new ItemObject[(typeCount > list.Count) ? list.Count : typeCount];
			for (int i = 0; i < array.Length; i++)
			{
				ItemObject itemObject = list[random.Next(0, list.Count)];
				array[i] = itemObject;
				list.Remove(itemObject);
			}
			int num = random.Next(goldValueMin, goldValueMax + 1);
			float[] array2 = new float[array.Length];
			float num2 = 0f;
			for (int j = 0; j < array.Length; j++)
			{
				array2[j] = random.NextFloat();
				num2 += array2[j];
			}
			int[] array3 = new int[array.Length];
			for (int k = 0; k < array.Length; k++)
			{
				int num3 = (int)((float)num * (array2[k] / num2));
				array3[k] = num3 / array[k].Value;
			}
			MobileParty mainParty = MobileParty.MainParty;
			float num4 = (float)mainParty.InventoryCapacity - mainParty.TotalWeightCarried;
			float num5 = 0f;
			for (int l = 0; l < array.Length; l++)
			{
				num5 += (float)array3[l] * array[l].Weight;
			}
			if (num5 > num4 && num5 > 0f)
			{
				float num6 = num4 / num5;
				for (int m = 0; m < array.Length; m++)
				{
					array3[m] = (int)((float)array3[m] * num6);
				}
			}
			for (int n = 0; n < array.Length; n++)
			{
				if (array3[n] > 0)
				{
					mainParty.ItemRoster.AddToCounts(new EquipmentElement(array[n], null, null, false), array3[n]);
				}
			}
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x0010117C File Offset: 0x000FF37C
		public static void GiveStartingCompanions(int count, MBFastRandom random)
		{
			Dictionary<CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType, List<Hero>> dictionary = new Dictionary<CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType, List<Hero>>();
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (hero.IsWanderer && hero.IsActive && hero.CompanionOf == null)
				{
					CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType companionType = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GetCompanionType(hero);
					if (!dictionary.ContainsKey(companionType))
					{
						dictionary[companionType] = new List<Hero>();
					}
					dictionary[companionType].Add(hero);
				}
			}
			List<CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType> list = new List<CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType>(dictionary.Keys);
			HashSet<CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType> hashSet = new HashSet<CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType>();
			int num = 0;
			while (num < count && list.Count != 0)
			{
				if (hashSet.Count == list.Count)
				{
					hashSet.Clear();
				}
				List<CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType> list2 = new List<CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType>();
				foreach (CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType companionType2 in list)
				{
					if (!hashSet.Contains(companionType2))
					{
						list2.Add(companionType2);
					}
				}
				CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType companionType3 = list2[random.Next(0, list2.Count)];
				hashSet.Add(companionType3);
				List<Hero> list3 = dictionary[companionType3];
				Hero hero2 = list3[random.Next(0, list3.Count)];
				list3.Remove(hero2);
				if (list3.Count == 0)
				{
					dictionary.Remove(companionType3);
					list.Remove(companionType3);
					hashSet.Remove(companionType3);
				}
				hero2.CompanionOf = Clan.PlayerClan;
				hero2.StayingInSettlement = null;
				hero2.SetHasMet();
				MobileParty.MainParty.AddElementToMemberRoster(hero2.CharacterObject, 1, false);
				num++;
			}
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x00101350 File Offset: 0x000FF550
		private static CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType GetCompanionTypeForSkill(SkillObject skill)
		{
			if (skill == DefaultSkills.Engineering)
			{
				return CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType.Engineering;
			}
			if (skill == DefaultSkills.Tactics)
			{
				return CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType.Tactics;
			}
			if (skill == DefaultSkills.Leadership)
			{
				return CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType.Leadership;
			}
			if (skill == DefaultSkills.Steward)
			{
				return CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType.Steward;
			}
			if (skill == DefaultSkills.Trade)
			{
				return CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType.Trade;
			}
			if (skill == DefaultSkills.Roguery)
			{
				return CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType.Roguery;
			}
			if (skill == DefaultSkills.Medicine)
			{
				return CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType.Medicine;
			}
			if (skill == DefaultSkills.Crafting)
			{
				return CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType.Smithing;
			}
			if (skill == DefaultSkills.Scouting)
			{
				return CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType.Scouting;
			}
			return CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType.Combat;
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x001013BC File Offset: 0x000FF5BC
		private static CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType GetCompanionType(Hero hero)
		{
			if (hero.CharacterObject.IsMariner)
			{
				return CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType.Sailor;
			}
			CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType companionType = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType.Combat;
			int num = 20;
			foreach (SkillObject skillObject in Skills.All)
			{
				int skillValue = hero.GetSkillValue(skillObject);
				if (skillValue > num)
				{
					CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType companionTypeForSkill = CampaignAdvancedStartingPlayerOptionsCampaignBehavior.GetCompanionTypeForSkill(skillObject);
					if (companionTypeForSkill != CampaignAdvancedStartingPlayerOptionsCampaignBehavior.CompanionType.Combat)
					{
						num = skillValue;
						companionType = companionTypeForSkill;
					}
				}
			}
			return companionType;
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x00101440 File Offset: 0x000FF640
		public static void EnsureMinimumClanTier(int minimumTier)
		{
			if (Clan.PlayerClan.Tier < minimumTier)
			{
				float num = (float)Campaign.Current.Models.ClanTierModel.GetRequiredRenownForTier(minimumTier) - Clan.PlayerClan.Renown;
				if (num > 0f)
				{
					Clan.PlayerClan.AddRenown(num, false);
				}
			}
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x00101490 File Offset: 0x000FF690
		public static Kingdom ResolveKingdom(string kingdomId)
		{
			Kingdom kingdom = Campaign.Current.Kingdoms.FirstOrDefaultQ<Kingdom>((Kingdom k) => k.StringId.Equals(kingdomId));
			if (Campaign.Current.AdvancedStartData.GetScenario() == "unitedempire" && kingdom.Culture.StringId == "empire")
			{
				return Campaign.Current.Kingdoms.FirstOrDefaultQ<Kingdom>((Kingdom k) => k.StringId.Equals("calradian_empire"));
			}
			return kingdom;
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x00101528 File Offset: 0x000FF728
		public static void AdjustStartingFood(MBFastRandom random, int minFoodAmount, int maxFoodAmount, CultureObject culture)
		{
			List<ItemObject> list = new List<ItemObject>();
			foreach (Village village in Village.All)
			{
				if (village.Bound.Culture == culture)
				{
					foreach (ValueTuple<ItemObject, float> valueTuple in village.VillageType.Productions)
					{
						if (valueTuple.Item1.IsFood && !list.Contains(valueTuple.Item1))
						{
							list.Add(valueTuple.Item1);
						}
					}
				}
			}
			int num = random.Next(minFoodAmount, maxFoodAmount + 1);
			int num2 = random.Next(1, list.Count + 1);
			int num3 = num / num2;
			int num4 = MathF.Max(1, num3 / 4);
			int num5 = num;
			for (int i = 0; i < num2; i++)
			{
				int num6;
				if (i < num2 - 1)
				{
					num6 = num3 + random.Next(-num4, num4 + 1);
				}
				else
				{
					num6 = num5;
				}
				num5 -= num6;
				if (num6 > 0)
				{
					int num7 = random.Next(0, list.Count);
					ItemObject itemObject = list[num7];
					list.RemoveAt(num7);
					MobileParty.MainParty.ItemRoster.AddToCounts(itemObject, num6);
				}
			}
		}

		// Token: 0x040012E8 RID: 4840
		private const int CompanionSkillThreshold = 20;

		// Token: 0x040012E9 RID: 4841
		private const uint KingSeed = 3324857891U;

		// Token: 0x040012EA RID: 4842
		private const uint VassalSeed = 753149491U;

		// Token: 0x040012EB RID: 4843
		private const uint MercenarySeed = 1160783149U;

		// Token: 0x040012EC RID: 4844
		private const uint TraderSeed = 1374241951U;

		// Token: 0x040012ED RID: 4845
		private const uint OutlawSeed = 4060451693U;

		// Token: 0x040012EE RID: 4846
		private const uint BeggarSeed = 2508482539U;

		// Token: 0x040012EF RID: 4847
		private const int RulerMinimumClanTier = 5;

		// Token: 0x040012F0 RID: 4848
		private const int RulerStartingSettlementCount = 2;

		// Token: 0x040012F1 RID: 4849
		private const int RulerLastStandStartingSettlementCount = 1;

		// Token: 0x040012F2 RID: 4850
		private const int RulerStartingCompanionCount = 4;

		// Token: 0x040012F3 RID: 4851
		private const int RulerStartingGoldMin = 100000;

		// Token: 0x040012F4 RID: 4852
		private const int RulerStartingGoldMax = 150000;

		// Token: 0x040012F5 RID: 4853
		private const int RulerStartingFoodMin = 100;

		// Token: 0x040012F6 RID: 4854
		private const int RulerStartingFoodMax = 200;

		// Token: 0x040012F7 RID: 4855
		private const int RulerStartingInfluenceMin = 75;

		// Token: 0x040012F8 RID: 4856
		private const int RulerStartingInfluenceMax = 150;

		// Token: 0x040012F9 RID: 4857
		[TupleElementNames(new string[] { "min", "max" })]
		private static readonly Dictionary<int, ValueTuple<int, int>> RulerEliteTroopCounts = new Dictionary<int, ValueTuple<int, int>> { 
		{
			4,
			new ValueTuple<int, int>(10, 15)
		} };

		// Token: 0x040012FA RID: 4858
		[TupleElementNames(new string[] { "min", "max" })]
		private static readonly Dictionary<int, ValueTuple<int, int>> RulerNormalTroopCounts = new Dictionary<int, ValueTuple<int, int>>
		{
			{
				1,
				new ValueTuple<int, int>(40, 51)
			},
			{
				2,
				new ValueTuple<int, int>(25, 35)
			},
			{
				3,
				new ValueTuple<int, int>(30, 45)
			},
			{
				5,
				new ValueTuple<int, int>(10, 15)
			}
		};

		// Token: 0x040012FB RID: 4859
		private const int VassalMinimumClanTier = 2;

		// Token: 0x040012FC RID: 4860
		private const int VassalStartingCastleCount = 1;

		// Token: 0x040012FD RID: 4861
		private const int VassalStartingCompanionCount = 2;

		// Token: 0x040012FE RID: 4862
		private const int VassalStartingGoldMin = 25000;

		// Token: 0x040012FF RID: 4863
		private const int VassalStartingGoldMax = 75000;

		// Token: 0x04001300 RID: 4864
		private const int VassalStartingFoodMin = 75;

		// Token: 0x04001301 RID: 4865
		private const int VassalStartingFoodMax = 100;

		// Token: 0x04001302 RID: 4866
		[TupleElementNames(new string[] { "min", "max" })]
		private static readonly Dictionary<int, ValueTuple<int, int>> VassalEliteTroopCounts = new Dictionary<int, ValueTuple<int, int>> { 
		{
			1,
			new ValueTuple<int, int>(5, 10)
		} };

		// Token: 0x04001303 RID: 4867
		[TupleElementNames(new string[] { "min", "max" })]
		private static readonly Dictionary<int, ValueTuple<int, int>> VassalNormalTroopCounts = new Dictionary<int, ValueTuple<int, int>>
		{
			{
				1,
				new ValueTuple<int, int>(15, 19)
			},
			{
				2,
				new ValueTuple<int, int>(15, 19)
			},
			{
				3,
				new ValueTuple<int, int>(5, 10)
			},
			{
				4,
				new ValueTuple<int, int>(5, 10)
			}
		};

		// Token: 0x04001304 RID: 4868
		private const int MercenaryAwardMultiplier = 50;

		// Token: 0x04001305 RID: 4869
		private const int MercenaryMinimumClanTier = 1;

		// Token: 0x04001306 RID: 4870
		private const int MercenaryStartingCompanionCount = 1;

		// Token: 0x04001307 RID: 4871
		private const int MercenaryStartingGoldMin = 10000;

		// Token: 0x04001308 RID: 4872
		private const int MercenaryStartingGoldMax = 15000;

		// Token: 0x04001309 RID: 4873
		private const int MercenaryStartingFoodMin = 30;

		// Token: 0x0400130A RID: 4874
		private const int MercenaryStartingFoodMax = 50;

		// Token: 0x0400130B RID: 4875
		[TupleElementNames(new string[] { "min", "max" })]
		private static readonly Dictionary<int, ValueTuple<int, int>> MercenaryTroopCounts = new Dictionary<int, ValueTuple<int, int>>
		{
			{
				1,
				new ValueTuple<int, int>(15, 20)
			},
			{
				2,
				new ValueTuple<int, int>(10, 15)
			},
			{
				3,
				new ValueTuple<int, int>(5, 9)
			}
		};

		// Token: 0x0400130C RID: 4876
		private const int TraderClanTier = 1;

		// Token: 0x0400130D RID: 4877
		private const int TraderStartingCompanionCount = 2;

		// Token: 0x0400130E RID: 4878
		private const int TraderStartingGoldMin = 5000;

		// Token: 0x0400130F RID: 4879
		private const int TraderStartingGoldMax = 10000;

		// Token: 0x04001310 RID: 4880
		private const string MuleStringId = "mule";

		// Token: 0x04001311 RID: 4881
		private const int TraderStartingFoodMin = 30;

		// Token: 0x04001312 RID: 4882
		private const int TraderStartingFoodMax = 50;

		// Token: 0x04001313 RID: 4883
		private const int TraderTradeGoodsGoldMin = 1500;

		// Token: 0x04001314 RID: 4884
		private const int TraderTradeGoodsGoldMax = 5000;

		// Token: 0x04001315 RID: 4885
		private const int TraderTradeGoodTypeCount = 1;

		// Token: 0x04001316 RID: 4886
		private const int TraderMuleCountMin = 3;

		// Token: 0x04001317 RID: 4887
		private const int TraderMuleCountMax = 5;

		// Token: 0x04001318 RID: 4888
		private const int OutlawStartingCompanionCount = 1;

		// Token: 0x04001319 RID: 4889
		private const int OutlawStartingGoldMin = 2000;

		// Token: 0x0400131A RID: 4890
		private const int OutlawStartingGoldMax = 5000;

		// Token: 0x0400131B RID: 4891
		private const int OutlawStartingFoodMin = 3;

		// Token: 0x0400131C RID: 4892
		private const int OutlawStartingFoodMax = 7;

		// Token: 0x0400131D RID: 4893
		private const int OutlawTroopCountMin = 10;

		// Token: 0x0400131E RID: 4894
		private const int OutlawTroopCountMax = 15;

		// Token: 0x0400131F RID: 4895
		private const float OutlawCrimeRating = 45f;

		// Token: 0x04001320 RID: 4896
		private const int OutlawPlunderGoldMin = 1500;

		// Token: 0x04001321 RID: 4897
		private const int OutlawPlunderGoldMax = 5000;

		// Token: 0x04001322 RID: 4898
		private const int OutlawTradeGoodTypeCountMin = 3;

		// Token: 0x04001323 RID: 4899
		private const int OutlawTradeGoodTypeCountMax = 5;

		// Token: 0x02000802 RID: 2050
		private enum CompanionType
		{
			// Token: 0x040020B8 RID: 8376
			Engineering,
			// Token: 0x040020B9 RID: 8377
			Tactics,
			// Token: 0x040020BA RID: 8378
			Leadership,
			// Token: 0x040020BB RID: 8379
			Steward,
			// Token: 0x040020BC RID: 8380
			Trade,
			// Token: 0x040020BD RID: 8381
			Roguery,
			// Token: 0x040020BE RID: 8382
			Medicine,
			// Token: 0x040020BF RID: 8383
			Smithing,
			// Token: 0x040020C0 RID: 8384
			Scouting,
			// Token: 0x040020C1 RID: 8385
			Combat,
			// Token: 0x040020C2 RID: 8386
			Sailor
		}
	}
}
