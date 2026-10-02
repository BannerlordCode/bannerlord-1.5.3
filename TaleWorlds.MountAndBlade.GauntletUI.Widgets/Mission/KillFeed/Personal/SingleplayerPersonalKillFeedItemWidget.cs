using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.KillFeed.Personal
{
	// Token: 0x020000FD RID: 253
	public class SingleplayerPersonalKillFeedItemWidget : Widget
	{
		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x00024F10 File Offset: 0x00023110
		// (set) Token: 0x06000D71 RID: 3441 RVA: 0x00024F18 File Offset: 0x00023118
		public Widget NotificationTypeIconWidget { get; set; }

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x00024F21 File Offset: 0x00023121
		// (set) Token: 0x06000D73 RID: 3443 RVA: 0x00024F29 File Offset: 0x00023129
		public Widget NotificationBackgroundWidget { get; set; }

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x00024F32 File Offset: 0x00023132
		// (set) Token: 0x06000D75 RID: 3445 RVA: 0x00024F3A File Offset: 0x0002313A
		public TextWidget AmountTextWidget { get; set; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x00024F43 File Offset: 0x00023143
		// (set) Token: 0x06000D77 RID: 3447 RVA: 0x00024F4B File Offset: 0x0002314B
		public RichTextWidget MessageTextWidget { get; set; }

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x00024F54 File Offset: 0x00023154
		// (set) Token: 0x06000D79 RID: 3449 RVA: 0x00024F5C File Offset: 0x0002315C
		public float FadeInTime { get; set; } = 0.2f;

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06000D7A RID: 3450 RVA: 0x00024F65 File Offset: 0x00023165
		// (set) Token: 0x06000D7B RID: 3451 RVA: 0x00024F6D File Offset: 0x0002316D
		public float StayTime { get; set; } = 2f;

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x00024F76 File Offset: 0x00023176
		// (set) Token: 0x06000D7D RID: 3453 RVA: 0x00024F7E File Offset: 0x0002317E
		public float FadeOutTime { get; set; } = 0.2f;

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x00024F87 File Offset: 0x00023187
		// (set) Token: 0x06000D7F RID: 3455 RVA: 0x00024F8F File Offset: 0x0002318F
		public float TimeSinceCreation { get; private set; }

		// Token: 0x06000D80 RID: 3456 RVA: 0x00024F98 File Offset: 0x00023198
		public SingleplayerPersonalKillFeedItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00024FC4 File Offset: 0x000231C4
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				this.SetGlobalAlphaRecursively(0f);
				this.UpdateNotificationBackgroundWidget();
				this.UpdateNotificationTypeIconWidget();
				this.UpdateNotificationMessageWidget();
				this.UpdateNotificationAmountWidget();
				this.UpdateTroopTypeVisualWidget();
				this._initialized = true;
			}
			this.UpdateAlphaValues(dt);
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00025018 File Offset: 0x00023218
		private void UpdateAlphaValues(float dt)
		{
			if (!this.IsPaused)
			{
				this.TimeSinceCreation += dt * this._speedModifier;
			}
			if (this.TimeSinceCreation <= this.FadeInTime)
			{
				this.SetGlobalAlphaRecursively(Mathf.Lerp(base.AlphaFactor, 1f, this.TimeSinceCreation / this.FadeInTime));
				return;
			}
			if (this.TimeSinceCreation - this.FadeInTime <= this.StayTime)
			{
				this.SetGlobalAlphaRecursively(1f);
				return;
			}
			if (this.TimeSinceCreation - (this.FadeInTime + this.StayTime) <= this.FadeOutTime)
			{
				this.SetGlobalAlphaRecursively(Mathf.Lerp(base.AlphaFactor, 0f, (this.TimeSinceCreation - (this.FadeInTime + this.StayTime)) / this.FadeOutTime));
				if (base.AlphaFactor <= 0.1f)
				{
					base.EventFired("OnRemove", Array.Empty<object>());
					return;
				}
			}
			else
			{
				base.EventFired("OnRemove", Array.Empty<object>());
			}
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x00025110 File Offset: 0x00023310
		public void SetSpeedModifier(float newSpeed)
		{
			if (newSpeed > this._speedModifier)
			{
				this._speedModifier = newSpeed;
			}
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x00025124 File Offset: 0x00023324
		private void UpdateNotificationTypeIconWidget()
		{
			if (this.ItemType == 0 || this.ItemType == 9)
			{
				this.NotificationTypeIconWidget.IsVisible = false;
				return;
			}
			switch (this.ItemType)
			{
			case 1:
				this.NotificationTypeIconWidget.SetState("FriendlyFireDamage");
				return;
			case 2:
				this.NotificationTypeIconWidget.SetState("FriendlyFireKill");
				return;
			case 3:
				this.NotificationTypeIconWidget.SetState("MountDamage");
				return;
			case 4:
				this.NotificationTypeIconWidget.SetState("NormalKill");
				return;
			case 5:
				this.NotificationTypeIconWidget.SetState("Assist");
				return;
			case 6:
				this.NotificationTypeIconWidget.SetState("MakeUnconscious");
				return;
			case 7:
				this.NotificationTypeIconWidget.SetState("NormalKillHeadshot");
				return;
			case 8:
				this.NotificationTypeIconWidget.SetState("MakeUnconsciousHeadshot");
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Mission\\KillFeed\\Personal\\SingleplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationTypeIconWidget", 128);
				this.NotificationTypeIconWidget.IsVisible = false;
				return;
			}
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00025234 File Offset: 0x00023434
		private void UpdateNotificationAmountWidget()
		{
			if (this.ItemType != 6 && this.Amount == -1)
			{
				this.AmountTextWidget.IsVisible = false;
				return;
			}
			switch (this.ItemType)
			{
			case 0:
			case 3:
			case 4:
			case 6:
			case 7:
			case 8:
				this.AmountTextWidget.SetState("Normal");
				this.AmountTextWidget.IntText = this.Amount;
				return;
			case 1:
			case 2:
				this.AmountTextWidget.SetState("FriendlyFire");
				this.AmountTextWidget.IntText = this.Amount;
				return;
			case 5:
			case 9:
				this.AmountTextWidget.IsVisible = false;
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Mission\\KillFeed\\Personal\\SingleplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationAmountWidget", 166);
				this.AmountTextWidget.IsVisible = false;
				return;
			}
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00025310 File Offset: 0x00023510
		private void UpdateNotificationMessageWidget()
		{
			this.MessageTextWidget.Text = this.Message;
			if (string.IsNullOrEmpty(this.Message))
			{
				this.MessageTextWidget.IsVisible = false;
				return;
			}
			switch (this.ItemType)
			{
			case 0:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				this.MessageTextWidget.SetState("Normal");
				return;
			case 1:
			case 2:
				this.MessageTextWidget.SetState("FriendlyFire");
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Mission\\KillFeed\\Personal\\SingleplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationMessageWidget", 200);
				this.MessageTextWidget.IsVisible = false;
				return;
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x000253C8 File Offset: 0x000235C8
		private void UpdateNotificationBackgroundWidget()
		{
			switch (this.ItemType)
			{
			case 0:
			case 1:
			case 3:
				this.NotificationBackgroundWidget.SetState("Hidden");
				return;
			case 2:
				this.NotificationBackgroundWidget.SetState("FriendlyFire");
				return;
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				this.NotificationBackgroundWidget.SetState("Normal");
				return;
			case 9:
				this.NotificationBackgroundWidget.SetState("Message");
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Mission\\KillFeed\\Personal\\SingleplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationBackgroundWidget", 230);
				this.NotificationBackgroundWidget.SetState("Hidden");
				return;
			}
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0002547C File Offset: 0x0002367C
		private void UpdateTroopTypeVisualWidget()
		{
			if (this.TroopTypeWidget != null)
			{
				if (string.IsNullOrEmpty(this.TypeID))
				{
					this.TroopTypeWidget.IsVisible = false;
					return;
				}
				Widget troopTypeWidget = this.TroopTypeWidget;
				Brush troopTypeIconBrush = this.TroopTypeIconBrush;
				Sprite sprite;
				if (troopTypeIconBrush == null)
				{
					sprite = null;
				}
				else
				{
					BrushLayer layer = troopTypeIconBrush.GetLayer(this._typeID);
					sprite = ((layer != null) ? layer.Sprite : null);
				}
				troopTypeWidget.Sprite = sprite;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x000254DA File Offset: 0x000236DA
		// (set) Token: 0x06000D8A RID: 3466 RVA: 0x000254E2 File Offset: 0x000236E2
		public bool IsDamage
		{
			get
			{
				return this._isDamage;
			}
			set
			{
				if (value != this._isDamage)
				{
					this._isDamage = value;
					base.OnPropertyChanged(value, "IsDamage");
				}
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00025500 File Offset: 0x00023700
		// (set) Token: 0x06000D8C RID: 3468 RVA: 0x00025508 File Offset: 0x00023708
		public int Amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				if (value != this._amount)
				{
					this._amount = value;
					base.OnPropertyChanged(value, "Amount");
				}
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x00025526 File Offset: 0x00023726
		// (set) Token: 0x06000D8E RID: 3470 RVA: 0x0002552E File Offset: 0x0002372E
		public int ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (value != this._itemType)
				{
					this._itemType = value;
					base.OnPropertyChanged(value, "ItemType");
				}
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x0002554C File Offset: 0x0002374C
		// (set) Token: 0x06000D90 RID: 3472 RVA: 0x00025554 File Offset: 0x00023754
		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				if (value != this._message)
				{
					this._message = value;
					base.OnPropertyChanged<string>(value, "Message");
				}
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x00025577 File Offset: 0x00023777
		// (set) Token: 0x06000D92 RID: 3474 RVA: 0x0002557F File Offset: 0x0002377F
		public string TypeID
		{
			get
			{
				return this._typeID;
			}
			set
			{
				if (value != this._typeID)
				{
					this._typeID = value;
					base.OnPropertyChanged<string>(value, "TypeID");
				}
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x000255A2 File Offset: 0x000237A2
		// (set) Token: 0x06000D94 RID: 3476 RVA: 0x000255AA File Offset: 0x000237AA
		public Brush TroopTypeIconBrush
		{
			get
			{
				return this._troopTypeIconBrush;
			}
			set
			{
				if (value != this._troopTypeIconBrush)
				{
					this._troopTypeIconBrush = value;
				}
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06000D95 RID: 3477 RVA: 0x000255BC File Offset: 0x000237BC
		// (set) Token: 0x06000D96 RID: 3478 RVA: 0x000255C4 File Offset: 0x000237C4
		public Widget TroopTypeWidget
		{
			get
			{
				return this._troopTypeWidget;
			}
			set
			{
				if (value != this._troopTypeWidget)
				{
					this._troopTypeWidget = value;
					if (!string.IsNullOrEmpty(this._typeID))
					{
						Widget troopTypeWidget = this._troopTypeWidget;
						Brush troopTypeIconBrush = this.TroopTypeIconBrush;
						Sprite sprite;
						if (troopTypeIconBrush == null)
						{
							sprite = null;
						}
						else
						{
							BrushLayer layer = troopTypeIconBrush.GetLayer(this._typeID);
							sprite = ((layer != null) ? layer.Sprite : null);
						}
						troopTypeWidget.Sprite = sprite;
					}
				}
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x0002561D File Offset: 0x0002381D
		// (set) Token: 0x06000D98 RID: 3480 RVA: 0x00025625 File Offset: 0x00023825
		public bool IsPaused
		{
			get
			{
				return this._isPaused;
			}
			set
			{
				if (value != this._isPaused)
				{
					this._isPaused = value;
					base.OnPropertyChanged(value, "IsPaused");
				}
			}
		}

		// Token: 0x04000621 RID: 1569
		private bool _initialized;

		// Token: 0x04000622 RID: 1570
		private float _speedModifier;

		// Token: 0x04000623 RID: 1571
		private bool _isDamage;

		// Token: 0x04000624 RID: 1572
		private int _amount;

		// Token: 0x04000625 RID: 1573
		private int _itemType;

		// Token: 0x04000626 RID: 1574
		private string _message;

		// Token: 0x04000627 RID: 1575
		private string _typeID;

		// Token: 0x04000628 RID: 1576
		private Brush _troopTypeIconBrush;

		// Token: 0x04000629 RID: 1577
		private Widget _troopTypeWidget;

		// Token: 0x0400062A RID: 1578
		private bool _isPaused;
	}
}
