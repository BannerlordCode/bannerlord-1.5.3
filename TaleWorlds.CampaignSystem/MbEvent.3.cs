using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000050 RID: 80
	public class MbEvent<T1, T2> : IMbEvent<T1, T2>, IMbEventBase
	{
		// Token: 0x060008BB RID: 2235 RVA: 0x00027874 File Offset: 0x00025A74
		public void AddNonSerializedListener(object owner, Action<T1, T2> action)
		{
			MbEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec = new MbEvent<T1, T2>.EventHandlerRec<T1, T2>(owner, action);
			MbEvent<T1, T2>.EventHandlerRec<T1, T2> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0002789E File Offset: 0x00025A9E
		public void Invoke(T1 t1, T2 t2)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2);
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x000278AE File Offset: 0x00025AAE
		private void InvokeList(MbEvent<T1, T2>.EventHandlerRec<T1, T2> list, T1 t1, T2 t2)
		{
			while (list != null)
			{
				list.Action(t1, t2);
				list = list.Next;
			}
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x000278CA File Offset: 0x00025ACA
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x000278DC File Offset: 0x00025ADC
		private void ClearListenerOfList(ref MbEvent<T1, T2>.EventHandlerRec<T1, T2> list, object o)
		{
			MbEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec2 = list;
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

		// Token: 0x040002C4 RID: 708
		private MbEvent<T1, T2>.EventHandlerRec<T1, T2> _nonSerializedListenerList;

		// Token: 0x02000538 RID: 1336
		internal class EventHandlerRec<TS, TQ>
		{
			// Token: 0x17000F3C RID: 3900
			// (get) Token: 0x06004F1E RID: 20254 RVA: 0x0018C9E6 File Offset: 0x0018ABE6
			// (set) Token: 0x06004F1F RID: 20255 RVA: 0x0018C9EE File Offset: 0x0018ABEE
			internal Action<TS, TQ> Action { get; private set; }

			// Token: 0x17000F3D RID: 3901
			// (get) Token: 0x06004F20 RID: 20256 RVA: 0x0018C9F7 File Offset: 0x0018ABF7
			// (set) Token: 0x06004F21 RID: 20257 RVA: 0x0018C9FF File Offset: 0x0018ABFF
			internal object Owner { get; private set; }

			// Token: 0x06004F22 RID: 20258 RVA: 0x0018CA08 File Offset: 0x0018AC08
			public EventHandlerRec(object owner, Action<TS, TQ> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040016ED RID: 5869
			public MbEvent<T1, T2>.EventHandlerRec<TS, TQ> Next;
		}
	}
}
