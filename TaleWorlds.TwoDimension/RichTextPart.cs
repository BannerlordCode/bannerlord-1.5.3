using System;
using System.Numerics;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000009 RID: 9
	public class RichTextPart
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00004820 File Offset: 0x00002A20
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00004828 File Offset: 0x00002A28
		public string Style { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00004831 File Offset: 0x00002A31
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00004839 File Offset: 0x00002A39
		internal TextMeshGenerator TextMeshGenerator { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00004842 File Offset: 0x00002A42
		// (set) Token: 0x06000068 RID: 104 RVA: 0x0000484A File Offset: 0x00002A4A
		public ImageDrawObject ImageDrawObject { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00004853 File Offset: 0x00002A53
		// (set) Token: 0x0600006A RID: 106 RVA: 0x0000485B File Offset: 0x00002A5B
		public TextDrawObject TextDrawObject { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00004864 File Offset: 0x00002A64
		// (set) Token: 0x0600006C RID: 108 RVA: 0x0000486C File Offset: 0x00002A6C
		public Font DefaultFont { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00004875 File Offset: 0x00002A75
		// (set) Token: 0x0600006E RID: 110 RVA: 0x0000487D File Offset: 0x00002A7D
		public float WordWidth { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00004886 File Offset: 0x00002A86
		// (set) Token: 0x06000070 RID: 112 RVA: 0x0000488E File Offset: 0x00002A8E
		public Vector2 PartPosition { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00004897 File Offset: 0x00002A97
		// (set) Token: 0x06000072 RID: 114 RVA: 0x0000489F File Offset: 0x00002A9F
		public Sprite Sprite { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000073 RID: 115 RVA: 0x000048A8 File Offset: 0x00002AA8
		// (set) Token: 0x06000074 RID: 116 RVA: 0x000048B0 File Offset: 0x00002AB0
		public Vector2 SpritePosition { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000048B9 File Offset: 0x00002AB9
		// (set) Token: 0x06000076 RID: 118 RVA: 0x000048C1 File Offset: 0x00002AC1
		public RichTextPartType Type { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000077 RID: 119 RVA: 0x000048CA File Offset: 0x00002ACA
		// (set) Token: 0x06000078 RID: 120 RVA: 0x000048D2 File Offset: 0x00002AD2
		public float Extend { get; set; }

		// Token: 0x06000079 RID: 121 RVA: 0x000048DB File Offset: 0x00002ADB
		internal RichTextPart()
		{
			this.Style = "Default";
		}
	}
}
