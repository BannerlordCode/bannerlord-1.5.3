using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000016 RID: 22
	internal class ArchiveDeserializer
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000040BD File Offset: 0x000022BD
		// (set) Token: 0x0600007F RID: 127 RVA: 0x000040C5 File Offset: 0x000022C5
		public SaveEntryFolder RootFolder { get; private set; }

		// Token: 0x06000080 RID: 128 RVA: 0x000040CE File Offset: 0x000022CE
		public ArchiveDeserializer()
		{
			this.RootFolder = new SaveEntryFolder(-1, -1, new FolderId(-1, SaveFolderExtension.Root), 3);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000040EC File Offset: 0x000022EC
		public void LoadFrom(byte[] binaryArchive)
		{
			Dictionary<int, SaveEntryFolder> dictionary = new Dictionary<int, SaveEntryFolder>();
			List<SaveEntry> list = new List<SaveEntry>();
			BinaryReader binaryReader = new BinaryReader(binaryArchive);
			int num = binaryReader.ReadInt();
			for (int i = 0; i < num; i++)
			{
				int num2 = binaryReader.Read3ByteInt();
				int num3 = binaryReader.Read3ByteInt();
				int num4 = binaryReader.Read3ByteInt();
				SaveFolderExtension saveFolderExtension = (SaveFolderExtension)binaryReader.ReadByte();
				FolderId folderId = new FolderId(num4, saveFolderExtension);
				SaveEntryFolder saveEntryFolder = new SaveEntryFolder(num2, num3, folderId, 3);
				dictionary.Add(saveEntryFolder.GlobalId, saveEntryFolder);
			}
			int num5 = binaryReader.ReadInt();
			for (int j = 0; j < num5; j++)
			{
				int num6 = binaryReader.Read3ByteInt();
				int num7 = binaryReader.Read3ByteInt();
				SaveEntryExtension saveEntryExtension = (SaveEntryExtension)binaryReader.ReadByte();
				short num8 = binaryReader.ReadShort();
				byte[] array = binaryReader.ReadBytes((int)num8);
				SaveEntry saveEntry = SaveEntry.CreateFrom(num6, new EntryId(num7, saveEntryExtension), array);
				list.Add(saveEntry);
			}
			foreach (SaveEntryFolder saveEntryFolder2 in dictionary.Values)
			{
				if (saveEntryFolder2.ParentGlobalId != -1)
				{
					dictionary[saveEntryFolder2.ParentGlobalId].AddChildFolderEntry(saveEntryFolder2);
				}
				else
				{
					this.RootFolder.AddChildFolderEntry(saveEntryFolder2);
				}
			}
			foreach (SaveEntry saveEntry2 in list)
			{
				if (saveEntry2.FolderId != -1)
				{
					dictionary[saveEntry2.FolderId].AddEntry(saveEntry2);
				}
				else
				{
					this.RootFolder.AddEntry(saveEntry2);
				}
			}
		}
	}
}
