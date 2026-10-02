using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000260 RID: 608
	public struct MissionObjectId
	{
		// Token: 0x060022C4 RID: 8900 RVA: 0x0007AAC6 File Offset: 0x00078CC6
		public MissionObjectId(int id, bool createdAtRuntime = false)
		{
			this.Id = id;
			this.CreatedAtRuntime = createdAtRuntime;
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x0007AAD6 File Offset: 0x00078CD6
		public static bool operator ==(MissionObjectId a, MissionObjectId b)
		{
			return a.Id == b.Id && a.CreatedAtRuntime == b.CreatedAtRuntime;
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x0007AAF6 File Offset: 0x00078CF6
		public static bool operator !=(MissionObjectId a, MissionObjectId b)
		{
			return a.Id != b.Id || a.CreatedAtRuntime != b.CreatedAtRuntime;
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x0007AB1C File Offset: 0x00078D1C
		public override bool Equals(object obj)
		{
			if (!(obj is MissionObjectId))
			{
				return false;
			}
			MissionObjectId missionObjectId = (MissionObjectId)obj;
			return missionObjectId.Id == this.Id && missionObjectId.CreatedAtRuntime == this.CreatedAtRuntime;
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x0007AB58 File Offset: 0x00078D58
		public override int GetHashCode()
		{
			int num = this.Id;
			if (this.CreatedAtRuntime)
			{
				num |= 1073741824;
			}
			return num.GetHashCode();
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x0007AB84 File Offset: 0x00078D84
		public override string ToString()
		{
			return this.Id + " - " + this.CreatedAtRuntime.ToString();
		}

		// Token: 0x04000D81 RID: 3457
		public readonly int Id;

		// Token: 0x04000D82 RID: 3458
		public readonly bool CreatedAtRuntime;

		// Token: 0x04000D83 RID: 3459
		public static readonly MissionObjectId Invalid = new MissionObjectId(-1, false);
	}
}
