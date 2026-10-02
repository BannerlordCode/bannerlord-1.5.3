using System;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000030 RID: 48
	public class AudioProperty
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000F0E8 File Offset: 0x0000D2E8
		// (set) Token: 0x0600035A RID: 858 RVA: 0x0000F0F0 File Offset: 0x0000D2F0
		[Editor(false)]
		public string AudioName { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000F0F9 File Offset: 0x0000D2F9
		// (set) Token: 0x0600035C RID: 860 RVA: 0x0000F101 File Offset: 0x0000D301
		[Editor(false)]
		public bool Delay { get; set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000F10A File Offset: 0x0000D30A
		// (set) Token: 0x0600035E RID: 862 RVA: 0x0000F112 File Offset: 0x0000D312
		[Editor(false)]
		public float DelaySeconds { get; set; }

		// Token: 0x0600035F RID: 863 RVA: 0x0000F11B File Offset: 0x0000D31B
		public void FillFrom(AudioProperty audioProperty)
		{
			this.AudioName = audioProperty.AudioName;
			this.Delay = audioProperty.Delay;
			this.DelaySeconds = audioProperty.DelaySeconds;
		}
	}
}
