using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SandBox.AdvancedStartOptions;
using SandBox.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.CampaignStartingOptions
{
	// Token: 0x02000063 RID: 99
	public class StartingOptionVM : ViewModel
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060005FE RID: 1534 RVA: 0x00016340 File Offset: 0x00014540
		// (remove) Token: 0x060005FF RID: 1535 RVA: 0x00016374 File Offset: 0x00014574
		public static event Action<StartingOptionVM> OnOptionFocusBegin;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000600 RID: 1536 RVA: 0x000163A8 File Offset: 0x000145A8
		// (remove) Token: 0x06000601 RID: 1537 RVA: 0x000163DC File Offset: 0x000145DC
		public static event Action<StartingOptionVM> OnOptionFocusEnd;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000602 RID: 1538 RVA: 0x00016410 File Offset: 0x00014610
		// (remove) Token: 0x06000603 RID: 1539 RVA: 0x00016444 File Offset: 0x00014644
		public static event Action OnOptionChanged;

		// Token: 0x06000604 RID: 1540 RVA: 0x00016478 File Offset: 0x00014678
		public StartingOptionVM(AdvancedStartOption data, AdvancedStartOptions stagedOptions, Func<bool> getAdditionalIsDisabled = null)
		{
			this._data = data;
			this._stagedOptions = stagedOptions;
			this._getAdditionalIsDisabled = getAdditionalIsDisabled;
			this._nameTextObj = stagedOptions.GetOptionName(data.StringId) ?? Module.CurrentModule.GlobalTextManager.FindText("str_campaign_starting_options_name", data.StringId);
			TextObject textObject = ((data is ListAdvancedStartOption) ? null : stagedOptions.GetOptionDescription(data.StringId)) ?? Module.CurrentModule.GlobalTextManager.FindText("str_campaign_starting_options_description", data.StringId);
			this.DescriptionHint = new HintViewModel(textObject, null);
			BooleanAdvancedStartOption booleanAdvancedStartOption;
			FloatAdvancedStartOption floatAdvancedStartOption;
			IntAdvancedStartOption intAdvancedStartOption;
			ListAdvancedStartOption listAdvancedStartOption;
			UIntAdvancedStartOption uintAdvancedStartOption;
			if ((booleanAdvancedStartOption = data as BooleanAdvancedStartOption) != null)
			{
				this.OptionType = 0;
				this.ValueAsBoolean = booleanAdvancedStartOption.GetValue<bool>();
			}
			else if ((floatAdvancedStartOption = data as FloatAdvancedStartOption) != null)
			{
				this.OptionType = 1;
				this.MinRange = floatAdvancedStartOption.MinValue;
				this.MaxRange = floatAdvancedStartOption.MaxValue;
				this.IsDiscrete = false;
				this.ValueAsFloat = floatAdvancedStartOption.GetValue<float>();
				this.OnNumericValueUpdated();
			}
			else if ((intAdvancedStartOption = data as IntAdvancedStartOption) != null)
			{
				this.OptionType = 1;
				this.IsDiscrete = true;
				this.MinRange = (float)intAdvancedStartOption.MinValue;
				this.MaxRange = (float)intAdvancedStartOption.MaxValue;
				this.ValueAsFloat = (float)intAdvancedStartOption.GetValue<int>();
				this.OnNumericValueUpdated();
			}
			else if ((listAdvancedStartOption = data as ListAdvancedStartOption) != null)
			{
				this.OptionType = 2;
				this._listData = listAdvancedStartOption;
				this._selectionItems = listAdvancedStartOption.GetItems();
				this.Selector = new SelectorVM<SelectorItemVM>(this.GetSelectionItemNames(), this.GetSelectedIndex(), delegate(SelectorVM<SelectorItemVM> selector)
				{
					this.OnSelectionChanged(selector.SelectedIndex);
				});
			}
			else if ((uintAdvancedStartOption = data as UIntAdvancedStartOption) != null)
			{
				this.OptionType = 3;
				this.MinInt = (int)MathF.Max(uintAdvancedStartOption.MinValue, 0U);
				this.MaxInt = (int)MathF.Min(uintAdvancedStartOption.MaxValue, 2147483647U);
				this.ValueAsInt = (int)uintAdvancedStartOption.GetValue<uint>();
			}
			this.RefreshValues();
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00016664 File Offset: 0x00014864
		public StartingOptionVM(string stringId, Func<bool> getValue, Action<bool> setValue)
		{
			this._nameTextObj = Module.CurrentModule.GlobalTextManager.FindText("str_campaign_starting_options_name", stringId);
			TextObject textObject = Module.CurrentModule.GlobalTextManager.FindText("str_campaign_starting_options_description", stringId);
			this.DescriptionHint = new HintViewModel(textObject, null);
			this.OptionType = 0;
			this._setBoolValueCustom = setValue;
			this.ValueAsBoolean = getValue != null && getValue();
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x000166D5 File Offset: 0x000148D5
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._nameTextObj.ToString();
			this.RandomizeHint = new HintViewModel(new TextObject("{=NSSsxBHV}Randomize", null), null);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00016705 File Offset: 0x00014905
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM randomizeInputKey = this.RandomizeInputKey;
			if (randomizeInputKey == null)
			{
				return;
			}
			randomizeInputKey.OnFinalize();
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0001671D File Offset: 0x0001491D
		public void ExecuteFocusBegin()
		{
			this.IsFocused = true;
			Action<StartingOptionVM> onOptionFocusBegin = StartingOptionVM.OnOptionFocusBegin;
			if (onOptionFocusBegin == null)
			{
				return;
			}
			onOptionFocusBegin(this);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00016736 File Offset: 0x00014936
		public void ExecuteFocusEnd()
		{
			this.IsFocused = false;
			Action<StartingOptionVM> onOptionFocusEnd = StartingOptionVM.OnOptionFocusEnd;
			if (onOptionFocusEnd == null)
			{
				return;
			}
			onOptionFocusEnd(this);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00016750 File Offset: 0x00014950
		public void UpdateOptionState()
		{
			Func<bool> getAdditionalIsDisabled = this._getAdditionalIsDisabled;
			this.IsDisabled = getAdditionalIsDisabled != null && getAdditionalIsDisabled();
			if (this._data != null)
			{
				bool isHidden = this._data.GetIsHidden(this._stagedOptions);
				this.IsHidden = isHidden;
				TextObject optionName = this._stagedOptions.GetOptionName(this._data.StringId);
				if (optionName != null)
				{
					this._nameTextObj = optionName;
					this.Name = optionName.ToString();
				}
			}
			else
			{
				this.IsHidden = false;
			}
			if (this._selectionItems != null)
			{
				for (int i = 0; i < this.Selector.ItemList.Count; i++)
				{
					TextObject textObject;
					bool itemCondition = this._listData.GetItemCondition(this._selectionItems[i].Item1, this._stagedOptions, out textObject);
					this.Selector.ItemList[i].CanBeSelected = !itemCondition;
					this.Selector.ItemList[i].Hint = ((itemCondition && textObject != null) ? new HintViewModel(textObject, null) : null);
				}
				if (!this.Selector.SelectedItem.CanBeSelected)
				{
					this.Selector.ExecuteSelectNextItem();
				}
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00016888 File Offset: 0x00014A88
		private IEnumerable<TextObject> GetSelectionItemNames()
		{
			List<TextObject> list = new List<TextObject>(this._selectionItems.Count);
			for (int i = 0; i < this._selectionItems.Count; i++)
			{
				list.Add(this._stagedOptions.GetItemName(this._data.StringId, this._selectionItems[i].Item1));
			}
			return list;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x000168EC File Offset: 0x00014AEC
		private int GetSelectedIndex()
		{
			string value = this._listData.GetValue<string>();
			for (int i = 0; i < this._selectionItems.Count; i++)
			{
				if (object.Equals(this._selectionItems[i].Item1, value))
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00016937 File Offset: 0x00014B37
		private void OnSelectionChanged(int selectedIndex)
		{
			this._listData.SetValue<string>(this._selectionItems[selectedIndex].Item1);
			Action onOptionChanged = StartingOptionVM.OnOptionChanged;
			if (onOptionChanged == null)
			{
				return;
			}
			onOptionChanged();
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00016964 File Offset: 0x00014B64
		public TextObject GetComposedDescription()
		{
			if (this._data == null)
			{
				return null;
			}
			return this._stagedOptions.GetItemDescription(this._data.StringId);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00016988 File Offset: 0x00014B88
		public void ExecuteRandomize()
		{
			if (!this.AllowRandomization)
			{
				return;
			}
			int num = ((this.MaxInt == int.MaxValue) ? int.MaxValue : (this.MaxInt + 1));
			this.ValueAsInt = MBRandom.RandomInt(this.MinInt, num);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x000169CD File Offset: 0x00014BCD
		private void OnNumericValueUpdated()
		{
			this.ValueAsString = this._valueAsFloat.ToString(this.IsDiscrete ? "0" : "0.##");
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x000169F4 File Offset: 0x00014BF4
		public void SetRandomizeInputKey(HotKey hotkey)
		{
			this.RandomizeInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x00016A03 File Offset: 0x00014C03
		// (set) Token: 0x06000613 RID: 1555 RVA: 0x00016A0B File Offset: 0x00014C0B
		public InputKeyItemVM RandomizeInputKey
		{
			get
			{
				return this._randomizeInputKey;
			}
			set
			{
				if (value != this._randomizeInputKey)
				{
					this._randomizeInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "RandomizeInputKey");
				}
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x00016A29 File Offset: 0x00014C29
		// (set) Token: 0x06000615 RID: 1557 RVA: 0x00016A31 File Offset: 0x00014C31
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

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x00016A54 File Offset: 0x00014C54
		// (set) Token: 0x06000617 RID: 1559 RVA: 0x00016A5C File Offset: 0x00014C5C
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x00016A7A File Offset: 0x00014C7A
		// (set) Token: 0x06000619 RID: 1561 RVA: 0x00016A82 File Offset: 0x00014C82
		[DataSourceProperty]
		public bool IsHidden
		{
			get
			{
				return this._isHidden;
			}
			set
			{
				if (value != this._isHidden)
				{
					this._isHidden = value;
					base.OnPropertyChangedWithValue(value, "IsHidden");
				}
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x00016AA0 File Offset: 0x00014CA0
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x00016AA8 File Offset: 0x00014CA8
		[DataSourceProperty]
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChangedWithValue(value, "IsFocused");
				}
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00016AC6 File Offset: 0x00014CC6
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x00016ACE File Offset: 0x00014CCE
		[DataSourceProperty]
		public int OptionType
		{
			get
			{
				return this._optionType;
			}
			set
			{
				if (value != this._optionType)
				{
					this._optionType = value;
					base.OnPropertyChangedWithValue(value, "OptionType");
				}
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00016AEC File Offset: 0x00014CEC
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x00016AF4 File Offset: 0x00014CF4
		[DataSourceProperty]
		public bool ValueAsBoolean
		{
			get
			{
				return this._valueAsBoolean;
			}
			set
			{
				if (value != this._valueAsBoolean)
				{
					this._valueAsBoolean = value;
					base.OnPropertyChangedWithValue(value, "ValueAsBoolean");
					BooleanAdvancedStartOption booleanAdvancedStartOption;
					if ((booleanAdvancedStartOption = this._data as BooleanAdvancedStartOption) != null)
					{
						booleanAdvancedStartOption.SetValue<bool>(value);
					}
					Action<bool> setBoolValueCustom = this._setBoolValueCustom;
					if (setBoolValueCustom != null)
					{
						setBoolValueCustom(value);
					}
					Action onOptionChanged = StartingOptionVM.OnOptionChanged;
					if (onOptionChanged == null)
					{
						return;
					}
					onOptionChanged();
				}
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00016B54 File Offset: 0x00014D54
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x00016B5C File Offset: 0x00014D5C
		[DataSourceProperty]
		public float ValueAsFloat
		{
			get
			{
				return this._valueAsFloat;
			}
			set
			{
				if (value != this._valueAsFloat)
				{
					this._valueAsFloat = value;
					base.OnPropertyChangedWithValue(value, "ValueAsFloat");
					this.OnNumericValueUpdated();
					FloatAdvancedStartOption floatAdvancedStartOption;
					IntAdvancedStartOption intAdvancedStartOption;
					if ((floatAdvancedStartOption = this._data as FloatAdvancedStartOption) != null)
					{
						floatAdvancedStartOption.SetValue<float>(value);
					}
					else if ((intAdvancedStartOption = this._data as IntAdvancedStartOption) != null)
					{
						intAdvancedStartOption.SetValue<int>(MathF.Round(value));
					}
					Action onOptionChanged = StartingOptionVM.OnOptionChanged;
					if (onOptionChanged == null)
					{
						return;
					}
					onOptionChanged();
				}
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x00016BCD File Offset: 0x00014DCD
		// (set) Token: 0x06000623 RID: 1571 RVA: 0x00016BD5 File Offset: 0x00014DD5
		[DataSourceProperty]
		public string ValueAsString
		{
			get
			{
				return this._valueAsString;
			}
			set
			{
				if (value != this._valueAsString)
				{
					this._valueAsString = value;
					base.OnPropertyChangedWithValue<string>(value, "ValueAsString");
				}
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x00016BF8 File Offset: 0x00014DF8
		// (set) Token: 0x06000625 RID: 1573 RVA: 0x00016C00 File Offset: 0x00014E00
		[DataSourceProperty]
		public int ValueAsInt
		{
			get
			{
				return this._valueAsInt;
			}
			set
			{
				if (value != this._valueAsInt)
				{
					this._valueAsInt = value;
					base.OnPropertyChangedWithValue(value, "ValueAsInt");
					UIntAdvancedStartOption uintAdvancedStartOption;
					if ((uintAdvancedStartOption = this._data as UIntAdvancedStartOption) != null)
					{
						uintAdvancedStartOption.SetValue<uint>((uint)value);
					}
					Action onOptionChanged = StartingOptionVM.OnOptionChanged;
					if (onOptionChanged == null)
					{
						return;
					}
					onOptionChanged();
				}
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x00016C4E File Offset: 0x00014E4E
		// (set) Token: 0x06000627 RID: 1575 RVA: 0x00016C56 File Offset: 0x00014E56
		[DataSourceProperty]
		public int MinInt
		{
			get
			{
				return this._minInt;
			}
			set
			{
				if (value != this._minInt)
				{
					this._minInt = value;
					base.OnPropertyChangedWithValue(value, "MinInt");
				}
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x00016C74 File Offset: 0x00014E74
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x00016C7C File Offset: 0x00014E7C
		[DataSourceProperty]
		public int MaxInt
		{
			get
			{
				return this._maxInt;
			}
			set
			{
				if (value != this._maxInt)
				{
					this._maxInt = value;
					base.OnPropertyChangedWithValue(value, "MaxInt");
				}
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x00016C9A File Offset: 0x00014E9A
		// (set) Token: 0x0600062B RID: 1579 RVA: 0x00016CA2 File Offset: 0x00014EA2
		[DataSourceProperty]
		public bool AllowRandomization
		{
			get
			{
				return this._allowRandomization;
			}
			set
			{
				if (value != this._allowRandomization)
				{
					this._allowRandomization = value;
					base.OnPropertyChangedWithValue(value, "AllowRandomization");
				}
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x00016CC0 File Offset: 0x00014EC0
		// (set) Token: 0x0600062D RID: 1581 RVA: 0x00016CC8 File Offset: 0x00014EC8
		[DataSourceProperty]
		public float MinRange
		{
			get
			{
				return this._minRange;
			}
			set
			{
				if (value != this._minRange)
				{
					this._minRange = value;
					base.OnPropertyChangedWithValue(value, "MinRange");
				}
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x00016CE6 File Offset: 0x00014EE6
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x00016CEE File Offset: 0x00014EEE
		[DataSourceProperty]
		public float MaxRange
		{
			get
			{
				return this._maxRange;
			}
			set
			{
				if (value != this._maxRange)
				{
					this._maxRange = value;
					base.OnPropertyChangedWithValue(value, "MaxRange");
				}
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x00016D0C File Offset: 0x00014F0C
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x00016D14 File Offset: 0x00014F14
		[DataSourceProperty]
		public bool IsDiscrete
		{
			get
			{
				return this._isDiscrete;
			}
			set
			{
				if (value != this._isDiscrete)
				{
					this._isDiscrete = value;
					base.OnPropertyChangedWithValue(value, "IsDiscrete");
				}
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x00016D32 File Offset: 0x00014F32
		// (set) Token: 0x06000633 RID: 1587 RVA: 0x00016D3A File Offset: 0x00014F3A
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> Selector
		{
			get
			{
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

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x00016D58 File Offset: 0x00014F58
		// (set) Token: 0x06000635 RID: 1589 RVA: 0x00016D60 File Offset: 0x00014F60
		[DataSourceProperty]
		public HintViewModel DescriptionHint
		{
			get
			{
				return this._descriptionHint;
			}
			set
			{
				if (value != this._descriptionHint)
				{
					this._descriptionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DescriptionHint");
				}
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x00016D7E File Offset: 0x00014F7E
		// (set) Token: 0x06000637 RID: 1591 RVA: 0x00016D86 File Offset: 0x00014F86
		[DataSourceProperty]
		public HintViewModel RandomizeHint
		{
			get
			{
				return this._randomizeHint;
			}
			set
			{
				if (value != this._randomizeHint)
				{
					this._randomizeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RandomizeHint");
				}
			}
		}

		// Token: 0x040002FD RID: 765
		private readonly AdvancedStartOption _data;

		// Token: 0x040002FE RID: 766
		private readonly ListAdvancedStartOption _listData;

		// Token: 0x040002FF RID: 767
		private readonly AdvancedStartOptions _stagedOptions;

		// Token: 0x04000300 RID: 768
		[TupleElementNames(new string[] { "Identifier", "Condition" })]
		private readonly IReadOnlyList<ValueTuple<string, ListAdvancedStartOption.ListItemCondition>> _selectionItems;

		// Token: 0x04000301 RID: 769
		private readonly Action<bool> _setBoolValueCustom;

		// Token: 0x04000302 RID: 770
		private readonly Func<bool> _getAdditionalIsDisabled;

		// Token: 0x04000303 RID: 771
		private TextObject _nameTextObj;

		// Token: 0x04000304 RID: 772
		private InputKeyItemVM _randomizeInputKey;

		// Token: 0x04000305 RID: 773
		private string _name;

		// Token: 0x04000306 RID: 774
		private bool _isDisabled;

		// Token: 0x04000307 RID: 775
		private bool _isHidden;

		// Token: 0x04000308 RID: 776
		private bool _isFocused;

		// Token: 0x04000309 RID: 777
		private int _optionType;

		// Token: 0x0400030A RID: 778
		private bool _valueAsBoolean;

		// Token: 0x0400030B RID: 779
		private float _valueAsFloat;

		// Token: 0x0400030C RID: 780
		private string _valueAsString;

		// Token: 0x0400030D RID: 781
		private int _valueAsInt;

		// Token: 0x0400030E RID: 782
		private int _minInt;

		// Token: 0x0400030F RID: 783
		private int _maxInt;

		// Token: 0x04000310 RID: 784
		private bool _allowRandomization;

		// Token: 0x04000311 RID: 785
		private float _minRange;

		// Token: 0x04000312 RID: 786
		private float _maxRange;

		// Token: 0x04000313 RID: 787
		private bool _isDiscrete;

		// Token: 0x04000314 RID: 788
		private SelectorVM<SelectorItemVM> _selector;

		// Token: 0x04000315 RID: 789
		private HintViewModel _descriptionHint;

		// Token: 0x04000316 RID: 790
		private HintViewModel _randomizeHint;

		// Token: 0x020000C0 RID: 192
		public enum CampaignStartingOptionTypes
		{
			// Token: 0x0400045C RID: 1116
			Bool,
			// Token: 0x0400045D RID: 1117
			Slider,
			// Token: 0x0400045E RID: 1118
			Selection,
			// Token: 0x0400045F RID: 1119
			Input
		}
	}
}
