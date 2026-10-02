using System;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x0200002D RID: 45
	internal class PropertySaveData : MemberSaveData
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00009857 File Offset: 0x00007A57
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x0000985F File Offset: 0x00007A5F
		public PropertyDefinition PropertyDefinition { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00009868 File Offset: 0x00007A68
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00009870 File Offset: 0x00007A70
		public MemberTypeId SaveId { get; private set; }

		// Token: 0x060001CB RID: 459 RVA: 0x00009879 File Offset: 0x00007A79
		public PropertySaveData(ObjectSaveData objectSaveData, PropertyDefinition propertyDefinition, MemberTypeId saveId)
			: base(objectSaveData)
		{
			this.PropertyDefinition = propertyDefinition;
			this.SaveId = saveId;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00009890 File Offset: 0x00007A90
		public override void Initialize(TypeDefinitionBase typeDefinition)
		{
			object value = this.PropertyDefinition.GetValue(base.ObjectSaveData.Target);
			base.InitializeData(this.SaveId, this.PropertyDefinition.PropertyInfo.PropertyType, typeDefinition, value);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000098D2 File Offset: 0x00007AD2
		public override void InitializeAsCustomStruct(int structId)
		{
			base.InitializeDataAsCustomStruct(this.SaveId, structId, base.TypeDefinition);
		}
	}
}
