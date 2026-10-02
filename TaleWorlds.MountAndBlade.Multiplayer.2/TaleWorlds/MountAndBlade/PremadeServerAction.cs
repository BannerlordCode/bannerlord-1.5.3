using System;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000013 RID: 19
	public class PremadeServerAction
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000053ED File Offset: 0x000035ED
		// (set) Token: 0x0600010E RID: 270 RVA: 0x000053F5 File Offset: 0x000035F5
		public Action Execute { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000053FE File Offset: 0x000035FE
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00005406 File Offset: 0x00003606
		public PremadeGameEntry GameServerEntry { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000540F File Offset: 0x0000360F
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00005417 File Offset: 0x00003617
		public string Name { get; private set; }

		// Token: 0x06000113 RID: 275 RVA: 0x00005420 File Offset: 0x00003620
		public PremadeServerAction(Action execute, PremadeGameEntry gameServerEntry, string name)
		{
			this.Execute = execute;
			this.GameServerEntry = gameServerEntry;
			this.Name = name;
		}
	}
}
