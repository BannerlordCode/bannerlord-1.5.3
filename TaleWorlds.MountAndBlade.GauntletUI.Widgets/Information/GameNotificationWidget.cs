using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Information
{
	// Token: 0x02000149 RID: 329
	public class GameNotificationWidget : BrushWidget
	{
		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001170 RID: 4464 RVA: 0x00030336 File Offset: 0x0002E536
		// (set) Token: 0x06001171 RID: 4465 RVA: 0x0003033E File Offset: 0x0002E53E
		public float RampUpInSeconds { get; set; } = 0.2f;

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001172 RID: 4466 RVA: 0x00030347 File Offset: 0x0002E547
		// (set) Token: 0x06001173 RID: 4467 RVA: 0x0003034F File Offset: 0x0002E54F
		public float RampDownInSeconds { get; set; } = 0.2f;

		// Token: 0x06001174 RID: 4468 RVA: 0x00030358 File Offset: 0x0002E558
		public GameNotificationWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00030380 File Offset: 0x0002E580
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsVisible && this._textWidgetAlignmentDirty)
			{
				ImageIdentifierWidget announcerImageIdentifier = this.AnnouncerImageIdentifier;
				if (announcerImageIdentifier != null && announcerImageIdentifier.IsVisible)
				{
					this.TextWidget.Brush.TextHorizontalAlignment = TextHorizontalAlignment.Left;
				}
				else
				{
					this.TextWidget.Brush.TextHorizontalAlignment = TextHorizontalAlignment.Center;
				}
				this._textWidgetAlignmentDirty = false;
			}
			if (base.IsVisible && !this.IsPaused)
			{
				this._notificationElapsedTimeInSeconds += dt;
				if (this.MustFadeOutCurrentNotification)
				{
					this._notificationElapsedTimeInSeconds = this.RampUpInSeconds + this.NotificationDurationInSeconds - this.NotificationFadeOutDelayInSeconds;
					this.MustFadeOutCurrentNotification = false;
					this.NotificationFadeOutDelayInSeconds = 0f;
				}
				if (this._notificationElapsedTimeInSeconds <= this.RampUpInSeconds)
				{
					float num = Mathf.Lerp(0f, 1f, this._notificationElapsedTimeInSeconds / this.RampUpInSeconds);
					this.SetGlobalAlphaRecursively(num);
					return;
				}
				if (this._notificationElapsedTimeInSeconds <= this.RampUpInSeconds + this.NotificationDurationInSeconds)
				{
					this.SetGlobalAlphaRecursively(1f);
					return;
				}
				if (this._notificationElapsedTimeInSeconds < this.RampUpInSeconds + this.NotificationDurationInSeconds + this.RampDownInSeconds)
				{
					float num2 = Mathf.Lerp(1f, 0f, (this._notificationElapsedTimeInSeconds - this.RampUpInSeconds - this.NotificationDurationInSeconds) / this.RampDownInSeconds);
					this.SetGlobalAlphaRecursively(num2);
					return;
				}
				this.SetGlobalAlphaRecursively(0f);
				base.EventFired("NotificationFinished", Array.Empty<object>());
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06001176 RID: 4470 RVA: 0x000304F9 File Offset: 0x0002E6F9
		// (set) Token: 0x06001177 RID: 4471 RVA: 0x00030501 File Offset: 0x0002E701
		public ImageIdentifierWidget AnnouncerImageIdentifier
		{
			get
			{
				return this._announcerImageIdentifier;
			}
			set
			{
				if (this._announcerImageIdentifier != value)
				{
					this._announcerImageIdentifier = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "AnnouncerImageIdentifier");
				}
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001178 RID: 4472 RVA: 0x0003051F File Offset: 0x0002E71F
		// (set) Token: 0x06001179 RID: 4473 RVA: 0x00030527 File Offset: 0x0002E727
		public int NotificationId
		{
			get
			{
				return this._notificationId;
			}
			set
			{
				if (this._notificationId != value)
				{
					this._notificationId = value;
					base.OnPropertyChanged(value, "NotificationId");
					this._textWidgetAlignmentDirty = true;
					this._notificationElapsedTimeInSeconds = 0f;
				}
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x0600117A RID: 4474 RVA: 0x00030557 File Offset: 0x0002E757
		// (set) Token: 0x0600117B RID: 4475 RVA: 0x0003055F File Offset: 0x0002E75F
		public float NotificationDurationInSeconds
		{
			get
			{
				return this._notificationDurationInSeconds;
			}
			set
			{
				if (this._notificationDurationInSeconds != value)
				{
					this._notificationDurationInSeconds = value;
				}
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x0600117C RID: 4476 RVA: 0x00030571 File Offset: 0x0002E771
		// (set) Token: 0x0600117D RID: 4477 RVA: 0x00030579 File Offset: 0x0002E779
		public RichTextWidget TextWidget
		{
			get
			{
				return this._textWidget;
			}
			set
			{
				if (this._textWidget != value)
				{
					this._textWidget = value;
					base.OnPropertyChanged<RichTextWidget>(value, "TextWidget");
				}
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x0600117E RID: 4478 RVA: 0x00030597 File Offset: 0x0002E797
		// (set) Token: 0x0600117F RID: 4479 RVA: 0x0003059F File Offset: 0x0002E79F
		public bool IsPaused
		{
			get
			{
				return this._isPaused;
			}
			set
			{
				if (this._isPaused != value)
				{
					this._isPaused = value;
					base.OnPropertyChanged(value, "IsPaused");
				}
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x000305BD File Offset: 0x0002E7BD
		// (set) Token: 0x06001181 RID: 4481 RVA: 0x000305C5 File Offset: 0x0002E7C5
		public bool MustFadeOutCurrentNotification
		{
			get
			{
				return this._mustFadeOutCurrentNotification;
			}
			set
			{
				if (this._mustFadeOutCurrentNotification != value)
				{
					this._mustFadeOutCurrentNotification = value;
					base.OnPropertyChanged(value, "MustFadeOutCurrentNotification");
				}
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001182 RID: 4482 RVA: 0x000305E3 File Offset: 0x0002E7E3
		// (set) Token: 0x06001183 RID: 4483 RVA: 0x000305EB File Offset: 0x0002E7EB
		public float NotificationFadeOutDelayInSeconds
		{
			get
			{
				return this._notificationFadeOutDelayInSeconds;
			}
			set
			{
				if (this._notificationFadeOutDelayInSeconds != value)
				{
					this._notificationFadeOutDelayInSeconds = value;
					base.OnPropertyChanged(value, "NotificationFadeOutDelayInSeconds");
				}
			}
		}

		// Token: 0x040007F0 RID: 2032
		private bool _textWidgetAlignmentDirty = true;

		// Token: 0x040007F1 RID: 2033
		private float _notificationElapsedTimeInSeconds;

		// Token: 0x040007F2 RID: 2034
		private int _notificationId;

		// Token: 0x040007F3 RID: 2035
		private RichTextWidget _textWidget;

		// Token: 0x040007F4 RID: 2036
		private ImageIdentifierWidget _announcerImageIdentifier;

		// Token: 0x040007F5 RID: 2037
		private float _notificationDurationInSeconds;

		// Token: 0x040007F6 RID: 2038
		private bool _isPaused;

		// Token: 0x040007F7 RID: 2039
		private bool _mustFadeOutCurrentNotification;

		// Token: 0x040007F8 RID: 2040
		private float _notificationFadeOutDelayInSeconds;
	}
}
