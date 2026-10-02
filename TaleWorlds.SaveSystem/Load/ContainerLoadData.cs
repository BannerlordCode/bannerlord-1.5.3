using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x02000036 RID: 54
	internal class ContainerLoadData
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000B0AD File Offset: 0x000092AD
		public int Id
		{
			get
			{
				return this.ContainerHeaderLoadData.Id;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000B0BA File Offset: 0x000092BA
		public object Target
		{
			get
			{
				return this.ContainerHeaderLoadData.Target;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000B0C7 File Offset: 0x000092C7
		public LoadContext Context
		{
			get
			{
				return this.ContainerHeaderLoadData.Context;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000B0D4 File Offset: 0x000092D4
		public ContainerDefinition TypeDefinition
		{
			get
			{
				return this.ContainerHeaderLoadData.TypeDefinition;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000B0E1 File Offset: 0x000092E1
		// (set) Token: 0x06000232 RID: 562 RVA: 0x0000B0E9 File Offset: 0x000092E9
		public ContainerHeaderLoadData ContainerHeaderLoadData { get; private set; }

		// Token: 0x06000233 RID: 563 RVA: 0x0000B0F4 File Offset: 0x000092F4
		public ContainerLoadData(ContainerHeaderLoadData headerLoadData)
		{
			this.ContainerHeaderLoadData = headerLoadData;
			this._childStructs = new Dictionary<int, ObjectLoadData>();
			this._saveId = headerLoadData.SaveId;
			this._containerType = headerLoadData.ContainerType;
			this._elementCount = headerLoadData.ElementCount;
			this._keys = new ElementLoadData[this._elementCount];
			this._values = new ElementLoadData[this._elementCount];
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000B160 File Offset: 0x00009360
		private FolderId[] GetChildStructNames(SaveEntryFolder saveEntryFolder)
		{
			List<FolderId> list = new List<FolderId>();
			foreach (SaveEntryFolder saveEntryFolder2 in saveEntryFolder.ChildFolders)
			{
				if (saveEntryFolder2.FolderId.Extension == SaveFolderExtension.Struct && !list.Contains(saveEntryFolder2.FolderId))
				{
					list.Add(saveEntryFolder2.FolderId);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000B1E4 File Offset: 0x000093E4
		public void InitializeReaders(SaveEntryFolder saveEntryFolder)
		{
			foreach (FolderId folderId in this.GetChildStructNames(saveEntryFolder))
			{
				int localId = folderId.LocalId;
				ObjectLoadData objectLoadData = new ObjectLoadData(this.Context, localId);
				this._childStructs.Add(localId, objectLoadData);
			}
			for (int j = 0; j < this._elementCount; j++)
			{
				BinaryReader binaryReader = saveEntryFolder.GetEntry(new EntryId(j, SaveEntryExtension.Value)).GetBinaryReader();
				ElementLoadData elementLoadData = new ElementLoadData(this, binaryReader);
				this._values[j] = elementLoadData;
				if (this._containerType == ContainerType.Dictionary)
				{
					BinaryReader binaryReader2 = saveEntryFolder.GetEntry(new EntryId(j, SaveEntryExtension.Key)).GetBinaryReader();
					ElementLoadData elementLoadData2 = new ElementLoadData(this, binaryReader2);
					this._keys[j] = elementLoadData2;
				}
			}
			foreach (KeyValuePair<int, ObjectLoadData> keyValuePair in this._childStructs)
			{
				int key = keyValuePair.Key;
				ObjectLoadData value = keyValuePair.Value;
				SaveEntryFolder childFolder = saveEntryFolder.GetChildFolder(new FolderId(key, SaveFolderExtension.Struct));
				value.InitializeReaders(childFolder);
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000B310 File Offset: 0x00009510
		public void FillCreatedObject()
		{
			foreach (ObjectLoadData objectLoadData in this._childStructs.Values)
			{
				objectLoadData.CreateStruct();
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000B368 File Offset: 0x00009568
		public void Read()
		{
			for (int i = 0; i < this._elementCount; i++)
			{
				this._values[i].Read();
				if (this._containerType == ContainerType.Dictionary)
				{
					this._keys[i].Read();
				}
			}
			foreach (ObjectLoadData objectLoadData in this._childStructs.Values)
			{
				objectLoadData.Read();
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000B3F4 File Offset: 0x000095F4
		private static Assembly GetAssemblyByName(string name)
		{
			return AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault<Assembly>((Assembly assembly) => assembly.GetName().FullName == name);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000B42C File Offset: 0x0000962C
		public void FillObject()
		{
			foreach (ObjectLoadData objectLoadData in this._childStructs.Values)
			{
				objectLoadData.FillObject();
			}
			for (int i = 0; i < this._elementCount; i++)
			{
				if (this._containerType == ContainerType.List || this._containerType == ContainerType.CustomList || this._containerType == ContainerType.CustomReadOnlyList)
				{
					IList list = (IList)this.Target;
					ElementLoadData elementLoadData = this._values[i];
					if (elementLoadData.SavedMemberType == SavedMemberType.CustomStruct)
					{
						int num = (int)elementLoadData.Data;
						ObjectLoadData objectLoadData2;
						object obj;
						if (this._childStructs.TryGetValue(num, out objectLoadData2))
						{
							obj = objectLoadData2.Target;
						}
						else
						{
							obj = ContainerLoadData.GetDefaultObject(this._saveId, this.Context, false);
						}
						elementLoadData.SetCustomStructData(obj);
					}
					object dataToUse = elementLoadData.GetDataToUse();
					if (list != null)
					{
						list.Add(dataToUse);
					}
				}
				else if (this._containerType == ContainerType.Dictionary)
				{
					IDictionary dictionary = (IDictionary)this.Target;
					ElementLoadData elementLoadData2 = this._keys[i];
					ElementLoadData elementLoadData3 = this._values[i];
					if (elementLoadData2.SavedMemberType == SavedMemberType.CustomStruct)
					{
						int num2 = (int)elementLoadData2.Data;
						ObjectLoadData objectLoadData3;
						object obj2;
						if (this._childStructs.TryGetValue(num2, out objectLoadData3))
						{
							obj2 = objectLoadData3.Target;
						}
						else
						{
							obj2 = ContainerLoadData.GetDefaultObject(this._saveId, this.Context, false);
						}
						elementLoadData2.SetCustomStructData(obj2);
					}
					if (elementLoadData3.SavedMemberType == SavedMemberType.CustomStruct)
					{
						int num3 = (int)elementLoadData3.Data;
						ObjectLoadData objectLoadData4;
						object obj3;
						if (this._childStructs.TryGetValue(num3, out objectLoadData4))
						{
							obj3 = objectLoadData4.Target;
						}
						else
						{
							obj3 = ContainerLoadData.GetDefaultObject(this._saveId, this.Context, true);
						}
						elementLoadData3.SetCustomStructData(obj3);
					}
					object dataToUse2 = elementLoadData2.GetDataToUse();
					object dataToUse3 = elementLoadData3.GetDataToUse();
					if (dictionary != null && dataToUse2 != null)
					{
						dictionary.Add(dataToUse2, dataToUse3);
					}
				}
				else if (this._containerType == ContainerType.Array)
				{
					Array array = (Array)this.Target;
					ElementLoadData elementLoadData4 = this._values[i];
					if (elementLoadData4.SavedMemberType == SavedMemberType.CustomStruct)
					{
						int num4 = (int)elementLoadData4.Data;
						ObjectLoadData objectLoadData5;
						object obj4;
						if (this._childStructs.TryGetValue(num4, out objectLoadData5))
						{
							obj4 = objectLoadData5.Target;
						}
						else
						{
							obj4 = ContainerLoadData.GetDefaultObject(this._saveId, this.Context, false);
						}
						elementLoadData4.SetCustomStructData(obj4);
					}
					object dataToUse4 = elementLoadData4.GetDataToUse();
					array.SetValue(dataToUse4, i);
				}
				else if (this._containerType == ContainerType.Queue)
				{
					ICollection collection = (ICollection)this.Target;
					ElementLoadData elementLoadData5 = this._values[i];
					if (elementLoadData5.SavedMemberType == SavedMemberType.CustomStruct)
					{
						int num5 = (int)elementLoadData5.Data;
						ObjectLoadData objectLoadData6;
						object obj5;
						if (this._childStructs.TryGetValue(num5, out objectLoadData6))
						{
							obj5 = objectLoadData6.Target;
						}
						else
						{
							obj5 = ContainerLoadData.GetDefaultObject(this._saveId, this.Context, false);
						}
						elementLoadData5.SetCustomStructData(obj5);
					}
					object dataToUse5 = elementLoadData5.GetDataToUse();
					collection.GetType().GetMethod("Enqueue").Invoke(collection, new object[] { dataToUse5 });
				}
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000B768 File Offset: 0x00009968
		private static object GetDefaultObject(SaveId saveId, LoadContext context, bool getValueId = false)
		{
			ContainerSaveId containerSaveId = (ContainerSaveId)saveId;
			TypeDefinitionBase typeDefinitionBase;
			if (getValueId)
			{
				typeDefinitionBase = context.DefinitionContext.TryGetTypeDefinition(containerSaveId.ValueId);
			}
			else
			{
				typeDefinitionBase = context.DefinitionContext.TryGetTypeDefinition(containerSaveId.KeyId);
			}
			return Activator.CreateInstance(((StructDefinition)typeDefinitionBase).Type);
		}

		// Token: 0x040000A2 RID: 162
		private SaveId _saveId;

		// Token: 0x040000A3 RID: 163
		private int _elementCount;

		// Token: 0x040000A4 RID: 164
		private ContainerType _containerType;

		// Token: 0x040000A5 RID: 165
		private ElementLoadData[] _keys;

		// Token: 0x040000A6 RID: 166
		private ElementLoadData[] _values;

		// Token: 0x040000A7 RID: 167
		private Dictionary<int, ObjectLoadData> _childStructs;
	}
}
