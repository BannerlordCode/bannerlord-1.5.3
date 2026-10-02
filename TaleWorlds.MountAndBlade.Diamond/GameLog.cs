using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200011C RID: 284
	[Serializable]
	public class GameLog
	{
		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x00008152 File Offset: 0x00006352
		// (set) Token: 0x0600063C RID: 1596 RVA: 0x0000815A File Offset: 0x0000635A
		public int Id { get; set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x00008163 File Offset: 0x00006363
		// (set) Token: 0x0600063E RID: 1598 RVA: 0x0000816B File Offset: 0x0000636B
		public GameLogType Type { get; set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x00008174 File Offset: 0x00006374
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x0000817C File Offset: 0x0000637C
		public PlayerId Player { get; set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x00008185 File Offset: 0x00006385
		// (set) Token: 0x06000642 RID: 1602 RVA: 0x0000818D File Offset: 0x0000638D
		public float GameTime { get; set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x00008196 File Offset: 0x00006396
		// (set) Token: 0x06000644 RID: 1604 RVA: 0x0000819E File Offset: 0x0000639E
		public Dictionary<string, string> Data { get; set; }

		// Token: 0x06000645 RID: 1605 RVA: 0x000081A7 File Offset: 0x000063A7
		public GameLog()
		{
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x000081AF File Offset: 0x000063AF
		public GameLog(GameLogType type, PlayerId player, float gameTime)
		{
			this.Type = type;
			this.Player = player;
			this.GameTime = gameTime;
			this.Data = new Dictionary<string, string>();
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x000081D8 File Offset: 0x000063D8
		public string GetDataAsString()
		{
			string text = "{}";
			try
			{
				text = JsonConvert.SerializeObject(this.Data, Formatting.None);
			}
			catch (Exception)
			{
			}
			return text;
		}
	}
}
