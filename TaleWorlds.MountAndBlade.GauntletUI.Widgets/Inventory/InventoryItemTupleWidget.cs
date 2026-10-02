using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000144 RID: 324
	public class InventoryItemTupleWidget : InventoryItemButtonWidget
	{
		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x0002E96C File Offset: 0x0002CB6C
		// (set) Token: 0x060010E5 RID: 4325 RVA: 0x0002E974 File Offset: 0x0002CB74
		public InventoryImageIdentifierWidget ItemImageIdentifier { get; set; }

		// Token: 0x060010E6 RID: 4326 RVA: 0x0002E97D File Offset: 0x0002CB7D
		public InventoryItemTupleWidget(UIContext context)
			: base(context)
		{
			base.OverrideDefaultStateSwitchingEnabled = false;
			base.AddState("Selected");
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0002E998 File Offset: 0x0002CB98
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			base.ScreenWidget.intPropertyChanged += this.InventoryScreenWidgetOnPropertyChanged;
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x0002E9B7 File Offset: 0x0002CBB7
		protected override void OnDisconnectedFromRoot()
		{
			base.OnDisconnectedFromRoot();
			base.ScreenWidget.intPropertyChanged -= this.InventoryScreenWidgetOnPropertyChanged;
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0002E9D8 File Offset: 0x0002CBD8
		private void SetWidgetsState(string state)
		{
			this.SetState(state);
			string currentState = this.ExtendedControlsContainer.CurrentState;
			this.ExtendedControlsContainer.SetState(base.IsSelected ? "Selected" : "Default");
			this.MainContainer.SetState(state);
			this.NameTextWidget.SetState((state == "Pressed") ? state : "Default");
			if (currentState == "Default" && base.IsSelected)
			{
				base.EventFired("Opened", Array.Empty<object>());
				this.Slider.IsExtended = true;
				return;
			}
			if (currentState == "Selected" && !base.IsSelected)
			{
				base.EventFired("Closed", Array.Empty<object>());
				this.Slider.IsExtended = false;
			}
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0002EAA8 File Offset: 0x0002CCA8
		private void OnExtendedHiddenUpdate(float dt)
		{
			if (!base.IsSelected)
			{
				this._extendedUpdateTimer += dt;
				if (this._extendedUpdateTimer > 2f)
				{
					this.ExtendedControlsContainer.IsVisible = false;
					return;
				}
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.OnExtendedHiddenUpdate), 1);
			}
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0002EB00 File Offset: 0x0002CD00
		protected override void RefreshState()
		{
			base.RefreshState();
			bool isVisible = this.ExtendedControlsContainer.IsVisible;
			this.ExtendedControlsContainer.IsExtended = base.IsSelected;
			if (base.IsSelected)
			{
				this.ExtendedControlsContainer.IsVisible = true;
			}
			else if (this.ExtendedControlsContainer.IsVisible)
			{
				this._extendedUpdateTimer = 0f;
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.OnExtendedHiddenUpdate), 1);
			}
			if (base.IsDisabled)
			{
				this.SetWidgetsState("Disabled");
				return;
			}
			if (base.IsPressed)
			{
				this.SetWidgetsState("Pressed");
				return;
			}
			if (base.IsHovered)
			{
				this.SetWidgetsState("Hovered");
				return;
			}
			if (base.IsSelected)
			{
				this.SetWidgetsState("Selected");
				return;
			}
			this.SetWidgetsState("Default");
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0002EBD4 File Offset: 0x0002CDD4
		private void UpdateEquipmentTypeState()
		{
			if (base.ScreenWidget != null)
			{
				bool flag = base.ScreenWidget.EquipmentMode == 0 && !this.IsCivilian && this.IsEquipable;
				bool flag2 = base.ScreenWidget.EquipmentMode == 2 && !this.IsStealth && this.IsEquipable;
				if (this.IsEquipable && !this.CanCharacterUseItem)
				{
					if (!this.MainContainer.Brush.IsCloneRelated(this.CharacterCantUseBrush))
					{
						this.MainContainer.Brush = this.CharacterCantUseBrush;
						this.EquipButton.IsVisible = true;
						this.EquipButton.IsEnabled = false;
						return;
					}
				}
				else if (flag || flag2)
				{
					if (!this.MainContainer.Brush.IsCloneRelated(this.CantUseInSetBrush))
					{
						this.MainContainer.Brush = this.CantUseInSetBrush;
						this.EquipButton.IsVisible = true;
						this.EquipButton.IsEnabled = false;
						return;
					}
				}
				else if (!this.MainContainer.Brush.IsCloneRelated(this.DefaultBrush))
				{
					this.MainContainer.Brush = this.DefaultBrush;
					this.EquipButton.IsVisible = this.IsEquipable;
					this.EquipButton.IsEnabled = this.IsEquipable;
				}
			}
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0002ED12 File Offset: 0x0002CF12
		private void SliderIntPropertyChanged(PropertyOwnerObject owner, string propertyName, int value)
		{
			if (propertyName == "ValueInt")
			{
				this.TransactionCount = this._slider.ValueInt;
			}
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x0002ED32 File Offset: 0x0002CF32
		private void CountTextWidgetOnPropertyChanged(PropertyOwnerObject owner, string propertyName, int value)
		{
			if (propertyName == "IntText")
			{
				this.UpdateCountText();
			}
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0002ED47 File Offset: 0x0002CF47
		private void InventoryScreenWidgetOnPropertyChanged(PropertyOwnerObject owner, string propertyName, int value)
		{
			if (propertyName == "EquipmentMode")
			{
				this.UpdateEquipmentTypeState();
			}
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x0002ED5C File Offset: 0x0002CF5C
		private void UpdateCountText()
		{
			if (this.SliderTextWidget != null)
			{
				this.SliderTextWidget.IsHidden = this.CountTextWidget.IsHidden;
			}
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x0002ED7C File Offset: 0x0002CF7C
		private void UpdateCostText()
		{
			if (this.CostTextWidget == null)
			{
				return;
			}
			switch (this.ProfitState)
			{
			case -2:
				this.CostTextWidget.SetState("VeryBad");
				return;
			case -1:
				this.CostTextWidget.SetState("Bad");
				return;
			case 0:
				this.CostTextWidget.SetState("Default");
				return;
			case 1:
				this.CostTextWidget.SetState("Good");
				return;
			case 2:
				this.CostTextWidget.SetState("VeryGood");
				return;
			default:
				return;
			}
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0002EE0B File Offset: 0x0002D00B
		private void UpdateDragAvailability()
		{
			base.AcceptDrag = this.ItemCount > 0 && (this.IsTransferable || this.IsEquipable);
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x0002EE30 File Offset: 0x0002D030
		// (set) Token: 0x060010F4 RID: 4340 RVA: 0x0002EE38 File Offset: 0x0002D038
		[Editor(false)]
		public string ItemID
		{
			get
			{
				return this._itemID;
			}
			set
			{
				if (this._itemID != value)
				{
					this._itemID = value;
					base.OnPropertyChanged<string>(value, "ItemID");
				}
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x0002EE5B File Offset: 0x0002D05B
		// (set) Token: 0x060010F6 RID: 4342 RVA: 0x0002EE63 File Offset: 0x0002D063
		[Editor(false)]
		public TextWidget NameTextWidget
		{
			get
			{
				return this._nameTextWidget;
			}
			set
			{
				if (this._nameTextWidget != value)
				{
					this._nameTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "NameTextWidget");
					this.NameTextWidget.AddState("Pressed");
				}
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x0002EE91 File Offset: 0x0002D091
		// (set) Token: 0x060010F8 RID: 4344 RVA: 0x0002EE9C File Offset: 0x0002D09C
		[Editor(false)]
		public TextWidget CountTextWidget
		{
			get
			{
				return this._countTextWidget;
			}
			set
			{
				if (this._countTextWidget != value)
				{
					if (this._countTextWidget != null)
					{
						this._countTextWidget.intPropertyChanged -= this.CountTextWidgetOnPropertyChanged;
					}
					this._countTextWidget = value;
					if (this._countTextWidget != null)
					{
						this._countTextWidget.intPropertyChanged += this.CountTextWidgetOnPropertyChanged;
					}
					base.OnPropertyChanged<TextWidget>(value, "CountTextWidget");
					this.UpdateCountText();
				}
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060010F9 RID: 4345 RVA: 0x0002EF09 File Offset: 0x0002D109
		// (set) Token: 0x060010FA RID: 4346 RVA: 0x0002EF11 File Offset: 0x0002D111
		[Editor(false)]
		public TextWidget CostTextWidget
		{
			get
			{
				return this._costTextWidget;
			}
			set
			{
				if (this._costTextWidget != value)
				{
					this._costTextWidget = value;
					this.UpdateCostText();
					base.OnPropertyChanged<TextWidget>(value, "CostTextWidget");
				}
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x060010FB RID: 4347 RVA: 0x0002EF35 File Offset: 0x0002D135
		// (set) Token: 0x060010FC RID: 4348 RVA: 0x0002EF3D File Offset: 0x0002D13D
		public int ProfitState
		{
			get
			{
				return this._profitState;
			}
			set
			{
				if (value != this._profitState)
				{
					this._profitState = value;
					this.UpdateCostText();
					base.OnPropertyChanged(value, "ProfitState");
				}
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060010FD RID: 4349 RVA: 0x0002EF61 File Offset: 0x0002D161
		// (set) Token: 0x060010FE RID: 4350 RVA: 0x0002EF69 File Offset: 0x0002D169
		[Editor(false)]
		public BrushListPanel MainContainer
		{
			get
			{
				return this._mainContainer;
			}
			set
			{
				if (this._mainContainer != value)
				{
					this._mainContainer = value;
					base.OnPropertyChanged<BrushListPanel>(value, "MainContainer");
				}
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060010FF RID: 4351 RVA: 0x0002EF87 File Offset: 0x0002D187
		// (set) Token: 0x06001100 RID: 4352 RVA: 0x0002EF8F File Offset: 0x0002D18F
		[Editor(false)]
		public InventoryTupleExtensionControlsWidget ExtendedControlsContainer
		{
			get
			{
				return this._extendedControlsContainer;
			}
			set
			{
				if (this._extendedControlsContainer != value)
				{
					this._extendedControlsContainer = value;
					base.OnPropertyChanged<InventoryTupleExtensionControlsWidget>(value, "ExtendedControlsContainer");
				}
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001101 RID: 4353 RVA: 0x0002EFAD File Offset: 0x0002D1AD
		// (set) Token: 0x06001102 RID: 4354 RVA: 0x0002EFB8 File Offset: 0x0002D1B8
		[Editor(false)]
		public InventoryTwoWaySliderWidget Slider
		{
			get
			{
				return this._slider;
			}
			set
			{
				if (this._slider != value)
				{
					if (this._slider != null)
					{
						this._slider.intPropertyChanged -= this.SliderIntPropertyChanged;
					}
					this._slider = value;
					if (this._slider != null)
					{
						this._slider.intPropertyChanged += this.SliderIntPropertyChanged;
					}
					base.OnPropertyChanged<InventoryTwoWaySliderWidget>(value, "Slider");
					this.Slider.AddState("Selected");
					this.Slider.OverrideDefaultStateSwitchingEnabled = true;
				}
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001103 RID: 4355 RVA: 0x0002F03B File Offset: 0x0002D23B
		// (set) Token: 0x06001104 RID: 4356 RVA: 0x0002F043 File Offset: 0x0002D243
		[Editor(false)]
		public Widget SliderParent
		{
			get
			{
				return this._sliderParent;
			}
			set
			{
				if (this._sliderParent != value)
				{
					this._sliderParent = value;
					base.OnPropertyChanged<Widget>(value, "SliderParent");
					this.SliderParent.AddState("Selected");
				}
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x0002F071 File Offset: 0x0002D271
		// (set) Token: 0x06001106 RID: 4358 RVA: 0x0002F079 File Offset: 0x0002D279
		[Editor(false)]
		public TextWidget SliderTextWidget
		{
			get
			{
				return this._sliderTextWidget;
			}
			set
			{
				if (this._sliderTextWidget != value)
				{
					this._sliderTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "SliderTextWidget");
					this.SliderTextWidget.AddState("Selected");
				}
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x0002F0A7 File Offset: 0x0002D2A7
		// (set) Token: 0x06001108 RID: 4360 RVA: 0x0002F0AF File Offset: 0x0002D2AF
		[Editor(false)]
		public bool IsTransferable
		{
			get
			{
				return this._isTransferable;
			}
			set
			{
				if (this._isTransferable != value)
				{
					this._isTransferable = value;
					base.OnPropertyChanged(value, "IsTransferable");
					this.UpdateDragAvailability();
				}
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x0002F0D3 File Offset: 0x0002D2D3
		// (set) Token: 0x0600110A RID: 4362 RVA: 0x0002F0DB File Offset: 0x0002D2DB
		[Editor(false)]
		public ButtonWidget EquipButton
		{
			get
			{
				return this._equipButton;
			}
			set
			{
				if (this._equipButton != value)
				{
					this._equipButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "EquipButton");
				}
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x0600110B RID: 4363 RVA: 0x0002F0F9 File Offset: 0x0002D2F9
		// (set) Token: 0x0600110C RID: 4364 RVA: 0x0002F101 File Offset: 0x0002D301
		[Editor(false)]
		public int TransactionCount
		{
			get
			{
				return this._transactionCount;
			}
			set
			{
				if (this._transactionCount != value)
				{
					this._transactionCount = value;
					base.OnPropertyChanged(value, "TransactionCount");
				}
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x0600110D RID: 4365 RVA: 0x0002F11F File Offset: 0x0002D31F
		// (set) Token: 0x0600110E RID: 4366 RVA: 0x0002F127 File Offset: 0x0002D327
		[Editor(false)]
		public int ItemCount
		{
			get
			{
				return this._itemCount;
			}
			set
			{
				if (this._itemCount != value)
				{
					this._itemCount = value;
					base.OnPropertyChanged(value, "ItemCount");
					this.UpdateDragAvailability();
				}
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x0002F14B File Offset: 0x0002D34B
		// (set) Token: 0x06001110 RID: 4368 RVA: 0x0002F153 File Offset: 0x0002D353
		[Editor(false)]
		public bool IsCivilian
		{
			get
			{
				return this._isCivilian;
			}
			set
			{
				if (this._isCivilian != value || !this._isCivilianStateSet)
				{
					this._isCivilian = value;
					base.OnPropertyChanged(value, "IsCivilian");
					this._isCivilianStateSet = true;
					this.UpdateEquipmentTypeState();
				}
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x0002F186 File Offset: 0x0002D386
		// (set) Token: 0x06001112 RID: 4370 RVA: 0x0002F18E File Offset: 0x0002D38E
		[Editor(false)]
		public bool IsStealth
		{
			get
			{
				return this._isStealth;
			}
			set
			{
				if (this._isStealth != value || !this._isStealthStateSet)
				{
					this._isStealth = value;
					base.OnPropertyChanged(value, "IsStealth");
					this._isStealthStateSet = true;
					this.UpdateEquipmentTypeState();
				}
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x0002F1C1 File Offset: 0x0002D3C1
		// (set) Token: 0x06001114 RID: 4372 RVA: 0x0002F1C9 File Offset: 0x0002D3C9
		[Editor(false)]
		public bool IsGenderDifferent
		{
			get
			{
				return this._isGenderDifferent;
			}
			set
			{
				if (this._isGenderDifferent != value)
				{
					this._isGenderDifferent = value;
					base.OnPropertyChanged(value, "IsGenderDifferent");
					this.UpdateEquipmentTypeState();
				}
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001115 RID: 4373 RVA: 0x0002F1ED File Offset: 0x0002D3ED
		// (set) Token: 0x06001116 RID: 4374 RVA: 0x0002F1F5 File Offset: 0x0002D3F5
		[Editor(false)]
		public bool IsEquipable
		{
			get
			{
				return this._isEquipable;
			}
			set
			{
				if (this._isEquipable != value)
				{
					this._isEquipable = value;
					base.OnPropertyChanged(value, "IsEquipable");
					this.UpdateDragAvailability();
				}
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001117 RID: 4375 RVA: 0x0002F219 File Offset: 0x0002D419
		// (set) Token: 0x06001118 RID: 4376 RVA: 0x0002F221 File Offset: 0x0002D421
		[Editor(false)]
		public bool IsNewlyAdded
		{
			get
			{
				return this._isNewlyAdded;
			}
			set
			{
				if (this._isNewlyAdded != value)
				{
					this._isNewlyAdded = value;
					base.OnPropertyChanged(value, "IsNewlyAdded");
					this.ItemImageIdentifier.SetRenderRequestedPreviousFrame(value);
				}
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001119 RID: 4377 RVA: 0x0002F24B File Offset: 0x0002D44B
		// (set) Token: 0x0600111A RID: 4378 RVA: 0x0002F253 File Offset: 0x0002D453
		[Editor(false)]
		public bool CanCharacterUseItem
		{
			get
			{
				return this._canCharacterUseItem;
			}
			set
			{
				if (this._canCharacterUseItem != value)
				{
					this._canCharacterUseItem = value;
					base.OnPropertyChanged(value, "CanCharacterUseItem");
					this.UpdateEquipmentTypeState();
				}
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x0600111B RID: 4379 RVA: 0x0002F277 File Offset: 0x0002D477
		// (set) Token: 0x0600111C RID: 4380 RVA: 0x0002F27F File Offset: 0x0002D47F
		[Editor(false)]
		public Brush DefaultBrush
		{
			get
			{
				return this._defaultBrush;
			}
			set
			{
				if (this._defaultBrush != value)
				{
					this._defaultBrush = value;
					base.OnPropertyChanged<Brush>(value, "DefaultBrush");
				}
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x0600111D RID: 4381 RVA: 0x0002F29D File Offset: 0x0002D49D
		// (set) Token: 0x0600111E RID: 4382 RVA: 0x0002F2A5 File Offset: 0x0002D4A5
		[Editor(false)]
		public Brush CantUseInSetBrush
		{
			get
			{
				return this._cantUseInSetBrush;
			}
			set
			{
				if (this._cantUseInSetBrush != value)
				{
					this._cantUseInSetBrush = value;
					base.OnPropertyChanged<Brush>(value, "CantUseInSetBrush");
				}
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x0600111F RID: 4383 RVA: 0x0002F2C3 File Offset: 0x0002D4C3
		// (set) Token: 0x06001120 RID: 4384 RVA: 0x0002F2CB File Offset: 0x0002D4CB
		[Editor(false)]
		public Brush CharacterCantUseBrush
		{
			get
			{
				return this._characterCantUseBrush;
			}
			set
			{
				if (this._characterCantUseBrush != value)
				{
					this._characterCantUseBrush = value;
					base.OnPropertyChanged<Brush>(value, "CharacterCantUseBrush");
				}
			}
		}

		// Token: 0x040007A8 RID: 1960
		private bool _isCivilianStateSet;

		// Token: 0x040007A9 RID: 1961
		private bool _isStealthStateSet;

		// Token: 0x040007AA RID: 1962
		private float _extendedUpdateTimer;

		// Token: 0x040007AB RID: 1963
		private TextWidget _nameTextWidget;

		// Token: 0x040007AC RID: 1964
		private TextWidget _countTextWidget;

		// Token: 0x040007AD RID: 1965
		private TextWidget _costTextWidget;

		// Token: 0x040007AE RID: 1966
		private int _profitState;

		// Token: 0x040007AF RID: 1967
		private BrushListPanel _mainContainer;

		// Token: 0x040007B0 RID: 1968
		private InventoryTupleExtensionControlsWidget _extendedControlsContainer;

		// Token: 0x040007B1 RID: 1969
		private InventoryTwoWaySliderWidget _slider;

		// Token: 0x040007B2 RID: 1970
		private Widget _sliderParent;

		// Token: 0x040007B3 RID: 1971
		private TextWidget _sliderTextWidget;

		// Token: 0x040007B4 RID: 1972
		private bool _isTransferable;

		// Token: 0x040007B5 RID: 1973
		private ButtonWidget _equipButton;

		// Token: 0x040007B6 RID: 1974
		private int _transactionCount;

		// Token: 0x040007B7 RID: 1975
		private int _itemCount;

		// Token: 0x040007B8 RID: 1976
		private bool _isCivilian;

		// Token: 0x040007B9 RID: 1977
		private bool _isStealth;

		// Token: 0x040007BA RID: 1978
		private bool _isGenderDifferent;

		// Token: 0x040007BB RID: 1979
		private bool _isEquipable;

		// Token: 0x040007BC RID: 1980
		private bool _canCharacterUseItem;

		// Token: 0x040007BD RID: 1981
		private bool _isNewlyAdded;

		// Token: 0x040007BE RID: 1982
		private Brush _defaultBrush;

		// Token: 0x040007BF RID: 1983
		private Brush _cantUseInSetBrush;

		// Token: 0x040007C0 RID: 1984
		private Brush _characterCantUseBrush;

		// Token: 0x040007C1 RID: 1985
		private string _itemID;
	}
}
