using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000B9 RID: 185
	public class MultiplayerLobbyArmoryCosmeticObtainPopupWidget : Widget
	{
		// Token: 0x060009D1 RID: 2513 RVA: 0x0001BB9D File Offset: 0x00019D9D
		public MultiplayerLobbyArmoryCosmeticObtainPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0001BBB0 File Offset: 0x00019DB0
		private void OnObtainStateChanged(int newState)
		{
			if (newState == 0)
			{
				this.ItemPreviewListPanel.IsVisible = true;
				this.ActionButtonWidget.IsEnabled = true;
				this.CancelButtonWidget.IsEnabled = true;
				this.ResultSuccessfulIconWidget.IsVisible = false;
				this.ResultFailedIconWidget.IsVisible = false;
				this.ResultTextWidget.IsVisible = false;
				this.LoadingAnimationWidget.IsVisible = false;
				return;
			}
			if (newState == 1)
			{
				this.LoadingAnimationWidget.IsVisible = true;
				this.CancelButtonWidget.IsEnabled = false;
				this.ActionButtonWidget.IsEnabled = false;
				this.ItemPreviewListPanel.IsVisible = false;
				this.ResultSuccessfulIconWidget.IsVisible = false;
				this.ResultFailedIconWidget.IsVisible = false;
				this.ResultTextWidget.IsVisible = false;
				return;
			}
			if (newState == 2 || newState == 3)
			{
				this.CancelButtonWidget.IsEnabled = true;
				this.ActionButtonWidget.IsEnabled = true;
				this.ResultTextWidget.IsVisible = true;
				if (newState == 2)
				{
					this.ResultSuccessfulIconWidget.IsVisible = true;
				}
				else
				{
					this.ResultFailedIconWidget.IsVisible = true;
				}
				this.ItemPreviewListPanel.IsVisible = false;
				this.LoadingAnimationWidget.IsVisible = false;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x0001BCD0 File Offset: 0x00019ED0
		// (set) Token: 0x060009D4 RID: 2516 RVA: 0x0001BCD8 File Offset: 0x00019ED8
		[Editor(false)]
		public int ObtainState
		{
			get
			{
				return this._obtainState;
			}
			set
			{
				if (value != this._obtainState)
				{
					this._obtainState = value;
					base.OnPropertyChanged(value, "ObtainState");
					this.OnObtainStateChanged(value);
				}
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x0001BCFD File Offset: 0x00019EFD
		// (set) Token: 0x060009D6 RID: 2518 RVA: 0x0001BD05 File Offset: 0x00019F05
		[Editor(false)]
		public ButtonWidget CancelButtonWidget
		{
			get
			{
				return this._cancelButtonWidget;
			}
			set
			{
				if (value != this._cancelButtonWidget)
				{
					this._cancelButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "CancelButtonWidget");
				}
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x0001BD23 File Offset: 0x00019F23
		// (set) Token: 0x060009D8 RID: 2520 RVA: 0x0001BD2B File Offset: 0x00019F2B
		[Editor(false)]
		public ListPanel ItemPreviewListPanel
		{
			get
			{
				return this._itemPreviewListPanel;
			}
			set
			{
				if (value != this._itemPreviewListPanel)
				{
					this._itemPreviewListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "ItemPreviewListPanel");
				}
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0001BD49 File Offset: 0x00019F49
		// (set) Token: 0x060009DA RID: 2522 RVA: 0x0001BD51 File Offset: 0x00019F51
		[Editor(false)]
		public ButtonWidget ActionButtonWidget
		{
			get
			{
				return this._actionButtonWidget;
			}
			set
			{
				if (value != this._actionButtonWidget)
				{
					this._actionButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ActionButtonWidget");
				}
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x0001BD6F File Offset: 0x00019F6F
		// (set) Token: 0x060009DC RID: 2524 RVA: 0x0001BD77 File Offset: 0x00019F77
		[Editor(false)]
		public Widget ResultSuccessfulIconWidget
		{
			get
			{
				return this._resultSuccessfulIconWidget;
			}
			set
			{
				if (value != this._resultSuccessfulIconWidget)
				{
					this._resultSuccessfulIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "ResultSuccessfulIconWidget");
				}
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x0001BD95 File Offset: 0x00019F95
		// (set) Token: 0x060009DE RID: 2526 RVA: 0x0001BD9D File Offset: 0x00019F9D
		[Editor(false)]
		public Widget ResultFailedIconWidget
		{
			get
			{
				return this._resultFailedIconWidget;
			}
			set
			{
				if (value != this._resultFailedIconWidget)
				{
					this._resultFailedIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "ResultFailedIconWidget");
				}
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x0001BDBB File Offset: 0x00019FBB
		// (set) Token: 0x060009E0 RID: 2528 RVA: 0x0001BDC3 File Offset: 0x00019FC3
		[Editor(false)]
		public TextWidget ResultTextWidget
		{
			get
			{
				return this._resultTextWidget;
			}
			set
			{
				if (value != this._resultTextWidget)
				{
					this._resultTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "ResultTextWidget");
				}
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x0001BDE1 File Offset: 0x00019FE1
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x0001BDE9 File Offset: 0x00019FE9
		[Editor(false)]
		public Widget LoadingAnimationWidget
		{
			get
			{
				return this._loadingAnimationWidget;
			}
			set
			{
				if (value != this._loadingAnimationWidget)
				{
					this._loadingAnimationWidget = value;
					base.OnPropertyChanged<Widget>(value, "LoadingAnimationWidget");
				}
			}
		}

		// Token: 0x0400046E RID: 1134
		private int _obtainState = -1;

		// Token: 0x0400046F RID: 1135
		private ButtonWidget _cancelButtonWidget;

		// Token: 0x04000470 RID: 1136
		private ListPanel _itemPreviewListPanel;

		// Token: 0x04000471 RID: 1137
		private ButtonWidget _actionButtonWidget;

		// Token: 0x04000472 RID: 1138
		private Widget _resultSuccessfulIconWidget;

		// Token: 0x04000473 RID: 1139
		private Widget _resultFailedIconWidget;

		// Token: 0x04000474 RID: 1140
		private TextWidget _resultTextWidget;

		// Token: 0x04000475 RID: 1141
		private Widget _loadingAnimationWidget;
	}
}
