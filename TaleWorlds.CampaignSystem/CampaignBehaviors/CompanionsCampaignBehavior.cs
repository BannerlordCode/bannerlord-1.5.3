using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003FF RID: 1023
	public class CompanionsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x06003FEC RID: 16364 RVA: 0x00116690 File Offset: 0x00114890
		private float _desiredTotalCompanionCount
		{
			get
			{
				return (float)Town.AllTowns.Count * 0.6f;
			}
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x001166A4 File Offset: 0x001148A4
		public override void RegisterEvents()
		{
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.HeroOccupationChangedEvent.AddNonSerializedListener(this, new Action<Hero, Occupation>(this.OnHeroOccupationChanged));
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, new Action(this.WeeklyTick));
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x00116754 File Offset: 0x00114954
		private void OnGameLoadFinished()
		{
			this.InitializeCompanionTemplateList();
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (hero.IsWanderer)
				{
					this.AddToAliveCompanions(hero, false);
				}
			}
			foreach (Hero hero2 in Hero.DeadOrDisabledHeroes)
			{
				if (hero2.IsAlive && hero2.IsWanderer)
				{
					this.AddToAliveCompanions(hero2, false);
				}
			}
		}

		// Token: 0x06003FEF RID: 16367 RVA: 0x00116808 File Offset: 0x00114A08
		private void DailyTick()
		{
			this.TryKillCompanion();
			this.SwapCompanions();
			this.TrySpawnNewCompanion();
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x0011681C File Offset: 0x00114A1C
		private void WeeklyTick()
		{
			foreach (Hero hero in Hero.DeadOrDisabledHeroes.ToList<Hero>())
			{
				if (hero.IsWanderer && hero.DeathDay.ElapsedDaysUntilNow >= 40f)
				{
					Campaign.Current.CampaignObjectManager.UnregisterDeadHero(hero);
				}
			}
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x0011689C File Offset: 0x00114A9C
		private void RemoveFromAliveCompanions(Hero companion)
		{
			CharacterObject template = companion.Template;
			if (this._aliveCompanionTemplates.Contains(template))
			{
				this._aliveCompanionTemplates.Remove(template);
			}
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x001168CC File Offset: 0x00114ACC
		private void AddToAliveCompanions(Hero companion, bool isTemplateControlled = false)
		{
			CharacterObject template = companion.Template;
			bool flag = true;
			if (!isTemplateControlled)
			{
				flag = this.IsTemplateKnown(template);
			}
			if (flag && !this._aliveCompanionTemplates.Contains(template))
			{
				this._aliveCompanionTemplates.Add(template);
			}
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x0011690B File Offset: 0x00114B0B
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			this.RemoveFromAliveCompanions(victim);
			if (victim.IsWanderer && !victim.HasMet)
			{
				Campaign.Current.CampaignObjectManager.UnregisterDeadHero(victim);
			}
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x00116934 File Offset: 0x00114B34
		private void OnHeroOccupationChanged(Hero hero, Occupation oldOccupation)
		{
			if (oldOccupation == Occupation.Wanderer)
			{
				this.RemoveFromAliveCompanions(hero);
				return;
			}
			if (hero.Occupation == Occupation.Wanderer)
			{
				this.AddToAliveCompanions(hero, false);
			}
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x00116955 File Offset: 0x00114B55
		private void OnHeroCreated(Hero hero, bool showNotification = true)
		{
			if (hero.IsAlive && hero.IsWanderer)
			{
				this.AddToAliveCompanions(hero, true);
			}
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x00116970 File Offset: 0x00114B70
		private void TryKillCompanion()
		{
			if (MBRandom.RandomFloat <= 0.1f && this._aliveCompanionTemplates.Count > 0)
			{
				CharacterObject randomElementInefficiently = this._aliveCompanionTemplates.GetRandomElementInefficiently<CharacterObject>();
				Hero hero = null;
				foreach (Hero hero2 in Hero.AllAliveHeroes)
				{
					if (hero2.Template == randomElementInefficiently && hero2.IsWanderer)
					{
						hero = hero2;
						break;
					}
				}
				if (hero != null && hero.CompanionOf == null && (hero.CurrentSettlement == null || hero.CurrentSettlement != Hero.MainHero.CurrentSettlement))
				{
					KillCharacterAction.ApplyByRemove(hero, false, true);
				}
			}
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x00116A28 File Offset: 0x00114C28
		private void TrySpawnNewCompanion()
		{
			if ((float)this._aliveCompanionTemplates.Count < this._desiredTotalCompanionCount)
			{
				Town randomElementWithPredicate = Town.AllTowns.GetRandomElementWithPredicate<Town>(delegate(Town x)
				{
					if (x.Settlement != Hero.MainHero.CurrentSettlement && x.Settlement.SiegeEvent == null)
					{
						return x.Settlement.HeroesWithoutParty.AllQ<Hero>((Hero y) => !y.IsWanderer || y.CompanionOf != null);
					}
					return false;
				});
				Settlement settlement = ((randomElementWithPredicate != null) ? randomElementWithPredicate.Settlement : null);
				if (settlement != null)
				{
					this.CreateCompanionAndAddToSettlement(settlement);
				}
			}
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x00116A8C File Offset: 0x00114C8C
		private void SwapCompanions()
		{
			int num = Town.AllTowns.Count / 2;
			int num2 = MBRandom.RandomInt(Town.AllTowns.Count % 2);
			Town town = Town.AllTowns[num2 + MBRandom.RandomInt(num)];
			Hero hero = town.Settlement.HeroesWithoutParty.Where<Hero>((Hero x) => x.IsWanderer && x.CompanionOf == null).GetRandomElementInefficiently<Hero>();
			for (int i = 1; i < 2; i++)
			{
				Town town2 = Town.AllTowns[i * num + num2 + MBRandom.RandomInt(num)];
				IEnumerable<Hero> enumerable = town2.Settlement.HeroesWithoutParty.Where<Hero>((Hero x) => x.IsWanderer && x.CompanionOf == null);
				Hero hero2 = null;
				if (enumerable.Any<Hero>())
				{
					hero2 = enumerable.GetRandomElementInefficiently<Hero>();
					LeaveSettlementAction.ApplyForCharacterOnly(hero2);
				}
				if (hero != null)
				{
					EnterSettlementAction.ApplyForCharacterOnly(hero, town2.Settlement);
				}
				hero = hero2;
			}
			if (hero != null)
			{
				EnterSettlementAction.ApplyForCharacterOnly(hero, town.Settlement);
			}
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x00116B9F File Offset: 0x00114D9F
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x00116BA4 File Offset: 0x00114DA4
		private void OnNewGameCreated(CampaignGameStarter starter)
		{
			this.InitializeCompanionTemplateList();
			List<Town> list = Town.AllTowns.ToListQ<Town>();
			list.Shuffle<Town>();
			int num = 0;
			while ((float)num < this._desiredTotalCompanionCount)
			{
				this.CreateCompanionAndAddToSettlement(list[num].Settlement);
				num++;
			}
		}

		// Token: 0x06003FFB RID: 16379 RVA: 0x00116BEC File Offset: 0x00114DEC
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeCompanionTemplateList();
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (hero.IsWanderer)
				{
					this.AddToAliveCompanions(hero, false);
				}
			}
			foreach (Hero hero2 in Hero.DeadOrDisabledHeroes)
			{
				if (hero2.IsAlive && hero2.IsWanderer)
				{
					this.AddToAliveCompanions(hero2, false);
				}
			}
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x00116CA0 File Offset: 0x00114EA0
		private void AdjustEquipments(Hero hero)
		{
			this.AdjustEquipmentModifiers(hero.BattleEquipment);
			this.AdjustEquipmentModifiers(hero.CivilianEquipment);
		}

		// Token: 0x06003FFD RID: 16381 RVA: 0x00116CBC File Offset: 0x00114EBC
		private void AdjustEquipmentModifiers(Equipment equipment)
		{
			ItemModifier @object = MBObjectManager.Instance.GetObject<ItemModifier>("companion_armor");
			ItemModifier object2 = MBObjectManager.Instance.GetObject<ItemModifier>("companion_weapon");
			ItemModifier object3 = MBObjectManager.Instance.GetObject<ItemModifier>("companion_horse");
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
			{
				EquipmentElement equipmentElement = equipment[equipmentIndex];
				if (equipmentElement.Item != null)
				{
					if (equipmentElement.Item.ArmorComponent != null)
					{
						equipment[equipmentIndex] = new EquipmentElement(equipmentElement.Item, @object, null, false);
					}
					else if (equipmentElement.Item.HorseComponent != null)
					{
						equipment[equipmentIndex] = new EquipmentElement(equipmentElement.Item, object3, null, false);
					}
					else if (equipmentElement.Item.WeaponComponent != null)
					{
						equipment[equipmentIndex] = new EquipmentElement(equipmentElement.Item, object2, null, false);
					}
				}
			}
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x00116D90 File Offset: 0x00114F90
		private void InitializeCompanionTemplateList()
		{
			foreach (CharacterObject characterObject in MBObjectManager.Instance.GetObjectTypeList<CharacterObject>())
			{
				if (characterObject.IsTemplate && characterObject.Occupation == Occupation.Wanderer)
				{
					this._companionsOfTemplates[this.GetTemplateTypeOfCompanion(characterObject)].Add(characterObject);
				}
			}
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x00116E0C File Offset: 0x0011500C
		private CompanionsCampaignBehavior.CompanionTemplateType GetTemplateTypeOfCompanion(CharacterObject character)
		{
			if (character.IsMariner)
			{
				return CompanionsCampaignBehavior.CompanionTemplateType.Sailor;
			}
			CompanionsCampaignBehavior.CompanionTemplateType companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Combat;
			int num = 20;
			foreach (SkillObject skillObject in Skills.All)
			{
				int skillValue = character.GetSkillValue(skillObject);
				if (skillValue > num)
				{
					CompanionsCampaignBehavior.CompanionTemplateType templateTypeForSkill = this.GetTemplateTypeForSkill(skillObject);
					if (templateTypeForSkill != CompanionsCampaignBehavior.CompanionTemplateType.Combat)
					{
						num = skillValue;
						companionTemplateType = templateTypeForSkill;
					}
				}
			}
			return companionTemplateType;
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x00116E8C File Offset: 0x0011508C
		private void CreateCompanionAndAddToSettlement(Settlement settlement)
		{
			CharacterObject companionTemplate = this.GetCompanionTemplateToSpawn();
			if (companionTemplate != null)
			{
				Town randomElementWithPredicate = Town.AllTowns.GetRandomElementWithPredicate<Town>((Town x) => x.Culture == companionTemplate.Culture);
				Settlement settlement2 = ((randomElementWithPredicate != null) ? randomElementWithPredicate.Settlement : null);
				if (settlement2 == null)
				{
					settlement2 = Town.AllTowns.GetRandomElement<Town>().Settlement;
				}
				Hero hero = HeroCreator.CreateSpecialHero(companionTemplate, settlement2, null, null, Campaign.Current.Models.AgeModel.HeroComesOfAge + 5 + MBRandom.RandomInt(12));
				this.AdjustEquipments(hero);
				hero.ChangeState(Hero.CharacterStates.Active);
				EnterSettlementAction.ApplyForCharacterOnly(hero, settlement);
			}
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x00116F2C File Offset: 0x0011512C
		private CompanionsCampaignBehavior.CompanionTemplateType GetCompanionTemplateTypeToSpawn()
		{
			CompanionsCampaignBehavior.CompanionTemplateType companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Combat;
			float num = -1f;
			foreach (KeyValuePair<CompanionsCampaignBehavior.CompanionTemplateType, List<CharacterObject>> keyValuePair in this._companionsOfTemplates)
			{
				float templateTypeScore = this.GetTemplateTypeScore(keyValuePair.Key);
				if (templateTypeScore > 0f)
				{
					int num2 = 0;
					foreach (CharacterObject characterObject in keyValuePair.Value)
					{
						if (this._aliveCompanionTemplates.Contains(characterObject))
						{
							num2++;
						}
					}
					float num3 = (float)num2 / this._desiredTotalCompanionCount;
					float num4 = (templateTypeScore - num3) / templateTypeScore;
					if (num2 < keyValuePair.Value.Count && num4 > num)
					{
						num = num4;
						companionTemplateType = keyValuePair.Key;
					}
				}
			}
			return companionTemplateType;
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x0011702C File Offset: 0x0011522C
		private bool IsTemplateKnown(CharacterObject companionTemplate)
		{
			foreach (KeyValuePair<CompanionsCampaignBehavior.CompanionTemplateType, List<CharacterObject>> keyValuePair in this._companionsOfTemplates)
			{
				for (int i = 0; i < keyValuePair.Value.Count; i++)
				{
					if (companionTemplate == keyValuePair.Value[i])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x001170A0 File Offset: 0x001152A0
		private CharacterObject GetCompanionTemplateToSpawn()
		{
			List<CharacterObject> list = this._companionsOfTemplates[this.GetCompanionTemplateTypeToSpawn()];
			list.Shuffle<CharacterObject>();
			CharacterObject characterObject = null;
			foreach (CharacterObject characterObject2 in list)
			{
				if (!this._aliveCompanionTemplates.Contains(characterObject2))
				{
					characterObject = characterObject2;
					break;
				}
			}
			return characterObject;
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x00117114 File Offset: 0x00115314
		private float GetTemplateTypeScore(CompanionsCampaignBehavior.CompanionTemplateType templateType)
		{
			switch (templateType)
			{
			case CompanionsCampaignBehavior.CompanionTemplateType.Engineering:
				return 0.05263158f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Tactics:
				return 0.10526316f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Leadership:
				return 0.078947365f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Steward:
				return 0.078947365f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Trade:
				return 0.078947365f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Roguery:
				return 0.10526316f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Medicine:
				return 0.078947365f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Smithing:
				return 0.05263158f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Scouting:
				return 0.13157895f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Combat:
				return 0.13157895f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Sailor:
				return 0.10526316f;
			default:
				return 0f;
			}
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x0011719C File Offset: 0x0011539C
		private CompanionsCampaignBehavior.CompanionTemplateType GetTemplateTypeForSkill(SkillObject skill)
		{
			CompanionsCampaignBehavior.CompanionTemplateType companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Combat;
			if (skill == DefaultSkills.Engineering)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Engineering;
			}
			else if (skill == DefaultSkills.Tactics)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Tactics;
			}
			else if (skill == DefaultSkills.Leadership)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Leadership;
			}
			else if (skill == DefaultSkills.Steward)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Steward;
			}
			else if (skill == DefaultSkills.Trade)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Trade;
			}
			else if (skill == DefaultSkills.Roguery)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Roguery;
			}
			else if (skill == DefaultSkills.Medicine)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Medicine;
			}
			else if (skill == DefaultSkills.Crafting)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Smithing;
			}
			else if (skill == DefaultSkills.Scouting)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Scouting;
			}
			return companionTemplateType;
		}

		// Token: 0x04001373 RID: 4979
		private const int CompanionMoveRandomIndex = 2;

		// Token: 0x04001374 RID: 4980
		private const float DesiredCompanionPerTown = 0.6f;

		// Token: 0x04001375 RID: 4981
		private const float KillChance = 0.1f;

		// Token: 0x04001376 RID: 4982
		private const int SkillThresholdValue = 20;

		// Token: 0x04001377 RID: 4983
		private const int RemoveWandererAfterDays = 40;

		// Token: 0x04001378 RID: 4984
		private IReadOnlyDictionary<CompanionsCampaignBehavior.CompanionTemplateType, List<CharacterObject>> _companionsOfTemplates = new Dictionary<CompanionsCampaignBehavior.CompanionTemplateType, List<CharacterObject>>
		{
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Engineering,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Tactics,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Leadership,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Steward,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Trade,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Roguery,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Medicine,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Smithing,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Scouting,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Combat,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Sailor,
				new List<CharacterObject>()
			}
		};

		// Token: 0x04001379 RID: 4985
		private HashSet<CharacterObject> _aliveCompanionTemplates = new HashSet<CharacterObject>();

		// Token: 0x0400137A RID: 4986
		private const float EngineerScore = 2f;

		// Token: 0x0400137B RID: 4987
		private const float TacticsScore = 4f;

		// Token: 0x0400137C RID: 4988
		private const float LeadershipScore = 3f;

		// Token: 0x0400137D RID: 4989
		private const float StewardScore = 3f;

		// Token: 0x0400137E RID: 4990
		private const float TradeScore = 3f;

		// Token: 0x0400137F RID: 4991
		private const float RogueryScore = 4f;

		// Token: 0x04001380 RID: 4992
		private const float MedicineScore = 3f;

		// Token: 0x04001381 RID: 4993
		private const float SmithingScore = 2f;

		// Token: 0x04001382 RID: 4994
		private const float ScoutingScore = 5f;

		// Token: 0x04001383 RID: 4995
		private const float CombatScore = 5f;

		// Token: 0x04001384 RID: 4996
		private const float SailorScore = 4f;

		// Token: 0x04001385 RID: 4997
		private const float AllScore = 38f;

		// Token: 0x02000820 RID: 2080
		private enum CompanionTemplateType
		{
			// Token: 0x0400212B RID: 8491
			Engineering,
			// Token: 0x0400212C RID: 8492
			Tactics,
			// Token: 0x0400212D RID: 8493
			Leadership,
			// Token: 0x0400212E RID: 8494
			Steward,
			// Token: 0x0400212F RID: 8495
			Trade,
			// Token: 0x04002130 RID: 8496
			Roguery,
			// Token: 0x04002131 RID: 8497
			Medicine,
			// Token: 0x04002132 RID: 8498
			Smithing,
			// Token: 0x04002133 RID: 8499
			Scouting,
			// Token: 0x04002134 RID: 8500
			Combat,
			// Token: 0x04002135 RID: 8501
			Sailor
		}
	}
}
