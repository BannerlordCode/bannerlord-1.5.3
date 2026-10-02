using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200003D RID: 61
	public class MbEvent : IMbEvent
	{
		// Token: 0x060003F6 RID: 1014 RVA: 0x0001F630 File Offset: 0x0001D830
		public void AddNonSerializedListener(object owner, Action action)
		{
			MbEvent.EventHandlerRec eventHandlerRec = new MbEvent.EventHandlerRec(owner, action);
			MbEvent.EventHandlerRec nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0001F65A File Offset: 0x0001D85A
		public void Invoke()
		{
			this.InvokeList(this._nonSerializedListenerList);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001F668 File Offset: 0x0001D868
		private void InvokeList(MbEvent.EventHandlerRec list)
		{
			while (list != null)
			{
				list.Action();
				list = list.Next;
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0001F682 File Offset: 0x0001D882
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0001F694 File Offset: 0x0001D894
		private void ClearListenerOfList(ref MbEvent.EventHandlerRec list, object o)
		{
			MbEvent.EventHandlerRec eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent.EventHandlerRec eventHandlerRec2 = list;
			if (eventHandlerRec2 == eventHandlerRec)
			{
				list = eventHandlerRec2.Next;
				return;
			}
			while (eventHandlerRec2 != null)
			{
				if (eventHandlerRec2.Next == eventHandlerRec)
				{
					eventHandlerRec2.Next = eventHandlerRec.Next;
				}
				else
				{
					eventHandlerRec2 = eventHandlerRec2.Next;
				}
			}
		}

		// Token: 0x04000188 RID: 392
		private MbEvent.EventHandlerRec _nonSerializedListenerList;

		// Token: 0x02000530 RID: 1328
		internal class EventHandlerRec
		{
			// Token: 0x17000F30 RID: 3888
			// (get) Token: 0x06004EE1 RID: 20193 RVA: 0x0018C57E File Offset: 0x0018A77E
			// (set) Token: 0x06004EE2 RID: 20194 RVA: 0x0018C586 File Offset: 0x0018A786
			internal Action Action { get; private set; }

			// Token: 0x17000F31 RID: 3889
			// (get) Token: 0x06004EE3 RID: 20195 RVA: 0x0018C58F File Offset: 0x0018A78F
			// (set) Token: 0x06004EE4 RID: 20196 RVA: 0x0018C597 File Offset: 0x0018A797
			internal object Owner { get; private set; }

			// Token: 0x06004EE5 RID: 20197 RVA: 0x0018C5A0 File Offset: 0x0018A7A0
			public EventHandlerRec(object owner, Action action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040016C3 RID: 5827
			public MbEvent.EventHandlerRec Next;
		}
	}
}
