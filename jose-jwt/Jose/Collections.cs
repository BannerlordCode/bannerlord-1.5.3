using System;
using System.Collections;
using System.Collections.Generic;

namespace Jose
{
	// Token: 0x0200002C RID: 44
	public class Collections
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x00005990 File Offset: 0x00003B90
		public static string[] Union(string[] src, object other)
		{
			IEnumerable enumerable = other as IEnumerable;
			if (enumerable == null)
			{
				return src;
			}
			ISet<string> set = ((src == null) ? new HashSet<string>() : new HashSet<string>(src));
			foreach (object obj in enumerable)
			{
				set.Add(obj.ToString());
			}
			string[] array = new string[set.Count];
			set.CopyTo(array, 0);
			return array;
		}
	}
}
