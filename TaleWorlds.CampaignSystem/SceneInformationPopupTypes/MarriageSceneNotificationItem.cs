using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000CC RID: 204
	public class MarriageSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060014B7 RID: 5303 RVA: 0x00061111 File Offset: 0x0005F311
		public Hero GroomHero { get; }

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060014B8 RID: 5304 RVA: 0x00061119 File Offset: 0x0005F319
		public Hero BrideHero { get; }

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x00061121 File Offset: 0x0005F321
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_wedding";
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x060014BA RID: 5306 RVA: 0x00061128 File Offset: 0x0005F328
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				Hero hero = ((this.GroomHero == Hero.MainHero) ? this.GroomHero : this.BrideHero);
				Hero hero2 = ((hero == this.GroomHero) ? this.BrideHero : this.GroomHero);
				GameTexts.SetVariable("FIRST_HERO", hero.Name);
				GameTexts.SetVariable("SECOND_HERO", hero2.Name);
				return GameTexts.FindText("str_marriage_notification", null);
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060014BB RID: 5307 RVA: 0x000611C1 File Offset: 0x0005F3C1
		public override SceneNotificationData.RelevantContextType RelevantContext { get; }

		// Token: 0x060014BC RID: 5308 RVA: 0x000611CC File Offset: 0x0005F3CC
		public override Banner[] GetBanners()
		{
			return new Banner[]
			{
				(this.GroomHero.Father != null) ? this.GroomHero.Father.ClanBanner : this.GroomHero.ClanBanner,
				(this.BrideHero.Father != null) ? this.BrideHero.Father.ClanBanner : this.BrideHero.ClanBanner,
				(this.GroomHero.Father != null) ? this.GroomHero.Father.ClanBanner : this.GroomHero.ClanBanner,
				(this.BrideHero.Father != null) ? this.BrideHero.Father.ClanBanner : this.BrideHero.ClanBanner
			};
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x00061294 File Offset: 0x0005F494
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.GroomHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.GroomHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			Equipment equipment2;
			if (this.BrideHero.Culture.MarriageBrideEquipmentRoster != null)
			{
				equipment2 = this.BrideHero.Culture.MarriageBrideEquipmentRoster.DefaultEquipment.Clone(false);
			}
			else
			{
				equipment2 = MBEquipmentRoster.EmptyEquipment.Clone(false);
				Debug.FailedAssert("Could not find marriage equipment for culture: " + this.BrideHero.Culture.StringId + ".", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\SceneInformationPopupTypes\\MarriageSceneNotificationItem.cs", "GetSceneNotificationCharacters", 61);
			}
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.BrideHero, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>("cutscene_monk");
			Equipment equipment3 = @object.Equipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment3, false, false);
			list.Add(new SceneNotificationData.SceneNotificationCharacter(@object, equipment3, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			List<Hero> audienceMembers = this.GetAudienceMembers(this.BrideHero, this.GroomHero);
			for (int i = 0; i < audienceMembers.Count; i++)
			{
				Hero hero = audienceMembers[i];
				if (hero != null)
				{
					Equipment equipment4 = hero.CivilianEquipment.Clone(false);
					CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment4, false, false);
					list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, equipment4, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
				}
				else
				{
					list.Add(new SceneNotificationData.SceneNotificationCharacter(null, null, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
				}
			}
			return list.ToArray();
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0006144B File Offset: 0x0005F64B
		public MarriageSceneNotificationItem(Hero groomHero, Hero brideHero, CampaignTime creationTime, SceneNotificationData.RelevantContextType relevantContextType = SceneNotificationData.RelevantContextType.Any)
		{
			this.GroomHero = groomHero;
			this.BrideHero = brideHero;
			this.RelevantContext = relevantContextType;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x00061470 File Offset: 0x0005F670
		private List<Hero> GetAudienceMembers(Hero brideHero, Hero groomHero)
		{
			Queue<Hero> groomSide = new Queue<Hero>();
			Queue<Hero> brideSide = new Queue<Hero>();
			List<Hero> list = new List<Hero>();
			Hero mother = groomHero.Mother;
			if (mother != null && mother.IsAlive)
			{
				groomSide.Enqueue(groomHero.Mother);
			}
			Hero father = groomHero.Father;
			if (father != null && father.IsAlive)
			{
				groomSide.Enqueue(groomHero.Father);
			}
			if (groomHero.Siblings != null)
			{
				foreach (Hero hero in groomHero.Siblings.Where<Hero>((Hero s) => s.IsAlive && !s.IsChild))
				{
					groomSide.Enqueue(hero);
				}
			}
			if (groomHero.Children != null)
			{
				foreach (Hero hero2 in groomHero.Children.Where<Hero>((Hero s) => s.IsAlive && !s.IsChild))
				{
					groomSide.Enqueue(hero2);
				}
			}
			Hero mother2 = brideHero.Mother;
			if (mother2 != null && mother2.IsAlive)
			{
				brideSide.Enqueue(brideHero.Mother);
			}
			Hero father2 = brideHero.Father;
			if (father2 != null && father2.IsAlive)
			{
				brideSide.Enqueue(brideHero.Father);
			}
			if (brideHero.Siblings != null)
			{
				foreach (Hero hero3 in brideHero.Siblings.Where<Hero>((Hero s) => s.IsAlive && !s.IsChild))
				{
					brideSide.Enqueue(hero3);
				}
			}
			if (brideHero.Children != null)
			{
				foreach (Hero hero4 in brideHero.Children.Where<Hero>((Hero s) => s.IsAlive && !s.IsChild))
				{
					brideSide.Enqueue(hero4);
				}
			}
			if (groomSide.Count < 3)
			{
				IEnumerable<Hero> allAliveHeroes = Hero.AllAliveHeroes;
				Func<Hero, bool> <>9__4;
				Func<Hero, bool> func;
				if ((func = <>9__4) == null)
				{
					func = (<>9__4 = (Hero h) => h.IsLord && !h.IsChild && h != groomHero && h != brideHero && h.IsFriend(groomHero) && !brideSide.Contains(h));
				}
				foreach (Hero hero5 in allAliveHeroes.Where<Hero>(func).Take<Hero>(MathF.Ceiling(3f - (float)groomSide.Count)))
				{
					groomSide.Enqueue(hero5);
				}
			}
			if (brideSide.Count < 3)
			{
				IEnumerable<Hero> allAliveHeroes2 = Hero.AllAliveHeroes;
				Func<Hero, bool> <>9__5;
				Func<Hero, bool> func2;
				if ((func2 = <>9__5) == null)
				{
					func2 = (<>9__5 = (Hero h) => h.IsLord && !h.IsChild && h != brideHero && h != groomHero && h.IsFriend(brideHero) && !groomSide.Contains(h));
				}
				foreach (Hero hero6 in allAliveHeroes2.Where<Hero>(func2).Take<Hero>(MathF.Ceiling(3f - (float)brideSide.Count)))
				{
					brideSide.Enqueue(hero6);
				}
			}
			for (int i = 0; i < 6; i++)
			{
				bool flag = i <= 1 || i == 4;
				Queue<Hero> queue = (flag ? brideSide : groomSide);
				if (queue.Count > 0 && queue.Peek() != null)
				{
					list.Add(queue.Dequeue());
				}
				else
				{
					list.Add(null);
				}
			}
			return list;
		}

		// Token: 0x040006DA RID: 1754
		private const int NumberOfAudienceHeroes = 6;

		// Token: 0x040006DE RID: 1758
		private readonly CampaignTime _creationCampaignTime;
	}
}
