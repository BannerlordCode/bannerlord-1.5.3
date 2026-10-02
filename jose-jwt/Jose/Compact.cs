using System;
using System.Text;

namespace Jose
{
	// Token: 0x0200002D RID: 45
	public class Compact
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x00005A1C File Offset: 0x00003C1C
		public static string Serialize(params byte[][] parts)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte[] array in parts)
			{
				stringBuilder.Append(Base64Url.Encode(array)).Append(".");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005A70 File Offset: 0x00003C70
		public static string Serialize(byte[] header, string payload, params byte[][] other)
		{
			StringBuilder stringBuilder = new StringBuilder().Append(Base64Url.Encode(header)).Append(".").Append(payload)
				.Append(".");
			foreach (byte[] array in other)
			{
				stringBuilder.Append(Base64Url.Encode(array)).Append(".");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005AEC File Offset: 0x00003CEC
		public static byte[][] Parse(string token)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			string[] array = token.Split(new char[] { '.' });
			byte[][] array2 = new byte[array.Length][];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = Base64Url.Decode(array[i]);
			}
			return array2;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00005B3C File Offset: 0x00003D3C
		public static Compact.Iterator Iterate(string token)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			return new Compact.Iterator(token.Split(new char[] { '.' }));
		}

		// Token: 0x02000036 RID: 54
		public class Iterator
		{
			// Token: 0x060000F3 RID: 243 RVA: 0x00005FF6 File Offset: 0x000041F6
			public Iterator(string[] parts)
			{
				this.parts = parts;
				this.current = 0;
			}

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000600C File Offset: 0x0000420C
			public int Count
			{
				get
				{
					return this.parts.Length;
				}
			}

			// Token: 0x060000F5 RID: 245 RVA: 0x00006018 File Offset: 0x00004218
			public byte[] Next(bool decode = true)
			{
				if (this.current >= this.parts.Length)
				{
					return null;
				}
				string[] array = this.parts;
				int num = this.current;
				this.current = num + 1;
				string text = array[num];
				if (!decode)
				{
					return Encoding.UTF8.GetBytes(text);
				}
				return Base64Url.Decode(text);
			}

			// Token: 0x04000077 RID: 119
			private string[] parts;

			// Token: 0x04000078 RID: 120
			private int current;
		}
	}
}
