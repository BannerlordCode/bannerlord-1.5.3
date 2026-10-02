using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.Launcher.Library.CustomWidgets
{
	// Token: 0x02000020 RID: 32
	public class LauncherCircleLoadingAnimWidget : Widget
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00006035 File Offset: 0x00004235
		// (set) Token: 0x06000146 RID: 326 RVA: 0x0000603D File Offset: 0x0000423D
		public float NumOfCirclesInASecond { get; set; } = 0.5f;

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00006046 File Offset: 0x00004246
		// (set) Token: 0x06000148 RID: 328 RVA: 0x0000604E File Offset: 0x0000424E
		public float FullAlpha { get; set; } = 1f;

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00006057 File Offset: 0x00004257
		// (set) Token: 0x0600014A RID: 330 RVA: 0x0000605F File Offset: 0x0000425F
		public float CircleRadius { get; set; } = 50f;

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00006068 File Offset: 0x00004268
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00006070 File Offset: 0x00004270
		public float StaySeconds { get; set; } = 2f;

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00006079 File Offset: 0x00004279
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00006081 File Offset: 0x00004281
		public float FadeInSeconds { get; set; } = 0.2f;

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600014F RID: 335 RVA: 0x0000608A File Offset: 0x0000428A
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00006092 File Offset: 0x00004292
		public float FadeOutSeconds { get; set; } = 0.2f;

		// Token: 0x06000151 RID: 337 RVA: 0x0000609C File Offset: 0x0000429C
		public LauncherCircleLoadingAnimWidget(UIContext context)
			: base(context)
		{
			this._isChildPositionsDirty = true;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000060F9 File Offset: 0x000042F9
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			this._isChildPositionsDirty = true;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006109 File Offset: 0x00004309
		protected override void OnAfterChildRemoved(Widget child, int previousIndexOfChild)
		{
			base.OnAfterChildRemoved(child, previousIndexOfChild);
			this._isChildPositionsDirty = true;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000611C File Offset: 0x0000431C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this._visualState = LauncherCircleLoadingAnimWidget.VisualState.FadeIn;
				this.SetGlobalAlphaRecursively(0f);
				this._initialized = true;
			}
			this._totalTime += dt;
			if (this._isChildPositionsDirty)
			{
				float num = 360f / (float)base.Children.Count;
				float num2 = 0f;
				for (int i = 0; i < base.Children.Count; i++)
				{
					float num3 = MathF.Cos(num2 * 0.017453292f) * this.CircleRadius;
					float num4 = MathF.Sin(num2 * 0.017453292f) * this.CircleRadius;
					base.Children[i].PositionXOffset = num3;
					base.Children[i].PositionYOffset = num4;
					num2 += num;
					num2 %= 360f;
				}
				this._isChildPositionsDirty = false;
			}
			if (base.IsRecursivelyVisible())
			{
				base.Rotation += dt * 360f * this.NumOfCirclesInASecond;
				this.UpdateAlphaValues(dt);
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00006224 File Offset: 0x00004424
		private void UpdateAlphaValues(float dt)
		{
			float num = 1f;
			if (this._visualState == LauncherCircleLoadingAnimWidget.VisualState.FadeIn)
			{
				num = Mathf.Lerp(base.AlphaFactor, 1f, dt / this.FadeInSeconds);
				if (base.AlphaFactor >= 0.9f)
				{
					this._visualState = LauncherCircleLoadingAnimWidget.VisualState.Animating;
					this._stayStartTime = this._totalTime;
				}
			}
			else if (this._visualState == LauncherCircleLoadingAnimWidget.VisualState.Animating)
			{
				num = 1f;
				if (this.StaySeconds != -1f && this._totalTime - this._stayStartTime > this.StaySeconds)
				{
					this._visualState = LauncherCircleLoadingAnimWidget.VisualState.FadeOut;
				}
			}
			else if (this._visualState == LauncherCircleLoadingAnimWidget.VisualState.FadeOut)
			{
				num = Mathf.Lerp(base.AlphaFactor, 0f, dt / this.FadeOutSeconds);
				if (base.AlphaFactor <= 0.01f && this._totalTime - (this._stayStartTime + this.StaySeconds + this.FadeOutSeconds) > 3f)
				{
					this._visualState = LauncherCircleLoadingAnimWidget.VisualState.FadeIn;
				}
			}
			else
			{
				Debug.FailedAssert("This visual state is not enabled", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Launcher.Library\\CustomWidgets\\LauncherCircleLoadingAnimWidget.cs", "UpdateAlphaValues", 122);
			}
			this.SetGlobalAlphaRecursively(num);
		}

		// Token: 0x040000A4 RID: 164
		private LauncherCircleLoadingAnimWidget.VisualState _visualState;

		// Token: 0x040000A5 RID: 165
		private float _stayStartTime;

		// Token: 0x040000A6 RID: 166
		private bool _initialized;

		// Token: 0x040000A7 RID: 167
		private float _totalTime;

		// Token: 0x040000A8 RID: 168
		private bool _isChildPositionsDirty;

		// Token: 0x02000046 RID: 70
		public enum VisualState
		{
			// Token: 0x04000102 RID: 258
			FadeIn,
			// Token: 0x04000103 RID: 259
			Animating,
			// Token: 0x04000104 RID: 260
			FadeOut
		}
	}
}
