using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.KillFeed.General
{
	// Token: 0x020000FF RID: 255
	public class SingleplayerGeneralKillFeedItemWidget : Widget
	{
		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x00025748 File Offset: 0x00023948
		// (set) Token: 0x06000D9F RID: 3487 RVA: 0x00025750 File Offset: 0x00023950
		public Brush TroopTypeIconBrush { get; set; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x00025759 File Offset: 0x00023959
		// (set) Token: 0x06000DA1 RID: 3489 RVA: 0x00025761 File Offset: 0x00023961
		public Widget MurdererTypeWidget { get; set; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x0002576A File Offset: 0x0002396A
		// (set) Token: 0x06000DA3 RID: 3491 RVA: 0x00025772 File Offset: 0x00023972
		public Widget VictimTypeWidget { get; set; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x0002577B File Offset: 0x0002397B
		// (set) Token: 0x06000DA5 RID: 3493 RVA: 0x00025783 File Offset: 0x00023983
		public Widget ActionIconWidget { get; set; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x0002578C File Offset: 0x0002398C
		// (set) Token: 0x06000DA7 RID: 3495 RVA: 0x00025794 File Offset: 0x00023994
		public TextWidget VictimNameWidget { get; set; }

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x0002579D File Offset: 0x0002399D
		// (set) Token: 0x06000DA9 RID: 3497 RVA: 0x000257A5 File Offset: 0x000239A5
		public TextWidget MurdererNameWidget { get; set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x000257AE File Offset: 0x000239AE
		// (set) Token: 0x06000DAB RID: 3499 RVA: 0x000257B6 File Offset: 0x000239B6
		public float FadeInTime { get; set; } = 0.7f;

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x000257BF File Offset: 0x000239BF
		// (set) Token: 0x06000DAD RID: 3501 RVA: 0x000257C7 File Offset: 0x000239C7
		public float StayTime { get; set; } = 3f;

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x000257D0 File Offset: 0x000239D0
		// (set) Token: 0x06000DAF RID: 3503 RVA: 0x000257D8 File Offset: 0x000239D8
		public float FadeOutTime { get; set; } = 0.7f;

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x000257E1 File Offset: 0x000239E1
		// (set) Token: 0x06000DB1 RID: 3505 RVA: 0x000257E9 File Offset: 0x000239E9
		public float TimeSinceCreation { get; private set; }

		// Token: 0x06000DB2 RID: 3506 RVA: 0x000257F2 File Offset: 0x000239F2
		public SingleplayerGeneralKillFeedItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00025828 File Offset: 0x00023A28
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				this.Initialize();
				this._initialized = true;
			}
			if (!this.IsPaused)
			{
				this.TimeSinceCreation += dt * this._speedModifier;
			}
			if (this.TimeSinceCreation <= this.FadeInTime)
			{
				this.SetGlobalAlphaRecursively(Mathf.Lerp(base.AlphaFactor, 0.5f, this.TimeSinceCreation / this.FadeInTime));
				return;
			}
			if (this.TimeSinceCreation - this.FadeInTime <= this.StayTime)
			{
				this.SetGlobalAlphaRecursively(0.5f);
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

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0002593C File Offset: 0x00023B3C
		private void Initialize()
		{
			this.SetGlobalAlphaRecursively(0f);
			Widget murdererTypeWidget = this.MurdererTypeWidget;
			Brush troopTypeIconBrush = this.TroopTypeIconBrush;
			Sprite sprite;
			if (troopTypeIconBrush == null)
			{
				sprite = null;
			}
			else
			{
				BrushLayer layer = troopTypeIconBrush.GetLayer(this.MurdererType);
				sprite = ((layer != null) ? layer.Sprite : null);
			}
			murdererTypeWidget.Sprite = sprite;
			Widget victimTypeWidget = this.VictimTypeWidget;
			Brush troopTypeIconBrush2 = this.TroopTypeIconBrush;
			Sprite sprite2;
			if (troopTypeIconBrush2 == null)
			{
				sprite2 = null;
			}
			else
			{
				BrushLayer layer2 = troopTypeIconBrush2.GetLayer(this.VictimType);
				sprite2 = ((layer2 != null) ? layer2.Sprite : null);
			}
			victimTypeWidget.Sprite = sprite2;
			this.ActionIconWidget.Sprite = this.ActionIconWidget.Context.SpriteData.GetSprite("General\\Mission\\PersonalKillfeed\\" + this.GetSpriteName());
			this.ActionIconWidget.Color = (this.IsUnconscious ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 0f, 0f, 1f));
			if (string.IsNullOrEmpty(this.VictimName))
			{
				this.VictimNameWidget.IsVisible = false;
				this.ActionIconWidget.MarginRight = 0f;
				this.VictimTypeWidget.MarginLeft = 5f;
			}
			if (string.IsNullOrEmpty(this.MurdererName))
			{
				this.MurdererNameWidget.IsVisible = false;
				this.ActionIconWidget.MarginLeft = 0f;
				this.MurdererTypeWidget.MarginRight = 5f;
			}
			if (this.IsSuicide)
			{
				this.MurdererNameWidget.IsVisible = false;
				this.MurdererTypeWidget.IsVisible = false;
			}
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00025ABB File Offset: 0x00023CBB
		private string GetSpriteName()
		{
			if (this.IsDrowning)
			{
				return "drowning_kill_icon";
			}
			if (this.IsHeadshot)
			{
				return "headshot_kill_icon";
			}
			return "kill_feed_skull";
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00025ADE File Offset: 0x00023CDE
		public void SetSpeedModifier(float newSpeed)
		{
			if (newSpeed > this._speedModifier)
			{
				this._speedModifier = newSpeed;
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x00025AF0 File Offset: 0x00023CF0
		// (set) Token: 0x06000DB8 RID: 3512 RVA: 0x00025AF8 File Offset: 0x00023CF8
		[Editor(false)]
		public string MurdererName
		{
			get
			{
				return this._murdererName;
			}
			set
			{
				if (value != this._murdererName)
				{
					this._murdererName = value;
					base.OnPropertyChanged<string>(value, "MurdererName");
				}
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x00025B1B File Offset: 0x00023D1B
		// (set) Token: 0x06000DBA RID: 3514 RVA: 0x00025B23 File Offset: 0x00023D23
		[Editor(false)]
		public string MurdererType
		{
			get
			{
				return this._murdererType;
			}
			set
			{
				if (value != this._murdererType)
				{
					this._murdererType = value;
					base.OnPropertyChanged<string>(value, "MurdererType");
				}
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x00025B46 File Offset: 0x00023D46
		// (set) Token: 0x06000DBC RID: 3516 RVA: 0x00025B4E File Offset: 0x00023D4E
		[Editor(false)]
		public string VictimName
		{
			get
			{
				return this._victimName;
			}
			set
			{
				if (value != this._victimName)
				{
					this._victimName = value;
					base.OnPropertyChanged<string>(value, "VictimName");
				}
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x00025B71 File Offset: 0x00023D71
		// (set) Token: 0x06000DBE RID: 3518 RVA: 0x00025B79 File Offset: 0x00023D79
		[Editor(false)]
		public string VictimType
		{
			get
			{
				return this._victimType;
			}
			set
			{
				if (value != this._victimType)
				{
					this._victimType = value;
					base.OnPropertyChanged<string>(value, "VictimType");
				}
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x00025B9C File Offset: 0x00023D9C
		// (set) Token: 0x06000DC0 RID: 3520 RVA: 0x00025BA4 File Offset: 0x00023DA4
		[Editor(false)]
		public bool IsUnconscious
		{
			get
			{
				return this._isUnconscious;
			}
			set
			{
				if (value != this._isUnconscious)
				{
					this._isUnconscious = value;
					base.OnPropertyChanged(value, "IsUnconscious");
				}
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x00025BC2 File Offset: 0x00023DC2
		// (set) Token: 0x06000DC2 RID: 3522 RVA: 0x00025BCA File Offset: 0x00023DCA
		[Editor(false)]
		public bool IsHeadshot
		{
			get
			{
				return this._isHeadshot;
			}
			set
			{
				if (value != this._isHeadshot)
				{
					this._isHeadshot = value;
					base.OnPropertyChanged(value, "IsHeadshot");
				}
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x00025BE8 File Offset: 0x00023DE8
		// (set) Token: 0x06000DC4 RID: 3524 RVA: 0x00025BF0 File Offset: 0x00023DF0
		[Editor(false)]
		public bool IsSuicide
		{
			get
			{
				return this._isSuicide;
			}
			set
			{
				if (value != this._isSuicide)
				{
					this._isSuicide = value;
					base.OnPropertyChanged(value, "IsSuicide");
				}
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x00025C0E File Offset: 0x00023E0E
		// (set) Token: 0x06000DC6 RID: 3526 RVA: 0x00025C16 File Offset: 0x00023E16
		[Editor(false)]
		public bool IsDrowning
		{
			get
			{
				return this._isDrowning;
			}
			set
			{
				if (value != this._isDrowning)
				{
					this._isDrowning = value;
					base.OnPropertyChanged(value, "IsDrowning");
				}
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00025C34 File Offset: 0x00023E34
		// (set) Token: 0x06000DC8 RID: 3528 RVA: 0x00025C3C File Offset: 0x00023E3C
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

		// Token: 0x04000635 RID: 1589
		private float _speedModifier = 1f;

		// Token: 0x04000637 RID: 1591
		private bool _initialized;

		// Token: 0x04000638 RID: 1592
		private string _murdererName;

		// Token: 0x04000639 RID: 1593
		private string _murdererType;

		// Token: 0x0400063A RID: 1594
		private string _victimName;

		// Token: 0x0400063B RID: 1595
		private string _victimType;

		// Token: 0x0400063C RID: 1596
		private bool _isUnconscious;

		// Token: 0x0400063D RID: 1597
		private bool _isHeadshot;

		// Token: 0x0400063E RID: 1598
		private bool _isSuicide;

		// Token: 0x0400063F RID: 1599
		private bool _isDrowning;

		// Token: 0x04000640 RID: 1600
		private bool _isPaused;
	}
}
