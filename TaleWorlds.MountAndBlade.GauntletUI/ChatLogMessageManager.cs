using System;
using System.Collections.Concurrent;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.Multiplayer;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000005 RID: 5
	public class ChatLogMessageManager : MessageManagerBase
	{
		// Token: 0x0600000C RID: 12 RVA: 0x0000229C File Offset: 0x0000049C
		public ChatLogMessageManager(MPChatVM chatDataSource)
		{
			this._chatDataSource = chatDataSource;
			this._queue = new ConcurrentQueue<ChatLogMessageManager.ChatLineData>();
			InformationManager.DisplayMessageInternal += this.OnDisplayMessageReceived;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022C7 File Offset: 0x000004C7
		private void OnDisplayMessageReceived(InformationMessage message)
		{
			if (!string.IsNullOrEmpty(message.SoundEventPath))
			{
				SoundEvent.PlaySound2D(message.SoundEventPath);
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022E4 File Offset: 0x000004E4
		public void Update()
		{
			ChatLogMessageManager.ChatLineData chatLineData;
			while (this._queue.TryDequeue(out chatLineData))
			{
				InformationManager.DisplayMessage(new InformationMessage(chatLineData.Text, Color.FromUint(chatLineData.Color), "Default"));
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002322 File Offset: 0x00000522
		protected override void PostWarningLine(string text)
		{
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002324 File Offset: 0x00000524
		protected override void PostSuccessLine(string text)
		{
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002326 File Offset: 0x00000526
		protected override void PostMessageLineFormatted(string text, uint color)
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002328 File Offset: 0x00000528
		protected override void PostMessageLine(string text, uint color)
		{
		}

		// Token: 0x04000005 RID: 5
		private const uint WarningColor = 4292235858U;

		// Token: 0x04000006 RID: 6
		private const uint SuccessColor = 4285126986U;

		// Token: 0x04000007 RID: 7
		private MPChatVM _chatDataSource;

		// Token: 0x04000008 RID: 8
		private ConcurrentQueue<ChatLogMessageManager.ChatLineData> _queue;

		// Token: 0x02000043 RID: 67
		public struct ChatLineData
		{
			// Token: 0x0600032E RID: 814 RVA: 0x00013A40 File Offset: 0x00011C40
			public ChatLineData(string text, uint color)
			{
				this.Text = text;
				this.Color = color;
			}

			// Token: 0x040001AC RID: 428
			public string Text;

			// Token: 0x040001AD RID: 429
			public uint Color;
		}
	}
}
