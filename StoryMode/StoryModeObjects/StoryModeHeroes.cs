using System;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace StoryMode.StoryModeObjects
{
	// Token: 0x02000018 RID: 24
	public class StoryModeHeroes
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000054FD File Offset: 0x000036FD
		public static Hero ElderBrother
		{
			get
			{
				return StoryModeManager.Current.StoryModeHeroes._elderBrother;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000BC RID: 188 RVA: 0x0000550E File Offset: 0x0000370E
		public static Hero LittleBrother
		{
			get
			{
				return StoryModeManager.Current.StoryModeHeroes._littleBrother;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000BD RID: 189 RVA: 0x0000551F File Offset: 0x0000371F
		public static Hero LittleSister
		{
			get
			{
				return StoryModeManager.Current.StoryModeHeroes._littleSister;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00005530 File Offset: 0x00003730
		public static Hero Tacitus
		{
			get
			{
				return StoryModeManager.Current.StoryModeHeroes._tacitus;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005541 File Offset: 0x00003741
		public static Hero Radagos
		{
			get
			{
				return StoryModeManager.Current.StoryModeHeroes._radagos;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00005552 File Offset: 0x00003752
		public static Hero ImperialMentor
		{
			get
			{
				return StoryModeManager.Current.StoryModeHeroes._imperialMentor;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005563 File Offset: 0x00003763
		public static Hero AntiImperialMentor
		{
			get
			{
				return StoryModeManager.Current.StoryModeHeroes._antiImperialMentor;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00005574 File Offset: 0x00003774
		public static Hero RadagosHenchman
		{
			get
			{
				return StoryModeManager.Current.StoryModeHeroes._radagosHenchman;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005585 File Offset: 0x00003785
		public static Hero MainHeroMother
		{
			get
			{
				return StoryModeManager.Current.StoryModeHeroes._mainHeroMother;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00005596 File Offset: 0x00003796
		public static Hero MainHeroFather
		{
			get
			{
				return StoryModeManager.Current.StoryModeHeroes._mainHeroFather;
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000055A7 File Offset: 0x000037A7
		internal StoryModeHeroes()
		{
			this.RegisterAll();
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000055B8 File Offset: 0x000037B8
		private void RegisterAll()
		{
			Clan clan = Campaign.Current.CampaignObjectManager.Find<Clan>("player_faction");
			CharacterObject @object = Game.Current.ObjectManager.GetObject<CharacterObject>("main_hero_mother");
			CharacterObject object2 = Game.Current.ObjectManager.GetObject<CharacterObject>("main_hero_father");
			if (HeroCreator.CreateBasicHero("main_hero_mother", @object, out this._mainHeroMother, false))
			{
				this._mainHeroMother.Clan = clan;
				CampaignTime campaignTime;
				CampaignTime campaignTime2;
				HeroHelper.GetRandomDeathDayAndBirthDay((int)@object.Age, out campaignTime, out campaignTime2);
				this._mainHeroMother.SetBirthDay(campaignTime);
				this._mainHeroMother.SetDeathDay(campaignTime2);
			}
			if (HeroCreator.CreateBasicHero("main_hero_father", object2, out this._mainHeroFather, false))
			{
				this._mainHeroFather.Clan = clan;
				CampaignTime campaignTime3;
				CampaignTime campaignTime4;
				HeroHelper.GetRandomDeathDayAndBirthDay((int)object2.Age, out campaignTime3, out campaignTime4);
				this._mainHeroFather.SetBirthDay(campaignTime3);
				this._mainHeroFather.SetDeathDay(campaignTime4);
			}
			if (HeroCreator.CreateBasicHero("tutorial_npc_brother", MBObjectManager.Instance.GetObject<CharacterObject>("tutorial_npc_brother"), out this._elderBrother, true))
			{
				this._elderBrother.Clan = clan;
				TextObject textObject = GameTexts.FindText("str_player_brother_name", @object.Culture.StringId);
				this._elderBrother.SetName(textObject, textObject);
				this._elderBrother.Mother = @object.HeroObject;
				this._elderBrother.Father = object2.HeroObject;
				this._elderBrother.HeroDeveloper.ResetCharacterStats();
			}
			if (HeroCreator.CreateBasicHero("storymode_little_brother", MBObjectManager.Instance.GetObject<CharacterObject>("storymode_little_brother"), out this._littleBrother, true))
			{
				TextObject textObject2 = GameTexts.FindText("str_player_little_brother_name", @object.Culture.StringId);
				this._littleBrother.SetName(textObject2, textObject2);
				this._littleBrother.Mother = @object.HeroObject;
				this._littleBrother.Father = object2.HeroObject;
			}
			if (HeroCreator.CreateBasicHero("storymode_little_sister", MBObjectManager.Instance.GetObject<CharacterObject>("storymode_little_sister"), out this._littleSister, true))
			{
				TextObject textObject3 = GameTexts.FindText("str_player_little_sister_name", @object.Culture.StringId);
				this._littleSister.SetName(textObject3, textObject3);
				this._littleSister.Mother = @object.HeroObject;
				this._littleSister.Father = object2.HeroObject;
			}
			HeroCreator.CreateBasicHero("tutorial_npc_tacitus", MBObjectManager.Instance.GetObject<CharacterObject>("tutorial_npc_tacitus"), out this._tacitus, true);
			HeroCreator.CreateBasicHero("tutorial_npc_radagos", MBObjectManager.Instance.GetObject<CharacterObject>("tutorial_npc_radagos"), out this._radagos, true);
			HeroCreator.CreateBasicHero("storymode_imperial_mentor_istiana", MBObjectManager.Instance.GetObject<CharacterObject>("storymode_imperial_mentor_istiana"), out this._imperialMentor, true);
			HeroCreator.CreateBasicHero("storymode_imperial_mentor_arzagos", MBObjectManager.Instance.GetObject<CharacterObject>("storymode_imperial_mentor_arzagos"), out this._antiImperialMentor, true);
			HeroCreator.CreateBasicHero("radagos_henchman", MBObjectManager.Instance.GetObject<CharacterObject>("radagos_henchman"), out this._radagosHenchman, true);
		}

		// Token: 0x04000042 RID: 66
		private const string BrotherStringId = "tutorial_npc_brother";

		// Token: 0x04000043 RID: 67
		private const string LittleBrotherStringId = "storymode_little_brother";

		// Token: 0x04000044 RID: 68
		private const string LittleSisterStringId = "storymode_little_sister";

		// Token: 0x04000045 RID: 69
		private const string TacitusStringId = "tutorial_npc_tacitus";

		// Token: 0x04000046 RID: 70
		private const string RadagosStringId = "tutorial_npc_radagos";

		// Token: 0x04000047 RID: 71
		private const string IstianaStringId = "storymode_imperial_mentor_istiana";

		// Token: 0x04000048 RID: 72
		private const string ArzagosStringId = "storymode_imperial_mentor_arzagos";

		// Token: 0x04000049 RID: 73
		private const string GalterStringId = "radagos_henchman";

		// Token: 0x0400004A RID: 74
		private const string MainHeroMotherId = "main_hero_mother";

		// Token: 0x0400004B RID: 75
		private const string MainHeroFatherId = "main_hero_father";

		// Token: 0x0400004C RID: 76
		private Hero _elderBrother;

		// Token: 0x0400004D RID: 77
		private Hero _littleBrother;

		// Token: 0x0400004E RID: 78
		private Hero _littleSister;

		// Token: 0x0400004F RID: 79
		private Hero _tacitus;

		// Token: 0x04000050 RID: 80
		private Hero _radagos;

		// Token: 0x04000051 RID: 81
		private Hero _imperialMentor;

		// Token: 0x04000052 RID: 82
		private Hero _antiImperialMentor;

		// Token: 0x04000053 RID: 83
		private Hero _radagosHenchman;

		// Token: 0x04000054 RID: 84
		private Hero _mainHeroMother;

		// Token: 0x04000055 RID: 85
		private Hero _mainHeroFather;
	}
}
