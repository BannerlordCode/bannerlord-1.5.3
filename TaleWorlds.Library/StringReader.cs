using System;
using System.Globalization;

namespace TaleWorlds.Library
{
	// Token: 0x02000091 RID: 145
	public class StringReader : IReader
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x00012741 File Offset: 0x00010941
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x00012749 File Offset: 0x00010949
		public string Data { get; private set; }

		// Token: 0x06000518 RID: 1304 RVA: 0x00012752 File Offset: 0x00010952
		private string GetNextToken()
		{
			string text = this._tokens[this._currentIndex];
			this._currentIndex++;
			return text;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001276F File Offset: 0x0001096F
		public StringReader(string data)
		{
			this.Data = data;
			this._tokens = data.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00012796 File Offset: 0x00010996
		public ISerializableObject ReadSerializableObject()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001279D File Offset: 0x0001099D
		public int ReadInt()
		{
			return Convert.ToInt32(this.GetNextToken());
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x000127AA File Offset: 0x000109AA
		public short ReadShort()
		{
			return Convert.ToInt16(this.GetNextToken());
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000127B8 File Offset: 0x000109B8
		public string ReadString()
		{
			int num = this.ReadInt();
			int i = 0;
			string text = "";
			while (i < num)
			{
				string nextToken = this.GetNextToken();
				text += nextToken;
				i = text.Length;
				if (i < num)
				{
					text += " ";
				}
			}
			if (text.Length != num)
			{
				throw new Exception("invalid string format, length does not match");
			}
			return text;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00012818 File Offset: 0x00010A18
		public Color ReadColor()
		{
			float num = this.ReadFloat();
			float num2 = this.ReadFloat();
			float num3 = this.ReadFloat();
			float num4 = this.ReadFloat();
			return new Color(num, num2, num3, num4);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001284C File Offset: 0x00010A4C
		public bool ReadBool()
		{
			string nextToken = this.GetNextToken();
			return nextToken == "1" || (!(nextToken == "0") && Convert.ToBoolean(nextToken));
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00012884 File Offset: 0x00010A84
		public float ReadFloat()
		{
			return Convert.ToSingle(this.GetNextToken(), CultureInfo.InvariantCulture);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00012896 File Offset: 0x00010A96
		public uint ReadUInt()
		{
			return Convert.ToUInt32(this.GetNextToken());
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000128A3 File Offset: 0x00010AA3
		public ulong ReadULong()
		{
			return Convert.ToUInt64(this.GetNextToken());
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000128B0 File Offset: 0x00010AB0
		public long ReadLong()
		{
			return Convert.ToInt64(this.GetNextToken());
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000128BD File Offset: 0x00010ABD
		public byte ReadByte()
		{
			return Convert.ToByte(this.GetNextToken());
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000128CA File Offset: 0x00010ACA
		public byte[] ReadBytes(int length)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000128D4 File Offset: 0x00010AD4
		public Vec2 ReadVec2()
		{
			float num = this.ReadFloat();
			float num2 = this.ReadFloat();
			return new Vec2(num, num2);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x000128F4 File Offset: 0x00010AF4
		public Vec3 ReadVec3()
		{
			float num = this.ReadFloat();
			float num2 = this.ReadFloat();
			float num3 = this.ReadFloat();
			float num4 = this.ReadFloat();
			return new Vec3(num, num2, num3, num4);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00012924 File Offset: 0x00010B24
		public Vec3i ReadVec3Int()
		{
			int num = this.ReadInt();
			int num2 = this.ReadInt();
			int num3 = this.ReadInt();
			return new Vec3i(num, num2, num3);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001294C File Offset: 0x00010B4C
		public sbyte ReadSByte()
		{
			return Convert.ToSByte(this.GetNextToken());
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00012959 File Offset: 0x00010B59
		public ushort ReadUShort()
		{
			return Convert.ToUInt16(this.GetNextToken());
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00012966 File Offset: 0x00010B66
		public double ReadDouble()
		{
			return Convert.ToDouble(this.GetNextToken());
		}

		// Token: 0x0400019A RID: 410
		private string[] _tokens;

		// Token: 0x0400019B RID: 411
		private int _currentIndex;
	}
}
