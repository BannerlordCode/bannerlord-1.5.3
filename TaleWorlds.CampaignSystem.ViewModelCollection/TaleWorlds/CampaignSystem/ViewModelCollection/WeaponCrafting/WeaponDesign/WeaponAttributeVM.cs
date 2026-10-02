using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x0200010A RID: 266
	public class WeaponAttributeVM : ViewModel
	{
		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x060017C1 RID: 6081 RVA: 0x0005B6FA File Offset: 0x000598FA
		public DamageTypes DamageType { get; }

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x060017C2 RID: 6082 RVA: 0x0005B702 File Offset: 0x00059902
		public CraftingTemplate.CraftingStatTypes AttributeType { get; }

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x060017C3 RID: 6083 RVA: 0x0005B70A File Offset: 0x0005990A
		public float AttributeValue { get; }

		// Token: 0x060017C4 RID: 6084 RVA: 0x0005B714 File Offset: 0x00059914
		public WeaponAttributeVM(CraftingTemplate.CraftingStatTypes type, DamageTypes damageType, string attributeName, float attributeValue)
		{
			this.AttributeType = type;
			this.DamageType = damageType;
			this.AttributeValue = attributeValue;
			string text = ((this.AttributeValue > 100f) ? attributeValue.ToString("F0") : attributeValue.ToString("F1"));
			string text2 = "<span style=\"Value\">" + text + "</span>";
			TextObject textObject = new TextObject("{=!}{ATTR_NAME}{ATTR_VALUE_RTT}", null);
			textObject.SetTextVariable("ATTR_NAME", attributeName);
			textObject.SetTextVariable("ATTR_VALUE_RTT", text2);
			this.AttributeFieldText = textObject.ToString();
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x060017C5 RID: 6085 RVA: 0x0005B7A8 File Offset: 0x000599A8
		// (set) Token: 0x060017C6 RID: 6086 RVA: 0x0005B7B0 File Offset: 0x000599B0
		[DataSourceProperty]
		public string AttributeFieldText
		{
			get
			{
				return this._attributeFieldText;
			}
			set
			{
				if (value != this._attributeFieldText)
				{
					this._attributeFieldText = value;
					base.OnPropertyChangedWithValue<string>(value, "AttributeFieldText");
				}
			}
		}

		// Token: 0x04000AD5 RID: 2773
		private string _attributeFieldText;
	}
}
