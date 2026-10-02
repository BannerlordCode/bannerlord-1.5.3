using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x02000224 RID: 548
	public class NarrativeMenuOptionArgs
	{
		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x0600210A RID: 8458 RVA: 0x0009434E File Offset: 0x0009254E
		// (set) Token: 0x0600210B RID: 8459 RVA: 0x00094356 File Offset: 0x00092556
		public MBList<SkillObject> AffectedSkills { get; private set; }

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x0600210C RID: 8460 RVA: 0x0009435F File Offset: 0x0009255F
		// (set) Token: 0x0600210D RID: 8461 RVA: 0x00094367 File Offset: 0x00092567
		public int SkillLevelToAdd { get; private set; }

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x0600210E RID: 8462 RVA: 0x00094370 File Offset: 0x00092570
		// (set) Token: 0x0600210F RID: 8463 RVA: 0x00094378 File Offset: 0x00092578
		public MBList<TraitObject> AffectedTraits { get; private set; }

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002110 RID: 8464 RVA: 0x00094381 File Offset: 0x00092581
		// (set) Token: 0x06002111 RID: 8465 RVA: 0x00094389 File Offset: 0x00092589
		public int TraitLevelToAdd { get; private set; }

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002112 RID: 8466 RVA: 0x00094392 File Offset: 0x00092592
		// (set) Token: 0x06002113 RID: 8467 RVA: 0x0009439A File Offset: 0x0009259A
		public int FocusToAdd { get; private set; }

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06002114 RID: 8468 RVA: 0x000943A3 File Offset: 0x000925A3
		// (set) Token: 0x06002115 RID: 8469 RVA: 0x000943AB File Offset: 0x000925AB
		public int UnspentFocusToAdd { get; private set; }

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06002116 RID: 8470 RVA: 0x000943B4 File Offset: 0x000925B4
		// (set) Token: 0x06002117 RID: 8471 RVA: 0x000943BC File Offset: 0x000925BC
		public CharacterAttribute EffectedAttribute { get; private set; }

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06002118 RID: 8472 RVA: 0x000943C5 File Offset: 0x000925C5
		// (set) Token: 0x06002119 RID: 8473 RVA: 0x000943CD File Offset: 0x000925CD
		public int AttributeLevelToAdd { get; private set; }

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x0600211A RID: 8474 RVA: 0x000943D6 File Offset: 0x000925D6
		// (set) Token: 0x0600211B RID: 8475 RVA: 0x000943DE File Offset: 0x000925DE
		public int UnspentAttributeToAdd { get; private set; }

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x0600211C RID: 8476 RVA: 0x000943E7 File Offset: 0x000925E7
		// (set) Token: 0x0600211D RID: 8477 RVA: 0x000943EF File Offset: 0x000925EF
		public int RenownToAdd { get; private set; }

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x0600211E RID: 8478 RVA: 0x000943F8 File Offset: 0x000925F8
		// (set) Token: 0x0600211F RID: 8479 RVA: 0x00094400 File Offset: 0x00092600
		public int GoldToAdd { get; private set; }

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06002120 RID: 8480 RVA: 0x0009440C File Offset: 0x0009260C
		public TextObject PositiveEffectText
		{
			get
			{
				return this.GetPositiveEffectText(this.AffectedSkills.ToMBList<SkillObject>(), this.EffectedAttribute, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, this.AffectedTraits.ToMBList<TraitObject>(), this.TraitLevelToAdd, this.RenownToAdd, this.GoldToAdd, this.UnspentFocusToAdd, this.UnspentAttributeToAdd);
			}
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x0009446B File Offset: 0x0009266B
		public NarrativeMenuOptionArgs()
		{
			this.AffectedSkills = new MBList<SkillObject>();
			this.AffectedTraits = new MBList<TraitObject>();
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x00094489 File Offset: 0x00092689
		public void SetAffectedSkills(SkillObject[] affectedSkills)
		{
			this.AffectedSkills = affectedSkills.ToMBList<SkillObject>();
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x00094497 File Offset: 0x00092697
		public void SetFocusToSkills(int focusToAdd)
		{
			this.FocusToAdd = focusToAdd;
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x000944A0 File Offset: 0x000926A0
		public void SetLevelToSkills(int levelToAdd)
		{
			this.SkillLevelToAdd = levelToAdd;
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x000944A9 File Offset: 0x000926A9
		public void SetAffectedTraits(TraitObject[] affectedTraits)
		{
			this.AffectedTraits = affectedTraits.ToMBList<TraitObject>();
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x000944B7 File Offset: 0x000926B7
		public void SetLevelToTraits(int levelToAdd)
		{
			this.TraitLevelToAdd = levelToAdd;
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x000944C0 File Offset: 0x000926C0
		public void SetLevelToAttribute(CharacterAttribute characterAttribute, int levelToAdd)
		{
			this.EffectedAttribute = characterAttribute;
			this.AttributeLevelToAdd = levelToAdd;
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x000944D0 File Offset: 0x000926D0
		public void SetRenownToAdd(int value)
		{
			this.RenownToAdd = value;
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x000944D9 File Offset: 0x000926D9
		public void SetUnspentFocusToAdd(int value)
		{
			this.UnspentFocusToAdd = value;
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x000944E2 File Offset: 0x000926E2
		public void SetUnspentAttributeToAdd(int value)
		{
			this.UnspentAttributeToAdd = value;
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x000944EC File Offset: 0x000926EC
		private TextObject GetPositiveEffectText(MBList<SkillObject> skills, CharacterAttribute attribute, int focusToAdd = 0, int skillLevelToAdd = 0, int attributeLevelToAdd = 0, MBList<TraitObject> traits = null, int traitLevelToAdd = 0, int renownToAdd = 0, int goldToAdd = 0, int unspentFocustoAdd = 0, int unspentAttributeToAdd = 0)
		{
			TextObject textObject;
			if (skills.Count == 3)
			{
				textObject = new TextObject("{=jeWV2uV3}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE}, {SKILL_TWO} and {SKILL_THREE}{NEWLINE}{ATTR_VALUE} Attribute {?IS_PLURAL_ATR}Points{?}Point{\\?} to {ATTR_NAME}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}", null);
				textObject.SetTextVariable("SKILL_ONE", skills.ElementAt<SkillObject>(0).Name);
				textObject.SetTextVariable("SKILL_TWO", skills.ElementAt<SkillObject>(1).Name);
				textObject.SetTextVariable("SKILL_THREE", skills.ElementAt<SkillObject>(2).Name);
			}
			else if (skills.Count == 2)
			{
				textObject = new TextObject("{=5JTEvvaO}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE} and {SKILL_TWO}{NEWLINE}{ATTR_VALUE} Attribute {?IS_PLURAL_ATR}Points{?}Point{\\?} to {ATTR_NAME}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}", null);
				textObject.SetTextVariable("SKILL_ONE", skills.ElementAt<SkillObject>(0).Name);
				textObject.SetTextVariable("SKILL_TWO", skills.ElementAt<SkillObject>(1).Name);
			}
			else if (skills.Count == 1)
			{
				textObject = new TextObject("{=uw2kKrQk}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE}{NEWLINE}{ATTR_VALUE} Attribute {?IS_PLURAL_ATR}Points{?}Point{\\?} to {ATTR_NAME}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}", null);
				textObject.SetTextVariable("SKILL_ONE", skills.ElementAt<SkillObject>(0).Name);
			}
			else
			{
				textObject = new TextObject("{=NDWdnpI5}{UNSPENT_FOCUS_VALUE} unspent Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?}{NEWLINE}{UNSPENT_ATTR_VALUE} unspent Attribute {?IS_PLURAL_ATR}Points{?}Point{\\?}", null);
			}
			if (skills.Count > 0)
			{
				textObject.SetTextVariable("FOCUS_VALUE", focusToAdd);
				textObject.SetTextVariable("EXP_VALUE", skillLevelToAdd);
				textObject.SetTextVariable("ATTR_VALUE", attributeLevelToAdd);
				textObject.SetTextVariable("IS_PLURAL_SKILL", (skillLevelToAdd > 1) ? 1 : 0);
				textObject.SetTextVariable("IS_PLURAL_FOCUS", (focusToAdd > 1) ? 1 : 0);
				textObject.SetTextVariable("IS_PLURAL_ATR", (attributeLevelToAdd > 1) ? 1 : 0);
			}
			else
			{
				textObject.SetTextVariable("IS_PLURAL_FOCUS", (unspentFocustoAdd > 1) ? 1 : 0);
				textObject.SetTextVariable("IS_PLURAL_ATR", (unspentAttributeToAdd > 1) ? 1 : 0);
			}
			if (attribute != null)
			{
				textObject.SetTextVariable("ATTR_NAME", attribute.Name);
			}
			textObject.SetTextVariable("UNSPENT_FOCUS_VALUE", unspentFocustoAdd);
			textObject.SetTextVariable("UNSPENT_ATTR_VALUE", unspentAttributeToAdd);
			if (traits != null && traits.Count > 0 && traits.Count < 4)
			{
				TextObject textObject2 = TextObject.GetEmpty();
				if (traits.Count == 1)
				{
					textObject2 = new TextObject("{=DuQvj7zd}{newline}+{VALUE} to {TRAIT_NAME}", null);
					textObject2.SetTextVariable("TRAIT_NAME", traits.ElementAt<TraitObject>(0).Name);
				}
				else if (traits.Count == 2)
				{
					textObject2 = new TextObject("{=F1syZDs4}{newline}+{VALUE} to {TRAIT_NAME_ONE} and {TRAIT_NAME_TWO}", null);
					textObject2.SetTextVariable("TRAIT_NAME_ONE", traits.ElementAt<TraitObject>(0).Name);
					textObject2.SetTextVariable("TRAIT_NAME_TWO", traits.ElementAt<TraitObject>(1).Name);
				}
				else if (traits.Count == 3)
				{
					textObject2 = new TextObject("{=i20baAus}{newline}+{VALUE} to {TRAIT_NAME_ONE}, {TRAIT_NAME_TWO} and {TRAIT_NAME_THREE}", null);
					textObject2.SetTextVariable("TRAIT_NAME_ONE", traits.ElementAt<TraitObject>(0).Name);
					textObject2.SetTextVariable("TRAIT_NAME_TWO", traits.ElementAt<TraitObject>(1).Name);
					textObject2.SetTextVariable("TRAIT_NAME_THREE", traits.ElementAt<TraitObject>(2).Name);
				}
				if (!textObject2.IsEmpty())
				{
					textObject.SetTextVariable("TRAIT_DESC", textObject2);
					textObject2.SetTextVariable("VALUE", traitLevelToAdd);
				}
			}
			else
			{
				textObject.SetTextVariable("TRAIT_DESC", TextObject.GetEmpty());
			}
			if (renownToAdd > 0)
			{
				TextObject textObject3 = new TextObject("{=KXtaJNo4}{newline}+{VALUE} renown", null);
				textObject3.SetTextVariable("VALUE", renownToAdd);
				textObject.SetTextVariable("RENOWN_DESC", textObject3);
			}
			else
			{
				textObject.SetTextVariable("RENOWN_DESC", TextObject.GetEmpty());
			}
			if (goldToAdd > 0)
			{
				TextObject textObject4 = new TextObject("{=YBqmnNGv}{newline}+{VALUE} gold", null);
				textObject4.SetTextVariable("VALUE", goldToAdd);
				textObject.SetTextVariable("GOLD_DESC", textObject4);
			}
			else
			{
				textObject.SetTextVariable("GOLD_DESC", TextObject.GetEmpty());
			}
			return textObject;
		}
	}
}
