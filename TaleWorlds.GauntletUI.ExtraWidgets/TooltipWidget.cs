using System;
using System.Numerics;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x02000017 RID: 23
	public class TooltipWidget : Widget
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00006D1B File Offset: 0x00004F1B
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00006D23 File Offset: 0x00004F23
		public TooltipPositioningType PositioningType { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00006D2C File Offset: 0x00004F2C
		private float _tooltipOffset
		{
			get
			{
				return 30f;
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006D33 File Offset: 0x00004F33
		public TooltipWidget(UIContext context)
			: base(context)
		{
			base.HorizontalAlignment = HorizontalAlignment.Left;
			base.VerticalAlignment = VerticalAlignment.Top;
			this._lastCheckedVisibility = true;
			base.IsVisible = true;
			this.PositioningType = TooltipPositioningType.FixedMouseMirrored;
			this.ResetAnimationProperties();
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006D70 File Offset: 0x00004F70
		protected override void RefreshState()
		{
			base.RefreshState();
			if (this._lastCheckedVisibility != base.IsVisible)
			{
				this._lastCheckedVisibility = base.IsVisible;
				if (base.IsVisible)
				{
					this.ResetAnimationProperties();
				}
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00006DA0 File Offset: 0x00004FA0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._animationState == TooltipWidget.AnimationState.NotStarted)
			{
				if (this._animationDelayTimerInFrames >= this._animationDelayInFrames)
				{
					this._animationState = TooltipWidget.AnimationState.InProgress;
				}
				else
				{
					this._animationDelayTimerInFrames++;
					this.SetGlobalAlphaRecursively(0f);
				}
			}
			if (this._animationState != TooltipWidget.AnimationState.NotStarted)
			{
				if (this._animationState == TooltipWidget.AnimationState.InProgress)
				{
					this._animationProgress += ((this.AnimTime < 1E-05f) ? 1f : (dt / this.AnimTime));
					this._animationProgress = MathF.Clamp(this._animationProgress, 0f, 1f);
					this.SetGlobalAlphaRecursively(this._animationProgress);
					if (this._animationProgress >= 1f)
					{
						this._animationState = TooltipWidget.AnimationState.Finished;
					}
				}
				this.UpdatePosition();
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006E68 File Offset: 0x00005068
		private void UpdatePosition()
		{
			if (this.PositioningType == TooltipPositioningType.FixedMouse || this.PositioningType == TooltipPositioningType.FixedMouseMirrored)
			{
				if (MathF.Abs(this._lastCheckedSize.X - base.Size.X) > 0.1f || MathF.Abs(this._lastCheckedSize.Y - base.Size.Y) > 0.1f)
				{
					this._lastCheckedSize = base.Size;
					if (this.PositioningType == TooltipPositioningType.FixedMouse)
					{
						this.SetPosition(base.EventManager.MousePosition);
						return;
					}
					this.SetMirroredPosition(base.EventManager.MousePosition);
					return;
				}
			}
			else
			{
				if (this.PositioningType == TooltipPositioningType.FollowMouse)
				{
					this.SetPosition(base.EventManager.MousePosition);
					return;
				}
				if (this.PositioningType == TooltipPositioningType.FollowMouseMirrored)
				{
					this.SetMirroredPosition(base.EventManager.MousePosition);
				}
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006F44 File Offset: 0x00005144
		private void SetPosition(Vector2 position)
		{
			Vector2 vector = position + new Vector2(this._tooltipOffset, this._tooltipOffset);
			bool flag = base.Size.X > base.EventManager.PageSize.X;
			bool flag2 = base.Size.Y > base.EventManager.PageSize.Y;
			base.ScaledPositionXOffset = (flag ? vector.X : MathF.Clamp(vector.X, 0f, base.EventManager.PageSize.X - base.Size.X));
			base.ScaledPositionYOffset = (flag2 ? vector.Y : MathF.Clamp(vector.Y, 0f, base.EventManager.PageSize.Y - base.Size.Y));
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00007020 File Offset: 0x00005220
		private void SetMirroredPosition(Vector2 tooltipPosition)
		{
			float num = 0f;
			float num2 = 0f;
			HorizontalAlignment horizontalAlignment;
			if ((double)tooltipPosition.X < (double)base.EventManager.PageSize.X * 0.5)
			{
				horizontalAlignment = HorizontalAlignment.Left;
				num = this._tooltipOffset;
			}
			else
			{
				horizontalAlignment = HorizontalAlignment.Right;
				tooltipPosition = new Vector2(-(base.EventManager.PageSize.X - tooltipPosition.X), tooltipPosition.Y);
			}
			VerticalAlignment verticalAlignment;
			if ((double)tooltipPosition.Y < (double)base.EventManager.PageSize.Y * 0.5)
			{
				verticalAlignment = VerticalAlignment.Top;
				num2 = this._tooltipOffset;
			}
			else
			{
				verticalAlignment = VerticalAlignment.Bottom;
				tooltipPosition = new Vector2(tooltipPosition.X, -(base.EventManager.PageSize.Y - tooltipPosition.Y));
			}
			tooltipPosition += new Vector2(num, num2);
			if (base.Size.X > base.EventManager.PageSize.X)
			{
				horizontalAlignment = HorizontalAlignment.Left;
				tooltipPosition = new Vector2(0f, tooltipPosition.Y);
			}
			else
			{
				if (horizontalAlignment == HorizontalAlignment.Left && tooltipPosition.X + base.Size.X > base.EventManager.PageSize.X)
				{
					tooltipPosition += new Vector2(-(tooltipPosition.X + base.Size.X - base.EventManager.PageSize.X), 0f);
				}
				if (horizontalAlignment == HorizontalAlignment.Right && tooltipPosition.X - base.Size.X + base.EventManager.PageSize.X < 0f)
				{
					tooltipPosition += new Vector2(-(tooltipPosition.X - base.Size.X + base.EventManager.PageSize.X), 0f);
				}
			}
			if (base.Size.Y > base.EventManager.PageSize.Y)
			{
				verticalAlignment = VerticalAlignment.Top;
				tooltipPosition = new Vector2(tooltipPosition.X, 0f);
			}
			else
			{
				if (verticalAlignment == VerticalAlignment.Top && tooltipPosition.Y + base.Size.Y > base.EventManager.PageSize.Y)
				{
					tooltipPosition += new Vector2(0f, -(tooltipPosition.Y + base.Size.Y - base.EventManager.PageSize.Y));
				}
				if (verticalAlignment == VerticalAlignment.Bottom && tooltipPosition.Y - base.Size.Y + base.EventManager.PageSize.Y < 0f)
				{
					tooltipPosition += new Vector2(0f, -(tooltipPosition.Y - base.Size.Y + base.EventManager.PageSize.Y));
				}
			}
			base.HorizontalAlignment = horizontalAlignment;
			base.VerticalAlignment = verticalAlignment;
			base.ScaledPositionXOffset = tooltipPosition.X - base.EventManager.LeftUsableAreaStart;
			base.ScaledPositionYOffset = tooltipPosition.Y - base.EventManager.TopUsableAreaStart;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00007324 File Offset: 0x00005524
		private void ResetAnimationProperties()
		{
			this._animationState = TooltipWidget.AnimationState.NotStarted;
			this._animationProgress = 0f;
			this._animationDelayTimerInFrames = 0;
			this.SetGlobalAlphaRecursively(0f);
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000141 RID: 321 RVA: 0x0000734A File Offset: 0x0000554A
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00007352 File Offset: 0x00005552
		[Editor(false)]
		public float AnimTime
		{
			get
			{
				return this._animTime;
			}
			set
			{
				if (this._animTime != value)
				{
					this._animTime = value;
					base.OnPropertyChanged(value, "AnimTime");
				}
			}
		}

		// Token: 0x04000098 RID: 152
		protected int _animationDelayInFrames;

		// Token: 0x04000099 RID: 153
		private int _animationDelayTimerInFrames;

		// Token: 0x0400009A RID: 154
		private TooltipWidget.AnimationState _animationState;

		// Token: 0x0400009B RID: 155
		private float _animationProgress;

		// Token: 0x0400009C RID: 156
		private bool _lastCheckedVisibility;

		// Token: 0x0400009D RID: 157
		private Vector2 _lastCheckedSize;

		// Token: 0x0400009E RID: 158
		private float _animTime = 0.2f;

		// Token: 0x02000022 RID: 34
		private enum AnimationState
		{
			// Token: 0x040000D6 RID: 214
			NotStarted,
			// Token: 0x040000D7 RID: 215
			InProgress,
			// Token: 0x040000D8 RID: 216
			Finished
		}
	}
}
