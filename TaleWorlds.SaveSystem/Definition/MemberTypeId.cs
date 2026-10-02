using System;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000066 RID: 102
	public struct MemberTypeId
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0000EDF9 File Offset: 0x0000CFF9
		public short SaveId
		{
			get
			{
				return (short)(this.TypeLevel << 8) + this.LocalSaveId;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000364 RID: 868 RVA: 0x0000EE0C File Offset: 0x0000D00C
		public static MemberTypeId Invalid
		{
			get
			{
				return new MemberTypeId(0, -1);
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000EE18 File Offset: 0x0000D018
		public override string ToString()
		{
			return string.Concat(new object[] { "(", this.TypeLevel, ",", this.LocalSaveId, ")" });
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000EE64 File Offset: 0x0000D064
		public MemberTypeId(byte typeLevel, short localSaveId)
		{
			this.TypeLevel = typeLevel;
			this.LocalSaveId = localSaveId;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000EE74 File Offset: 0x0000D074
		public override bool Equals(object obj)
		{
			if (obj is MemberTypeId)
			{
				MemberTypeId memberTypeId = (MemberTypeId)obj;
				return memberTypeId.TypeLevel == this.TypeLevel && memberTypeId.LocalSaveId == this.LocalSaveId;
			}
			return false;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000EEB2 File Offset: 0x0000D0B2
		public static bool operator ==(MemberTypeId m1, MemberTypeId m2)
		{
			if (m1 == null)
			{
				return m2 == null;
			}
			return m1.Equals(m2);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000EED9 File Offset: 0x0000D0D9
		public static bool operator !=(MemberTypeId m1, MemberTypeId m2)
		{
			return !(m1 == m2);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000EEE5 File Offset: 0x0000D0E5
		public override int GetHashCode()
		{
			return (int)((short)((17 * 31 + this.TypeLevel) * 31) + this.LocalSaveId);
		}

		// Token: 0x04000102 RID: 258
		public byte TypeLevel;

		// Token: 0x04000103 RID: 259
		public short LocalSaveId;
	}
}
