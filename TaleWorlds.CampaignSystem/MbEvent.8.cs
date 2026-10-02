using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200005A RID: 90
	public class MbEvent<T1, T2, T3, T4, T5, T6, T7> : IMbEvent<T1, T2, T3, T4, T5, T6, T7>, IMbEventBase
	{
		// Token: 0x060008DE RID: 2270 RVA: 0x00027C88 File Offset: 0x00025E88
		public void AddNonSerializedListener(object owner, Action<T1, T2, T3, T4, T5, T6, T7> action)
		{
			MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7> eventHandlerRec = new MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7>(owner, action);
			MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00027CB4 File Offset: 0x00025EB4
		public void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, t3, t4, t5, t6, t7);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00027CD8 File Offset: 0x00025ED8
		private void InvokeList(MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7> list, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
		{
			while (list != null)
			{
				list.Action(t1, t2, t3, t4, t5, t6, t7);
				list = list.Next;
			}
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00027CFE File Offset: 0x00025EFE
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00027D10 File Offset: 0x00025F10
		private void ClearListenerOfList(ref MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7> list, object o)
		{
			MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7> eventHandlerRec2 = list;
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

		// Token: 0x040002C9 RID: 713
		private MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7> _nonSerializedListenerList;

		// Token: 0x0200053D RID: 1341
		internal class EventHandlerRec<TA, TB, TC, TD, TE, TF, TG>
		{
			// Token: 0x17000F46 RID: 3910
			// (get) Token: 0x06004F37 RID: 20279 RVA: 0x0018CAFE File Offset: 0x0018ACFE
			// (set) Token: 0x06004F38 RID: 20280 RVA: 0x0018CB06 File Offset: 0x0018AD06
			internal Action<TA, TB, TC, TD, TE, TF, TG> Action { get; private set; }

			// Token: 0x17000F47 RID: 3911
			// (get) Token: 0x06004F39 RID: 20281 RVA: 0x0018CB0F File Offset: 0x0018AD0F
			// (set) Token: 0x06004F3A RID: 20282 RVA: 0x0018CB17 File Offset: 0x0018AD17
			internal object Owner { get; private set; }

			// Token: 0x06004F3B RID: 20283 RVA: 0x0018CB20 File Offset: 0x0018AD20
			public EventHandlerRec(object owner, Action<TA, TB, TC, TD, TE, TF, TG> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040016FC RID: 5884
			public MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<TA, TB, TC, TD, TE, TF, TG> Next;
		}
	}
}
