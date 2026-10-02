using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.EventSystem
{
	// Token: 0x020000B8 RID: 184
	public class EventManager
	{
		// Token: 0x060006E5 RID: 1765 RVA: 0x00017675 File Offset: 0x00015875
		public EventManager()
		{
			this._eventsByType = new DictionaryByType();
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00017688 File Offset: 0x00015888
		public void RegisterEvent<T>(Action<T> eventObjType)
		{
			if (typeof(T).IsSubclassOf(typeof(EventBase)))
			{
				this._eventsByType.Add<T>(eventObjType);
				return;
			}
			Debug.FailedAssert("Events have to derived from EventSystemBase", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\EventSystem\\EventManager.cs", "RegisterEvent", 31);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x000176C8 File Offset: 0x000158C8
		public void UnregisterEvent<T>(Action<T> eventObjType)
		{
			if (typeof(T).IsSubclassOf(typeof(EventBase)))
			{
				this._eventsByType.Remove<T>(eventObjType);
				return;
			}
			Debug.FailedAssert("Events have to derived from EventSystemBase", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\EventSystem\\EventManager.cs", "UnregisterEvent", 48);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00017708 File Offset: 0x00015908
		public void TriggerEvent<T>(T eventObj)
		{
			this._eventsByType.InvokeActions<T>(eventObj);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00017716 File Offset: 0x00015916
		public void Clear()
		{
			this._eventsByType.Clear();
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00017723 File Offset: 0x00015923
		public IDictionary<Type, object> GetCloneOfEventDictionary()
		{
			return this._eventsByType.GetClone();
		}

		// Token: 0x04000220 RID: 544
		private readonly DictionaryByType _eventsByType;
	}
}
