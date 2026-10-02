using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000133 RID: 307
	[Serializable]
	public class LobbyNotification
	{
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x0000C153 File Offset: 0x0000A353
		// (set) Token: 0x06000856 RID: 2134 RVA: 0x0000C15B File Offset: 0x0000A35B
		public int Id { get; set; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x0000C164 File Offset: 0x0000A364
		// (set) Token: 0x06000858 RID: 2136 RVA: 0x0000C16C File Offset: 0x0000A36C
		public NotificationType Type { get; set; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x0000C175 File Offset: 0x0000A375
		// (set) Token: 0x0600085A RID: 2138 RVA: 0x0000C17D File Offset: 0x0000A37D
		public DateTime Date { get; set; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x0000C186 File Offset: 0x0000A386
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x0000C18E File Offset: 0x0000A38E
		public string Message { get; set; }

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x0000C197 File Offset: 0x0000A397
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x0000C19F File Offset: 0x0000A39F
		public Dictionary<string, string> Parameters { get; set; }

		// Token: 0x0600085F RID: 2143 RVA: 0x0000C1A8 File Offset: 0x0000A3A8
		public LobbyNotification()
		{
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0000C1B0 File Offset: 0x0000A3B0
		public LobbyNotification(NotificationType type, DateTime date, string message)
		{
			this.Id = -1;
			this.Type = type;
			this.Date = date;
			this.Message = message;
			this.Parameters = new Dictionary<string, string>();
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0000C1E0 File Offset: 0x0000A3E0
		public LobbyNotification(int id, NotificationType type, DateTime date, string message, string serializedParameters)
		{
			this.Id = id;
			this.Type = type;
			this.Date = date;
			this.Message = message;
			try
			{
				this.Parameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedParameters);
			}
			catch (Exception)
			{
				this.Parameters = new Dictionary<string, string>();
			}
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0000C240 File Offset: 0x0000A440
		public string GetParametersAsString()
		{
			string text = "{}";
			try
			{
				text = JsonConvert.SerializeObject(this.Parameters, Formatting.None);
			}
			catch (Exception)
			{
			}
			return text;
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0000C278 File Offset: 0x0000A478
		public TextObject GetTextObjectOfMessage()
		{
			TextObject textObject;
			if (!GameTexts.TryGetText(this.Message, out textObject, null))
			{
				textObject = new TextObject("{=!}" + this.Message, null);
			}
			return textObject;
		}

		// Token: 0x0400037A RID: 890
		public const string BadgeIdParameterName = "badge_id";

		// Token: 0x0400037B RID: 891
		public const string FriendRequesterParameterName = "friend_requester";
	}
}
