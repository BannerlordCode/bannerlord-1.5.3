using System;
using System.IO;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public class GameData
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000026F9 File Offset: 0x000008F9
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002701 File Offset: 0x00000901
		public byte[] Header { get; internal set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002B RID: 43 RVA: 0x0000270A File Offset: 0x0000090A
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002712 File Offset: 0x00000912
		public byte[] Strings { get; internal set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002D RID: 45 RVA: 0x0000271B File Offset: 0x0000091B
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002723 File Offset: 0x00000923
		public byte[][] ObjectData { get; internal set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002F RID: 47 RVA: 0x0000272C File Offset: 0x0000092C
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002734 File Offset: 0x00000934
		public byte[][] ContainerData { get; internal set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002740 File Offset: 0x00000940
		public int TotalSize
		{
			get
			{
				int num = this.Header.Length;
				num += this.Strings.Length;
				for (int i = 0; i < this.ObjectData.Length; i++)
				{
					num += this.ObjectData[i].Length;
				}
				for (int j = 0; j < this.ContainerData.Length; j++)
				{
					num += this.ContainerData[j].Length;
				}
				return num;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000027A2 File Offset: 0x000009A2
		public GameData(byte[] header, byte[] strings, byte[][] objectData, byte[][] containerData)
		{
			this.Header = header;
			this.Strings = strings;
			this.ObjectData = objectData;
			this.ContainerData = containerData;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000027C7 File Offset: 0x000009C7
		public GameData()
		{
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000027D0 File Offset: 0x000009D0
		public void Inspect()
		{
			Debug.Print(string.Format("Header Size: {0} Strings Size: {1} Object Size: {2} Container Size: {3}", new object[]
			{
				this.Header.Length,
				this.Strings.Length,
				this.ObjectData.Length,
				this.ContainerData.Length
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			float num = (float)this.TotalSize / 1048576f;
			Debug.Print(string.Format("Total size: {0:##.00} MB", num), 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002870 File Offset: 0x00000A70
		public static GameData CreateFrom(byte[] readBytes)
		{
			BinaryReader binaryReader = new BinaryReader(readBytes);
			int num = binaryReader.ReadInt();
			byte[] array = binaryReader.ReadBytes(num);
			int num2 = binaryReader.ReadInt();
			byte[] array2 = binaryReader.ReadBytes(num2);
			int num3 = binaryReader.ReadInt();
			byte[][] array3 = new byte[num3][];
			for (int i = 0; i < num3; i++)
			{
				int num4 = binaryReader.ReadInt();
				byte[] array4 = binaryReader.ReadBytes(num4);
				array3[i] = array4;
			}
			int num5 = binaryReader.ReadInt();
			byte[][] array5 = new byte[num5][];
			for (int j = 0; j < num5; j++)
			{
				int num6 = binaryReader.ReadInt();
				byte[] array6 = binaryReader.ReadBytes(num6);
				array5[j] = array6;
			}
			return new GameData(array, array2, array3, array5);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002928 File Offset: 0x00000B28
		public byte[] GetData()
		{
			BinaryWriter binaryWriter = new BinaryWriter();
			binaryWriter.WriteInt(this.Header.Length);
			binaryWriter.WriteBytes(this.Header);
			binaryWriter.WriteInt(this.Strings.Length);
			binaryWriter.WriteBytes(this.Strings);
			binaryWriter.WriteInt(this.ObjectData.Length);
			foreach (byte[] array2 in this.ObjectData)
			{
				binaryWriter.WriteInt(array2.Length);
				binaryWriter.WriteBytes(array2);
			}
			binaryWriter.WriteInt(this.ContainerData.Length);
			foreach (byte[] array3 in this.ContainerData)
			{
				binaryWriter.WriteInt(array3.Length);
				binaryWriter.WriteBytes(array3);
			}
			return binaryWriter.GetFinalData();
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000029E8 File Offset: 0x00000BE8
		public static void Write(BinaryWriter writer, GameData gameData)
		{
			Debug.Print("---------------SAVE STATISTICS---------------", 0, Debug.DebugColor.White, 17592186044416UL);
			Debug.Print(string.Format("[GameData.Write] WRITING LITTLE ENDIAN: {0}", BitConverter.IsLittleEndian), 0, Debug.DebugColor.White, 17592186044416UL);
			Debug.Print(string.Format("[GameData.Write] Header Length: {0}", gameData.Header.Length), 0, Debug.DebugColor.White, 17592186044416UL);
			writer.Write(gameData.Header.Length);
			writer.Write(gameData.Header);
			Debug.Print(string.Format("[GameData.Write] ObjectData Length: {0}", gameData.ObjectData.Length), 0, Debug.DebugColor.White, 17592186044416UL);
			Debug.Print(string.Format("[GameData.Write] ObjectData Total Size: {0}", gameData.ObjectData.Sum<byte[]>((byte[] x) => x.Length)), 0, Debug.DebugColor.White, 17592186044416UL);
			writer.Write(gameData.ObjectData.Length);
			foreach (byte[] array2 in gameData.ObjectData)
			{
				writer.Write(array2.Length);
				writer.Write(array2);
			}
			Debug.Print(string.Format("[GameData.Write] ContainerData Length: {0}", gameData.ContainerData.Length), 0, Debug.DebugColor.White, 17592186044416UL);
			Debug.Print(string.Format("[GameData.Write] ContainerData Total Size: {0}", gameData.ContainerData.Sum<byte[]>((byte[] x) => x.Length)), 0, Debug.DebugColor.White, 17592186044416UL);
			writer.Write(gameData.ContainerData.Length);
			foreach (byte[] array3 in gameData.ContainerData)
			{
				writer.Write(array3.Length);
				writer.Write(array3);
			}
			Debug.Print(string.Format("[GameData.Write] Strings Length: {0}", gameData.Strings.Length), 0, Debug.DebugColor.White, 17592186044416UL);
			writer.Write(gameData.Strings.Length);
			writer.Write(gameData.Strings);
			Debug.Print("---------------SAVE STATISTICS---------------", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002C1C File Offset: 0x00000E1C
		public static GameData Read(BinaryReader reader)
		{
			Debug.Print("---------------LOAD STATISTICS---------------", 0, Debug.DebugColor.White, 17592186044416UL);
			int num = reader.ReadInt32();
			Debug.Print(string.Format("[GameData.Read] Header Length: {0}", num), 0, Debug.DebugColor.White, 17592186044416UL);
			byte[] array = reader.ReadBytes(num);
			int num2 = reader.ReadInt32();
			Debug.Print(string.Format("[GameData.Read] Object Length: {0}", num2), 0, Debug.DebugColor.White, 17592186044416UL);
			byte[][] array2 = new byte[num2][];
			for (int i = 0; i < num2; i++)
			{
				int num3 = reader.ReadInt32();
				array2[i] = reader.ReadBytes(num3);
			}
			Debug.Print(string.Format("[GameData.Read] ObjectData Total Size: {0}", array2.Sum<byte[]>((byte[] x) => x.Length)), 0, Debug.DebugColor.White, 17592186044416UL);
			int num4 = reader.ReadInt32();
			Debug.Print(string.Format("[GameData.Read] Container Length: {0}", num4), 0, Debug.DebugColor.White, 17592186044416UL);
			byte[][] array3 = new byte[num4][];
			for (int j = 0; j < num4; j++)
			{
				int num5 = reader.ReadInt32();
				array3[j] = reader.ReadBytes(num5);
			}
			Debug.Print(string.Format("[GameData.Read] ContainerData Total Size: {0}", array3.Sum<byte[]>((byte[] x) => x.Length)), 0, Debug.DebugColor.White, 17592186044416UL);
			int num6 = reader.ReadInt32();
			Debug.Print(string.Format("[GameData.Read] String Length: {0}", num6), 0, Debug.DebugColor.White, 17592186044416UL);
			byte[] array4 = reader.ReadBytes(num6);
			return new GameData(array, array4, array2, array3);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002DE8 File Offset: 0x00000FE8
		public bool IsEqualTo(GameData gameData)
		{
			bool flag = this.CompareByteArrays(this.Header, gameData.Header, "Header");
			bool flag2 = this.CompareByteArrays(this.Strings, gameData.Strings, "Strings");
			bool flag3 = this.CompareByteArrays(this.ObjectData, gameData.ObjectData, "ObjectData");
			bool flag4 = this.CompareByteArrays(this.ContainerData, gameData.ContainerData, "ContainerData");
			return flag && flag2 && flag3 && flag4;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002E5C File Offset: 0x0000105C
		private bool CompareByteArrays(byte[] arr1, byte[] arr2, string name)
		{
			if (arr1.Length != arr2.Length)
			{
				Debug.FailedAssert(name + " failed length comparison.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\GameData.cs", "CompareByteArrays", 212);
				return false;
			}
			for (int i = 0; i < arr1.Length; i++)
			{
				if (arr1[i] != arr2[i])
				{
					Debug.FailedAssert(string.Format("{0} failed byte comparison at index {1}.", name, i), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\GameData.cs", "CompareByteArrays", 220);
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002ED0 File Offset: 0x000010D0
		private bool CompareByteArrays(byte[][] arr1, byte[][] arr2, string name)
		{
			if (arr1.Length != arr2.Length)
			{
				Debug.FailedAssert(name + " failed length comparison.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\GameData.cs", "CompareByteArrays", 231);
				return false;
			}
			for (int i = 0; i < arr1.Length; i++)
			{
				if (!this.CompareByteArrays(arr1[i], arr2[i], name + string.Format(" Index: {0}", i)))
				{
					return false;
				}
			}
			return true;
		}
	}
}
