using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.HUD
{
	// Token: 0x020000C7 RID: 199
	public class MultiplayerDeathCardWidget : Widget
	{
		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x0001DB07 File Offset: 0x0001BD07
		// (set) Token: 0x06000A82 RID: 2690 RVA: 0x0001DB0F File Offset: 0x0001BD0F
		public TextWidget WeaponTextWidget { get; set; }

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x0001DB18 File Offset: 0x0001BD18
		// (set) Token: 0x06000A84 RID: 2692 RVA: 0x0001DB20 File Offset: 0x0001BD20
		public TextWidget TitleTextWidget { get; set; }

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x0001DB29 File Offset: 0x0001BD29
		// (set) Token: 0x06000A86 RID: 2694 RVA: 0x0001DB31 File Offset: 0x0001BD31
		public ScrollingRichTextWidget KillerNameTextWidget { get; set; }

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x0001DB3A File Offset: 0x0001BD3A
		// (set) Token: 0x06000A88 RID: 2696 RVA: 0x0001DB42 File Offset: 0x0001BD42
		public Widget KillCountContainer { get; set; }

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x0001DB4B File Offset: 0x0001BD4B
		// (set) Token: 0x06000A8A RID: 2698 RVA: 0x0001DB53 File Offset: 0x0001BD53
		public Brush SelfInflictedTitleBrush { get; set; }

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x0001DB5C File Offset: 0x0001BD5C
		// (set) Token: 0x06000A8C RID: 2700 RVA: 0x0001DB64 File Offset: 0x0001BD64
		public Brush NormalBrushTitleBrush { get; set; }

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x0001DB6D File Offset: 0x0001BD6D
		// (set) Token: 0x06000A8E RID: 2702 RVA: 0x0001DB75 File Offset: 0x0001BD75
		public float FadeInModifier { get; set; } = 2f;

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x0001DB7E File Offset: 0x0001BD7E
		// (set) Token: 0x06000A90 RID: 2704 RVA: 0x0001DB86 File Offset: 0x0001BD86
		public float FadeOutModifier { get; set; } = 10f;

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x0001DB8F File Offset: 0x0001BD8F
		// (set) Token: 0x06000A92 RID: 2706 RVA: 0x0001DB97 File Offset: 0x0001BD97
		public float StayTime { get; set; } = 7f;

		// Token: 0x06000A93 RID: 2707 RVA: 0x0001DBA0 File Offset: 0x0001BDA0
		public MultiplayerDeathCardWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0001DBCC File Offset: 0x0001BDCC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this._initialized = true;
				base.IsEnabled = false;
				this._initAlpha = base.AlphaFactor;
				this.SetGlobalAlphaRecursively(this._targetAlpha);
			}
			if (Math.Abs(base.AlphaFactor - this._targetAlpha) > 1E-45f)
			{
				float num = ((base.AlphaFactor > this._targetAlpha) ? this.FadeOutModifier : this.FadeInModifier);
				float num2 = Mathf.Lerp(base.AlphaFactor, this._targetAlpha, dt * num);
				this.SetGlobalAlphaRecursively(num2);
			}
			if ((this.IsActive && base.AlphaFactor < 1E-45f) || base.Context.EventManager.Time - this._activeTimeStart > this.StayTime)
			{
				this.IsActive = false;
			}
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0001DC9C File Offset: 0x0001BE9C
		private void HandleIsActiveToggle(bool isActive)
		{
			this._targetAlpha = (isActive ? 1f : 0f);
			if (isActive)
			{
				this._activeTimeStart = base.Context.EventManager.Time;
			}
			this.KillCountContainer.IsVisible = !this.IsSelfInflicted && this.KillCountsEnabled;
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0001DCF4 File Offset: 0x0001BEF4
		private void HandleSelfInflictedToggle(bool isSelfInflicted)
		{
			this.TitleTextWidget.IsVisible = true;
			this.TitleTextWidget.Brush = (isSelfInflicted ? this.SelfInflictedTitleBrush : this.NormalBrushTitleBrush);
			this.KillerNameTextWidget.IsVisible = !isSelfInflicted;
			this.WeaponTextWidget.IsVisible = !isSelfInflicted;
			this.KillCountContainer.IsVisible = !this.IsSelfInflicted && this.KillCountsEnabled;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0001DD63 File Offset: 0x0001BF63
		private void HandleKillCountsEnabledSwitch(bool killCountsEnabled)
		{
			this.KillCountContainer.IsVisible = killCountsEnabled;
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x0001DD71 File Offset: 0x0001BF71
		// (set) Token: 0x06000A99 RID: 2713 RVA: 0x0001DD79 File Offset: 0x0001BF79
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChanged(value, "IsActive");
					this.HandleIsActiveToggle(value);
				}
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x0001DD9E File Offset: 0x0001BF9E
		// (set) Token: 0x06000A9B RID: 2715 RVA: 0x0001DDA6 File Offset: 0x0001BFA6
		public bool IsSelfInflicted
		{
			get
			{
				return this._isSelfInflicted;
			}
			set
			{
				if (value != this._isSelfInflicted)
				{
					this._isSelfInflicted = value;
					base.OnPropertyChanged(value, "IsSelfInflicted");
					this.HandleSelfInflictedToggle(value);
				}
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000A9C RID: 2716 RVA: 0x0001DDCB File Offset: 0x0001BFCB
		// (set) Token: 0x06000A9D RID: 2717 RVA: 0x0001DDD3 File Offset: 0x0001BFD3
		public bool KillCountsEnabled
		{
			get
			{
				return this._killCountsEnabled;
			}
			set
			{
				if (value != this._killCountsEnabled)
				{
					this._killCountsEnabled = value;
					base.OnPropertyChanged(value, "KillCountsEnabled");
					this.HandleKillCountsEnabledSwitch(value);
				}
			}
		}

		// Token: 0x040004CF RID: 1231
		private float _targetAlpha;

		// Token: 0x040004D0 RID: 1232
		private float _initAlpha;

		// Token: 0x040004D4 RID: 1236
		private float _activeTimeStart;

		// Token: 0x040004D5 RID: 1237
		private bool _initialized;

		// Token: 0x040004D6 RID: 1238
		private bool _isActive;

		// Token: 0x040004D7 RID: 1239
		private bool _isSelfInflicted;

		// Token: 0x040004D8 RID: 1240
		private bool _killCountsEnabled;
	}
}
