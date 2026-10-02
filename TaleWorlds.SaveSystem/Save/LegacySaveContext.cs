using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x0200002A RID: 42
	public class LegacySaveContext : ISaveContext
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00007F39 File Offset: 0x00006139
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00007F41 File Offset: 0x00006141
		public object RootObject { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00007F4A File Offset: 0x0000614A
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00007F52 File Offset: 0x00006152
		public GameData SaveData { get; private set; }

		// Token: 0x0600018C RID: 396 RVA: 0x00007F5B File Offset: 0x0000615B
		public void ReportSaveIntegrityDrift(string message)
		{
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00007F5D File Offset: 0x0000615D
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00007F65 File Offset: 0x00006165
		public DefinitionContext DefinitionContext { get; private set; }

		// Token: 0x0600018F RID: 399 RVA: 0x00007F6E File Offset: 0x0000616E
		public static LegacySaveContext.SaveStatistics GetStatistics()
		{
			return new LegacySaveContext.SaveStatistics(LegacySaveContext._typeStatistics, LegacySaveContext._containerStatistics);
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00007F7F File Offset: 0x0000617F
		public static bool EnableSaveStatistics
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00007F84 File Offset: 0x00006184
		public LegacySaveContext(DefinitionContext definitionContext)
		{
			this.DefinitionContext = definitionContext;
			this._childObjects = new List<object>(131072);
			this._idsOfChildObjects = new Dictionary<object, int>(131072);
			this._strings = new List<string>(131072);
			this._idsOfStrings = new Dictionary<string, int>(131072);
			this._childContainers = new List<object>(131072);
			this._idsOfChildContainers = new Dictionary<object, int>(131072);
			this._temporaryCollectedObjects = new List<object>(4096);
			this._locker = new object();
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000801C File Offset: 0x0000621C
		private void CollectObjects()
		{
			using (new PerformanceTestBlock("SaveContext::CollectObjects"))
			{
				this._objectsToIterate = new Queue<object>(1024);
				this._objectsToIterate.Enqueue(this.RootObject);
				while (this._objectsToIterate.Count > 0)
				{
					object obj = this._objectsToIterate.Dequeue();
					ContainerType containerType;
					if (obj.GetType().IsContainer(out containerType))
					{
						this.CollectContainerObjects(containerType, obj);
					}
					else
					{
						this.CollectObjects(obj);
					}
				}
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000080B0 File Offset: 0x000062B0
		private void CollectContainerObjects(ContainerType containerType, object parent)
		{
			if (!this._idsOfChildContainers.ContainsKey(parent))
			{
				int count = this._childContainers.Count;
				this._childContainers.Add(parent);
				this._idsOfChildContainers.Add(parent, count);
				Type type = parent.GetType();
				ContainerDefinition containerDefinition = this.DefinitionContext.GetContainerDefinition(type);
				if (containerDefinition == null)
				{
					string text = "Cant find definition for " + type.FullName;
					Debug.Print(text, 0, Debug.DebugColor.Red, 17592186044416UL);
					Debug.FailedAssert(text, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\Save\\LegacySaveContext.cs", "CollectContainerObjects", 155);
				}
				ContainerSaveData.GetChildObjects(this, containerDefinition, containerType, parent, this._temporaryCollectedObjects);
				for (int i = 0; i < this._temporaryCollectedObjects.Count; i++)
				{
					object obj = this._temporaryCollectedObjects[i];
					if (obj != null)
					{
						this._objectsToIterate.Enqueue(obj);
					}
				}
				this._temporaryCollectedObjects.Clear();
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00008190 File Offset: 0x00006390
		private void CollectObjects(object parent)
		{
			if (!this._idsOfChildObjects.ContainsKey(parent))
			{
				int count = this._childObjects.Count;
				this._childObjects.Add(parent);
				this._idsOfChildObjects.Add(parent, count);
				Type type = parent.GetType();
				TypeDefinition classDefinition = this.DefinitionContext.GetClassDefinition(type);
				if (classDefinition == null)
				{
					throw new Exception("Could not find type definition of type: " + type);
				}
				ObjectSaveData.GetChildObjects(this, classDefinition, parent, this._temporaryCollectedObjects);
				for (int i = 0; i < this._temporaryCollectedObjects.Count; i++)
				{
					object obj = this._temporaryCollectedObjects[i];
					if (obj != null)
					{
						this._objectsToIterate.Enqueue(obj);
					}
				}
				this._temporaryCollectedObjects.Clear();
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000824C File Offset: 0x0000644C
		public void AddStrings(List<string> texts)
		{
			object locker = this._locker;
			lock (locker)
			{
				for (int i = 0; i < texts.Count; i++)
				{
					string text = texts[i];
					if (text != null && !this._idsOfStrings.ContainsKey(text))
					{
						int count = this._strings.Count;
						this._idsOfStrings.Add(text, count);
						this._strings.Add(text);
					}
				}
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000082D8 File Offset: 0x000064D8
		public int AddOrGetStringId(string text)
		{
			int num = -1;
			if (text == null)
			{
				num = -1;
			}
			else
			{
				object locker = this._locker;
				lock (locker)
				{
					int num2;
					if (this._idsOfStrings.TryGetValue(text, out num2))
					{
						num = num2;
					}
					else
					{
						num = this._strings.Count;
						this._idsOfStrings.Add(text, num);
						this._strings.Add(text);
					}
				}
			}
			return num;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00008358 File Offset: 0x00006558
		public int GetObjectId(object target)
		{
			int num;
			if (!this._idsOfChildObjects.TryGetValue(target, out num))
			{
				Debug.Print(string.Format("SAVE ERROR. Cant find {0} with type {1}", target, target.GetType()), 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.FailedAssert("SAVE ERROR. Cant find target object on save", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\Save\\LegacySaveContext.cs", "GetObjectId", 262);
			}
			return num;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000083B1 File Offset: 0x000065B1
		public int GetContainerId(object target)
		{
			return this._idsOfChildContainers[target];
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000083C0 File Offset: 0x000065C0
		public int GetStringId(string target)
		{
			if (target == null)
			{
				return -1;
			}
			int num = -1;
			object locker = this._locker;
			lock (locker)
			{
				num = this._idsOfStrings[target];
			}
			return num;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00008410 File Offset: 0x00006610
		private static void SaveStringTo(SaveEntryFolder stringsFolder, int id, string value)
		{
			BinaryWriter binaryWriter = BinaryWriterFactory.GetBinaryWriter();
			binaryWriter.WriteString(value);
			stringsFolder.CreateEntry(new EntryId(id, SaveEntryExtension.Txt)).FillFrom(binaryWriter);
			BinaryWriterFactory.ReleaseBinaryWriter(binaryWriter);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00008444 File Offset: 0x00006644
		public bool Save(object target, MetaData metaData, out string errorMessage)
		{
			errorMessage = "";
			bool flag = false;
			if (LegacySaveContext.EnableSaveStatistics)
			{
				LegacySaveContext._typeStatistics = new Dictionary<string, ValueTuple<int, int, int, long>>();
				LegacySaveContext._containerStatistics = new Dictionary<string, ValueTuple<int, int, int, int, long>>();
			}
			try
			{
				this.RootObject = target;
				using (new PerformanceTestBlock("SaveContext::Save"))
				{
					BinaryWriterFactory.Initialize();
					this.CollectObjects();
					ArchiveConcurrentSerializer headerSerializer = new ArchiveConcurrentSerializer();
					byte[][] objectData = new byte[this._childObjects.Count][];
					using (new PerformanceTestBlock("SaveContext::Saving Objects"))
					{
						if (!LegacySaveContext.EnableSaveStatistics)
						{
							TWParallel.ForWithoutRenderThread(0, this._childObjects.Count, delegate(int startInclusive, int endExclusive)
							{
								for (int l = startInclusive; l < endExclusive; l++)
								{
									this.SaveSingleObject(headerSerializer, objectData, l);
								}
							}, 16);
						}
						else
						{
							for (int i = 0; i < this._childObjects.Count; i++)
							{
								this.SaveSingleObject(headerSerializer, objectData, i);
							}
						}
					}
					byte[][] containerData = new byte[this._childContainers.Count][];
					using (new PerformanceTestBlock("SaveContext::Saving Containers"))
					{
						if (!LegacySaveContext.EnableSaveStatistics)
						{
							TWParallel.ForWithoutRenderThread(0, this._childContainers.Count, delegate(int startInclusive, int endExclusive)
							{
								for (int m = startInclusive; m < endExclusive; m++)
								{
									this.SaveSingleContainer(headerSerializer, containerData, m);
								}
							}, 16);
						}
						else
						{
							for (int j = 0; j < this._childContainers.Count; j++)
							{
								this.SaveSingleContainer(headerSerializer, containerData, j);
							}
						}
					}
					SaveEntryFolder saveEntryFolder = SaveEntryFolder.CreateRootFolder();
					BinaryWriter binaryWriter = BinaryWriterFactory.GetBinaryWriter();
					binaryWriter.WriteInt(this._idsOfChildObjects.Count);
					binaryWriter.WriteInt(this._strings.Count);
					binaryWriter.WriteInt(this._idsOfChildContainers.Count);
					saveEntryFolder.CreateEntry(new EntryId(-1, SaveEntryExtension.Config)).FillFrom(binaryWriter);
					headerSerializer.SerializeFolderConcurrent(saveEntryFolder);
					BinaryWriterFactory.ReleaseBinaryWriter(binaryWriter);
					ArchiveSerializer archiveSerializer = new ArchiveSerializer();
					SaveEntryFolder saveEntryFolder2 = SaveEntryFolder.CreateRootFolder();
					SaveEntryFolder saveEntryFolder3 = archiveSerializer.CreateFolder(saveEntryFolder2, new FolderId(-1, SaveFolderExtension.Strings), this._strings.Count);
					for (int k = 0; k < this._strings.Count; k++)
					{
						string text = this._strings[k];
						LegacySaveContext.SaveStringTo(saveEntryFolder3, k, text);
					}
					archiveSerializer.SerializeFolder(saveEntryFolder2);
					byte[] array = headerSerializer.FinalizeAndGetBinaryDataConcurrent();
					byte[] array2 = archiveSerializer.FinalizeAndGetBinaryData();
					this.SaveData = new GameData(array, array2, objectData, containerData);
					BinaryWriterFactory.Release();
				}
				flag = true;
			}
			catch (Exception ex)
			{
				errorMessage = "SaveContext Error\n";
				errorMessage += ex.Message;
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008758 File Offset: 0x00006958
		private void SaveSingleObject(ArchiveConcurrentSerializer headerSerializer, byte[][] objectData, int id)
		{
			object obj = this._childObjects[id];
			ArchiveSerializer archiveSerializer = new ArchiveSerializer();
			SaveEntryFolder saveEntryFolder = SaveEntryFolder.CreateRootFolder();
			SaveEntryFolder saveEntryFolder2 = SaveEntryFolder.CreateRootFolder();
			ObjectSaveData objectSaveData = new ObjectSaveData(this, id, obj, true);
			objectSaveData.CollectStructs();
			objectSaveData.CollectMembers();
			objectSaveData.CollectStrings();
			objectSaveData.SaveHeaderTo(saveEntryFolder2, headerSerializer);
			objectSaveData.SaveTo(saveEntryFolder, archiveSerializer);
			headerSerializer.SerializeFolderConcurrent(saveEntryFolder2);
			archiveSerializer.SerializeFolder(saveEntryFolder);
			byte[] array = archiveSerializer.FinalizeAndGetBinaryData();
			objectData[id] = array;
			if (LegacySaveContext.EnableSaveStatistics)
			{
				string name = objectSaveData.Type.Name;
				int num = array.Length;
				ValueTuple<int, int, int, long> valueTuple;
				if (LegacySaveContext._typeStatistics.TryGetValue(name, out valueTuple))
				{
					LegacySaveContext._typeStatistics[name] = new ValueTuple<int, int, int, long>(valueTuple.Item1 + 1, valueTuple.Item2, valueTuple.Item3, valueTuple.Item4 + (long)num);
					return;
				}
				LegacySaveContext._typeStatistics[name] = new ValueTuple<int, int, int, long>(1, objectSaveData.FieldCount, objectSaveData.PropertyCount, (long)num);
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00008854 File Offset: 0x00006A54
		private void SaveSingleContainer(ArchiveConcurrentSerializer headerSerializer, byte[][] containerData, int id)
		{
			object obj = this._childContainers[id];
			ArchiveSerializer archiveSerializer = new ArchiveSerializer();
			SaveEntryFolder saveEntryFolder = SaveEntryFolder.CreateRootFolder();
			SaveEntryFolder saveEntryFolder2 = SaveEntryFolder.CreateRootFolder();
			ContainerType containerType;
			obj.GetType().IsContainer(out containerType);
			ContainerSaveData containerSaveData = new ContainerSaveData(this, id, obj, containerType);
			containerSaveData.CollectChildren();
			containerSaveData.CollectStructs();
			containerSaveData.CollectMembers();
			containerSaveData.CollectStrings();
			containerSaveData.SaveHeaderTo(saveEntryFolder2, headerSerializer);
			containerSaveData.SaveTo(saveEntryFolder, archiveSerializer);
			headerSerializer.SerializeFolderConcurrent(saveEntryFolder2);
			archiveSerializer.SerializeFolder(saveEntryFolder);
			byte[] array = archiveSerializer.FinalizeAndGetBinaryData();
			containerData[id] = array;
			if (LegacySaveContext.EnableSaveStatistics)
			{
				string containerName = this.GetContainerName(containerSaveData.Type);
				long num = (long)array.Length;
				ValueTuple<int, int, int, int, long> valueTuple;
				if (LegacySaveContext._containerStatistics.TryGetValue(containerName, out valueTuple))
				{
					LegacySaveContext._containerStatistics[containerName] = new ValueTuple<int, int, int, int, long>(valueTuple.Item1 + 1, valueTuple.Item2 + containerSaveData.GetElementCount(), valueTuple.Item3, valueTuple.Item4, LegacySaveContext._containerStatistics[containerName].Item5 + num);
					return;
				}
				LegacySaveContext._containerStatistics[containerName] = new ValueTuple<int, int, int, int, long>(1, containerSaveData.GetElementCount(), containerSaveData.ElementFieldCount, containerSaveData.ElementPropertyCount, num);
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008988 File Offset: 0x00006B88
		private string GetContainerName(Type t)
		{
			string text = t.Name;
			foreach (Type type in t.GetGenericArguments())
			{
				if (t.IsContainer())
				{
					text += this.GetContainerName(type);
				}
				else
				{
					text = text + type.Name + ".";
				}
			}
			return text.TrimEnd(new char[] { '.' });
		}

		// Token: 0x04000063 RID: 99
		private List<object> _childObjects;

		// Token: 0x04000064 RID: 100
		private Dictionary<object, int> _idsOfChildObjects;

		// Token: 0x04000065 RID: 101
		private List<object> _childContainers;

		// Token: 0x04000066 RID: 102
		private Dictionary<object, int> _idsOfChildContainers;

		// Token: 0x04000067 RID: 103
		private List<string> _strings;

		// Token: 0x04000068 RID: 104
		private Dictionary<string, int> _idsOfStrings;

		// Token: 0x04000069 RID: 105
		private List<object> _temporaryCollectedObjects;

		// Token: 0x0400006B RID: 107
		private object _locker;

		// Token: 0x0400006D RID: 109
		private static Dictionary<string, ValueTuple<int, int, int, long>> _typeStatistics;

		// Token: 0x0400006E RID: 110
		private static Dictionary<string, ValueTuple<int, int, int, int, long>> _containerStatistics;

		// Token: 0x0400006F RID: 111
		private Queue<object> _objectsToIterate;

		// Token: 0x02000075 RID: 117
		public struct SaveStatistics
		{
			// Token: 0x060003F4 RID: 1012 RVA: 0x00011E0B File Offset: 0x0001000B
			public SaveStatistics(Dictionary<string, ValueTuple<int, int, int, long>> typeStatistics, Dictionary<string, ValueTuple<int, int, int, int, long>> containerStatistics)
			{
				this._typeStatistics = typeStatistics;
				this._containerStatistics = containerStatistics;
			}

			// Token: 0x060003F5 RID: 1013 RVA: 0x00011E1C File Offset: 0x0001001C
			public ValueTuple<int, int, int, long> GetObjectCounts(string key)
			{
				if (this._typeStatistics.ContainsKey(key))
				{
					return this._typeStatistics[key];
				}
				return default(ValueTuple<int, int, int, long>);
			}

			// Token: 0x060003F6 RID: 1014 RVA: 0x00011E4D File Offset: 0x0001004D
			public ValueTuple<int, int, int, int, long> GetContainerCounts(string key)
			{
				return this._containerStatistics[key];
			}

			// Token: 0x060003F7 RID: 1015 RVA: 0x00011E5B File Offset: 0x0001005B
			public long GetContainerSize(string key)
			{
				return this._containerStatistics[key].Item5;
			}

			// Token: 0x060003F8 RID: 1016 RVA: 0x00011E6E File Offset: 0x0001006E
			public List<string> GetTypeKeys()
			{
				return this._typeStatistics.Keys.ToList<string>();
			}

			// Token: 0x060003F9 RID: 1017 RVA: 0x00011E80 File Offset: 0x00010080
			public List<string> GetContainerKeys()
			{
				return this._containerStatistics.Keys.ToList<string>();
			}

			// Token: 0x0400014B RID: 331
			private Dictionary<string, ValueTuple<int, int, int, long>> _typeStatistics;

			// Token: 0x0400014C RID: 332
			private Dictionary<string, ValueTuple<int, int, int, int, long>> _containerStatistics;
		}
	}
}
