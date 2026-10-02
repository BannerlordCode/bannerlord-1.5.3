using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Notification
{
	// Token: 0x02000125 RID: 293
	public class MapNotificationItemWidget : BrushWidget
	{
		// Token: 0x06000F8D RID: 3981 RVA: 0x0002B053 File Offset: 0x00029253
		public MapNotificationItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0002B068 File Offset: 0x00029268
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._imageDetermined)
			{
				this.NotificationRingImageWidget.RegisterBrushStatesOfWidget();
				this.NotificationRingImageWidget.SetState(this.NotificationType);
				this._imageDetermined = true;
			}
			if (!this._sizeDetermined && this.NotificationDescriptionText != null)
			{
				this.DetermineSize();
			}
			bool flag = this._ringHoverBegan || this._extensionHoverBegan || this._removeHoverBegan;
			this._isExtended = flag;
			if (this.RemoveButtonVisualWidget != null)
			{
				this.RemoveButtonVisualWidget.IsVisible = this._isExtended && base.EventManager.IsControllerActive;
			}
			this.NotificationRingWidget.IsEnabled = !this._removeInitiated;
			this.NotificationExtensionWidget.IsEnabled = !this._removeInitiated;
			this.RemoveNotificationButtonWidget.IsVisible = flag && !this.IsInspectionForced;
			this.NotificationTextContainerWidget.IsVisible = flag;
			this.RefreshHorizontalPositioning(dt, flag);
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0002B160 File Offset: 0x00029360
		private void DetermineSize()
		{
			if (this.NotificationDescriptionText.Size.Y > base.Size.Y - 45f * base._scaleToUse)
			{
				this.NotificationExtensionWidget.Sprite = this.ExtendedWidthSprite;
				this.NotificationExtensionWidget.SuggestedWidth = this.ExtendedWidth;
			}
			else
			{
				this.NotificationExtensionWidget.Sprite = this.DefaultWidthSprite;
			}
			this._sizeDetermined = true;
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0002B1D4 File Offset: 0x000293D4
		private void RefreshHorizontalPositioning(float dt, bool shouldExtend)
		{
			float num = this.NotificationExtensionWidget.Size.X - this.NotificationRingWidget.Size.X + 20f * base._scaleToUse;
			float num2 = -(this.NotificationExtensionWidget.Size.X - (this.NotificationExtensionWidget.Size.X - this.NotificationRingWidget.Size.X)) + 35f * base._scaleToUse;
			float num3 = (shouldExtend ? num2 : num);
			this.NotificationExtensionWidget.ScaledPositionXOffset = this.LocalLerp(this.NotificationExtensionWidget.ScaledPositionXOffset, num3, dt * 18f);
			float num4 = 0f;
			if (this._removeInitiated)
			{
				num4 = this.NotificationRingWidget.Size.X;
			}
			else if (!base.IsVisible)
			{
				num4 = this.NotificationRingWidget.Size.X;
			}
			base.ScaledPositionXOffset = this.LocalLerp(base.ScaledPositionXOffset, num4, dt * 18f);
			if (this._removeInitiated && MathF.Abs(base.ScaledPositionXOffset - num4) < 0.7f)
			{
				base.EventFired("OnRemove", Array.Empty<object>());
			}
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0002B2FD File Offset: 0x000294FD
		private void OnRemoveClick(Widget button)
		{
			if (!this.IsInspectionForced)
			{
				this._removeInitiated = true;
				base.EventFired("OnRemoveBegin", Array.Empty<object>());
			}
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0002B31E File Offset: 0x0002951E
		private void OnInspectionClick(Widget button)
		{
			base.EventFired("OnInspection", Array.Empty<object>());
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06000F93 RID: 3987 RVA: 0x0002B330 File Offset: 0x00029530
		// (set) Token: 0x06000F94 RID: 3988 RVA: 0x0002B338 File Offset: 0x00029538
		[Editor(false)]
		public bool IsFocusItem
		{
			get
			{
				return this._isFocusItem;
			}
			set
			{
				if (value != this._isFocusItem)
				{
					this._isFocusItem = value;
					base.OnPropertyChanged(value, "IsFocusItem");
				}
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06000F95 RID: 3989 RVA: 0x0002B356 File Offset: 0x00029556
		// (set) Token: 0x06000F96 RID: 3990 RVA: 0x0002B35E File Offset: 0x0002955E
		[Editor(false)]
		public float DefaultWidth
		{
			get
			{
				return this._defaultWidth;
			}
			set
			{
				if (value != this._defaultWidth)
				{
					this._defaultWidth = value;
					base.OnPropertyChanged(value, "DefaultWidth");
				}
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06000F97 RID: 3991 RVA: 0x0002B37C File Offset: 0x0002957C
		// (set) Token: 0x06000F98 RID: 3992 RVA: 0x0002B384 File Offset: 0x00029584
		[Editor(false)]
		public float ExtendedWidth
		{
			get
			{
				return this._extendedWidth;
			}
			set
			{
				if (value != this._extendedWidth)
				{
					this._extendedWidth = value;
					base.OnPropertyChanged(value, "ExtendedWidth");
				}
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06000F99 RID: 3993 RVA: 0x0002B3A2 File Offset: 0x000295A2
		// (set) Token: 0x06000F9A RID: 3994 RVA: 0x0002B3AC File Offset: 0x000295AC
		[Editor(false)]
		public ButtonWidget RemoveNotificationButtonWidget
		{
			get
			{
				return this._removeNotificationButtonWidget;
			}
			set
			{
				if (this._removeNotificationButtonWidget != value)
				{
					this._removeNotificationButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "RemoveNotificationButtonWidget");
					value.ClickEventHandlers.Add(new Action<Widget>(this.OnRemoveClick));
					value.boolPropertyChanged += this.RemoveButtonWidgetPropertyChanged;
				}
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06000F9B RID: 3995 RVA: 0x0002B3FE File Offset: 0x000295FE
		// (set) Token: 0x06000F9C RID: 3996 RVA: 0x0002B406 File Offset: 0x00029606
		[Editor(false)]
		public Widget NotificationRingImageWidget
		{
			get
			{
				return this._notificationRingImageWidget;
			}
			set
			{
				if (this._notificationRingImageWidget != value)
				{
					this._notificationRingImageWidget = value;
					base.OnPropertyChanged<Widget>(value, "NotificationRingImageWidget");
				}
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06000F9D RID: 3997 RVA: 0x0002B424 File Offset: 0x00029624
		// (set) Token: 0x06000F9E RID: 3998 RVA: 0x0002B42C File Offset: 0x0002962C
		[Editor(false)]
		public bool IsInspectionForced
		{
			get
			{
				return this._isInspectionForced;
			}
			set
			{
				if (this._isInspectionForced != value)
				{
					this._isInspectionForced = value;
					base.OnPropertyChanged(value, "IsInspectionForced");
				}
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06000F9F RID: 3999 RVA: 0x0002B44A File Offset: 0x0002964A
		// (set) Token: 0x06000FA0 RID: 4000 RVA: 0x0002B452 File Offset: 0x00029652
		[Editor(false)]
		public string NotificationType
		{
			get
			{
				return this._notificationType;
			}
			set
			{
				if (this._notificationType != value)
				{
					this._notificationType = value;
					base.OnPropertyChanged<string>(value, "NotificationType");
				}
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x0002B475 File Offset: 0x00029675
		// (set) Token: 0x06000FA2 RID: 4002 RVA: 0x0002B47D File Offset: 0x0002967D
		[Editor(false)]
		public Sprite DefaultWidthSprite
		{
			get
			{
				return this._defaultWidthSprite;
			}
			set
			{
				if (this._defaultWidthSprite != value)
				{
					this._defaultWidthSprite = value;
					base.OnPropertyChanged<Sprite>(value, "DefaultWidthSprite");
				}
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x0002B49B File Offset: 0x0002969B
		// (set) Token: 0x06000FA4 RID: 4004 RVA: 0x0002B4A3 File Offset: 0x000296A3
		[Editor(false)]
		public Sprite ExtendedWidthSprite
		{
			get
			{
				return this._extendedWidthSprite;
			}
			set
			{
				if (this._extendedWidthSprite != value)
				{
					this._extendedWidthSprite = value;
					base.OnPropertyChanged<Sprite>(value, "ExtendedWidthSprite");
				}
			}
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0002B4C1 File Offset: 0x000296C1
		private void RemoveButtonWidgetPropertyChanged(PropertyOwnerObject widget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsHovered")
			{
				this._removeHoverBegan = propertyValue;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x0002B4D7 File Offset: 0x000296D7
		// (set) Token: 0x06000FA7 RID: 4007 RVA: 0x0002B4E0 File Offset: 0x000296E0
		[Editor(false)]
		public Widget NotificationRingWidget
		{
			get
			{
				return this._notificationRingWidget;
			}
			set
			{
				if (this._notificationRingWidget != value)
				{
					this._notificationRingWidget = value;
					base.OnPropertyChanged<Widget>(value, "NotificationRingWidget");
					value.boolPropertyChanged += this.RingWidgetOnPropertyChanged;
					value.EventFire += this.InspectionWidgetsEventFire;
				}
			}
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0002B52D File Offset: 0x0002972D
		private void RingWidgetOnPropertyChanged(PropertyOwnerObject widget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsHovered")
			{
				this._ringHoverBegan = propertyValue;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x0002B543 File Offset: 0x00029743
		// (set) Token: 0x06000FAA RID: 4010 RVA: 0x0002B54C File Offset: 0x0002974C
		[Editor(false)]
		public Widget NotificationExtensionWidget
		{
			get
			{
				return this._notificationExtensionWidget;
			}
			set
			{
				if (this._notificationExtensionWidget != value)
				{
					this._notificationExtensionWidget = value;
					base.OnPropertyChanged<Widget>(value, "NotificationExtensionWidget");
					value.boolPropertyChanged += this.ExtensionWidgetOnPropertyChanged;
					value.EventFire += this.InspectionWidgetsEventFire;
				}
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06000FAB RID: 4011 RVA: 0x0002B599 File Offset: 0x00029799
		// (set) Token: 0x06000FAC RID: 4012 RVA: 0x0002B5A1 File Offset: 0x000297A1
		[Editor(false)]
		public Widget NotificationTextContainerWidget
		{
			get
			{
				return this._notificationTextContainerWidget;
			}
			set
			{
				if (this._notificationTextContainerWidget != value)
				{
					this._notificationTextContainerWidget = value;
					base.OnPropertyChanged<Widget>(value, "NotificationTextContainerWidget");
				}
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06000FAD RID: 4013 RVA: 0x0002B5BF File Offset: 0x000297BF
		// (set) Token: 0x06000FAE RID: 4014 RVA: 0x0002B5C7 File Offset: 0x000297C7
		[Editor(false)]
		public RichTextWidget NotificationDescriptionText
		{
			get
			{
				return this._notificationDescriptionText;
			}
			set
			{
				if (this._notificationDescriptionText != value)
				{
					this._notificationDescriptionText = value;
					base.OnPropertyChanged<RichTextWidget>(value, "NotificationDescriptionText");
				}
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x0002B5E5 File Offset: 0x000297E5
		// (set) Token: 0x06000FB0 RID: 4016 RVA: 0x0002B5ED File Offset: 0x000297ED
		[Editor(false)]
		public InputKeyVisualWidget RemoveButtonVisualWidget
		{
			get
			{
				return this._removeButtonVisualWidget;
			}
			set
			{
				if (this._removeButtonVisualWidget != value)
				{
					this._removeButtonVisualWidget = value;
					base.OnPropertyChanged<InputKeyVisualWidget>(value, "RemoveButtonVisualWidget");
				}
			}
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0002B60B File Offset: 0x0002980B
		private void InspectionWidgetsEventFire(Widget widget, string eventName, object[] eventParameters)
		{
			if (eventName == "Click")
			{
				this.OnInspectionClick(widget);
				return;
			}
			if (eventName == "AlternateClick")
			{
				this.OnRemoveClick(this);
			}
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0002B636 File Offset: 0x00029836
		private void ExtensionWidgetOnPropertyChanged(PropertyOwnerObject widget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsHovered")
			{
				this._extensionHoverBegan = propertyValue;
			}
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0002B64C File Offset: 0x0002984C
		private float LocalLerp(float start, float end, float delta)
		{
			if (MathF.Abs(start - end) > 1E-45f)
			{
				return (end - start) * delta + start;
			}
			return end;
		}

		// Token: 0x0400071A RID: 1818
		private bool _ringHoverBegan;

		// Token: 0x0400071B RID: 1819
		private bool _extensionHoverBegan;

		// Token: 0x0400071C RID: 1820
		private bool _removeHoverBegan;

		// Token: 0x0400071D RID: 1821
		private bool _removeInitiated;

		// Token: 0x0400071E RID: 1822
		private bool _imageDetermined;

		// Token: 0x0400071F RID: 1823
		private bool _sizeDetermined;

		// Token: 0x04000720 RID: 1824
		private bool _isExtended;

		// Token: 0x04000721 RID: 1825
		private bool _isFocusItem;

		// Token: 0x04000722 RID: 1826
		private float _defaultWidth;

		// Token: 0x04000723 RID: 1827
		private float _extendedWidth;

		// Token: 0x04000724 RID: 1828
		private bool _isInspectionForced;

		// Token: 0x04000725 RID: 1829
		private string _notificationType = "Default";

		// Token: 0x04000726 RID: 1830
		private Sprite _defaultWidthSprite;

		// Token: 0x04000727 RID: 1831
		private Sprite _extendedWidthSprite;

		// Token: 0x04000728 RID: 1832
		private Widget _notificationRingWidget;

		// Token: 0x04000729 RID: 1833
		private Widget _notificationRingImageWidget;

		// Token: 0x0400072A RID: 1834
		private Widget _notificationExtensionWidget;

		// Token: 0x0400072B RID: 1835
		private Widget _notificationTextContainerWidget;

		// Token: 0x0400072C RID: 1836
		private ButtonWidget _removeNotificationButtonWidget;

		// Token: 0x0400072D RID: 1837
		private RichTextWidget _notificationDescriptionText;

		// Token: 0x0400072E RID: 1838
		private InputKeyVisualWidget _removeButtonVisualWidget;
	}
}
