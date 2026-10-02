using System;

namespace SandBox.View.Conversation
{
	// Token: 0x0200007C RID: 124
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class ConversationViewEventHandler : Attribute
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00029031 File Offset: 0x00027231
		public string Id { get; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00029039 File Offset: 0x00027239
		public ConversationViewEventHandler.EventType Type { get; }

		// Token: 0x06000572 RID: 1394 RVA: 0x00029041 File Offset: 0x00027241
		public ConversationViewEventHandler(string id, ConversationViewEventHandler.EventType type)
		{
			this.Id = id;
			this.Type = type;
		}

		// Token: 0x020000D5 RID: 213
		public enum EventType
		{
			// Token: 0x04000405 RID: 1029
			OnCondition,
			// Token: 0x04000406 RID: 1030
			OnConsequence
		}
	}
}
