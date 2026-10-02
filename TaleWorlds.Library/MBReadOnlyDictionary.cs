using System;
using System.Collections;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x0200006D RID: 109
	[Serializable]
	public class MBReadOnlyDictionary<TKey, TValue> : ICollection, IEnumerable, IReadOnlyDictionary<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
	{
		// Token: 0x060003D9 RID: 985 RVA: 0x0000DD3B File Offset: 0x0000BF3B
		public MBReadOnlyDictionary(Dictionary<TKey, TValue> dictionary)
		{
			this._dictionary = dictionary;
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0000DD4A File Offset: 0x0000BF4A
		public int Count
		{
			get
			{
				return this._dictionary.Count;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060003DB RID: 987 RVA: 0x0000DD57 File Offset: 0x0000BF57
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0000DD5A File Offset: 0x0000BF5A
		public object SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000DD5D File Offset: 0x0000BF5D
		public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return this._dictionary.GetEnumerator();
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000DD6A File Offset: 0x0000BF6A
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return this._dictionary.GetEnumerator();
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000DD7C File Offset: 0x0000BF7C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._dictionary.GetEnumerator();
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000DD8E File Offset: 0x0000BF8E
		public bool ContainsKey(TKey key)
		{
			return this._dictionary.ContainsKey(key);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000DD9C File Offset: 0x0000BF9C
		public bool TryGetValue(TKey key, out TValue value)
		{
			return this._dictionary.TryGetValue(key, out value);
		}

		// Token: 0x17000058 RID: 88
		public TValue this[TKey key]
		{
			get
			{
				return this._dictionary[key];
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0000DDB9 File Offset: 0x0000BFB9
		public IEnumerable<TKey> Keys
		{
			get
			{
				return this._dictionary.Keys;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x0000DDC6 File Offset: 0x0000BFC6
		public IEnumerable<TValue> Values
		{
			get
			{
				return this._dictionary.Values;
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000DDD4 File Offset: 0x0000BFD4
		public void CopyTo(Array array, int index)
		{
			KeyValuePair<TKey, TValue>[] array2 = array as KeyValuePair<TKey, TValue>[];
			if (array2 != null)
			{
				((ICollection)this._dictionary).CopyTo(array2, index);
				return;
			}
			DictionaryEntry[] array3 = array as DictionaryEntry[];
			if (array3 != null)
			{
				using (Dictionary<TKey, TValue>.Enumerator enumerator = this._dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<TKey, TValue> keyValuePair = enumerator.Current;
						array3[index++] = new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
					}
					return;
				}
			}
			object[] array4 = array as object[];
			try
			{
				foreach (KeyValuePair<TKey, TValue> keyValuePair2 in this._dictionary)
				{
					array4[index++] = new KeyValuePair<TKey, TValue>(keyValuePair2.Key, keyValuePair2.Value);
				}
			}
			catch (ArrayTypeMismatchException)
			{
				Debug.FailedAssert("Invalid array type", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\MBReadOnlyDictionary.cs", "CopyTo", 95);
			}
		}

		// Token: 0x04000142 RID: 322
		private Dictionary<TKey, TValue> _dictionary;
	}
}
