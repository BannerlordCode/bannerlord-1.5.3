using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000037 RID: 55
	public class ParallaxItemBrushWidget : BrushWidget
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0000A663 File Offset: 0x00008863
		// (set) Token: 0x0600033E RID: 830 RVA: 0x0000A66B File Offset: 0x0000886B
		public bool IsEaseInOutEnabled { get; set; } = true;

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600033F RID: 831 RVA: 0x0000A674 File Offset: 0x00008874
		// (set) Token: 0x06000340 RID: 832 RVA: 0x0000A67C File Offset: 0x0000887C
		public float OneDirectionDuration { get; set; } = 1f;

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000A685 File Offset: 0x00008885
		// (set) Token: 0x06000342 RID: 834 RVA: 0x0000A68D File Offset: 0x0000888D
		public float OneDirectionDistance { get; set; } = 1f;

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000A696 File Offset: 0x00008896
		// (set) Token: 0x06000344 RID: 836 RVA: 0x0000A69E File Offset: 0x0000889E
		public ParallaxItemBrushWidget.ParallaxMovementDirection InitialDirection { get; set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000A6A7 File Offset: 0x000088A7
		private float _centerOffset
		{
			get
			{
				return this.OneDirectionDuration / 2f;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0000A6B5 File Offset: 0x000088B5
		private float _localTime
		{
			get
			{
				return base.Context.EventManager.Time + this._centerOffset;
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000A6CE File Offset: 0x000088CE
		public ParallaxItemBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000A6F4 File Offset: 0x000088F4
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				this.OneDirectionDuration = MathF.Max(float.Epsilon, this.OneDirectionDuration);
				this._initialized = true;
			}
			if (this.InitialDirection != ParallaxItemBrushWidget.ParallaxMovementDirection.None)
			{
				bool flag = this._localTime % (this.OneDirectionDuration * 4f) > this.OneDirectionDuration * 2f;
				float num3;
				if (this.IsEaseInOutEnabled)
				{
					float num = this._localTime % (this.OneDirectionDuration * 4f);
					float oneDirectionDuration = this.OneDirectionDuration;
					float num2 = MathF.PingPong(0f, this.OneDirectionDuration * 4f, this._localTime) / (this.OneDirectionDuration * 4f);
					float quadEaseInOut = this.GetQuadEaseInOut(num2);
					num3 = MathF.Lerp(-this.OneDirectionDistance, this.OneDirectionDistance, quadEaseInOut, 1E-05f);
				}
				else
				{
					float num4 = MathF.PingPong(0f, this.OneDirectionDuration, this._localTime) / this.OneDirectionDuration;
					num3 = this.OneDirectionDistance * num4;
					num3 = (flag ? (-num3) : num3);
				}
				switch (this.InitialDirection)
				{
				case ParallaxItemBrushWidget.ParallaxMovementDirection.Left:
					base.PositionXOffset = num3;
					return;
				case ParallaxItemBrushWidget.ParallaxMovementDirection.Right:
					base.PositionXOffset = -num3;
					return;
				case ParallaxItemBrushWidget.ParallaxMovementDirection.Up:
					base.PositionYOffset = -num3;
					return;
				case ParallaxItemBrushWidget.ParallaxMovementDirection.Down:
					base.PositionYOffset = num3;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000A848 File Offset: 0x00008A48
		private float GetCubicEaseInOut(float t)
		{
			if (t < 0.5f)
			{
				return 4f * t * t * t;
			}
			float num = 2f * t - 2f;
			return 0.5f * num * num * num + 1f;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000A888 File Offset: 0x00008A88
		private float GetElasticEaseInOut(float t)
		{
			if (t < 0.5f)
			{
				return (float)(0.5 * Math.Sin(20.420352248333657 * (double)(2f * t)) * Math.Pow(2.0, (double)(10f * (2f * t - 1f))));
			}
			return (float)(0.5 * (Math.Sin(-20.420352248333657 * (double)(2f * t - 1f + 1f)) * Math.Pow(2.0, (double)(-10f * (2f * t - 1f))) + 2.0));
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000A940 File Offset: 0x00008B40
		private float ExponentialEaseInOut(float t)
		{
			if (t == 0f || t == 1f)
			{
				return t;
			}
			if (t < 0.5f)
			{
				return (float)(0.5 * Math.Pow(2.0, (double)(20f * t - 10f)));
			}
			return (float)(-0.5 * Math.Pow(2.0, (double)(-20f * t + 10f)) + 1.0);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000A9C0 File Offset: 0x00008BC0
		private float GetQuadEaseInOut(float t)
		{
			if (t < 0.5f)
			{
				return 2f * t * t;
			}
			return -2f * t * t + 4f * t - 1f;
		}

		// Token: 0x04000153 RID: 339
		private bool _initialized;

		// Token: 0x020001A4 RID: 420
		public enum ParallaxMovementDirection
		{
			// Token: 0x040009D9 RID: 2521
			None,
			// Token: 0x040009DA RID: 2522
			Left,
			// Token: 0x040009DB RID: 2523
			Right,
			// Token: 0x040009DC RID: 2524
			Up,
			// Token: 0x040009DD RID: 2525
			Down
		}
	}
}
