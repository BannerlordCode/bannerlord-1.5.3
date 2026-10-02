using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004E RID: 78
	public class ReferenceMBEvent<T1, T2, T3> : ReferenceIMBEvent<T1, T2, T3>, IMbEventBase
	{
		// Token: 0x060008B4 RID: 2228 RVA: 0x000277A8 File Offset: 0x000259A8
		public void AddNonSerializedListener(object owner, ReferenceAction<T1, T2, T3> action)
		{
			ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec = new ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3>(owner, action);
			ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x000277D2 File Offset: 0x000259D2
		public void Invoke(T1 t1, T2 t2, ref T3 t3)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, ref t3);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x000277E3 File Offset: 0x000259E3
		private void InvokeList(ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> list, T1 t1, T2 t2, ref T3 t3)
		{
			while (list != null)
			{
				list.Action(t1, t2, ref t3);
				list = list.Next;
			}
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00027801 File Offset: 0x00025A01
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00027810 File Offset: 0x00025A10
		private void ClearListenerOfList(ref ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> list, object o)
		{
			ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec2 = list;
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

		// Token: 0x040002C3 RID: 707
		private ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> _nonSerializedListenerList;

		// Token: 0x02000537 RID: 1335
		internal class EventHandlerRec<TS, TQ, TR>
		{
			// Token: 0x17000F3A RID: 3898
			// (get) Token: 0x06004F19 RID: 20249 RVA: 0x0018C9AE File Offset: 0x0018ABAE
			// (set) Token: 0x06004F1A RID: 20250 RVA: 0x0018C9B6 File Offset: 0x0018ABB6
			internal ReferenceAction<TS, TQ, TR> Action { get; private set; }

			// Token: 0x17000F3B RID: 3899
			// (get) Token: 0x06004F1B RID: 20251 RVA: 0x0018C9BF File Offset: 0x0018ABBF
			// (set) Token: 0x06004F1C RID: 20252 RVA: 0x0018C9C7 File Offset: 0x0018ABC7
			internal object Owner { get; private set; }

			// Token: 0x06004F1D RID: 20253 RVA: 0x0018C9D0 File Offset: 0x0018ABD0
			public EventHandlerRec(object owner, ReferenceAction<TS, TQ, TR> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040016EA RID: 5866
			public ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<TS, TQ, TR> Next;
		}
	}
}
