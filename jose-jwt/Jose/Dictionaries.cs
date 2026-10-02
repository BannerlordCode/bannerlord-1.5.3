using System;
using System.Collections.Generic;
using System.Linq;

namespace Jose
{
	// Token: 0x0200002E RID: 46
	public class Dictionaries
	{
		// Token: 0x060000CA RID: 202 RVA: 0x00005B64 File Offset: 0x00003D64
		public static void Append<K, V>(IDictionary<K, V> src, IDictionary<K, V> other)
		{
			if (src != null && other != null)
			{
				Func<KeyValuePair<K, V>, bool> <>9__0;
				Func<KeyValuePair<K, V>, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (KeyValuePair<K, V> pair) => !src.ContainsKey(pair.Key));
				}
				foreach (KeyValuePair<K, V> keyValuePair in other.Where<KeyValuePair<K, V>>(func))
				{
					src.Add(keyValuePair);
				}
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005BF0 File Offset: 0x00003DF0
		public static V Get<K, V>(IDictionary<K, V> src, K key)
		{
			V v;
			src.TryGetValue(key, out v);
			return v;
		}
	}
}
