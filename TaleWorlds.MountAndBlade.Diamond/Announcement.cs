using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000EA RID: 234
	[Serializable]
	public class Announcement
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x0000520E File Offset: 0x0000340E
		// (set) Token: 0x06000484 RID: 1156 RVA: 0x00005216 File Offset: 0x00003416
		public int Id { get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x0000521F File Offset: 0x0000341F
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x00005227 File Offset: 0x00003427
		public Guid BattleId { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x00005230 File Offset: 0x00003430
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x00005238 File Offset: 0x00003438
		public AnnouncementType Type { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x00005241 File Offset: 0x00003441
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x00005249 File Offset: 0x00003449
		public string Text { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x00005252 File Offset: 0x00003452
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x0000525A File Offset: 0x0000345A
		public bool IsEnabled { get; set; }

		// Token: 0x0600048D RID: 1165 RVA: 0x00005263 File Offset: 0x00003463
		public Announcement()
		{
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000526B File Offset: 0x0000346B
		public Announcement(int id, Guid battleId, AnnouncementType type, string text, bool isEnabled)
		{
			this.Id = id;
			this.BattleId = battleId;
			this.Type = type;
			this.Text = text;
			this.IsEnabled = isEnabled;
		}
	}
}
