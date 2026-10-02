using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x02000223 RID: 547
	public sealed class NarrativeMenuOption
	{
		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002101 RID: 8449 RVA: 0x00094166 File Offset: 0x00092366
		public TextObject PositiveEffectText
		{
			get
			{
				return this.Args.PositiveEffectText;
			}
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x00094174 File Offset: 0x00092374
		public NarrativeMenuOption(string stringId, TextObject text, TextObject descriptionText, GetNarrativeMenuOptionArgsDelegate getNarrativeMenuOptionArgs, NarrativeMenuOptionOnConditionDelegate onCondition, NarrativeMenuOptionOnSelectDelegate onSelect, NarrativeMenuOptionOnConsequenceDelegate onConsequence)
		{
			this.StringId = stringId;
			this.Text = text;
			this.DescriptionText = descriptionText;
			this._onConditionInternal = onCondition;
			this._onSelectInternal = onSelect;
			this._onConsequenceInternal = onConsequence;
			this._getNarrativeMenuOptionArgs = getNarrativeMenuOptionArgs;
			this.Args = new NarrativeMenuOptionArgs();
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x000941C7 File Offset: 0x000923C7
		public bool OnCondition(CharacterCreationManager characterCreationManager)
		{
			return this._onConditionInternal == null || this._onConditionInternal(characterCreationManager);
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x000941E0 File Offset: 0x000923E0
		public void OnSelect(CharacterCreationManager characterCreationManager)
		{
			GetNarrativeMenuOptionArgsDelegate getNarrativeMenuOptionArgs = this._getNarrativeMenuOptionArgs;
			if (getNarrativeMenuOptionArgs != null)
			{
				getNarrativeMenuOptionArgs(this.Args);
			}
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.IsHuman)
				{
					narrativeMenuCharacter.SetRightHandItem("");
					narrativeMenuCharacter.SetLeftHandItem("");
					narrativeMenuCharacter.EquipLeftHandItemWithEquipmentIndex(EquipmentIndex.WeaponItemBeginSlot);
					narrativeMenuCharacter.EquipRightHandItemWithEquipmentIndex(EquipmentIndex.Weapon1);
				}
			}
			NarrativeMenuOptionOnSelectDelegate onSelectInternal = this._onSelectInternal;
			if (onSelectInternal == null)
			{
				return;
			}
			onSelectInternal(characterCreationManager);
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x00094288 File Offset: 0x00092488
		public void OnConsequence(CharacterCreationManager characterCreationManager)
		{
			NarrativeMenuOptionOnConsequenceDelegate onConsequenceInternal = this._onConsequenceInternal;
			if (onConsequenceInternal == null)
			{
				return;
			}
			onConsequenceInternal(characterCreationManager);
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x0009429B File Offset: 0x0009249B
		public void SetOnCondition(NarrativeMenuOptionOnConditionDelegate onCondition)
		{
			this._onConditionInternal = onCondition;
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x000942A4 File Offset: 0x000924A4
		public void SetOnSelect(NarrativeMenuOptionOnSelectDelegate onSelect)
		{
			this._onSelectInternal = onSelect;
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x000942AD File Offset: 0x000924AD
		public void SetOnConsequence(NarrativeMenuOptionOnConsequenceDelegate onConsequence)
		{
			this._onConsequenceInternal = onConsequence;
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x000942B8 File Offset: 0x000924B8
		public void ApplyFinalEffects(CharacterCreationContent characterCreationContent)
		{
			characterCreationContent.ApplySkillAndAttributeEffects(this.Args.AffectedSkills.ToList<SkillObject>(), this.Args.FocusToAdd, this.Args.SkillLevelToAdd, this.Args.EffectedAttribute, this.Args.AttributeLevelToAdd, this.Args.AffectedTraits.ToList<TraitObject>(), this.Args.TraitLevelToAdd, this.Args.RenownToAdd, this.Args.GoldToAdd, this.Args.UnspentFocusToAdd, this.Args.UnspentAttributeToAdd);
		}

		// Token: 0x040009A6 RID: 2470
		public readonly string StringId;

		// Token: 0x040009A7 RID: 2471
		public readonly TextObject Text;

		// Token: 0x040009A8 RID: 2472
		public readonly TextObject DescriptionText;

		// Token: 0x040009A9 RID: 2473
		private NarrativeMenuOptionOnConditionDelegate _onConditionInternal;

		// Token: 0x040009AA RID: 2474
		private NarrativeMenuOptionOnSelectDelegate _onSelectInternal;

		// Token: 0x040009AB RID: 2475
		private NarrativeMenuOptionOnConsequenceDelegate _onConsequenceInternal;

		// Token: 0x040009AC RID: 2476
		private readonly GetNarrativeMenuOptionArgsDelegate _getNarrativeMenuOptionArgs;

		// Token: 0x040009AD RID: 2477
		public readonly NarrativeMenuOptionArgs Args;
	}
}
