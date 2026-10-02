using System;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.Overlay
{
	// Token: 0x02000111 RID: 273
	public class ArmyOverlayWidget : OverlayBaseWidget
	{
		// Token: 0x06000E90 RID: 3728 RVA: 0x00028453 File Offset: 0x00026653
		public ArmyOverlayWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00028464 File Offset: 0x00026664
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			int num = this._armyListGridWidget.Children.Count<Widget>((Widget c) => c.IsVisible);
			if (num != this._armyItemCount)
			{
				this.Overlay.SetState("Reset");
				this._armyItemCount = num;
			}
			this.RefreshOverlayExtendState(!this._initialized);
			this.UpdateExtendButtonVisual();
			this._initialized = true;
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x000284E4 File Offset: 0x000266E4
		private void RefreshOverlayExtendState(bool forceSetPosition)
		{
			string text = (this._isInfoBarExtended ? "MapExtended" : "MapNormal");
			string text2 = (this._isOverlayExtended ? "OverlayExtended" : "OverlayNormal");
			if (text + text2 != this.Overlay.CurrentState)
			{
				if (!this._isOverlayExtended)
				{
					if (forceSetPosition)
					{
						VisualState visualState;
						this.Overlay.VisualDefinition.VisualStates.TryGetValue(text + text2, out visualState);
						this.Overlay.PositionYOffset = visualState.PositionYOffset;
					}
				}
				else
				{
					float y = this.ArmyListGridWidget.Size.Y;
					float num;
					if (this._isInfoBarExtended)
					{
						VisualState visualState2;
						this.Overlay.VisualDefinition.VisualStates.TryGetValue("MapExtendedOverlayNormal", out visualState2);
						num = visualState2.PositionYOffset - y * base._inverseScaleToUse;
					}
					else
					{
						VisualState visualState3;
						this.Overlay.VisualDefinition.VisualStates.TryGetValue("MapNormalOverlayNormal", out visualState3);
						num = visualState3.PositionYOffset - y * base._inverseScaleToUse;
					}
					if (forceSetPosition)
					{
						this.Overlay.PositionYOffset = num;
					}
					VisualState visualState4;
					this.Overlay.VisualDefinition.VisualStates.TryGetValue(text + text2, out visualState4);
					visualState4.PositionYOffset = num;
				}
				this.Overlay.SetState(text + text2);
			}
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00028640 File Offset: 0x00026840
		private void UpdateExtendButtonVisual()
		{
			foreach (Style style in this.ExtendButton.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].VerticalFlip = this._isOverlayExtended;
				}
			}
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x000286B8 File Offset: 0x000268B8
		private void OnExtendButtonClick(Widget button)
		{
			this._isOverlayExtended = !this._isOverlayExtended;
			this.UpdateExtendButtonVisual();
			this.RefreshOverlayExtendState(false);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x000286D8 File Offset: 0x000268D8
		private void OnArmyListPageCountChanged()
		{
			if (this.PageControlWidget.PageCount == 1)
			{
				this.Overlay.PositionXOffset = 40f;
				this.ExtendButton.PositionXOffset = -40f;
				return;
			}
			this.Overlay.PositionXOffset = 0f;
			this.ExtendButton.PositionXOffset = 0f;
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x00028734 File Offset: 0x00026934
		// (set) Token: 0x06000E97 RID: 3735 RVA: 0x0002873C File Offset: 0x0002693C
		[Editor(false)]
		public Widget Overlay
		{
			get
			{
				return this._overlay;
			}
			set
			{
				if (this._overlay != value)
				{
					this._overlay = value;
					base.OnPropertyChanged<Widget>(value, "Overlay");
				}
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x0002875A File Offset: 0x0002695A
		// (set) Token: 0x06000E99 RID: 3737 RVA: 0x00028762 File Offset: 0x00026962
		[Editor(false)]
		public GridWidget ArmyListGridWidget
		{
			get
			{
				return this._armyListGridWidget;
			}
			set
			{
				if (this._armyListGridWidget != value)
				{
					this._armyListGridWidget = value;
					base.OnPropertyChanged<GridWidget>(value, "ArmyListGridWidget");
				}
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06000E9A RID: 3738 RVA: 0x00028780 File Offset: 0x00026980
		// (set) Token: 0x06000E9B RID: 3739 RVA: 0x00028788 File Offset: 0x00026988
		[Editor(false)]
		public ButtonWidget ExtendButton
		{
			get
			{
				return this._extendButton;
			}
			set
			{
				if (this._extendButton != value)
				{
					this._extendButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ExtendButton");
					if (this._extendButton != null)
					{
						this._extendButton.ClickEventHandlers.Add(new Action<Widget>(this.OnExtendButtonClick));
					}
				}
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06000E9C RID: 3740 RVA: 0x000287D5 File Offset: 0x000269D5
		// (set) Token: 0x06000E9D RID: 3741 RVA: 0x000287DD File Offset: 0x000269DD
		[Editor(false)]
		public bool IsInfoBarExtended
		{
			get
			{
				return this._isInfoBarExtended;
			}
			set
			{
				if (this._isInfoBarExtended != value)
				{
					this._isInfoBarExtended = value;
					base.OnPropertyChanged(value, "IsInfoBarExtended");
				}
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06000E9E RID: 3742 RVA: 0x000287FB File Offset: 0x000269FB
		// (set) Token: 0x06000E9F RID: 3743 RVA: 0x00028804 File Offset: 0x00026A04
		[Editor(false)]
		public ContainerPageControlWidget PageControlWidget
		{
			get
			{
				return this._pageControlWidget;
			}
			set
			{
				if (value != this._pageControlWidget)
				{
					if (this._pageControlWidget != null)
					{
						this._pageControlWidget.OnPageCountChanged -= this.OnArmyListPageCountChanged;
					}
					this._pageControlWidget = value;
					this._pageControlWidget.OnPageCountChanged += this.OnArmyListPageCountChanged;
					base.OnPropertyChanged<ContainerPageControlWidget>(value, "PageControlWidget");
				}
			}
		}

		// Token: 0x0400069E RID: 1694
		private bool _isOverlayExtended = true;

		// Token: 0x0400069F RID: 1695
		private int _armyItemCount;

		// Token: 0x040006A0 RID: 1696
		private bool _initialized;

		// Token: 0x040006A1 RID: 1697
		private Widget _overlay;

		// Token: 0x040006A2 RID: 1698
		private bool _isInfoBarExtended;

		// Token: 0x040006A3 RID: 1699
		private ButtonWidget _extendButton;

		// Token: 0x040006A4 RID: 1700
		private GridWidget _armyListGridWidget;

		// Token: 0x040006A5 RID: 1701
		private ContainerPageControlWidget _pageControlWidget;
	}
}
