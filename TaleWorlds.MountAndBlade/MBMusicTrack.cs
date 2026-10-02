using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001DC RID: 476
	public struct MBMusicTrack
	{
		// Token: 0x06001C76 RID: 7286 RVA: 0x00061E48 File Offset: 0x00060048
		public MBMusicTrack(MBMusicTrack obj)
		{
			this.index = obj.index;
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x00061E56 File Offset: 0x00060056
		internal MBMusicTrack(int i)
		{
			this.index = i;
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001C78 RID: 7288 RVA: 0x00061E5F File Offset: 0x0006005F
		private bool IsValid
		{
			get
			{
				return this.index >= 0;
			}
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x00061E6D File Offset: 0x0006006D
		public bool Equals(MBMusicTrack obj)
		{
			return this.index == obj.index;
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x00061E7D File Offset: 0x0006007D
		public override int GetHashCode()
		{
			return this.index;
		}

		// Token: 0x0400098F RID: 2447
		private int index;
	}
}
