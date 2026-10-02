using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000269 RID: 617
	public interface ICommanderInfo : IMissionBehavior
	{
		// Token: 0x14000031 RID: 49
		// (add) Token: 0x060022EB RID: 8939
		// (remove) Token: 0x060022EC RID: 8940
		event Action<BattleSideEnum, float> OnMoraleChangedEvent;

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x060022ED RID: 8941
		// (remove) Token: 0x060022EE RID: 8942
		event Action OnFlagNumberChangedEvent;

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x060022EF RID: 8943
		// (remove) Token: 0x060022F0 RID: 8944
		event Action<FlagCapturePoint, Team> OnCapturePointOwnerChangedEvent;

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060022F1 RID: 8945
		IEnumerable<FlagCapturePoint> AllCapturePoints { get; }

		// Token: 0x060022F2 RID: 8946
		Team GetFlagOwner(FlagCapturePoint flag);

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x060022F3 RID: 8947
		bool AreMoralesIndependent { get; }
	}
}
