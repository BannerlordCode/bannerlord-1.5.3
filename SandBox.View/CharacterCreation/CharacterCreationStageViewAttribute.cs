using System;

namespace SandBox.View.CharacterCreation
{
	// Token: 0x02000080 RID: 128
	public sealed class CharacterCreationStageViewAttribute : Attribute
	{
		// Token: 0x0600058A RID: 1418 RVA: 0x000296F4 File Offset: 0x000278F4
		public CharacterCreationStageViewAttribute(Type stageType)
		{
			this.StageType = stageType;
		}

		// Token: 0x04000299 RID: 665
		public readonly Type StageType;
	}
}
