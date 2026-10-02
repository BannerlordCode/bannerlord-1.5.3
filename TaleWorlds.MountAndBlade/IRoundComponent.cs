using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002AB RID: 683
	public interface IRoundComponent : IMissionBehavior
	{
		// Token: 0x14000040 RID: 64
		// (add) Token: 0x060025DE RID: 9694
		// (remove) Token: 0x060025DF RID: 9695
		event Action OnRoundStarted;

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x060025E0 RID: 9696
		// (remove) Token: 0x060025E1 RID: 9697
		event Action OnPreparationEnded;

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x060025E2 RID: 9698
		// (remove) Token: 0x060025E3 RID: 9699
		event Action OnPreRoundEnding;

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x060025E4 RID: 9700
		// (remove) Token: 0x060025E5 RID: 9701
		event Action OnRoundEnding;

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x060025E6 RID: 9702
		// (remove) Token: 0x060025E7 RID: 9703
		event Action OnPostRoundEnded;

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x060025E8 RID: 9704
		// (remove) Token: 0x060025E9 RID: 9705
		event Action OnCurrentRoundStateChanged;

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x060025EA RID: 9706
		float LastRoundEndRemainingTime { get; }

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x060025EB RID: 9707
		float RemainingRoundTime { get; }

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x060025EC RID: 9708
		MultiplayerRoundState CurrentRoundState { get; }

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x060025ED RID: 9709
		int RoundCount { get; }

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060025EE RID: 9710
		BattleSideEnum RoundWinner { get; }

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060025EF RID: 9711
		RoundEndReason RoundEndReason { get; }
	}
}
