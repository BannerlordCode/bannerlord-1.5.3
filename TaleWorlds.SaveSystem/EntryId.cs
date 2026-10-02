using System;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x0200001D RID: 29
	public struct EntryId : IEquatable<EntryId>
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000048CC File Offset: 0x00002ACC
		// (set) Token: 0x06000099 RID: 153 RVA: 0x000048D4 File Offset: 0x00002AD4
		public int Id { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000048DD File Offset: 0x00002ADD
		// (set) Token: 0x0600009B RID: 155 RVA: 0x000048E5 File Offset: 0x00002AE5
		public SaveEntryExtension Extension { get; private set; }

		// Token: 0x0600009C RID: 156 RVA: 0x000048EE File Offset: 0x00002AEE
		public EntryId(int id, SaveEntryExtension extension)
		{
			this.Id = id;
			this.Extension = extension;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004900 File Offset: 0x00002B00
		public override bool Equals(object obj)
		{
			if (!(obj is EntryId))
			{
				return false;
			}
			EntryId entryId = (EntryId)obj;
			return entryId.Id == this.Id && entryId.Extension == this.Extension;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000493E File Offset: 0x00002B3E
		public bool Equals(EntryId other)
		{
			return other.Id == this.Id && other.Extension == this.Extension;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004960 File Offset: 0x00002B60
		public override int GetHashCode()
		{
			return (this.Id.GetHashCode() * 397) ^ ((int)this.Extension).GetHashCode();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004990 File Offset: 0x00002B90
		public static bool operator ==(EntryId a, EntryId b)
		{
			return a.Id == b.Id && a.Extension == b.Extension;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000049B4 File Offset: 0x00002BB4
		public static bool operator !=(EntryId a, EntryId b)
		{
			return !(a == b);
		}
	}
}
