using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000108 RID: 264
	public class DefaultCharacterDevelopmentModel : CharacterDevelopmentModel
	{
		// Token: 0x06001761 RID: 5985 RVA: 0x0006C8F3 File Offset: 0x0006AAF3
		public DefaultCharacterDevelopmentModel()
		{
			this.InitializeSkillsRequiredForLevel();
			this.InitializeXpRequiredForSkillLevel();
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x0006C924 File Offset: 0x0006AB24
		public void InitializeSkillsRequiredForLevel()
		{
			int num = 1000;
			int num2 = 1;
			this._skillsRequiredForLevel[0] = 0;
			this._skillsRequiredForLevel[1] = 1;
			for (int i = 2; i < this._skillsRequiredForLevel.Length; i++)
			{
				num2 += num;
				this._skillsRequiredForLevel[i] = num2;
				num += 1000 + num / 5;
			}
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x0006C978 File Offset: 0x0006AB78
		public void InitializeXpRequiredForSkillLevel()
		{
			int num = 30;
			this._xpRequiredForSkillLevel[0] = num;
			for (int i = 1; i < 1024; i++)
			{
				num += 10 + i;
				this._xpRequiredForSkillLevel[i] = this._xpRequiredForSkillLevel[i - 1] + num;
			}
			if (Campaign.Current.Options.AccelerationMode == GameAccelerationMode.Fast)
			{
				for (int j = 0; j < this._xpRequiredForSkillLevel.Length; j++)
				{
					this._xpRequiredForSkillLevel[j] = (int)((float)this._xpRequiredForSkillLevel[j] * 0.3f);
				}
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x0006C9F9 File Offset: 0x0006ABF9
		public override int MaxFocusPerSkill
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001765 RID: 5989 RVA: 0x0006C9FC File Offset: 0x0006ABFC
		public override int MaxAttribute
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x0006CA00 File Offset: 0x0006AC00
		public override int SkillsRequiredForLevel(int level)
		{
			if (level > 62)
			{
				return Campaign.Current.Models.CharacterDevelopmentModel.GetMaxSkillPoint();
			}
			return this._skillsRequiredForLevel[level];
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x0006CA24 File Offset: 0x0006AC24
		public override int GetMaxSkillPoint()
		{
			return int.MaxValue;
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x0006CA2B File Offset: 0x0006AC2B
		public override int GetXpRequiredForSkillLevel(int skillLevel)
		{
			if (skillLevel > 1024)
			{
				skillLevel = 1024;
			}
			if (skillLevel <= 0)
			{
				return 0;
			}
			return this._xpRequiredForSkillLevel[skillLevel - 1];
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x0006CA4C File Offset: 0x0006AC4C
		public override int GetSkillLevelChange(Hero hero, SkillObject skill, float skillXp)
		{
			CharacterDevelopmentModel characterDevelopmentModel = Campaign.Current.Models.CharacterDevelopmentModel;
			int num = 0;
			int skillValue = hero.GetSkillValue(skill);
			for (int i = 0; i < 1024 - skillValue; i++)
			{
				int num2 = skillValue + i;
				if (num2 < 1023)
				{
					if (skillXp < (float)characterDevelopmentModel.GetXpRequiredForSkillLevel(num2 + 1))
					{
						break;
					}
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x0006CAA8 File Offset: 0x0006ACA8
		public override int GetXpAmountForSkillLevelChange(Hero hero, SkillObject skill, int skillLevelChange)
		{
			CharacterDevelopmentModel characterDevelopmentModel = Campaign.Current.Models.CharacterDevelopmentModel;
			int skillValue = hero.GetSkillValue(skill);
			return characterDevelopmentModel.GetXpRequiredForSkillLevel(skillValue + skillLevelChange + 1) - characterDevelopmentModel.GetXpRequiredForSkillLevel(skillValue + 1);
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x0006CAE4 File Offset: 0x0006ACE4
		public override void GetTraitLevelForTraitXp(Hero hero, TraitObject trait, int xpValue, out int traitLevel, out int clampedTraitXp)
		{
			clampedTraitXp = xpValue;
			int num = ((trait.MinValue < -1) ? (-6000) : ((trait.MinValue == -1) ? (-2500) : 0));
			int num2 = ((trait.MaxValue > 1) ? 6000 : ((trait.MaxValue == 1) ? 2500 : 0));
			if (xpValue > num2)
			{
				clampedTraitXp = num2;
			}
			else if (xpValue < num)
			{
				clampedTraitXp = num;
			}
			traitLevel = ((clampedTraitXp <= -4000) ? (-2) : ((clampedTraitXp <= -1000) ? (-1) : ((clampedTraitXp < 1000) ? 0 : ((clampedTraitXp < 4000) ? 1 : 2))));
			if (traitLevel < trait.MinValue)
			{
				traitLevel = trait.MinValue;
				return;
			}
			if (traitLevel > trait.MaxValue)
			{
				traitLevel = trait.MaxValue;
			}
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x0006CBAD File Offset: 0x0006ADAD
		public override int GetTraitXpRequiredForTraitLevel(TraitObject trait, int traitLevel)
		{
			if (traitLevel < -1)
			{
				return -4000;
			}
			if (traitLevel == -1)
			{
				return -1000;
			}
			if (traitLevel == 0)
			{
				return 0;
			}
			if (traitLevel != 1)
			{
				return 4000;
			}
			return 1000;
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x0600176D RID: 5997 RVA: 0x0006CBD7 File Offset: 0x0006ADD7
		public override int AttributePointsAtStart
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x0600176E RID: 5998 RVA: 0x0006CBDB File Offset: 0x0006ADDB
		public override int LevelsPerAttributePoint
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x0600176F RID: 5999 RVA: 0x0006CBDE File Offset: 0x0006ADDE
		public override int FocusPointsPerLevel
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06001770 RID: 6000 RVA: 0x0006CBE1 File Offset: 0x0006ADE1
		public override int FocusPointsAtStart
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06001771 RID: 6001 RVA: 0x0006CBE4 File Offset: 0x0006ADE4
		public override int MaxSkillRequiredForEpicPerkBonus
		{
			get
			{
				return 250;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06001772 RID: 6002 RVA: 0x0006CBEB File Offset: 0x0006ADEB
		public override int MinSkillRequiredForEpicPerkBonus
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x0006CBF4 File Offset: 0x0006ADF4
		public override ExplainedNumber CalculateLearningLimit(IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes, int focusValue, SkillObject skill, bool includeDescriptions = false)
		{
			float num = 0f;
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			foreach (CharacterAttribute characterAttribute in skill.Attributes)
			{
				num += (float)characterAttributes.GetPropertyValue(characterAttribute);
			}
			float num2 = num / (float)skill.Attributes.Length;
			explainedNumber.Add(Math.Max(0f, (num2 - 1f) * 10f), DefaultCharacterDevelopmentModel._attributeEffectText, null);
			explainedNumber.Add((float)(focusValue * 30), DefaultCharacterDevelopmentModel._skillFocusText, null);
			explainedNumber.LimitMin(0f);
			return explainedNumber;
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x0006CC98 File Offset: 0x0006AE98
		public override ExplainedNumber CalculateLearningRate(IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes, int focusValue, int skillValue, SkillObject skill, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(1.25f, includeDescriptions, null);
			float num = 0f;
			foreach (CharacterAttribute characterAttribute in skill.Attributes)
			{
				num += (float)characterAttributes.GetPropertyValue(characterAttribute);
			}
			float num2 = num / (float)skill.Attributes.Length;
			explainedNumber.AddFactor(0.4f * num2, DefaultCharacterDevelopmentModel._attributeEffectText);
			int num3 = MathF.Round(Campaign.Current.Models.CharacterDevelopmentModel.CalculateLearningLimit(characterAttributes, focusValue, skill, false).ResultNumber);
			explainedNumber.AddFactor((float)focusValue * 1f, DefaultCharacterDevelopmentModel._skillFocusText);
			if (skillValue > num3)
			{
				int num4 = skillValue - num3;
				explainedNumber.AddFactor(-1f - 0.1f * (float)num4, DefaultCharacterDevelopmentModel._overLimitText);
			}
			explainedNumber.LimitMin(0f);
			return explainedNumber;
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x0006CD7C File Offset: 0x0006AF7C
		public override SkillObject GetNextSkillToAddFocus(Hero hero)
		{
			SkillObject skillObject = null;
			float num = float.MinValue;
			foreach (SkillObject skillObject2 in Skills.All)
			{
				if (hero.HeroDeveloper.CanAddFocusToSkill(skillObject2))
				{
					int focus = hero.HeroDeveloper.GetFocus(skillObject2);
					float num2 = (float)hero.GetSkillValue(skillObject2) - Campaign.Current.Models.CharacterDevelopmentModel.CalculateLearningLimit(hero.CharacterAttributes, focus, skillObject2, false).ResultNumber;
					if (num2 > num)
					{
						num = num2;
						skillObject = skillObject2;
					}
				}
			}
			return skillObject;
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x0006CE28 File Offset: 0x0006B028
		public override CharacterAttribute GetNextAttributeToUpgrade(Hero hero)
		{
			CharacterAttribute characterAttribute = null;
			float num = float.MinValue;
			using (List<CharacterAttribute>.Enumerator enumerator = Attributes.All.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharacterAttribute currentAttribute = enumerator.Current;
					int attributeValue = hero.GetAttributeValue(currentAttribute);
					if (attributeValue < Campaign.Current.Models.CharacterDevelopmentModel.MaxAttribute)
					{
						float num2 = 0f;
						if (attributeValue == 0)
						{
							num2 = float.MaxValue;
						}
						else
						{
							float num3 = 0f;
							List<SkillObject> list = Skills.All.Where<SkillObject>((SkillObject skill) => skill.Attributes.Contains(currentAttribute)).ToList<SkillObject>();
							foreach (SkillObject skillObject in list)
							{
								num3 += MathF.Max(0f, (float)(75 + hero.GetSkillValue(skillObject)) - Campaign.Current.Models.CharacterDevelopmentModel.CalculateLearningLimit(hero.CharacterAttributes, hero.HeroDeveloper.GetFocus(skillObject), skillObject, false).ResultNumber);
							}
							num2 += num3 / (float)list.Count;
							int num4 = 1;
							foreach (CharacterAttribute characterAttribute2 in Attributes.All)
							{
								if (characterAttribute2 != currentAttribute)
								{
									int attributeValue2 = hero.GetAttributeValue(characterAttribute2);
									if (num4 < attributeValue2)
									{
										num4 = attributeValue2;
									}
								}
							}
							float num5 = MathF.Sqrt((float)num4 / (float)attributeValue);
							num2 *= num5;
						}
						if (num2 > num)
						{
							num = num2;
							characterAttribute = currentAttribute;
						}
					}
				}
			}
			return characterAttribute;
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x0006D030 File Offset: 0x0006B230
		public override PerkObject GetNextPerkToChoose(Hero hero, PerkObject perk)
		{
			PerkObject perkObject = perk;
			if (perk.AlternativePerk != null && MBRandom.RandomFloat < 0.5f)
			{
				perkObject = perk.AlternativePerk;
			}
			return perkObject;
		}

		// Token: 0x040007BD RID: 1981
		private const int MaxCharacterLevels = 62;

		// Token: 0x040007BE RID: 1982
		private const int SkillPointsAtLevel1 = 1;

		// Token: 0x040007BF RID: 1983
		private const int SkillPointsGainNeededInitialValue = 1000;

		// Token: 0x040007C0 RID: 1984
		private const int SkillPointsGainNeededIncreasePerLevel = 1000;

		// Token: 0x040007C1 RID: 1985
		private readonly int[] _skillsRequiredForLevel = new int[63];

		// Token: 0x040007C2 RID: 1986
		private const int FocusPointsPerLevelConst = 1;

		// Token: 0x040007C3 RID: 1987
		private const int LevelsPerAttributePointConst = 4;

		// Token: 0x040007C4 RID: 1988
		private const int FocusPointsAtStartConst = 5;

		// Token: 0x040007C5 RID: 1989
		private const int AttributePointsAtStartConst = 15;

		// Token: 0x040007C6 RID: 1990
		private const int MaxSkillLevels = 1024;

		// Token: 0x040007C7 RID: 1991
		private readonly int[] _xpRequiredForSkillLevel = new int[1024];

		// Token: 0x040007C8 RID: 1992
		private const int XpRequirementForFirstLevel = 30;

		// Token: 0x040007C9 RID: 1993
		private const int MaxSkillPoint = 2147483647;

		// Token: 0x040007CA RID: 1994
		private const float BaseLearningRate = 1.25f;

		// Token: 0x040007CB RID: 1995
		private const int TraitThreshold2 = 4000;

		// Token: 0x040007CC RID: 1996
		private const int TraitMaxValue1 = 2500;

		// Token: 0x040007CD RID: 1997
		private const int TraitThreshold1 = 1000;

		// Token: 0x040007CE RID: 1998
		private const int TraitMaxValue2 = 6000;

		// Token: 0x040007CF RID: 1999
		private const int SkillLevelVariant = 10;

		// Token: 0x040007D0 RID: 2000
		private static readonly TextObject _attributeEffectText = new TextObject("{=jlrvzwFb}Attribute Effect", null);

		// Token: 0x040007D1 RID: 2001
		private static readonly TextObject _skillFocusText = new TextObject("{=MRktqZwu}Skill Focus", null);

		// Token: 0x040007D2 RID: 2002
		private static readonly TextObject _overLimitText = new TextObject("{=bcA7ZuyO}Learning Limit Exceeded", null);
	}
}
