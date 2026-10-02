using System;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x0200006C RID: 108
	public class MBQueue<T> : MBReadOnlyQueue<T>, IMBCollection
	{
		// Token: 0x060003D3 RID: 979 RVA: 0x0000DCC2 File Offset: 0x0000BEC2
		public MBQueue()
		{
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000DCCA File Offset: 0x0000BECA
		public MBQueue(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000DCD3 File Offset: 0x0000BED3
		public MBQueue(Queue<T> queue)
			: base(queue)
		{
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000DCDC File Offset: 0x0000BEDC
		public MBQueue(IEnumerable<T> collection)
			: base(collection)
		{
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000DCE8 File Offset: 0x0000BEE8
		public bool Remove(T item)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			int count = base.Count;
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				T t = base.Dequeue();
				if (!flag && @default.Equals(t, item))
				{
					flag = true;
				}
				else
				{
					base.Enqueue(t);
				}
			}
			return flag;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000DD33 File Offset: 0x0000BF33
		void IMBCollection.Clear()
		{
			base.Clear();
		}
	}
}
