using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.KillFeed
{
	// Token: 0x020000C1 RID: 193
	public class MultiplayerPersonalKillFeedItemWidget : Widget
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x0001C728 File Offset: 0x0001A928
		// (set) Token: 0x06000A1F RID: 2591 RVA: 0x0001C730 File Offset: 0x0001A930
		public Widget NotificationTypeIconWidget { get; set; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x0001C739 File Offset: 0x0001A939
		// (set) Token: 0x06000A21 RID: 2593 RVA: 0x0001C741 File Offset: 0x0001A941
		public Widget NotificationBackgroundWidget { get; set; }

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x0001C74A File Offset: 0x0001A94A
		// (set) Token: 0x06000A23 RID: 2595 RVA: 0x0001C752 File Offset: 0x0001A952
		public TextWidget AmountTextWidget { get; set; }

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x0001C75B File Offset: 0x0001A95B
		// (set) Token: 0x06000A25 RID: 2597 RVA: 0x0001C763 File Offset: 0x0001A963
		public RichTextWidget MessageTextWidget { get; set; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x0001C76C File Offset: 0x0001A96C
		// (set) Token: 0x06000A27 RID: 2599 RVA: 0x0001C774 File Offset: 0x0001A974
		public float FadeInTime { get; set; } = 0.2f;

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x0001C77D File Offset: 0x0001A97D
		// (set) Token: 0x06000A29 RID: 2601 RVA: 0x0001C785 File Offset: 0x0001A985
		public float StayTime { get; set; } = 2f;

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000A2A RID: 2602 RVA: 0x0001C78E File Offset: 0x0001A98E
		// (set) Token: 0x06000A2B RID: 2603 RVA: 0x0001C796 File Offset: 0x0001A996
		public float FadeOutTime { get; set; } = 0.2f;

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x0001C79F File Offset: 0x0001A99F
		// (set) Token: 0x06000A2D RID: 2605 RVA: 0x0001C7A7 File Offset: 0x0001A9A7
		public float TimeSinceCreation { get; private set; }

		// Token: 0x06000A2E RID: 2606 RVA: 0x0001C7B0 File Offset: 0x0001A9B0
		public MultiplayerPersonalKillFeedItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0001C7EC File Offset: 0x0001A9EC
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
				this.DetermineSoundEvent();
				this._initialized = true;
			}
			this.UpdateAlphaValues(dt);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0001C83F File Offset: 0x0001AA3F
		private void DetermineSoundEvent()
		{
			if (this.ItemType == 6)
			{
				base.Context.TwoDimensionContext.PlaySound(this._goldGainedSound);
			}
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0001C860 File Offset: 0x0001AA60
		private void UpdateAlphaValues(float dt)
		{
			this.TimeSinceCreation += dt * this._speedModifier;
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

		// Token: 0x06000A32 RID: 2610 RVA: 0x0001C950 File Offset: 0x0001AB50
		public void SetSpeedModifier(float newSpeed)
		{
			if (newSpeed > this._speedModifier)
			{
				this._speedModifier = newSpeed;
			}
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0001C964 File Offset: 0x0001AB64
		private void UpdateNotificationTypeIconWidget()
		{
			if (this.ItemType == 0)
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
				this.NotificationTypeIconWidget.SetState("GoldChange");
				return;
			case 7:
				this.NotificationTypeIconWidget.SetState("NormalKillHeadshot");
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Multiplayer\\KillFeed\\MultiplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationTypeIconWidget", 122);
				this.NotificationTypeIconWidget.IsVisible = false;
				return;
			}
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0001CA4C File Offset: 0x0001AC4C
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
			case 7:
				this.MessageTextWidget.SetState("Normal");
				return;
			case 1:
			case 2:
				this.MessageTextWidget.SetState("FriendlyFire");
				return;
			case 6:
				if (this.Amount >= 0)
				{
					this.MessageTextWidget.SetState("GoldChangePositive");
					return;
				}
				this.MessageTextWidget.SetState("GoldChangeNegative");
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Multiplayer\\KillFeed\\MultiplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationMessageWidget", 163);
				this.MessageTextWidget.IsVisible = false;
				return;
			}
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0001CB28 File Offset: 0x0001AD28
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
			case 7:
				this.AmountTextWidget.SetState("Normal");
				this.AmountTextWidget.IntText = this.Amount;
				return;
			case 1:
			case 2:
				this.AmountTextWidget.SetState("FriendlyFire");
				this.AmountTextWidget.IntText = this.Amount;
				return;
			case 5:
				this.AmountTextWidget.IsVisible = false;
				return;
			case 6:
				if (this.Amount >= 0)
				{
					this.AmountTextWidget.SetState("GoldChangePositive");
					this.AmountTextWidget.Text = "+" + this.Amount.ToString();
					return;
				}
				this.AmountTextWidget.SetState("GoldChangeNegative");
				this.AmountTextWidget.Text = this.Amount.ToString();
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Multiplayer\\KillFeed\\MultiplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationAmountWidget", 209);
				this.AmountTextWidget.IsVisible = false;
				return;
			}
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0001CC64 File Offset: 0x0001AE64
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
			case 7:
				this.NotificationBackgroundWidget.SetState("Normal");
				return;
			case 5:
				break;
			case 6:
				if (this.Amount >= 0)
				{
					this.NotificationBackgroundWidget.SetState("GoldChangePositive");
					return;
				}
				this.NotificationBackgroundWidget.SetState("GoldChangeNegative");
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Multiplayer\\KillFeed\\MultiplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationBackgroundWidget", 245);
				this.NotificationBackgroundWidget.SetState("Hidden");
				break;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0001CD27 File Offset: 0x0001AF27
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x0001CD2F File Offset: 0x0001AF2F
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

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0001CD4D File Offset: 0x0001AF4D
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x0001CD55 File Offset: 0x0001AF55
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

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x0001CD78 File Offset: 0x0001AF78
		// (set) Token: 0x06000A3C RID: 2620 RVA: 0x0001CD80 File Offset: 0x0001AF80
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

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0001CD9E File Offset: 0x0001AF9E
		// (set) Token: 0x06000A3E RID: 2622 RVA: 0x0001CDA6 File Offset: 0x0001AFA6
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

		// Token: 0x0400049B RID: 1179
		private bool _initialized;

		// Token: 0x0400049C RID: 1180
		private float _speedModifier;

		// Token: 0x0400049D RID: 1181
		private readonly string _goldGainedSound = "multiplayer/coin_add";

		// Token: 0x0400049E RID: 1182
		private bool _isDamage;

		// Token: 0x0400049F RID: 1183
		private int _itemType;

		// Token: 0x040004A0 RID: 1184
		private int _amount = -1;

		// Token: 0x040004A1 RID: 1185
		private string _message;
	}
}
