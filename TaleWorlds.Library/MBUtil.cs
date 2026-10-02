using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaleWorlds.Library
{
	// Token: 0x02000071 RID: 113
	public static class MBUtil
	{
		// Token: 0x06000417 RID: 1047 RVA: 0x0000E820 File Offset: 0x0000CA20
		public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
			if (!directoryInfo.Exists)
			{
				return;
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				string text = Path.Combine(destDirName, fileInfo.Name);
				fileInfo.CopyTo(text, false);
			}
			if (copySubDirs)
			{
				foreach (DirectoryInfo directoryInfo2 in directories)
				{
					string text2 = Path.Combine(destDirName, directoryInfo2.Name);
					MBUtil.DirectoryCopy(directoryInfo2.FullName, text2, copySubDirs);
				}
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000E8C0 File Offset: 0x0000CAC0
		public static T[] ArrayAdd<T>(T[] tArray, T t)
		{
			List<T> list = tArray.ToList<T>();
			list.Add(t);
			return list.ToArray();
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000E8D4 File Offset: 0x0000CAD4
		public static T[] ArrayRemove<T>(T[] tArray, T t)
		{
			List<T> list = tArray.ToList<T>();
			if (!list.Remove(t))
			{
				return tArray;
			}
			return list.ToArray();
		}
	}
}
