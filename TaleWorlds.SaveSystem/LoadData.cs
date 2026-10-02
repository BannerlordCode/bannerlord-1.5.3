using System;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000011 RID: 17
	public class LoadData
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004F RID: 79 RVA: 0x000033A0 File Offset: 0x000015A0
		// (set) Token: 0x06000050 RID: 80 RVA: 0x000033A8 File Offset: 0x000015A8
		public MetaData MetaData { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000033B1 File Offset: 0x000015B1
		// (set) Token: 0x06000052 RID: 82 RVA: 0x000033B9 File Offset: 0x000015B9
		public GameData GameData { get; private set; }

		// Token: 0x06000053 RID: 83 RVA: 0x000033C2 File Offset: 0x000015C2
		public LoadData(MetaData metaData, GameData gameData)
		{
			this.MetaData = metaData;
			this.GameData = gameData;
		}
	}
}
