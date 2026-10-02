using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GameMenu
{
	// Token: 0x02000157 RID: 343
	public class GameMenuWidget : Widget
	{
		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001243 RID: 4675 RVA: 0x00032D75 File Offset: 0x00030F75
		// (set) Token: 0x06001244 RID: 4676 RVA: 0x00032D7D File Offset: 0x00030F7D
		public int EncounterModeMenuWidth { get; set; }

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001245 RID: 4677 RVA: 0x00032D86 File Offset: 0x00030F86
		// (set) Token: 0x06001246 RID: 4678 RVA: 0x00032D8E File Offset: 0x00030F8E
		public int EncounterModeMenuHeight { get; set; }

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06001247 RID: 4679 RVA: 0x00032D97 File Offset: 0x00030F97
		// (set) Token: 0x06001248 RID: 4680 RVA: 0x00032D9F File Offset: 0x00030F9F
		public int EncounterModeMenuMarginTop { get; set; }

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06001249 RID: 4681 RVA: 0x00032DA8 File Offset: 0x00030FA8
		// (set) Token: 0x0600124A RID: 4682 RVA: 0x00032DB0 File Offset: 0x00030FB0
		public int NormalModeMenuWidth { get; set; }

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x0600124B RID: 4683 RVA: 0x00032DB9 File Offset: 0x00030FB9
		// (set) Token: 0x0600124C RID: 4684 RVA: 0x00032DC1 File Offset: 0x00030FC1
		public int NormalModeMenuHeight { get; set; }

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x0600124D RID: 4685 RVA: 0x00032DCA File Offset: 0x00030FCA
		// (set) Token: 0x0600124E RID: 4686 RVA: 0x00032DD2 File Offset: 0x00030FD2
		public int NormalModeMenuMarginTop { get; set; }

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x0600124F RID: 4687 RVA: 0x00032DDB File Offset: 0x00030FDB
		// (set) Token: 0x06001250 RID: 4688 RVA: 0x00032DE3 File Offset: 0x00030FE3
		public bool IsOverlayExtended
		{
			get
			{
				return this._isOverlayExtended;
			}
			private set
			{
				if (value != this._isOverlayExtended)
				{
					this._isOverlayExtended = value;
					this.UpdateOverlayState();
				}
			}
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00032DFB File Offset: 0x00030FFB
		public GameMenuWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x00032E14 File Offset: 0x00031014
		protected override void OnLateUpdate(float dt)
		{
			if (!this._firstFrame)
			{
				if (this.IsNight)
				{
					base.Color = Color.Lerp(base.Color, new Color(0.23921569f, 0.4509804f, 0.8f, 1f), dt);
				}
				else
				{
					base.Color = Color.Lerp(base.Color, Color.White, dt);
				}
			}
			else
			{
				if (this.IsNight)
				{
					base.Color = new Color(0.23921569f, 0.4509804f, 0.8f, 1f);
				}
				else
				{
					base.Color = Color.White;
				}
				this._firstFrame = false;
				this.RefreshSize();
			}
			if (base.Sprite == null && this.OverriddenSpriteMapBrush != null && this.SpriteName != null)
			{
				BrushLayer layer = this.OverriddenSpriteMapBrush.GetLayer(this.SpriteName);
				base.Sprite = ((layer != null) ? layer.Sprite : null);
			}
			base.OnLateUpdate(dt);
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x00032EFC File Offset: 0x000310FC
		private void RefreshSize()
		{
			base.SuggestedWidth = (float)(this.IsEncounterMenu ? this.EncounterModeMenuWidth : this.NormalModeMenuWidth);
			base.SuggestedHeight = (float)(this.IsEncounterMenu ? this.EncounterModeMenuHeight : this.NormalModeMenuHeight);
			base.ScaledSuggestedWidth = base.SuggestedWidth * base._scaleToUse;
			base.ScaledSuggestedHeight = base.SuggestedHeight * base._scaleToUse;
			base.MarginTop = (float)(this.IsEncounterMenu ? this.EncounterModeMenuMarginTop : this.NormalModeMenuMarginTop);
			this.ExtendButtonWidget.MarginTop = base.MarginTop;
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00032F97 File Offset: 0x00031197
		private void OnExtendButtonClick(Widget button)
		{
			this.IsOverlayExtended = !this.IsOverlayExtended;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00032FA8 File Offset: 0x000311A8
		public void UpdateOverlayState()
		{
			this.ScopeTargeter.IsScopeEnabled = this._isOverlayExtended;
			string text = (this._isOverlayExtended ? "Default" : "Disabled");
			this.Overlay.SetState(text);
			foreach (Style style in this.ExtendButtonArrowWidget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].HorizontalFlip = !this._isOverlayExtended;
				}
			}
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x00033054 File Offset: 0x00031254
		private void TitleTextWidget_PropertyChanged(PropertyOwnerObject widget, string propertyName, object propertyValue)
		{
			if (propertyName == "Text")
			{
				this.TitleContainerWidget.IsVisible = !string.IsNullOrEmpty((string)propertyValue);
				this.IsOverlayExtended = true;
			}
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x00033083 File Offset: 0x00031283
		private void OnOptionAdded(Widget parentWidget, Widget childWidget)
		{
			GameMenuItemWidget gameMenuItemWidget = childWidget as GameMenuItemWidget;
			gameMenuItemWidget.OnOptionStateChanged = (Action)Delegate.Combine(gameMenuItemWidget.OnOptionStateChanged, new Action(this.OnOptionStateChanged));
			this.IsOverlayExtended = true;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x000330B3 File Offset: 0x000312B3
		public void OnOptionStateChanged()
		{
			this.IsOverlayExtended = true;
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x000330BC File Offset: 0x000312BC
		private void OnOptionRemoved(Widget widget, Widget child)
		{
			this.IsOverlayExtended = true;
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x0600125A RID: 4698 RVA: 0x000330C5 File Offset: 0x000312C5
		// (set) Token: 0x0600125B RID: 4699 RVA: 0x000330CD File Offset: 0x000312CD
		[Editor(false)]
		public NavigationScopeTargeter ScopeTargeter
		{
			get
			{
				return this._scopeTargeter;
			}
			set
			{
				if (this._scopeTargeter != value)
				{
					this._scopeTargeter = value;
					base.OnPropertyChanged<NavigationScopeTargeter>(value, "ScopeTargeter");
				}
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x0600125C RID: 4700 RVA: 0x000330EB File Offset: 0x000312EB
		// (set) Token: 0x0600125D RID: 4701 RVA: 0x000330F3 File Offset: 0x000312F3
		[Editor(false)]
		public TextWidget TitleTextWidget
		{
			get
			{
				return this._titleTextWidget;
			}
			set
			{
				if (this._titleTextWidget != value)
				{
					this._titleTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "TitleTextWidget");
					if (value != null)
					{
						value.PropertyChanged += this.TitleTextWidget_PropertyChanged;
					}
				}
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x0600125E RID: 4702 RVA: 0x00033126 File Offset: 0x00031326
		// (set) Token: 0x0600125F RID: 4703 RVA: 0x0003312E File Offset: 0x0003132E
		[Editor(false)]
		public Widget TitleContainerWidget
		{
			get
			{
				return this._titleContainerWidget;
			}
			set
			{
				if (this._titleContainerWidget != value)
				{
					this._titleContainerWidget = value;
					base.OnPropertyChanged<Widget>(value, "TitleContainerWidget");
				}
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x0003314C File Offset: 0x0003134C
		// (set) Token: 0x06001261 RID: 4705 RVA: 0x00033154 File Offset: 0x00031354
		[Editor(false)]
		public bool IsNight
		{
			get
			{
				return this._isNight;
			}
			set
			{
				if (this._isNight != value)
				{
					this._isNight = value;
					base.OnPropertyChanged(value, "IsNight");
				}
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001262 RID: 4706 RVA: 0x00033172 File Offset: 0x00031372
		// (set) Token: 0x06001263 RID: 4707 RVA: 0x0003317A File Offset: 0x0003137A
		[Editor(false)]
		public bool IsEncounterMenu
		{
			get
			{
				return this._isEncounterMenu;
			}
			set
			{
				if (this._isEncounterMenu != value)
				{
					this._isEncounterMenu = value;
					base.OnPropertyChanged(value, "IsEncounterMenu");
					this.RefreshSize();
				}
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x0003319E File Offset: 0x0003139E
		// (set) Token: 0x06001265 RID: 4709 RVA: 0x000331A6 File Offset: 0x000313A6
		[Editor(false)]
		public Widget Overlay
		{
			get
			{
				return this._overlay;
			}
			set
			{
				if (value != this._overlay)
				{
					this._overlay = value;
					base.OnPropertyChanged<Widget>(value, "Overlay");
				}
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001266 RID: 4710 RVA: 0x000331C4 File Offset: 0x000313C4
		// (set) Token: 0x06001267 RID: 4711 RVA: 0x000331CC File Offset: 0x000313CC
		[Editor(false)]
		public ButtonWidget ExtendButtonWidget
		{
			get
			{
				return this._extendButtonWidget;
			}
			set
			{
				if (this._extendButtonWidget != value)
				{
					this._extendButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ExtendButtonWidget");
					if (this._extendButtonWidget != null)
					{
						this._extendButtonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnExtendButtonClick));
					}
				}
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x00033219 File Offset: 0x00031419
		// (set) Token: 0x06001269 RID: 4713 RVA: 0x00033221 File Offset: 0x00031421
		[Editor(false)]
		public BrushWidget ExtendButtonArrowWidget
		{
			get
			{
				return this._extendButtonArrowWidget;
			}
			set
			{
				if (value != this._extendButtonArrowWidget)
				{
					this._extendButtonArrowWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "ExtendButtonArrowWidget");
				}
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x0600126A RID: 4714 RVA: 0x0003323F File Offset: 0x0003143F
		// (set) Token: 0x0600126B RID: 4715 RVA: 0x00033248 File Offset: 0x00031448
		[Editor(false)]
		public ListPanel OptionItemsList
		{
			get
			{
				return this._optionItemsList;
			}
			set
			{
				if (value != this._optionItemsList)
				{
					this._optionItemsList = value;
					this._optionItemsList.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnOptionAdded));
					this._optionItemsList.ItemRemoveEventHandlers.Add(new Action<Widget, Widget>(this.OnOptionRemoved));
					base.OnPropertyChanged<ListPanel>(value, "OptionItemsList");
				}
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x000332A9 File Offset: 0x000314A9
		// (set) Token: 0x0600126D RID: 4717 RVA: 0x000332B1 File Offset: 0x000314B1
		[Editor(false)]
		public string SpriteName
		{
			get
			{
				return this._spriteName;
			}
			set
			{
				if (value != this._spriteName)
				{
					this._spriteName = value;
					base.OnPropertyChanged<string>(value, "SpriteName");
				}
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x0600126E RID: 4718 RVA: 0x000332D4 File Offset: 0x000314D4
		// (set) Token: 0x0600126F RID: 4719 RVA: 0x000332DC File Offset: 0x000314DC
		[Editor(false)]
		public string MenuId
		{
			get
			{
				return this._menuId;
			}
			set
			{
				if (value != this._menuId)
				{
					this._menuId = value;
					base.OnPropertyChanged<string>(value, "MenuId");
				}
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06001270 RID: 4720 RVA: 0x000332FF File Offset: 0x000314FF
		// (set) Token: 0x06001271 RID: 4721 RVA: 0x00033307 File Offset: 0x00031507
		[Editor(false)]
		public Brush OverriddenSpriteMapBrush
		{
			get
			{
				return this._overriddenSpriteMapBrush;
			}
			set
			{
				if (value != this._overriddenSpriteMapBrush)
				{
					this._overriddenSpriteMapBrush = value;
					base.OnPropertyChanged<Brush>(value, "OverriddenSpriteMapBrush");
				}
			}
		}

		// Token: 0x0400085B RID: 2139
		private bool _firstFrame = true;

		// Token: 0x04000862 RID: 2146
		private const string _extendedState = "Default";

		// Token: 0x04000863 RID: 2147
		private const string _hiddenState = "Disabled";

		// Token: 0x04000864 RID: 2148
		private bool _isOverlayExtended = true;

		// Token: 0x04000865 RID: 2149
		private NavigationScopeTargeter _scopeTargeter;

		// Token: 0x04000866 RID: 2150
		private TextWidget _titleTextWidget;

		// Token: 0x04000867 RID: 2151
		private Widget _titleContainerWidget;

		// Token: 0x04000868 RID: 2152
		private bool _isNight;

		// Token: 0x04000869 RID: 2153
		private bool _isEncounterMenu;

		// Token: 0x0400086A RID: 2154
		private Widget _overlay;

		// Token: 0x0400086B RID: 2155
		private ButtonWidget _extendButtonWidget;

		// Token: 0x0400086C RID: 2156
		private BrushWidget _extendButtonArrowWidget;

		// Token: 0x0400086D RID: 2157
		private ListPanel _optionItemsList;

		// Token: 0x0400086E RID: 2158
		private string _spriteName;

		// Token: 0x0400086F RID: 2159
		private string _menuId;

		// Token: 0x04000870 RID: 2160
		private Brush _overriddenSpriteMapBrush;
	}
}
