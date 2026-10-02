using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003E9 RID: 1001
	public class AgingCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003C5F RID: 15455 RVA: 0x000F62B0 File Offset: 0x000F44B0
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action<int>(this.OnCharacterCreationIsOver));
			CampaignEvents.HeroComesOfAgeEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroComesOfAge));
			CampaignEvents.HeroReachesTeenAgeEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroReachesTeenAge));
			CampaignEvents.HeroGrowsOutOfInfancyEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroGrowsOutOfInfancy));
			CampaignEvents.PerkOpenedEvent.AddNonSerializedListener(this, new Action<Hero, PerkObject>(this.OnPerkOpened));
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x000F63A3 File Offset: 0x000F45A3
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Hero, int>>("_extraLivesContainer", ref this._extraLivesContainer);
			dataStore.SyncData<Dictionary<Hero, int>>("_heroesYoungerThanHeroComesOfAge", ref this._heroesYoungerThanHeroComesOfAge);
		}

		// Token: 0x06003C61 RID: 15457 RVA: 0x000F63CC File Offset: 0x000F45CC
		private void OnHeroCreated(Hero hero, bool isBornNaturally)
		{
			int num = (int)hero.Age;
			if (num < Campaign.Current.Models.AgeModel.HeroComesOfAge)
			{
				this._heroesYoungerThanHeroComesOfAge.Add(hero, num);
			}
		}

		// Token: 0x06003C62 RID: 15458 RVA: 0x000F6405 File Offset: 0x000F4605
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			if (this._heroesYoungerThanHeroComesOfAge.ContainsKey(victim))
			{
				this._heroesYoungerThanHeroComesOfAge.Remove(victim);
			}
		}

		// Token: 0x06003C63 RID: 15459 RVA: 0x000F6422 File Offset: 0x000F4622
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.IsPlayerMapEvent && Hero.MainHero.DeathMark == KillCharacterAction.KillCharacterActionDetail.DiedInBattle)
			{
				KillCharacterAction.ApplyByDeathMark(Hero.MainHero, false);
			}
		}

		// Token: 0x06003C64 RID: 15460 RVA: 0x000F6444 File Offset: 0x000F4644
		private void AddExtraLife(Hero hero)
		{
			if (hero.IsAlive)
			{
				if (this._extraLivesContainer.ContainsKey(hero))
				{
					Dictionary<Hero, int> extraLivesContainer = this._extraLivesContainer;
					extraLivesContainer[hero]++;
					return;
				}
				this._extraLivesContainer.Add(hero, 1);
			}
		}

		// Token: 0x06003C65 RID: 15461 RVA: 0x000F6490 File Offset: 0x000F4690
		private void OnPerkOpened(Hero hero, PerkObject perk)
		{
			if (perk == DefaultPerks.Medicine.CheatDeath)
			{
				this.AddExtraLife(hero);
			}
			if (perk == DefaultPerks.Medicine.HealthAdvise)
			{
				Clan clan = hero.Clan;
				if (((clan != null) ? clan.Leader : null) == hero)
				{
					foreach (Hero hero2 in hero.Clan.Heroes)
					{
						if (hero2.IsAlive)
						{
							this.AddExtraLife(hero2);
						}
					}
				}
			}
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x000F651C File Offset: 0x000F471C
		private void DailyTickHero(Hero hero)
		{
			bool flag = (int)CampaignTime.Now.ToDays == this._gameStartDay;
			if (!CampaignOptions.IsLifeDeathCycleDisabled && !flag && !hero.IsTemplate)
			{
				if (hero.IsAlive && hero.CanDie(KillCharacterAction.KillCharacterActionDetail.DiedOfOldAge))
				{
					if (hero.DeathMark != KillCharacterAction.KillCharacterActionDetail.None && hero.DeathMark != KillCharacterAction.KillCharacterActionDetail.ExecutionAfterMapEvent && (hero.PartyBelongedTo == null || (hero.PartyBelongedTo.MapEvent == null && hero.PartyBelongedTo.SiegeEvent == null)))
					{
						KillCharacterAction.ApplyByDeathMark(hero, false);
					}
					else
					{
						this.IsItTimeOfDeath(hero);
					}
				}
				int num;
				if (this._heroesYoungerThanHeroComesOfAge.TryGetValue(hero, out num))
				{
					int num2 = (int)hero.Age;
					if (num != num2)
					{
						if (num2 >= Campaign.Current.Models.AgeModel.HeroComesOfAge)
						{
							this._heroesYoungerThanHeroComesOfAge.Remove(hero);
							CampaignEventDispatcher.Instance.OnHeroComesOfAge(hero);
						}
						else
						{
							this._heroesYoungerThanHeroComesOfAge[hero] = num2;
							if (num2 == Campaign.Current.Models.AgeModel.BecomeTeenagerAge)
							{
								CampaignEventDispatcher.Instance.OnHeroReachesTeenAge(hero);
							}
							else if (num2 == Campaign.Current.Models.AgeModel.BecomeChildAge)
							{
								CampaignEventDispatcher.Instance.OnHeroGrowsOutOfInfancy(hero);
							}
						}
					}
				}
				if (hero == Hero.MainHero && Hero.IsMainHeroIll && Hero.MainHero.HeroState != Hero.CharacterStates.Dead)
				{
					Campaign.Current.MainHeroIllDays++;
					if (Campaign.Current.MainHeroIllDays > 3)
					{
						Hero.MainHero.HitPoints -= MathF.Ceiling((float)Hero.MainHero.HitPoints * (0.05f * (float)Campaign.Current.MainHeroIllDays));
						if (Hero.MainHero.HitPoints <= 1 && Hero.MainHero.DeathMark == KillCharacterAction.KillCharacterActionDetail.None)
						{
							int num3;
							if (this._extraLivesContainer.TryGetValue(Hero.MainHero, out num3))
							{
								if (num3 == 0)
								{
									this.KillMainHeroWithIllness();
									return;
								}
								Campaign.Current.MainHeroIllDays = -1;
								this._extraLivesContainer[Hero.MainHero] = num3 - 1;
								if (this._extraLivesContainer[Hero.MainHero] == 0)
								{
									this._extraLivesContainer.Remove(Hero.MainHero);
									return;
								}
							}
							else
							{
								this.KillMainHeroWithIllness();
							}
						}
					}
				}
			}
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x000F6750 File Offset: 0x000F4950
		private void KillMainHeroWithIllness()
		{
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			Hero.MainHero.AddDeathMark(null, KillCharacterAction.KillCharacterActionDetail.DiedOfOldAge);
			KillCharacterAction.ApplyByOldAge(Hero.MainHero, true);
		}

		// Token: 0x06003C68 RID: 15464 RVA: 0x000F6774 File Offset: 0x000F4974
		private void OnGameLoaded(CampaignGameStarter obj)
		{
			this.CheckYoungHeroes();
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.4.0", 0)) && Hero.MainHero.DeathMark == KillCharacterAction.KillCharacterActionDetail.DiedInBattle)
			{
				Hero.MainHero.AddDeathMark(null, KillCharacterAction.KillCharacterActionDetail.None);
			}
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x000F67C4 File Offset: 0x000F49C4
		private void OnCharacterCreationIsOver(int index)
		{
			if (index == 1)
			{
				this._gameStartDay = (int)CampaignTime.Now.ToDays;
				if (!CampaignOptions.IsLifeDeathCycleDisabled)
				{
					this.InitializeHeroesYoungerThanHeroComesOfAge();
				}
			}
		}

		// Token: 0x06003C6A RID: 15466 RVA: 0x000F67F6 File Offset: 0x000F49F6
		private void OnHeroGrowsOutOfInfancy(Hero hero)
		{
			if (hero.Clan != Clan.PlayerClan)
			{
				hero.HeroDeveloper.InitializeHeroDeveloper(true);
			}
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x000F6814 File Offset: 0x000F4A14
		private void OnHeroReachesTeenAge(Hero hero)
		{
			Equipment equipmentForHeroReachesTeenAge = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentForHeroReachesTeenAge(hero);
			if (equipmentForHeroReachesTeenAge != null)
			{
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipmentForHeroReachesTeenAge);
				new Equipment(Equipment.EquipmentType.Battle).FillFrom(equipmentForHeroReachesTeenAge, false);
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipmentForHeroReachesTeenAge);
			}
			if (hero.Clan != Clan.PlayerClan)
			{
				foreach (TraitObject traitObject in DefaultTraits.Personality)
				{
					int num = hero.GetTraitLevel(traitObject);
					if (hero.Father == null && hero.Mother == null && hero.Template != null)
					{
						hero.SetTraitLevel(traitObject, hero.Template.GetTraitLevel(traitObject));
					}
					else
					{
						float randomFloat = MBRandom.RandomFloat;
						float randomFloat2 = MBRandom.RandomFloat;
						if ((double)randomFloat < 0.2 && hero.Father != null)
						{
							num = hero.Father.GetTraitLevel(traitObject);
						}
						else if ((double)randomFloat < 0.6 && !hero.CharacterObject.IsFemale && hero.Father != null)
						{
							num = hero.Father.GetTraitLevel(traitObject);
						}
						else if ((double)randomFloat < 0.6 && hero.Mother != null)
						{
							num = hero.Mother.GetTraitLevel(traitObject);
						}
						else if ((double)randomFloat < 0.7 && hero.Mother != null)
						{
							num = hero.Mother.GetTraitLevel(traitObject);
						}
						else if ((double)randomFloat2 < 0.3)
						{
							num--;
						}
						else if ((double)randomFloat2 >= 0.7)
						{
							num++;
						}
						num = MBMath.ClampInt(num, traitObject.MinValue, traitObject.MaxValue);
						if (num != hero.GetTraitLevel(traitObject))
						{
							hero.SetTraitLevel(traitObject, num);
						}
					}
				}
				hero.HeroDeveloper.InitializeHeroDeveloper(true);
			}
		}

		// Token: 0x06003C6C RID: 15468 RVA: 0x000F69F0 File Offset: 0x000F4BF0
		private void OnHeroComesOfAge(Hero hero)
		{
			if (hero.HeroState != Hero.CharacterStates.Active)
			{
				if (hero.Clan != Clan.PlayerClan)
				{
					foreach (ValueTuple<SkillObject, int> valueTuple in Campaign.Current.Models.HeroCreationModel.GetInheritedSkillsForHero(hero))
					{
						hero.SetSkillValue(valueTuple.Item1, valueTuple.Item2);
					}
					hero.HeroDeveloper.InitializeHeroDeveloper(true);
				}
				else
				{
					hero.HeroDeveloper.SetInitialLevel(hero.Level);
				}
				Equipment equipment = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentForHeroComeOfAge(hero, Equipment.EquipmentType.Battle);
				Equipment equipment2 = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentForHeroComeOfAge(hero, Equipment.EquipmentType.Civilian);
				if (equipment == null)
				{
					Debug.FailedAssert("Battle equipment should not be empty", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\AgingCampaignBehavior.cs", "OnHeroComesOfAge", 331);
					equipment = MBEquipmentRosterExtensions.All.Find((MBEquipmentRoster x) => x.StringId == "generic_bat_dummy").GetBattleEquipments().First<Equipment>();
				}
				if (equipment2 == null)
				{
					Debug.FailedAssert("Civilian equipment should not be empty", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\AgingCampaignBehavior.cs", "OnHeroComesOfAge", 340);
					equipment2 = MBEquipmentRosterExtensions.All.Find((MBEquipmentRoster x) => x.StringId == "generic_civ_dummy").GetCivilianEquipments().First<Equipment>();
				}
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipment);
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipment2);
			}
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x000F6B74 File Offset: 0x000F4D74
		private void IsItTimeOfDeath(Hero hero)
		{
			if (hero.IsAlive && hero.Age >= (float)Campaign.Current.Models.AgeModel.BecomeOldAge && !CampaignOptions.IsLifeDeathCycleDisabled && hero.DeathMark == KillCharacterAction.KillCharacterActionDetail.None && MBRandom.RandomFloat < hero.ProbabilityOfDeath)
			{
				int num;
				if (this._extraLivesContainer.TryGetValue(hero, out num) && num > 0)
				{
					this._extraLivesContainer[hero] = num - 1;
					if (this._extraLivesContainer[hero] == 0)
					{
						this._extraLivesContainer.Remove(hero);
						return;
					}
				}
				else
				{
					if (hero == Hero.MainHero && !Hero.IsMainHeroIll)
					{
						Campaign.Current.MainHeroIllDays++;
						Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
						InformationManager.ShowInquiry(new InquiryData(new TextObject("{=2duoimiP}Caught Illness", null).ToString(), new TextObject("{=vo3MqtMn}You are at death's door, wracked by fever, drifting in and out of consciousness. The healers do not believe that you can recover. You should resolve your final affairs and determine a heir for your clan while you still have the strength to speak.", null).ToString(), true, false, new TextObject("{=yQtzabbe}Close", null).ToString(), "", null, null, "event:/ui/notification/quest_fail", 0f, null, null, null), false, false);
						return;
					}
					if (hero != Hero.MainHero && (hero.PartyBelongedTo == null || (hero.PartyBelongedTo.MapEvent == null && hero.PartyBelongedTo.SiegeEvent == null)))
					{
						KillCharacterAction.ApplyByOldAge(hero, true);
					}
				}
			}
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x000F6CC4 File Offset: 0x000F4EC4
		private void InitializeHeroesYoungerThanHeroComesOfAge()
		{
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				int num = (int)hero.Age;
				if (num < Campaign.Current.Models.AgeModel.HeroComesOfAge && !this._heroesYoungerThanHeroComesOfAge.ContainsKey(hero))
				{
					this._heroesYoungerThanHeroComesOfAge.Add(hero, num);
				}
			}
			foreach (Hero hero2 in Hero.DeadOrDisabledHeroes)
			{
				if (!hero2.IsDead && hero2.Age < (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && !this._heroesYoungerThanHeroComesOfAge.ContainsKey(hero2))
				{
					this._heroesYoungerThanHeroComesOfAge.Add(hero2, (int)hero2.Age);
				}
			}
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x000F6DCC File Offset: 0x000F4FCC
		private void CheckYoungHeroes()
		{
			foreach (Hero hero in Hero.FindAll((Hero x) => !x.IsDead && x.Age < (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && !this._heroesYoungerThanHeroComesOfAge.ContainsKey(x)))
			{
				this._heroesYoungerThanHeroComesOfAge.Add(hero, (int)hero.Age);
				if (!hero.IsDisabled && !this._heroesYoungerThanHeroComesOfAge.ContainsKey(hero))
				{
					if (hero.Age > (float)Campaign.Current.Models.AgeModel.BecomeChildAge)
					{
						CampaignEventDispatcher.Instance.OnHeroGrowsOutOfInfancy(hero);
					}
					if (hero.Age > (float)Campaign.Current.Models.AgeModel.BecomeTeenagerAge)
					{
						CampaignEventDispatcher.Instance.OnHeroReachesTeenAge(hero);
					}
				}
			}
		}

		// Token: 0x0400129E RID: 4766
		private Dictionary<Hero, int> _extraLivesContainer = new Dictionary<Hero, int>();

		// Token: 0x0400129F RID: 4767
		private Dictionary<Hero, int> _heroesYoungerThanHeroComesOfAge = new Dictionary<Hero, int>();

		// Token: 0x040012A0 RID: 4768
		private int _gameStartDay;
	}
}
