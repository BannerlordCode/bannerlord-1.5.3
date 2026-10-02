using System;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x0200002B RID: 43
	internal abstract class MemberSaveData : VariableSaveData
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000089F0 File Offset: 0x00006BF0
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x000089F8 File Offset: 0x00006BF8
		public ObjectSaveData ObjectSaveData { get; private set; }

		// Token: 0x060001A1 RID: 417 RVA: 0x00008A01 File Offset: 0x00006C01
		protected MemberSaveData(ObjectSaveData objectSaveData)
			: base(objectSaveData.Context)
		{
			this.ObjectSaveData = objectSaveData;
		}

		// Token: 0x060001A2 RID: 418
		public abstract void Initialize(TypeDefinitionBase typeDefinition);

		// Token: 0x060001A3 RID: 419
		public abstract void InitializeAsCustomStruct(int structId);
	}
}
