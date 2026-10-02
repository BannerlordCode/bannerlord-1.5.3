using System;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x02000027 RID: 39
	internal class ElementSaveData : VariableSaveData
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00007DFF File Offset: 0x00005FFF
		// (set) Token: 0x06000175 RID: 373 RVA: 0x00007E07 File Offset: 0x00006007
		public object ElementValue { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00007E10 File Offset: 0x00006010
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00007E18 File Offset: 0x00006018
		public int ElementIndex { get; private set; }

		// Token: 0x06000178 RID: 376 RVA: 0x00007E24 File Offset: 0x00006024
		public ElementSaveData(ContainerSaveData containerSaveData, object value, int index)
			: base(containerSaveData.Context)
		{
			this.ElementValue = value;
			this.ElementIndex = index;
			if (value == null)
			{
				base.InitializeDataAsNullObject(MemberTypeId.Invalid);
				return;
			}
			TypeDefinitionBase typeDefinition = containerSaveData.Context.DefinitionContext.GetTypeDefinition(value.GetType());
			TypeDefinition typeDefinition2 = typeDefinition as TypeDefinition;
			if (typeDefinition2 != null && !typeDefinition2.IsClassDefinition)
			{
				base.InitializeDataAsCustomStruct(MemberTypeId.Invalid, index, typeDefinition);
				return;
			}
			base.InitializeData(MemberTypeId.Invalid, value.GetType(), typeDefinition, value);
		}
	}
}
