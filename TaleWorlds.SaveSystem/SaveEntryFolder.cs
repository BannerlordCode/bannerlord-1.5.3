using System;
using System.Collections.Generic;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000020 RID: 32
	public class SaveEntryFolder
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004A43 File Offset: 0x00002C43
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00004A4B File Offset: 0x00002C4B
		public int GlobalId { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00004A54 File Offset: 0x00002C54
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00004A5C File Offset: 0x00002C5C
		public int ParentGlobalId { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004A65 File Offset: 0x00002C65
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00004A6D File Offset: 0x00002C6D
		public FolderId FolderId { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004A76 File Offset: 0x00002C76
		public Dictionary<EntryId, SaveEntry>.ValueCollection ChildEntries
		{
			get
			{
				return this._entries.Values;
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004A84 File Offset: 0x00002C84
		public List<SaveEntry> GetAllEntries()
		{
			List<SaveEntry> list = new List<SaveEntry>(this._entries.Values);
			foreach (SaveEntryFolder saveEntryFolder in this._saveEntryFolders.Values)
			{
				list.AddRange(saveEntryFolder.GetAllEntries());
			}
			return list;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004AF4 File Offset: 0x00002CF4
		public static SaveEntryFolder CreateRootFolder()
		{
			return new SaveEntryFolder(-1, -1, new FolderId(-1, SaveFolderExtension.Root), 3);
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00004B05 File Offset: 0x00002D05
		public Dictionary<FolderId, SaveEntryFolder>.ValueCollection ChildFolders
		{
			get
			{
				return this._saveEntryFolders.Values;
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004B12 File Offset: 0x00002D12
		public SaveEntryFolder(SaveEntryFolder parent, int globalId, FolderId folderId, int entryCount)
			: this(parent.GlobalId, globalId, folderId, entryCount)
		{
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004B24 File Offset: 0x00002D24
		public SaveEntryFolder(int parentGlobalId, int globalId, FolderId folderId, int entryCount)
		{
			this.ParentGlobalId = parentGlobalId;
			this.GlobalId = globalId;
			this.FolderId = folderId;
			this._entries = new Dictionary<EntryId, SaveEntry>(entryCount);
			this._saveEntryFolders = new Dictionary<FolderId, SaveEntryFolder>();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004B59 File Offset: 0x00002D59
		public void AddEntry(SaveEntry saveEntry)
		{
			this._entries.Add(saveEntry.Id, saveEntry);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004B6D File Offset: 0x00002D6D
		public SaveEntry GetEntry(EntryId entryId)
		{
			return this._entries[entryId];
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004B7B File Offset: 0x00002D7B
		public void AddChildFolderEntry(SaveEntryFolder saveEntryFolder)
		{
			this._saveEntryFolders.Add(saveEntryFolder.FolderId, saveEntryFolder);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004B8F File Offset: 0x00002D8F
		internal SaveEntryFolder GetChildFolder(FolderId folderId)
		{
			return this._saveEntryFolders[folderId];
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004BA0 File Offset: 0x00002DA0
		public SaveEntry CreateEntry(EntryId entryId)
		{
			SaveEntry saveEntry = SaveEntry.CreateNew(this, entryId);
			this.AddEntry(saveEntry);
			return saveEntry;
		}

		// Token: 0x0400004A RID: 74
		private Dictionary<FolderId, SaveEntryFolder> _saveEntryFolders;

		// Token: 0x0400004B RID: 75
		private Dictionary<EntryId, SaveEntry> _entries;
	}
}
