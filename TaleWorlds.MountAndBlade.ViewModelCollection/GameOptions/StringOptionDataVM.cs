using System;
using System.Collections.Generic;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x02000074 RID: 116
	public class StringOptionDataVM : GenericOptionDataVM
	{
		// Token: 0x06000948 RID: 2376 RVA: 0x0001F544 File Offset: 0x0001D744
		public StringOptionDataVM(OptionsVM optionsVM, ISelectionOptionData option, TextObject name, TextObject description)
			: base(optionsVM, option, name, description, OptionsVM.OptionsDataType.MultipleSelectionOption)
		{
			this.Selector = new SelectorVM<SelectorItemVM>(0, null);
			this._selectionOptionData = option;
			this.UpdateData(true);
			this._initialValue = (int)this.Option.GetValue(false);
			this.Selector.SelectedIndex = this._initialValue;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0001F59C File Offset: 0x0001D79C
		public override void UpdateData(bool initialUpdate)
		{
			base.UpdateData(initialUpdate);
			IEnumerable<SelectionData> selectableOptionNames = this._selectionOptionData.GetSelectableOptionNames();
			this.Selector.SetOnChangeAction(null);
			bool flag = (int)this.Option.GetValue(true) != this.Selector.SelectedIndex;
			this.Selector.ItemList.Clear();
			foreach (SelectionData selectionData in selectableOptionNames)
			{
				if (selectionData.IsLocalizationId)
				{
					TextObject textObject = Module.CurrentModule.GlobalTextManager.FindText(selectionData.Data, null);
					this.Selector.AddItem(new SelectorItemVM(textObject));
				}
				else
				{
					this.Selector.AddItem(new SelectorItemVM(selectionData.Data));
				}
			}
			int num = (int)this.Option.GetValue(!initialUpdate);
			if (this.Selector.ItemList.Count > 0 && num == -1)
			{
				num = 0;
			}
			this.Selector.SelectedIndex = num;
			this.Selector.SetOnChangeAction(new Action<SelectorVM<SelectorItemVM>>(this.UpdateValue));
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0001F6C0 File Offset: 0x0001D8C0
		public override void RefreshValues()
		{
			base.RefreshValues();
			SelectorVM<SelectorItemVM> selector = this.Selector;
			if (selector == null)
			{
				return;
			}
			selector.RefreshValues();
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0001F6D8 File Offset: 0x0001D8D8
		public void UpdateValue(SelectorVM<SelectorItemVM> selector)
		{
			if (selector.SelectedIndex >= 0)
			{
				this.Option.SetValue((float)selector.SelectedIndex);
				this.Option.Commit();
				this._optionsVM.SetConfig(this.Option, (float)selector.SelectedIndex);
			}
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0001F718 File Offset: 0x0001D918
		public override void UpdateValue()
		{
			if (this.Selector.SelectedIndex >= 0 && (float)this.Selector.SelectedIndex != this.Option.GetValue(false))
			{
				this.Option.Commit();
				this._optionsVM.SetConfig(this.Option, (float)this.Selector.SelectedIndex);
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0001F775 File Offset: 0x0001D975
		public override void Cancel()
		{
			this.Selector.SelectedIndex = this._initialValue;
			this.UpdateValue();
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0001F78E File Offset: 0x0001D98E
		public override void SetValue(float value)
		{
			this.Selector.SelectedIndex = (int)value;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0001F79D File Offset: 0x0001D99D
		public override void ResetData()
		{
			this.Selector.SelectedIndex = (int)this.Option.GetDefaultValue();
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0001F7B6 File Offset: 0x0001D9B6
		public override bool IsChanged()
		{
			return this._initialValue != this.Selector.SelectedIndex;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0001F7CE File Offset: 0x0001D9CE
		public override void ApplyValue()
		{
			if (this._initialValue != this.Selector.SelectedIndex)
			{
				this._initialValue = this.Selector.SelectedIndex;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x0001F7F4 File Offset: 0x0001D9F4
		// (set) Token: 0x06000953 RID: 2387 RVA: 0x0001F803 File Offset: 0x0001DA03
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> Selector
		{
			get
			{
				SelectorVM<SelectorItemVM> selector = this._selector;
				return this._selector;
			}
			set
			{
				if (value != this._selector)
				{
					this._selector = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "Selector");
				}
			}
		}

		// Token: 0x0400042E RID: 1070
		private int _initialValue;

		// Token: 0x0400042F RID: 1071
		private ISelectionOptionData _selectionOptionData;

		// Token: 0x04000430 RID: 1072
		public SelectorVM<SelectorItemVM> _selector;
	}
}
