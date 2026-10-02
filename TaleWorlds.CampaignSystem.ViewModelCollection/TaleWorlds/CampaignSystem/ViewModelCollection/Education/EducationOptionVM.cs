using System;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Education
{
	// Token: 0x020000FD RID: 253
	public class EducationOptionVM : StringItemWithActionVM
	{
		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x0600168C RID: 5772 RVA: 0x00058733 File Offset: 0x00056933
		// (set) Token: 0x0600168D RID: 5773 RVA: 0x0005873B File Offset: 0x0005693B
		public string OptionEffect { get; private set; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x0600168E RID: 5774 RVA: 0x00058744 File Offset: 0x00056944
		// (set) Token: 0x0600168F RID: 5775 RVA: 0x0005874C File Offset: 0x0005694C
		public string OptionDescription { get; private set; }

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06001690 RID: 5776 RVA: 0x00058755 File Offset: 0x00056955
		// (set) Token: 0x06001691 RID: 5777 RVA: 0x0005875D File Offset: 0x0005695D
		public EducationCampaignBehavior.EducationCharacterProperties[] CharacterProperties { get; private set; }

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06001692 RID: 5778 RVA: 0x00058766 File Offset: 0x00056966
		// (set) Token: 0x06001693 RID: 5779 RVA: 0x0005876E File Offset: 0x0005696E
		public string ActionID { get; private set; }

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06001694 RID: 5780 RVA: 0x00058777 File Offset: 0x00056977
		// (set) Token: 0x06001695 RID: 5781 RVA: 0x0005877F File Offset: 0x0005697F
		public ValueTuple<CharacterAttribute, int>[] OptionAttributes { get; private set; }

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06001696 RID: 5782 RVA: 0x00058788 File Offset: 0x00056988
		// (set) Token: 0x06001697 RID: 5783 RVA: 0x00058790 File Offset: 0x00056990
		public ValueTuple<SkillObject, int>[] OptionSkills { get; private set; }

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06001698 RID: 5784 RVA: 0x00058799 File Offset: 0x00056999
		// (set) Token: 0x06001699 RID: 5785 RVA: 0x000587A1 File Offset: 0x000569A1
		public ValueTuple<SkillObject, int>[] OptionFocusPoints { get; private set; }

		// Token: 0x0600169A RID: 5786 RVA: 0x000587AC File Offset: 0x000569AC
		public EducationOptionVM(Action<object> onExecute, string optionId, TextObject optionText, TextObject optionDescription, TextObject optionEffect, bool isSelected, ValueTuple<CharacterAttribute, int>[] optionAttributes, ValueTuple<SkillObject, int>[] optionSkills, ValueTuple<SkillObject, int>[] optionFocusPoints, EducationCampaignBehavior.EducationCharacterProperties[] characterProperties)
			: base(onExecute, optionText.ToString(), optionId)
		{
			this.IsSelected = isSelected;
			this.CharacterProperties = characterProperties;
			this._optionTextObject = optionText;
			this._optionDescriptionObject = optionDescription;
			this._optionEffectObject = optionEffect;
			this.OptionAttributes = optionAttributes;
			this.OptionSkills = optionSkills;
			this.OptionFocusPoints = optionFocusPoints;
			this.RefreshValues();
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x0005880C File Offset: 0x00056A0C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.OptionEffect = this._optionEffectObject.ToString();
			this.OptionDescription = this._optionDescriptionObject.ToString();
			base.ActionText = this._optionTextObject.ToString();
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x0600169C RID: 5788 RVA: 0x00058847 File Offset: 0x00056A47
		// (set) Token: 0x0600169D RID: 5789 RVA: 0x0005884F File Offset: 0x00056A4F
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x04000A46 RID: 2630
		private readonly TextObject _optionTextObject;

		// Token: 0x04000A47 RID: 2631
		private readonly TextObject _optionDescriptionObject;

		// Token: 0x04000A48 RID: 2632
		private readonly TextObject _optionEffectObject;

		// Token: 0x04000A49 RID: 2633
		private bool _isSelected;
	}
}
