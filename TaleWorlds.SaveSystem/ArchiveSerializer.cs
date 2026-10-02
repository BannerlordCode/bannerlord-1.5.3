using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000019 RID: 25
	internal class ArchiveSerializer : IArchiveContext
	{
		// Token: 0x06000088 RID: 136 RVA: 0x00004524 File Offset: 0x00002724
		public ArchiveSerializer()
		{
			this._writer = BinaryWriterFactory.GetBinaryWriter();
			this._folders = new List<SaveEntryFolder>();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004544 File Offset: 0x00002744
		public void SerializeEntry(SaveEntry entry)
		{
			this._writer.Write3ByteInt(entry.FolderId);
			this._writer.Write3ByteInt(entry.Id.Id);
			this._writer.WriteByte((byte)entry.Id.Extension);
			this._writer.WriteShort((short)entry.Data.Length);
			this._writer.WriteBytes(entry.Data);
			this._entryCount++;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000045C8 File Offset: 0x000027C8
		public void SerializeFolder(SaveEntryFolder folder)
		{
			foreach (SaveEntry saveEntry in folder.GetAllEntries())
			{
				this.SerializeEntry(saveEntry);
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000461C File Offset: 0x0000281C
		public SaveEntryFolder CreateFolder(SaveEntryFolder parentFolder, FolderId folderId, int entryCount)
		{
			int folderCount = this._folderCount;
			this._folderCount++;
			SaveEntryFolder saveEntryFolder = new SaveEntryFolder(parentFolder, folderCount, folderId, entryCount);
			parentFolder.AddChildFolderEntry(saveEntryFolder);
			this._folders.Add(saveEntryFolder);
			return saveEntryFolder;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000465C File Offset: 0x0000285C
		public byte[] FinalizeAndGetBinaryData()
		{
			BinaryWriter binaryWriter = BinaryWriterFactory.GetBinaryWriter();
			binaryWriter.WriteInt(this._folderCount);
			for (int i = 0; i < this._folderCount; i++)
			{
				SaveEntryFolder saveEntryFolder = this._folders[i];
				int parentGlobalId = saveEntryFolder.ParentGlobalId;
				int globalId = saveEntryFolder.GlobalId;
				int localId = saveEntryFolder.FolderId.LocalId;
				SaveFolderExtension extension = saveEntryFolder.FolderId.Extension;
				binaryWriter.Write3ByteInt(parentGlobalId);
				binaryWriter.Write3ByteInt(globalId);
				binaryWriter.Write3ByteInt(localId);
				binaryWriter.WriteByte((byte)extension);
			}
			binaryWriter.WriteInt(this._entryCount);
			binaryWriter.AppendData(this._writer);
			byte[] finalData = binaryWriter.GetFinalData();
			BinaryWriterFactory.ReleaseBinaryWriter(binaryWriter);
			BinaryWriterFactory.ReleaseBinaryWriter(this._writer);
			this._writer = null;
			return finalData;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004720 File Offset: 0x00002920
		public byte[] GetBinaryDataDebug()
		{
			BinaryWriter binaryWriter = BinaryWriterFactory.GetBinaryWriter();
			binaryWriter.WriteInt(this._folderCount);
			for (int i = 0; i < this._folderCount; i++)
			{
				SaveEntryFolder saveEntryFolder = this._folders[i];
				int parentGlobalId = saveEntryFolder.ParentGlobalId;
				int globalId = saveEntryFolder.GlobalId;
				int localId = saveEntryFolder.FolderId.LocalId;
				SaveFolderExtension extension = saveEntryFolder.FolderId.Extension;
				binaryWriter.Write3ByteInt(parentGlobalId);
				binaryWriter.Write3ByteInt(globalId);
				binaryWriter.Write3ByteInt(localId);
				binaryWriter.WriteByte((byte)extension);
			}
			binaryWriter.WriteInt(this._entryCount);
			binaryWriter.AppendData(this._writer);
			byte[] finalData = binaryWriter.GetFinalData();
			BinaryWriterFactory.ReleaseBinaryWriter(binaryWriter);
			this._writer = null;
			return finalData;
		}

		// Token: 0x04000022 RID: 34
		private BinaryWriter _writer;

		// Token: 0x04000023 RID: 35
		private int _entryCount;

		// Token: 0x04000024 RID: 36
		private int _folderCount;

		// Token: 0x04000025 RID: 37
		private List<SaveEntryFolder> _folders;
	}
}
