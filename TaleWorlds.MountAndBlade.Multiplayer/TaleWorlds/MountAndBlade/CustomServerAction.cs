using System;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000011 RID: 17
	public class CustomServerAction
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000539D File Offset: 0x0000359D
		// (set) Token: 0x06000103 RID: 259 RVA: 0x000053A5 File Offset: 0x000035A5
		public Action Execute { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000053AE File Offset: 0x000035AE
		// (set) Token: 0x06000105 RID: 261 RVA: 0x000053B6 File Offset: 0x000035B6
		public GameServerEntry GameServerEntry { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000053BF File Offset: 0x000035BF
		// (set) Token: 0x06000107 RID: 263 RVA: 0x000053C7 File Offset: 0x000035C7
		public string Name { get; private set; }

		// Token: 0x06000108 RID: 264 RVA: 0x000053D0 File Offset: 0x000035D0
		public CustomServerAction(Action execute, GameServerEntry gameServerEntry, string name)
		{
			this.Execute = execute;
			this.GameServerEntry = gameServerEntry;
			this.Name = name;
		}
	}
}
