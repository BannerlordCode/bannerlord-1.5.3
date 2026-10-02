using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Perks
{
	// Token: 0x02000098 RID: 152
	public class MultiplayerPerkContainerPanelWidget : Widget
	{
		// Token: 0x06000848 RID: 2120 RVA: 0x00018035 File Offset: 0x00016235
		public MultiplayerPerkContainerPanelWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00018040 File Offset: 0x00016240
		protected override void OnUpdate(float dt)
		{
			Widget latestMouseUpWidget = base.EventManager.LatestMouseUpWidget;
			if (this.TroopTupleBodyWidget != null)
			{
				MultiplayerClassLoadoutTroopSubclassButtonWidget troopTupleBodyWidget = this.TroopTupleBodyWidget;
				if (troopTupleBodyWidget == null || !troopTupleBodyWidget.IsSelected)
				{
					goto IL_005E;
				}
			}
			if (!base.CheckIsMyChildRecursive(latestMouseUpWidget) && (this.PopupWidgetFirst.IsVisible || this.PopupWidgetSecond.IsVisible || this.PopupWidgetThird.IsVisible))
			{
				this.ClosePanel();
			}
			IL_005E:
			MultiplayerClassLoadoutTroopSubclassButtonWidget troopTupleBodyWidget2 = this.TroopTupleBodyWidget;
			if ((troopTupleBodyWidget2 == null || !troopTupleBodyWidget2.IsSelected) && this._currentSelectedItem != null)
			{
				this._currentSelectedItem.IsSelected = false;
				this._currentSelectedItem = null;
			}
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x000180E0 File Offset: 0x000162E0
		public void PerkSelected(MultiplayerPerkItemToggleWidget selectedItem)
		{
			if (selectedItem == this._currentSelectedItem || selectedItem == null)
			{
				this.ClosePanel();
				return;
			}
			if (selectedItem != null && selectedItem.ParentWidget != null)
			{
				if (this._currentSelectedItem != null)
				{
					this._currentSelectedItem.IsSelected = false;
				}
				int childIndex = selectedItem.ParentWidget.GetChildIndex(selectedItem);
				this.PopupWidgetFirst.IsVisible = childIndex == 0;
				this.PopupWidgetFirst.IsEnabled = childIndex == 0;
				this.PopupWidgetSecond.IsVisible = childIndex == 1;
				this.PopupWidgetSecond.IsEnabled = childIndex == 1;
				this.PopupWidgetThird.IsVisible = childIndex == 2;
				this.PopupWidgetThird.IsEnabled = childIndex == 2;
				this.PopupWidgetFirst.SetPopupPerksContainer(this);
				this.PopupWidgetSecond.SetPopupPerksContainer(this);
				this.PopupWidgetThird.SetPopupPerksContainer(this);
				this._currentSelectedItem = selectedItem;
				this._currentSelectedItem.IsSelected = true;
			}
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x000181C4 File Offset: 0x000163C4
		private void ClosePanel()
		{
			if (this._currentSelectedItem != null)
			{
				this._currentSelectedItem.IsSelected = false;
			}
			this._currentSelectedItem = null;
			this.PopupWidgetFirst.IsVisible = false;
			this.PopupWidgetSecond.IsVisible = false;
			this.PopupWidgetThird.IsVisible = false;
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x00018210 File Offset: 0x00016410
		// (set) Token: 0x0600084D RID: 2125 RVA: 0x00018218 File Offset: 0x00016418
		public MultiplayerPerkPopupWidget PopupWidgetFirst
		{
			get
			{
				return this._popupWidgetFirst;
			}
			set
			{
				if (value != this._popupWidgetFirst)
				{
					this._popupWidgetFirst = value;
					base.OnPropertyChanged<MultiplayerPerkPopupWidget>(value, "PopupWidgetFirst");
				}
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x00018236 File Offset: 0x00016436
		// (set) Token: 0x0600084F RID: 2127 RVA: 0x0001823E File Offset: 0x0001643E
		public MultiplayerPerkPopupWidget PopupWidgetSecond
		{
			get
			{
				return this._popupWidgetSecond;
			}
			set
			{
				if (value != this._popupWidgetSecond)
				{
					this._popupWidgetSecond = value;
					base.OnPropertyChanged<MultiplayerPerkPopupWidget>(value, "PopupWidgetSecond");
				}
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x0001825C File Offset: 0x0001645C
		// (set) Token: 0x06000851 RID: 2129 RVA: 0x00018264 File Offset: 0x00016464
		public MultiplayerPerkPopupWidget PopupWidgetThird
		{
			get
			{
				return this._popupWidgetThird;
			}
			set
			{
				if (value != this._popupWidgetThird)
				{
					this._popupWidgetThird = value;
					base.OnPropertyChanged<MultiplayerPerkPopupWidget>(value, "PopupWidgetThird");
				}
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x00018282 File Offset: 0x00016482
		// (set) Token: 0x06000853 RID: 2131 RVA: 0x0001828A File Offset: 0x0001648A
		public MultiplayerClassLoadoutTroopSubclassButtonWidget TroopTupleBodyWidget
		{
			get
			{
				return this._troopTupleBodyWidget;
			}
			set
			{
				if (value != this._troopTupleBodyWidget)
				{
					this._troopTupleBodyWidget = value;
					base.OnPropertyChanged<MultiplayerClassLoadoutTroopSubclassButtonWidget>(value, "TroopTupleBodyWidget");
				}
			}
		}

		// Token: 0x040003B1 RID: 945
		private MultiplayerPerkItemToggleWidget _currentSelectedItem;

		// Token: 0x040003B2 RID: 946
		private MultiplayerPerkPopupWidget _popupWidgetFirst;

		// Token: 0x040003B3 RID: 947
		private MultiplayerPerkPopupWidget _popupWidgetSecond;

		// Token: 0x040003B4 RID: 948
		private MultiplayerPerkPopupWidget _popupWidgetThird;

		// Token: 0x040003B5 RID: 949
		private MultiplayerClassLoadoutTroopSubclassButtonWidget _troopTupleBodyWidget;
	}
}
