using System;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x02000041 RID: 65
	internal abstract class VariableLoadData
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000CE17 File Offset: 0x0000B017
		// (set) Token: 0x0600029A RID: 666 RVA: 0x0000CE1F File Offset: 0x0000B01F
		public LoadContext Context { get; private set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000CE28 File Offset: 0x0000B028
		// (set) Token: 0x0600029C RID: 668 RVA: 0x0000CE30 File Offset: 0x0000B030
		public MemberTypeId MemberSaveId { get; private set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000CE39 File Offset: 0x0000B039
		// (set) Token: 0x0600029E RID: 670 RVA: 0x0000CE41 File Offset: 0x0000B041
		public SavedMemberType SavedMemberType { get; private set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000CE4A File Offset: 0x0000B04A
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x0000CE52 File Offset: 0x0000B052
		public object Data { get; private set; }

		// Token: 0x060002A1 RID: 673 RVA: 0x0000CE5B File Offset: 0x0000B05B
		protected VariableLoadData(LoadContext context, IReader reader)
		{
			this.Context = context;
			this._reader = reader;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000CE74 File Offset: 0x0000B074
		public void Read()
		{
			this.SavedMemberType = (SavedMemberType)this._reader.ReadByte();
			this.MemberSaveId = new MemberTypeId
			{
				TypeLevel = this._reader.ReadByte(),
				LocalSaveId = this._reader.ReadShort()
			};
			if (this.SavedMemberType == SavedMemberType.Object)
			{
				this.Data = this._reader.ReadInt();
				return;
			}
			if (this.SavedMemberType == SavedMemberType.Container)
			{
				this.Data = this._reader.ReadInt();
				return;
			}
			if (this.SavedMemberType == SavedMemberType.String)
			{
				this.Data = this._reader.ReadInt();
				return;
			}
			if (this.SavedMemberType == SavedMemberType.Enum)
			{
				this._saveId = SaveId.ReadSaveIdFrom(this._reader);
				this._typeDefinition = this.Context.DefinitionContext.TryGetTypeDefinition(this._saveId);
				string text = this._reader.ReadString();
				EnumDefinition enumDefinition = (EnumDefinition)this._typeDefinition;
				if (((enumDefinition != null) ? enumDefinition.Resolver : null) != null)
				{
					this.Data = enumDefinition.Resolver.ResolveObject(text);
					return;
				}
				this.Data = text;
				return;
			}
			else
			{
				if (this.SavedMemberType == SavedMemberType.BasicType)
				{
					this._saveId = SaveId.ReadSaveIdFrom(this._reader);
					this._typeDefinition = this.Context.DefinitionContext.TryGetTypeDefinition(this._saveId);
					BasicTypeDefinition basicTypeDefinition = (BasicTypeDefinition)this._typeDefinition;
					this.Data = basicTypeDefinition.Serializer.Deserialize(this._reader);
					return;
				}
				if (this.SavedMemberType == SavedMemberType.CustomStruct)
				{
					this.Data = this._reader.ReadInt();
				}
				return;
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000D014 File Offset: 0x0000B214
		public void SetCustomStructData(object customStructObject)
		{
			this._customStructObject = customStructObject;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000D020 File Offset: 0x0000B220
		public object GetDataToUse()
		{
			object obj = null;
			if (this.SavedMemberType == SavedMemberType.Object)
			{
				ObjectHeaderLoadData objectWithId = this.Context.GetObjectWithId((int)this.Data);
				if (objectWithId != null)
				{
					obj = objectWithId.Target;
				}
			}
			else if (this.SavedMemberType == SavedMemberType.Container)
			{
				ContainerHeaderLoadData containerWithId = this.Context.GetContainerWithId((int)this.Data);
				if (containerWithId != null)
				{
					obj = containerWithId.Target;
				}
			}
			else if (this.SavedMemberType == SavedMemberType.String)
			{
				int num = (int)this.Data;
				obj = this.Context.GetStringWithId(num);
			}
			else if (this.SavedMemberType == SavedMemberType.Enum)
			{
				if (this._typeDefinition == null)
				{
					obj = (string)this.Data;
				}
				else
				{
					EnumDefinition enumDefinition = (EnumDefinition)this._typeDefinition;
					Type type = this._typeDefinition.Type;
					if (Enum.IsDefined(type, this.Data) || enumDefinition.HasFlags)
					{
						obj = Enum.Parse(type, (string)this.Data);
					}
				}
			}
			else if (this.SavedMemberType == SavedMemberType.BasicType)
			{
				obj = this.Data;
			}
			else if (this.SavedMemberType == SavedMemberType.CustomStruct)
			{
				obj = this._customStructObject;
			}
			return obj;
		}

		// Token: 0x040000D1 RID: 209
		private IReader _reader;

		// Token: 0x040000D6 RID: 214
		private TypeDefinitionBase _typeDefinition;

		// Token: 0x040000D7 RID: 215
		private SaveId _saveId;

		// Token: 0x040000D8 RID: 216
		private object _customStructObject;
	}
}
