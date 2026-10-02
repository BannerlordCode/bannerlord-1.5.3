using System;

namespace TaleWorlds.PlayerServices.Avatar
{
	// Token: 0x0200000E RID: 14
	public interface IAvatarService
	{
		// Token: 0x06000071 RID: 113
		AvatarData GetPlayerAvatar(PlayerId playerId);

		// Token: 0x06000072 RID: 114
		void Initialize();

		// Token: 0x06000073 RID: 115
		void ClearCache();

		// Token: 0x06000074 RID: 116
		bool IsInitialized();

		// Token: 0x06000075 RID: 117
		void Tick(float dt);
	}
}
