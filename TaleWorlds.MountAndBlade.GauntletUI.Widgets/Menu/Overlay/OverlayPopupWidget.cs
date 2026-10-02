using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.Overlay
{
	// Token: 0x02000114 RID: 276
	public class OverlayPopupWidget : Widget
	{
		// Token: 0x06000EC1 RID: 3777 RVA: 0x00028BA7 File Offset: 0x00026DA7
		public OverlayPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00028BB0 File Offset: 0x00026DB0
		public void SetCurrentCharacter(GameMenuPartyItemButtonWidget item)
		{
			this.NameTextWidget.Text = item.Name;
			this.DescriptionTextWidget.Text = item.Description;
			this.LocationTextWidget.Text = item.Location;
			this.PowerTextWidget.Text = item.Power;
			if (item.CurrentCharacterImageWidget != null)
			{
				this.CurrentCharacterImageWidget.ImageId = item.CurrentCharacterImageWidget.ImageId;
				this.CurrentCharacterImageWidget.TextureProviderName = item.CurrentCharacterImageWidget.TextureProviderName;
				this.CurrentCharacterImageWidget.AdditionalArgs = item.CurrentCharacterImageWidget.AdditionalArgs;
			}
			if (!base.ParentWidget.IsVisible)
			{
				this.OpenPopup();
			}
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x00028C5E File Offset: 0x00026E5E
		private void OpenPopup()
		{
			base.ParentWidget.IsVisible = true;
			base.EventFired("OnOpen", Array.Empty<object>());
			this._isOpen = true;
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x00028C83 File Offset: 0x00026E83
		private void ClosePopup()
		{
			base.ParentWidget.IsVisible = false;
			base.EventFired("OnClose", Array.Empty<object>());
			this._isOpen = false;
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00028CA8 File Offset: 0x00026EA8
		public void OnCloseButtonClick(Widget widget)
		{
			this.ClosePopup();
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00028CB0 File Offset: 0x00026EB0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isOpen && !base.IsRecursivelyVisible())
			{
				this.ClosePopup();
			}
			else if (!this._isOpen && base.IsRecursivelyVisible())
			{
				this.OpenPopup();
			}
			if (!(base.EventManager.LatestMouseDownWidget is GameMenuPartyItemButtonWidget) && base.EventManager.LatestMouseDownWidget != this && base.EventManager.LatestMouseDownWidget != this._closeButton && base.ParentWidget.IsVisible && (!base.CheckIsMyChildRecursive(base.EventManager.LatestMouseDownWidget) || this.ActionButtonsList.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget)))
			{
				this.ClosePopup();
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x00028D63 File Offset: 0x00026F63
		// (set) Token: 0x06000EC8 RID: 3784 RVA: 0x00028D6B File Offset: 0x00026F6B
		[Editor(false)]
		public ImageIdentifierWidget CurrentCharacterImageWidget
		{
			get
			{
				return this._currentCharacterImageWidget;
			}
			set
			{
				if (this._currentCharacterImageWidget != value)
				{
					this._currentCharacterImageWidget = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "CurrentCharacterImageWidget");
				}
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x00028D89 File Offset: 0x00026F89
		// (set) Token: 0x06000ECA RID: 3786 RVA: 0x00028D91 File Offset: 0x00026F91
		[Editor(false)]
		public TextWidget LocationTextWidget
		{
			get
			{
				return this._locationTextWidget;
			}
			set
			{
				if (this._locationTextWidget != value)
				{
					this._locationTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "LocationTextWidget");
				}
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x00028DAF File Offset: 0x00026FAF
		// (set) Token: 0x06000ECC RID: 3788 RVA: 0x00028DB7 File Offset: 0x00026FB7
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
				}
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x00028DD5 File Offset: 0x00026FD5
		// (set) Token: 0x06000ECE RID: 3790 RVA: 0x00028DDD File Offset: 0x00026FDD
		[Editor(false)]
		public TextWidget PowerTextWidget
		{
			get
			{
				return this._powerTextWidget;
			}
			set
			{
				if (this._powerTextWidget != value)
				{
					this._powerTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "PowerTextWidget");
				}
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x00028DFB File Offset: 0x00026FFB
		// (set) Token: 0x06000ED0 RID: 3792 RVA: 0x00028E03 File Offset: 0x00027003
		[Editor(false)]
		public TextWidget DescriptionTextWidget
		{
			get
			{
				return this._descriptionTextWidget;
			}
			set
			{
				if (this._descriptionTextWidget != value)
				{
					this._descriptionTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "DescriptionTextWidget");
				}
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x00028E21 File Offset: 0x00027021
		// (set) Token: 0x06000ED2 RID: 3794 RVA: 0x00028E29 File Offset: 0x00027029
		[Editor(false)]
		public Widget RelationBackgroundWidget
		{
			get
			{
				return this._relationBackgroundWidget;
			}
			set
			{
				if (this._relationBackgroundWidget != value)
				{
					this._relationBackgroundWidget = value;
					base.OnPropertyChanged<Widget>(value, "RelationBackgroundWidget");
				}
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x00028E47 File Offset: 0x00027047
		// (set) Token: 0x06000ED4 RID: 3796 RVA: 0x00028E4F File Offset: 0x0002704F
		[Editor(false)]
		public ListPanel ActionButtonsList
		{
			get
			{
				return this._actionButtonsList;
			}
			set
			{
				if (this._actionButtonsList != value)
				{
					this._actionButtonsList = value;
					base.OnPropertyChanged<ListPanel>(value, "ActionButtonsList");
				}
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x00028E6D File Offset: 0x0002706D
		// (set) Token: 0x06000ED6 RID: 3798 RVA: 0x00028E78 File Offset: 0x00027078
		[Editor(false)]
		public ButtonWidget CloseButton
		{
			get
			{
				return this._closeButton;
			}
			set
			{
				if (this._closeButton != value)
				{
					ButtonWidget closeButton = this._closeButton;
					if (closeButton != null)
					{
						closeButton.ClickEventHandlers.Remove(new Action<Widget>(this.OnCloseButtonClick));
					}
					this._closeButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "CloseButton");
					ButtonWidget closeButton2 = this._closeButton;
					if (closeButton2 == null)
					{
						return;
					}
					closeButton2.ClickEventHandlers.Add(new Action<Widget>(this.OnCloseButtonClick));
				}
			}
		}

		// Token: 0x040006B7 RID: 1719
		private bool _isOpen;

		// Token: 0x040006B8 RID: 1720
		private ImageIdentifierWidget _currentCharacterImageWidget;

		// Token: 0x040006B9 RID: 1721
		private TextWidget _locationTextWidget;

		// Token: 0x040006BA RID: 1722
		private TextWidget _descriptionTextWidget;

		// Token: 0x040006BB RID: 1723
		private TextWidget _powerTextWidget;

		// Token: 0x040006BC RID: 1724
		private TextWidget _nameTextWidget;

		// Token: 0x040006BD RID: 1725
		private Widget _relationBackgroundWidget;

		// Token: 0x040006BE RID: 1726
		private ButtonWidget _closeButton;

		// Token: 0x040006BF RID: 1727
		private ListPanel _actionButtonsList;
	}
}
