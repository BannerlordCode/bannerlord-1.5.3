using System;
using System.Collections.Generic;
using SandBox.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem.Incidents;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Map.Incidents
{
	// Token: 0x0200004D RID: 77
	public class MapIncidentVM : ViewModel
	{
		// Token: 0x060004CD RID: 1229 RVA: 0x00012DE4 File Offset: 0x00010FE4
		public MapIncidentVM(Incident incident, Action onClose)
		{
			this._incident = incident;
			this._onClose = onClose;
			this.IncidentType = incident.TypeId;
			this.ConfirmHint = new HintViewModel();
			this.Options = new MBBindingList<MapIncidentOptionVM>();
			this.PopulateOptions();
			this.RefreshValues();
			this.UpdateCanConfirm();
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00012E3C File Offset: 0x0001103C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Title = this._incident.Title.ToString();
			this.Description = this._incident.Description.ToString();
			this.ConfirmText = new TextObject("{=WiNRdfsm}Done", null).ToString();
			this.InactiveHint = new TextObject("{=aTs9tbol}No option selected", null).ToString();
			this.Options.ApplyActionOnAllItems(delegate(MapIncidentOptionVM o)
			{
				o.RefreshValues();
			});
			this.UpdateActiveHints();
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00012ED7 File Offset: 0x000110D7
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Options.ApplyActionOnAllItems(delegate(MapIncidentOptionVM o)
			{
				o.OnFinalize();
			});
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00012F0C File Offset: 0x0001110C
		public void ExecuteConfirm()
		{
			if (this.SelectedOption != null)
			{
				int index = this.SelectedOption.Index;
				if (index >= 0 && index < this._incident.NumOfOptions)
				{
					using (List<TextObject>.Enumerator enumerator = this._incident.InvokeOption(index).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							TextObject textObject = enumerator.Current;
							MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
						}
						goto IL_0095;
					}
				}
				Debug.FailedAssert("Selected incident option is out of bounds. Action won't be invoked", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Map\\Incidents\\MapIncidentVM.cs", "ExecuteConfirm", 73);
			}
			else
			{
				Debug.FailedAssert("An incident option must be selected before confirm", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Map\\Incidents\\MapIncidentVM.cs", "ExecuteConfirm", 78);
			}
			IL_0095:
			this._onClose();
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00012FCC File Offset: 0x000111CC
		private void PopulateOptions()
		{
			this.Options.Clear();
			for (int i = 0; i < this._incident.NumOfOptions; i++)
			{
				TextObject optionText = this._incident.GetOptionText(i);
				IncidentHint optionHint = this._incident.GetOptionHint(i);
				MapIncidentOptionVM mapIncidentOptionVM = new MapIncidentOptionVM(optionText, optionHint, i, new Action<MapIncidentOptionVM>(this.OnOptionSelected), new Action<MapIncidentOptionVM>(this.OnOptionFocused));
				this.Options.Add(mapIncidentOptionVM);
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001303F File Offset: 0x0001123F
		private void OnOptionSelected(MapIncidentOptionVM option)
		{
			this.SelectedOption = option;
			this.UpdateActiveHints();
			this.UpdateCanConfirm();
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00013054 File Offset: 0x00011254
		private void OnOptionFocused(MapIncidentOptionVM option)
		{
			this.FocusedOption = option;
			this.UpdateActiveHints();
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00013064 File Offset: 0x00011264
		private void UpdateActiveHints()
		{
			if (this.FocusedOption != null)
			{
				this.ActiveHint = this.FocusedOption.Hint;
				this.ShowActiveHints = true;
				return;
			}
			if (this.SelectedOption != null)
			{
				this.ActiveHint = this.SelectedOption.Hint;
				this.ShowActiveHints = true;
				return;
			}
			this.ActiveHint = null;
			this.ShowActiveHints = false;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x000130C1 File Offset: 0x000112C1
		private void UpdateCanConfirm()
		{
			if (this.SelectedOption == null)
			{
				this.CanConfirm = false;
				this.ConfirmHint.HintText = new TextObject("{=R3Zn7x07}You must select an option", null);
				return;
			}
			this.CanConfirm = true;
			this.ConfirmHint.HintText = TextObject.GetEmpty();
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x00013100 File Offset: 0x00011300
		// (set) Token: 0x060004D7 RID: 1239 RVA: 0x00013108 File Offset: 0x00011308
		[DataSourceProperty]
		public bool CanConfirm
		{
			get
			{
				return this._canConfirm;
			}
			set
			{
				if (value != this._canConfirm)
				{
					this._canConfirm = value;
					base.OnPropertyChangedWithValue(value, "CanConfirm");
				}
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00013126 File Offset: 0x00011326
		// (set) Token: 0x060004D9 RID: 1241 RVA: 0x0001312E File Offset: 0x0001132E
		[DataSourceProperty]
		public bool HasFocusedOption
		{
			get
			{
				return this._hasFocusedOption;
			}
			set
			{
				if (value != this._hasFocusedOption)
				{
					this._hasFocusedOption = value;
					base.OnPropertyChangedWithValue(value, "HasFocusedOption");
				}
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x0001314C File Offset: 0x0001134C
		// (set) Token: 0x060004DB RID: 1243 RVA: 0x00013154 File Offset: 0x00011354
		[DataSourceProperty]
		public bool HasSelectedOption
		{
			get
			{
				return this._hasSelectedOption;
			}
			set
			{
				if (value != this._hasSelectedOption)
				{
					this._hasSelectedOption = value;
					base.OnPropertyChangedWithValue(value, "HasSelectedOption");
				}
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00013172 File Offset: 0x00011372
		// (set) Token: 0x060004DD RID: 1245 RVA: 0x0001317A File Offset: 0x0001137A
		[DataSourceProperty]
		public bool ShowActiveHints
		{
			get
			{
				return this._showActiveHints;
			}
			set
			{
				if (value != this._showActiveHints)
				{
					this._showActiveHints = value;
					base.OnPropertyChangedWithValue(value, "ShowActiveHints");
				}
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00013198 File Offset: 0x00011398
		// (set) Token: 0x060004DF RID: 1247 RVA: 0x000131A0 File Offset: 0x000113A0
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x000131C3 File Offset: 0x000113C3
		// (set) Token: 0x060004E1 RID: 1249 RVA: 0x000131CB File Offset: 0x000113CB
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

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x000131EE File Offset: 0x000113EE
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x000131F6 File Offset: 0x000113F6
		[DataSourceProperty]
		public string ConfirmText
		{
			get
			{
				return this._confirmText;
			}
			set
			{
				if (value != this._confirmText)
				{
					this._confirmText = value;
					base.OnPropertyChangedWithValue<string>(value, "ConfirmText");
				}
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00013219 File Offset: 0x00011419
		// (set) Token: 0x060004E5 RID: 1253 RVA: 0x00013221 File Offset: 0x00011421
		[DataSourceProperty]
		public string IncidentType
		{
			get
			{
				return this._incidentType;
			}
			set
			{
				if (value != this._incidentType)
				{
					this._incidentType = value;
					base.OnPropertyChangedWithValue<string>(value, "IncidentType");
				}
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00013244 File Offset: 0x00011444
		// (set) Token: 0x060004E7 RID: 1255 RVA: 0x0001324C File Offset: 0x0001144C
		[DataSourceProperty]
		public MapIncidentHintVM ActiveHint
		{
			get
			{
				return this._activeHint;
			}
			set
			{
				if (value != this._activeHint)
				{
					this._activeHint = value;
					base.OnPropertyChangedWithValue<MapIncidentHintVM>(value, "ActiveHint");
				}
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x0001326A File Offset: 0x0001146A
		// (set) Token: 0x060004E9 RID: 1257 RVA: 0x00013272 File Offset: 0x00011472
		[DataSourceProperty]
		public string InactiveHint
		{
			get
			{
				return this._inactiveHint;
			}
			set
			{
				if (value != this._inactiveHint)
				{
					this._inactiveHint = value;
					base.OnPropertyChangedWithValue<string>(value, "InactiveHint");
				}
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00013295 File Offset: 0x00011495
		// (set) Token: 0x060004EB RID: 1259 RVA: 0x0001329D File Offset: 0x0001149D
		[DataSourceProperty]
		public HintViewModel ConfirmHint
		{
			get
			{
				return this._confirmHint;
			}
			set
			{
				if (value != this._confirmHint)
				{
					this._confirmHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ConfirmHint");
				}
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x000132BB File Offset: 0x000114BB
		// (set) Token: 0x060004ED RID: 1261 RVA: 0x000132C4 File Offset: 0x000114C4
		[DataSourceProperty]
		public MapIncidentOptionVM FocusedOption
		{
			get
			{
				return this._focusedOption;
			}
			set
			{
				if (value != this._focusedOption)
				{
					if (this._focusedOption != null)
					{
						this._focusedOption.IsFocused = false;
					}
					this._focusedOption = value;
					base.OnPropertyChangedWithValue<MapIncidentOptionVM>(value, "FocusedOption");
					if (this._focusedOption != null)
					{
						this._focusedOption.IsFocused = true;
					}
					this.HasFocusedOption = value != null;
				}
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x0001331F File Offset: 0x0001151F
		// (set) Token: 0x060004EF RID: 1263 RVA: 0x00013328 File Offset: 0x00011528
		[DataSourceProperty]
		public MapIncidentOptionVM SelectedOption
		{
			get
			{
				return this._selectedOption;
			}
			set
			{
				if (value != this._selectedOption)
				{
					if (this._selectedOption != null)
					{
						this._selectedOption.IsSelected = false;
					}
					this._selectedOption = value;
					base.OnPropertyChangedWithValue<MapIncidentOptionVM>(value, "SelectedOption");
					if (this._selectedOption != null)
					{
						this._selectedOption.IsSelected = true;
					}
					this.HasSelectedOption = value != null;
				}
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00013383 File Offset: 0x00011583
		// (set) Token: 0x060004F1 RID: 1265 RVA: 0x0001338B File Offset: 0x0001158B
		[DataSourceProperty]
		public MBBindingList<MapIncidentOptionVM> Options
		{
			get
			{
				return this._options;
			}
			set
			{
				if (value != this._options)
				{
					this._options = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapIncidentOptionVM>>(value, "Options");
				}
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000133A9 File Offset: 0x000115A9
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x000133B8 File Offset: 0x000115B8
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x000133C0 File Offset: 0x000115C0
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x0400026D RID: 621
		private readonly Incident _incident;

		// Token: 0x0400026E RID: 622
		private readonly Action _onClose;

		// Token: 0x0400026F RID: 623
		private bool _canConfirm;

		// Token: 0x04000270 RID: 624
		private bool _hasFocusedOption;

		// Token: 0x04000271 RID: 625
		private bool _hasSelectedOption;

		// Token: 0x04000272 RID: 626
		private bool _showActiveHints;

		// Token: 0x04000273 RID: 627
		private string _title;

		// Token: 0x04000274 RID: 628
		private string _description;

		// Token: 0x04000275 RID: 629
		private string _confirmText;

		// Token: 0x04000276 RID: 630
		private string _incidentType;

		// Token: 0x04000277 RID: 631
		private MapIncidentHintVM _activeHint;

		// Token: 0x04000278 RID: 632
		private string _inactiveHint;

		// Token: 0x04000279 RID: 633
		private HintViewModel _confirmHint;

		// Token: 0x0400027A RID: 634
		private MapIncidentOptionVM _focusedOption;

		// Token: 0x0400027B RID: 635
		private MapIncidentOptionVM _selectedOption;

		// Token: 0x0400027C RID: 636
		private MBBindingList<MapIncidentOptionVM> _options;

		// Token: 0x0400027D RID: 637
		private InputKeyItemVM _doneInputKey;
	}
}
