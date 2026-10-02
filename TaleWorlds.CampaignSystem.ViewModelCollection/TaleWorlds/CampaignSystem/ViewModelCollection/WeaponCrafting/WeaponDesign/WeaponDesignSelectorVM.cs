using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000113 RID: 275
	public class WeaponDesignSelectorVM : ViewModel
	{
		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x0600183A RID: 6202 RVA: 0x0005C7E3 File Offset: 0x0005A9E3
		public WeaponDesign Design { get; }

		// Token: 0x0600183B RID: 6203 RVA: 0x0005C7EC File Offset: 0x0005A9EC
		public WeaponDesignSelectorVM(WeaponDesign design, Action<WeaponDesignSelectorVM> onSelection)
		{
			this._onSelection = onSelection;
			this.Design = design;
			TextObject textObject = new TextObject("{=uZhHh7pm}Crafted {CURR_TEMPLATE_NAME}", null);
			textObject.SetTextVariable("CURR_TEMPLATE_NAME", design.Template.TemplateName);
			TextObject textObject2 = design.WeaponName ?? textObject;
			this.Name = textObject2.ToString();
			Crafting.GenerateItem(design, textObject2, Hero.MainHero.Culture, design.Template.ItemModifierGroup, ref this._generatedVisualItem, design.HashedCode);
			MBObjectManager.Instance.RegisterObject<ItemObject>(this._generatedVisualItem);
			this.Visual = new ItemImageIdentifierVM(this._generatedVisualItem, "");
			this.WeaponTypeCode = design.Template.StringId;
			this.Hint = new BasicTooltipViewModel(() => this.GetHint());
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x0005C8C0 File Offset: 0x0005AAC0
		private List<TooltipProperty> GetHint()
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			list.Add(new TooltipProperty("", this._generatedVisualItem.Name.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.Title));
			foreach (CraftingStatData craftingStatData in Crafting.GetStatDatasFromTemplate(0, this._generatedVisualItem, this.Design.Template))
			{
				if (craftingStatData.IsValid && craftingStatData.CurValue > 0f && craftingStatData.MaxValue > 0f)
				{
					list.Add(new TooltipProperty(craftingStatData.DescriptionText.ToString(), craftingStatData.CurValue.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
			}
			return list;
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x0005C990 File Offset: 0x0005AB90
		public void ExecuteSelect()
		{
			Action<WeaponDesignSelectorVM> onSelection = this._onSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this);
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x0005C9A3 File Offset: 0x0005ABA3
		public override void OnFinalize()
		{
			base.OnFinalize();
			MBObjectManager.Instance.UnregisterObject(this._generatedVisualItem);
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x0600183F RID: 6207 RVA: 0x0005C9BB File Offset: 0x0005ABBB
		// (set) Token: 0x06001840 RID: 6208 RVA: 0x0005C9C3 File Offset: 0x0005ABC3
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

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06001841 RID: 6209 RVA: 0x0005C9E1 File Offset: 0x0005ABE1
		// (set) Token: 0x06001842 RID: 6210 RVA: 0x0005C9E9 File Offset: 0x0005ABE9
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06001843 RID: 6211 RVA: 0x0005CA0C File Offset: 0x0005AC0C
		// (set) Token: 0x06001844 RID: 6212 RVA: 0x0005CA14 File Offset: 0x0005AC14
		[DataSourceProperty]
		public string WeaponTypeCode
		{
			get
			{
				return this._weaponTypeCode;
			}
			set
			{
				if (value != this._weaponTypeCode)
				{
					this._weaponTypeCode = value;
					base.OnPropertyChangedWithValue<string>(value, "WeaponTypeCode");
				}
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06001845 RID: 6213 RVA: 0x0005CA37 File Offset: 0x0005AC37
		// (set) Token: 0x06001846 RID: 6214 RVA: 0x0005CA3F File Offset: 0x0005AC3F
		[DataSourceProperty]
		public ItemImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06001847 RID: 6215 RVA: 0x0005CA5D File Offset: 0x0005AC5D
		// (set) Token: 0x06001848 RID: 6216 RVA: 0x0005CA65 File Offset: 0x0005AC65
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x04000B14 RID: 2836
		private readonly Action<WeaponDesignSelectorVM> _onSelection;

		// Token: 0x04000B15 RID: 2837
		private readonly ItemObject _generatedVisualItem;

		// Token: 0x04000B16 RID: 2838
		private bool _isSelected;

		// Token: 0x04000B17 RID: 2839
		private string _name;

		// Token: 0x04000B18 RID: 2840
		private string _weaponTypeCode;

		// Token: 0x04000B19 RID: 2841
		private ItemImageIdentifierVM _visual;

		// Token: 0x04000B1A RID: 2842
		private BasicTooltipViewModel _hint;
	}
}
