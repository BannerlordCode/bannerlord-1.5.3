using System;
using System.Text;

namespace TaleWorlds.Library
{
	// Token: 0x0200001C RID: 28
	public class BinaryWriter : IWriter
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000031F0 File Offset: 0x000013F0
		public int Length
		{
			get
			{
				return this._availableIndex;
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000031F8 File Offset: 0x000013F8
		public BinaryWriter()
			: this(4096)
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003205 File Offset: 0x00001405
		public BinaryWriter(int capacity)
		{
			this._data = new byte[capacity];
			this._availableIndex = 0;
			this._ownerThreadId = Environment.CurrentManagedThreadId;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000322B File Offset: 0x0000142B
		public void Clear()
		{
			Array.Clear(this._data, 0, this._data.Length);
			this._availableIndex = 0;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003248 File Offset: 0x00001448
		public void EnsureLength(int added)
		{
			this.VerifyThreadSafety();
			int num = this._availableIndex + added;
			if (num > this._data.Length)
			{
				int num2 = this._data.Length * 2;
				if (num > num2)
				{
					num2 = num;
				}
				byte[] array = new byte[num2];
				Buffer.BlockCopy(this._data, 0, array, 0, this._availableIndex);
				this._data = array;
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000032A2 File Offset: 0x000014A2
		private void VerifyThreadSafety()
		{
			if (Environment.CurrentManagedThreadId != this._ownerThreadId)
			{
				Debug.Print("Binary writer used by another thread.", 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.FailedAssert("Binary writer used by another thread.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\BinaryWriter.cs", "VerifyThreadSafety", 64);
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000032DD File Offset: 0x000014DD
		public void WriteSerializableObject(ISerializableObject serializableObject)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000032E4 File Offset: 0x000014E4
		public void WriteByte(byte value)
		{
			this.EnsureLength(1);
			this._data[this._availableIndex] = value;
			this._availableIndex++;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003309 File Offset: 0x00001509
		public void WriteBytes(byte[] bytes)
		{
			this.EnsureLength(bytes.Length);
			Buffer.BlockCopy(bytes, 0, this._data, this._availableIndex, bytes.Length);
			this._availableIndex += bytes.Length;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000333C File Offset: 0x0000153C
		public void Write3ByteInt(int value)
		{
			if (value < -1 || value > 16777215)
			{
				Debug.Print("Overflowed while writing 3byte int ({value})", 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.FailedAssert("Overflowed while writing 3byte int ({value})", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\BinaryWriter.cs", "Write3ByteInt", 92);
			}
			this.EnsureLength(3);
			byte[] data = this._data;
			int num = this._availableIndex;
			this._availableIndex = num + 1;
			data[num] = (byte)value;
			byte[] data2 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data2[num] = (byte)(value >> 8);
			byte[] data3 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data3[num] = (byte)(value >> 16);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000033DC File Offset: 0x000015DC
		public void WriteInt(int value)
		{
			this.EnsureLength(4);
			byte[] data = this._data;
			int num = this._availableIndex;
			this._availableIndex = num + 1;
			data[num] = (byte)value;
			byte[] data2 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data2[num] = (byte)(value >> 8);
			byte[] data3 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data3[num] = (byte)(value >> 16);
			byte[] data4 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data4[num] = (byte)(value >> 24);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003460 File Offset: 0x00001660
		public void WriteShort(short value)
		{
			this.EnsureLength(2);
			byte[] data = this._data;
			int num = this._availableIndex;
			this._availableIndex = num + 1;
			data[num] = (byte)value;
			byte[] data2 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data2[num] = (byte)(value >> 8);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000034AC File Offset: 0x000016AC
		public void WriteString(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(value);
				this.WriteInt(bytes.Length);
				this.WriteBytes(bytes);
				return;
			}
			this.WriteInt(0);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000034E5 File Offset: 0x000016E5
		public void WriteColor(Color value)
		{
			this.WriteFloat(value.Red);
			this.WriteFloat(value.Green);
			this.WriteFloat(value.Blue);
			this.WriteFloat(value.Alpha);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003517 File Offset: 0x00001717
		public void WriteBool(bool value)
		{
			this.EnsureLength(1);
			this._data[this._availableIndex] = (value ? 1 : 0);
			this._availableIndex++;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003544 File Offset: 0x00001744
		public void WriteFloat(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			this.EnsureLength(bytes.Length);
			Buffer.BlockCopy(bytes, 0, this._data, this._availableIndex, bytes.Length);
			this._availableIndex += bytes.Length;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003588 File Offset: 0x00001788
		public void WriteUInt(uint value)
		{
			this.EnsureLength(4);
			byte[] data = this._data;
			int num = this._availableIndex;
			this._availableIndex = num + 1;
			data[num] = (byte)value;
			byte[] data2 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data2[num] = (byte)(value >> 8);
			byte[] data3 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data3[num] = (byte)(value >> 16);
			byte[] data4 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data4[num] = (byte)(value >> 24);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000360C File Offset: 0x0000180C
		public void WriteULong(ulong value)
		{
			this.EnsureLength(8);
			byte[] data = this._data;
			int num = this._availableIndex;
			this._availableIndex = num + 1;
			data[num] = (byte)value;
			byte[] data2 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data2[num] = (byte)(value >> 8);
			byte[] data3 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data3[num] = (byte)(value >> 16);
			byte[] data4 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data4[num] = (byte)(value >> 24);
			byte[] data5 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data5[num] = (byte)(value >> 32);
			byte[] data6 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data6[num] = (byte)(value >> 40);
			byte[] data7 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data7[num] = (byte)(value >> 48);
			byte[] data8 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data8[num] = (byte)(value >> 56);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003704 File Offset: 0x00001904
		public void WriteLong(long value)
		{
			this.EnsureLength(8);
			byte[] data = this._data;
			int num = this._availableIndex;
			this._availableIndex = num + 1;
			data[num] = (byte)value;
			byte[] data2 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data2[num] = (byte)(value >> 8);
			byte[] data3 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data3[num] = (byte)(value >> 16);
			byte[] data4 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data4[num] = (byte)(value >> 24);
			byte[] data5 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data5[num] = (byte)(value >> 32);
			byte[] data6 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data6[num] = (byte)(value >> 40);
			byte[] data7 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data7[num] = (byte)(value >> 48);
			byte[] data8 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data8[num] = (byte)(value >> 56);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000037FC File Offset: 0x000019FC
		public void WriteVec2(Vec2 vec2)
		{
			this.WriteFloat(vec2.x);
			this.WriteFloat(vec2.y);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003816 File Offset: 0x00001A16
		public void WriteVec3(Vec3 vec3)
		{
			this.WriteFloat(vec3.x);
			this.WriteFloat(vec3.y);
			this.WriteFloat(vec3.z);
			this.WriteFloat(vec3.w);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003848 File Offset: 0x00001A48
		public void WriteVec3Int(Vec3i vec3)
		{
			this.WriteInt(vec3.X);
			this.WriteInt(vec3.Y);
			this.WriteInt(vec3.Z);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000386E File Offset: 0x00001A6E
		public void WriteSByte(sbyte value)
		{
			this.EnsureLength(1);
			this._data[this._availableIndex] = (byte)value;
			this._availableIndex++;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003894 File Offset: 0x00001A94
		public void WriteUShort(ushort value)
		{
			this.EnsureLength(2);
			byte[] data = this._data;
			int num = this._availableIndex;
			this._availableIndex = num + 1;
			data[num] = (byte)value;
			byte[] data2 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data2[num] = (byte)(value >> 8);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000038E0 File Offset: 0x00001AE0
		public void WriteDouble(double value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			this.EnsureLength(bytes.Length);
			Buffer.BlockCopy(bytes, 0, this._data, this._availableIndex, bytes.Length);
			this._availableIndex += bytes.Length;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003923 File Offset: 0x00001B23
		public void AppendData(BinaryWriter writer)
		{
			this.EnsureLength(writer._availableIndex);
			Buffer.BlockCopy(writer._data, 0, this._data, this._availableIndex, writer._availableIndex);
			this._availableIndex += writer._availableIndex;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003964 File Offset: 0x00001B64
		public byte[] GetFinalData()
		{
			byte[] array = new byte[this._availableIndex];
			Buffer.BlockCopy(this._data, 0, array, 0, this._availableIndex);
			return array;
		}

		// Token: 0x04000061 RID: 97
		private byte[] _data;

		// Token: 0x04000062 RID: 98
		private int _availableIndex;

		// Token: 0x04000063 RID: 99
		private readonly int _ownerThreadId;
	}
}
