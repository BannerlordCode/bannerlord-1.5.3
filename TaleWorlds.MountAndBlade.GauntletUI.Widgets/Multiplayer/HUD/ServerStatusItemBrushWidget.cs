using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.HUD
{
	// Token: 0x020000CA RID: 202
	public class ServerStatusItemBrushWidget : BrushWidget
	{
		// Token: 0x06000AAA RID: 2730 RVA: 0x0001DF73 File Offset: 0x0001C173
		public ServerStatusItemBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0001DF84 File Offset: 0x0001C184
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.RegisterBrushStatesOfWidget();
				this._initialized = true;
				this.OnStatusChange(this.Status);
			}
			if (Math.Abs(base.ReadOnlyBrush.GlobalAlphaFactor - this._currentAlphaTarget) > 0.001f)
			{
				this.SetGlobalAlphaRecursively(MathF.Lerp(base.ReadOnlyBrush.GlobalAlphaFactor, this._currentAlphaTarget, dt * 5f, 1E-05f));
			}
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0001DFFF File Offset: 0x0001C1FF
		private void OnStatusChange(int value)
		{
			this.SetState(value.ToString());
			if (value == 0)
			{
				this._currentAlphaTarget = 0f;
				return;
			}
			if (value - 1 > 1)
			{
				return;
			}
			this._currentAlphaTarget = 1f;
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x0001E02F File Offset: 0x0001C22F
		// (set) Token: 0x06000AAE RID: 2734 RVA: 0x0001E037 File Offset: 0x0001C237
		public int Status
		{
			get
			{
				return this._status;
			}
			set
			{
				if (value != this._status)
				{
					this._status = value;
					base.OnPropertyChanged(value, "Status");
					this.OnStatusChange(value);
				}
			}
		}

		// Token: 0x040004DE RID: 1246
		private float _currentAlphaTarget;

		// Token: 0x040004DF RID: 1247
		private bool _initialized;

		// Token: 0x040004E0 RID: 1248
		private int _status = -1;
	}
}
