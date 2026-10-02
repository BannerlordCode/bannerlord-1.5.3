using System;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x02000038 RID: 56
	internal class FieldLoadData : MemberLoadData
	{
		// Token: 0x0600023E RID: 574 RVA: 0x0000B7DE File Offset: 0x000099DE
		public FieldLoadData(ObjectLoadData objectLoadData, IReader reader)
			: base(objectLoadData, reader)
		{
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000B7E8 File Offset: 0x000099E8
		public void FillObject()
		{
			FieldDefinition fieldDefinitionWithId;
			if (base.ObjectLoadData.TypeDefinition == null || (fieldDefinitionWithId = base.ObjectLoadData.TypeDefinition.GetFieldDefinitionWithId(this.GetMemberTypeId())) == null)
			{
				return;
			}
			FieldInfo fieldInfo = fieldDefinitionWithId.FieldInfo;
			object target = base.ObjectLoadData.Target;
			object dataToUse = base.GetDataToUse();
			if (dataToUse != null && !fieldInfo.FieldType.IsInstanceOfType(dataToUse) && !LoadContext.TryConvertType(dataToUse.GetType(), fieldInfo.FieldType, ref dataToUse))
			{
				return;
			}
			fieldInfo.SetValue(target, dataToUse);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000B868 File Offset: 0x00009A68
		private MemberTypeId GetMemberTypeId()
		{
			MemberTypeId memberSaveId = base.MemberSaveId;
			base.Context.DefinitionContext.GetConflictedFieldMemberTypeId(base.ObjectLoadData.TypeDefinition, ref memberSaveId);
			return memberSaveId;
		}
	}
}
