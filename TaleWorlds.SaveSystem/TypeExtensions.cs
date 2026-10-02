using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000024 RID: 36
	internal static class TypeExtensions
	{
		// Token: 0x06000146 RID: 326 RVA: 0x00006BA4 File Offset: 0x00004DA4
		internal static bool IsContainer(this Type type)
		{
			ContainerType containerType;
			return type.IsContainer(out containerType);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006BBC File Offset: 0x00004DBC
		internal static bool IsContainer(this Type type, out ContainerType containerType)
		{
			containerType = ContainerType.None;
			if (type.IsGenericType && !type.IsGenericTypeDefinition)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(Dictionary<, >))
				{
					containerType = ContainerType.Dictionary;
					return true;
				}
				if (genericTypeDefinition == typeof(List<>))
				{
					containerType = ContainerType.List;
					return true;
				}
				if (genericTypeDefinition == typeof(MBList<>))
				{
					containerType = ContainerType.CustomList;
					return true;
				}
				if (genericTypeDefinition == typeof(MBReadOnlyList<>))
				{
					containerType = ContainerType.CustomReadOnlyList;
					return true;
				}
				if (genericTypeDefinition == typeof(Queue<>))
				{
					containerType = ContainerType.Queue;
					return true;
				}
			}
			else if (type.IsArray)
			{
				containerType = ContainerType.Array;
				return true;
			}
			return false;
		}
	}
}
