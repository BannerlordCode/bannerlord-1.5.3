using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200037C RID: 892
	public interface IOrderableWithInteractionArea : IOrderable
	{
		// Token: 0x06003321 RID: 13089
		bool IsPointInsideInteractionArea(Vec3 point);
	}
}
