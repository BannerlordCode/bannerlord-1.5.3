using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Options;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x0200006E RID: 110
	public class GroupedOptionCategoryVM : ViewModel
	{
		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x0001CD76 File Offset: 0x0001AF76
		public IEnumerable<GenericOptionDataVM> AllOptions
		{
			get
			{
				return this.BaseOptions.Concat<GenericOptionDataVM>(this.Groups.SelectMany<OptionGroupVM, GenericOptionDataVM>((OptionGroupVM g) => g.Options));
			}
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0001CDAD File Offset: 0x0001AFAD
		public GroupedOptionCategoryVM(OptionsVM options, TextObject name, OptionCategory category, bool isEnabled, bool isResetSupported = false)
		{
			this._category = category;
			this._nameTextObject = name;
			this._options = options;
			this.IsEnabled = isEnabled;
			this.IsResetSupported = isResetSupported;
			this.InitializeOptions();
			this.RefreshValues();
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0001CDE8 File Offset: 0x0001AFE8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._nameTextObject.ToString();
			this.BaseOptions.ApplyActionOnAllItems(delegate(GenericOptionDataVM b)
			{
				b.RefreshValues();
			});
			this.Groups.ApplyActionOnAllItems(delegate(OptionGroupVM g)
			{
				g.RefreshValues();
			});
			this.ResetText = new TextObject("{=RVIKFCno}Reset to Defaults", null).ToString();
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0001CE78 File Offset: 0x0001B078
		private void InitializeOptions()
		{
			this.BaseOptions = new MBBindingList<GenericOptionDataVM>();
			this.Groups = new MBBindingList<OptionGroupVM>();
			if (this._category == null)
			{
				return;
			}
			if (this._category.Groups != null)
			{
				foreach (OptionGroup optionGroup in this._category.Groups)
				{
					this.Groups.Add(new OptionGroupVM(optionGroup.GroupName, this._options, optionGroup.Options));
				}
			}
			if (this._category.BaseOptions != null)
			{
				foreach (IOptionData optionData in this._category.BaseOptions)
				{
					this.BaseOptions.Add(this._options.GetOptionItem(optionData));
				}
			}
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0001CF70 File Offset: 0x0001B170
		internal IEnumerable<IOptionData> GetManagedOptions()
		{
			List<IOptionData> managedOptions = new List<IOptionData>();
			this.Groups.ApplyActionOnAllItems(delegate(OptionGroupVM g)
			{
				managedOptions.AppendList<IOptionData>(g.GetManagedOptions());
			});
			return managedOptions.AsReadOnly();
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0001CFB0 File Offset: 0x0001B1B0
		internal void InitializeDependentConfigs(Action<IOptionData, float> updateDependentConfigs)
		{
			this.Groups.ApplyActionOnAllItems(delegate(OptionGroupVM g)
			{
				g.InitializeDependentConfigs(updateDependentConfigs);
			});
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
		internal bool IsChanged()
		{
			if (!this.BaseOptions.Any<GenericOptionDataVM>((GenericOptionDataVM b) => b.IsChanged()))
			{
				return this.Groups.Any<OptionGroupVM>((OptionGroupVM g) => g.IsChanged());
			}
			return true;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0001D04C File Offset: 0x0001B24C
		internal void Cancel()
		{
			this.BaseOptions.ApplyActionOnAllItems(delegate(GenericOptionDataVM b)
			{
				b.Cancel();
			});
			this.Groups.ApplyActionOnAllItems(delegate(OptionGroupVM g)
			{
				g.Cancel();
			});
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0001D0B0 File Offset: 0x0001B2B0
		public void ResetData()
		{
			this.BaseOptions.ApplyActionOnAllItems(delegate(GenericOptionDataVM b)
			{
				b.ResetData();
			});
			foreach (OptionGroupVM optionGroupVM in this.Groups)
			{
				optionGroupVM.Options.ApplyActionOnAllItems(delegate(GenericOptionDataVM o)
				{
					o.ResetData();
				});
			}
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0001D148 File Offset: 0x0001B348
		public void ExecuteResetToDefault()
		{
			InformationManager.ShowInquiry(new InquiryData(new TextObject("{=oZc8oEAP}Reset this category to default", null).ToString(), new TextObject("{=CCBcdzGa}This will reset ALL options of this category to their default states. You won't be able to undo this action. {newline} {newline}Are you sure?", null).ToString(), true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=8OkPHu4f}No", null).ToString(), new Action(this.ResetToDefault), null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0001D1C0 File Offset: 0x0001B3C0
		private void ResetToDefault()
		{
			this.BaseOptions.ApplyActionOnAllItems(delegate(GenericOptionDataVM b)
			{
				b.ResetToDefault();
			});
			this.Groups.ApplyActionOnAllItems(delegate(OptionGroupVM g)
			{
				g.Options.ApplyActionOnAllItems(delegate(GenericOptionDataVM o)
				{
					o.ResetToDefault();
				});
			});
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0001D224 File Offset: 0x0001B424
		public GenericOptionDataVM GetOption(ManagedOptions.ManagedOptionsType optionType)
		{
			return this.AllOptions.FirstOrDefault<GenericOptionDataVM>((GenericOptionDataVM o) => !o.IsNative && (ManagedOptions.ManagedOptionsType)o.GetOptionType() == optionType);
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0001D258 File Offset: 0x0001B458
		public GenericOptionDataVM GetOption(NativeOptions.NativeOptionsType optionType)
		{
			return this.AllOptions.FirstOrDefault<GenericOptionDataVM>((GenericOptionDataVM o) => o.IsNative && (NativeOptions.NativeOptionsType)o.GetOptionType() == optionType);
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0001D289 File Offset: 0x0001B489
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x0001D291 File Offset: 0x0001B491
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x0001D2AF File Offset: 0x0001B4AF
		// (set) Token: 0x060008A4 RID: 2212 RVA: 0x0001D2B7 File Offset: 0x0001B4B7
		[DataSourceProperty]
		public bool IsResetSupported
		{
			get
			{
				return this._isResetSupported;
			}
			set
			{
				if (value != this._isResetSupported)
				{
					this._isResetSupported = value;
					base.OnPropertyChangedWithValue(value, "IsResetSupported");
				}
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x0001D2D5 File Offset: 0x0001B4D5
		// (set) Token: 0x060008A6 RID: 2214 RVA: 0x0001D2DD File Offset: 0x0001B4DD
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

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0001D300 File Offset: 0x0001B500
		// (set) Token: 0x060008A8 RID: 2216 RVA: 0x0001D308 File Offset: 0x0001B508
		[DataSourceProperty]
		public string ResetText
		{
			get
			{
				return this._resetText;
			}
			set
			{
				if (value != this._resetText)
				{
					this._resetText = value;
					base.OnPropertyChangedWithValue<string>(value, "ResetText");
				}
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0001D32B File Offset: 0x0001B52B
		// (set) Token: 0x060008AA RID: 2218 RVA: 0x0001D333 File Offset: 0x0001B533
		[DataSourceProperty]
		public MBBindingList<OptionGroupVM> Groups
		{
			get
			{
				return this._groups;
			}
			set
			{
				if (value != this._groups)
				{
					this._groups = value;
					base.OnPropertyChangedWithValue<MBBindingList<OptionGroupVM>>(value, "Groups");
				}
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0001D351 File Offset: 0x0001B551
		// (set) Token: 0x060008AC RID: 2220 RVA: 0x0001D359 File Offset: 0x0001B559
		[DataSourceProperty]
		public MBBindingList<GenericOptionDataVM> BaseOptions
		{
			get
			{
				return this._baseOptions;
			}
			set
			{
				if (value != this._baseOptions)
				{
					this._baseOptions = value;
					base.OnPropertyChangedWithValue<MBBindingList<GenericOptionDataVM>>(value, "BaseOptions");
				}
			}
		}

		// Token: 0x040003DE RID: 990
		private readonly OptionCategory _category;

		// Token: 0x040003DF RID: 991
		private readonly TextObject _nameTextObject;

		// Token: 0x040003E0 RID: 992
		protected readonly OptionsVM _options;

		// Token: 0x040003E1 RID: 993
		private bool _isEnabled;

		// Token: 0x040003E2 RID: 994
		private bool _isResetSupported;

		// Token: 0x040003E3 RID: 995
		private string _name;

		// Token: 0x040003E4 RID: 996
		private string _resetText;

		// Token: 0x040003E5 RID: 997
		private MBBindingList<GenericOptionDataVM> _baseOptions;

		// Token: 0x040003E6 RID: 998
		private MBBindingList<OptionGroupVM> _groups;
	}
}
