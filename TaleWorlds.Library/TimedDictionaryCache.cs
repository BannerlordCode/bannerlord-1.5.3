using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TaleWorlds.Library
{
	// Token: 0x02000095 RID: 149
	public class TimedDictionaryCache<TKey, TValue>
	{
		// Token: 0x06000556 RID: 1366 RVA: 0x00013159 File Offset: 0x00011359
		public TimedDictionaryCache(long validMilliseconds)
		{
			this._dictionary = new Dictionary<TKey, ValueTuple<long, TValue>>();
			this._stopwatch = new Stopwatch();
			this._stopwatch.Start();
			this._validMilliseconds = validMilliseconds;
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00013189 File Offset: 0x00011389
		public TimedDictionaryCache(TimeSpan validTimeSpan)
			: this((long)validTimeSpan.TotalMilliseconds)
		{
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00013199 File Offset: 0x00011399
		private bool IsItemExpired(TKey key)
		{
			return this._stopwatch.ElapsedMilliseconds - this._dictionary[key].Item1 >= this._validMilliseconds;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x000131C3 File Offset: 0x000113C3
		private bool RemoveIfExpired(TKey key)
		{
			if (this.IsItemExpired(key))
			{
				this._dictionary.Remove(key);
				return true;
			}
			return false;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x000131E0 File Offset: 0x000113E0
		public void PruneExpiredItems()
		{
			List<TKey> list = new List<TKey>();
			foreach (KeyValuePair<TKey, ValueTuple<long, TValue>> keyValuePair in this._dictionary)
			{
				if (this.IsItemExpired(keyValuePair.Key))
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (TKey tkey in list)
			{
				this._dictionary.Remove(tkey);
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00013294 File Offset: 0x00011494
		public void Clear()
		{
			this._dictionary.Clear();
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x000132A1 File Offset: 0x000114A1
		public bool ContainsKey(TKey key)
		{
			return this._dictionary.ContainsKey(key) && !this.RemoveIfExpired(key);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000132BD File Offset: 0x000114BD
		public bool Remove(TKey key)
		{
			this.RemoveIfExpired(key);
			return this._dictionary.Remove(key);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x000132D3 File Offset: 0x000114D3
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this.ContainsKey(key))
			{
				value = this._dictionary[key].Item2;
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x17000091 RID: 145
		public TValue this[TKey key]
		{
			get
			{
				this.RemoveIfExpired(key);
				return this._dictionary[key].Item2;
			}
			set
			{
				this._dictionary[key] = new ValueTuple<long, TValue>(this._stopwatch.ElapsedMilliseconds, value);
			}
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001333C File Offset: 0x0001153C
		public MBReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary()
		{
			this.PruneExpiredItems();
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
			foreach (KeyValuePair<TKey, ValueTuple<long, TValue>> keyValuePair in this._dictionary)
			{
				dictionary[keyValuePair.Key] = keyValuePair.Value.Item2;
			}
			return dictionary.GetReadOnlyDictionary<TKey, TValue>();
		}

		// Token: 0x040001AC RID: 428
		[TupleElementNames(new string[] { "Timestamp", "Value" })]
		private readonly Dictionary<TKey, ValueTuple<long, TValue>> _dictionary;

		// Token: 0x040001AD RID: 429
		private readonly Stopwatch _stopwatch;

		// Token: 0x040001AE RID: 430
		private readonly long _validMilliseconds;
	}
}
