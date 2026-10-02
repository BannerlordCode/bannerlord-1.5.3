using System;
using Newtonsoft.Json;
using TaleWorlds.Library;

namespace TaleWorlds.PlayerServices
{
	// Token: 0x02000003 RID: 3
	[JsonConverter(typeof(PlayerIdJsonConverter))]
	[Serializable]
	public struct PlayerId : IComparable<PlayerId>, IEquatable<PlayerId>
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000021FC File Offset: 0x000003FC
		public ulong Id1
		{
			get
			{
				return this._id1;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002204 File Offset: 0x00000404
		public ulong Id2
		{
			get
			{
				return this._id2;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000220C File Offset: 0x0000040C
		public bool IsValid
		{
			get
			{
				return this._id1 != 0UL || this._id2 > 0UL;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002222 File Offset: 0x00000422
		public PlayerIdProvidedTypes ProvidedType
		{
			get
			{
				return (PlayerIdProvidedTypes)this._providedType;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000E RID: 14 RVA: 0x0000222C File Offset: 0x0000042C
		public ulong Part1
		{
			get
			{
				return (ulong)this._providedType | ((ulong)this._reserved1 << 8) | ((ulong)this._reserved2 << 16) | ((ulong)this._reserved3 << 24) | ((ulong)this._reserved4 << 32) | ((ulong)this._reserved5 << 40) | ((ulong)this._reserved6 << 48) | ((ulong)this._reserved7 << 56);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000228C File Offset: 0x0000048C
		public ulong Part2
		{
			get
			{
				return this._reservedBig;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002294 File Offset: 0x00000494
		public ulong Part3
		{
			get
			{
				return this._id1;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000229C File Offset: 0x0000049C
		public ulong Part4
		{
			get
			{
				return this._id2;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000022A4 File Offset: 0x000004A4
		public static PlayerId Empty
		{
			get
			{
				return new PlayerId(0, 0UL, 0UL, 0UL);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000022B4 File Offset: 0x000004B4
		public PlayerId(byte providedType, ulong id1, ulong id2)
		{
			this._providedType = providedType;
			this._reserved1 = 0;
			this._reserved2 = 0;
			this._reserved3 = 0;
			this._reserved4 = 0;
			this._reserved5 = 0;
			this._reserved6 = 0;
			this._reserved7 = 0;
			this._reservedBig = 0UL;
			this._id1 = id1;
			this._id2 = id2;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002310 File Offset: 0x00000510
		public PlayerId(byte providedType, ulong reservedBig, ulong id1, ulong id2)
		{
			this._providedType = providedType;
			this._reserved1 = 0;
			this._reserved2 = 0;
			this._reserved3 = 0;
			this._reserved4 = 0;
			this._reserved5 = 0;
			this._reserved6 = 0;
			this._reserved7 = 0;
			this._reservedBig = reservedBig;
			this._id1 = id1;
			this._id2 = id2;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000236C File Offset: 0x0000056C
		public PlayerId(byte providedType, string guid)
		{
			byte[] array = Guid.Parse(guid).ToByteArray();
			this._providedType = providedType;
			this._reserved1 = 0;
			this._reserved2 = 0;
			this._reserved3 = 0;
			this._reserved4 = 0;
			this._reserved5 = 0;
			this._reserved6 = 0;
			this._reserved7 = 0;
			this._reservedBig = 0UL;
			this._id1 = BitConverter.ToUInt64(array, 0);
			this._id2 = BitConverter.ToUInt64(array, 8);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000023E4 File Offset: 0x000005E4
		public PlayerId(ulong part1, ulong part2, ulong part3, ulong part4)
		{
			byte[] bytes = BitConverter.GetBytes(part1);
			this._providedType = bytes[0];
			this._reserved1 = bytes[1];
			this._reserved2 = bytes[2];
			this._reserved3 = bytes[3];
			this._reserved4 = bytes[4];
			this._reserved5 = bytes[5];
			this._reserved6 = bytes[6];
			this._reserved7 = bytes[7];
			this._reservedBig = part2;
			this._id1 = part3;
			this._id2 = part4;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002458 File Offset: 0x00000658
		public PlayerId(byte[] data)
		{
			this._providedType = data[0];
			this._reserved1 = data[1];
			this._reserved2 = data[2];
			this._reserved3 = data[3];
			this._reserved4 = data[4];
			this._reserved5 = data[5];
			this._reserved6 = data[6];
			this._reserved7 = data[7];
			this._reservedBig = BitConverter.ToUInt64(data, 8);
			this._id1 = BitConverter.ToUInt64(data, 16);
			this._id2 = BitConverter.ToUInt64(data, 24);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000024D8 File Offset: 0x000006D8
		public PlayerId(Guid guid)
		{
			byte[] array = guid.ToByteArray();
			this._providedType = 0;
			this._reserved1 = 0;
			this._reserved2 = 0;
			this._reserved3 = 0;
			this._reserved4 = 0;
			this._reserved5 = 0;
			this._reserved6 = 0;
			this._reserved7 = 0;
			this._reservedBig = 0UL;
			this._id1 = BitConverter.ToUInt64(array, 0);
			this._id2 = BitConverter.ToUInt64(array, 8);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002548 File Offset: 0x00000748
		public byte[] ToByteArray()
		{
			byte[] array = new byte[32];
			array[0] = this._providedType;
			array[1] = this._reserved1;
			array[2] = this._reserved2;
			array[3] = this._reserved3;
			array[4] = this._reserved4;
			array[5] = this._reserved5;
			array[6] = this._reserved6;
			array[7] = this._reserved7;
			byte[] bytes = BitConverter.GetBytes(this._reservedBig);
			byte[] bytes2 = BitConverter.GetBytes(this._id1);
			byte[] bytes3 = BitConverter.GetBytes(this._id2);
			for (int i = 0; i < 8; i++)
			{
				array[8 + i] = bytes[i];
				array[16 + i] = bytes2[i];
				array[24 + i] = bytes3[i];
			}
			return array;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000025FA File Offset: 0x000007FA
		public void Serialize(IWriter writer)
		{
			writer.WriteULong(this.Part1);
			writer.WriteULong(this.Part2);
			writer.WriteULong(this.Part3);
			writer.WriteULong(this.Part4);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000262C File Offset: 0x0000082C
		public void Deserialize(IReader reader)
		{
			ulong num = reader.ReadULong();
			ulong num2 = reader.ReadULong();
			ulong num3 = reader.ReadULong();
			ulong num4 = reader.ReadULong();
			byte[] bytes = BitConverter.GetBytes(num);
			this._providedType = bytes[0];
			this._reserved1 = bytes[1];
			this._reserved2 = bytes[2];
			this._reserved3 = bytes[3];
			this._reserved4 = bytes[4];
			this._reserved5 = bytes[5];
			this._reserved6 = bytes[6];
			this._reserved7 = bytes[7];
			this._reservedBig = num2;
			this._id1 = num3;
			this._id2 = num4;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000026B8 File Offset: 0x000008B8
		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}.{3}", new object[] { this.Part1, this.Part2, this.Part3, this.Part4 });
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002710 File Offset: 0x00000910
		public static bool operator ==(PlayerId a, PlayerId b)
		{
			return a.Part1 == b.Part1 && a.Part2 == b.Part2 && a.Part3 == b.Part3 && a.Part4 == b.Part4;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002760 File Offset: 0x00000960
		public static bool operator !=(PlayerId a, PlayerId b)
		{
			return a.Part1 != b.Part1 || a.Part2 != b.Part2 || a.Part3 != b.Part3 || a.Part4 != b.Part4;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000027B4 File Offset: 0x000009B4
		public override bool Equals(object o)
		{
			if (o != null && o is PlayerId)
			{
				PlayerId playerId = (PlayerId)o;
				if (this == playerId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000027E4 File Offset: 0x000009E4
		public override int GetHashCode()
		{
			int hashCode = this.Part1.GetHashCode();
			int hashCode2 = this.Part2.GetHashCode();
			int hashCode3 = this.Part3.GetHashCode();
			int hashCode4 = this.Part4.GetHashCode();
			return hashCode ^ hashCode2 ^ hashCode3 ^ hashCode4;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002834 File Offset: 0x00000A34
		public static PlayerId FromString(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				string[] array = id.Split(new char[] { '.' });
				ulong num = Convert.ToUInt64(array[0]);
				ulong num2 = Convert.ToUInt64(array[1]);
				ulong num3 = Convert.ToUInt64(array[2]);
				ulong num4 = Convert.ToUInt64(array[3]);
				return new PlayerId(num, num2, num3, num4);
			}
			return default(PlayerId);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002894 File Offset: 0x00000A94
		public int CompareTo(PlayerId other)
		{
			if (this.Part1 != other.Part1)
			{
				return this.Part1.CompareTo(other.Part1);
			}
			if (this.Part2 != other.Part2)
			{
				return this.Part2.CompareTo(other.Part2);
			}
			if (this.Part3 != other.Part3)
			{
				return this.Part3.CompareTo(other.Part3);
			}
			return this.Part4.CompareTo(other.Part4);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002925 File Offset: 0x00000B25
		public bool Equals(PlayerId other)
		{
			return this == other;
		}

		// Token: 0x04000004 RID: 4
		private byte _providedType;

		// Token: 0x04000005 RID: 5
		private byte _reserved1;

		// Token: 0x04000006 RID: 6
		private byte _reserved2;

		// Token: 0x04000007 RID: 7
		private byte _reserved3;

		// Token: 0x04000008 RID: 8
		private byte _reserved4;

		// Token: 0x04000009 RID: 9
		private byte _reserved5;

		// Token: 0x0400000A RID: 10
		private byte _reserved6;

		// Token: 0x0400000B RID: 11
		private byte _reserved7;

		// Token: 0x0400000C RID: 12
		private ulong _reservedBig;

		// Token: 0x0400000D RID: 13
		private ulong _id1;

		// Token: 0x0400000E RID: 14
		private ulong _id2;
	}
}
