using System;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x0200000B RID: 11
	public class InputData
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000078 RID: 120 RVA: 0x0000531F File Offset: 0x0000351F
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00005327 File Offset: 0x00003527
		public bool[] KeyData { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00005330 File Offset: 0x00003530
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00005338 File Offset: 0x00003538
		public bool LeftMouse { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00005341 File Offset: 0x00003541
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00005349 File Offset: 0x00003549
		public bool RightMouse { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00005352 File Offset: 0x00003552
		// (set) Token: 0x0600007F RID: 127 RVA: 0x0000535A File Offset: 0x0000355A
		public int CursorX { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00005363 File Offset: 0x00003563
		// (set) Token: 0x06000081 RID: 129 RVA: 0x0000536B File Offset: 0x0000356B
		public int CursorY { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00005374 File Offset: 0x00003574
		// (set) Token: 0x06000083 RID: 131 RVA: 0x0000537C File Offset: 0x0000357C
		public bool MouseMove { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00005385 File Offset: 0x00003585
		// (set) Token: 0x06000085 RID: 133 RVA: 0x0000538D File Offset: 0x0000358D
		public float MouseScrollDelta { get; set; }

		// Token: 0x06000086 RID: 134 RVA: 0x00005398 File Offset: 0x00003598
		public InputData()
		{
			this.KeyData = new bool[256];
			this.CursorX = 0;
			this.CursorY = 0;
			this.LeftMouse = false;
			this.RightMouse = false;
			this.MouseMove = false;
			this.MouseScrollDelta = 0f;
			for (int i = 0; i < 256; i++)
			{
				this.KeyData[i] = false;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005402 File Offset: 0x00003602
		public void Reset()
		{
			this.MouseScrollDelta = 0f;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005410 File Offset: 0x00003610
		public void FillFrom(InputData inputData)
		{
			this.CursorX = inputData.CursorX;
			this.CursorY = inputData.CursorY;
			this.LeftMouse = inputData.LeftMouse;
			this.RightMouse = inputData.RightMouse;
			this.MouseMove = inputData.MouseMove;
			this.MouseScrollDelta = inputData.MouseScrollDelta;
			for (int i = 0; i < 256; i++)
			{
				this.KeyData[i] = inputData.KeyData[i];
			}
		}
	}
}
