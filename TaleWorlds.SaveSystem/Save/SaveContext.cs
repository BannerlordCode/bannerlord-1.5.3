using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x0200002E RID: 46
	public class SaveContext : ISaveContext
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001CE RID: 462 RVA: 0x000098E7 File Offset: 0x00007AE7
		// (set) Token: 0x060001CF RID: 463 RVA: 0x000098EF File Offset: 0x00007AEF
		public object RootObject { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x000098F8 File Offset: 0x00007AF8
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x00009900 File Offset: 0x00007B00
		public GameData SaveData { get; private set; }

		// Token: 0x060001D2 RID: 466 RVA: 0x0000990C File Offset: 0x00007B0C
		public void ReportSaveIntegrityDrift(string message)
		{
			Debug.Print(message, 0, Debug.DebugColor.White, 17592186044416UL);
			object driftLocker = this._driftLocker;
			lock (driftLocker)
			{
				if (this._integrityDriftReports.Count < 4)
				{
					this._integrityDriftReports.Add(message);
				}
			}
			Debug.FailedAssert(message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\Save\\SaveContext.cs", "ReportSaveIntegrityDrift", 71);
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00009984 File Offset: 0x00007B84
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x0000998C File Offset: 0x00007B8C
		public DefinitionContext DefinitionContext { get; private set; }

		// Token: 0x060001D5 RID: 469 RVA: 0x00009995 File Offset: 0x00007B95
		public static SaveContext.SaveStatistics GetStatistics()
		{
			return new SaveContext.SaveStatistics(SaveContext._typeStatistics, SaveContext._containerStatistics);
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x000099A6 File Offset: 0x00007BA6
		public static bool EnableSaveStatistics
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000099AC File Offset: 0x00007BAC
		public SaveContext(DefinitionContext definitionContext)
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

		// Token: 0x060001D8 RID: 472 RVA: 0x00009A4C File Offset: 0x00007C4C
		private void CollectSaveDatas()
		{
			SaveContext.SizeRecord.HeaderSize = SaveContext.GetConfigEntrySize();
			SaveContext.SizeRecord.StringSize = SaveContext.GetStringFolderSize();
			SaveContext.SizeRecord.ObjectSize = 0;
			SaveContext.SizeRecord.ContainerSize = 0;
			using (new PerformanceTestBlock("SaveContext::CollectSaveDataForObject::Objects"))
			{
				if (!SaveContext.EnableSaveStatistics)
				{
					TWParallel.ForWithoutRenderThread(0, this._childObjects.Count, delegate(int startInclusive, int endExclusive)
					{
						for (int k = startInclusive; k < endExclusive; k++)
						{
							this.CollectSaveDataForObject(k, ref SaveContext.SizeRecord);
						}
					}, 16);
				}
				else
				{
					for (int i = 0; i < this._childObjects.Count; i++)
					{
						this.CollectSaveDataForObject(i, ref SaveContext.SizeRecord);
					}
				}
			}
			using (new PerformanceTestBlock("SaveContext::CollectSaveDataForObject::Containers"))
			{
				if (!SaveContext.EnableSaveStatistics)
				{
					TWParallel.ForWithoutRenderThread(0, this._childContainers.Count, delegate(int startInclusive, int endExclusive)
					{
						for (int l = startInclusive; l < endExclusive; l++)
						{
							this.CollectSaveDataForContainer(l, ref SaveContext.SizeRecord);
						}
					}, 16);
				}
				else
				{
					for (int j = 0; j < this._childContainers.Count; j++)
					{
						this.CollectSaveDataForContainer(j, ref SaveContext.SizeRecord);
					}
				}
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00009B6C File Offset: 0x00007D6C
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

		// Token: 0x060001DA RID: 474 RVA: 0x00009C00 File Offset: 0x00007E00
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
					string text = "[SaveSystem] Cant find definition for " + type.FullName;
					Debug.Print(text, 0, Debug.DebugColor.Red, 17592186044416UL);
					Debug.FailedAssert(text, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\Save\\SaveContext.cs", "CollectContainerObjects", 242);
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

		// Token: 0x060001DB RID: 475 RVA: 0x00009CE0 File Offset: 0x00007EE0
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

		// Token: 0x060001DC RID: 476 RVA: 0x00009D9C File Offset: 0x00007F9C
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
						int stringSizeWithOverhead = SaveContext.GetStringSizeWithOverhead(text);
						Interlocked.Add(ref SaveContext.SizeRecord.StringSize, stringSizeWithOverhead);
					}
				}
			}
			return num;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009E34 File Offset: 0x00008034
		public int GetObjectId(object target)
		{
			int num;
			if (!this._idsOfChildObjects.TryGetValue(target, out num))
			{
				Debug.Print(string.Format("[SaveSystem] SAVE ERROR. Cant find {0} with type {1}", target, target.GetType()), 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.FailedAssert("[SaveSystem] SAVE ERROR. Cant find target object on save", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\Save\\SaveContext.cs", "GetObjectId", 330);
			}
			return num;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00009E8D File Offset: 0x0000808D
		public int GetContainerId(object target)
		{
			return this._idsOfChildContainers[target];
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00009E9C File Offset: 0x0000809C
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

		// Token: 0x060001E0 RID: 480 RVA: 0x00009EEC File Offset: 0x000080EC
		private static void SaveStringTo(BinaryWriter stringWriter, int id, string text)
		{
			stringWriter.Write3ByteInt(0);
			stringWriter.Write3ByteInt(id);
			stringWriter.WriteByte(10);
			int stringSizeInBytes = SaveContext.GetStringSizeInBytes(text);
			if (stringSizeInBytes > 32767)
			{
				string text2 = ((text.Length > 32) ? (text.Substring(0, 32) + "...") : text);
				string text3 = string.Format("[SaveSystem] SaveStringTo: encoded string length {0} exceeds short.MaxValue and will wrap. Text starts with: '{1}'", stringSizeInBytes, text2);
				Debug.Print(text3, 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.FailedAssert(text3, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\Save\\SaveContext.cs", "SaveStringTo", 375);
			}
			stringWriter.WriteShort((short)stringSizeInBytes);
			stringWriter.WriteString(text);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009F85 File Offset: 0x00008185
		public static int GetStringSizeInBytes(string text)
		{
			return 4 + Encoding.UTF8.GetByteCount(text);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00009F94 File Offset: 0x00008194
		private static int GetStringSizeWithOverhead(string text)
		{
			return SaveContext.GetStringSizeInBytes(text) + 9;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009FA0 File Offset: 0x000081A0
		public bool Save(object target, MetaData metaData, out string errorMessage)
		{
			errorMessage = "";
			bool flag = false;
			if (SaveContext.EnableSaveStatistics)
			{
				SaveContext._typeStatistics = new Dictionary<string, ValueTuple<int, int, int, long>>();
				SaveContext._containerStatistics = new Dictionary<string, ValueTuple<int, int, int, int, long>>();
			}
			try
			{
				this.RootObject = target;
				using (new PerformanceTestBlock("SaveContext::Save"))
				{
					this.CollectObjects();
					this._objectSaveDataList = new ObjectSaveData[this._childObjects.Count];
					this._containerSaveDataList = new ContainerSaveData[this._childContainers.Count];
					SaveContext.SizeRecord = default(SaveContext.SaveDataSizeRecord);
					this._integrityDriftReports = new List<string>();
					this.CollectSaveDatas();
					byte[][] array = this.WriteObjects();
					byte[][] array2 = this.WriteContainers();
					byte[] array3 = SaveContext.WriteHeaders(this._objectSaveDataList, this._containerSaveDataList, SaveContext.SizeRecord.HeaderSize, this._strings.Count);
					byte[] array4 = SaveContext.WriteAllStrings(this._strings, SaveContext.SizeRecord.StringSize);
					this.SaveData = new GameData(array3, array4, array, array2);
					Debug.Print(SaveContext.SizeRecord.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
					metaData["SavePredictedSizes"] = string.Format("h={0} obj={1} ctr={2} str={3}", new object[]
					{
						SaveContext.SizeRecord.HeaderSize,
						SaveContext.SizeRecord.ObjectSize,
						SaveContext.SizeRecord.ContainerSize,
						SaveContext.SizeRecord.StringSize
					});
					if (this._integrityDriftReports != null && this._integrityDriftReports.Count > 0)
					{
						metaData["SaveIntegrityDrift"] = string.Join(" | ", this._integrityDriftReports);
					}
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

		// Token: 0x060001E4 RID: 484 RVA: 0x0000A1A8 File Offset: 0x000083A8
		private byte[][] WriteObjects()
		{
			byte[][] objectData = new byte[this._childObjects.Count][];
			using (new PerformanceTestBlock("SaveContext::Saving Objects"))
			{
				if (!SaveContext.EnableSaveStatistics)
				{
					TWParallel.ForWithoutRenderThread(0, this._childObjects.Count, delegate(int startInclusive, int endExclusive)
					{
						for (int j = startInclusive; j < endExclusive; j++)
						{
							this.SaveSingleObject(objectData, j);
						}
					}, 16);
				}
				else
				{
					for (int i = 0; i < this._childObjects.Count; i++)
					{
						this.SaveSingleObject(objectData, i);
					}
				}
			}
			return objectData;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000A250 File Offset: 0x00008450
		private byte[][] WriteContainers()
		{
			byte[][] containerData = new byte[this._childContainers.Count][];
			using (new PerformanceTestBlock("SaveContext::Saving Containers"))
			{
				if (!SaveContext.EnableSaveStatistics)
				{
					TWParallel.ForWithoutRenderThread(0, this._childContainers.Count, delegate(int startInclusive, int endExclusive)
					{
						for (int j = startInclusive; j < endExclusive; j++)
						{
							this.SaveSingleContainer(containerData, j);
						}
					}, 16);
				}
				else
				{
					for (int i = 0; i < this._childContainers.Count; i++)
					{
						this.SaveSingleContainer(containerData, i);
					}
				}
			}
			return containerData;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000A2F8 File Offset: 0x000084F8
		private static byte[] WriteHeaders(ObjectSaveData[] objects, ContainerSaveData[] containers, int headerSize, int stringCount)
		{
			BinaryWriter binaryWriter = new BinaryWriter(SaveContext.SizeRecord.HeaderSize);
			binaryWriter.WriteInt(objects.Length + containers.Length);
			for (int i = 0; i < objects.Length; i++)
			{
				objects[i].SaveHeaderFolderTo(binaryWriter, i);
			}
			for (int j = 0; j < containers.Length; j++)
			{
				containers[j].SaveHeaderFolderTo(binaryWriter, objects.Length + j);
			}
			binaryWriter.WriteInt(objects.Length + containers.Length + 1);
			for (int k = 0; k < objects.Length; k++)
			{
				objects[k].SaveHeaderDataTo(binaryWriter, k);
			}
			for (int l = 0; l < containers.Length; l++)
			{
				containers[l].SaveHeaderDataTo(binaryWriter, objects.Length + l);
			}
			SaveContext.WriteConfigEntry(binaryWriter, objects.Length, stringCount, containers.Length);
			return binaryWriter.GetFinalData();
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000A3B4 File Offset: 0x000085B4
		private static byte[] WriteAllStrings(List<string> strings, int stringSize)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stringSize);
			SaveContext.WriteStringsEntry(binaryWriter, strings.Count);
			for (int i = 0; i < strings.Count; i++)
			{
				string text = strings[i];
				SaveContext.SaveStringTo(binaryWriter, i, text);
			}
			return binaryWriter.GetFinalData();
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000A3FB File Offset: 0x000085FB
		private static void WriteConfigEntry(BinaryWriter headerWriter, int objects, int strings, int containers)
		{
			headerWriter.Write3ByteInt(-1);
			headerWriter.Write3ByteInt(-1);
			headerWriter.WriteByte(7);
			headerWriter.WriteShort(12);
			headerWriter.WriteInt(objects);
			headerWriter.WriteInt(strings);
			headerWriter.WriteInt(containers);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000A42F File Offset: 0x0000862F
		private static void WriteStringsEntry(BinaryWriter headerWriter, int strings)
		{
			headerWriter.WriteInt(1);
			headerWriter.Write3ByteInt(-1);
			headerWriter.Write3ByteInt(0);
			headerWriter.Write3ByteInt(-1);
			headerWriter.WriteByte(4);
			headerWriter.WriteInt(strings);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000A45B File Offset: 0x0000865B
		private static int GetConfigEntrySize()
		{
			return 29;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000A45F File Offset: 0x0000865F
		private static int GetStringFolderSize()
		{
			return 18;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000A464 File Offset: 0x00008664
		private void CollectSaveDataForObject(int id, ref SaveContext.SaveDataSizeRecord headerSize)
		{
			object obj = this._childObjects[id];
			ObjectSaveData objectSaveData = new ObjectSaveData(this, id, obj, true);
			objectSaveData.CollectStructs();
			objectSaveData.CollectMembers();
			objectSaveData.CollectStrings();
			this._objectSaveDataList[id] = objectSaveData;
			Interlocked.Add(ref headerSize.HeaderSize, objectSaveData.GetHeaderSize());
			Interlocked.Add(ref headerSize.ObjectSize, objectSaveData.GetDataSize());
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000A4C8 File Offset: 0x000086C8
		private void CollectSaveDataForContainer(int id, ref SaveContext.SaveDataSizeRecord headerSize)
		{
			object obj = this._childContainers[id];
			ContainerType containerType;
			obj.GetType().IsContainer(out containerType);
			ContainerSaveData containerSaveData = new ContainerSaveData(this, id, obj, containerType);
			containerSaveData.CollectChildren();
			containerSaveData.CollectStructs();
			containerSaveData.CollectMembers();
			containerSaveData.CollectStrings();
			this._containerSaveDataList[id] = containerSaveData;
			Interlocked.Add(ref headerSize.HeaderSize, containerSaveData.GetHeaderSize());
			Interlocked.Add(ref headerSize.ContainerSize, containerSaveData.GetDataSize());
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000A540 File Offset: 0x00008740
		private void SaveSingleObject(byte[][] objectData, int id)
		{
			object obj = this._childObjects[id];
			ObjectSaveData objectSaveData = this._objectSaveDataList[id];
			int dataSize = objectSaveData.GetDataSize();
			BinaryWriter binaryWriter = new BinaryWriter(dataSize);
			int folderCount = objectSaveData.GetFolderCount();
			binaryWriter.WriteInt(folderCount);
			int num = 0;
			objectSaveData.SaveDataFolder(binaryWriter, -1, ref num);
			int entryCount = objectSaveData.GetEntryCount();
			binaryWriter.WriteInt(entryCount);
			num = 0;
			objectSaveData.SaveTo(binaryWriter, ref num);
			if (binaryWriter.Length != dataSize)
			{
				string text = "[SaveSystem] Obj[{0}] {1} drift predicted={2} written={3}";
				object[] array = new object[4];
				array[0] = id;
				int num2 = 1;
				Type type = objectSaveData.Type;
				array[num2] = ((type != null) ? type.FullName : null);
				array[2] = dataSize;
				array[3] = binaryWriter.Length;
				this.ReportSaveIntegrityDrift(string.Format(text, array));
			}
			objectData[id] = binaryWriter.GetFinalData();
			if (SaveContext.EnableSaveStatistics)
			{
				string name = objectSaveData.Type.Name;
				ValueTuple<int, int, int, long> valueTuple;
				if (SaveContext._typeStatistics.TryGetValue(name, out valueTuple))
				{
					SaveContext._typeStatistics[name] = new ValueTuple<int, int, int, long>(valueTuple.Item1 + 1, valueTuple.Item2, valueTuple.Item3, valueTuple.Item4 + (long)dataSize);
					return;
				}
				SaveContext._typeStatistics[name] = new ValueTuple<int, int, int, long>(1, objectSaveData.FieldCount, objectSaveData.PropertyCount, (long)dataSize);
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000A67C File Offset: 0x0000887C
		private void SaveSingleContainer(byte[][] containerData, int id)
		{
			object obj = this._childContainers[id];
			ContainerSaveData containerSaveData = this._containerSaveDataList[id];
			int capturedElementCount = containerSaveData.CapturedElementCount;
			int elementCount = containerSaveData.GetElementCount();
			if (capturedElementCount != elementCount)
			{
				string text = "[SaveSystem] Ctr[{0}] {1} elem count drift captured={2} live={3}";
				object[] array = new object[4];
				array[0] = id;
				int num = 1;
				Type type = containerSaveData.Type;
				array[num] = ((type != null) ? type.FullName : null);
				array[2] = capturedElementCount;
				array[3] = elementCount;
				this.ReportSaveIntegrityDrift(string.Format(text, array));
			}
			int dataSize = containerSaveData.GetDataSize();
			BinaryWriter binaryWriter = new BinaryWriter(dataSize);
			binaryWriter.WriteInt(containerSaveData.GetFolderCount());
			int num2 = 0;
			containerSaveData.SaveDataFolder(binaryWriter, ref num2);
			int entryCount = containerSaveData.GetEntryCount();
			binaryWriter.WriteInt(entryCount);
			num2 = 0;
			containerSaveData.SaveTo(binaryWriter, ref num2);
			if (binaryWriter.Length != dataSize)
			{
				string text2 = "[SaveSystem] Ctr[{0}] {1} drift predicted={2} written={3}";
				object[] array2 = new object[4];
				array2[0] = id;
				int num3 = 1;
				Type type2 = containerSaveData.Type;
				array2[num3] = ((type2 != null) ? type2.FullName : null);
				array2[2] = dataSize;
				array2[3] = binaryWriter.Length;
				this.ReportSaveIntegrityDrift(string.Format(text2, array2));
			}
			containerData[id] = binaryWriter.GetFinalData();
			if (SaveContext.EnableSaveStatistics)
			{
				string containerName = this.GetContainerName(containerSaveData.Type);
				ValueTuple<int, int, int, int, long> valueTuple;
				if (SaveContext._containerStatistics.TryGetValue(containerName, out valueTuple))
				{
					SaveContext._containerStatistics[containerName] = new ValueTuple<int, int, int, int, long>(valueTuple.Item1 + 1, valueTuple.Item2 + containerSaveData.GetElementCount(), valueTuple.Item3, valueTuple.Item4, SaveContext._containerStatistics[containerName].Item5 + (long)dataSize);
					return;
				}
				SaveContext._containerStatistics[containerName] = new ValueTuple<int, int, int, int, long>(1, containerSaveData.GetElementCount(), containerSaveData.ElementFieldCount, containerSaveData.ElementPropertyCount, (long)dataSize);
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000A838 File Offset: 0x00008A38
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

		// Token: 0x0400007D RID: 125
		private static SaveContext.SaveDataSizeRecord SizeRecord;

		// Token: 0x0400007F RID: 127
		private List<object> _childObjects;

		// Token: 0x04000080 RID: 128
		private Dictionary<object, int> _idsOfChildObjects;

		// Token: 0x04000081 RID: 129
		private List<object> _childContainers;

		// Token: 0x04000082 RID: 130
		private Dictionary<object, int> _idsOfChildContainers;

		// Token: 0x04000083 RID: 131
		private List<string> _strings;

		// Token: 0x04000084 RID: 132
		private Dictionary<string, int> _idsOfStrings;

		// Token: 0x04000085 RID: 133
		private List<object> _temporaryCollectedObjects;

		// Token: 0x04000086 RID: 134
		private ObjectSaveData[] _objectSaveDataList;

		// Token: 0x04000087 RID: 135
		private ContainerSaveData[] _containerSaveDataList;

		// Token: 0x04000089 RID: 137
		private object _locker;

		// Token: 0x0400008A RID: 138
		private const int MaxIntegrityDriftReports = 4;

		// Token: 0x0400008B RID: 139
		private readonly object _driftLocker = new object();

		// Token: 0x0400008C RID: 140
		private List<string> _integrityDriftReports;

		// Token: 0x0400008E RID: 142
		private static Dictionary<string, ValueTuple<int, int, int, long>> _typeStatistics;

		// Token: 0x0400008F RID: 143
		private static Dictionary<string, ValueTuple<int, int, int, int, long>> _containerStatistics;

		// Token: 0x04000090 RID: 144
		private Queue<object> _objectsToIterate;

		// Token: 0x02000077 RID: 119
		private struct SaveDataSizeRecord
		{
			// Token: 0x060003FD RID: 1021 RVA: 0x00011F04 File Offset: 0x00010104
			public override string ToString()
			{
				float num = (float)(this.HeaderSize + this.StringSize + this.ObjectSize + this.ContainerSize) / 1048576f;
				string.Format("Total size: {0:##.00} MB", num);
				return string.Format("[SaveDataSizeRecord] Header size: {0}, String Size: {1}, Object Size: {2}, Container Size: {3}.\nTotal Size: {4}", new object[] { this.HeaderSize, this.StringSize, this.ObjectSize, this.ContainerSize, num });
			}

			// Token: 0x04000151 RID: 337
			public int HeaderSize;

			// Token: 0x04000152 RID: 338
			public int StringSize;

			// Token: 0x04000153 RID: 339
			public int ObjectSize;

			// Token: 0x04000154 RID: 340
			public int ContainerSize;
		}

		// Token: 0x02000078 RID: 120
		public struct SaveStatistics
		{
			// Token: 0x060003FE RID: 1022 RVA: 0x00011F96 File Offset: 0x00010196
			public SaveStatistics(Dictionary<string, ValueTuple<int, int, int, long>> typeStatistics, Dictionary<string, ValueTuple<int, int, int, int, long>> containerStatistics)
			{
				this._typeStatistics = typeStatistics;
				this._containerStatistics = containerStatistics;
			}

			// Token: 0x060003FF RID: 1023 RVA: 0x00011FA8 File Offset: 0x000101A8
			public ValueTuple<int, int, int, long> GetObjectCounts(string key)
			{
				if (this._typeStatistics.ContainsKey(key))
				{
					return this._typeStatistics[key];
				}
				return default(ValueTuple<int, int, int, long>);
			}

			// Token: 0x06000400 RID: 1024 RVA: 0x00011FD9 File Offset: 0x000101D9
			public ValueTuple<int, int, int, int, long> GetContainerCounts(string key)
			{
				return this._containerStatistics[key];
			}

			// Token: 0x06000401 RID: 1025 RVA: 0x00011FE7 File Offset: 0x000101E7
			public long GetContainerSize(string key)
			{
				return this._containerStatistics[key].Item5;
			}

			// Token: 0x06000402 RID: 1026 RVA: 0x00011FFA File Offset: 0x000101FA
			public List<string> GetTypeKeys()
			{
				return this._typeStatistics.Keys.ToList<string>();
			}

			// Token: 0x06000403 RID: 1027 RVA: 0x0001200C File Offset: 0x0001020C
			public List<string> GetContainerKeys()
			{
				return this._containerStatistics.Keys.ToList<string>();
			}

			// Token: 0x04000155 RID: 341
			private Dictionary<string, ValueTuple<int, int, int, long>> _typeStatistics;

			// Token: 0x04000156 RID: 342
			private Dictionary<string, ValueTuple<int, int, int, int, long>> _containerStatistics;
		}
	}
}
