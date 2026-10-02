using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.EventSystem
{
	// Token: 0x020000B9 RID: 185
	public class DictionaryByType
	{
		// Token: 0x060006EB RID: 1771 RVA: 0x00017730 File Offset: 0x00015930
		public void Add<T>(Action<T> value)
		{
			object obj;
			if (!this._eventsByType.TryGetValue(typeof(T), out obj))
			{
				obj = new List<Action<T>>();
				this._eventsByType[typeof(T)] = obj;
			}
			((List<Action<T>>)obj).Add(value);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00017780 File Offset: 0x00015980
		public void Remove<T>(Action<T> value)
		{
			object obj;
			if (this._eventsByType.TryGetValue(typeof(T), out obj))
			{
				List<Action<T>> list = (List<Action<T>>)obj;
				list.Remove(value);
				this._eventsByType[typeof(T)] = list;
				return;
			}
			Debug.FailedAssert("Event: " + typeof(T).Name + " were not registered in the first place", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\EventSystem\\EventManager.cs", "Remove", 106);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x000177FC File Offset: 0x000159FC
		public void InvokeActions<T>(T item)
		{
			object obj;
			if (this._eventsByType.TryGetValue(typeof(T), out obj))
			{
				foreach (Action<T> action in ((List<Action<T>>)obj))
				{
					action(item);
				}
			}
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00017868 File Offset: 0x00015A68
		public List<Action<T>> Get<T>()
		{
			return (List<Action<T>>)this._eventsByType[typeof(T)];
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00017884 File Offset: 0x00015A84
		public bool TryGet<T>(out List<Action<T>> value)
		{
			object obj;
			if (this._eventsByType.TryGetValue(typeof(T), out obj))
			{
				value = (List<Action<T>>)obj;
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x000178B8 File Offset: 0x00015AB8
		public IDictionary<Type, object> GetClone()
		{
			return new Dictionary<Type, object>(this._eventsByType);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x000178C5 File Offset: 0x00015AC5
		public void Clear()
		{
			this._eventsByType.Clear();
		}

		// Token: 0x04000221 RID: 545
		private readonly IDictionary<Type, object> _eventsByType = new Dictionary<Type, object>();
	}
}
