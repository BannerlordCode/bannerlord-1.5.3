using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Launcher.Library.CustomWidgets
{
	// Token: 0x02000023 RID: 35
	public class LauncherHintWidget : Widget
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000162 RID: 354 RVA: 0x000063D6 File Offset: 0x000045D6
		private float TooltipOffset
		{
			get
			{
				return 30f;
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000063DD File Offset: 0x000045DD
		public LauncherHintWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000063E6 File Offset: 0x000045E6
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.UpdateAlpha(dt);
			if (this.IsActive)
			{
				if (!this._prevIsActive)
				{
					this._frame = 0;
				}
				this.UpdatePosition();
			}
			this._prevIsActive = this.IsActive;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00006420 File Offset: 0x00004620
		private void UpdatePosition()
		{
			Vector2 vector;
			if (this._frame < 3)
			{
				this._tooltipPosition = base.EventManager.MousePosition;
				vector = new Vector2(-2000f, -2000f);
			}
			else
			{
				vector = this._tooltipPosition;
			}
			this._frame++;
			float num = 0f;
			HorizontalAlignment horizontalAlignment;
			if ((double)vector.X < (double)base.EventManager.PageSize.X * 0.5)
			{
				horizontalAlignment = HorizontalAlignment.Left;
				num = this.TooltipOffset;
			}
			else
			{
				horizontalAlignment = HorizontalAlignment.Right;
				num -= 0f;
				vector = new Vector2(-(base.EventManager.PageSize.X - vector.X), vector.Y);
			}
			VerticalAlignment verticalAlignment;
			float num2;
			if ((double)vector.Y < (double)base.EventManager.PageSize.Y * 0.5)
			{
				verticalAlignment = VerticalAlignment.Top;
				num2 = this.TooltipOffset;
			}
			else
			{
				verticalAlignment = VerticalAlignment.Bottom;
				num2 = 0f;
				vector = new Vector2(vector.X, -(base.EventManager.PageSize.Y - vector.Y));
			}
			vector += new Vector2(num, num2);
			if (this._frame > 3)
			{
				if (base.Size.Y > base.EventManager.PageSize.Y)
				{
					verticalAlignment = VerticalAlignment.Center;
					vector = new Vector2(vector.X, 0f);
				}
				else
				{
					if (verticalAlignment == VerticalAlignment.Top && vector.Y + base.Size.Y > base.EventManager.PageSize.Y)
					{
						vector += new Vector2(0f, -(vector.Y + base.Size.Y - base.EventManager.PageSize.Y));
					}
					if (verticalAlignment == VerticalAlignment.Bottom && vector.Y - base.Size.Y + base.EventManager.PageSize.Y < 0f)
					{
						vector += new Vector2(0f, -(vector.Y - base.Size.Y + base.EventManager.PageSize.Y));
					}
				}
			}
			base.HorizontalAlignment = horizontalAlignment;
			base.VerticalAlignment = verticalAlignment;
			base.ScaledPositionXOffset = vector.X;
			base.ScaledPositionYOffset = vector.Y;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006674 File Offset: 0x00004874
		private void UpdateAlpha(float dt)
		{
			float num;
			if (this.IsActive)
			{
				num = 1f;
			}
			else
			{
				num = 0f;
			}
			float num2 = MathF.Lerp(base.AlphaFactor, num, dt * 20f, 1E-05f);
			this.SetGlobalAlphaRecursively(num2);
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000167 RID: 359 RVA: 0x000066B7 File Offset: 0x000048B7
		// (set) Token: 0x06000168 RID: 360 RVA: 0x000066BF File Offset: 0x000048BF
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChanged(value, "IsActive");
				}
			}
		}

		// Token: 0x040000A9 RID: 169
		private int _frame;

		// Token: 0x040000AA RID: 170
		private bool _prevIsActive;

		// Token: 0x040000AB RID: 171
		private Vector2 _tooltipPosition;

		// Token: 0x040000AC RID: 172
		private bool _isActive;
	}
}
