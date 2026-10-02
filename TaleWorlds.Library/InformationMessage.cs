using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200003C RID: 60
	public class InformationMessage
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x000077DA File Offset: 0x000059DA
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x000077E2 File Offset: 0x000059E2
		public string Information { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x000077EB File Offset: 0x000059EB
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x000077F3 File Offset: 0x000059F3
		public string Detail { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x000077FC File Offset: 0x000059FC
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x00007804 File Offset: 0x00005A04
		public Color Color { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000780D File Offset: 0x00005A0D
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x00007815 File Offset: 0x00005A15
		public string SoundEventPath { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000781E File Offset: 0x00005A1E
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x00007826 File Offset: 0x00005A26
		public string Category { get; set; }

		// Token: 0x060001FA RID: 506 RVA: 0x0000782F File Offset: 0x00005A2F
		public InformationMessage(string information)
		{
			this.Information = information;
			this.Color = Color.White;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00007849 File Offset: 0x00005A49
		public InformationMessage(string information, Color color)
		{
			this.Information = information;
			this.Color = color;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000785F File Offset: 0x00005A5F
		public InformationMessage(string information, Color color, string category)
		{
			this.Information = information;
			this.Color = color;
			this.Category = category;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000787C File Offset: 0x00005A7C
		public InformationMessage(string information, string soundEventPath)
		{
			this.Information = information;
			this.SoundEventPath = soundEventPath;
			this.Color = Color.White;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000789D File Offset: 0x00005A9D
		public InformationMessage()
		{
			this.Information = "";
		}
	}
}
