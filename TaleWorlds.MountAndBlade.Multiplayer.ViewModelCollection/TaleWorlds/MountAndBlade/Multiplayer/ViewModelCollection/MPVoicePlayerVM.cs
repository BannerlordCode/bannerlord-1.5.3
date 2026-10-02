using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x0200000E RID: 14
	public class MPVoicePlayerVM : MPPlayerVM
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x000044A9 File Offset: 0x000026A9
		public MPVoicePlayerVM(MissionPeer peer)
			: base(peer)
		{
			this.UpdatesSinceSilence = 0;
			this.IsMyPeer = peer.IsMine;
		}

		// Token: 0x0400006C RID: 108
		public const int UpdatesRequiredToRemoveForSilence = 30;

		// Token: 0x0400006D RID: 109
		public readonly bool IsMyPeer;

		// Token: 0x0400006E RID: 110
		public int UpdatesSinceSilence;
	}
}
