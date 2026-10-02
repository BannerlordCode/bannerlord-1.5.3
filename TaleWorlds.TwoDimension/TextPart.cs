using System;
using System.Numerics;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000012 RID: 18
	public class TextPart
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00006090 File Offset: 0x00004290
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00006098 File Offset: 0x00004298
		internal TextMeshGenerator TextMeshGenerator { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x000060A1 File Offset: 0x000042A1
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x000060A9 File Offset: 0x000042A9
		public TextDrawObject DrawObject2D { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000060B2 File Offset: 0x000042B2
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x000060BA File Offset: 0x000042BA
		public Font DefaultFont { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x000060C3 File Offset: 0x000042C3
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x000060CB File Offset: 0x000042CB
		public float WordWidth { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000060D4 File Offset: 0x000042D4
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x000060DC File Offset: 0x000042DC
		public Vector2 PartPosition { get; set; }

		// Token: 0x060000CA RID: 202 RVA: 0x000060E5 File Offset: 0x000042E5
		internal TextPart()
		{
		}
	}
}
