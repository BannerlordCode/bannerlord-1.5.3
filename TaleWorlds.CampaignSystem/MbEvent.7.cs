using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000058 RID: 88
	public class MbEvent<T1, T2, T3, T4, T5, T6> : IMbEvent<T1, T2, T3, T4, T5, T6>, IMbEventBase
	{
		// Token: 0x060008D7 RID: 2263 RVA: 0x00027BB0 File Offset: 0x00025DB0
		public void AddNonSerializedListener(object owner, Action<T1, T2, T3, T4, T5, T6> action)
		{
			MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6> eventHandlerRec = new MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6>(owner, action);
			MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00027BDA File Offset: 0x00025DDA
		public void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, t3, t4, t5, t6);
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00027BF1 File Offset: 0x00025DF1
		private void InvokeList(MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6> list, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
		{
			while (list != null)
			{
				list.Action(t1, t2, t3, t4, t5, t6);
				list = list.Next;
			}
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x00027C15 File Offset: 0x00025E15
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00027C24 File Offset: 0x00025E24
		private void ClearListenerOfList(ref MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6> list, object o)
		{
			MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6> eventHandlerRec2 = list;
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

		// Token: 0x040002C8 RID: 712
		private MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6> _nonSerializedListenerList;

		// Token: 0x0200053C RID: 1340
		internal class EventHandlerRec<TA, TB, TC, TD, TE, TF>
		{
			// Token: 0x17000F44 RID: 3908
			// (get) Token: 0x06004F32 RID: 20274 RVA: 0x0018CAC6 File Offset: 0x0018ACC6
			// (set) Token: 0x06004F33 RID: 20275 RVA: 0x0018CACE File Offset: 0x0018ACCE
			internal Action<TA, TB, TC, TD, TE, TF> Action { get; private set; }

			// Token: 0x17000F45 RID: 3909
			// (get) Token: 0x06004F34 RID: 20276 RVA: 0x0018CAD7 File Offset: 0x0018ACD7
			// (set) Token: 0x06004F35 RID: 20277 RVA: 0x0018CADF File Offset: 0x0018ACDF
			internal object Owner { get; private set; }

			// Token: 0x06004F36 RID: 20278 RVA: 0x0018CAE8 File Offset: 0x0018ACE8
			public EventHandlerRec(object owner, Action<TA, TB, TC, TD, TE, TF> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040016F9 RID: 5881
			public MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<TA, TB, TC, TD, TE, TF> Next;
		}
	}
}
