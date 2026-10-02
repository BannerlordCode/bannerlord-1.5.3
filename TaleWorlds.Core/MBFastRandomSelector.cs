using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x020000AD RID: 173
	public class MBFastRandomSelector<T>
	{
		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x0001DEFA File Offset: 0x0001C0FA
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x0001DF02 File Offset: 0x0001C102
		public ushort RemainingCount { get; private set; }

		// Token: 0x06000922 RID: 2338 RVA: 0x0001DF0B File Offset: 0x0001C10B
		public MBFastRandomSelector(ushort capacity = 32)
		{
			this.ReallocateIndexArray(capacity);
			this._list = null;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0001DF21 File Offset: 0x0001C121
		public MBFastRandomSelector(MBReadOnlyList<T> list, ushort capacity = 32)
		{
			this.ReallocateIndexArray(capacity);
			this.Initialize(list);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0001DF38 File Offset: 0x0001C138
		public void Initialize(MBReadOnlyList<T> list)
		{
			if (list != null && list.Count <= 65535)
			{
				this._list = list;
				this.TryExpand();
			}
			else
			{
				Debug.FailedAssert("Cannot initialize random selector as passed list is null or it exceeds " + ushort.MaxValue + " elements).", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\MBFastRandomSelector.cs", "Initialize", 63);
				this._list = null;
			}
			this.Reset();
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0001DF9C File Offset: 0x0001C19C
		public void Reset()
		{
			if (this._list != null)
			{
				if (this._currentVersion < 65535)
				{
					this._currentVersion += 1;
				}
				else
				{
					for (int i = 0; i < this._indexArray.Length; i++)
					{
						this._indexArray[i] = default(MBFastRandomSelector<T>.IndexEntry);
					}
					this._currentVersion = 1;
				}
				this.RemainingCount = (ushort)this._list.Count;
				return;
			}
			this._currentVersion = 1;
			this.RemainingCount = 0;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0001E01C File Offset: 0x0001C21C
		public void Pack()
		{
			if (this._list != null)
			{
				ushort num = (ushort)MathF.Max(32, this._list.Count);
				if (this._indexArray.Length != (int)num)
				{
					this.ReallocateIndexArray(num);
					return;
				}
			}
			else if (this._indexArray.Length != 32)
			{
				this.ReallocateIndexArray(32);
			}
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0001E06C File Offset: 0x0001C26C
		public bool SelectRandom(out T selection, Predicate<T> conditions = null)
		{
			selection = default(T);
			if (this._list == null)
			{
				return false;
			}
			bool flag = false;
			while (this.RemainingCount > 0 && !flag)
			{
				ushort num = (ushort)MBRandom.RandomInt((int)this.RemainingCount);
				ushort num2 = this.RemainingCount - 1;
				MBFastRandomSelector<T>.IndexEntry indexEntry = this._indexArray[(int)num];
				T t = ((indexEntry.Version == this._currentVersion) ? this._list[(int)indexEntry.Index] : this._list[(int)num]);
				if (conditions == null || conditions(t))
				{
					flag = true;
					selection = t;
				}
				MBFastRandomSelector<T>.IndexEntry indexEntry2 = this._indexArray[(int)num2];
				this._indexArray[(int)num] = ((indexEntry2.Version == this._currentVersion) ? new MBFastRandomSelector<T>.IndexEntry(indexEntry2.Index, this._currentVersion) : new MBFastRandomSelector<T>.IndexEntry(num2, this._currentVersion));
				ushort remainingCount = this.RemainingCount;
				this.RemainingCount = remainingCount - 1;
			}
			return flag;
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0001E168 File Offset: 0x0001C368
		private void TryExpand()
		{
			if (this._indexArray.Length >= this._list.Count)
			{
				return;
			}
			ushort num = (ushort)(this._list.Count * 2);
			this.ReallocateIndexArray(num);
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0001E1A1 File Offset: 0x0001C3A1
		private void ReallocateIndexArray(ushort capacity)
		{
			capacity = (ushort)MBMath.ClampInt((int)capacity, 32, 65535);
			this._indexArray = new MBFastRandomSelector<T>.IndexEntry[(int)capacity];
			this._currentVersion = 1;
		}

		// Token: 0x04000517 RID: 1303
		public const ushort MinimumCapacity = 32;

		// Token: 0x04000518 RID: 1304
		public const ushort MaximumCapacity = 65535;

		// Token: 0x04000519 RID: 1305
		private const ushort InitialVersion = 1;

		// Token: 0x0400051A RID: 1306
		private const ushort MaximumVersion = 65535;

		// Token: 0x0400051C RID: 1308
		private MBReadOnlyList<T> _list;

		// Token: 0x0400051D RID: 1309
		private MBFastRandomSelector<T>.IndexEntry[] _indexArray;

		// Token: 0x0400051E RID: 1310
		private ushort _currentVersion;

		// Token: 0x02000120 RID: 288
		public struct IndexEntry
		{
			// Token: 0x06000C1A RID: 3098 RVA: 0x00026B13 File Offset: 0x00024D13
			public IndexEntry(ushort index, ushort version)
			{
				this.Index = index;
				this.Version = version;
			}

			// Token: 0x040007BC RID: 1980
			public ushort Index;

			// Token: 0x040007BD RID: 1981
			public ushort Version;
		}
	}
}
