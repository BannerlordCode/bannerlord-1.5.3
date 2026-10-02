using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200005B RID: 91
	public class ListChangedEventArgs : EventArgs
	{
		// Token: 0x060002A1 RID: 673 RVA: 0x00007E48 File Offset: 0x00006048
		public ListChangedEventArgs(ListChangedType listChangedType, int newIndex)
		{
			this.ListChangedType = listChangedType;
			this.NewIndex = newIndex;
			this.OldIndex = -1;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00007E65 File Offset: 0x00006065
		public ListChangedEventArgs(ListChangedType listChangedType, int newIndex, int oldIndex)
		{
			this.ListChangedType = listChangedType;
			this.NewIndex = newIndex;
			this.OldIndex = oldIndex;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x00007E82 File Offset: 0x00006082
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x00007E8A File Offset: 0x0000608A
		public ListChangedType ListChangedType { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00007E93 File Offset: 0x00006093
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x00007E9B File Offset: 0x0000609B
		public int NewIndex { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x00007EA4 File Offset: 0x000060A4
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x00007EAC File Offset: 0x000060AC
		public int OldIndex { get; private set; }
	}
}
