using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x0200006B RID: 107
	public class PartyTroopTupleButtonWidget : ButtonWidget
	{
		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x000116C0 File Offset: 0x0000F8C0
		// (set) Token: 0x060005D4 RID: 1492 RVA: 0x000116C8 File Offset: 0x0000F8C8
		public string CharacterID { get; set; }

		// Token: 0x060005D5 RID: 1493 RVA: 0x000116D1 File Offset: 0x0000F8D1
		public PartyTroopTupleButtonWidget(UIContext context)
			: base(context)
		{
			base.OverrideDefaultStateSwitchingEnabled = true;
			base.AddState("Selected");
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x000116EC File Offset: 0x0000F8EC
		private void SetWidgetsState(string state)
		{
			this.SetState(state);
			string currentState = this._extendedControlsContainer.CurrentState;
			this._extendedControlsContainer.SetState(base.IsSelected ? "Selected" : "Default");
			this._main.SetState(state);
			if (currentState == "Default" && base.IsSelected)
			{
				base.EventFired("Opened", Array.Empty<object>());
				this.TransferSlider.IsExtended = true;
				this._extendedControlsContainer.IsExtended = true;
				return;
			}
			if (currentState == "Selected" && !base.IsSelected)
			{
				base.EventFired("Closed", Array.Empty<object>());
				this.TransferSlider.IsExtended = false;
				this._extendedControlsContainer.IsExtended = false;
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000117B4 File Offset: 0x0000F9B4
		protected override void RefreshState()
		{
			base.RefreshState();
			this._extendedControlsContainer.IsEnabled = base.IsSelected;
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

		// Token: 0x060005D8 RID: 1496 RVA: 0x00011834 File Offset: 0x0000FA34
		private void AssignScreenWidget()
		{
			Widget widget = this;
			while (widget != base.EventManager.Root && this._screenWidget == null)
			{
				PartyScreenWidget partyScreenWidget;
				if ((partyScreenWidget = widget as PartyScreenWidget) != null)
				{
					this._screenWidget = partyScreenWidget;
				}
				else
				{
					widget = widget.ParentWidget;
				}
			}
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00011875 File Offset: 0x0000FA75
		private void OnValueChanged(PropertyOwnerObject arg1, string arg2, int arg3)
		{
			if (arg2 == "ValueInt")
			{
				base.AcceptDrag = arg3 > 0;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001188E File Offset: 0x0000FA8E
		public PartyScreenWidget ScreenWidget
		{
			get
			{
				if (this._screenWidget == null)
				{
					this.AssignScreenWidget();
				}
				return this._screenWidget;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x000118A4 File Offset: 0x0000FAA4
		// (set) Token: 0x060005DC RID: 1500 RVA: 0x000118AC File Offset: 0x0000FAAC
		[Editor(false)]
		public bool IsTupleLeftSide
		{
			get
			{
				return this._isTupleLeftSide;
			}
			set
			{
				if (this._isTupleLeftSide != value)
				{
					this._isTupleLeftSide = value;
					base.OnPropertyChanged(value, "IsTupleLeftSide");
				}
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x000118CA File Offset: 0x0000FACA
		// (set) Token: 0x060005DE RID: 1502 RVA: 0x000118D4 File Offset: 0x0000FAD4
		[Editor(false)]
		public InventoryTwoWaySliderWidget TransferSlider
		{
			get
			{
				return this._transferSlider;
			}
			set
			{
				if (this._transferSlider != value)
				{
					this._transferSlider = value;
					base.OnPropertyChanged<InventoryTwoWaySliderWidget>(value, "TransferSlider");
					value.intPropertyChanged += this.OnValueChanged;
					this._transferSlider.AddState("Selected");
					this._transferSlider.OverrideDefaultStateSwitchingEnabled = true;
				}
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x0001192B File Offset: 0x0000FB2B
		// (set) Token: 0x060005E0 RID: 1504 RVA: 0x00011933 File Offset: 0x0000FB33
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
				}
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x00011951 File Offset: 0x0000FB51
		// (set) Token: 0x060005E2 RID: 1506 RVA: 0x00011959 File Offset: 0x0000FB59
		[Editor(false)]
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (this._isMainHero != value)
				{
					base.AcceptDrag = !value;
					this._isMainHero = value;
					base.OnPropertyChanged(value, "IsMainHero");
				}
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x00011981 File Offset: 0x0000FB81
		// (set) Token: 0x060005E4 RID: 1508 RVA: 0x00011989 File Offset: 0x0000FB89
		[Editor(false)]
		public bool IsPrisoner
		{
			get
			{
				return this._isPrisoner;
			}
			set
			{
				if (this._isPrisoner != value)
				{
					this._isPrisoner = value;
					base.OnPropertyChanged(value, "IsPrisoner");
				}
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x000119A7 File Offset: 0x0000FBA7
		// (set) Token: 0x060005E6 RID: 1510 RVA: 0x000119AF File Offset: 0x0000FBAF
		[Editor(false)]
		public int TransferAmount
		{
			get
			{
				return this._transferAmount;
			}
			set
			{
				if (this._transferAmount != value)
				{
					this._transferAmount = value;
					base.OnPropertyChanged(value, "TransferAmount");
				}
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x000119CD File Offset: 0x0000FBCD
		// (set) Token: 0x060005E8 RID: 1512 RVA: 0x000119D5 File Offset: 0x0000FBD5
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

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x000119F3 File Offset: 0x0000FBF3
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x000119FB File Offset: 0x0000FBFB
		[Editor(false)]
		public Widget Main
		{
			get
			{
				return this._main;
			}
			set
			{
				if (this._main != value)
				{
					this._main = value;
					base.OnPropertyChanged<Widget>(value, "Main");
				}
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x00011A19 File Offset: 0x0000FC19
		// (set) Token: 0x060005EC RID: 1516 RVA: 0x00011A21 File Offset: 0x0000FC21
		[Editor(false)]
		public Widget UpgradesPanel
		{
			get
			{
				return this._upgradesPanel;
			}
			set
			{
				if (this._upgradesPanel != value)
				{
					this._upgradesPanel = value;
					base.OnPropertyChanged<Widget>(value, "UpgradesPanel");
				}
			}
		}

		// Token: 0x0400027D RID: 637
		private PartyScreenWidget _screenWidget;

		// Token: 0x0400027E RID: 638
		public InventoryTwoWaySliderWidget _transferSlider;

		// Token: 0x0400027F RID: 639
		private bool _isTupleLeftSide;

		// Token: 0x04000280 RID: 640
		private bool _isTransferable;

		// Token: 0x04000281 RID: 641
		private bool _isMainHero;

		// Token: 0x04000282 RID: 642
		private bool _isPrisoner;

		// Token: 0x04000283 RID: 643
		private int _transferAmount;

		// Token: 0x04000284 RID: 644
		private InventoryTupleExtensionControlsWidget _extendedControlsContainer;

		// Token: 0x04000285 RID: 645
		private Widget _main;

		// Token: 0x04000286 RID: 646
		private Widget _upgradesPanel;
	}
}
