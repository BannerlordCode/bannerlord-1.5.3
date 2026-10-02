using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000069 RID: 105
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct StringReference
	{
		// Token: 0x170000D2 RID: 210
		public char this[int i]
		{
			get
			{
				return this._chars[i];
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x0001893F File Offset: 0x00016B3F
		public char[] Chars
		{
			get
			{
				return this._chars;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00018947 File Offset: 0x00016B47
		public int StartIndex
		{
			get
			{
				return this._startIndex;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0001894F File Offset: 0x00016B4F
		public int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00018957 File Offset: 0x00016B57
		public StringReference(char[] chars, int startIndex, int length)
		{
			this._chars = chars;
			this._startIndex = startIndex;
			this._length = length;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001896E File Offset: 0x00016B6E
		public override string ToString()
		{
			return new string(this._chars, this._startIndex, this._length);
		}

		// Token: 0x0400021D RID: 541
		private readonly char[] _chars;

		// Token: 0x0400021E RID: 542
		private readonly int _startIndex;

		// Token: 0x0400021F RID: 543
		private readonly int _length;
	}
}
