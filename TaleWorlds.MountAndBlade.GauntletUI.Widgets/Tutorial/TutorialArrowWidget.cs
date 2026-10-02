using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x02000049 RID: 73
	public class TutorialArrowWidget : Widget
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0000CCF7 File Offset: 0x0000AEF7
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x0000CCFF File Offset: 0x0000AEFF
		public bool IsArrowEnabled { get; set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x0000CD08 File Offset: 0x0000AF08
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x0000CD10 File Offset: 0x0000AF10
		public float FadeInTime { get; set; } = 1f;

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x0000CD19 File Offset: 0x0000AF19
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x0000CD21 File Offset: 0x0000AF21
		public float BigCircleRadius { get; set; } = 2f;

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x0000CD2A File Offset: 0x0000AF2A
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x0000CD32 File Offset: 0x0000AF32
		public float SmallCircleRadius { get; set; } = 2f;

		// Token: 0x06000415 RID: 1045 RVA: 0x0000CD3B File Offset: 0x0000AF3B
		public TutorialArrowWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000CD68 File Offset: 0x0000AF68
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this.IsArrowEnabled)
			{
				base.IsVisible = false;
				this.SetGlobalAlphaRecursively(0f);
				return;
			}
			base.IsVisible = true;
			base.ScaledSuggestedWidth = this._localWidth;
			base.ScaledSuggestedHeight = this._localHeight;
			if (this._startTime > -1f)
			{
				float num = Mathf.Lerp(0f, 1f, Mathf.Clamp((base.EventManager.Time - this._startTime) / this.FadeInTime, 0f, 1f));
				this.SetGlobalAlphaRecursively(num);
				return;
			}
			this.SetGlobalAlphaRecursively(0f);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000CE10 File Offset: 0x0000B010
		public void SetArrowProperties(float width, float height, bool isDirectionDown, bool isDirectionRight)
		{
			if (this._localWidth != width || this._localHeight != height || this._isDirectionDown != isDirectionDown || this._isDirectionRight != isDirectionRight)
			{
				base.RemoveAllChildren();
				float num = (float)Math.Sqrt((double)(width * width + height * height));
				float num2 = (this.BigCircleRadius + this.SmallCircleRadius) / 2f;
				int num3 = (int)(num / num2);
				float num4 = 0f;
				float num5 = 0f;
				float num6;
				float num7;
				if (isDirectionDown)
				{
					num6 = width;
					num7 = height;
				}
				else
				{
					num6 = width;
					num5 = height;
					num7 = 0f;
				}
				float num8 = (isDirectionRight ? this.BigCircleRadius : this.SmallCircleRadius);
				float num9 = (isDirectionRight ? this.SmallCircleRadius : this.BigCircleRadius);
				for (int i = 0; i < num3; i++)
				{
					Widget defaultCircleWidgetTemplate = this.GetDefaultCircleWidgetTemplate();
					base.AddChild(defaultCircleWidgetTemplate);
					float num10 = num2 * (float)i / MathF.Abs(num4 - num6);
					float num11 = Mathf.Lerp(num8, num9, num10);
					defaultCircleWidgetTemplate.PositionXOffset = Mathf.Lerp(num4, num6, num10);
					defaultCircleWidgetTemplate.PositionYOffset = Mathf.Lerp(num5, num7, num10);
					defaultCircleWidgetTemplate.SuggestedHeight = num11;
					defaultCircleWidgetTemplate.SuggestedWidth = num11;
				}
				this._localWidth = width;
				this._localHeight = height;
				this._isDirectionDown = isDirectionDown;
				this._isDirectionRight = isDirectionRight;
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000CF59 File Offset: 0x0000B159
		public void ResetFade()
		{
			this._startTime = base.EventManager.Time;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000CF6C File Offset: 0x0000B16C
		public void DisableFade()
		{
			this._startTime = base.EventManager.Time;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000CF7F File Offset: 0x0000B17F
		private Widget GetDefaultCircleWidgetTemplate()
		{
			return new Widget(base.Context)
			{
				WidthSizePolicy = SizePolicy.Fixed,
				HeightSizePolicy = SizePolicy.Fixed,
				Sprite = base.Context.SpriteData.GetSprite("BlankWhiteCircle"),
				IsEnabled = false
			};
		}

		// Token: 0x040001AD RID: 429
		private float _localWidth;

		// Token: 0x040001AE RID: 430
		private float _localHeight;

		// Token: 0x040001AF RID: 431
		private bool _isDirectionDown;

		// Token: 0x040001B0 RID: 432
		private bool _isDirectionRight;

		// Token: 0x040001B1 RID: 433
		private float _startTime;
	}
}
