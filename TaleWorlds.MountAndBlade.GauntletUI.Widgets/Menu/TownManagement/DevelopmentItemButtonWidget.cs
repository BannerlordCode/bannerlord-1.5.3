using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x0200010A RID: 266
	public class DevelopmentItemButtonWidget : ButtonWidget
	{
		// Token: 0x06000E2D RID: 3629 RVA: 0x00026F9D File Offset: 0x0002519D
		public DevelopmentItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00026FA8 File Offset: 0x000251A8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			ButtonWidget buttonWidget;
			if (!this._isParentInitialized && (buttonWidget = base.ParentWidget as ButtonWidget) != null)
			{
				buttonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnParentClick));
				this._isParentInitialized = true;
			}
			if (!this.IsDaily)
			{
				this.HandleFocus();
				this.HandleEnabledStates();
				this.DevelopmentFrontVisualWidget.HeightSizePolicy = SizePolicy.Fixed;
				this.DevelopmentFrontVisualWidget.WidthSizePolicy = SizePolicy.Fixed;
				this.DevelopmentFrontVisualWidget.ScaledSuggestedHeight = this.DevelopmentBackVisualWidget.Size.Y;
				this.DevelopmentFrontVisualWidget.ScaledSuggestedWidth = this.DevelopmentBackVisualWidget.Size.X;
				if (this.IsProgressShown)
				{
					if (this.Progress > 0 || this.Level == 0)
					{
						this.ProgressClipWidget.HeightSizePolicy = SizePolicy.Fixed;
						this.ProgressClipWidget.ScaledSuggestedHeight = this.DevelopmentBackVisualWidget.Size.Y * ((float)this.Progress / 100f);
					}
					if (this.Level == 0)
					{
						this.DevelopmentBackVisualWidget.AlphaFactor = 0.8f;
						this.DevelopmentBackVisualWidget.SaturationFactor = -80f;
					}
					else
					{
						this.DevelopmentBackVisualWidget.AlphaFactor = 0.2f;
					}
				}
				else
				{
					this.ProgressClipWidget.HeightSizePolicy = SizePolicy.StretchToParent;
				}
				this.HandleChildrenVisibilities();
			}
			this.DevelopmentBackVisualWidget.CircularClipEnabled = true;
			this.DevelopmentBackVisualWidget.CircularClipRadius = this.DevelopmentBackVisualWidget.Size.X / 2f * base._inverseScaleToUse - 10f * base._scaleToUse;
			this.DevelopmentBackVisualWidget.CircularClipSmoothingRadius = 3f;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00027148 File Offset: 0x00025348
		private void HandleFocus()
		{
			if (this.IsSelectedItem)
			{
				if (base.EventManager.LatestMouseUpWidget != null && base.EventManager.LatestMouseUpWidget != base.ParentWidget)
				{
					DevelopmentItemVisualButtonWidget developmentItemVisualButtonWidget;
					if ((developmentItemVisualButtonWidget = base.EventManager.LatestMouseUpWidget as DevelopmentItemVisualButtonWidget) != null)
					{
						string spriteCode = developmentItemVisualButtonWidget.SpriteCode;
						DevelopmentItemVisualButtonWidget developmentItemVisualButtonWidget2 = this.DevelopmentBackVisualWidget as DevelopmentItemVisualButtonWidget;
						if (spriteCode == ((developmentItemVisualButtonWidget2 != null) ? developmentItemVisualButtonWidget2.SpriteCode : null))
						{
							goto IL_0067;
						}
					}
					this.IsSelectedItem = false;
				}
				IL_0067:
				if (base.EventManager.DraggedWidget != null)
				{
					this.IsSelectedItem = false;
				}
			}
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x000271D0 File Offset: 0x000253D0
		private void HandleChildrenVisibilities()
		{
			this.SetAsActiveButtonWidget.IsVisible = this.IsSelectedItem;
			this.AddToQueueButtonWidget.IsVisible = this.IsSelectedItem;
			this.SelectedBlackOverlayWidget.IsVisible = this.IsSelectedItem;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00027205 File Offset: 0x00025405
		private void HandleEnabledStates()
		{
			base.ParentWidget.DoNotPassEventsToChildren = !this.IsSelectedItem;
			base.DoNotPassEventsToChildren = !this.IsSelectedItem;
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0002722A File Offset: 0x0002542A
		private void OnParentClick(Widget widget)
		{
			if (!this.IsSelectedItem && this.CanBuild)
			{
				this.IsSelectedItem = true;
			}
			if (!this.CanBuild)
			{
				DevelopmentNameTextWidget nameTextWidget = this.NameTextWidget;
				if (nameTextWidget == null)
				{
					return;
				}
				nameTextWidget.StartMaxTextAnimation();
			}
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0002725B File Offset: 0x0002545B
		private void OnAddToQueueClick(Widget widget)
		{
			this.IsSelectedItem = false;
			base.EventFired("OnAddToQueue", Array.Empty<object>());
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00027274 File Offset: 0x00025474
		private void OnSetAsActiveDevelopmentClick(Widget widget)
		{
			this.IsSelectedItem = false;
			base.EventFired("SetAsActive", Array.Empty<object>());
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x0002728D File Offset: 0x0002548D
		private void UpdateDevelopmentLevelVisual(int level)
		{
			if (!this.IsDaily)
			{
				this.DevelopmentLevelVisualWidget.SetState(level.ToString());
				this.DevelopmentLevelVisualWidget.IsVisible = level >= 0;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x000272BB File Offset: 0x000254BB
		// (set) Token: 0x06000E37 RID: 3639 RVA: 0x000272C3 File Offset: 0x000254C3
		[Editor(false)]
		public bool IsSelectedItem
		{
			get
			{
				return this._isSelectedItem;
			}
			set
			{
				if (this._isSelectedItem != value)
				{
					this._isSelectedItem = value;
					base.OnPropertyChanged(value, "IsSelectedItem");
				}
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x000272E1 File Offset: 0x000254E1
		// (set) Token: 0x06000E39 RID: 3641 RVA: 0x000272E9 File Offset: 0x000254E9
		[Editor(false)]
		public Widget SelectedBlackOverlayWidget
		{
			get
			{
				return this._selectedBlackOverlayWidget;
			}
			set
			{
				if (this._selectedBlackOverlayWidget != value)
				{
					this._selectedBlackOverlayWidget = value;
					base.OnPropertyChanged<Widget>(value, "SelectedBlackOverlayWidget");
				}
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00027307 File Offset: 0x00025507
		// (set) Token: 0x06000E3B RID: 3643 RVA: 0x0002730F File Offset: 0x0002550F
		[Editor(false)]
		public DevelopmentNameTextWidget NameTextWidget
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
					base.OnPropertyChanged<DevelopmentNameTextWidget>(value, "NameTextWidget");
				}
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x0002732D File Offset: 0x0002552D
		// (set) Token: 0x06000E3D RID: 3645 RVA: 0x00027335 File Offset: 0x00025535
		[Editor(false)]
		public ButtonWidget AddToQueueButtonWidget
		{
			get
			{
				return this._addToQueueButtonWidget;
			}
			set
			{
				if (this._addToQueueButtonWidget != value)
				{
					this._addToQueueButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "AddToQueueButtonWidget");
					value.ClickEventHandlers.Add(new Action<Widget>(this.OnAddToQueueClick));
				}
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x0002736A File Offset: 0x0002556A
		// (set) Token: 0x06000E3F RID: 3647 RVA: 0x00027372 File Offset: 0x00025572
		[Editor(false)]
		public ButtonWidget SetAsActiveButtonWidget
		{
			get
			{
				return this._setAsActiveButtonWidget;
			}
			set
			{
				if (this._setAsActiveButtonWidget != value)
				{
					this._setAsActiveButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "SetAsActiveButtonWidget");
					value.ClickEventHandlers.Add(new Action<Widget>(this.OnSetAsActiveDevelopmentClick));
				}
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x000273A7 File Offset: 0x000255A7
		// (set) Token: 0x06000E41 RID: 3649 RVA: 0x000273AF File Offset: 0x000255AF
		[Editor(false)]
		public Widget DevelopmentLevelVisualWidget
		{
			get
			{
				return this._developmentLevelVisualWidget;
			}
			set
			{
				if (this._developmentLevelVisualWidget != value)
				{
					this._developmentLevelVisualWidget = value;
					base.OnPropertyChanged<Widget>(value, "DevelopmentLevelVisualWidget");
					this.UpdateDevelopmentLevelVisual(this.Level);
				}
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x000273D9 File Offset: 0x000255D9
		// (set) Token: 0x06000E43 RID: 3651 RVA: 0x000273E1 File Offset: 0x000255E1
		[Editor(false)]
		public Widget ProgressClipWidget
		{
			get
			{
				return this._progressClipWidget;
			}
			set
			{
				if (this._progressClipWidget != value)
				{
					this._progressClipWidget = value;
					base.OnPropertyChanged<Widget>(value, "ProgressClipWidget");
				}
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x000273FF File Offset: 0x000255FF
		// (set) Token: 0x06000E45 RID: 3653 RVA: 0x00027407 File Offset: 0x00025607
		[Editor(false)]
		public bool IsProgressShown
		{
			get
			{
				return this._isProgressShown;
			}
			set
			{
				if (this._isProgressShown != value)
				{
					this._isProgressShown = value;
					base.OnPropertyChanged(value, "IsProgressShown");
				}
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x00027425 File Offset: 0x00025625
		// (set) Token: 0x06000E47 RID: 3655 RVA: 0x0002742D File Offset: 0x0002562D
		[Editor(false)]
		public bool CanBuild
		{
			get
			{
				return this._canBuild;
			}
			set
			{
				if (this._canBuild != value)
				{
					this._canBuild = value;
					base.OnPropertyChanged(value, "CanBuild");
				}
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x0002744B File Offset: 0x0002564B
		// (set) Token: 0x06000E49 RID: 3657 RVA: 0x00027453 File Offset: 0x00025653
		[Editor(false)]
		public Widget DevelopmentBackVisualWidget
		{
			get
			{
				return this._developmentBackVisualWidget;
			}
			set
			{
				if (this._developmentBackVisualWidget != value)
				{
					this._developmentBackVisualWidget = value;
					base.OnPropertyChanged<Widget>(value, "DevelopmentBackVisualWidget");
				}
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06000E4A RID: 3658 RVA: 0x00027471 File Offset: 0x00025671
		// (set) Token: 0x06000E4B RID: 3659 RVA: 0x00027479 File Offset: 0x00025679
		[Editor(false)]
		public Widget DevelopmentFrontVisualWidget
		{
			get
			{
				return this._developmentFrontVisualWidget;
			}
			set
			{
				if (this._developmentFrontVisualWidget != value)
				{
					this._developmentFrontVisualWidget = value;
					base.OnPropertyChanged<Widget>(value, "DevelopmentFrontVisualWidget");
				}
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x00027497 File Offset: 0x00025697
		// (set) Token: 0x06000E4D RID: 3661 RVA: 0x0002749F File Offset: 0x0002569F
		[Editor(false)]
		public bool IsProgressIndicatorsEnabled
		{
			get
			{
				return this._isProgressIndicatorsEnabled;
			}
			set
			{
				if (this._isProgressIndicatorsEnabled != value)
				{
					this._isProgressIndicatorsEnabled = value;
					base.OnPropertyChanged(value, "IsProgressIndicatorsEnabled");
				}
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06000E4E RID: 3662 RVA: 0x000274BD File Offset: 0x000256BD
		// (set) Token: 0x06000E4F RID: 3663 RVA: 0x000274C5 File Offset: 0x000256C5
		[Editor(false)]
		public bool IsDaily
		{
			get
			{
				return this._isDaily;
			}
			set
			{
				if (this._isDaily != value)
				{
					this._isDaily = value;
					base.OnPropertyChanged(value, "IsDaily");
				}
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x000274E3 File Offset: 0x000256E3
		// (set) Token: 0x06000E51 RID: 3665 RVA: 0x000274EB File Offset: 0x000256EB
		[Editor(false)]
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (this._level != value)
				{
					this._level = value;
					base.OnPropertyChanged(value, "Level");
					this.UpdateDevelopmentLevelVisual(value);
				}
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x00027510 File Offset: 0x00025710
		// (set) Token: 0x06000E53 RID: 3667 RVA: 0x00027518 File Offset: 0x00025718
		[Editor(false)]
		public int Progress
		{
			get
			{
				return this._progress;
			}
			set
			{
				if (this._progress != value)
				{
					this._progress = value;
					base.OnPropertyChanged(value, "Progress");
				}
			}
		}

		// Token: 0x04000672 RID: 1650
		private bool _isParentInitialized;

		// Token: 0x04000673 RID: 1651
		private bool _isSelectedItem;

		// Token: 0x04000674 RID: 1652
		private int _level;

		// Token: 0x04000675 RID: 1653
		private int _progress;

		// Token: 0x04000676 RID: 1654
		private bool _isDaily;

		// Token: 0x04000677 RID: 1655
		private bool _isProgressIndicatorsEnabled;

		// Token: 0x04000678 RID: 1656
		private Widget _developmentLevelVisualWidget;

		// Token: 0x04000679 RID: 1657
		private Widget _developmentBackVisualWidget;

		// Token: 0x0400067A RID: 1658
		private Widget _developmentFrontVisualWidget;

		// Token: 0x0400067B RID: 1659
		private Widget _selectedBlackOverlayWidget;

		// Token: 0x0400067C RID: 1660
		private ButtonWidget _addToQueueButtonWidget;

		// Token: 0x0400067D RID: 1661
		private ButtonWidget _setAsActiveButtonWidget;

		// Token: 0x0400067E RID: 1662
		private DevelopmentNameTextWidget _nameTextWidget;

		// Token: 0x0400067F RID: 1663
		private Widget _progressClipWidget;

		// Token: 0x04000680 RID: 1664
		private bool _isProgressShown;

		// Token: 0x04000681 RID: 1665
		private bool _canBuild;
	}
}
