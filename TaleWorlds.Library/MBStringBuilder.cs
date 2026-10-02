using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace TaleWorlds.Library
{
	// Token: 0x0200001F RID: 31
	public struct MBStringBuilder
	{
		// Token: 0x0600009E RID: 158 RVA: 0x00003E37 File Offset: 0x00002037
		public void Initialize(int capacity = 16, [CallerMemberName] string callerMemberName = "")
		{
			this._cachedStringBuilder = MBStringBuilder.CachedStringBuilder.Acquire(capacity);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003E45 File Offset: 0x00002045
		public string ToStringAndRelease()
		{
			string text = this._cachedStringBuilder.ToString();
			this.Release();
			return text;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003E58 File Offset: 0x00002058
		public void Release()
		{
			MBStringBuilder.CachedStringBuilder.Release(this._cachedStringBuilder);
			this._cachedStringBuilder = null;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003E6C File Offset: 0x0000206C
		public MBStringBuilder Append(char value)
		{
			this._cachedStringBuilder.Append(value);
			return this;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003E81 File Offset: 0x00002081
		public MBStringBuilder Append(int value)
		{
			this._cachedStringBuilder.Append(value);
			return this;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003E96 File Offset: 0x00002096
		public MBStringBuilder Append(uint value)
		{
			this._cachedStringBuilder.Append(value);
			return this;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003EAB File Offset: 0x000020AB
		public MBStringBuilder Append(float value)
		{
			this._cachedStringBuilder.Append(value);
			return this;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003EC0 File Offset: 0x000020C0
		public MBStringBuilder Append(double value)
		{
			this._cachedStringBuilder.Append(value);
			return this;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003ED5 File Offset: 0x000020D5
		public MBStringBuilder Append<T>(T value)
		{
			this._cachedStringBuilder.Append(value);
			return this;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003EEF File Offset: 0x000020EF
		public MBStringBuilder AppendLine()
		{
			this._cachedStringBuilder.AppendLine();
			return this;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003F03 File Offset: 0x00002103
		public MBStringBuilder AppendLine<T>(T value)
		{
			this.Append<T>(value);
			this.AppendLine();
			return this;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003F1A File Offset: 0x0000211A
		public int Length
		{
			get
			{
				return this._cachedStringBuilder.Length;
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003F27 File Offset: 0x00002127
		public override string ToString()
		{
			Debug.FailedAssert("Don't use this. Use ToStringAndRelease instead!", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\CachedStringBuilder.cs", "ToString", 190);
			return null;
		}

		// Token: 0x04000066 RID: 102
		private StringBuilder _cachedStringBuilder;

		// Token: 0x020000C9 RID: 201
		private static class CachedStringBuilder
		{
			// Token: 0x06000753 RID: 1875 RVA: 0x000186BF File Offset: 0x000168BF
			public static StringBuilder Acquire(int capacity = 16)
			{
				if (capacity <= 4096 && MBStringBuilder.CachedStringBuilder._cachedStringBuilder != null)
				{
					StringBuilder cachedStringBuilder = MBStringBuilder.CachedStringBuilder._cachedStringBuilder;
					MBStringBuilder.CachedStringBuilder._cachedStringBuilder = null;
					cachedStringBuilder.EnsureCapacity(capacity);
					return cachedStringBuilder;
				}
				return new StringBuilder(capacity);
			}

			// Token: 0x06000754 RID: 1876 RVA: 0x000186EA File Offset: 0x000168EA
			public static void Release(StringBuilder sb)
			{
				if (sb.Capacity <= 4096)
				{
					MBStringBuilder.CachedStringBuilder._cachedStringBuilder = sb;
					MBStringBuilder.CachedStringBuilder._cachedStringBuilder.Clear();
				}
			}

			// Token: 0x06000755 RID: 1877 RVA: 0x0001870A File Offset: 0x0001690A
			public static string GetStringAndReleaseBuilder(StringBuilder sb)
			{
				string text = sb.ToString();
				MBStringBuilder.CachedStringBuilder.Release(sb);
				return text;
			}

			// Token: 0x0400025C RID: 604
			private const int MaxBuilderSize = 4096;

			// Token: 0x0400025D RID: 605
			[ThreadStatic]
			private static StringBuilder _cachedStringBuilder;
		}
	}
}
