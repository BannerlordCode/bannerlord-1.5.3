using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200026A RID: 618
	public interface IAnalyticsFlagInfo : IMissionBehavior
	{
		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x060022F4 RID: 8948
		MBReadOnlyList<FlagCapturePoint> AllCapturePoints { get; }

		// Token: 0x060022F5 RID: 8949
		Team GetFlagOwnerTeam(FlagCapturePoint flag);
	}
}
