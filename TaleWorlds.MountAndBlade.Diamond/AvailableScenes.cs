using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000ED RID: 237
	[Serializable]
	public class AvailableScenes
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x00005324 File Offset: 0x00003524
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x0000532B File Offset: 0x0000352B
		public static AvailableScenes Empty { get; private set; } = new AvailableScenes(new Dictionary<string, string[]>());

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00005344 File Offset: 0x00003544
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x0000534C File Offset: 0x0000354C
		public Dictionary<string, string[]> ScenesByGameTypes { get; set; }

		// Token: 0x06000498 RID: 1176 RVA: 0x00005355 File Offset: 0x00003555
		public AvailableScenes()
		{
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0000535D File Offset: 0x0000355D
		public AvailableScenes(Dictionary<string, string[]> scenesByGameTypes)
		{
			this.ScenesByGameTypes = scenesByGameTypes;
		}
	}
}
