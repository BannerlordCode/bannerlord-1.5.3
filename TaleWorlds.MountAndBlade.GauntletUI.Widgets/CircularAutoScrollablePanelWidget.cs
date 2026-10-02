using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200000D RID: 13
	public class CircularAutoScrollablePanelWidget : Widget
	{
		// Token: 0x06000092 RID: 146 RVA: 0x0000364C File Offset: 0x0000184C
		public CircularAutoScrollablePanelWidget(UIContext context)
			: base(context)
		{
			this.IdleTime = 0.8f;
			this.ScrollRatioPerSecond = 0.25f;
			this.ScrollPixelsPerSecond = 35f;
			this.ScrollType = CircularAutoScrollablePanelWidget.ScrollMovementType.ByPixels;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000368C File Offset: 0x0000188C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this._isScrolling = this._isScrolling || (base.CurrentState == "Selected" && this.AutoScrollWhenSelected) || this.AutoScroll;
			this._maxScroll = 0f;
			Widget innerPanel = this.InnerPanel;
			if (innerPanel != null && innerPanel.Size.Y > 0f)
			{
				Widget clipRect = this.ClipRect;
				if (clipRect != null && clipRect.Size.Y > 0f && this.InnerPanel.Size.Y > this.ClipRect.Size.Y)
				{
					this._maxScroll = this.InnerPanel.Size.Y - this.ClipRect.Size.Y;
				}
			}
			if (this._isScrolling && !this._isIdle)
			{
				this.ScrollToDirection(this._direction, dt);
				if (this._currentScrollValue.ApproximatelyEqualsTo(0f, 1E-05f) || this._currentScrollValue.ApproximatelyEqualsTo(this._maxScroll, 1E-05f))
				{
					this._isIdle = true;
					this._idleTimer = 0f;
					this._direction *= -1;
					return;
				}
			}
			else if (this._isScrolling && this._isIdle)
			{
				this.ScrollToDirection(0, dt);
				this._idleTimer += dt;
				if (this._idleTimer > this.IdleTime)
				{
					this._isIdle = false;
					this._idleTimer = 0f;
					return;
				}
			}
			else if (this._currentScrollValue > 0f)
			{
				this.ScrollToDirection(-1, dt);
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003830 File Offset: 0x00001A30
		private void ScrollToDirection(int direction, float dt)
		{
			float num = 0f;
			if (this.ScrollType == CircularAutoScrollablePanelWidget.ScrollMovementType.ByPixels)
			{
				num = this.ScrollPixelsPerSecond;
			}
			else if (this.ScrollType == CircularAutoScrollablePanelWidget.ScrollMovementType.ByRatio)
			{
				num = this.ScrollRatioPerSecond * this._maxScroll;
			}
			this._currentScrollValue += num * (float)direction * dt;
			this._currentScrollValue = MathF.Clamp(this._currentScrollValue, 0f, this._maxScroll);
			this.InnerPanel.ScaledPositionYOffset = -this._currentScrollValue;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000038AC File Offset: 0x00001AAC
		protected override void OnMouseScroll()
		{
			base.OnMouseScroll();
			if (!this.AutoScroll && (!this.AutoScrollWhenSelected || !(base.CurrentState == "Selected")))
			{
				this._isScrolling = false;
				float num = ((this.ScrollPixelsPerSecond != 0f) ? (this.ScrollPixelsPerSecond * 0.2f) : 10f);
				float num2 = base.EventManager.DeltaMouseScroll * num;
				this._currentScrollValue += num2;
				this.InnerPanel.ScaledPositionYOffset = -this._currentScrollValue;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003937 File Offset: 0x00001B37
		public void SetScrollMouse()
		{
			this.OnMouseScroll();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003940 File Offset: 0x00001B40
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
			if (!this.AutoScroll && (!this.AutoScrollWhenSelected || !(base.CurrentState == "Selected")) && !this._isScrolling)
			{
				this._isScrolling = true;
				this._isIdle = false;
				this._direction = 1;
				this._idleTimer = 0f;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000399D File Offset: 0x00001B9D
		public void SetHoverBegin()
		{
			this.OnHoverBegin();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000039A5 File Offset: 0x00001BA5
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
			if (!this.AutoScroll && (!this.AutoScrollWhenSelected || !(base.CurrentState == "Selected")) && this._isScrolling)
			{
				this.StopScrolling();
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000039DD File Offset: 0x00001BDD
		public void SetHoverEnd()
		{
			this.OnHoverEnd();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000039E8 File Offset: 0x00001BE8
		public override void SetState(string stateName)
		{
			base.SetState(stateName);
			if (this._isScrolling && !this.AutoScroll && !base.IsHovered && (!this.AutoScrollWhenSelected || !(base.CurrentState == "Selected")))
			{
				this.StopScrolling();
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003A34 File Offset: 0x00001C34
		protected void StopScrolling()
		{
			this._isScrolling = false;
			this._direction = -1;
			if (this._isIdle && this._currentScrollValue < 1E-45f)
			{
				this._currentScrollValue = 1f;
			}
			if (this.ShouldResetImmediately)
			{
				this._currentScrollValue = 0f;
				this.InnerPanel.ScaledPositionYOffset = 0f;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00003A92 File Offset: 0x00001C92
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00003A9A File Offset: 0x00001C9A
		public Widget InnerPanel { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00003AA3 File Offset: 0x00001CA3
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00003AAB File Offset: 0x00001CAB
		public Widget ClipRect { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00003AB4 File Offset: 0x00001CB4
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003ABC File Offset: 0x00001CBC
		public float ScrollRatioPerSecond { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003AC5 File Offset: 0x00001CC5
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003ACD File Offset: 0x00001CCD
		public float ScrollPixelsPerSecond { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003AD6 File Offset: 0x00001CD6
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00003ADE File Offset: 0x00001CDE
		public float IdleTime { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00003AE7 File Offset: 0x00001CE7
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00003AEF File Offset: 0x00001CEF
		public bool AutoScrollWhenSelected { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003AF8 File Offset: 0x00001CF8
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00003B00 File Offset: 0x00001D00
		public bool AutoScroll { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003B09 File Offset: 0x00001D09
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00003B11 File Offset: 0x00001D11
		public CircularAutoScrollablePanelWidget.ScrollMovementType ScrollType { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003B1A File Offset: 0x00001D1A
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00003B22 File Offset: 0x00001D22
		public bool ShouldResetImmediately
		{
			get
			{
				return this._shouldResetImmediately;
			}
			set
			{
				if (value != this._shouldResetImmediately)
				{
					this._shouldResetImmediately = value;
				}
			}
		}

		// Token: 0x04000043 RID: 67
		private float _currentScrollValue;

		// Token: 0x04000044 RID: 68
		private bool _isScrolling;

		// Token: 0x04000045 RID: 69
		private bool _isIdle;

		// Token: 0x04000046 RID: 70
		private float _idleTimer;

		// Token: 0x04000047 RID: 71
		private int _direction = 1;

		// Token: 0x04000048 RID: 72
		private float _maxScroll;

		// Token: 0x04000049 RID: 73
		private bool _shouldResetImmediately = true;

		// Token: 0x0200019B RID: 411
		public enum ScrollMovementType
		{
			// Token: 0x040009BB RID: 2491
			ByPixels,
			// Token: 0x040009BC RID: 2492
			ByRatio
		}
	}
}
