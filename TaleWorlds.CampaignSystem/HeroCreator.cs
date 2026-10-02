using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000093 RID: 147
	public static class HeroCreator
	{
		// Token: 0x060012C8 RID: 4808 RVA: 0x00057148 File Offset: 0x00055348
		public static Hero CreateNotable(Occupation occupation, Settlement settlement = null)
		{
			CharacterObject randomTemplateByOccupation = Campaign.Current.Models.HeroCreationModel.GetRandomTemplateByOccupation(occupation, settlement);
			ValueTuple<CampaignTime, CampaignTime> birthAndDeathDay = Campaign.Current.Models.HeroCreationModel.GetBirthAndDeathDay(randomTemplateByOccupation, true, -1);
			CampaignTime item = birthAndDeathDay.Item1;
			CampaignTime item2 = birthAndDeathDay.Item2;
			Hero hero = HeroCreator.CreateHero(randomTemplateByOccupation, true, item, item2);
			HeroCreator.HeroInitializationArgs heroInitializationArgs = new HeroCreator.HeroInitializationArgs(hero, false).SetGenerateFirstAndFullName(true);
			if (settlement != null)
			{
				heroInitializationArgs.SetBornSettlement(settlement);
			}
			heroInitializationArgs.SetAppearance(new StaticBodyProperties?(Campaign.Current.Models.HeroCreationModel.GetStaticBodyProperties(hero, false, 0f)), -1f, -1f, -1, -1, -1);
			HeroCreator.InitializeHeroFromSettings(heroInitializationArgs.Hero, heroInitializationArgs);
			return hero;
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x000571FC File Offset: 0x000553FC
		public static Hero CreateSpecialHero(CharacterObject template, Settlement bornSettlement = null, Clan faction = null, Clan supporterOfClan = null, int age = -1)
		{
			ValueTuple<CampaignTime, CampaignTime> birthAndDeathDay = Campaign.Current.Models.HeroCreationModel.GetBirthAndDeathDay(template, true, age);
			CampaignTime item = birthAndDeathDay.Item1;
			CampaignTime item2 = birthAndDeathDay.Item2;
			Hero hero = HeroCreator.CreateHero(template, true, item, item2);
			HeroCreator.HeroInitializationArgs heroInitializationArgs = new HeroCreator.HeroInitializationArgs(hero, false).SetGenerateFirstAndFullName(true);
			if (bornSettlement != null)
			{
				heroInitializationArgs.SetBornSettlement(bornSettlement);
			}
			if (faction != null)
			{
				heroInitializationArgs.SetClan(faction);
			}
			if (supporterOfClan != null)
			{
				heroInitializationArgs.SetSupporterOf(supporterOfClan);
			}
			HeroCreator.InitializeHeroFromSettings(heroInitializationArgs.Hero, heroInitializationArgs);
			return hero;
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00057274 File Offset: 0x00055474
		public static Hero CreateChild(CharacterObject template, Settlement bornSettlement, Clan clan, int age)
		{
			ValueTuple<CampaignTime, CampaignTime> birthAndDeathDay = Campaign.Current.Models.HeroCreationModel.GetBirthAndDeathDay(template, true, age);
			CampaignTime item = birthAndDeathDay.Item1;
			CampaignTime item2 = birthAndDeathDay.Item2;
			Hero hero = HeroCreator.CreateHero(template, true, item, item2);
			HeroCreator.HeroInitializationArgs heroInitializationArgs = new HeroCreator.HeroInitializationArgs(hero, false).SetGenerateFirstAndFullName(true).SetBornSettlement(bornSettlement).SetClan(clan)
				.SetLevel(1);
			HeroCreator.InitializeHeroFromSettings(heroInitializationArgs.Hero, heroInitializationArgs);
			return hero;
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x000572DC File Offset: 0x000554DC
		public static Hero CreateRelativeNotableHero(Hero relative)
		{
			CharacterObject randomTemplateByOccupation = Campaign.Current.Models.HeroCreationModel.GetRandomTemplateByOccupation(relative.Occupation, relative.HomeSettlement);
			ValueTuple<CampaignTime, CampaignTime> birthAndDeathDay = Campaign.Current.Models.HeroCreationModel.GetBirthAndDeathDay(randomTemplateByOccupation, true, -1);
			CampaignTime item = birthAndDeathDay.Item1;
			CampaignTime item2 = birthAndDeathDay.Item2;
			Hero hero = HeroCreator.CreateHero(randomTemplateByOccupation, true, item, item2);
			BodyProperties bodyPropertiesMin = relative.CharacterObject.GetBodyPropertiesMin(false);
			BodyProperties bodyPropertiesMin2 = randomTemplateByOccupation.GetBodyPropertiesMin(false);
			int defaultFaceSeed = relative.CharacterObject.GetDefaultFaceSeed(1);
			MBBodyProperty bodyPropertyRange = hero.CharacterObject.BodyPropertyRange;
			BodyProperties randomBodyProperties = BodyProperties.GetRandomBodyProperties(randomTemplateByOccupation.Race, randomTemplateByOccupation.IsFemale, bodyPropertiesMin, bodyPropertiesMin2, 1, defaultFaceSeed, bodyPropertyRange.HairTags, bodyPropertyRange.BeardTags, bodyPropertyRange.TattooTags, 0f);
			HeroCreator.HeroInitializationArgs heroInitializationArgs = new HeroCreator.HeroInitializationArgs(hero, false).SetBornSettlement(relative.HomeSettlement).SetCulture(relative.Culture).SetAppearance(new StaticBodyProperties?(randomBodyProperties.StaticProperties), -1f, -1f, -1, -1, -1)
				.SetGenerateFirstAndFullName(true);
			HeroCreator.InitializeHeroFromSettings(heroInitializationArgs.Hero, heroInitializationArgs);
			return hero;
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x000573EC File Offset: 0x000555EC
		public static bool CreateBasicHero(string stringId, CharacterObject character, out Hero hero, bool isAlive = true)
		{
			hero = Campaign.Current.CampaignObjectManager.Find<Hero>(stringId);
			if (hero == null)
			{
				ValueTuple<CampaignTime, CampaignTime> birthAndDeathDay = Campaign.Current.Models.HeroCreationModel.GetBirthAndDeathDay(character, isAlive, (int)character.Age);
				CampaignTime item = birthAndDeathDay.Item1;
				CampaignTime item2 = birthAndDeathDay.Item2;
				hero = HeroCreator.CreateHero(character, false, item, item2);
				HeroCreator.HeroInitializationArgs heroInitializationArgs = new HeroCreator.HeroInitializationArgs(hero, false);
				HeroCreator.InitializeHeroFromSettings(heroInitializationArgs.Hero, heroInitializationArgs);
				return true;
			}
			return false;
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0005745C File Offset: 0x0005565C
		public static Hero DeliverOffSpring(Hero mother, Hero father, bool isOffspringFemale)
		{
			Debug.SilentAssert(mother.CharacterObject.Race == father.CharacterObject.Race, "", false, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\HeroCreator.cs", "DeliverOffSpring", 272);
			CharacterObject characterTemplateForOffspring = Campaign.Current.Models.HeroCreationModel.GetCharacterTemplateForOffspring(mother, father, isOffspringFemale);
			ValueTuple<CampaignTime, CampaignTime> birthAndDeathDay = Campaign.Current.Models.HeroCreationModel.GetBirthAndDeathDay(characterTemplateForOffspring, true, 0);
			CampaignTime item = birthAndDeathDay.Item1;
			CampaignTime item2 = birthAndDeathDay.Item2;
			Hero hero = HeroCreator.CreateHero(characterTemplateForOffspring, true, item, item2);
			HeroCreator.HeroInitializationArgs heroInitializationArgs = new HeroCreator.HeroInitializationArgs(hero, true).SetMother(mother).SetFather(father).SetIsFemale(isOffspringFemale)
				.SetOccupation(isOffspringFemale ? mother.Occupation : father.Occupation)
				.SetLevel(1)
				.SetGenerateFirstAndFullName(true);
			if (mother == Hero.MainHero || father == Hero.MainHero)
			{
				heroInitializationArgs.SetClan(Hero.MainHero.Clan).SetCulture(Hero.MainHero.Culture);
			}
			else
			{
				CultureObject cultureObject = ((MBRandom.RandomFloat < 0.5f) ? father.Culture : mother.Culture);
				heroInitializationArgs.SetClan(father.Clan).SetCulture(cultureObject);
			}
			HeroCreator.InitializeHeroFromSettings(heroInitializationArgs.Hero, heroInitializationArgs);
			return hero;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x0005758C File Offset: 0x0005578C
		private static Hero CreateHero(CharacterObject character, bool useCharacterAsTemplate, CampaignTime birthDay, CampaignTime deathDay)
		{
			if (useCharacterAsTemplate)
			{
				Debug.Print("creating hero from template with id: " + character.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
				character = CharacterObject.CreateFrom(character, null);
			}
			else
			{
				Debug.Print("creating hero for character with id: " + character.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
			}
			return new Hero(character.StringId, character, birthDay, deathDay);
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00057600 File Offset: 0x00055800
		private static void InitializeHeroFromSettings(Hero hero, HeroCreator.HeroInitializationArgs initializationArgs)
		{
			hero.Mother = initializationArgs.Mother;
			hero.Father = initializationArgs.Father;
			hero.IsFemale = initializationArgs.IsFemale;
			hero.BornSettlement = (initializationArgs.HasBornSettlementBeenSet ? initializationArgs.BornSettlement : Campaign.Current.Models.HeroCreationModel.GetBornSettlement(hero));
			hero.PreferredUpgradeFormation = initializationArgs.PreferredUpgradeFormation ?? Campaign.Current.Models.HeroCreationModel.GetPreferredUpgradeFormation(hero);
			hero.Clan = (initializationArgs.HasClanBeenSet ? initializationArgs.Clan : Campaign.Current.Models.HeroCreationModel.GetClan(hero));
			hero.Culture = initializationArgs.Culture ?? Campaign.Current.Models.HeroCreationModel.GetCulture(hero, hero.BornSettlement, hero.Clan);
			hero.StaticBodyProperties = initializationArgs.StaticBodyProperties ?? Campaign.Current.Models.HeroCreationModel.GetStaticBodyProperties(hero, initializationArgs.IsOffspring, 0.2f);
			hero.SupporterOf = initializationArgs.SupporterOf;
			hero.Level = initializationArgs.Level;
			hero.Weight = initializationArgs.Weight;
			hero.Build = initializationArgs.Build;
			if (initializationArgs.GenerateFirstAndFullName)
			{
				ValueTuple<TextObject, TextObject> valueTuple = Campaign.Current.Models.HeroCreationModel.GenerateFirstAndFullName(hero);
				TextObject item = valueTuple.Item1;
				TextObject item2 = valueTuple.Item2;
				hero.SetName(item2, item);
			}
			else
			{
				hero.SetName(initializationArgs.Name, initializationArgs.FirstName);
			}
			if (initializationArgs.Occupation != hero.Occupation)
			{
				hero.SetNewOccupation(initializationArgs.Occupation);
			}
			foreach (ValueTuple<TraitObject, int> valueTuple2 in Campaign.Current.Models.HeroCreationModel.GetTraitsForHero(hero))
			{
				TraitObject item3 = valueTuple2.Item1;
				int item4 = valueTuple2.Item2;
				hero.SetTraitLevel(item3, item4);
			}
			foreach (ValueTuple<SkillObject, int> valueTuple3 in Campaign.Current.Models.HeroCreationModel.GetDefaultSkillsForHero(hero))
			{
				SkillObject item5 = valueTuple3.Item1;
				int item6 = valueTuple3.Item2;
				hero.SetSkillValue(item5, item6);
			}
			if (initializationArgs.IsOffspring)
			{
				hero.HeroDeveloper.InitializeHeroDeveloper(true);
				hero.ClearTraits();
			}
			else if (hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
			{
				hero.HeroDeveloper.InitializeHeroDeveloper(true);
			}
			Equipment civilianEquipment = Campaign.Current.Models.HeroCreationModel.GetCivilianEquipment(hero);
			EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, civilianEquipment);
			Equipment battleEquipment = Campaign.Current.Models.HeroCreationModel.GetBattleEquipment(hero);
			EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, battleEquipment);
			CampaignEventDispatcher.Instance.OnHeroCreated(initializationArgs.Hero, initializationArgs.IsOffspring);
		}

		// Token: 0x0200056F RID: 1391
		private class HeroInitializationArgs
		{
			// Token: 0x17000F5F RID: 3935
			// (get) Token: 0x06005018 RID: 20504 RVA: 0x0018E86E File Offset: 0x0018CA6E
			public Hero Hero { get; }

			// Token: 0x17000F60 RID: 3936
			// (get) Token: 0x06005019 RID: 20505 RVA: 0x0018E876 File Offset: 0x0018CA76
			// (set) Token: 0x0600501A RID: 20506 RVA: 0x0018E87E File Offset: 0x0018CA7E
			public TextObject Name { get; private set; }

			// Token: 0x17000F61 RID: 3937
			// (get) Token: 0x0600501B RID: 20507 RVA: 0x0018E887 File Offset: 0x0018CA87
			// (set) Token: 0x0600501C RID: 20508 RVA: 0x0018E88F File Offset: 0x0018CA8F
			public TextObject FirstName { get; private set; }

			// Token: 0x17000F62 RID: 3938
			// (get) Token: 0x0600501D RID: 20509 RVA: 0x0018E898 File Offset: 0x0018CA98
			// (set) Token: 0x0600501E RID: 20510 RVA: 0x0018E8A0 File Offset: 0x0018CAA0
			public Hero Mother { get; private set; }

			// Token: 0x17000F63 RID: 3939
			// (get) Token: 0x0600501F RID: 20511 RVA: 0x0018E8A9 File Offset: 0x0018CAA9
			// (set) Token: 0x06005020 RID: 20512 RVA: 0x0018E8B1 File Offset: 0x0018CAB1
			public Hero Father { get; private set; }

			// Token: 0x17000F64 RID: 3940
			// (get) Token: 0x06005021 RID: 20513 RVA: 0x0018E8BA File Offset: 0x0018CABA
			// (set) Token: 0x06005022 RID: 20514 RVA: 0x0018E8C2 File Offset: 0x0018CAC2
			public bool IsFemale { get; private set; }

			// Token: 0x17000F65 RID: 3941
			// (get) Token: 0x06005023 RID: 20515 RVA: 0x0018E8CB File Offset: 0x0018CACB
			// (set) Token: 0x06005024 RID: 20516 RVA: 0x0018E8D3 File Offset: 0x0018CAD3
			public Settlement BornSettlement { get; private set; }

			// Token: 0x17000F66 RID: 3942
			// (get) Token: 0x06005025 RID: 20517 RVA: 0x0018E8DC File Offset: 0x0018CADC
			// (set) Token: 0x06005026 RID: 20518 RVA: 0x0018E8E4 File Offset: 0x0018CAE4
			public int Level { get; private set; }

			// Token: 0x17000F67 RID: 3943
			// (get) Token: 0x06005027 RID: 20519 RVA: 0x0018E8ED File Offset: 0x0018CAED
			// (set) Token: 0x06005028 RID: 20520 RVA: 0x0018E8F5 File Offset: 0x0018CAF5
			public float Weight { get; private set; }

			// Token: 0x17000F68 RID: 3944
			// (get) Token: 0x06005029 RID: 20521 RVA: 0x0018E8FE File Offset: 0x0018CAFE
			// (set) Token: 0x0600502A RID: 20522 RVA: 0x0018E906 File Offset: 0x0018CB06
			public float Build { get; private set; }

			// Token: 0x17000F69 RID: 3945
			// (get) Token: 0x0600502B RID: 20523 RVA: 0x0018E90F File Offset: 0x0018CB0F
			// (set) Token: 0x0600502C RID: 20524 RVA: 0x0018E917 File Offset: 0x0018CB17
			public StaticBodyProperties? StaticBodyProperties { get; private set; }

			// Token: 0x17000F6A RID: 3946
			// (get) Token: 0x0600502D RID: 20525 RVA: 0x0018E920 File Offset: 0x0018CB20
			// (set) Token: 0x0600502E RID: 20526 RVA: 0x0018E928 File Offset: 0x0018CB28
			public FormationClass? PreferredUpgradeFormation { get; private set; }

			// Token: 0x17000F6B RID: 3947
			// (get) Token: 0x0600502F RID: 20527 RVA: 0x0018E931 File Offset: 0x0018CB31
			// (set) Token: 0x06005030 RID: 20528 RVA: 0x0018E939 File Offset: 0x0018CB39
			public Clan Clan { get; private set; }

			// Token: 0x17000F6C RID: 3948
			// (get) Token: 0x06005031 RID: 20529 RVA: 0x0018E942 File Offset: 0x0018CB42
			// (set) Token: 0x06005032 RID: 20530 RVA: 0x0018E94A File Offset: 0x0018CB4A
			public CultureObject Culture { get; private set; }

			// Token: 0x17000F6D RID: 3949
			// (get) Token: 0x06005033 RID: 20531 RVA: 0x0018E953 File Offset: 0x0018CB53
			// (set) Token: 0x06005034 RID: 20532 RVA: 0x0018E95B File Offset: 0x0018CB5B
			public Clan SupporterOf { get; private set; }

			// Token: 0x17000F6E RID: 3950
			// (get) Token: 0x06005035 RID: 20533 RVA: 0x0018E964 File Offset: 0x0018CB64
			// (set) Token: 0x06005036 RID: 20534 RVA: 0x0018E96C File Offset: 0x0018CB6C
			public Occupation Occupation { get; private set; }

			// Token: 0x17000F6F RID: 3951
			// (get) Token: 0x06005037 RID: 20535 RVA: 0x0018E975 File Offset: 0x0018CB75
			// (set) Token: 0x06005038 RID: 20536 RVA: 0x0018E97D File Offset: 0x0018CB7D
			public bool IsOffspring { get; private set; }

			// Token: 0x17000F70 RID: 3952
			// (get) Token: 0x06005039 RID: 20537 RVA: 0x0018E986 File Offset: 0x0018CB86
			// (set) Token: 0x0600503A RID: 20538 RVA: 0x0018E98E File Offset: 0x0018CB8E
			public bool GenerateFirstAndFullName { get; private set; }

			// Token: 0x17000F71 RID: 3953
			// (get) Token: 0x0600503B RID: 20539 RVA: 0x0018E997 File Offset: 0x0018CB97
			// (set) Token: 0x0600503C RID: 20540 RVA: 0x0018E99F File Offset: 0x0018CB9F
			public bool HasBornSettlementBeenSet { get; private set; }

			// Token: 0x17000F72 RID: 3954
			// (get) Token: 0x0600503D RID: 20541 RVA: 0x0018E9A8 File Offset: 0x0018CBA8
			// (set) Token: 0x0600503E RID: 20542 RVA: 0x0018E9B0 File Offset: 0x0018CBB0
			public bool HasClanBeenSet { get; private set; }

			// Token: 0x0600503F RID: 20543 RVA: 0x0018E9BC File Offset: 0x0018CBBC
			public HeroInitializationArgs(Hero hero, bool isOffspring)
			{
				DynamicBodyProperties dynamicBodyPropertiesBetweenMinMaxRange = CharacterHelper.GetDynamicBodyPropertiesBetweenMinMaxRange(hero.CharacterObject);
				this.Hero = hero;
				this.IsOffspring = isOffspring;
				this.Name = hero.Name;
				this.FirstName = hero.FirstName;
				this.Mother = hero.Mother;
				this.Father = hero.Father;
				this.IsFemale = hero.IsFemale;
				this.BornSettlement = null;
				this.Level = hero.Level;
				this.Weight = dynamicBodyPropertiesBetweenMinMaxRange.Weight;
				this.Build = dynamicBodyPropertiesBetweenMinMaxRange.Build;
				this.StaticBodyProperties = null;
				this.PreferredUpgradeFormation = null;
				this.Clan = null;
				this.SupporterOf = hero.SupporterOf;
				this.Occupation = hero.Occupation;
				this.Culture = null;
			}

			// Token: 0x06005040 RID: 20544 RVA: 0x0018EA94 File Offset: 0x0018CC94
			public HeroCreator.HeroInitializationArgs SetGenerateFirstAndFullName(bool value)
			{
				this.GenerateFirstAndFullName = value;
				return this;
			}

			// Token: 0x06005041 RID: 20545 RVA: 0x0018EA9E File Offset: 0x0018CC9E
			public HeroCreator.HeroInitializationArgs SetName(TextObject name)
			{
				this.Name = name;
				return this;
			}

			// Token: 0x06005042 RID: 20546 RVA: 0x0018EAA8 File Offset: 0x0018CCA8
			public HeroCreator.HeroInitializationArgs SetFirstName(TextObject firstName)
			{
				this.FirstName = firstName;
				return this;
			}

			// Token: 0x06005043 RID: 20547 RVA: 0x0018EAB2 File Offset: 0x0018CCB2
			public HeroCreator.HeroInitializationArgs SetMother(Hero mother)
			{
				this.Mother = mother;
				return this;
			}

			// Token: 0x06005044 RID: 20548 RVA: 0x0018EABC File Offset: 0x0018CCBC
			public HeroCreator.HeroInitializationArgs SetFather(Hero father)
			{
				this.Father = father;
				return this;
			}

			// Token: 0x06005045 RID: 20549 RVA: 0x0018EAC6 File Offset: 0x0018CCC6
			public HeroCreator.HeroInitializationArgs SetIsFemale(bool isFemale)
			{
				this.IsFemale = isFemale;
				return this;
			}

			// Token: 0x06005046 RID: 20550 RVA: 0x0018EAD0 File Offset: 0x0018CCD0
			public HeroCreator.HeroInitializationArgs SetBornSettlement(Settlement bornSettlement)
			{
				this.BornSettlement = bornSettlement;
				this.HasBornSettlementBeenSet = true;
				return this;
			}

			// Token: 0x06005047 RID: 20551 RVA: 0x0018EAE1 File Offset: 0x0018CCE1
			public HeroCreator.HeroInitializationArgs SetLevel(int level)
			{
				this.Level = level;
				return this;
			}

			// Token: 0x06005048 RID: 20552 RVA: 0x0018EAEC File Offset: 0x0018CCEC
			public HeroCreator.HeroInitializationArgs SetAppearance(StaticBodyProperties? staticBodyProperties, float weight = -1f, float build = -1f, int hair = -1, int beard = -1, int tattoo = -1)
			{
				if (weight > 0f)
				{
					this.Weight = weight;
				}
				if (build > 0f)
				{
					this.Build = build;
				}
				BodyProperties bodyProperties = new BodyProperties(new DynamicBodyProperties(this.Hero.Age, this.Weight, this.Build), staticBodyProperties ?? default(StaticBodyProperties));
				FaceGen.SetHair(ref bodyProperties, hair, beard, tattoo);
				this.StaticBodyProperties = new StaticBodyProperties?(bodyProperties.StaticProperties);
				return this;
			}

			// Token: 0x06005049 RID: 20553 RVA: 0x0018EB77 File Offset: 0x0018CD77
			public HeroCreator.HeroInitializationArgs SetPreferredUpgradeFormation(FormationClass preferredUpgradeFormation)
			{
				this.PreferredUpgradeFormation = new FormationClass?(preferredUpgradeFormation);
				return this;
			}

			// Token: 0x0600504A RID: 20554 RVA: 0x0018EB86 File Offset: 0x0018CD86
			public HeroCreator.HeroInitializationArgs SetClan(Clan clan)
			{
				this.Clan = clan;
				this.HasClanBeenSet = true;
				return this;
			}

			// Token: 0x0600504B RID: 20555 RVA: 0x0018EB97 File Offset: 0x0018CD97
			public HeroCreator.HeroInitializationArgs SetCulture(CultureObject culture)
			{
				this.Culture = culture;
				return this;
			}

			// Token: 0x0600504C RID: 20556 RVA: 0x0018EBA1 File Offset: 0x0018CDA1
			public HeroCreator.HeroInitializationArgs SetSupporterOf(Clan supporterOf)
			{
				this.SupporterOf = supporterOf;
				return this;
			}

			// Token: 0x0600504D RID: 20557 RVA: 0x0018EBAB File Offset: 0x0018CDAB
			public HeroCreator.HeroInitializationArgs SetOccupation(Occupation occupation)
			{
				this.Occupation = occupation;
				return this;
			}
		}
	}
}
