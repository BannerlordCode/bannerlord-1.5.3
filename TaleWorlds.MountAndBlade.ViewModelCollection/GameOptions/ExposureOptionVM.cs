using System;
using TaleWorlds.Engine.Options;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x0200006C RID: 108
	public class ExposureOptionVM : ViewModel
	{
		// Token: 0x06000860 RID: 2144 RVA: 0x0001C859 File Offset: 0x0001AA59
		public ExposureOptionVM(Action<bool> onClose = null)
		{
			this._onClose = onClose;
			this.InitialValue = 0f;
			this.Value = this.InitialValue;
			this.RefreshValues();
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0001C888 File Offset: 0x0001AA88
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = Module.CurrentModule.GlobalTextManager.FindText("str_exposure_option_title", null).ToString();
			TextObject textObject = Module.CurrentModule.GlobalTextManager.FindText("str_exposure_option_explainer", null);
			textObject.SetTextVariable("newline", "\n");
			this.ExplanationText = textObject.ToString();
			this.CancelText = new TextObject("{=3CpNUnVl}Cancel", null).ToString();
			this.AcceptText = new TextObject("{=Y94H6XnK}Accept", null).ToString();
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0001C91A File Offset: 0x0001AB1A
		public void ExecuteConfirm()
		{
			this.InitialValue = this.Value;
			NativeOptions.SetConfig(NativeOptions.NativeOptionsType.ExposureCompensation, this.Value);
			Action<bool> onClose = this._onClose;
			if (onClose != null)
			{
				onClose(true);
			}
			this.Visible = false;
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0001C94E File Offset: 0x0001AB4E
		public void ExecuteCancel()
		{
			this.Value = this.InitialValue;
			NativeOptions.SetConfig(NativeOptions.NativeOptionsType.ExposureCompensation, this.InitialValue);
			this.Visible = false;
			Action<bool> onClose = this._onClose;
			if (onClose == null)
			{
				return;
			}
			onClose(false);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0001C981 File Offset: 0x0001AB81
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM confirmInputKey = this.ConfirmInputKey;
			if (confirmInputKey != null)
			{
				confirmInputKey.OnFinalize();
			}
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey == null)
			{
				return;
			}
			cancelInputKey.OnFinalize();
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x0001C9AA File Offset: 0x0001ABAA
		// (set) Token: 0x06000866 RID: 2150 RVA: 0x0001C9B2 File Offset: 0x0001ABB2
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x0001C9D5 File Offset: 0x0001ABD5
		// (set) Token: 0x06000868 RID: 2152 RVA: 0x0001C9DD File Offset: 0x0001ABDD
		[DataSourceProperty]
		public string ExplanationText
		{
			get
			{
				return this._explanationText;
			}
			set
			{
				if (value != this._explanationText)
				{
					this._explanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExplanationText");
				}
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x0001CA00 File Offset: 0x0001AC00
		// (set) Token: 0x0600086A RID: 2154 RVA: 0x0001CA08 File Offset: 0x0001AC08
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x0001CA2B File Offset: 0x0001AC2B
		// (set) Token: 0x0600086C RID: 2156 RVA: 0x0001CA33 File Offset: 0x0001AC33
		[DataSourceProperty]
		public string AcceptText
		{
			get
			{
				return this._acceptText;
			}
			set
			{
				if (value != this._acceptText)
				{
					this._acceptText = value;
					base.OnPropertyChangedWithValue<string>(value, "AcceptText");
				}
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x0001CA56 File Offset: 0x0001AC56
		// (set) Token: 0x0600086E RID: 2158 RVA: 0x0001CA5E File Offset: 0x0001AC5E
		public float Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (this._value != value)
				{
					this._value = value;
					base.OnPropertyChangedWithValue(value, "Value");
					NativeOptions.SetConfig(NativeOptions.NativeOptionsType.ExposureCompensation, this.Value);
				}
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x0001CA89 File Offset: 0x0001AC89
		// (set) Token: 0x06000870 RID: 2160 RVA: 0x0001CA91 File Offset: 0x0001AC91
		public float InitialValue
		{
			get
			{
				return this._initialValue;
			}
			set
			{
				if (this._initialValue != value)
				{
					this._initialValue = value;
					base.OnPropertyChangedWithValue(value, "InitialValue");
				}
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0001CAAF File Offset: 0x0001ACAF
		// (set) Token: 0x06000872 RID: 2162 RVA: 0x0001CAB7 File Offset: 0x0001ACB7
		public bool Visible
		{
			get
			{
				return this._visible;
			}
			set
			{
				if (this._visible != value)
				{
					this._visible = value;
					base.OnPropertyChangedWithValue(value, "Visible");
					if (value)
					{
						this.Value = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.ExposureCompensation);
					}
				}
			}
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0001CAE6 File Offset: 0x0001ACE6
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0001CAF5 File Offset: 0x0001ACF5
		public void SetConfirmInputKey(HotKey hotkey)
		{
			this.ConfirmInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x0001CB04 File Offset: 0x0001AD04
		// (set) Token: 0x06000876 RID: 2166 RVA: 0x0001CB0C File Offset: 0x0001AD0C
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0001CB2A File Offset: 0x0001AD2A
		// (set) Token: 0x06000878 RID: 2168 RVA: 0x0001CB32 File Offset: 0x0001AD32
		[DataSourceProperty]
		public InputKeyItemVM ConfirmInputKey
		{
			get
			{
				return this._confirmInputKey;
			}
			set
			{
				if (value != this._confirmInputKey)
				{
					this._confirmInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ConfirmInputKey");
				}
			}
		}

		// Token: 0x040003CA RID: 970
		private readonly Action<bool> _onClose;

		// Token: 0x040003CB RID: 971
		private string _titleText;

		// Token: 0x040003CC RID: 972
		private string _explanationText;

		// Token: 0x040003CD RID: 973
		private string _cancelText;

		// Token: 0x040003CE RID: 974
		private string _acceptText;

		// Token: 0x040003CF RID: 975
		private float _initialValue;

		// Token: 0x040003D0 RID: 976
		private float _value;

		// Token: 0x040003D1 RID: 977
		private bool _visible;

		// Token: 0x040003D2 RID: 978
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x040003D3 RID: 979
		private InputKeyItemVM _confirmInputKey;
	}
}
