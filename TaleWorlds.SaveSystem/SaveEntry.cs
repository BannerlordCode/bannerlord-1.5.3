using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x0200001F RID: 31
	public class SaveEntry
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000049C0 File Offset: 0x00002BC0
		public byte[] Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000049C8 File Offset: 0x00002BC8
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x000049D0 File Offset: 0x00002BD0
		public EntryId Id { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000049D9 File Offset: 0x00002BD9
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x000049E1 File Offset: 0x00002BE1
		public int FolderId { get; private set; }

		// Token: 0x060000A8 RID: 168 RVA: 0x000049F2 File Offset: 0x00002BF2
		public static SaveEntry CreateFrom(int entryFolderId, EntryId entryId, byte[] data)
		{
			return new SaveEntry
			{
				FolderId = entryFolderId,
				Id = entryId,
				_data = data
			};
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004A0E File Offset: 0x00002C0E
		public static SaveEntry CreateNew(SaveEntryFolder parentFolder, EntryId entryId)
		{
			return new SaveEntry
			{
				Id = entryId,
				FolderId = parentFolder.GlobalId
			};
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004A28 File Offset: 0x00002C28
		public BinaryReader GetBinaryReader()
		{
			return new BinaryReader(this._data);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004A35 File Offset: 0x00002C35
		public void FillFrom(BinaryWriter writer)
		{
			this._data = writer.GetFinalData();
		}

		// Token: 0x04000044 RID: 68
		private byte[] _data;
	}
}
