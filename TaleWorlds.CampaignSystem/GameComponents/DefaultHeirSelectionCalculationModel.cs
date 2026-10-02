using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000121 RID: 289
	public class DefaultHeirSelectionCalculationModel : HeirSelectionCalculationModel
	{
		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x060018A3 RID: 6307 RVA: 0x00076FB9 File Offset: 0x000751B9
		public override int HighestSkillPoint
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x00076FBC File Offset: 0x000751BC
		public override int CalculateHeirSelectionPoint(Hero candidateHeir, Hero deadHero, ref Hero maxSkillHero)
		{
			return DefaultHeirSelectionCalculationModel.CalculateHeirSelectionPointInternal(candidateHeir, deadHero, ref maxSkillHero);
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x00076FC8 File Offset: 0x000751C8
		private static int CalculateHeirSelectionPointInternal(Hero candidateHeir, Hero deadHero, ref Hero maxSkillHero)
		{
			int num = 0;
			if (!candidateHeir.IsFemale)
			{
				num += 10;
			}
			IOrderedEnumerable<Hero> orderedEnumerable = from x in candidateHeir.Clan.Heroes
				where x != deadHero
				select x into h
				orderby h.Age
				select h;
			Hero hero = orderedEnumerable.LastOrDefault<Hero>();
			float? num2 = ((hero != null) ? new float?(hero.Age) : null);
			Hero hero2 = orderedEnumerable.FirstOrDefault<Hero>();
			float? num3 = ((hero2 != null) ? new float?(hero2.Age) : null);
			float age = candidateHeir.Age;
			float? num4 = num2;
			if ((age == num4.GetValueOrDefault()) & (num4 != null))
			{
				num += 5;
			}
			else
			{
				float age2 = candidateHeir.Age;
				num4 = num3;
				if ((age2 == num4.GetValueOrDefault()) & (num4 != null))
				{
					num += -5;
				}
			}
			if (deadHero.Father == candidateHeir || deadHero.Mother == candidateHeir || candidateHeir.Father == deadHero || candidateHeir.Mother == deadHero || candidateHeir.Father == deadHero.Father || candidateHeir.Mother == deadHero.Mother)
			{
				num += 10;
			}
			Hero hero3 = deadHero.Father;
			while (hero3 != null && hero3.Father != null)
			{
				hero3 = hero3.Father;
			}
			if (((hero3 != null) ? hero3.Children : null) != null && DefaultHeirSelectionCalculationModel.DoesHaveSameBloodLine((hero3 != null) ? hero3.Children : null, candidateHeir))
			{
				num += 10;
			}
			int num5 = 0;
			foreach (SkillObject skillObject in Skills.All)
			{
				num5 += candidateHeir.GetSkillValue(skillObject);
			}
			int num6 = 0;
			foreach (SkillObject skillObject2 in Skills.All)
			{
				num6 += maxSkillHero.GetSkillValue(skillObject2);
			}
			if (num5 > num6)
			{
				maxSkillHero = candidateHeir;
			}
			return num;
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x00077210 File Offset: 0x00075410
		private static bool DoesHaveSameBloodLine(IEnumerable<Hero> children, Hero candidateHeir)
		{
			if (!children.Any<Hero>())
			{
				return false;
			}
			foreach (Hero hero in children)
			{
				if (hero == candidateHeir)
				{
					return true;
				}
				if (DefaultHeirSelectionCalculationModel.DoesHaveSameBloodLine(hero.Children, candidateHeir))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000812 RID: 2066
		private const int MaleHeirPoint = 10;

		// Token: 0x04000813 RID: 2067
		private const int EldestPoint = 5;

		// Token: 0x04000814 RID: 2068
		private const int YoungestPoint = -5;

		// Token: 0x04000815 RID: 2069
		private const int DirectDescendentPoint = 10;

		// Token: 0x04000816 RID: 2070
		private const int CollateralHeirPoint = 10;
	}
}
