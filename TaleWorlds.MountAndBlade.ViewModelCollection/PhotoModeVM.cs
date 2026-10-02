using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection
{
	// Token: 0x0200000A RID: 10
	public class PhotoModeVM : ViewModel
	{
		// Token: 0x0600008E RID: 142 RVA: 0x000037B8 File Offset: 0x000019B8
		public PhotoModeVM(Scene missionScene, Func<bool> getVignetteOn, Func<bool> getHideAgentsOn)
		{
			this._missionScene = missionScene;
			this.Keys = new MBBindingList<InputKeyItemVM>();
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			bool flag = false;
			float num5 = 65f;
			missionScene.SetPhotoModeFov(num5);
			this._missionScene.GetPhotoModeFocus(ref num, ref num2, ref num3, ref num4, ref flag);
			this.FocusEndValueOption = new PhotoModeValueOptionVM(new TextObject("{=eeJcVeQG}Focus End", null), 0f, 1000f, num3, new Action<float>(this.OnFocusEndValueChange));
			this.FocusStartValueOption = new PhotoModeValueOptionVM(new TextObject("{=j5pLIV91}Focus Start", null), 0f, 100f, num2, new Action<float>(this.OnFocusStartValueChange));
			this.FocusValueOption = new PhotoModeValueOptionVM(new TextObject("{=photomodefocus}Focus", null), 0f, 100f, num, new Action<float>(this.OnFocusValueChange));
			this.ExposureOption = new PhotoModeValueOptionVM(new TextObject("{=iPx4jep6}Exposure", null), -5f, 5f, num4, new Action<float>(this.OnExposureValueChange));
			this.VerticalFovOption = new PhotoModeValueOptionVM(new TextObject("{=7XtICVeZ}Field of View", null), 2f, 140f, num5, new Action<float>(this.OnVerticalFovValueChange));
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000038FE File Offset: 0x00001AFE
		private void OnFocusValueChange(float newFocusValue)
		{
			this._missionScene.SetPhotoModeFocus(this.FocusStartValueOption.CurrentValue, this.FocusEndValueOption.CurrentValue, newFocusValue, this.ExposureOption.CurrentValue);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000392D File Offset: 0x00001B2D
		private void OnFocusStartValueChange(float newFocusStartValue)
		{
			this._missionScene.SetPhotoModeFocus(newFocusStartValue, this.FocusEndValueOption.CurrentValue, this.FocusValueOption.CurrentValue, this.ExposureOption.CurrentValue);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000395C File Offset: 0x00001B5C
		private void OnFocusEndValueChange(float newFocusEndValue)
		{
			this._missionScene.SetPhotoModeFocus(this.FocusStartValueOption.CurrentValue, newFocusEndValue, this.FocusValueOption.CurrentValue, this.ExposureOption.CurrentValue);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000398B File Offset: 0x00001B8B
		private void OnExposureValueChange(float newExposureValue)
		{
			this._missionScene.SetPhotoModeFocus(this.FocusStartValueOption.CurrentValue, this.FocusEndValueOption.CurrentValue, this.FocusValueOption.CurrentValue, newExposureValue);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000039BA File Offset: 0x00001BBA
		private void OnVerticalFovValueChange(float newVerticalFov)
		{
			this._missionScene.SetPhotoModeFov(newVerticalFov);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000039C8 File Offset: 0x00001BC8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Keys.ApplyActionOnAllItems(delegate(InputKeyItemVM k)
			{
				k.RefreshValues();
			});
			this.FocusEndValueOption.RefreshValues();
			this.FocusStartValueOption.RefreshValues();
			this.FocusValueOption.RefreshValues();
			this.ExposureOption.RefreshValues();
			this.VerticalFovOption.RefreshValues();
			List<string> list = new List<string>();
			foreach (string text in this._missionScene.GetAllColorGradeNames().Split(new string[] { "*/*" }, StringSplitOptions.RemoveEmptyEntries))
			{
				string text2 = GameTexts.FindText("str_photo_mode_color_grade", text).ToString();
				list.Add(text2);
			}
			if (list.Count == 0)
			{
				list.Add("Photo Mode Not Active");
			}
			this.ColorGradeSelector = new SelectorVM<SelectorItemVM>(list, this._missionScene.GetSceneColorGradeIndex(), new Action<SelectorVM<SelectorItemVM>>(this.OnColorGradeSelectionChanged));
			List<string> list2 = new List<string>();
			foreach (string text3 in this._missionScene.GetAllFilterNames().Split(new string[] { "*/*" }, StringSplitOptions.RemoveEmptyEntries))
			{
				string text4 = GameTexts.FindText("str_photo_mode_overlay", text3).ToString();
				list2.Add(text4);
			}
			if (list2.Count == 0)
			{
				list.Add("Photo Mode Not Active");
			}
			this.OverlaySelector = new SelectorVM<SelectorItemVM>(list2, this._missionScene.GetSceneFilterIndex(), new Action<SelectorVM<SelectorItemVM>>(this.OnOverlaySelectionChanged));
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003B50 File Offset: 0x00001D50
		public void AddTakePictureKey(GameKey key)
		{
			this._takePictureKey = InputKeyItemVM.CreateFromGameKey(key, false);
			this.Keys.Add(this._takePictureKey);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003B70 File Offset: 0x00001D70
		public void AddFasterCameraKey(HotKey hotkey)
		{
			this._fasterCameraKey = InputKeyItemVM.CreateFromHotKey(hotkey, false);
			this.Keys.Add(this._fasterCameraKey);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003B90 File Offset: 0x00001D90
		public void AddKey(GameKey key)
		{
			this.Keys.Add(InputKeyItemVM.CreateFromGameKey(key, false));
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003BA4 File Offset: 0x00001DA4
		public void AddHotkey(HotKey hotkey)
		{
			this.Keys.Add(InputKeyItemVM.CreateFromHotKey(hotkey, false));
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003BB8 File Offset: 0x00001DB8
		public void AddHotkeyWithForcedName(HotKey hotkey, TextObject forcedName)
		{
			this.Keys.Add(InputKeyItemVM.CreateFromHotKeyWithForcedName(hotkey, forcedName, false));
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003BCD File Offset: 0x00001DCD
		public void AddConsoleTakePictureKey(string keyID, TextObject forcedName)
		{
			this._takePictureKey = InputKeyItemVM.CreateFromForcedID(keyID, forcedName, false);
			this.Keys.Add(this._takePictureKey);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003BF0 File Offset: 0x00001DF0
		public override void OnFinalize()
		{
			base.OnFinalize();
			foreach (InputKeyItemVM inputKeyItemVM in this.Keys)
			{
				inputKeyItemVM.OnFinalize();
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003C40 File Offset: 0x00001E40
		private void OnColorGradeSelectionChanged(SelectorVM<SelectorItemVM> obj)
		{
			if (this._missionScene.GetSceneColorGradeIndex() != obj.SelectedIndex)
			{
				this._missionScene.SetSceneColorGradeIndex(obj.SelectedIndex);
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003C68 File Offset: 0x00001E68
		private void OnOverlaySelectionChanged(SelectorVM<SelectorItemVM> obj)
		{
			if (this._missionScene.GetSceneFilterIndex() != obj.SelectedIndex)
			{
				int num = this._missionScene.SetSceneFilterIndex(obj.SelectedIndex);
				if (num >= 0)
				{
					this.ColorGradeSelector.SelectedIndex = num;
				}
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003CAC File Offset: 0x00001EAC
		public void Reset()
		{
			this.ColorGradeSelector.SelectedIndex = 0;
			this.OverlaySelector.SelectedIndex = 0;
			this.FocusValueOption.CurrentValue = 0f;
			this.FocusStartValueOption.CurrentValue = 0f;
			this.FocusEndValueOption.CurrentValue = 0f;
			this.ExposureOption.CurrentValue = 0f;
			this.VerticalFovOption.CurrentValue = 65f;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003D24 File Offset: 0x00001F24
		public void UpdateTakePictureKeyVisibility(bool canTakePicture)
		{
			InputKeyItemVM takePictureKey = this._takePictureKey;
			if (takePictureKey == null)
			{
				return;
			}
			takePictureKey.SetForcedVisibility(canTakePicture ? null : new bool?(false));
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003D58 File Offset: 0x00001F58
		public void UpdateFasterCameraKeyVisibility(bool canMoveCamera)
		{
			InputKeyItemVM fasterCameraKey = this._fasterCameraKey;
			if (fasterCameraKey == null)
			{
				return;
			}
			fasterCameraKey.SetForcedVisibility(canMoveCamera ? null : new bool?(false));
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00003D89 File Offset: 0x00001F89
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003D91 File Offset: 0x00001F91
		[DataSourceProperty]
		public MBBindingList<InputKeyItemVM> Keys
		{
			get
			{
				return this._keys;
			}
			set
			{
				if (value != this._keys)
				{
					this._keys = value;
					base.OnPropertyChangedWithValue<MBBindingList<InputKeyItemVM>>(value, "Keys");
				}
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003DAF File Offset: 0x00001FAF
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003DB7 File Offset: 0x00001FB7
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> ColorGradeSelector
		{
			get
			{
				return this._colorGradeSelector;
			}
			set
			{
				if (value != this._colorGradeSelector)
				{
					this._colorGradeSelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "ColorGradeSelector");
				}
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003DD5 File Offset: 0x00001FD5
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00003DDD File Offset: 0x00001FDD
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> OverlaySelector
		{
			get
			{
				return this._overlaySelector;
			}
			set
			{
				if (value != this._overlaySelector)
				{
					this._overlaySelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "OverlaySelector");
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00003DFB File Offset: 0x00001FFB
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00003E03 File Offset: 0x00002003
		[DataSourceProperty]
		public PhotoModeValueOptionVM FocusEndValueOption
		{
			get
			{
				return this._focusEndValueOption;
			}
			set
			{
				if (value != this._focusEndValueOption)
				{
					this._focusEndValueOption = value;
					base.OnPropertyChangedWithValue<PhotoModeValueOptionVM>(value, "FocusEndValueOption");
				}
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003E21 File Offset: 0x00002021
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00003E29 File Offset: 0x00002029
		[DataSourceProperty]
		public PhotoModeValueOptionVM FocusStartValueOption
		{
			get
			{
				return this._focusStartValueOption;
			}
			set
			{
				if (value != this._focusStartValueOption)
				{
					this._focusStartValueOption = value;
					base.OnPropertyChangedWithValue<PhotoModeValueOptionVM>(value, "FocusStartValueOption");
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003E47 File Offset: 0x00002047
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00003E4F File Offset: 0x0000204F
		[DataSourceProperty]
		public PhotoModeValueOptionVM FocusValueOption
		{
			get
			{
				return this._focusValueOption;
			}
			set
			{
				if (value != this._focusValueOption)
				{
					this._focusValueOption = value;
					base.OnPropertyChangedWithValue<PhotoModeValueOptionVM>(value, "FocusValueOption");
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003E6D File Offset: 0x0000206D
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00003E75 File Offset: 0x00002075
		[DataSourceProperty]
		public PhotoModeValueOptionVM ExposureOption
		{
			get
			{
				return this._exposureOption;
			}
			set
			{
				if (value != this._exposureOption)
				{
					this._exposureOption = value;
					base.OnPropertyChangedWithValue<PhotoModeValueOptionVM>(value, "ExposureOption");
				}
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00003E93 File Offset: 0x00002093
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00003E9B File Offset: 0x0000209B
		[DataSourceProperty]
		public PhotoModeValueOptionVM VerticalFovOption
		{
			get
			{
				return this._verticalFovOption;
			}
			set
			{
				if (value != this._verticalFovOption)
				{
					this._verticalFovOption = value;
					base.OnPropertyChangedWithValue<PhotoModeValueOptionVM>(value, "VerticalFovOption");
				}
			}
		}

		// Token: 0x0400003E RID: 62
		private readonly Scene _missionScene;

		// Token: 0x0400003F RID: 63
		private InputKeyItemVM _takePictureKey;

		// Token: 0x04000040 RID: 64
		private InputKeyItemVM _fasterCameraKey;

		// Token: 0x04000041 RID: 65
		private SelectorVM<SelectorItemVM> _colorGradeSelector;

		// Token: 0x04000042 RID: 66
		private SelectorVM<SelectorItemVM> _overlaySelector;

		// Token: 0x04000043 RID: 67
		private MBBindingList<InputKeyItemVM> _keys;

		// Token: 0x04000044 RID: 68
		private PhotoModeValueOptionVM _focusEndValueOption;

		// Token: 0x04000045 RID: 69
		private PhotoModeValueOptionVM _focusStartValueOption;

		// Token: 0x04000046 RID: 70
		private PhotoModeValueOptionVM _focusValueOption;

		// Token: 0x04000047 RID: 71
		private PhotoModeValueOptionVM _exposureOption;

		// Token: 0x04000048 RID: 72
		private PhotoModeValueOptionVM _verticalFovOption;
	}
}
