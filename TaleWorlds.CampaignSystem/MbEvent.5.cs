using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000054 RID: 84
	public class MbEvent<T1, T2, T3, T4> : IMbEvent<T1, T2, T3, T4>, IMbEventBase
	{
		// Token: 0x060008C9 RID: 2249 RVA: 0x00027A0C File Offset: 0x00025C0C
		public void AddNonSerializedListener(object owner, Action<T1, T2, T3, T4> action)
		{
			MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> eventHandlerRec = new MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4>(owner, action);
			MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00027A36 File Offset: 0x00025C36
		public void Invoke(T1 t1, T2 t2, T3 t3, T4 t4)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, t3, t4);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00027A49 File Offset: 0x00025C49
		private void InvokeList(MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> list, T1 t1, T2 t2, T3 t3, T4 t4)
		{
			while (list != null)
			{
				list.Action(t1, t2, t3, t4);
				list = list.Next;
			}
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00027A69 File Offset: 0x00025C69
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00027A78 File Offset: 0x00025C78
		private void ClearListenerOfList(ref MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> list, object o)
		{
			MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> eventHandlerRec2 = list;
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

		// Token: 0x040002C6 RID: 710
		private MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> _nonSerializedListenerList;

		// Token: 0x0200053A RID: 1338
		internal class EventHandlerRec<TA, TB, TC, TD>
		{
			// Token: 0x17000F40 RID: 3904
			// (get) Token: 0x06004F28 RID: 20264 RVA: 0x0018CA56 File Offset: 0x0018AC56
			// (set) Token: 0x06004F29 RID: 20265 RVA: 0x0018CA5E File Offset: 0x0018AC5E
			internal Action<TA, TB, TC, TD> Action { get; private set; }

			// Token: 0x17000F41 RID: 3905
			// (get) Token: 0x06004F2A RID: 20266 RVA: 0x0018CA67 File Offset: 0x0018AC67
			// (set) Token: 0x06004F2B RID: 20267 RVA: 0x0018CA6F File Offset: 0x0018AC6F
			internal object Owner { get; private set; }

			// Token: 0x06004F2C RID: 20268 RVA: 0x0018CA78 File Offset: 0x0018AC78
			public EventHandlerRec(object owner, Action<TA, TB, TC, TD> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040016F3 RID: 5875
			public MbEvent<T1, T2, T3, T4>.EventHandlerRec<TA, TB, TC, TD> Next;
		}
	}
}
