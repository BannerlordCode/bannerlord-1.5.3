using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000100 RID: 256
	[Serializable]
	public class ChatRoomInformationForClient
	{
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00006F65 File Offset: 0x00005165
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x00006F6D File Offset: 0x0000516D
		[JsonProperty]
		public Guid RoomId { get; private set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00006F76 File Offset: 0x00005176
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x00006F7E File Offset: 0x0000517E
		[JsonProperty]
		public string Name { get; private set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x00006F87 File Offset: 0x00005187
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x00006F8F File Offset: 0x0000518F
		[JsonProperty]
		public string Endpoint { get; private set; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x00006F98 File Offset: 0x00005198
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x00006FA0 File Offset: 0x000051A0
		[JsonProperty]
		public string RoomColor { get; private set; }

		// Token: 0x06000581 RID: 1409 RVA: 0x00006FA9 File Offset: 0x000051A9
		public ChatRoomInformationForClient()
		{
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00006FB1 File Offset: 0x000051B1
		public ChatRoomInformationForClient(Guid roomId, string name, string endpoint, string color)
		{
			this.RoomId = roomId;
			this.Name = name;
			this.Endpoint = endpoint;
			this.RoomColor = color;
		}
	}
}
