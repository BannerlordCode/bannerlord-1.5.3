using System;
using TaleWorlds.Library;

namespace TaleWorlds.InputSystem
{
	// Token: 0x0200000F RID: 15
	public class InputState
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00005330 File Offset: 0x00003530
		public Vec2 NativeResolution
		{
			get
			{
				return Input.Resolution;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00005337 File Offset: 0x00003537
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00005340 File Offset: 0x00003540
		public Vec2 MousePositionRanged
		{
			get
			{
				return this._mousePositionRanged;
			}
			set
			{
				this._mousePositionRanged = value;
				this._mousePositionPixel = new Vec2(this._mousePositionRanged.x * this.NativeResolution.x, this._mousePositionRanged.y * this.NativeResolution.y);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600017A RID: 378 RVA: 0x0000538D File Offset: 0x0000358D
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00005395 File Offset: 0x00003595
		public Vec2 OldMousePositionRanged { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0000539E File Offset: 0x0000359E
		// (set) Token: 0x0600017D RID: 381 RVA: 0x000053A6 File Offset: 0x000035A6
		public bool MousePositionChanged { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600017E RID: 382 RVA: 0x000053AF File Offset: 0x000035AF
		// (set) Token: 0x0600017F RID: 383 RVA: 0x000053B8 File Offset: 0x000035B8
		public Vec2 MousePositionPixel
		{
			get
			{
				return this._mousePositionPixel;
			}
			set
			{
				this._mousePositionPixel = value;
				this._mousePositionRanged = new Vec2(this._mousePositionPixel.x / Input.Resolution.x, this._mousePositionPixel.y / this.NativeResolution.y);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00005404 File Offset: 0x00003604
		// (set) Token: 0x06000181 RID: 385 RVA: 0x0000540C File Offset: 0x0000360C
		public Vec2 OldMousePositionPixel { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00005415 File Offset: 0x00003615
		// (set) Token: 0x06000183 RID: 387 RVA: 0x0000541D File Offset: 0x0000361D
		public float MouseScrollValue { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00005426 File Offset: 0x00003626
		// (set) Token: 0x06000185 RID: 389 RVA: 0x0000542E File Offset: 0x0000362E
		public bool MouseScrollChanged { get; private set; }

		// Token: 0x06000186 RID: 390 RVA: 0x00005438 File Offset: 0x00003638
		public InputState()
		{
			this.MousePositionRanged = default(Vec2);
			this.OldMousePositionRanged = default(Vec2);
			this.MousePositionPixel = default(Vec2);
			this.OldMousePositionPixel = default(Vec2);
			this._mousePositionRanged = new Vec2(0f, 0f);
			this._mousePositionPixel = new Vec2(0f, 0f);
			this._mousePositionPixelDevice = new Vec2(0f, 0f);
			this._mousePositionRangedDevice = new Vec2(0f, 0f);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000054DC File Offset: 0x000036DC
		public bool UpdateMousePosition(float mousePositionX, float mousePositionY)
		{
			this.OldMousePositionRanged = new Vec2(this._mousePositionRangedDevice.x, this._mousePositionRangedDevice.y);
			this._mousePositionRangedDevice = new Vec2(mousePositionX, mousePositionY);
			this.OldMousePositionPixel = new Vec2(this._mousePositionPixelDevice.x, this._mousePositionPixelDevice.y);
			this._mousePositionPixelDevice = new Vec2(this._mousePositionRangedDevice.x * this.NativeResolution.x, this._mousePositionRangedDevice.y * this.NativeResolution.y);
			if (this._mousePositionRangedDevice.x == this.OldMousePositionRanged.x && this._mousePositionRangedDevice.y == this.OldMousePositionRanged.y)
			{
				this.MousePositionChanged = false;
			}
			else
			{
				this.MousePositionChanged = true;
				this.MousePositionPixel = this._mousePositionPixelDevice;
				this.MousePositionRanged = this._mousePositionRangedDevice;
			}
			return this.MousePositionChanged;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000055D0 File Offset: 0x000037D0
		public bool UpdateMouseScroll(float mouseScrollValue)
		{
			if (!this.MouseScrollValue.Equals(mouseScrollValue))
			{
				this.MouseScrollValue = mouseScrollValue;
				this.MouseScrollChanged = true;
			}
			else
			{
				this.MouseScrollChanged = false;
			}
			return this.MouseScrollChanged;
		}

		// Token: 0x04000147 RID: 327
		private Vec2 _mousePositionRanged;

		// Token: 0x04000149 RID: 329
		private Vec2 _mousePositionRangedDevice;

		// Token: 0x0400014B RID: 331
		private Vec2 _mousePositionPixel;

		// Token: 0x0400014C RID: 332
		private Vec2 _mousePositionPixelDevice;
	}
}
