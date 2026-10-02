using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core.ViewModelCollection.Selector
{
	// Token: 0x02000011 RID: 17
	public class SelectorVM<T> : ViewModel where T : SelectorItemVM
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x00003649 File Offset: 0x00001849
		public SelectorVM(int selectedIndex, Action<SelectorVM<T>> onChange)
		{
			this.ItemList = new MBBindingList<T>();
			this.HasSingleItem = true;
			this._onChange = onChange;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003671 File Offset: 0x00001871
		public SelectorVM(IEnumerable<string> list, int selectedIndex, Action<SelectorVM<T>> onChange)
		{
			this.ItemList = new MBBindingList<T>();
			this.Refresh(list, selectedIndex, onChange);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003694 File Offset: 0x00001894
		public SelectorVM(IEnumerable<TextObject> list, int selectedIndex, Action<SelectorVM<T>> onChange)
		{
			this.ItemList = new MBBindingList<T>();
			this.Refresh(list, selectedIndex, onChange);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000036B8 File Offset: 0x000018B8
		public void Refresh(IEnumerable<string> list, int selectedIndex, Action<SelectorVM<T>> onChange)
		{
			this.ItemList.Clear();
			this._selectedIndex = -1;
			foreach (string text in list)
			{
				T t = (T)((object)Activator.CreateInstance(typeof(T), new object[] { text }));
				this.ItemList.Add(t);
			}
			this.HasSingleItem = this.ItemList.Count <= 1;
			this._onChange = onChange;
			this.SelectedIndex = selectedIndex;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000375C File Offset: 0x0000195C
		public void Refresh(IEnumerable<TextObject> list, int selectedIndex, Action<SelectorVM<T>> onChange)
		{
			this.ItemList.Clear();
			this._selectedIndex = -1;
			foreach (TextObject textObject in list)
			{
				T t = (T)((object)Activator.CreateInstance(typeof(T), new object[] { textObject }));
				this.ItemList.Add(t);
			}
			this.HasSingleItem = this.ItemList.Count <= 1;
			this._onChange = onChange;
			this.SelectedIndex = selectedIndex;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003800 File Offset: 0x00001A00
		public void Refresh(IEnumerable<T> list, int selectedIndex, Action<SelectorVM<T>> onChange)
		{
			this.ItemList.Clear();
			this._selectedIndex = -1;
			foreach (T t in list)
			{
				this.ItemList.Add(t);
			}
			this.HasSingleItem = this.ItemList.Count <= 1;
			this._onChange = onChange;
			this.SelectedIndex = selectedIndex;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00003884 File Offset: 0x00001A84
		public void SetOnChangeAction(Action<SelectorVM<T>> onChange)
		{
			this._onChange = onChange;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000388D File Offset: 0x00001A8D
		public void AddItem(T item)
		{
			this.ItemList.Add(item);
			this.HasSingleItem = this.ItemList.Count <= 1;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000038B4 File Offset: 0x00001AB4
		public void ExecuteRandomize()
		{
			MBBindingList<T> itemList = this.ItemList;
			T t;
			if (itemList == null)
			{
				t = default(T);
			}
			else
			{
				t = itemList.GetRandomElementWithPredicate<T>((T i) => i.CanBeSelected);
			}
			T t2 = t;
			if (t2 != null)
			{
				this.SelectedIndex = this.ItemList.IndexOf(t2);
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003918 File Offset: 0x00001B18
		public void ExecuteSelectNextItem()
		{
			MBBindingList<T> itemList = this.ItemList;
			if (itemList != null && itemList.Count > 0)
			{
				for (int num = (this.SelectedIndex + 1) % this.ItemList.Count; num != this.SelectedIndex; num = (num + 1) % this.ItemList.Count)
				{
					if (this.ItemList[num].CanBeSelected)
					{
						this.SelectedIndex = num;
						return;
					}
				}
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000398C File Offset: 0x00001B8C
		public void ExecuteSelectPreviousItem()
		{
			MBBindingList<T> itemList = this.ItemList;
			if (itemList != null && itemList.Count > 0)
			{
				for (int num = ((this.SelectedIndex - 1 >= 0) ? (this.SelectedIndex - 1) : (this.ItemList.Count - 1)); num != this.SelectedIndex; num = ((num - 1 >= 0) ? (num - 1) : (this.ItemList.Count - 1)))
				{
					if (this.ItemList[num].CanBeSelected)
					{
						this.SelectedIndex = num;
						return;
					}
				}
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003A18 File Offset: 0x00001C18
		public T GetCurrentItem()
		{
			MBBindingList<T> itemList = this._itemList;
			if (itemList != null && itemList.Count > 0 && this.SelectedIndex >= 0 && this.SelectedIndex < this._itemList.Count)
			{
				return this._itemList[this.SelectedIndex];
			}
			return default(T);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003A73 File Offset: 0x00001C73
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._itemList.ApplyActionOnAllItems(delegate(T x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00003AA5 File Offset: 0x00001CA5
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00003AAD File Offset: 0x00001CAD
		[DataSourceProperty]
		public MBBindingList<T> ItemList
		{
			get
			{
				return this._itemList;
			}
			set
			{
				if (value != this._itemList)
				{
					this._itemList = value;
					base.OnPropertyChangedWithValue<MBBindingList<T>>(value, "ItemList");
				}
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00003ACB File Offset: 0x00001CCB
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00003AD4 File Offset: 0x00001CD4
		[DataSourceProperty]
		public int SelectedIndex
		{
			get
			{
				return this._selectedIndex;
			}
			set
			{
				if (value != this._selectedIndex)
				{
					this._selectedIndex = value;
					base.OnPropertyChangedWithValue(value, "SelectedIndex");
					if (this.SelectedItem != null)
					{
						this.SelectedItem.IsSelected = false;
					}
					this.SelectedItem = this.GetCurrentItem();
					if (this.SelectedItem != null)
					{
						this.SelectedItem.IsSelected = true;
					}
					Action<SelectorVM<T>> onChange = this._onChange;
					if (onChange == null)
					{
						return;
					}
					onChange(this);
				}
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00003B56 File Offset: 0x00001D56
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00003B5E File Offset: 0x00001D5E
		[DataSourceProperty]
		public T SelectedItem
		{
			get
			{
				return this._selectedItem;
			}
			set
			{
				if (value != this._selectedItem)
				{
					this._selectedItem = value;
					base.OnPropertyChangedWithValue<T>(value, "SelectedItem");
				}
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00003B86 File Offset: 0x00001D86
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00003B8E File Offset: 0x00001D8E
		[DataSourceProperty]
		public bool HasSingleItem
		{
			get
			{
				return this._hasSingleItem;
			}
			set
			{
				if (value != this._hasSingleItem)
				{
					this._hasSingleItem = value;
					base.OnPropertyChangedWithValue(value, "HasSingleItem");
				}
			}
		}

		// Token: 0x0400005B RID: 91
		private Action<SelectorVM<T>> _onChange;

		// Token: 0x0400005C RID: 92
		private MBBindingList<T> _itemList;

		// Token: 0x0400005D RID: 93
		private int _selectedIndex = -1;

		// Token: 0x0400005E RID: 94
		private T _selectedItem;

		// Token: 0x0400005F RID: 95
		private bool _hasSingleItem;
	}
}
