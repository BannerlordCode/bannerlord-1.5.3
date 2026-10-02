using System;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x0200000F RID: 15
	public class MultiplayerAdminInformationVM : ViewModel
	{
		// Token: 0x060000B9 RID: 185 RVA: 0x000044C5 File Offset: 0x000026C5
		public MultiplayerAdminInformationVM()
		{
			this.MessageQueue = new MBBindingList<StringItemWithActionVM>();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000044D8 File Offset: 0x000026D8
		public void OnNewMessageReceived(string message)
		{
			StringItemWithActionVM stringItemWithActionVM = new StringItemWithActionVM(new Action<object>(this.ExecuteRemoveMessage), message, message);
			this.MessageQueue.Add(stringItemWithActionVM);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004508 File Offset: 0x00002708
		private void ExecuteRemoveMessage(object messageToRemove)
		{
			int num = this.MessageQueue.FindIndex<StringItemWithActionVM>((StringItemWithActionVM m) => m.ActionText == messageToRemove as string);
			this.MessageQueue.RemoveAt(num);
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004546 File Offset: 0x00002746
		// (set) Token: 0x060000BD RID: 189 RVA: 0x0000454E File Offset: 0x0000274E
		[DataSourceProperty]
		public MBBindingList<StringItemWithActionVM> MessageQueue
		{
			get
			{
				return this._messageQueue;
			}
			set
			{
				if (value != this._messageQueue)
				{
					this._messageQueue = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringItemWithActionVM>>(value, "MessageQueue");
				}
			}
		}

		// Token: 0x0400006F RID: 111
		private MBBindingList<StringItemWithActionVM> _messageQueue;
	}
}
