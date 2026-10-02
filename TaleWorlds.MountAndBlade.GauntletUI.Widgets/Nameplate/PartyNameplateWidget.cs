using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate
{
	// Token: 0x0200007E RID: 126
	public class PartyNameplateWidget : Widget
	{
		// Token: 0x060006EB RID: 1771 RVA: 0x000142BD File Offset: 0x000124BD
		public PartyNameplateWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x000142D8 File Offset: 0x000124D8
		protected float _animSpeedModifier
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x000142DF File Offset: 0x000124DF
		protected int _armyFontSizeOffset
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x000142E3 File Offset: 0x000124E3
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x000142EB File Offset: 0x000124EB
		public Widget HeadGroupWidget { get; set; }

		// Token: 0x060006F0 RID: 1776 RVA: 0x000142F4 File Offset: 0x000124F4
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._isFirstFrame)
			{
				this.NameplateFullNameTextWidget.Brush.GlobalAlphaFactor = 0f;
				this.NameplateTextWidget.Brush.GlobalAlphaFactor = 0f;
				this.NameplateExtraInfoTextWidget.Brush.GlobalAlphaFactor = 0f;
				this.PartyBannerWidget.Brush.GlobalAlphaFactor = 0f;
				this.SpeedTextWidget.Brush.GlobalAlphaFactor = 0f;
				this.ParleyIconWidget.AlphaFactor = 0f;
				this.ShipBannerContainerWidget.SetGlobalAlphaRecursively(0f);
				this._defaultNameplateFontSize = this.NameplateTextWidget.ReadOnlyBrush.FontSize;
				this._isFirstFrame = false;
			}
			int num = (this.IsArmy ? (this._defaultNameplateFontSize + this._armyFontSizeOffset) : this._defaultNameplateFontSize);
			if (this.NameplateTextWidget.Brush.FontSize != num)
			{
				this.NameplateTextWidget.Brush.FontSize = num;
			}
			this._screenWidth = base.Context.TwoDimensionContext.Width;
			this._screenHeight = base.Context.TwoDimensionContext.Height;
			this.UpdateNameplatesVisibility(dt);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001442E File Offset: 0x0001262E
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.UpdateNameplatesScreenPosition();
			this.UpdateTutorialStatus();
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00014444 File Offset: 0x00012644
		protected virtual void UpdateNameplatesVisibility(float dt)
		{
			float num = (float)(this.IsVisibleOnMap ? 1 : 0);
			float num2 = 0f;
			this.PartyBannerWidget.IsVisible = true;
			this.NameplateTextWidget.IsVisible = this.IsVisibleOnMap;
			this.NameplateFullNameTextWidget.IsVisible = this.IsVisibleOnMap;
			this.BloodFeudIconWidget.IsVisible = this.IsVisibleOnMap && this.HasBloodFeud;
			this.SpeedTextWidget.IsVisible = this.IsVisibleOnMap;
			this.SpeedIconWidget.IsVisible = this.IsVisibleOnMap;
			this.DisorganizedWidget.IsVisible = this.IsVisibleOnMap && this.IsDisorganized;
			this.TrackerFrame.IsVisible = false;
			base.IsEnabled = false;
			this._isNameplateVisible = this.IsVisibleOnMap && !this.IsPositionOutsideScreen();
			base.IsVisible = this._isNameplateVisible || this.IsShipBannerVisible;
			this.NameplateLayoutListPanel.IsVisible = this._isNameplateVisible;
			this.HeadGroupWidget.IsVisible = this._isNameplateVisible;
			this.ShipBannerContainerWidget.IsVisible = this.IsShipBannerVisible;
			if (this.IsVisibleOnMap)
			{
				if (this._initialDelayAmount <= 0f)
				{
					num2 = (float)(this.ShouldShowFullName ? 1 : 0);
				}
				else
				{
					this._initialDelayAmount -= dt;
					num2 = 1f;
				}
			}
			this.NameplateTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(this.NameplateTextWidget.ReadOnlyBrush.GlobalAlphaFactor, num, dt * this._animSpeedModifier, 1E-05f);
			this.NameplateFullNameTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(this.NameplateFullNameTextWidget.ReadOnlyBrush.GlobalAlphaFactor, num2, dt * this._animSpeedModifier, 1E-05f);
			this.SpeedTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(this.SpeedTextWidget.ReadOnlyBrush.GlobalAlphaFactor, num2, dt * this._animSpeedModifier, 1E-05f);
			float num3 = MathF.Lerp(this.SpeedIconWidget.AlphaFactor, num2, dt * this._animSpeedModifier, 1E-05f);
			this.SpeedIconWidget.SetGlobalAlphaRecursively(num3);
			this.BloodFeudIconWidget.SetGlobalAlphaRecursively(num3);
			this.NameplateExtraInfoTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(this.NameplateExtraInfoTextWidget.ReadOnlyBrush.GlobalAlphaFactor, (float)(this.ShouldShowFullName ? 1 : 0), dt * this._animSpeedModifier, 1E-05f);
			this.PartyBannerWidget.Brush.GlobalAlphaFactor = MathF.Lerp(this.PartyBannerWidget.ReadOnlyBrush.GlobalAlphaFactor, num, dt * this._animSpeedModifier, 1E-05f);
			this.ParleyIconWidget.AlphaFactor = MathF.Lerp(this.ParleyIconWidget.AlphaFactor, (float)(this.CanParley ? 1 : 0), dt * this._animSpeedModifier, 1E-05f);
			float num4 = MathF.Lerp(this.ShipBannerWidget.ReadOnlyBrush.GlobalAlphaFactor, (float)(this.IsShipBannerVisible ? 1 : 0), dt * this._animSpeedModifier, 1E-05f);
			this.ShipBannerContainerWidget.SetGlobalAlphaRecursively(num4);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00014750 File Offset: 0x00012950
		protected virtual void UpdateNameplatesScreenPosition()
		{
			if (!base.IsVisible)
			{
				return;
			}
			Widget headGroupWidget = this.HeadGroupWidget;
			float num = ((headGroupWidget != null) ? headGroupWidget.Size.Y : 0f);
			if (this._isNameplateVisible)
			{
				this.NameplateLayoutListPanel.ScaledPositionXOffset = base.Size.X / 2f - this.PartyBannerWidget.Size.X;
				this.NameplateLayoutListPanel.ScaledPositionYOffset = this.Position.y - this.HeadPosition.y + num;
			}
			base.ScaledPositionXOffset = this.HeadPosition.x - base.Size.X / 2f;
			base.ScaledPositionYOffset = this.HeadPosition.y - num;
			if (this.IsShipBannerVisible)
			{
				this.ShipBannerContainerWidget.ScaledPositionXOffset = this.ShipBannerPosition.x - base.ScaledPositionXOffset - this.ShipBannerContainerWidget.Size.X / 2f;
				this.ShipBannerContainerWidget.ScaledPositionYOffset = this.ShipBannerPosition.y - base.ScaledPositionYOffset - this.ShipBannerContainerWidget.Size.Y;
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0001487B File Offset: 0x00012A7B
		private void UpdateTutorialStatus()
		{
			if (this._tutorialAnimState == PartyNameplateWidget.TutorialAnimState.Start)
			{
				this._tutorialAnimState = PartyNameplateWidget.TutorialAnimState.FirstFrame;
			}
			else
			{
				PartyNameplateWidget.TutorialAnimState tutorialAnimState = this._tutorialAnimState;
			}
			if (this.IsTargetedByTutorial)
			{
				this.SetState("Default");
				return;
			}
			this.SetState("Disabled");
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x000148B8 File Offset: 0x00012AB8
		protected bool IsPositionOutsideScreen()
		{
			return this.Position.X > this._screenWidth || this.HeadPosition.X > this._screenWidth || this.Position.X < 0f || this.HeadPosition.X < 0f || this.Position.Y > this._screenHeight || this.HeadPosition.Y > this._screenHeight || this.Position.Y < 0f || this.HeadPosition.Y < 0f;
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x00014979 File Offset: 0x00012B79
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x00014981 File Offset: 0x00012B81
		public ListPanel NameplateLayoutListPanel
		{
			get
			{
				return this._nameplateLayoutListPanel;
			}
			set
			{
				if (this._nameplateLayoutListPanel != value)
				{
					this._nameplateLayoutListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "NameplateLayoutListPanel");
				}
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0001499F File Offset: 0x00012B9F
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x000149A7 File Offset: 0x00012BA7
		public MaskedTextureWidget PartyBannerWidget
		{
			get
			{
				return this._partyBannerWidget;
			}
			set
			{
				if (this._partyBannerWidget != value)
				{
					this._partyBannerWidget = value;
					base.OnPropertyChanged<MaskedTextureWidget>(value, "PartyBannerWidget");
				}
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x000149C5 File Offset: 0x00012BC5
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x000149CD File Offset: 0x00012BCD
		public MaskedTextureWidget ShipBannerWidget
		{
			get
			{
				return this._shipBannerWidget;
			}
			set
			{
				if (this._shipBannerWidget != value)
				{
					this._shipBannerWidget = value;
					base.OnPropertyChanged<MaskedTextureWidget>(value, "ShipBannerWidget");
				}
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x000149EB File Offset: 0x00012BEB
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x000149F3 File Offset: 0x00012BF3
		public Widget ShipBannerContainerWidget
		{
			get
			{
				return this._shipBannerContainerWidget;
			}
			set
			{
				if (this._shipBannerContainerWidget != value)
				{
					this._shipBannerContainerWidget = value;
					base.OnPropertyChanged<Widget>(value, "ShipBannerContainerWidget");
				}
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x00014A11 File Offset: 0x00012C11
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x00014A19 File Offset: 0x00012C19
		public Widget ShipBannerFrameWidget
		{
			get
			{
				return this._shipBannerFrameWidget;
			}
			set
			{
				if (this._shipBannerFrameWidget != value)
				{
					this._shipBannerFrameWidget = value;
					base.OnPropertyChanged<Widget>(value, "ShipBannerFrameWidget");
				}
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x00014A37 File Offset: 0x00012C37
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x00014A3F File Offset: 0x00012C3F
		public Vec2 ShipBannerPosition
		{
			get
			{
				return this._shipBannerPosition;
			}
			set
			{
				if (this._shipBannerPosition != value)
				{
					this._shipBannerPosition = value;
					base.OnPropertyChanged(value, "ShipBannerPosition");
				}
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x00014A62 File Offset: 0x00012C62
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x00014A6A File Offset: 0x00012C6A
		public bool IsShipBannerVisible
		{
			get
			{
				return this._isShipBannerVisible;
			}
			set
			{
				if (this._isShipBannerVisible != value)
				{
					this._isShipBannerVisible = value;
					base.OnPropertyChanged(value, "IsShipBannerVisible");
				}
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x00014A88 File Offset: 0x00012C88
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x00014A90 File Offset: 0x00012C90
		public Widget TrackerFrame
		{
			get
			{
				return this._trackerFrame;
			}
			set
			{
				if (this._trackerFrame != value)
				{
					this._trackerFrame = value;
					base.OnPropertyChanged<Widget>(value, "TrackerFrame");
				}
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x00014AAE File Offset: 0x00012CAE
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x00014AB6 File Offset: 0x00012CB6
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position != value)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x00014AD9 File Offset: 0x00012CD9
		// (set) Token: 0x06000709 RID: 1801 RVA: 0x00014AE1 File Offset: 0x00012CE1
		public Vec2 HeadPosition
		{
			get
			{
				return this._headPosition;
			}
			set
			{
				if (this._headPosition != value)
				{
					this._headPosition = value;
					base.OnPropertyChanged(value, "HeadPosition");
				}
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x00014B04 File Offset: 0x00012D04
		// (set) Token: 0x0600070B RID: 1803 RVA: 0x00014B0C File Offset: 0x00012D0C
		public bool ShouldShowFullName
		{
			get
			{
				return this._shouldShowFullName;
			}
			set
			{
				if (this._shouldShowFullName != value)
				{
					this._shouldShowFullName = value;
					base.OnPropertyChanged(value, "ShouldShowFullName");
				}
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x00014B2A File Offset: 0x00012D2A
		// (set) Token: 0x0600070D RID: 1805 RVA: 0x00014B32 File Offset: 0x00012D32
		public bool CanParley
		{
			get
			{
				return this._canParley;
			}
			set
			{
				if (this._canParley != value)
				{
					this._canParley = value;
					base.OnPropertyChanged(value, "CanParley");
				}
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x00014B50 File Offset: 0x00012D50
		// (set) Token: 0x0600070F RID: 1807 RVA: 0x00014B58 File Offset: 0x00012D58
		public bool IsTargetedByTutorial
		{
			get
			{
				return this._isTargetedByTutorial;
			}
			set
			{
				if (this._isTargetedByTutorial != value)
				{
					this._isTargetedByTutorial = value;
					base.OnPropertyChanged(value, "IsTargetedByTutorial");
					this._tutorialAnimState = PartyNameplateWidget.TutorialAnimState.Start;
				}
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x00014B7D File Offset: 0x00012D7D
		// (set) Token: 0x06000711 RID: 1809 RVA: 0x00014B85 File Offset: 0x00012D85
		public bool IsInArmy
		{
			get
			{
				return this._isInArmy;
			}
			set
			{
				if (this._isInArmy != value)
				{
					this._isInArmy = value;
					base.OnPropertyChanged(value, "IsInArmy");
				}
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x00014BA3 File Offset: 0x00012DA3
		// (set) Token: 0x06000713 RID: 1811 RVA: 0x00014BAB File Offset: 0x00012DAB
		public bool IsInSettlement
		{
			get
			{
				return this._isInSettlement;
			}
			set
			{
				if (this._isInSettlement != value)
				{
					this._isInSettlement = value;
					base.OnPropertyChanged(value, "IsInSettlement");
				}
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x00014BC9 File Offset: 0x00012DC9
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x00014BD1 File Offset: 0x00012DD1
		public bool IsArmy
		{
			get
			{
				return this._isArmy;
			}
			set
			{
				if (this._isArmy != value)
				{
					this._isArmy = value;
					base.OnPropertyChanged(value, "IsArmy");
				}
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x00014BEF File Offset: 0x00012DEF
		// (set) Token: 0x06000717 RID: 1815 RVA: 0x00014BF7 File Offset: 0x00012DF7
		public bool IsVisibleOnMap
		{
			get
			{
				return this._isVisibleOnMap;
			}
			set
			{
				if (this._isVisibleOnMap != value)
				{
					this._isVisibleOnMap = value;
					base.OnPropertyChanged(value, "IsVisibleOnMap");
				}
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x00014C15 File Offset: 0x00012E15
		// (set) Token: 0x06000719 RID: 1817 RVA: 0x00014C1D File Offset: 0x00012E1D
		public bool IsInside
		{
			get
			{
				return this._isInside;
			}
			set
			{
				if (this._isInside != value)
				{
					this._isInside = value;
					base.OnPropertyChanged(value, "IsInside");
				}
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x00014C3B File Offset: 0x00012E3B
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x00014C43 File Offset: 0x00012E43
		public bool IsHigh
		{
			get
			{
				return this._isHigh;
			}
			set
			{
				if (this._isHigh != value)
				{
					this._isHigh = value;
					base.OnPropertyChanged(value, "IsHigh");
				}
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x00014C61 File Offset: 0x00012E61
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x00014C69 File Offset: 0x00012E69
		public bool IsBehind
		{
			get
			{
				return this._isBehind;
			}
			set
			{
				if (this._isBehind != value)
				{
					this._isBehind = value;
					base.OnPropertyChanged(value, "IsBehind");
				}
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x00014C87 File Offset: 0x00012E87
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x00014C8F File Offset: 0x00012E8F
		public bool IsDisorganized
		{
			get
			{
				return this._isDisorganized;
			}
			set
			{
				if (this._isDisorganized != value)
				{
					this._isDisorganized = value;
					base.OnPropertyChanged(value, "IsDisorganized");
				}
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x00014CAD File Offset: 0x00012EAD
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x00014CB5 File Offset: 0x00012EB5
		public bool HasBloodFeud
		{
			get
			{
				return this._hasBloodFeud;
			}
			set
			{
				if (this._hasBloodFeud != value)
				{
					this._hasBloodFeud = value;
					base.OnPropertyChanged(value, "HasBloodFeud");
				}
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x00014CD3 File Offset: 0x00012ED3
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x00014CDB File Offset: 0x00012EDB
		public TextWidget NameplateTextWidget
		{
			get
			{
				return this._nameplateTextWidget;
			}
			set
			{
				if (this._nameplateTextWidget != value)
				{
					this._nameplateTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "NameplateTextWidget");
				}
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x00014CF9 File Offset: 0x00012EF9
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x00014D01 File Offset: 0x00012F01
		public TextWidget NameplateExtraInfoTextWidget
		{
			get
			{
				return this._nameplateExtraInfoTextWidget;
			}
			set
			{
				if (this._nameplateExtraInfoTextWidget != value)
				{
					this._nameplateExtraInfoTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "NameplateExtraInfoTextWidget");
				}
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x00014D1F File Offset: 0x00012F1F
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x00014D27 File Offset: 0x00012F27
		public TextWidget NameplateFullNameTextWidget
		{
			get
			{
				return this._nameplateFullNameTextWidget;
			}
			set
			{
				if (this._nameplateFullNameTextWidget != value)
				{
					this._nameplateFullNameTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "NameplateFullNameTextWidget");
				}
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x00014D45 File Offset: 0x00012F45
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x00014D4D File Offset: 0x00012F4D
		public TextWidget SpeedTextWidget
		{
			get
			{
				return this._speedTextWidget;
			}
			set
			{
				if (this._speedTextWidget != value)
				{
					this._speedTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "SpeedTextWidget");
				}
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x00014D6B File Offset: 0x00012F6B
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x00014D73 File Offset: 0x00012F73
		public Widget SpeedIconWidget
		{
			get
			{
				return this._speedIconWidget;
			}
			set
			{
				if (value != this._speedIconWidget)
				{
					this._speedIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "SpeedIconWidget");
				}
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x00014D91 File Offset: 0x00012F91
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x00014D99 File Offset: 0x00012F99
		public Widget BloodFeudIconWidget
		{
			get
			{
				return this._bloodFeudIconWidget;
			}
			set
			{
				if (value != this._bloodFeudIconWidget)
				{
					this._bloodFeudIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "BloodFeudIconWidget");
				}
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x00014DB7 File Offset: 0x00012FB7
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x00014DBF File Offset: 0x00012FBF
		public Widget ParleyIconWidget
		{
			get
			{
				return this._parleyIconWidget;
			}
			set
			{
				if (value != this._parleyIconWidget)
				{
					this._parleyIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "ParleyIconWidget");
				}
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x00014DDD File Offset: 0x00012FDD
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x00014DE5 File Offset: 0x00012FE5
		public Widget DisorganizedWidget
		{
			get
			{
				return this._disorganizedWidget;
			}
			set
			{
				if (this._disorganizedWidget != value)
				{
					this._disorganizedWidget = value;
					base.OnPropertyChanged<Widget>(value, "DisorganizedWidget");
				}
			}
		}

		// Token: 0x040002F8 RID: 760
		protected bool _isFirstFrame = true;

		// Token: 0x040002F9 RID: 761
		protected float _screenWidth;

		// Token: 0x040002FA RID: 762
		protected float _screenHeight;

		// Token: 0x040002FB RID: 763
		protected float _initialDelayAmount = 2f;

		// Token: 0x040002FC RID: 764
		protected int _defaultNameplateFontSize;

		// Token: 0x040002FD RID: 765
		protected PartyNameplateWidget.TutorialAnimState _tutorialAnimState;

		// Token: 0x040002FE RID: 766
		protected bool _isNameplateVisible;

		// Token: 0x04000300 RID: 768
		private Vec2 _position;

		// Token: 0x04000301 RID: 769
		private Vec2 _headPosition;

		// Token: 0x04000302 RID: 770
		private TextWidget _nameplateTextWidget;

		// Token: 0x04000303 RID: 771
		private TextWidget _nameplateFullNameTextWidget;

		// Token: 0x04000304 RID: 772
		private TextWidget _speedTextWidget;

		// Token: 0x04000305 RID: 773
		private Widget _speedIconWidget;

		// Token: 0x04000306 RID: 774
		private Widget _bloodFeudIconWidget;

		// Token: 0x04000307 RID: 775
		private Widget _parleyIconWidget;

		// Token: 0x04000308 RID: 776
		private TextWidget _nameplateExtraInfoTextWidget;

		// Token: 0x04000309 RID: 777
		private Widget _trackerFrame;

		// Token: 0x0400030A RID: 778
		private Widget _disorganizedWidget;

		// Token: 0x0400030B RID: 779
		private ListPanel _nameplateLayoutListPanel;

		// Token: 0x0400030C RID: 780
		private MaskedTextureWidget _partyBannerWidget;

		// Token: 0x0400030D RID: 781
		private MaskedTextureWidget _shipBannerWidget;

		// Token: 0x0400030E RID: 782
		private Widget _shipBannerContainerWidget;

		// Token: 0x0400030F RID: 783
		private Widget _shipBannerFrameWidget;

		// Token: 0x04000310 RID: 784
		private Vec2 _shipBannerPosition;

		// Token: 0x04000311 RID: 785
		private bool _isShipBannerVisible;

		// Token: 0x04000312 RID: 786
		private bool _isVisibleOnMap;

		// Token: 0x04000313 RID: 787
		private bool _isInside;

		// Token: 0x04000314 RID: 788
		private bool _isBehind;

		// Token: 0x04000315 RID: 789
		private bool _isHigh;

		// Token: 0x04000316 RID: 790
		private bool _isInArmy;

		// Token: 0x04000317 RID: 791
		private bool _isInSettlement;

		// Token: 0x04000318 RID: 792
		private bool _isArmy;

		// Token: 0x04000319 RID: 793
		private bool _isTargetedByTutorial;

		// Token: 0x0400031A RID: 794
		private bool _shouldShowFullName;

		// Token: 0x0400031B RID: 795
		private bool _canParley;

		// Token: 0x0400031C RID: 796
		private bool _isDisorganized;

		// Token: 0x0400031D RID: 797
		private bool _hasBloodFeud;

		// Token: 0x020001B7 RID: 439
		public enum TutorialAnimState
		{
			// Token: 0x04000A15 RID: 2581
			Idle,
			// Token: 0x04000A16 RID: 2582
			Start,
			// Token: 0x04000A17 RID: 2583
			FirstFrame,
			// Token: 0x04000A18 RID: 2584
			Playing
		}
	}
}
