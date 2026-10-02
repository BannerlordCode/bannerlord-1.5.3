using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200003F RID: 63
	public class SettlementStatTextWidget : TextWidget
	{
		// Token: 0x060003B6 RID: 950 RVA: 0x0000BC49 File Offset: 0x00009E49
		public SettlementStatTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000BC54 File Offset: 0x00009E54
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			switch (this._state)
			{
			case SettlementStatTextWidget.State.Idle:
				if (this.IsWarning)
				{
					this.SetState("Warning");
				}
				else
				{
					this.SetState("Default");
				}
				this._state = SettlementStatTextWidget.State.Start;
				return;
			case SettlementStatTextWidget.State.Start:
				this._state = ((base.BrushRenderer.Brush != null) ? SettlementStatTextWidget.State.Playing : SettlementStatTextWidget.State.Start);
				return;
			case SettlementStatTextWidget.State.Playing:
				base.BrushRenderer.RestartAnimation();
				this._state = SettlementStatTextWidget.State.End;
				break;
			case SettlementStatTextWidget.State.End:
				break;
			default:
				return;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x0000BCD8 File Offset: 0x00009ED8
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x0000BCE0 File Offset: 0x00009EE0
		[Editor(false)]
		public bool IsWarning
		{
			get
			{
				return this._isWarning;
			}
			set
			{
				if (value != this._isWarning)
				{
					this._isWarning = value;
					base.OnPropertyChanged(value, "IsWarning");
					this.SetState(this._isWarning ? "Warning" : "Default");
				}
			}
		}

		// Token: 0x04000189 RID: 393
		private SettlementStatTextWidget.State _state;

		// Token: 0x0400018A RID: 394
		private bool _isWarning;

		// Token: 0x020001A5 RID: 421
		public enum State
		{
			// Token: 0x040009DF RID: 2527
			Idle,
			// Token: 0x040009E0 RID: 2528
			Start,
			// Token: 0x040009E1 RID: 2529
			Playing,
			// Token: 0x040009E2 RID: 2530
			End
		}
	}
}
