using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterCreation.Culture
{
	// Token: 0x0200018E RID: 398
	public class CharacterCreationFirstStageFadeOutWidget : Widget
	{
		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x00038A87 File Offset: 0x00036C87
		// (set) Token: 0x060014C3 RID: 5315 RVA: 0x00038A8F File Offset: 0x00036C8F
		public float StayTime { get; set; } = 1.5f;

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x00038A98 File Offset: 0x00036C98
		// (set) Token: 0x060014C5 RID: 5317 RVA: 0x00038AA0 File Offset: 0x00036CA0
		public float FadeOutTime { get; set; } = 1.5f;

		// Token: 0x060014C6 RID: 5318 RVA: 0x00038AA9 File Offset: 0x00036CA9
		public CharacterCreationFirstStageFadeOutWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00038AC8 File Offset: 0x00036CC8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._totalTime < this.StayTime)
			{
				this.SetGlobalAlphaRecursively(1f);
				base.IsEnabled = true;
			}
			else if (this._totalTime > this.StayTime && this._totalTime < this.StayTime + this.FadeOutTime)
			{
				float num = Mathf.Lerp(1f, 0f, (this._totalTime - this.StayTime) / this.FadeOutTime);
				this.SetGlobalAlphaRecursively(num);
				base.IsEnabled = num > 0.2f;
			}
			else
			{
				this.SetGlobalAlphaRecursively(0f);
				base.IsEnabled = false;
			}
			this._totalTime += dt;
		}

		// Token: 0x04000977 RID: 2423
		private float _totalTime;
	}
}
