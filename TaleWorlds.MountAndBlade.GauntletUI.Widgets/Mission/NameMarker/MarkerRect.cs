using System;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.NameMarker
{
	// Token: 0x020000F8 RID: 248
	public class MarkerRect
	{
		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x00023A53 File Offset: 0x00021C53
		// (set) Token: 0x06000D01 RID: 3329 RVA: 0x00023A5B File Offset: 0x00021C5B
		public float Left { get; private set; }

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00023A64 File Offset: 0x00021C64
		// (set) Token: 0x06000D03 RID: 3331 RVA: 0x00023A6C File Offset: 0x00021C6C
		public float Right { get; private set; }

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x00023A75 File Offset: 0x00021C75
		// (set) Token: 0x06000D05 RID: 3333 RVA: 0x00023A7D File Offset: 0x00021C7D
		public float Top { get; private set; }

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00023A86 File Offset: 0x00021C86
		// (set) Token: 0x06000D07 RID: 3335 RVA: 0x00023A8E File Offset: 0x00021C8E
		public float Bottom { get; private set; }

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x00023A97 File Offset: 0x00021C97
		public float CenterX
		{
			get
			{
				return this.Left + (this.Right - this.Left) / 2f;
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00023AB3 File Offset: 0x00021CB3
		public float CenterY
		{
			get
			{
				return this.Top + (this.Bottom - this.Top) / 2f;
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00023ACF File Offset: 0x00021CCF
		public float Width
		{
			get
			{
				return this.Right - this.Left;
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x00023ADE File Offset: 0x00021CDE
		public float Height
		{
			get
			{
				return this.Bottom - this.Top;
			}
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00023AED File Offset: 0x00021CED
		public MarkerRect()
		{
			this.Reset();
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00023AFB File Offset: 0x00021CFB
		public void Reset()
		{
			this.Left = 0f;
			this.Right = 0f;
			this.Top = 0f;
			this.Bottom = 0f;
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00023B29 File Offset: 0x00021D29
		public void UpdatePoints(float left, float right, float top, float bottom)
		{
			this.Left = left;
			this.Right = right;
			this.Top = top;
			this.Bottom = bottom;
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00023B48 File Offset: 0x00021D48
		public bool IsOverlapping(MarkerRect other)
		{
			return other.Left <= this.Right && other.Right >= this.Left && other.Top <= this.Bottom && other.Bottom >= this.Top;
		}
	}
}
