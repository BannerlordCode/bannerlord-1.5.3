using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000056 RID: 86
	public class MbEvent<T1, T2, T3, T4, T5> : IMbEvent<T1, T2, T3, T4, T5>, IMbEventBase
	{
		// Token: 0x060008D0 RID: 2256 RVA: 0x00027ADC File Offset: 0x00025CDC
		public void AddNonSerializedListener(object owner, Action<T1, T2, T3, T4, T5> action)
		{
			MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> eventHandlerRec = new MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5>(owner, action);
			MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00027B06 File Offset: 0x00025D06
		public void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, t3, t4, t5);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00027B1B File Offset: 0x00025D1B
		private void InvokeList(MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> list, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
		{
			while (list != null)
			{
				list.Action(t1, t2, t3, t4, t5);
				list = list.Next;
			}
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00027B3D File Offset: 0x00025D3D
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x00027B4C File Offset: 0x00025D4C
		private void ClearListenerOfList(ref MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> list, object o)
		{
			MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> eventHandlerRec2 = list;
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

		// Token: 0x040002C7 RID: 711
		private MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> _nonSerializedListenerList;

		// Token: 0x0200053B RID: 1339
		internal class EventHandlerRec<TA, TB, TC, TD, TE>
		{
			// Token: 0x17000F42 RID: 3906
			// (get) Token: 0x06004F2D RID: 20269 RVA: 0x0018CA8E File Offset: 0x0018AC8E
			// (set) Token: 0x06004F2E RID: 20270 RVA: 0x0018CA96 File Offset: 0x0018AC96
			internal Action<TA, TB, TC, TD, TE> Action { get; private set; }

			// Token: 0x17000F43 RID: 3907
			// (get) Token: 0x06004F2F RID: 20271 RVA: 0x0018CA9F File Offset: 0x0018AC9F
			// (set) Token: 0x06004F30 RID: 20272 RVA: 0x0018CAA7 File Offset: 0x0018ACA7
			internal object Owner { get; private set; }

			// Token: 0x06004F31 RID: 20273 RVA: 0x0018CAB0 File Offset: 0x0018ACB0
			public EventHandlerRec(object owner, Action<TA, TB, TC, TD, TE> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040016F6 RID: 5878
			public MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<TA, TB, TC, TD, TE> Next;
		}
	}
}
