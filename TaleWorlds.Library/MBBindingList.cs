using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TaleWorlds.Library
{
	// Token: 0x02000068 RID: 104
	public class MBBindingList<T> : Collection<T>, IMBBindingList, IList, ICollection, IEnumerable
	{
		// Token: 0x06000349 RID: 841 RVA: 0x0000C170 File Offset: 0x0000A370
		public MBBindingList()
			: base(new List<T>(64))
		{
			this._list = (List<T>)base.Items;
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x0600034A RID: 842 RVA: 0x0000C190 File Offset: 0x0000A390
		// (remove) Token: 0x0600034B RID: 843 RVA: 0x0000C1B1 File Offset: 0x0000A3B1
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				if (this._eventHandlers == null)
				{
					this._eventHandlers = new List<ListChangedEventHandler>();
				}
				this._eventHandlers.Add(value);
			}
			remove
			{
				if (this._eventHandlers != null)
				{
					this._eventHandlers.Remove(value);
				}
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000C1C8 File Offset: 0x0000A3C8
		protected override void ClearItems()
		{
			base.ClearItems();
			this.FireListChanged(ListChangedType.Reset, -1);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000C1D8 File Offset: 0x0000A3D8
		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
			this.FireListChanged(ListChangedType.ItemAdded, index);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000C1EA File Offset: 0x0000A3EA
		protected override void RemoveItem(int index)
		{
			this.FireListChanged(ListChangedType.ItemBeforeDeleted, index);
			base.RemoveItem(index);
			this.FireListChanged(ListChangedType.ItemDeleted, index);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000C203 File Offset: 0x0000A403
		protected override void SetItem(int index, T item)
		{
			base.SetItem(index, item);
			this.FireListChanged(ListChangedType.ItemChanged, index);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000C215 File Offset: 0x0000A415
		private void FireListChanged(ListChangedType type, int index)
		{
			this.OnListChanged(new ListChangedEventArgs(type, index));
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000C224 File Offset: 0x0000A424
		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			if (this._eventHandlers != null)
			{
				foreach (ListChangedEventHandler listChangedEventHandler in this._eventHandlers)
				{
					listChangedEventHandler(this, e);
				}
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000C280 File Offset: 0x0000A480
		public void Sort()
		{
			this._list.Sort();
			this.FireListChanged(ListChangedType.Sorted, -1);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000C295 File Offset: 0x0000A495
		public void Sort(IComparer<T> comparer)
		{
			if (!this.IsOrdered(comparer))
			{
				this._list.Sort(comparer);
				this.FireListChanged(ListChangedType.Sorted, -1);
			}
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000C2B4 File Offset: 0x0000A4B4
		public bool IsOrdered(IComparer<T> comparer)
		{
			for (int i = 1; i < this._list.Count; i++)
			{
				if (comparer.Compare(this._list[i - 1], this._list[i]) == 1)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000C300 File Offset: 0x0000A500
		public void ApplyActionOnAllItems(Action<T> action)
		{
			for (int i = 0; i < this._list.Count; i++)
			{
				T t = this._list[i];
				action(t);
			}
		}

		// Token: 0x04000132 RID: 306
		private readonly List<T> _list;

		// Token: 0x04000133 RID: 307
		private List<ListChangedEventHandler> _eventHandlers;
	}
}
