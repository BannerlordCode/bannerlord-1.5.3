using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000018 RID: 24
	internal class ArchiveConcurrentSerializer : IArchiveContext
	{
		// Token: 0x06000083 RID: 131 RVA: 0x000042A0 File Offset: 0x000024A0
		public ArchiveConcurrentSerializer()
		{
			this._locker = new object();
			this._writers = new Dictionary<int, BinaryWriter>();
			this._folders = new ConcurrentBag<SaveEntryFolder>();
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000042CC File Offset: 0x000024CC
		public void SerializeFolderConcurrent(SaveEntryFolder folder)
		{
			int managedThreadId = Thread.CurrentThread.ManagedThreadId;
			object locker = this._locker;
			BinaryWriter binaryWriter;
			lock (locker)
			{
				if (!this._writers.TryGetValue(managedThreadId, out binaryWriter))
				{
					binaryWriter = new BinaryWriter(262144);
					this._writers.Add(managedThreadId, binaryWriter);
				}
			}
			foreach (SaveEntry saveEntry in folder.GetAllEntries())
			{
				this.SerializeEntryConcurrent(saveEntry, binaryWriter);
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004380 File Offset: 0x00002580
		public SaveEntryFolder CreateFolder(SaveEntryFolder parentFolder, FolderId folderId, int entryCount)
		{
			int num = Interlocked.Increment(ref this._folderCount) - 1;
			SaveEntryFolder saveEntryFolder = new SaveEntryFolder(parentFolder, num, folderId, entryCount);
			parentFolder.AddChildFolderEntry(saveEntryFolder);
			this._folders.Add(saveEntryFolder);
			return saveEntryFolder;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000043BC File Offset: 0x000025BC
		private void SerializeEntryConcurrent(SaveEntry entry, BinaryWriter writer)
		{
			writer.Write3ByteInt(entry.FolderId);
			writer.Write3ByteInt(entry.Id.Id);
			writer.WriteByte((byte)entry.Id.Extension);
			writer.WriteShort((short)entry.Data.Length);
			writer.WriteBytes(entry.Data);
			Interlocked.Increment(ref this._entryCount);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004424 File Offset: 0x00002624
		public byte[] FinalizeAndGetBinaryDataConcurrent()
		{
			BinaryWriter binaryWriter = new BinaryWriter();
			binaryWriter.WriteInt(this._folderCount);
			foreach (SaveEntryFolder saveEntryFolder in this._folders)
			{
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
			foreach (BinaryWriter binaryWriter2 in this._writers.Values)
			{
				binaryWriter.AppendData(binaryWriter2);
			}
			return binaryWriter.GetFinalData();
		}

		// Token: 0x0400001D RID: 29
		private int _entryCount;

		// Token: 0x0400001E RID: 30
		private int _folderCount;

		// Token: 0x0400001F RID: 31
		private object _locker;

		// Token: 0x04000020 RID: 32
		private Dictionary<int, BinaryWriter> _writers;

		// Token: 0x04000021 RID: 33
		private ConcurrentBag<SaveEntryFolder> _folders;
	}
}
