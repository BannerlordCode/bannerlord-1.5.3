using System;
using System.Collections.Specialized;

namespace TaleWorlds.Core
{
	// Token: 0x020000D2 RID: 210
	public class StackArray
	{
		// Token: 0x0200012D RID: 301
		public struct StackArray3Float
		{
			// Token: 0x17000406 RID: 1030
			public float this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._element0;
					case 1:
						return this._element1;
					case 2:
						return this._element2;
					default:
						return 0f;
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._element0 = value;
						return;
					case 1:
						this._element1 = value;
						return;
					case 2:
						this._element2 = value;
						return;
					default:
						return;
					}
				}
			}

			// Token: 0x040007EA RID: 2026
			private float _element0;

			// Token: 0x040007EB RID: 2027
			private float _element1;

			// Token: 0x040007EC RID: 2028
			private float _element2;

			// Token: 0x040007ED RID: 2029
			public const int Length = 3;
		}

		// Token: 0x0200012E RID: 302
		public struct StackArray5Float
		{
			// Token: 0x17000407 RID: 1031
			public float this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._element0;
					case 1:
						return this._element1;
					case 2:
						return this._element2;
					case 3:
						return this._element3;
					case 4:
						return this._element4;
					default:
						return 0f;
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._element0 = value;
						return;
					case 1:
						this._element1 = value;
						return;
					case 2:
						this._element2 = value;
						return;
					case 3:
						this._element3 = value;
						return;
					case 4:
						this._element4 = value;
						return;
					default:
						return;
					}
				}
			}

			// Token: 0x040007EE RID: 2030
			private float _element0;

			// Token: 0x040007EF RID: 2031
			private float _element1;

			// Token: 0x040007F0 RID: 2032
			private float _element2;

			// Token: 0x040007F1 RID: 2033
			private float _element3;

			// Token: 0x040007F2 RID: 2034
			private float _element4;

			// Token: 0x040007F3 RID: 2035
			public const int Length = 5;
		}

		// Token: 0x0200012F RID: 303
		public struct StackArray3Int
		{
			// Token: 0x17000408 RID: 1032
			public int this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._element0;
					case 1:
						return this._element1;
					case 2:
						return this._element2;
					default:
						return 0;
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._element0 = value;
						return;
					case 1:
						this._element1 = value;
						return;
					case 2:
						this._element2 = value;
						return;
					default:
						return;
					}
				}
			}

			// Token: 0x040007F4 RID: 2036
			private int _element0;

			// Token: 0x040007F5 RID: 2037
			private int _element1;

			// Token: 0x040007F6 RID: 2038
			private int _element2;

			// Token: 0x040007F7 RID: 2039
			public const int Length = 3;
		}

		// Token: 0x02000130 RID: 304
		public struct StackArray4Int
		{
			// Token: 0x17000409 RID: 1033
			public int this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._element0;
					case 1:
						return this._element1;
					case 2:
						return this._element2;
					case 3:
						return this._element3;
					default:
						return 0;
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._element0 = value;
						return;
					case 1:
						this._element1 = value;
						return;
					case 2:
						this._element2 = value;
						return;
					case 3:
						this._element3 = value;
						return;
					default:
						return;
					}
				}
			}

			// Token: 0x040007F8 RID: 2040
			private int _element0;

			// Token: 0x040007F9 RID: 2041
			private int _element1;

			// Token: 0x040007FA RID: 2042
			private int _element2;

			// Token: 0x040007FB RID: 2043
			private int _element3;

			// Token: 0x040007FC RID: 2044
			public const int Length = 4;
		}

		// Token: 0x02000131 RID: 305
		public struct StackArray2Bool
		{
			// Token: 0x1700040A RID: 1034
			public bool this[int index]
			{
				get
				{
					return ((int)this._element & (1 << index)) != 0;
				}
				set
				{
					this._element = (byte)(value ? ((int)this._element | (1 << index)) : ((int)this._element & ~(1 << index)));
				}
			}

			// Token: 0x040007FD RID: 2045
			private byte _element;

			// Token: 0x040007FE RID: 2046
			public const int Length = 2;
		}

		// Token: 0x02000132 RID: 306
		public struct StackArray8Int
		{
			// Token: 0x1700040B RID: 1035
			public int this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._element0;
					case 1:
						return this._element1;
					case 2:
						return this._element2;
					case 3:
						return this._element3;
					case 4:
						return this._element4;
					case 5:
						return this._element5;
					case 6:
						return this._element6;
					case 7:
						return this._element7;
					default:
						return 0;
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._element0 = value;
						return;
					case 1:
						this._element1 = value;
						return;
					case 2:
						this._element2 = value;
						return;
					case 3:
						this._element3 = value;
						return;
					case 4:
						this._element4 = value;
						return;
					case 5:
						this._element5 = value;
						return;
					case 6:
						this._element6 = value;
						return;
					case 7:
						this._element7 = value;
						return;
					default:
						return;
					}
				}
			}

			// Token: 0x040007FF RID: 2047
			private int _element0;

			// Token: 0x04000800 RID: 2048
			private int _element1;

			// Token: 0x04000801 RID: 2049
			private int _element2;

			// Token: 0x04000802 RID: 2050
			private int _element3;

			// Token: 0x04000803 RID: 2051
			private int _element4;

			// Token: 0x04000804 RID: 2052
			private int _element5;

			// Token: 0x04000805 RID: 2053
			private int _element6;

			// Token: 0x04000806 RID: 2054
			private int _element7;

			// Token: 0x04000807 RID: 2055
			public const int Length = 8;
		}

		// Token: 0x02000133 RID: 307
		public struct StackArray10FloatFloatTuple
		{
			// Token: 0x1700040C RID: 1036
			public ValueTuple<float, float> this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._element0;
					case 1:
						return this._element1;
					case 2:
						return this._element2;
					case 3:
						return this._element3;
					case 4:
						return this._element4;
					case 5:
						return this._element5;
					case 6:
						return this._element6;
					case 7:
						return this._element7;
					case 8:
						return this._element8;
					case 9:
						return this._element9;
					default:
						return new ValueTuple<float, float>(0f, 0f);
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._element0 = value;
						return;
					case 1:
						this._element1 = value;
						return;
					case 2:
						this._element2 = value;
						return;
					case 3:
						this._element3 = value;
						return;
					case 4:
						this._element4 = value;
						return;
					case 5:
						this._element5 = value;
						return;
					case 6:
						this._element6 = value;
						return;
					case 7:
						this._element7 = value;
						return;
					case 8:
						this._element8 = value;
						return;
					case 9:
						this._element9 = value;
						return;
					default:
						return;
					}
				}
			}

			// Token: 0x04000808 RID: 2056
			private ValueTuple<float, float> _element0;

			// Token: 0x04000809 RID: 2057
			private ValueTuple<float, float> _element1;

			// Token: 0x0400080A RID: 2058
			private ValueTuple<float, float> _element2;

			// Token: 0x0400080B RID: 2059
			private ValueTuple<float, float> _element3;

			// Token: 0x0400080C RID: 2060
			private ValueTuple<float, float> _element4;

			// Token: 0x0400080D RID: 2061
			private ValueTuple<float, float> _element5;

			// Token: 0x0400080E RID: 2062
			private ValueTuple<float, float> _element6;

			// Token: 0x0400080F RID: 2063
			private ValueTuple<float, float> _element7;

			// Token: 0x04000810 RID: 2064
			private ValueTuple<float, float> _element8;

			// Token: 0x04000811 RID: 2065
			private ValueTuple<float, float> _element9;

			// Token: 0x04000812 RID: 2066
			public const int Length = 10;
		}

		// Token: 0x02000134 RID: 308
		public struct StackArray3Bool
		{
			// Token: 0x1700040D RID: 1037
			public bool this[int index]
			{
				get
				{
					return ((int)this._element & (1 << index)) != 0;
				}
				set
				{
					this._element = (byte)(value ? ((int)this._element | (1 << index)) : ((int)this._element & ~(1 << index)));
				}
			}

			// Token: 0x04000813 RID: 2067
			private byte _element;

			// Token: 0x04000814 RID: 2068
			public const int Length = 3;
		}

		// Token: 0x02000135 RID: 309
		public struct StackArray4Bool
		{
			// Token: 0x1700040E RID: 1038
			public bool this[int index]
			{
				get
				{
					return ((int)this._element & (1 << index)) != 0;
				}
				set
				{
					this._element = (byte)(value ? ((int)this._element | (1 << index)) : ((int)this._element & ~(1 << index)));
				}
			}

			// Token: 0x04000815 RID: 2069
			private byte _element;

			// Token: 0x04000816 RID: 2070
			public const int Length = 4;
		}

		// Token: 0x02000136 RID: 310
		public struct StackArray5Bool
		{
			// Token: 0x1700040F RID: 1039
			public bool this[int index]
			{
				get
				{
					return ((int)this._element & (1 << index)) != 0;
				}
				set
				{
					this._element = (byte)(value ? ((int)this._element | (1 << index)) : ((int)this._element & ~(1 << index)));
				}
			}

			// Token: 0x04000817 RID: 2071
			private byte _element;

			// Token: 0x04000818 RID: 2072
			public const int Length = 5;
		}

		// Token: 0x02000137 RID: 311
		public struct StackArray6Bool
		{
			// Token: 0x17000410 RID: 1040
			public bool this[int index]
			{
				get
				{
					return ((int)this._element & (1 << index)) != 0;
				}
				set
				{
					this._element = (byte)(value ? ((int)this._element | (1 << index)) : ((int)this._element & ~(1 << index)));
				}
			}

			// Token: 0x04000819 RID: 2073
			private byte _element;

			// Token: 0x0400081A RID: 2074
			public const int Length = 6;
		}

		// Token: 0x02000138 RID: 312
		public struct StackArray7Bool
		{
			// Token: 0x17000411 RID: 1041
			public bool this[int index]
			{
				get
				{
					return ((int)this._element & (1 << index)) != 0;
				}
				set
				{
					this._element = (byte)(value ? ((int)this._element | (1 << index)) : ((int)this._element & ~(1 << index)));
				}
			}

			// Token: 0x0400081B RID: 2075
			private byte _element;

			// Token: 0x0400081C RID: 2076
			public const int Length = 7;
		}

		// Token: 0x02000139 RID: 313
		public struct StackArray8Bool
		{
			// Token: 0x17000412 RID: 1042
			public bool this[int index]
			{
				get
				{
					return ((int)this._element & (1 << index)) != 0;
				}
				set
				{
					this._element = (byte)(value ? ((int)this._element | (1 << index)) : ((int)this._element & ~(1 << index)));
				}
			}

			// Token: 0x0400081D RID: 2077
			private byte _element;

			// Token: 0x0400081E RID: 2078
			public const int Length = 8;
		}

		// Token: 0x0200013A RID: 314
		public struct StackArray32Bool
		{
			// Token: 0x17000413 RID: 1043
			public bool this[int index]
			{
				get
				{
					return this._element[1 << index];
				}
				set
				{
					this._element[1 << index] = value;
				}
			}

			// Token: 0x06000C49 RID: 3145 RVA: 0x000271F0 File Offset: 0x000253F0
			public StackArray32Bool(int init)
			{
				this._element = new BitVector32(init);
			}

			// Token: 0x0400081F RID: 2079
			private BitVector32 _element;

			// Token: 0x04000820 RID: 2080
			public const int Length = 32;
		}
	}
}
