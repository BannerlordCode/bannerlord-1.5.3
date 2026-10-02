using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.HUD
{
	// Token: 0x020000C4 RID: 196
	public class HUDExtensionBrushWidget : BrushWidget
	{
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x0001CF5B File Offset: 0x0001B15B
		// (set) Token: 0x06000A49 RID: 2633 RVA: 0x0001CF63 File Offset: 0x0001B163
		public float AlphaChangeDuration { get; set; } = 0.15f;

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0001CF6C File Offset: 0x0001B16C
		// (set) Token: 0x06000A4B RID: 2635 RVA: 0x0001CF74 File Offset: 0x0001B174
		public float OrderEnabledAlpha { get; set; } = 0.3f;

		// Token: 0x06000A4C RID: 2636 RVA: 0x0001CF7D File Offset: 0x0001B17D
		public HUDExtensionBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0001CFC0 File Offset: 0x0001B1C0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._currentAlpha - this._targetAlpha > 1E-45f)
			{
				if (this._alphaChangeTimeElapsed < this.AlphaChangeDuration)
				{
					this._currentAlpha = MathF.Lerp(this._initialAlpha, this._targetAlpha, this._alphaChangeTimeElapsed / this.AlphaChangeDuration, 1E-05f);
					this.SetGlobalAlphaRecursively(this._currentAlpha);
					this._alphaChangeTimeElapsed += dt;
					return;
				}
			}
			else if (this._currentAlpha != this._targetAlpha)
			{
				this._currentAlpha = this._targetAlpha;
				this.SetGlobalAlphaRecursively(this._targetAlpha);
			}
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0001D060 File Offset: 0x0001B260
		private void OnIsOrderEnabledChanged()
		{
			this._alphaChangeTimeElapsed = 0f;
			this._targetAlpha = (this.IsOrderActive ? this.OrderEnabledAlpha : 1f);
			this._initialAlpha = this._currentAlpha;
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x0001D094 File Offset: 0x0001B294
		// (set) Token: 0x06000A50 RID: 2640 RVA: 0x0001D09C File Offset: 0x0001B29C
		[Editor(false)]
		public bool IsOrderActive
		{
			get
			{
				return this._isOrderActive;
			}
			set
			{
				if (this._isOrderActive != value)
				{
					this._isOrderActive = value;
					base.OnPropertyChanged(value, "IsOrderActive");
					this.OnIsOrderEnabledChanged();
				}
			}
		}

		// Token: 0x040004A6 RID: 1190
		private float _alphaChangeTimeElapsed;

		// Token: 0x040004A7 RID: 1191
		private float _initialAlpha = 1f;

		// Token: 0x040004A8 RID: 1192
		private float _targetAlpha = 1f;

		// Token: 0x040004A9 RID: 1193
		private float _currentAlpha = 1f;

		// Token: 0x040004AA RID: 1194
		private bool _isOrderActive;
	}
}
