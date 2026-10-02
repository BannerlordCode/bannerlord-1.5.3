using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004C RID: 76
	public class ReferenceMBEvent<T1, T2> : ReferenceIMBEvent<T1, T2>, IMbEventBase
	{
		// Token: 0x060008AD RID: 2221 RVA: 0x000276DC File Offset: 0x000258DC
		public void AddNonSerializedListener(object owner, ReferenceAction<T1, T2> action)
		{
			ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec = new ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2>(owner, action);
			ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00027706 File Offset: 0x00025906
		public void Invoke(T1 t1, ref T2 t2)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, ref t2);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00027716 File Offset: 0x00025916
		private void InvokeList(ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2> list, T1 t1, ref T2 t2)
		{
			while (list != null)
			{
				list.Action(t1, ref t2);
				list = list.Next;
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00027732 File Offset: 0x00025932
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00027744 File Offset: 0x00025944
		private void ClearListenerOfList(ref ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2> list, object o)
		{
			ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec2 = list;
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

		// Token: 0x040002C2 RID: 706
		private ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2> _nonSerializedListenerList;

		// Token: 0x02000536 RID: 1334
		internal class EventHandlerRec<TS, TQ>
		{
			// Token: 0x17000F38 RID: 3896
			// (get) Token: 0x06004F14 RID: 20244 RVA: 0x0018C976 File Offset: 0x0018AB76
			// (set) Token: 0x06004F15 RID: 20245 RVA: 0x0018C97E File Offset: 0x0018AB7E
			internal ReferenceAction<TS, TQ> Action { get; private set; }

			// Token: 0x17000F39 RID: 3897
			// (get) Token: 0x06004F16 RID: 20246 RVA: 0x0018C987 File Offset: 0x0018AB87
			// (set) Token: 0x06004F17 RID: 20247 RVA: 0x0018C98F File Offset: 0x0018AB8F
			internal object Owner { get; private set; }

			// Token: 0x06004F18 RID: 20248 RVA: 0x0018C998 File Offset: 0x0018AB98
			public EventHandlerRec(object owner, ReferenceAction<TS, TQ> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040016E7 RID: 5863
			public ReferenceMBEvent<T1, T2>.EventHandlerRec<TS, TQ> Next;
		}
	}
}
