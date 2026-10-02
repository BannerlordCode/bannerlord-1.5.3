using System;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x02000028 RID: 40
	internal class FieldSaveData : MemberSaveData
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00007EA4 File Offset: 0x000060A4
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00007EAC File Offset: 0x000060AC
		public FieldDefinition FieldDefinition { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00007EB5 File Offset: 0x000060B5
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00007EBD File Offset: 0x000060BD
		public MemberTypeId SaveId { get; private set; }

		// Token: 0x0600017D RID: 381 RVA: 0x00007EC6 File Offset: 0x000060C6
		public FieldSaveData(ObjectSaveData objectSaveData, FieldDefinition fieldDefinition, MemberTypeId saveId)
			: base(objectSaveData)
		{
			this.FieldDefinition = fieldDefinition;
			this.SaveId = saveId;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007EE0 File Offset: 0x000060E0
		public override void Initialize(TypeDefinitionBase typeDefinition)
		{
			object value = this.FieldDefinition.GetValue(base.ObjectSaveData.Target);
			Type fieldType = this.FieldDefinition.FieldInfo.FieldType;
			base.InitializeData(this.SaveId, fieldType, typeDefinition, value);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007F24 File Offset: 0x00006124
		public override void InitializeAsCustomStruct(int structId)
		{
			base.InitializeDataAsCustomStruct(this.SaveId, structId, base.TypeDefinition);
		}
	}
}
