using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000048 RID: 72
	public class MbEvent<T> : IMbEvent<T>, IMbEventBase
	{
		// Token: 0x0600089F RID: 2207 RVA: 0x0002754C File Offset: 0x0002574C
		public void AddNonSerializedListener(object owner, Action<T> action)
		{
			MbEvent<T>.EventHandlerRec<T> eventHandlerRec = new MbEvent<T>.EventHandlerRec<T>(owner, action);
			MbEvent<T>.EventHandlerRec<T> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00027576 File Offset: 0x00025776
		public void Invoke(T t)
		{
			this.InvokeList(this._nonSerializedListenerList, t);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00027585 File Offset: 0x00025785
		private void InvokeList(MbEvent<T>.EventHandlerRec<T> list, T t)
		{
			while (list != null)
			{
				list.Action(t);
				list = list.Next;
			}
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x000275A0 File Offset: 0x000257A0
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x000275B0 File Offset: 0x000257B0
		private void ClearListenerOfList(ref MbEvent<T>.EventHandlerRec<T> list, object o)
		{
			MbEvent<T>.EventHandlerRec<T> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T>.EventHandlerRec<T> eventHandlerRec2 = list;
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

		// Token: 0x040002C0 RID: 704
		private MbEvent<T>.EventHandlerRec<T> _nonSerializedListenerList;

		// Token: 0x02000534 RID: 1332
		internal class EventHandlerRec<TS>
		{
			// Token: 0x17000F34 RID: 3892
			// (get) Token: 0x06004F0A RID: 20234 RVA: 0x0018C906 File Offset: 0x0018AB06
			// (set) Token: 0x06004F0B RID: 20235 RVA: 0x0018C90E File Offset: 0x0018AB0E
			internal Action<TS> Action { get; private set; }

			// Token: 0x17000F35 RID: 3893
			// (get) Token: 0x06004F0C RID: 20236 RVA: 0x0018C917 File Offset: 0x0018AB17
			// (set) Token: 0x06004F0D RID: 20237 RVA: 0x0018C91F File Offset: 0x0018AB1F
			internal object Owner { get; private set; }

			// Token: 0x06004F0E RID: 20238 RVA: 0x0018C928 File Offset: 0x0018AB28
			public EventHandlerRec(object owner, Action<TS> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040016E1 RID: 5857
			public MbEvent<T>.EventHandlerRec<TS> Next;
		}
	}
}
