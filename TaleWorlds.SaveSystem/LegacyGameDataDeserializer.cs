using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000010 RID: 16
	public static class LegacyGameDataDeserializer
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00003038 File Offset: 0x00001238
		public static GameData Deserialize(Stream stream)
		{
			Dictionary<int, object> dictionary = new Dictionary<int, object>();
			GameData gameData2;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress, true))
				{
					deflateStream.CopyTo(memoryStream);
				}
				memoryStream.Position = 0L;
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					if (binaryReader.ReadByte() != 0)
					{
						throw new InvalidDataException("Expected SerializationHeaderRecord.");
					}
					binaryReader.BaseStream.Position += 16L;
					if (binaryReader.ReadByte() != 12)
					{
						throw new InvalidDataException("Expected BinaryLibrary record.");
					}
					binaryReader.ReadInt32();
					binaryReader.ReadString();
					if (binaryReader.ReadByte() != 5)
					{
						throw new InvalidDataException("Expected GameData ClassWithMembersAndTypes record.");
					}
					binaryReader.ReadInt32();
					binaryReader.ReadString();
					int num = binaryReader.ReadInt32();
					for (int i = 0; i < num; i++)
					{
						binaryReader.ReadString();
					}
					binaryReader.ReadBytes(4);
					binaryReader.ReadBytes(2);
					binaryReader.ReadString();
					binaryReader.ReadString();
					binaryReader.ReadInt32();
					if (binaryReader.ReadByte() != 9)
					{
						throw new InvalidDataException("Expected MemberReference.");
					}
					int num2 = binaryReader.ReadInt32();
					if (binaryReader.ReadByte() != 9)
					{
						throw new InvalidDataException("Expected MemberReference.");
					}
					int num3 = binaryReader.ReadInt32();
					if (binaryReader.ReadByte() != 9)
					{
						throw new InvalidDataException("Expected MemberReference.");
					}
					int num4 = binaryReader.ReadInt32();
					if (binaryReader.ReadByte() != 9)
					{
						throw new InvalidDataException("Expected MemberReference.");
					}
					int num5 = binaryReader.ReadInt32();
					while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
					{
						byte b = binaryReader.ReadByte();
						if (b == 11)
						{
							break;
						}
						int num6 = binaryReader.ReadInt32();
						if (b == 15)
						{
							int num7 = binaryReader.ReadInt32();
							binaryReader.ReadByte();
							dictionary[num6] = binaryReader.ReadBytes(num7);
						}
						else if (b == 7)
						{
							binaryReader.ReadByte();
							binaryReader.ReadInt32();
							int num8 = binaryReader.ReadInt32();
							if (binaryReader.ReadByte() == 7)
							{
								binaryReader.ReadByte();
							}
							int[] array = new int[num8];
							for (int j = 0; j < num8; j++)
							{
								if (binaryReader.ReadByte() != 9)
								{
									throw new InvalidDataException("Expected MemberReference for jagged array element.");
								}
								array[j] = binaryReader.ReadInt32();
							}
							dictionary[num6] = array;
						}
					}
					GameData gameData = new GameData
					{
						Header = (byte[])dictionary[num2],
						Strings = (byte[])dictionary[num3]
					};
					int[] array2 = (int[])dictionary[num4];
					gameData.ObjectData = new byte[array2.Length][];
					for (int k = 0; k < array2.Length; k++)
					{
						gameData.ObjectData[k] = (byte[])dictionary[array2[k]];
					}
					int[] array3 = (int[])dictionary[num5];
					gameData.ContainerData = new byte[array3.Length][];
					for (int l = 0; l < array3.Length; l++)
					{
						gameData.ContainerData[l] = (byte[])dictionary[array3[l]];
					}
					gameData2 = gameData;
				}
			}
			return gameData2;
		}
	}
}
