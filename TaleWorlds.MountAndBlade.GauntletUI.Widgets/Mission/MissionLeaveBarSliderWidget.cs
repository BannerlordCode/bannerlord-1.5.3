using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000E3 RID: 227
	public class MissionLeaveBarSliderWidget : SliderWidget
	{
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x00020FBE File Offset: 0x0001F1BE
		private float CurrentAlpha
		{
			get
			{
				return base.ReadOnlyBrush.GlobalAlphaFactor;
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06000BCB RID: 3019 RVA: 0x00020FCB File Offset: 0x0001F1CB
		// (set) Token: 0x06000BCC RID: 3020 RVA: 0x00020FD3 File Offset: 0x0001F1D3
		public float FadeInMultiplier { get; set; } = 1f;

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000BCD RID: 3021 RVA: 0x00020FDC File Offset: 0x0001F1DC
		// (set) Token: 0x06000BCE RID: 3022 RVA: 0x00020FE4 File Offset: 0x0001F1E4
		public float FadeOutMultiplier { get; set; } = 1f;

		// Token: 0x06000BCF RID: 3023 RVA: 0x00020FED File Offset: 0x0001F1ED
		public MissionLeaveBarSliderWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0002100C File Offset: 0x0001F20C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.SetGlobalAlphaRecursively(0f);
				this._initialized = true;
			}
			float num = ((base.ValueFloat > 0f) ? this.FadeInMultiplier : this.FadeOutMultiplier);
			float num2 = (float)((base.ValueFloat > 0f) ? 1 : 0);
			float num3 = Mathf.Clamp(Mathf.Lerp(this.CurrentAlpha, num2, num * 0.2f), 0f, 1f);
			this.SetGlobalAlphaRecursively(num3);
		}

		// Token: 0x04000556 RID: 1366
		private bool _initialized;
	}
}
