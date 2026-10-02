using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000068 RID: 104
	[NullableContext(2)]
	[Nullable(0)]
	internal struct StringBuffer
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x000187D5 File Offset: 0x000169D5
		// (set) Token: 0x060005B7 RID: 1463 RVA: 0x000187DD File Offset: 0x000169DD
		public int Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x000187E6 File Offset: 0x000169E6
		public bool IsEmpty
		{
			get
			{
				return this._buffer == null;
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x000187F1 File Offset: 0x000169F1
		public StringBuffer(IArrayPool<char> bufferPool, int initalSize)
		{
			this = new StringBuffer(BufferUtils.RentBuffer(bufferPool, initalSize));
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00018800 File Offset: 0x00016A00
		[NullableContext(1)]
		private StringBuffer(char[] buffer)
		{
			this._buffer = buffer;
			this._position = 0;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00018810 File Offset: 0x00016A10
		public void Append(IArrayPool<char> bufferPool, char value)
		{
			if (this._position == this._buffer.Length)
			{
				this.EnsureSize(bufferPool, 1);
			}
			char[] buffer = this._buffer;
			int position = this._position;
			this._position = position + 1;
			buffer[position] = value;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00018850 File Offset: 0x00016A50
		[NullableContext(1)]
		public void Append([Nullable(2)] IArrayPool<char> bufferPool, char[] buffer, int startIndex, int count)
		{
			if (this._position + count >= this._buffer.Length)
			{
				this.EnsureSize(bufferPool, count);
			}
			Array.Copy(buffer, startIndex, this._buffer, this._position, count);
			this._position += count;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001889D File Offset: 0x00016A9D
		public void Clear(IArrayPool<char> bufferPool)
		{
			if (this._buffer != null)
			{
				BufferUtils.ReturnBuffer(bufferPool, this._buffer);
				this._buffer = null;
			}
			this._position = 0;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000188C4 File Offset: 0x00016AC4
		private void EnsureSize(IArrayPool<char> bufferPool, int appendLength)
		{
			char[] array = BufferUtils.RentBuffer(bufferPool, (this._position + appendLength) * 2);
			if (this._buffer != null)
			{
				Array.Copy(this._buffer, array, this._position);
				BufferUtils.ReturnBuffer(bufferPool, this._buffer);
			}
			this._buffer = array;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001890F File Offset: 0x00016B0F
		[NullableContext(1)]
		public override string ToString()
		{
			return this.ToString(0, this._position);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001891E File Offset: 0x00016B1E
		[NullableContext(1)]
		public string ToString(int start, int length)
		{
			return new string(this._buffer, start, length);
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x0001892D File Offset: 0x00016B2D
		public char[] InternalBuffer
		{
			get
			{
				return this._buffer;
			}
		}

		// Token: 0x0400021B RID: 539
		private char[] _buffer;

		// Token: 0x0400021C RID: 540
		private int _position;
	}
}
