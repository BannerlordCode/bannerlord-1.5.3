using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200003A RID: 58
	internal class WidgetContainer
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x0000FE4D File Offset: 0x0000E04D
		internal int Count
		{
			get
			{
				return this.GetActiveList().Count;
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000FE5A File Offset: 0x0000E05A
		internal WidgetContainer(int initialCapacity)
		{
			this._backList = new HashSet<Widget>();
			this._frontList = new MBList<Widget>(initialCapacity);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000FE7C File Offset: 0x0000E07C
		internal void Add(Widget widget)
		{
			HashSet<Widget> backList = this._backList;
			lock (backList)
			{
				this._backList.Add(widget);
			}
			this._isFragmented = true;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000FECC File Offset: 0x0000E0CC
		internal void Remove(Widget widget)
		{
			HashSet<Widget> backList = this._backList;
			lock (backList)
			{
				this._backList.Remove(widget);
			}
			this._isFragmented = true;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000FF1C File Offset: 0x0000E11C
		public void Clear()
		{
			this._backList.Clear();
			this._frontList.Clear();
			this._backList = null;
			this._frontList = null;
			this._isFragmented = true;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000FF49 File Offset: 0x0000E149
		public MBReadOnlyList<Widget> GetActiveList()
		{
			return this._frontList;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000FF54 File Offset: 0x0000E154
		public void Defrag()
		{
			if (!this._isFragmented)
			{
				return;
			}
			this._frontList.Clear();
			HashSet<Widget> backList = this._backList;
			lock (backList)
			{
				foreach (Widget widget in this._backList)
				{
					this._frontList.Add(widget);
				}
			}
			this._isFragmented = false;
		}

		// Token: 0x040001F1 RID: 497
		private HashSet<Widget> _backList;

		// Token: 0x040001F2 RID: 498
		private MBList<Widget> _frontList;

		// Token: 0x040001F3 RID: 499
		private bool _isFragmented;

		// Token: 0x02000083 RID: 131
		internal enum ContainerType
		{
			// Token: 0x04000463 RID: 1123
			Update,
			// Token: 0x04000464 RID: 1124
			ParallelUpdate,
			// Token: 0x04000465 RID: 1125
			LateUpdate,
			// Token: 0x04000466 RID: 1126
			VisualDefinition,
			// Token: 0x04000467 RID: 1127
			UpdateBrushes
		}
	}
}
