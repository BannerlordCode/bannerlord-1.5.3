using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000E6 RID: 230
	public class TakenDamageItemBrushWidget : BrushWidget
	{
		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06000BED RID: 3053 RVA: 0x00021396 File Offset: 0x0001F596
		// (set) Token: 0x06000BEE RID: 3054 RVA: 0x0002139E File Offset: 0x0001F59E
		public float VerticalWidth { get; set; }

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06000BEF RID: 3055 RVA: 0x000213A7 File Offset: 0x0001F5A7
		// (set) Token: 0x06000BF0 RID: 3056 RVA: 0x000213AF File Offset: 0x0001F5AF
		public float VerticalHeight { get; set; }

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x000213B8 File Offset: 0x0001F5B8
		// (set) Token: 0x06000BF2 RID: 3058 RVA: 0x000213C0 File Offset: 0x0001F5C0
		public float HorizontalWidth { get; set; }

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x000213C9 File Offset: 0x0001F5C9
		// (set) Token: 0x06000BF4 RID: 3060 RVA: 0x000213D1 File Offset: 0x0001F5D1
		public float HorizontalHeight { get; set; }

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x000213DA File Offset: 0x0001F5DA
		// (set) Token: 0x06000BF6 RID: 3062 RVA: 0x000213E2 File Offset: 0x0001F5E2
		public float RangedOnScreenStayTime { get; set; } = 0.3f;

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x000213EB File Offset: 0x0001F5EB
		// (set) Token: 0x06000BF8 RID: 3064 RVA: 0x000213F3 File Offset: 0x0001F5F3
		public float MeleeOnScreenStayTime { get; set; } = 1f;

		// Token: 0x06000BF9 RID: 3065 RVA: 0x000213FC File Offset: 0x0001F5FC
		public TakenDamageItemBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0002141C File Offset: 0x0001F61C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.RegisterBrushStatesOfWidget();
				this._initialized = true;
				if (!this.IsRanged)
				{
					float num = (float)this.DamageAmount / 70f;
					num = MathF.Clamp(num, 0f, 1f);
					base.AlphaFactor = MathF.Lerp(0.3f, 1f, num, 1E-05f);
				}
			}
			this.UpdateAlpha(dt);
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x00021490 File Offset: 0x0001F690
		private void UpdateAlpha(float dt)
		{
			if (base.AlphaFactor < 0.01f)
			{
				base.EventFired("OnRemove", Array.Empty<object>());
			}
			float num = (this.IsRanged ? this.RangedOnScreenStayTime : this.MeleeOnScreenStayTime);
			this.SetGlobalAlphaRecursively(MathF.Lerp(base.AlphaFactor, 0f, dt / num, 1E-05f));
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x000214EF File Offset: 0x0001F6EF
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			if (base.AlphaFactor > 0f)
			{
				base.OnRender(twoDimensionContext, drawContext);
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06000BFD RID: 3069 RVA: 0x00021506 File Offset: 0x0001F706
		// (set) Token: 0x06000BFE RID: 3070 RVA: 0x0002150E File Offset: 0x0001F70E
		[DataSourceProperty]
		public int DamageAmount
		{
			get
			{
				return this._damageAmount;
			}
			set
			{
				if (this._damageAmount != value)
				{
					this._damageAmount = value;
					base.OnPropertyChanged(value, "DamageAmount");
				}
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06000BFF RID: 3071 RVA: 0x0002152C File Offset: 0x0001F72C
		// (set) Token: 0x06000C00 RID: 3072 RVA: 0x00021534 File Offset: 0x0001F734
		[DataSourceProperty]
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

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06000C01 RID: 3073 RVA: 0x00021552 File Offset: 0x0001F752
		// (set) Token: 0x06000C02 RID: 3074 RVA: 0x0002155A File Offset: 0x0001F75A
		[DataSourceProperty]
		public bool IsRanged
		{
			get
			{
				return this._isRanged;
			}
			set
			{
				if (this._isRanged != value)
				{
					this._isRanged = value;
					base.OnPropertyChanged(value, "IsRanged");
				}
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06000C03 RID: 3075 RVA: 0x00021578 File Offset: 0x0001F778
		// (set) Token: 0x06000C04 RID: 3076 RVA: 0x00021580 File Offset: 0x0001F780
		[DataSourceProperty]
		public Vec2 ScreenPosOfAffectorAgent
		{
			get
			{
				return this._screenPosOfAffectorAgent;
			}
			set
			{
				if (this._screenPosOfAffectorAgent != value)
				{
					this._screenPosOfAffectorAgent = value;
					base.OnPropertyChanged(value, "ScreenPosOfAffectorAgent");
				}
			}
		}

		// Token: 0x04000568 RID: 1384
		private bool _initialized;

		// Token: 0x0400056F RID: 1391
		private int _damageAmount;

		// Token: 0x04000570 RID: 1392
		private Vec2 _screenPosOfAffectorAgent;

		// Token: 0x04000571 RID: 1393
		private bool _isBehind;

		// Token: 0x04000572 RID: 1394
		private bool _isRanged;
	}
}
