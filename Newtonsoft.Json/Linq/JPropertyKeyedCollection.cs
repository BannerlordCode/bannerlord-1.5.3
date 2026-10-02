using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BE RID: 190
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	internal class JPropertyKeyedCollection : Collection<JToken>
	{
		// Token: 0x06000A8B RID: 2699 RVA: 0x0002A287 File Offset: 0x00028487
		public JPropertyKeyedCollection()
			: base(new List<JToken>())
		{
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0002A294 File Offset: 0x00028494
		private void AddKey(string key, JToken item)
		{
			this.EnsureDictionary();
			this._dictionary[key] = item;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0002A2AC File Offset: 0x000284AC
		protected void ChangeItemKey(JToken item, string newKey)
		{
			if (!this.ContainsItem(item))
			{
				throw new ArgumentException("The specified item does not exist in this KeyedCollection.");
			}
			string keyForItem = this.GetKeyForItem(item);
			if (!JPropertyKeyedCollection.Comparer.Equals(keyForItem, newKey))
			{
				if (newKey != null)
				{
					this.AddKey(newKey, item);
				}
				if (keyForItem != null)
				{
					this.RemoveKey(keyForItem);
				}
			}
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0002A2F8 File Offset: 0x000284F8
		protected override void ClearItems()
		{
			base.ClearItems();
			Dictionary<string, JToken> dictionary = this._dictionary;
			if (dictionary == null)
			{
				return;
			}
			dictionary.Clear();
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0002A310 File Offset: 0x00028510
		public bool Contains(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this._dictionary != null && this._dictionary.ContainsKey(key);
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0002A338 File Offset: 0x00028538
		private bool ContainsItem(JToken item)
		{
			if (this._dictionary == null)
			{
				return false;
			}
			string keyForItem = this.GetKeyForItem(item);
			JToken jtoken;
			return this._dictionary.TryGetValue(keyForItem, out jtoken);
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0002A365 File Offset: 0x00028565
		private void EnsureDictionary()
		{
			if (this._dictionary == null)
			{
				this._dictionary = new Dictionary<string, JToken>(JPropertyKeyedCollection.Comparer);
			}
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0002A37F File Offset: 0x0002857F
		private string GetKeyForItem(JToken item)
		{
			return ((JProperty)item).Name;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0002A38C File Offset: 0x0002858C
		protected override void InsertItem(int index, JToken item)
		{
			this.AddKey(this.GetKeyForItem(item), item);
			base.InsertItem(index, item);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0002A3A4 File Offset: 0x000285A4
		public bool Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			JToken jtoken;
			return this._dictionary != null && this._dictionary.TryGetValue(key, out jtoken) && base.Remove(jtoken);
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0002A3E4 File Offset: 0x000285E4
		protected override void RemoveItem(int index)
		{
			string keyForItem = this.GetKeyForItem(base.Items[index]);
			this.RemoveKey(keyForItem);
			base.RemoveItem(index);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0002A412 File Offset: 0x00028612
		private void RemoveKey(string key)
		{
			Dictionary<string, JToken> dictionary = this._dictionary;
			if (dictionary == null)
			{
				return;
			}
			dictionary.Remove(key);
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0002A428 File Offset: 0x00028628
		protected override void SetItem(int index, JToken item)
		{
			string keyForItem = this.GetKeyForItem(item);
			string keyForItem2 = this.GetKeyForItem(base.Items[index]);
			if (JPropertyKeyedCollection.Comparer.Equals(keyForItem2, keyForItem))
			{
				if (this._dictionary != null)
				{
					this._dictionary[keyForItem] = item;
				}
			}
			else
			{
				this.AddKey(keyForItem, item);
				if (keyForItem2 != null)
				{
					this.RemoveKey(keyForItem2);
				}
			}
			base.SetItem(index, item);
		}

		// Token: 0x170001E8 RID: 488
		public JToken this[string key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				if (this._dictionary != null)
				{
					return this._dictionary[key];
				}
				throw new KeyNotFoundException();
			}
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0002A4B9 File Offset: 0x000286B9
		public bool TryGetValue(string key, [Nullable(2)] [NotNullWhen(true)] out JToken value)
		{
			if (this._dictionary == null)
			{
				value = null;
				return false;
			}
			return this._dictionary.TryGetValue(key, out value);
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x0002A4D5 File Offset: 0x000286D5
		public ICollection<string> Keys
		{
			get
			{
				this.EnsureDictionary();
				return this._dictionary.Keys;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000A9B RID: 2715 RVA: 0x0002A4E8 File Offset: 0x000286E8
		public ICollection<JToken> Values
		{
			get
			{
				this.EnsureDictionary();
				return this._dictionary.Values;
			}
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0002A4FB File Offset: 0x000286FB
		public int IndexOfReference(JToken t)
		{
			return ((List<JToken>)base.Items).IndexOfReference(t);
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0002A510 File Offset: 0x00028710
		public bool Compare(JPropertyKeyedCollection other)
		{
			if (this == other)
			{
				return true;
			}
			Dictionary<string, JToken> dictionary = this._dictionary;
			Dictionary<string, JToken> dictionary2 = other._dictionary;
			if (dictionary == null && dictionary2 == null)
			{
				return true;
			}
			if (dictionary == null)
			{
				return dictionary2.Count == 0;
			}
			if (dictionary2 == null)
			{
				return dictionary.Count == 0;
			}
			if (dictionary.Count != dictionary2.Count)
			{
				return false;
			}
			foreach (KeyValuePair<string, JToken> keyValuePair in dictionary)
			{
				JToken jtoken;
				if (!dictionary2.TryGetValue(keyValuePair.Key, out jtoken))
				{
					return false;
				}
				JProperty jproperty = (JProperty)keyValuePair.Value;
				JProperty jproperty2 = (JProperty)jtoken;
				if (jproperty.Value == null)
				{
					return jproperty2.Value == null;
				}
				if (!jproperty.Value.DeepEquals(jproperty2.Value))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000384 RID: 900
		private static readonly IEqualityComparer<string> Comparer = StringComparer.Ordinal;

		// Token: 0x04000385 RID: 901
		[Nullable(new byte[] { 2, 1, 1 })]
		private Dictionary<string, JToken> _dictionary;
	}
}
