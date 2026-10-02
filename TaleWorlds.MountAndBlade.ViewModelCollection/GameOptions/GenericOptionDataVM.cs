using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x0200006D RID: 109
	public abstract class GenericOptionDataVM : ViewModel
	{
		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0001CB50 File Offset: 0x0001AD50
		public bool IsNative
		{
			get
			{
				return this.Option.IsNative();
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x0001CB5D File Offset: 0x0001AD5D
		public bool IsAction
		{
			get
			{
				return this.Option.IsAction();
			}
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0001CB6C File Offset: 0x0001AD6C
		protected GenericOptionDataVM(OptionsVM optionsVM, IOptionData option, TextObject name, TextObject description, OptionsVM.OptionsDataType typeID)
		{
			this._nameObj = name;
			this._descriptionObj = description;
			this._optionsVM = optionsVM;
			this.Option = option;
			this.OptionTypeID = (int)typeID;
			this.Hint = new HintViewModel();
			this.RefreshValues();
			this.UpdateEnableState();
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0001CBC9 File Offset: 0x0001ADC9
		public virtual void UpdateData(bool initUpdate)
		{
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0001CBCB File Offset: 0x0001ADCB
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._nameObj.ToString();
			this.Description = this._descriptionObj.ToString();
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0001CBF5 File Offset: 0x0001ADF5
		public object GetOptionType()
		{
			return this.Option.GetOptionType();
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0001CC02 File Offset: 0x0001AE02
		public IOptionData GetOptionData()
		{
			return this.Option;
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0001CC0A File Offset: 0x0001AE0A
		public void ResetToDefault()
		{
			this.SetValue(this.Option.GetDefaultValue());
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0001CC20 File Offset: 0x0001AE20
		public void UpdateEnableState()
		{
			ValueTuple<string, bool> isDisabledAndReasonID = this.Option.GetIsDisabledAndReasonID();
			if (!string.IsNullOrEmpty(isDisabledAndReasonID.Item1))
			{
				this.Hint.HintText = Module.CurrentModule.GlobalTextManager.FindText(isDisabledAndReasonID.Item1, null);
			}
			else
			{
				this.Hint.HintText = TextObject.GetEmpty();
			}
			this.IsEnabled = !isDisabledAndReasonID.Item2;
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x0001CC88 File Offset: 0x0001AE88
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x0001CC90 File Offset: 0x0001AE90
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x0001CCB3 File Offset: 0x0001AEB3
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x0001CCBB File Offset: 0x0001AEBB
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

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0001CCDE File Offset: 0x0001AEDE
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x0001CCE6 File Offset: 0x0001AEE6
		[DataSourceProperty]
		public string[] ImageIDs
		{
			get
			{
				return this._imageIDs;
			}
			set
			{
				if (value != this._imageIDs)
				{
					this._imageIDs = value;
					base.OnPropertyChangedWithValue<string[]>(value, "ImageIDs");
				}
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0001CD04 File Offset: 0x0001AF04
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x0001CD0C File Offset: 0x0001AF0C
		[DataSourceProperty]
		public int OptionTypeID
		{
			get
			{
				return this._optionTypeId;
			}
			set
			{
				if (value != this._optionTypeId)
				{
					this._optionTypeId = value;
					base.OnPropertyChangedWithValue(value, "OptionTypeID");
				}
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x0001CD2A File Offset: 0x0001AF2A
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x0001CD32 File Offset: 0x0001AF32
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

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x0001CD50 File Offset: 0x0001AF50
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x0001CD58 File Offset: 0x0001AF58
		[DataSourceProperty]
		public HintViewModel Hint
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
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x0600088E RID: 2190
		public abstract void UpdateValue();

		// Token: 0x0600088F RID: 2191
		public abstract void Cancel();

		// Token: 0x06000890 RID: 2192
		public abstract bool IsChanged();

		// Token: 0x06000891 RID: 2193
		public abstract void SetValue(float value);

		// Token: 0x06000892 RID: 2194
		public abstract void ResetData();

		// Token: 0x06000893 RID: 2195
		public abstract void ApplyValue();

		// Token: 0x040003D4 RID: 980
		private TextObject _nameObj;

		// Token: 0x040003D5 RID: 981
		private TextObject _descriptionObj;

		// Token: 0x040003D6 RID: 982
		protected OptionsVM _optionsVM;

		// Token: 0x040003D7 RID: 983
		protected IOptionData Option;

		// Token: 0x040003D8 RID: 984
		private string _description;

		// Token: 0x040003D9 RID: 985
		private string _name;

		// Token: 0x040003DA RID: 986
		private int _optionTypeId = -1;

		// Token: 0x040003DB RID: 987
		private string[] _imageIDs;

		// Token: 0x040003DC RID: 988
		private bool _isEnabled = true;

		// Token: 0x040003DD RID: 989
		private HintViewModel _hint;
	}
}
