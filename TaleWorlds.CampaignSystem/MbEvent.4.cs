using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000052 RID: 82
	public class MbEvent<T1, T2, T3> : IMbEvent<T1, T2, T3>, IMbEventBase
	{
		// Token: 0x060008C2 RID: 2242 RVA: 0x00027940 File Offset: 0x00025B40
		public void AddNonSerializedListener(object owner, Action<T1, T2, T3> action)
		{
			MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec = new MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3>(owner, action);
			MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0002796A File Offset: 0x00025B6A
		public void Invoke(T1 t1, T2 t2, T3 t3)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, t3);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0002797B File Offset: 0x00025B7B
		private void InvokeList(MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> list, T1 t1, T2 t2, T3 t3)
		{
			while (list != null)
			{
				list.Action(t1, t2, t3);
				list = list.Next;
			}
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00027999 File Offset: 0x00025B99
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x000279A8 File Offset: 0x00025BA8
		private void ClearListenerOfList(ref MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> list, object o)
		{
			MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec2 = list;
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

		// Token: 0x040002C5 RID: 709
		private MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> _nonSerializedListenerList;

		// Token: 0x02000539 RID: 1337
		internal class EventHandlerRec<TS, TQ, TR>
		{
			// Token: 0x17000F3E RID: 3902
			// (get) Token: 0x06004F23 RID: 20259 RVA: 0x0018CA1E File Offset: 0x0018AC1E
			// (set) Token: 0x06004F24 RID: 20260 RVA: 0x0018CA26 File Offset: 0x0018AC26
			internal Action<TS, TQ, TR> Action { get; private set; }

			// Token: 0x17000F3F RID: 3903
			// (get) Token: 0x06004F25 RID: 20261 RVA: 0x0018CA2F File Offset: 0x0018AC2F
			// (set) Token: 0x06004F26 RID: 20262 RVA: 0x0018CA37 File Offset: 0x0018AC37
			internal object Owner { get; private set; }

			// Token: 0x06004F27 RID: 20263 RVA: 0x0018CA40 File Offset: 0x0018AC40
			public EventHandlerRec(object owner, Action<TS, TQ, TR> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040016F0 RID: 5872
			public MbEvent<T1, T2, T3>.EventHandlerRec<TS, TQ, TR> Next;
		}
	}
}
