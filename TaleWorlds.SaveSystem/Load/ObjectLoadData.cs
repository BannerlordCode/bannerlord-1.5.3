using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x0200003F RID: 63
	public class ObjectLoadData
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000C6B2 File Offset: 0x0000A8B2
		// (set) Token: 0x06000281 RID: 641 RVA: 0x0000C6BA File Offset: 0x0000A8BA
		public int Id { get; private set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000C6C3 File Offset: 0x0000A8C3
		// (set) Token: 0x06000283 RID: 643 RVA: 0x0000C6CB File Offset: 0x0000A8CB
		public object Target { get; private set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000C6D4 File Offset: 0x0000A8D4
		// (set) Token: 0x06000285 RID: 645 RVA: 0x0000C6DC File Offset: 0x0000A8DC
		public LoadContext Context { get; private set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000C6E5 File Offset: 0x0000A8E5
		// (set) Token: 0x06000287 RID: 647 RVA: 0x0000C6ED File Offset: 0x0000A8ED
		public TypeDefinition TypeDefinition { get; private set; }

		// Token: 0x06000288 RID: 648 RVA: 0x0000C6F8 File Offset: 0x0000A8F8
		public object GetDataBySaveId(int localSaveId)
		{
			MemberLoadData memberLoadData = this._memberValues.SingleOrDefault<MemberLoadData>((MemberLoadData value) => (int)value.MemberSaveId.LocalSaveId == localSaveId);
			if (memberLoadData != null)
			{
				return memberLoadData.GetDataToUse();
			}
			return null;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000C738 File Offset: 0x0000A938
		public object GetMemberValueBySaveId(int localSaveId, int typeLevel)
		{
			MemberLoadData memberLoadData = this._memberValues.SingleOrDefault<MemberLoadData>((MemberLoadData value) => (int)value.MemberSaveId.LocalSaveId == localSaveId && (int)value.MemberSaveId.TypeLevel == typeLevel);
			if (memberLoadData == null)
			{
				return null;
			}
			return memberLoadData.GetDataToUse();
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000C77C File Offset: 0x0000A97C
		public object GetMemberValueBySaveId(int localSaveId)
		{
			MemberLoadData memberLoadData = this._memberValues.SingleOrDefault<MemberLoadData>((MemberLoadData value) => (int)value.MemberSaveId.LocalSaveId == localSaveId);
			if (memberLoadData == null)
			{
				return null;
			}
			return memberLoadData.GetDataToUse();
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000C7B8 File Offset: 0x0000A9B8
		public object GetFieldValueBySaveId(int localSaveId)
		{
			FieldLoadData fieldLoadData = this._fieldValues.SingleOrDefault<FieldLoadData>((FieldLoadData value) => (int)value.MemberSaveId.LocalSaveId == localSaveId);
			if (fieldLoadData == null)
			{
				return null;
			}
			return fieldLoadData.GetDataToUse();
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000C7F4 File Offset: 0x0000A9F4
		public object GetPropertyValueBySaveId(int localSaveId)
		{
			PropertyLoadData propertyLoadData = this._propertyValues.SingleOrDefault<PropertyLoadData>((PropertyLoadData value) => (int)value.MemberSaveId.LocalSaveId == localSaveId);
			if (propertyLoadData == null)
			{
				return null;
			}
			return propertyLoadData.GetDataToUse();
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000C830 File Offset: 0x0000AA30
		public bool HasMember(int localSaveId)
		{
			return this._memberValues.Any<MemberLoadData>((MemberLoadData x) => (int)x.MemberSaveId.LocalSaveId == localSaveId);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000C864 File Offset: 0x0000AA64
		public bool HasMember(int localSaveId, int typeLevel)
		{
			return this._memberValues.Any<MemberLoadData>((MemberLoadData x) => (int)x.MemberSaveId.LocalSaveId == localSaveId && (int)x.MemberSaveId.TypeLevel == typeLevel);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000C89C File Offset: 0x0000AA9C
		public ObjectLoadData(LoadContext context, int id)
		{
			this.Context = context;
			this.Id = id;
			this._propertyValues = new List<PropertyLoadData>();
			this._fieldValues = new List<FieldLoadData>();
			this._memberValues = new List<MemberLoadData>();
			this._childStructs = new List<ObjectLoadData>();
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000C8EC File Offset: 0x0000AAEC
		public ObjectLoadData(ObjectHeaderLoadData headerLoadData)
		{
			this.Id = headerLoadData.Id;
			this.Target = headerLoadData.Target;
			this.Context = headerLoadData.Context;
			this.TypeDefinition = headerLoadData.TypeDefinition;
			this._propertyValues = new List<PropertyLoadData>();
			this._fieldValues = new List<FieldLoadData>();
			this._memberValues = new List<MemberLoadData>();
			this._childStructs = new List<ObjectLoadData>();
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000C95C File Offset: 0x0000AB5C
		public void InitializeReaders(SaveEntryFolder saveEntryFolder)
		{
			BinaryReader binaryReader = saveEntryFolder.GetEntry(new EntryId(-1, SaveEntryExtension.Basics)).GetBinaryReader();
			this._saveId = SaveId.ReadSaveIdFrom(binaryReader);
			this._propertyCount = binaryReader.ReadShort();
			this._childStructCount = binaryReader.ReadShort();
			for (int i = 0; i < (int)this._childStructCount; i++)
			{
				ObjectLoadData objectLoadData = new ObjectLoadData(this.Context, i);
				this._childStructs.Add(objectLoadData);
			}
			foreach (SaveEntry saveEntry in saveEntryFolder.ChildEntries)
			{
				if (saveEntry.Id.Extension == SaveEntryExtension.Property)
				{
					BinaryReader binaryReader2 = saveEntry.GetBinaryReader();
					PropertyLoadData propertyLoadData = new PropertyLoadData(this, binaryReader2);
					this._propertyValues.Add(propertyLoadData);
					this._memberValues.Add(propertyLoadData);
				}
				else if (saveEntry.Id.Extension == SaveEntryExtension.Field)
				{
					BinaryReader binaryReader3 = saveEntry.GetBinaryReader();
					FieldLoadData fieldLoadData = new FieldLoadData(this, binaryReader3);
					this._fieldValues.Add(fieldLoadData);
					this._memberValues.Add(fieldLoadData);
				}
			}
			for (int j = 0; j < (int)this._childStructCount; j++)
			{
				ObjectLoadData objectLoadData2 = this._childStructs[j];
				SaveEntryFolder childFolder = saveEntryFolder.GetChildFolder(new FolderId(j, SaveFolderExtension.Struct));
				objectLoadData2.InitializeReaders(childFolder);
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000CACC File Offset: 0x0000ACCC
		public void CreateStruct()
		{
			this.TypeDefinition = this.Context.DefinitionContext.TryGetTypeDefinition(this._saveId) as TypeDefinition;
			if (this.TypeDefinition != null)
			{
				Type type = this.TypeDefinition.Type;
				this.Target = FormatterServices.GetUninitializedObject(type);
			}
			foreach (ObjectLoadData objectLoadData in this._childStructs)
			{
				objectLoadData.CreateStruct();
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000CB60 File Offset: 0x0000AD60
		public void FillCreatedObject()
		{
			foreach (ObjectLoadData objectLoadData in this._childStructs)
			{
				objectLoadData.CreateStruct();
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000CBB0 File Offset: 0x0000ADB0
		public void Read()
		{
			foreach (ObjectLoadData objectLoadData in this._childStructs)
			{
				objectLoadData.Read();
			}
			foreach (MemberLoadData memberLoadData in this._memberValues)
			{
				memberLoadData.Read();
				if (memberLoadData.SavedMemberType == SavedMemberType.CustomStruct)
				{
					int num = (int)memberLoadData.Data;
					object target = this._childStructs[num].Target;
					memberLoadData.SetCustomStructData(target);
				}
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000CC74 File Offset: 0x0000AE74
		public void FillObject()
		{
			foreach (ObjectLoadData objectLoadData in this._childStructs)
			{
				objectLoadData.FillObject();
			}
			foreach (FieldLoadData fieldLoadData in this._fieldValues)
			{
				fieldLoadData.FillObject();
			}
			foreach (PropertyLoadData propertyLoadData in this._propertyValues)
			{
				propertyLoadData.FillObject();
			}
		}

		// Token: 0x040000CA RID: 202
		private short _propertyCount;

		// Token: 0x040000CB RID: 203
		private List<PropertyLoadData> _propertyValues;

		// Token: 0x040000CC RID: 204
		private List<FieldLoadData> _fieldValues;

		// Token: 0x040000CD RID: 205
		private List<MemberLoadData> _memberValues;

		// Token: 0x040000CE RID: 206
		private SaveId _saveId;

		// Token: 0x040000CF RID: 207
		private List<ObjectLoadData> _childStructs;

		// Token: 0x040000D0 RID: 208
		private short _childStructCount;
	}
}
