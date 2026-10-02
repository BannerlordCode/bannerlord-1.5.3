using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x0200002C RID: 44
	internal class ObjectSaveData
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00008A16 File Offset: 0x00006C16
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00008A1E File Offset: 0x00006C1E
		public int ObjectId { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00008A27 File Offset: 0x00006C27
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00008A2F File Offset: 0x00006C2F
		public ISaveContext Context { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00008A38 File Offset: 0x00006C38
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00008A40 File Offset: 0x00006C40
		public object Target { get; private set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00008A49 File Offset: 0x00006C49
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00008A51 File Offset: 0x00006C51
		public Type Type { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00008A5A File Offset: 0x00006C5A
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00008A62 File Offset: 0x00006C62
		public bool IsClass { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00008A6B File Offset: 0x00006C6B
		internal int PropertyCount
		{
			get
			{
				return this._propertyValues.Count;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00008A78 File Offset: 0x00006C78
		internal int FieldCount
		{
			get
			{
				return this._fieldValues.Count;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00008A85 File Offset: 0x00006C85
		internal int ChildCount
		{
			get
			{
				return this._childStructs.Count;
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00008A94 File Offset: 0x00006C94
		public int GetFolderCount()
		{
			int num = 1;
			foreach (KeyValuePair<MemberDefinition, ObjectSaveData> keyValuePair in this._childStructs)
			{
				num += keyValuePair.Value.GetFolderCount();
			}
			return num;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00008AF4 File Offset: 0x00006CF4
		public ObjectSaveData(ISaveContext context, int objectId, object target, bool isClass)
		{
			this.ObjectId = objectId;
			this.Context = context;
			this.Target = target;
			this.IsClass = isClass;
			this._stringMembers = new List<MemberSaveData>();
			this.Type = target.GetType();
			if (this.IsClass)
			{
				this._typeDefinition = context.DefinitionContext.GetClassDefinition(this.Type);
			}
			else
			{
				this._typeDefinition = context.DefinitionContext.GetStructDefinition(this.Type);
			}
			this._childStructs = new Dictionary<MemberDefinition, ObjectSaveData>(3);
			this._propertyValues = new Dictionary<PropertyInfo, PropertySaveData>(this._typeDefinition.PropertyDefinitions.Count);
			this._fieldValues = new Dictionary<FieldInfo, FieldSaveData>(this._typeDefinition.FieldDefinitions.Count);
			if (this._typeDefinition == null)
			{
				throw new Exception("Could not find type definition of type: " + this.Type);
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00008BD4 File Offset: 0x00006DD4
		public int GetEntryCount()
		{
			int num = 1 + this.PropertyCount + this.FieldCount;
			foreach (KeyValuePair<MemberDefinition, ObjectSaveData> keyValuePair in this._childStructs)
			{
				num += keyValuePair.Value.GetEntryCount();
			}
			return num;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00008C40 File Offset: 0x00006E40
		public void CollectMembers()
		{
			for (int i = 0; i < this._typeDefinition.MemberDefinitions.Count; i++)
			{
				MemberDefinition memberDefinition = this._typeDefinition.MemberDefinitions[i];
				MemberSaveData memberSaveData = null;
				if (memberDefinition is PropertyDefinition)
				{
					PropertyDefinition propertyDefinition = (PropertyDefinition)memberDefinition;
					PropertyInfo propertyInfo = propertyDefinition.PropertyInfo;
					MemberTypeId id = propertyDefinition.Id;
					PropertySaveData propertySaveData = new PropertySaveData(this, propertyDefinition, id);
					this._propertyValues.Add(propertyInfo, propertySaveData);
					memberSaveData = propertySaveData;
				}
				else if (memberDefinition is FieldDefinition)
				{
					FieldDefinition fieldDefinition = (FieldDefinition)memberDefinition;
					FieldInfo fieldInfo = fieldDefinition.FieldInfo;
					MemberTypeId id2 = fieldDefinition.Id;
					FieldSaveData fieldSaveData = new FieldSaveData(this, fieldDefinition, id2);
					this._fieldValues.Add(fieldInfo, fieldSaveData);
					memberSaveData = fieldSaveData;
				}
				Type memberType = memberDefinition.GetMemberType();
				TypeDefinitionBase typeDefinition = this.Context.DefinitionContext.GetTypeDefinition(memberType);
				TypeDefinition typeDefinition2 = typeDefinition as TypeDefinition;
				if (typeDefinition2 != null && !typeDefinition2.IsClassDefinition)
				{
					ObjectSaveData objectSaveData = this._childStructs[memberDefinition];
					memberSaveData.InitializeAsCustomStruct(objectSaveData.ObjectId);
				}
				else
				{
					memberSaveData.Initialize(typeDefinition);
				}
				if (memberSaveData.MemberType == SavedMemberType.String)
				{
					this._stringMembers.Add(memberSaveData);
				}
			}
			foreach (ObjectSaveData objectSaveData2 in this._childStructs.Values)
			{
				objectSaveData2.CollectMembers();
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00008DB8 File Offset: 0x00006FB8
		public void CollectStringsInto(List<string> collection)
		{
			for (int i = 0; i < this._stringMembers.Count; i++)
			{
				string text = (string)this._stringMembers[i].Value;
				collection.Add(text);
			}
			foreach (ObjectSaveData objectSaveData in this._childStructs.Values)
			{
				objectSaveData.CollectStringsInto(collection);
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00008E44 File Offset: 0x00007044
		public void CollectStrings()
		{
			for (int i = 0; i < this._stringMembers.Count; i++)
			{
				string text = (string)this._stringMembers[i].Value;
				this.Context.AddOrGetStringId(text);
			}
			foreach (ObjectSaveData objectSaveData in this._childStructs.Values)
			{
				objectSaveData.CollectStrings();
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00008ED4 File Offset: 0x000070D4
		public void CollectStructs()
		{
			int num = 0;
			for (int i = 0; i < this._typeDefinition.MemberDefinitions.Count; i++)
			{
				MemberDefinition memberDefinition = this._typeDefinition.MemberDefinitions[i];
				Type memberType = memberDefinition.GetMemberType();
				if (this.Context.DefinitionContext.GetStructDefinition(memberType) != null)
				{
					object value = memberDefinition.GetValue(this.Target);
					ObjectSaveData objectSaveData = new ObjectSaveData(this.Context, num, value, false);
					this._childStructs.Add(memberDefinition, objectSaveData);
					num++;
				}
			}
			foreach (ObjectSaveData objectSaveData2 in this._childStructs.Values)
			{
				objectSaveData2.CollectStructs();
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00008FA4 File Offset: 0x000071A4
		public void SaveHeaderTo(SaveEntryFolder parentFolder, IArchiveContext archiveContext)
		{
			SaveFolderExtension saveFolderExtension = (this.IsClass ? SaveFolderExtension.Object : SaveFolderExtension.Struct);
			SaveEntryFolder saveEntryFolder = archiveContext.CreateFolder(parentFolder, new FolderId(this.ObjectId, saveFolderExtension), 1);
			BinaryWriter binaryWriter = BinaryWriterFactory.GetBinaryWriter();
			this._typeDefinition.SaveId.WriteTo(binaryWriter);
			binaryWriter.WriteShort((short)this._propertyValues.Count);
			binaryWriter.WriteShort((short)this._childStructs.Count);
			saveEntryFolder.CreateEntry(new EntryId(-1, SaveEntryExtension.Basics)).FillFrom(binaryWriter);
			BinaryWriterFactory.ReleaseBinaryWriter(binaryWriter);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00009028 File Offset: 0x00007228
		public void SaveHeaderFolderTo(BinaryWriter headerWriter, int folderId)
		{
			headerWriter.Write3ByteInt(-1);
			headerWriter.Write3ByteInt(folderId);
			headerWriter.Write3ByteInt(this.ObjectId);
			SaveFolderExtension saveFolderExtension = (this.IsClass ? SaveFolderExtension.Object : SaveFolderExtension.Struct);
			headerWriter.WriteByte((byte)saveFolderExtension);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00009063 File Offset: 0x00007263
		public void SaveHeaderDataTo(BinaryWriter headerWriter, int folderId)
		{
			headerWriter.Write3ByteInt(folderId);
			headerWriter.Write3ByteInt(-1);
			headerWriter.WriteByte(8);
			this.WriteHeader(headerWriter);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00009081 File Offset: 0x00007281
		private int GetHeaderDataSize()
		{
			return 4 + this._typeDefinition.SaveId.GetSizeInBytes();
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00009095 File Offset: 0x00007295
		public int GetHeaderSize()
		{
			return this.GetHeaderDataSize() + 19;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000090A0 File Offset: 0x000072A0
		public int GetDataSize()
		{
			int num = 31 + this._typeDefinition.SaveId.GetSizeInBytes();
			foreach (KeyValuePair<FieldInfo, FieldSaveData> keyValuePair in this._fieldValues)
			{
				num += this.GetMemberEntrySize();
				num += keyValuePair.Value.GetDataSize();
			}
			foreach (KeyValuePair<PropertyInfo, PropertySaveData> keyValuePair2 in this._propertyValues)
			{
				num += this.GetMemberEntrySize();
				num += keyValuePair2.Value.GetDataSize();
			}
			foreach (KeyValuePair<MemberDefinition, ObjectSaveData> keyValuePair3 in this._childStructs)
			{
				num += keyValuePair3.Value.GetDataSize() - 8;
			}
			return num;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000091BC File Offset: 0x000073BC
		public void SaveDataFolder(BinaryWriter writer, int parentFolderId, ref int folderId)
		{
			writer.Write3ByteInt(parentFolderId);
			writer.Write3ByteInt(folderId);
			writer.Write3ByteInt(this.ObjectId);
			SaveFolderExtension saveFolderExtension = (this.IsClass ? SaveFolderExtension.Object : SaveFolderExtension.Struct);
			writer.WriteByte((byte)saveFolderExtension);
			int num = folderId;
			folderId++;
			foreach (KeyValuePair<MemberDefinition, ObjectSaveData> keyValuePair in this._childStructs)
			{
				keyValuePair.Value.SaveDataFolder(writer, num, ref folderId);
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00009250 File Offset: 0x00007450
		public void SaveTo(BinaryWriter writer, ref int folderId)
		{
			this.SaveHeaderDataTo(writer, folderId);
			int num = 0;
			foreach (KeyValuePair<FieldInfo, FieldSaveData> keyValuePair in this._fieldValues)
			{
				this.WriteMemberEntry(writer, keyValuePair.Value, folderId, num++, SaveEntryExtension.Field);
			}
			num = 0;
			foreach (KeyValuePair<PropertyInfo, PropertySaveData> keyValuePair2 in this._propertyValues)
			{
				this.WriteMemberEntry(writer, keyValuePair2.Value, folderId, num++, SaveEntryExtension.Property);
			}
			folderId++;
			foreach (KeyValuePair<MemberDefinition, ObjectSaveData> keyValuePair3 in this._childStructs)
			{
				keyValuePair3.Value.SaveTo(writer, ref folderId);
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00009364 File Offset: 0x00007564
		public void SaveTo(SaveEntryFolder parentFolder, IArchiveContext archiveContext)
		{
			SaveFolderExtension saveFolderExtension = (this.IsClass ? SaveFolderExtension.Object : SaveFolderExtension.Struct);
			int num = 1 + this._fieldValues.Values.Count + this._propertyValues.Values.Count;
			SaveEntryFolder saveEntryFolder = archiveContext.CreateFolder(parentFolder, new FolderId(this.ObjectId, saveFolderExtension), num);
			BinaryWriter binaryWriter = BinaryWriterFactory.GetBinaryWriter();
			this._typeDefinition.SaveId.WriteTo(binaryWriter);
			binaryWriter.WriteShort((short)this._propertyValues.Count);
			binaryWriter.WriteShort((short)this._childStructs.Count);
			saveEntryFolder.CreateEntry(new EntryId(-1, SaveEntryExtension.Basics)).FillFrom(binaryWriter);
			BinaryWriterFactory.ReleaseBinaryWriter(binaryWriter);
			int num2 = 0;
			foreach (VariableSaveData variableSaveData in this._fieldValues.Values)
			{
				BinaryWriter binaryWriter2 = BinaryWriterFactory.GetBinaryWriter();
				variableSaveData.SaveTo(binaryWriter2);
				saveEntryFolder.CreateEntry(new EntryId(num2, SaveEntryExtension.Field)).FillFrom(binaryWriter2);
				BinaryWriterFactory.ReleaseBinaryWriter(binaryWriter2);
				num2++;
			}
			int num3 = 0;
			foreach (VariableSaveData variableSaveData2 in this._propertyValues.Values)
			{
				BinaryWriter binaryWriter3 = BinaryWriterFactory.GetBinaryWriter();
				variableSaveData2.SaveTo(binaryWriter3);
				saveEntryFolder.CreateEntry(new EntryId(num3, SaveEntryExtension.Property)).FillFrom(binaryWriter3);
				BinaryWriterFactory.ReleaseBinaryWriter(binaryWriter3);
				num3++;
			}
			foreach (ObjectSaveData objectSaveData in this._childStructs.Values)
			{
				objectSaveData.SaveTo(saveEntryFolder, archiveContext);
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000953C File Offset: 0x0000773C
		private void WriteHeader(BinaryWriter writer)
		{
			int headerDataSize = this.GetHeaderDataSize();
			if (headerDataSize > 32767)
			{
				ISaveContext context = this.Context;
				string text = "[SaveSystem] Obj {0} header size {1} overflows short";
				Type type = this.Type;
				context.ReportSaveIntegrityDrift(string.Format(text, (type != null) ? type.FullName : null, headerDataSize));
			}
			writer.WriteShort((short)headerDataSize);
			this._typeDefinition.SaveId.WriteTo(writer);
			int count = this._propertyValues.Count;
			int count2 = this._childStructs.Count;
			if (count > 32767)
			{
				ISaveContext context2 = this.Context;
				string text2 = "[SaveSystem] Obj {0} property count {1} overflows short";
				Type type2 = this.Type;
				context2.ReportSaveIntegrityDrift(string.Format(text2, (type2 != null) ? type2.FullName : null, count));
			}
			if (count2 > 32767)
			{
				ISaveContext context3 = this.Context;
				string text3 = "[SaveSystem] Obj {0} child struct count {1} overflows short";
				Type type3 = this.Type;
				context3.ReportSaveIntegrityDrift(string.Format(text3, (type3 != null) ? type3.FullName : null, count2));
			}
			writer.WriteShort((short)count);
			writer.WriteShort((short)count2);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00009630 File Offset: 0x00007830
		private void WriteMemberEntry(BinaryWriter writer, VariableSaveData data, int parentFolderId, int id, SaveEntryExtension extension)
		{
			int dataSize = data.GetDataSize();
			BinaryWriter binaryWriter = new BinaryWriter((dataSize > 0) ? dataSize : 16);
			data.SaveTo(binaryWriter);
			int length = binaryWriter.Length;
			if (dataSize != length)
			{
				ISaveContext context = this.Context;
				string text = "[SaveSystem] Obj[{0}] {1} member {2}[{3}] len mismatch predicted={4} actual={5}";
				object[] array = new object[6];
				array[0] = this.ObjectId;
				int num = 1;
				Type type = this.Type;
				array[num] = ((type != null) ? type.FullName : null);
				array[2] = extension;
				array[3] = id;
				array[4] = dataSize;
				array[5] = length;
				context.ReportSaveIntegrityDrift(string.Format(text, array));
			}
			writer.Write3ByteInt(parentFolderId);
			writer.Write3ByteInt(id);
			writer.WriteByte((byte)extension);
			if (length > 32767)
			{
				ISaveContext context2 = this.Context;
				string text2 = "[SaveSystem] Obj {0} member {1}[{2}] size {3} overflows short";
				object[] array2 = new object[4];
				int num2 = 0;
				Type type2 = this.Type;
				array2[num2] = ((type2 != null) ? type2.FullName : null);
				array2[1] = extension;
				array2[2] = id;
				array2[3] = length;
				context2.ReportSaveIntegrityDrift(string.Format(text2, array2));
			}
			writer.WriteShort((short)length);
			writer.AppendData(binaryWriter);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00009744 File Offset: 0x00007944
		private int GetMemberEntrySize()
		{
			return 9;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00009748 File Offset: 0x00007948
		internal static void GetChildObjectFrom(ISaveContext context, object target, MemberDefinition memberDefinition, List<object> collectedObjects)
		{
			Type memberType = memberDefinition.GetMemberType();
			if (memberType.IsClass || memberType.IsInterface)
			{
				if (memberType != typeof(string))
				{
					object value = memberDefinition.GetValue(target);
					if (value != null)
					{
						collectedObjects.Add(value);
						return;
					}
				}
			}
			else
			{
				TypeDefinition structDefinition = context.DefinitionContext.GetStructDefinition(memberType);
				if (structDefinition != null)
				{
					object value2 = memberDefinition.GetValue(target);
					for (int i = 0; i < structDefinition.MemberDefinitions.Count; i++)
					{
						MemberDefinition memberDefinition2 = structDefinition.MemberDefinitions[i];
						ObjectSaveData.GetChildObjectFrom(context, value2, memberDefinition2, collectedObjects);
					}
				}
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000097DC File Offset: 0x000079DC
		public IEnumerable<object> GetChildObjects()
		{
			List<object> list = new List<object>();
			ObjectSaveData.GetChildObjects(this.Context, this._typeDefinition, this.Target, list);
			return list;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00009808 File Offset: 0x00007A08
		public static void GetChildObjects(ISaveContext context, TypeDefinition typeDefinition, object target, List<object> collectedObjects)
		{
			if (typeDefinition.CollectObjectsMethod != null)
			{
				typeDefinition.CollectObjectsMethod(target, collectedObjects);
				return;
			}
			for (int i = 0; i < typeDefinition.MemberDefinitions.Count; i++)
			{
				MemberDefinition memberDefinition = typeDefinition.MemberDefinitions[i];
				ObjectSaveData.GetChildObjectFrom(context, target, memberDefinition, collectedObjects);
			}
		}

		// Token: 0x04000075 RID: 117
		private Dictionary<PropertyInfo, PropertySaveData> _propertyValues;

		// Token: 0x04000076 RID: 118
		private Dictionary<FieldInfo, FieldSaveData> _fieldValues;

		// Token: 0x04000077 RID: 119
		private List<MemberSaveData> _stringMembers;

		// Token: 0x04000078 RID: 120
		private TypeDefinition _typeDefinition;

		// Token: 0x04000079 RID: 121
		private Dictionary<MemberDefinition, ObjectSaveData> _childStructs;
	}
}
