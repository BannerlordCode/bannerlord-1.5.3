using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000086 RID: 134
	public interface IFaceGeneratorCustomFilter
	{
		// Token: 0x0600089E RID: 2206
		int[] GetHaircutIndices(BasicCharacterObject character);

		// Token: 0x0600089F RID: 2207
		int[] GetFacialHairIndices(BasicCharacterObject character);

		// Token: 0x060008A0 RID: 2208
		FaceGeneratorStage[] GetAvailableStages();
	}
}
