using System;
using System.Collections;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x02000082 RID: 130
	public class PriorityQueue<TPriority, TValue> : ICollection<KeyValuePair<TPriority, TValue>>, IEnumerable<KeyValuePair<TPriority, TValue>>, IEnumerable
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x000101B6 File Offset: 0x0000E3B6
		private IComparer<TPriority> Comparer
		{
			get
			{
				if (this._customComparer == null)
				{
					return Comparer<TPriority>.Default;
				}
				return this._customComparer;
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000101CC File Offset: 0x0000E3CC
		public PriorityQueue()
		{
			this._baseHeap = new List<KeyValuePair<TPriority, TValue>>();
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x000101DF File Offset: 0x0000E3DF
		public PriorityQueue(int capacity)
		{
			this._baseHeap = new List<KeyValuePair<TPriority, TValue>>(capacity);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x000101F3 File Offset: 0x0000E3F3
		public PriorityQueue(int capacity, IComparer<TPriority> comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException();
			}
			this._baseHeap = new List<KeyValuePair<TPriority, TValue>>(capacity);
			this._customComparer = comparer;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00010217 File Offset: 0x0000E417
		public PriorityQueue(IComparer<TPriority> comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException();
			}
			this._baseHeap = new List<KeyValuePair<TPriority, TValue>>();
			this._customComparer = comparer;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0001023A File Offset: 0x0000E43A
		public PriorityQueue(IEnumerable<KeyValuePair<TPriority, TValue>> data)
			: this(data, Comparer<TPriority>.Default)
		{
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00010248 File Offset: 0x0000E448
		public PriorityQueue(IEnumerable<KeyValuePair<TPriority, TValue>> data, IComparer<TPriority> comparer)
		{
			if (data == null || comparer == null)
			{
				throw new ArgumentNullException();
			}
			this._customComparer = comparer;
			this._baseHeap = new List<KeyValuePair<TPriority, TValue>>(data);
			for (int i = this._baseHeap.Count / 2 - 1; i >= 0; i--)
			{
				this.HeapifyFromBeginningToEnd(i);
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0001029B File Offset: 0x0000E49B
		public static PriorityQueue<TPriority, TValue> MergeQueues(PriorityQueue<TPriority, TValue> pq1, PriorityQueue<TPriority, TValue> pq2)
		{
			if (pq1 == null || pq2 == null)
			{
				throw new ArgumentNullException();
			}
			if (pq1.Comparer != pq2.Comparer)
			{
				throw new InvalidOperationException("Priority queues to be merged must have equal comparers");
			}
			return PriorityQueue<TPriority, TValue>.MergeQueues(pq1, pq2, pq1.Comparer);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x000102D0 File Offset: 0x0000E4D0
		public static PriorityQueue<TPriority, TValue> MergeQueues(PriorityQueue<TPriority, TValue> pq1, PriorityQueue<TPriority, TValue> pq2, IComparer<TPriority> comparer)
		{
			if (pq1 == null || pq2 == null || comparer == null)
			{
				throw new ArgumentNullException();
			}
			PriorityQueue<TPriority, TValue> priorityQueue = new PriorityQueue<TPriority, TValue>(pq1.Count + pq2.Count, comparer);
			priorityQueue._baseHeap.AddRange(pq1._baseHeap);
			priorityQueue._baseHeap.AddRange(pq2._baseHeap);
			for (int i = priorityQueue._baseHeap.Count / 2 - 1; i >= 0; i--)
			{
				priorityQueue.HeapifyFromBeginningToEnd(i);
			}
			return priorityQueue;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00010344 File Offset: 0x0000E544
		public void Enqueue(TPriority priority, TValue value)
		{
			this.Insert(priority, value);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001034E File Offset: 0x0000E54E
		public KeyValuePair<TPriority, TValue> Dequeue()
		{
			if (!this.IsEmpty)
			{
				KeyValuePair<TPriority, TValue> keyValuePair = this._baseHeap[0];
				this.DeleteRoot();
				return keyValuePair;
			}
			throw new InvalidOperationException("Priority queue is empty");
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00010378 File Offset: 0x0000E578
		public TValue DequeueValue()
		{
			return this.Dequeue().Value;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00010393 File Offset: 0x0000E593
		public KeyValuePair<TPriority, TValue> Peek()
		{
			if (!this.IsEmpty)
			{
				return this._baseHeap[0];
			}
			throw new InvalidOperationException("Priority queue is empty");
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000103B4 File Offset: 0x0000E5B4
		public TValue PeekValue()
		{
			return this.Peek().Value;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x000103CF File Offset: 0x0000E5CF
		public bool IsEmpty
		{
			get
			{
				return this._baseHeap.Count == 0;
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x000103E0 File Offset: 0x0000E5E0
		private void ExchangeElements(int pos1, int pos2)
		{
			KeyValuePair<TPriority, TValue> keyValuePair = this._baseHeap[pos1];
			this._baseHeap[pos1] = this._baseHeap[pos2];
			this._baseHeap[pos2] = keyValuePair;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00010420 File Offset: 0x0000E620
		private void Insert(TPriority priority, TValue value)
		{
			KeyValuePair<TPriority, TValue> keyValuePair = new KeyValuePair<TPriority, TValue>(priority, value);
			this._baseHeap.Add(keyValuePair);
			this.HeapifyFromEndToBeginning(this._baseHeap.Count - 1);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00010458 File Offset: 0x0000E658
		private int HeapifyFromEndToBeginning(int pos)
		{
			if (pos >= this._baseHeap.Count)
			{
				return -1;
			}
			IComparer<TPriority> comparer = this.Comparer;
			while (pos > 0)
			{
				int num = (pos - 1) / 2;
				if (comparer.Compare(this._baseHeap[num].Key, this._baseHeap[pos].Key) >= 0)
				{
					break;
				}
				this.ExchangeElements(num, pos);
				pos = num;
			}
			return pos;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000104C8 File Offset: 0x0000E6C8
		private void DeleteRoot()
		{
			if (this._baseHeap.Count <= 1)
			{
				this._baseHeap.Clear();
				return;
			}
			this._baseHeap[0] = this._baseHeap[this._baseHeap.Count - 1];
			this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
			this.HeapifyFromBeginningToEnd(0);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00010534 File Offset: 0x0000E734
		private void HeapifyFromBeginningToEnd(int pos)
		{
			if (pos >= this._baseHeap.Count)
			{
				return;
			}
			IComparer<TPriority> comparer = this.Comparer;
			for (;;)
			{
				int num = pos;
				int num2 = 2 * pos + 1;
				int num3 = 2 * pos + 2;
				if (num2 < this._baseHeap.Count && comparer.Compare(this._baseHeap[num].Key, this._baseHeap[num2].Key) < 0)
				{
					num = num2;
				}
				if (num3 < this._baseHeap.Count && comparer.Compare(this._baseHeap[num].Key, this._baseHeap[num3].Key) < 0)
				{
					num = num3;
				}
				if (num == pos)
				{
					break;
				}
				this.ExchangeElements(num, pos);
				pos = num;
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000105FF File Offset: 0x0000E7FF
		public void Add(KeyValuePair<TPriority, TValue> item)
		{
			this.Enqueue(item.Key, item.Value);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00010615 File Offset: 0x0000E815
		public void Clear()
		{
			this._baseHeap.Clear();
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00010622 File Offset: 0x0000E822
		public bool Contains(KeyValuePair<TPriority, TValue> item)
		{
			return this._baseHeap.Contains(item);
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x00010630 File Offset: 0x0000E830
		public int Count
		{
			get
			{
				return this._baseHeap.Count;
			}
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001063D File Offset: 0x0000E83D
		public void CopyTo(KeyValuePair<TPriority, TValue>[] array, int arrayIndex)
		{
			this._baseHeap.CopyTo(array, arrayIndex);
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x0001064C File Offset: 0x0000E84C
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00010650 File Offset: 0x0000E850
		public bool Remove(KeyValuePair<TPriority, TValue> item)
		{
			int num = this._baseHeap.IndexOf(item);
			if (num < 0)
			{
				return false;
			}
			this._baseHeap[num] = this._baseHeap[this._baseHeap.Count - 1];
			this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
			if (this.HeapifyFromEndToBeginning(num) == num)
			{
				this.HeapifyFromBeginningToEnd(num);
			}
			return true;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x000106BE File Offset: 0x0000E8BE
		public IEnumerator<KeyValuePair<TPriority, TValue>> GetEnumerator()
		{
			return this._baseHeap.GetEnumerator();
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x000106D0 File Offset: 0x0000E8D0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400016F RID: 367
		private readonly List<KeyValuePair<TPriority, TValue>> _baseHeap;

		// Token: 0x04000170 RID: 368
		private readonly IComparer<TPriority> _customComparer;
	}
}
