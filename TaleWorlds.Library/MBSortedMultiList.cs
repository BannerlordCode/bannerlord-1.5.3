using System;
using System.Collections;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x02000070 RID: 112
	public class MBSortedMultiList<TKey, TValue> : IReadOnlyList<TValue>, IEnumerable<TValue>, IEnumerable, IReadOnlyCollection<TValue>, IMBCollection where TKey : IComparable<TKey>
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0000DF39 File Offset: 0x0000C139
		public MBSortedMultiList<TKey, TValue>.ComparerType Comparer
		{
			get
			{
				return this._comparerType;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0000DF41 File Offset: 0x0000C141
		private bool IsAscending
		{
			get
			{
				return this._comparerType == MBSortedMultiList<TKey, TValue>.ComparerType.Ascending;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x0000DF4C File Offset: 0x0000C14C
		private bool IsDescending
		{
			get
			{
				return this._comparerType == MBSortedMultiList<TKey, TValue>.ComparerType.Descending;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x0000DF57 File Offset: 0x0000C157
		public int Count
		{
			get
			{
				return this._items.Count;
			}
		}

		// Token: 0x1700005F RID: 95
		public TValue this[int index]
		{
			get
			{
				return this._items[index].Value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0000DF88 File Offset: 0x0000C188
		public TValue FirstValue
		{
			get
			{
				return this._items[0].Value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0000DFAC File Offset: 0x0000C1AC
		public TValue LastValue
		{
			get
			{
				return this._items[this._items.Count - 1].Value;
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000DFD9 File Offset: 0x0000C1D9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000DFE1 File Offset: 0x0000C1E1
		public MBSortedMultiList(IComparer<TKey> customComparer)
		{
			this._items = new List<KeyValuePair<TKey, TValue>>();
			this.SetCustomComparer(customComparer);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000DFFB File Offset: 0x0000C1FB
		public MBSortedMultiList(bool isAscending = true)
		{
			this._items = new List<KeyValuePair<TKey, TValue>>();
			this.SetDefaultComparer(isAscending);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000E015 File Offset: 0x0000C215
		public bool Contains(TKey key)
		{
			return this.FirstIndexOf(key) >= 0;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000E024 File Offset: 0x0000C224
		public bool Contains(TKey key, TValue value)
		{
			return this.FirstIndexOf(key, value) >= 0;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000E034 File Offset: 0x0000C234
		public KeyValuePair<TKey, TValue> Get(int index)
		{
			return this._items[index];
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000E044 File Offset: 0x0000C244
		public int FirstIndexOf(TKey key)
		{
			if (this._items.Count > 0)
			{
				int num = this.LowerBound(key);
				if (num < this._items.Count)
				{
					TKey key2 = this._items[num].Key;
					if (key2.CompareTo(key) == 0)
					{
						return num;
					}
				}
			}
			return -1;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000E0A0 File Offset: 0x0000C2A0
		public int FirstIndexOf(TKey key, TValue value)
		{
			if (this._items.Count > 0)
			{
				int i = this.LowerBound(key);
				EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
				while (i < this._items.Count)
				{
					TKey key2 = this._items[i].Key;
					if (key2.CompareTo(key) != 0)
					{
						break;
					}
					if (@default.Equals(this._items[i].Value, value))
					{
						return i;
					}
					i++;
				}
			}
			return -1;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000E124 File Offset: 0x0000C324
		public int LastIndexOf(TKey key)
		{
			if (this._items.Count > 0)
			{
				int num = this.UpperBound(key) - 1;
				if (num >= 0 && num < this._items.Count)
				{
					TKey key2 = this._items[num].Key;
					if (key2.CompareTo(key) == 0)
					{
						return num;
					}
				}
			}
			return -1;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000E184 File Offset: 0x0000C384
		public int LastIndexOf(TKey key, TValue value)
		{
			if (this._items.Count > 0)
			{
				int i = this.UpperBound(key) - 1;
				EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
				while (i >= 0)
				{
					TKey key2 = this._items[i].Key;
					if (key2.CompareTo(key) != 0)
					{
						break;
					}
					if (@default.Equals(this._items[i].Value, value))
					{
						return i;
					}
					i--;
				}
			}
			return -1;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000E200 File Offset: 0x0000C400
		public bool All(Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this._items)
			{
				if (!predicate(keyValuePair))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000E25C File Offset: 0x0000C45C
		public bool Any(Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this._items)
			{
				if (predicate(keyValuePair))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000E2B8 File Offset: 0x0000C4B8
		public IEnumerator<TValue> GetValues(TKey key)
		{
			int num = this.LowerBound(key);
			List<KeyValuePair<TKey, TValue>> items = this._items;
			int num2;
			if (num < this._items.Count)
			{
				TKey key2 = this._items[num].Key;
				if (key2.CompareTo(key) == 0)
				{
					num2 = num;
					goto IL_0050;
				}
			}
			num2 = this._items.Count;
			IL_0050:
			return new MBSortedMultiList<TKey, TValue>.SMLKeyValueEnumerator(items, key, num2);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000E320 File Offset: 0x0000C520
		public bool Find(Predicate<KeyValuePair<TKey, TValue>> predicate, out KeyValuePair<TKey, TValue> found, bool searchForward = true)
		{
			if (searchForward)
			{
				for (int i = 0; i < this._items.Count; i++)
				{
					KeyValuePair<TKey, TValue> keyValuePair = this._items[i];
					if (predicate(keyValuePair))
					{
						found = keyValuePair;
						return true;
					}
				}
			}
			else
			{
				for (int j = this._items.Count - 1; j >= 0; j--)
				{
					KeyValuePair<TKey, TValue> keyValuePair2 = this._items[j];
					if (predicate(keyValuePair2))
					{
						found = keyValuePair2;
						return true;
					}
				}
			}
			found = default(KeyValuePair<TKey, TValue>);
			return false;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000E3A8 File Offset: 0x0000C5A8
		public int FindIndex(Predicate<KeyValuePair<TKey, TValue>> predicate, bool searchForward = true)
		{
			if (searchForward)
			{
				for (int i = 0; i < this._items.Count; i++)
				{
					KeyValuePair<TKey, TValue> keyValuePair = this._items[i];
					if (predicate(keyValuePair))
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = this._items.Count - 1; j >= 0; j--)
				{
					KeyValuePair<TKey, TValue> keyValuePair2 = this._items[j];
					if (predicate(keyValuePair2))
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000E41C File Offset: 0x0000C61C
		public MBList<KeyValuePair<TKey, TValue>> FindAll(Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			MBList<KeyValuePair<TKey, TValue>> mblist = new MBList<KeyValuePair<TKey, TValue>>();
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this._items)
			{
				if (predicate(keyValuePair))
				{
					mblist.Add(keyValuePair);
				}
			}
			return mblist;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000E480 File Offset: 0x0000C680
		public void Add(TKey key, TValue value)
		{
			KeyValuePair<TKey, TValue> keyValuePair = new KeyValuePair<TKey, TValue>(key, value);
			int num = this.UpperBound(key);
			this._items.Insert(num, keyValuePair);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000E4AB File Offset: 0x0000C6AB
		public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
		{
			this._items.AddRange(items);
			this._items.Sort(this._pairComparer);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000E4CC File Offset: 0x0000C6CC
		public bool Remove(TKey key, TValue value)
		{
			int num = this.LastIndexOf(key, value);
			if (num >= 0)
			{
				this._items.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000E4F8 File Offset: 0x0000C6F8
		public bool Remove(TKey key)
		{
			int num = this.LastIndexOf(key);
			if (num >= 0)
			{
				this._items.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000E520 File Offset: 0x0000C720
		public int RemoveAll(Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			int num = 0;
			for (int i = this._items.Count - 1; i >= 0; i--)
			{
				if (predicate(this._items[i]))
				{
					this._items.RemoveAt(i);
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000E56C File Offset: 0x0000C76C
		public void RemoveAt(int index)
		{
			this._items.RemoveAt(index);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000E57A File Offset: 0x0000C77A
		public void RemoveLast()
		{
			this._items.RemoveAt(this._items.Count - 1);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000E594 File Offset: 0x0000C794
		public void Clear()
		{
			this._items.Clear();
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000E5A1 File Offset: 0x0000C7A1
		public void SetCustomComparer(IComparer<TKey> customComparer)
		{
			this._keyComparer = customComparer;
			this._pairComparer = this.GetPairComparerFromKeyComparer();
			this._comparerType = MBSortedMultiList<TKey, TValue>.ComparerType.Custom;
			if (this._items.Count > 0)
			{
				this._items.Sort(this._pairComparer);
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000E5DC File Offset: 0x0000C7DC
		public void SetDefaultComparer(bool isAscending = true)
		{
			bool flag = false;
			if (isAscending && this._comparerType != MBSortedMultiList<TKey, TValue>.ComparerType.Ascending)
			{
				this._keyComparer = MBSortedMultiList<TKey, TValue>.DefaultAscendingKeyComparer;
				this._pairComparer = this.GetPairComparerFromKeyComparer();
				this._comparerType = MBSortedMultiList<TKey, TValue>.ComparerType.Ascending;
				flag = true;
			}
			else if (!isAscending && this._comparerType != MBSortedMultiList<TKey, TValue>.ComparerType.Descending)
			{
				this._keyComparer = MBSortedMultiList<TKey, TValue>.DefaultDescendingKeyComparer;
				this._pairComparer = this.GetPairComparerFromKeyComparer();
				this._comparerType = MBSortedMultiList<TKey, TValue>.ComparerType.Descending;
				flag = true;
			}
			if (flag && this._items.Count > 0)
			{
				this._items.Sort(this._pairComparer);
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000E667 File Offset: 0x0000C867
		public void Reverse()
		{
			if (this._comparerType == MBSortedMultiList<TKey, TValue>.ComparerType.Ascending)
			{
				this.SetDefaultComparer(false);
				return;
			}
			if (this._comparerType == MBSortedMultiList<TKey, TValue>.ComparerType.Descending)
			{
				this.SetDefaultComparer(true);
				return;
			}
			Debug.FailedAssert("Comparer type must not be custom", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\MBSortedMultiList.cs", "Reverse", 562);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000E6A4 File Offset: 0x0000C8A4
		public override string ToString()
		{
			return string.Format("MBSortedMultiList[{0}, {1}], Count = {2}, Comparer Type = {3}", new object[]
			{
				typeof(TKey).Name,
				typeof(TValue).Name,
				this.Count,
				this._comparerType.ToString()
			});
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000E707 File Offset: 0x0000C907
		public IEnumerator<TValue> GetEnumerator()
		{
			return new MBSortedMultiList<TKey, TValue>.SMLValueEnumerator(this._items);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000E71C File Offset: 0x0000C91C
		private int LowerBound(TKey key)
		{
			int i = 0;
			int num = this._items.Count;
			while (i < num)
			{
				int num2 = (i + num) / 2;
				if (this._keyComparer.Compare(this._items[num2].Key, key) < 0)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2;
				}
			}
			return i;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000E770 File Offset: 0x0000C970
		private int UpperBound(TKey key)
		{
			int i = 0;
			int num = this._items.Count;
			while (i < num)
			{
				int num2 = (i + num) / 2;
				if (this._keyComparer.Compare(this._items[num2].Key, key) <= 0)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2;
				}
			}
			return i;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000E7C3 File Offset: 0x0000C9C3
		private IComparer<KeyValuePair<TKey, TValue>> GetPairComparerFromKeyComparer()
		{
			return Comparer<KeyValuePair<TKey, TValue>>.Create((KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) => this._keyComparer.Compare(x.Key, y.Key));
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0000E7D6 File Offset: 0x0000C9D6
		private static IComparer<TKey> DefaultAscendingKeyComparer
		{
			get
			{
				return Comparer<TKey>.Default;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0000E7DD File Offset: 0x0000C9DD
		private static IComparer<TKey> DefaultDescendingKeyComparer
		{
			get
			{
				return Comparer<TKey>.Create((TKey x, TKey y) => y.CompareTo(x));
			}
		}

		// Token: 0x04000143 RID: 323
		private readonly List<KeyValuePair<TKey, TValue>> _items;

		// Token: 0x04000144 RID: 324
		private MBSortedMultiList<TKey, TValue>.ComparerType _comparerType;

		// Token: 0x04000145 RID: 325
		private IComparer<TKey> _keyComparer;

		// Token: 0x04000146 RID: 326
		private IComparer<KeyValuePair<TKey, TValue>> _pairComparer;

		// Token: 0x020000DA RID: 218
		public enum ComparerType
		{
			// Token: 0x040002CF RID: 719
			None,
			// Token: 0x040002D0 RID: 720
			Custom,
			// Token: 0x040002D1 RID: 721
			Ascending,
			// Token: 0x040002D2 RID: 722
			Descending
		}

		// Token: 0x020000DB RID: 219
		private struct SMLValueEnumerator : IEnumerator<TValue>, IEnumerator, IDisposable
		{
			// Token: 0x0600077E RID: 1918 RVA: 0x00018F2B File Offset: 0x0001712B
			public SMLValueEnumerator(List<KeyValuePair<TKey, TValue>> list)
			{
				this._list = list;
				this._index = -1;
				this._current = default(TValue);
			}

			// Token: 0x0600077F RID: 1919 RVA: 0x00018F48 File Offset: 0x00017148
			public bool MoveNext()
			{
				int num = this._index + 1;
				this._index = num;
				if (num < this._list.Count)
				{
					this._current = this._list[this._index].Value;
					return true;
				}
				return false;
			}

			// Token: 0x170000F9 RID: 249
			// (get) Token: 0x06000780 RID: 1920 RVA: 0x00018F95 File Offset: 0x00017195
			public TValue Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x170000FA RID: 250
			// (get) Token: 0x06000781 RID: 1921 RVA: 0x00018F9D File Offset: 0x0001719D
			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x06000782 RID: 1922 RVA: 0x00018FAA File Offset: 0x000171AA
			public void Dispose()
			{
			}

			// Token: 0x06000783 RID: 1923 RVA: 0x00018FAC File Offset: 0x000171AC
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040002D3 RID: 723
			private readonly List<KeyValuePair<TKey, TValue>> _list;

			// Token: 0x040002D4 RID: 724
			private int _index;

			// Token: 0x040002D5 RID: 725
			private TValue _current;
		}

		// Token: 0x020000DC RID: 220
		private struct SMLKeyValueEnumerator : IEnumerator<TValue>, IEnumerator, IDisposable
		{
			// Token: 0x06000784 RID: 1924 RVA: 0x00018FB3 File Offset: 0x000171B3
			public SMLKeyValueEnumerator(List<KeyValuePair<TKey, TValue>> list, TKey key, int startIndex)
			{
				this._list = list;
				this._key = key;
				this._index = startIndex - 1;
				this._current = default(TValue);
			}

			// Token: 0x06000785 RID: 1925 RVA: 0x00018FD8 File Offset: 0x000171D8
			public bool MoveNext()
			{
				this._index++;
				if (this._index < this._list.Count)
				{
					TKey key = this._list[this._index].Key;
					if (key.CompareTo(this._key) == 0)
					{
						this._current = this._list[this._index].Value;
						return true;
					}
				}
				return false;
			}

			// Token: 0x170000FB RID: 251
			// (get) Token: 0x06000786 RID: 1926 RVA: 0x00019057 File Offset: 0x00017257
			public TValue Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x170000FC RID: 252
			// (get) Token: 0x06000787 RID: 1927 RVA: 0x0001905F File Offset: 0x0001725F
			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x06000788 RID: 1928 RVA: 0x0001906C File Offset: 0x0001726C
			public void Dispose()
			{
			}

			// Token: 0x06000789 RID: 1929 RVA: 0x0001906E File Offset: 0x0001726E
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040002D6 RID: 726
			private readonly List<KeyValuePair<TKey, TValue>> _list;

			// Token: 0x040002D7 RID: 727
			private readonly TKey _key;

			// Token: 0x040002D8 RID: 728
			private int _index;

			// Token: 0x040002D9 RID: 729
			private TValue _current;
		}
	}
}
