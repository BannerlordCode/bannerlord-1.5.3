using System;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x0200001C RID: 28
	public struct FolderId : IEquatable<FolderId>
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000047D8 File Offset: 0x000029D8
		// (set) Token: 0x0600008F RID: 143 RVA: 0x000047E0 File Offset: 0x000029E0
		public int LocalId { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000047E9 File Offset: 0x000029E9
		// (set) Token: 0x06000091 RID: 145 RVA: 0x000047F1 File Offset: 0x000029F1
		public SaveFolderExtension Extension { get; private set; }

		// Token: 0x06000092 RID: 146 RVA: 0x000047FA File Offset: 0x000029FA
		public FolderId(int localId, SaveFolderExtension extension)
		{
			this.LocalId = localId;
			this.Extension = extension;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000480C File Offset: 0x00002A0C
		public override bool Equals(object obj)
		{
			if (!(obj is FolderId))
			{
				return false;
			}
			FolderId folderId = (FolderId)obj;
			return folderId.LocalId == this.LocalId && folderId.Extension == this.Extension;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000484A File Offset: 0x00002A4A
		public bool Equals(FolderId other)
		{
			return other.LocalId == this.LocalId && other.Extension == this.Extension;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000486C File Offset: 0x00002A6C
		public override int GetHashCode()
		{
			return (this.LocalId.GetHashCode() * 397) ^ ((int)this.Extension).GetHashCode();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000489C File Offset: 0x00002A9C
		public static bool operator ==(FolderId a, FolderId b)
		{
			return a.LocalId == b.LocalId && a.Extension == b.Extension;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000048C0 File Offset: 0x00002AC0
		public static bool operator !=(FolderId a, FolderId b)
		{
			return !(a == b);
		}
	}
}
