using System;

namespace TaleWorlds.Engine.Options
{
	// Token: 0x020000A6 RID: 166
	public interface INumericOptionData : IOptionData
	{
		// Token: 0x06000F65 RID: 3941
		float GetMinValue();

		// Token: 0x06000F66 RID: 3942
		float GetMaxValue();

		// Token: 0x06000F67 RID: 3943
		bool GetIsDiscrete();

		// Token: 0x06000F68 RID: 3944
		int GetDiscreteIncrementInterval();

		// Token: 0x06000F69 RID: 3945
		bool GetShouldUpdateContinuously();
	}
}
