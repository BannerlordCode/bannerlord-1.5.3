using System;
using System.Collections.Generic;
using System.Threading;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000009 RID: 9
	internal static class BinaryWriterFactory
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002248 File Offset: 0x00000448
		public static BinaryWriter GetBinaryWriter()
		{
			if (BinaryWriterFactory._binaryWriters.Value == null)
			{
				BinaryWriterFactory._binaryWriters.Value = new Stack<BinaryWriter>();
			}
			Stack<BinaryWriter> value = BinaryWriterFactory._binaryWriters.Value;
			BinaryWriter binaryWriter;
			if (value.Count != 0)
			{
				binaryWriter = value.Pop();
			}
			else
			{
				binaryWriter = new BinaryWriter(4096);
			}
			return binaryWriter;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000229C File Offset: 0x0000049C
		public static void ReleaseBinaryWriter(BinaryWriter writer)
		{
			if (BinaryWriterFactory._binaryWriters != null)
			{
				if (BinaryWriterFactory._binaryWriters.Value == null)
				{
					Debug.Print("Release used before Get", 0, Debug.DebugColor.White, 17592186044416UL);
					BinaryWriterFactory._binaryWriters.Value = new Stack<BinaryWriter>();
				}
				writer.Clear();
				BinaryWriterFactory._binaryWriters.Value.Push(writer);
				return;
			}
			Debug.FailedAssert("_binaryWriters != null", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\BinaryWriterFactory.cs", "ReleaseBinaryWriter", 46);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000230E File Offset: 0x0000050E
		public static void Initialize()
		{
			BinaryWriterFactory._binaryWriters = new ThreadLocal<Stack<BinaryWriter>>();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000231A File Offset: 0x0000051A
		public static void Release()
		{
			BinaryWriterFactory._binaryWriters.Dispose();
			BinaryWriterFactory._binaryWriters = null;
		}

		// Token: 0x04000008 RID: 8
		private static ThreadLocal<Stack<BinaryWriter>> _binaryWriters;
	}
}
