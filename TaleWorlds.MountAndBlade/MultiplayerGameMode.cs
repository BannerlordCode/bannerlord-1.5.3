using System;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E7 RID: 743
	public abstract class MultiplayerGameMode
	{
		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06002B42 RID: 11074 RVA: 0x000A7063 File Offset: 0x000A5263
		// (set) Token: 0x06002B43 RID: 11075 RVA: 0x000A706B File Offset: 0x000A526B
		public string Name { get; private set; }

		// Token: 0x06002B44 RID: 11076 RVA: 0x000A7074 File Offset: 0x000A5274
		protected MultiplayerGameMode(string name)
		{
			this.Name = name;
		}

		// Token: 0x06002B45 RID: 11077
		public abstract void JoinCustomGame(JoinGameData joinGameData);

		// Token: 0x06002B46 RID: 11078
		public abstract void StartMultiplayerGame(string scene);
	}
}
