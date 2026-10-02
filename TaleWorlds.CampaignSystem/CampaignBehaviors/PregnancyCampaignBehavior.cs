using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000457 RID: 1111
	public class PregnancyCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060047AF RID: 18351 RVA: 0x001603C4 File Offset: 0x0015E5C4
		public override void RegisterEvents()
		{
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.OnChildConceivedEvent.AddNonSerializedListener(this, new Action<Hero>(this.ChildConceived));
		}

		// Token: 0x060047B0 RID: 18352 RVA: 0x00160418 File Offset: 0x0015E618
		private void DailyTickHero(Hero hero)
		{
			if (hero.IsFemale && !CampaignOptions.IsLifeDeathCycleDisabled && hero.IsAlive && hero.Age > (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && (hero.Clan == null || !hero.Clan.IsRebelClan))
			{
				if (hero.Age > (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && hero.Spouse != null && hero.Spouse.IsAlive && !hero.IsPregnant)
				{
					this.RefreshSpouseVisit(hero);
				}
				if (hero.IsPregnant)
				{
					this.CheckOffspringsToDeliver(hero);
				}
			}
		}

		// Token: 0x060047B1 RID: 18353 RVA: 0x001604C8 File Offset: 0x0015E6C8
		private void CheckOffspringsToDeliver(Hero hero)
		{
			PregnancyCampaignBehavior.Pregnancy pregnancy = this._heroPregnancies.Find((PregnancyCampaignBehavior.Pregnancy x) => x.Mother == hero);
			if (pregnancy == null)
			{
				hero.IsPregnant = false;
				return;
			}
			this.CheckOffspringToDeliver(pregnancy);
		}

		// Token: 0x060047B2 RID: 18354 RVA: 0x00160511 File Offset: 0x0015E711
		private void RefreshSpouseVisit(Hero hero)
		{
			if (this.CheckAreNearby(hero, hero.Spouse) && MBRandom.RandomFloat <= Campaign.Current.Models.PregnancyModel.GetDailyChanceOfPregnancyForHero(hero))
			{
				MakePregnantAction.Apply(hero);
			}
		}

		// Token: 0x060047B3 RID: 18355 RVA: 0x00160544 File Offset: 0x0015E744
		private bool CheckAreNearby(Hero hero, Hero spouse)
		{
			Settlement settlement;
			MobileParty mobileParty;
			this.GetLocation(hero, out settlement, out mobileParty);
			Settlement settlement2;
			MobileParty mobileParty2;
			this.GetLocation(spouse, out settlement2, out mobileParty2);
			return (settlement != null && settlement == settlement2) || (mobileParty != null && mobileParty == mobileParty2) || (hero.Clan != Hero.MainHero.Clan && MBRandom.RandomFloat < 0.2f);
		}

		// Token: 0x060047B4 RID: 18356 RVA: 0x00160596 File Offset: 0x0015E796
		private void GetLocation(Hero hero, out Settlement heroSettlement, out MobileParty heroParty)
		{
			heroSettlement = hero.CurrentSettlement;
			heroParty = hero.PartyBelongedTo;
			MobileParty mobileParty = heroParty;
			if (((mobileParty != null) ? mobileParty.AttachedTo : null) != null)
			{
				heroParty = heroParty.AttachedTo;
			}
			if (heroSettlement == null)
			{
				MobileParty mobileParty2 = heroParty;
				heroSettlement = ((mobileParty2 != null) ? mobileParty2.CurrentSettlement : null);
			}
		}

		// Token: 0x060047B5 RID: 18357 RVA: 0x001605D8 File Offset: 0x0015E7D8
		private void CheckOffspringToDeliver(PregnancyCampaignBehavior.Pregnancy pregnancy)
		{
			PregnancyModel pregnancyModel = Campaign.Current.Models.PregnancyModel;
			if (!pregnancy.DueDate.IsFuture && pregnancy.Mother.IsAlive)
			{
				Hero mother = pregnancy.Mother;
				bool flag = MBRandom.RandomFloat <= pregnancyModel.DeliveringTwinsProbability;
				List<Hero> list = new List<Hero>();
				int num = (flag ? 2 : 1);
				int num2 = 0;
				for (int i = 0; i < num; i++)
				{
					if (MBRandom.RandomFloat > pregnancyModel.StillbirthProbability)
					{
						bool flag2 = MBRandom.RandomFloat <= pregnancyModel.DeliveringFemaleOffspringProbability;
						Hero hero = HeroCreator.DeliverOffSpring(mother, pregnancy.Father, flag2);
						list.Add(hero);
					}
					else
					{
						TextObject textObject = new TextObject("{=pw4cUPEn}{MOTHER.LINK} has delivered stillborn.", null);
						StringHelpers.SetCharacterProperties("MOTHER", mother.CharacterObject, textObject, false);
						InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
						num2++;
					}
				}
				CampaignEventDispatcher.Instance.OnGivenBirth(mother, list, num2);
				mother.IsPregnant = false;
				this._heroPregnancies.Remove(pregnancy);
				if (mother != Hero.MainHero && MBRandom.RandomFloat <= pregnancyModel.MaternalMortalityProbabilityInLabor)
				{
					KillCharacterAction.ApplyInLabor(mother, true);
				}
			}
		}

		// Token: 0x060047B6 RID: 18358 RVA: 0x00160710 File Offset: 0x0015E910
		private void ChildConceived(Hero mother)
		{
			this._heroPregnancies.Add(new PregnancyCampaignBehavior.Pregnancy(mother, mother.Spouse, CampaignTime.DaysFromNow(Campaign.Current.Models.PregnancyModel.PregnancyDurationInDays)));
		}

		// Token: 0x060047B7 RID: 18359 RVA: 0x00160744 File Offset: 0x0015E944
		public void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			if (victim.IsFemale && this._heroPregnancies.Any<PregnancyCampaignBehavior.Pregnancy>((PregnancyCampaignBehavior.Pregnancy pregnancy) => pregnancy.Mother == victim))
			{
				this._heroPregnancies.RemoveAll((PregnancyCampaignBehavior.Pregnancy pregnancy) => pregnancy.Mother == victim);
			}
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x0016079C File Offset: 0x0015E99C
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<PregnancyCampaignBehavior.Pregnancy>>("_heroPregnancies", ref this._heroPregnancies);
		}

		// Token: 0x04001455 RID: 5205
		private List<PregnancyCampaignBehavior.Pregnancy> _heroPregnancies = new List<PregnancyCampaignBehavior.Pregnancy>();

		// Token: 0x02000883 RID: 2179
		public class PregnancyCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06006B69 RID: 27497 RVA: 0x001DAC9E File Offset: 0x001D8E9E
			public PregnancyCampaignBehaviorTypeDefiner()
				: base(110000)
			{
			}

			// Token: 0x06006B6A RID: 27498 RVA: 0x001DACAB File Offset: 0x001D8EAB
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(PregnancyCampaignBehavior.Pregnancy), 2, null);
			}

			// Token: 0x06006B6B RID: 27499 RVA: 0x001DACBF File Offset: 0x001D8EBF
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(List<PregnancyCampaignBehavior.Pregnancy>));
			}
		}

		// Token: 0x02000884 RID: 2180
		internal class Pregnancy
		{
			// Token: 0x06006B6C RID: 27500 RVA: 0x001DACD1 File Offset: 0x001D8ED1
			public Pregnancy(Hero pregnantHero, Hero father, CampaignTime dueDate)
			{
				this.Mother = pregnantHero;
				this.Father = father;
				this.DueDate = dueDate;
			}

			// Token: 0x06006B6D RID: 27501 RVA: 0x001DACEE File Offset: 0x001D8EEE
			internal static void AutoGeneratedStaticCollectObjectsPregnancy(object o, List<object> collectedObjects)
			{
				((PregnancyCampaignBehavior.Pregnancy)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006B6E RID: 27502 RVA: 0x001DACFC File Offset: 0x001D8EFC
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Mother);
				collectedObjects.Add(this.Father);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.DueDate, collectedObjects);
			}

			// Token: 0x06006B6F RID: 27503 RVA: 0x001DAD27 File Offset: 0x001D8F27
			internal static object AutoGeneratedGetMemberValueMother(object o)
			{
				return ((PregnancyCampaignBehavior.Pregnancy)o).Mother;
			}

			// Token: 0x06006B70 RID: 27504 RVA: 0x001DAD34 File Offset: 0x001D8F34
			internal static object AutoGeneratedGetMemberValueFather(object o)
			{
				return ((PregnancyCampaignBehavior.Pregnancy)o).Father;
			}

			// Token: 0x06006B71 RID: 27505 RVA: 0x001DAD41 File Offset: 0x001D8F41
			internal static object AutoGeneratedGetMemberValueDueDate(object o)
			{
				return ((PregnancyCampaignBehavior.Pregnancy)o).DueDate;
			}

			// Token: 0x04002533 RID: 9523
			[SaveableField(1)]
			public readonly Hero Mother;

			// Token: 0x04002534 RID: 9524
			[SaveableField(2)]
			public readonly Hero Father;

			// Token: 0x04002535 RID: 9525
			[SaveableField(3)]
			public readonly CampaignTime DueDate;
		}
	}
}
