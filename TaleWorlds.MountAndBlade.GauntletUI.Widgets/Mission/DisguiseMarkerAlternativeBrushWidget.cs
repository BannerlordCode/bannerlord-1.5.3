using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000DE RID: 222
	public class DisguiseMarkerAlternativeBrushWidget : BrushWidget
	{
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x0001FFB1 File Offset: 0x0001E1B1
		// (set) Token: 0x06000B67 RID: 2919 RVA: 0x0001FFB9 File Offset: 0x0001E1B9
		public Widget BackgroundGlowWidget { get; set; }

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x0001FFC2 File Offset: 0x0001E1C2
		// (set) Token: 0x06000B69 RID: 2921 RVA: 0x0001FFCA File Offset: 0x0001E1CA
		public Widget FrameWidget { get; set; }

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x0001FFD3 File Offset: 0x0001E1D3
		// (set) Token: 0x06000B6B RID: 2923 RVA: 0x0001FFDB File Offset: 0x0001E1DB
		public Widget FillBarWidget { get; set; }

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x0001FFE4 File Offset: 0x0001E1E4
		// (set) Token: 0x06000B6D RID: 2925 RVA: 0x0001FFEC File Offset: 0x0001E1EC
		public float AlarmedHeight { get; set; } = 40f;

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000B6E RID: 2926 RVA: 0x0001FFF5 File Offset: 0x0001E1F5
		// (set) Token: 0x06000B6F RID: 2927 RVA: 0x0001FFFD File Offset: 0x0001E1FD
		public float DefaultHeight { get; set; } = 20f;

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000B70 RID: 2928 RVA: 0x00020006 File Offset: 0x0001E206
		// (set) Token: 0x06000B71 RID: 2929 RVA: 0x0002000E File Offset: 0x0001E20E
		public Vec2 Position { get; set; }

		// Token: 0x06000B72 RID: 2930 RVA: 0x00020017 File Offset: 0x0001E217
		public DisguiseMarkerAlternativeBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00020038 File Offset: 0x0001E238
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.ScaledPositionYOffset = this.Position.y - base.Size.Y / 2f;
			base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
			bool flag = !string.IsNullOrEmpty(this.AlarmState) && (float)this.AlarmProgress > 0f;
			base.SuggestedHeight = MathF.Lerp(base.SuggestedHeight, flag ? this.AlarmedHeight : this.DefaultHeight, dt * 5f, 1E-05f);
			base.SuggestedWidth = MathF.Lerp(base.SuggestedWidth, (float)(flag ? 38 : 32), dt * 5f, 1E-05f);
			if (!string.IsNullOrEmpty(this.OffenseTypeIdentifier))
			{
				Widget backgroundGlowWidget = this.BackgroundGlowWidget;
				if (backgroundGlowWidget != null)
				{
					backgroundGlowWidget.SetState(this.OffenseTypeIdentifier);
				}
			}
			if (!string.IsNullOrEmpty(this.AlarmState))
			{
				Widget fillBarWidget = this.FillBarWidget;
				if (fillBarWidget == null)
				{
					return;
				}
				fillBarWidget.SetState(this.AlarmState);
			}
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0002014D File Offset: 0x0001E34D
		private void UpdateState()
		{
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0002014F File Offset: 0x0001E34F
		private void UpdateAlarmState()
		{
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x00020151 File Offset: 0x0001E351
		// (set) Token: 0x06000B77 RID: 2935 RVA: 0x00020159 File Offset: 0x0001E359
		public int AlarmProgress
		{
			get
			{
				return this._alarmProgress;
			}
			set
			{
				if (value != this._alarmProgress)
				{
					this._alarmProgress = value;
					base.OnPropertyChanged(value, "AlarmProgress");
				}
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x00020177 File Offset: 0x0001E377
		// (set) Token: 0x06000B79 RID: 2937 RVA: 0x0002017F File Offset: 0x0001E37F
		public string AlarmState
		{
			get
			{
				return this._alarmState;
			}
			set
			{
				if (value != this._alarmState)
				{
					this._alarmState = value;
					base.OnPropertyChanged<string>(value, "AlarmState");
					this.UpdateAlarmState();
				}
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x000201A8 File Offset: 0x0001E3A8
		// (set) Token: 0x06000B7B RID: 2939 RVA: 0x000201B0 File Offset: 0x0001E3B0
		public string OffenseTypeIdentifier
		{
			get
			{
				return this._offenseTypeIdentifier;
			}
			set
			{
				if (value != this._offenseTypeIdentifier)
				{
					this._offenseTypeIdentifier = value;
					base.OnPropertyChanged<string>(value, "OffenseTypeIdentifier");
					this.UpdateState();
				}
			}
		}

		// Token: 0x0400052C RID: 1324
		private int _alarmProgress;

		// Token: 0x0400052D RID: 1325
		private string _alarmState;

		// Token: 0x0400052E RID: 1326
		private string _offenseTypeIdentifier;
	}
}
