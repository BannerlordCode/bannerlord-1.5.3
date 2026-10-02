using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Barter
{
	// Token: 0x02000191 RID: 401
	public class BarterItemCountControlButtonWidget : ButtonWidget
	{
		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x060014D8 RID: 5336 RVA: 0x00038DD8 File Offset: 0x00036FD8
		// (set) Token: 0x060014D9 RID: 5337 RVA: 0x00038DE0 File Offset: 0x00036FE0
		public float IncreaseToHoldDelay { get; set; } = 1f;

		// Token: 0x060014DA RID: 5338 RVA: 0x00038DE9 File Offset: 0x00036FE9
		public BarterItemCountControlButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x00038E00 File Offset: 0x00037000
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this._totalTime += dt;
			if (base.IsPressed && this._clickStartTime + this.IncreaseToHoldDelay < this._totalTime)
			{
				base.EventFired("MoveOne", Array.Empty<object>());
			}
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x00038E4F File Offset: 0x0003704F
		protected override void OnMousePressed()
		{
			base.OnMousePressed();
			this._clickStartTime = this._totalTime;
			base.EventFired("MoveOne", Array.Empty<object>());
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x00038E73 File Offset: 0x00037073
		protected override void OnMouseReleased(bool isFromInput)
		{
			base.OnMouseReleased(isFromInput);
			this._clickStartTime = 0f;
		}

		// Token: 0x04000980 RID: 2432
		private float _clickStartTime;

		// Token: 0x04000981 RID: 2433
		private float _totalTime;
	}
}
