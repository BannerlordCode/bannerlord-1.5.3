using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E3 RID: 483
	public struct MBSoundTrack
	{
		// Token: 0x06001C92 RID: 7314 RVA: 0x000620C5 File Offset: 0x000602C5
		internal MBSoundTrack(int i)
		{
			this.index = i;
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x000620CE File Offset: 0x000602CE
		public bool Equals(MBSoundTrack a)
		{
			return this.index == a.index;
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x000620DE File Offset: 0x000602DE
		public override int GetHashCode()
		{
			return this.index;
		}

		// Token: 0x040009A7 RID: 2471
		private int index;
	}
}
