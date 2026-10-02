using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200000C RID: 12
	public class CircleLoadingAnimWidget : Widget
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000081 RID: 129 RVA: 0x0000334C File Offset: 0x0000154C
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00003354 File Offset: 0x00001554
		public float NumOfCirclesInASecond { get; set; } = 0.5f;

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000083 RID: 131 RVA: 0x0000335D File Offset: 0x0000155D
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00003365 File Offset: 0x00001565
		public float FullAlpha { get; set; } = 1f;

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000085 RID: 133 RVA: 0x0000336E File Offset: 0x0000156E
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00003376 File Offset: 0x00001576
		public float CircleRadius { get; set; } = 50f;

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000087 RID: 135 RVA: 0x0000337F File Offset: 0x0000157F
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00003387 File Offset: 0x00001587
		public float StaySeconds { get; set; } = 2f;

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003390 File Offset: 0x00001590
		// (set) Token: 0x0600008A RID: 138 RVA: 0x00003398 File Offset: 0x00001598
		public float FadeInSeconds { get; set; } = 0.2f;

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600008B RID: 139 RVA: 0x000033A1 File Offset: 0x000015A1
		// (set) Token: 0x0600008C RID: 140 RVA: 0x000033A9 File Offset: 0x000015A9
		public float FadeOutSeconds { get; set; } = 0.2f;

		// Token: 0x0600008D RID: 141 RVA: 0x000033B4 File Offset: 0x000015B4
		public CircleLoadingAnimWidget(UIContext context)
			: base(context)
		{
			this._isChildPositionsDirty = true;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003411 File Offset: 0x00001611
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			this._isChildPositionsDirty = true;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003421 File Offset: 0x00001621
		protected override void OnAfterChildRemoved(Widget child, int previousIndexOfChild)
		{
			base.OnAfterChildRemoved(child, previousIndexOfChild);
			this._isChildPositionsDirty = true;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003434 File Offset: 0x00001634
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this._visualState = CircleLoadingAnimWidget.VisualState.FadeIn;
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

		// Token: 0x06000091 RID: 145 RVA: 0x0000353C File Offset: 0x0000173C
		private void UpdateAlphaValues(float dt)
		{
			float num = 1f;
			if (this._visualState == CircleLoadingAnimWidget.VisualState.FadeIn)
			{
				num = Mathf.Lerp(base.AlphaFactor, 1f, dt / this.FadeInSeconds);
				if (base.AlphaFactor >= 0.9f)
				{
					this._visualState = CircleLoadingAnimWidget.VisualState.Animating;
					this._stayStartTime = this._totalTime;
				}
			}
			else if (this._visualState == CircleLoadingAnimWidget.VisualState.Animating)
			{
				num = 1f;
				if (this.StaySeconds != -1f && this._totalTime - this._stayStartTime > this.StaySeconds)
				{
					this._visualState = CircleLoadingAnimWidget.VisualState.FadeOut;
				}
			}
			else if (this._visualState == CircleLoadingAnimWidget.VisualState.FadeOut)
			{
				num = Mathf.Lerp(base.AlphaFactor, 0f, dt / this.FadeOutSeconds);
				if (base.AlphaFactor <= 0.01f && this._totalTime - (this._stayStartTime + this.StaySeconds + this.FadeOutSeconds) > 3f)
				{
					this._visualState = CircleLoadingAnimWidget.VisualState.FadeIn;
				}
			}
			else
			{
				Debug.FailedAssert("This visual state is not enabled", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\CircleLoadingAnimWidget.cs", "UpdateAlphaValues", 122);
			}
			this.SetGlobalAlphaRecursively(num);
		}

		// Token: 0x0400003E RID: 62
		private CircleLoadingAnimWidget.VisualState _visualState;

		// Token: 0x0400003F RID: 63
		private float _stayStartTime;

		// Token: 0x04000040 RID: 64
		private bool _initialized;

		// Token: 0x04000041 RID: 65
		private float _totalTime;

		// Token: 0x04000042 RID: 66
		private bool _isChildPositionsDirty;

		// Token: 0x0200019A RID: 410
		public enum VisualState
		{
			// Token: 0x040009B7 RID: 2487
			FadeIn,
			// Token: 0x040009B8 RID: 2488
			Animating,
			// Token: 0x040009B9 RID: 2489
			FadeOut
		}
	}
}
