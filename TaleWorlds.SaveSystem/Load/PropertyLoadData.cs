using System;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x02000040 RID: 64
	internal class PropertyLoadData : MemberLoadData
	{
		// Token: 0x06000296 RID: 662 RVA: 0x0000CD44 File Offset: 0x0000AF44
		public PropertyLoadData(ObjectLoadData objectLoadData, IReader reader)
			: base(objectLoadData, reader)
		{
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000CD50 File Offset: 0x0000AF50
		public void FillObject()
		{
			PropertyDefinition propertyDefinitionWithId;
			if (base.ObjectLoadData.TypeDefinition == null || (propertyDefinitionWithId = base.ObjectLoadData.TypeDefinition.GetPropertyDefinitionWithId(this.GetMemberTypeId())) == null)
			{
				return;
			}
			MethodInfo setMethod = propertyDefinitionWithId.SetMethod;
			object target = base.ObjectLoadData.Target;
			object dataToUse = base.GetDataToUse();
			if (dataToUse != null && !propertyDefinitionWithId.PropertyInfo.PropertyType.IsInstanceOfType(dataToUse) && !LoadContext.TryConvertType(dataToUse.GetType(), propertyDefinitionWithId.PropertyInfo.PropertyType, ref dataToUse))
			{
				return;
			}
			setMethod.Invoke(target, new object[] { dataToUse });
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000CDE4 File Offset: 0x0000AFE4
		private MemberTypeId GetMemberTypeId()
		{
			MemberTypeId memberSaveId = base.MemberSaveId;
			base.Context.DefinitionContext.GetConflictedPropertyMemberTypeId(base.ObjectLoadData.TypeDefinition, ref memberSaveId);
			return memberSaveId;
		}
	}
}
