using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004A RID: 74
	public class ReferenceMBEvent<T1> : ReferenceIMBEvent<T1>, IMbEventBase
	{
		// Token: 0x060008A6 RID: 2214 RVA: 0x00027614 File Offset: 0x00025814
		public void AddNonSerializedListener(object owner, ReferenceAction<T1> action)
		{
			ReferenceMBEvent<T1>.EventHandlerRec<T1> eventHandlerRec = new ReferenceMBEvent<T1>.EventHandlerRec<T1>(owner, action);
			ReferenceMBEvent<T1>.EventHandlerRec<T1> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0002763E File Offset: 0x0002583E
		public void Invoke(ref T1 t1)
		{
			this.InvokeList(this._nonSerializedListenerList, ref t1);
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0002764D File Offset: 0x0002584D
		private void InvokeList(ReferenceMBEvent<T1>.EventHandlerRec<T1> list, ref T1 t1)
		{
			while (list != null)
			{
				list.Action(ref t1);
				list = list.Next;
			}
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00027668 File Offset: 0x00025868
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00027678 File Offset: 0x00025878
		private void ClearListenerOfList(ref ReferenceMBEvent<T1>.EventHandlerRec<T1> list, object o)
		{
			ReferenceMBEvent<T1>.EventHandlerRec<T1> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			ReferenceMBEvent<T1>.EventHandlerRec<T1> eventHandlerRec2 = list;
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

		// Token: 0x040002C1 RID: 705
		private ReferenceMBEvent<T1>.EventHandlerRec<T1> _nonSerializedListenerList;

		// Token: 0x02000535 RID: 1333
		internal class EventHandlerRec<TS>
		{
			// Token: 0x17000F36 RID: 3894
			// (get) Token: 0x06004F0F RID: 20239 RVA: 0x0018C93E File Offset: 0x0018AB3E
			// (set) Token: 0x06004F10 RID: 20240 RVA: 0x0018C946 File Offset: 0x0018AB46
			internal ReferenceAction<TS> Action { get; private set; }

			// Token: 0x17000F37 RID: 3895
			// (get) Token: 0x06004F11 RID: 20241 RVA: 0x0018C94F File Offset: 0x0018AB4F
			// (set) Token: 0x06004F12 RID: 20242 RVA: 0x0018C957 File Offset: 0x0018AB57
			internal object Owner { get; private set; }

			// Token: 0x06004F13 RID: 20243 RVA: 0x0018C960 File Offset: 0x0018AB60
			public EventHandlerRec(object owner, ReferenceAction<TS> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040016E4 RID: 5860
			public ReferenceMBEvent<T1>.EventHandlerRec<TS> Next;
		}
	}
}
