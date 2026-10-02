using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006E RID: 110
	[NullableContext(1)]
	[Nullable(0)]
	internal static class TypeExtensions
	{
		// Token: 0x060005E7 RID: 1511 RVA: 0x00018FC0 File Offset: 0x000171C0
		public static MethodInfo Method(this Delegate d)
		{
			return d.Method;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00018FC8 File Offset: 0x000171C8
		public static MemberTypes MemberType(this MemberInfo memberInfo)
		{
			return memberInfo.MemberType;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00018FD0 File Offset: 0x000171D0
		public static bool ContainsGenericParameters(this Type type)
		{
			return type.ContainsGenericParameters;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00018FD8 File Offset: 0x000171D8
		public static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00018FE0 File Offset: 0x000171E0
		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00018FE8 File Offset: 0x000171E8
		public static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00018FF0 File Offset: 0x000171F0
		[return: Nullable(2)]
		public static Type BaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00018FF8 File Offset: 0x000171F8
		public static Assembly Assembly(this Type type)
		{
			return type.Assembly;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00019000 File Offset: 0x00017200
		public static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00019008 File Offset: 0x00017208
		public static bool IsClass(this Type type)
		{
			return type.IsClass;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00019010 File Offset: 0x00017210
		public static bool IsSealed(this Type type)
		{
			return type.IsSealed;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00019018 File Offset: 0x00017218
		public static bool IsAbstract(this Type type)
		{
			return type.IsAbstract;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00019020 File Offset: 0x00017220
		public static bool IsVisible(this Type type)
		{
			return type.IsVisible;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00019028 File Offset: 0x00017228
		public static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00019030 File Offset: 0x00017230
		public static bool IsPrimitive(this Type type)
		{
			return type.IsPrimitive;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00019038 File Offset: 0x00017238
		public static bool AssignableToTypeName(this Type type, string fullTypeName, bool searchInterfaces, [Nullable(2)] [NotNullWhen(true)] out Type match)
		{
			Type type2 = type;
			while (type2 != null)
			{
				if (string.Equals(type2.FullName, fullTypeName, StringComparison.Ordinal))
				{
					match = type2;
					return true;
				}
				type2 = type2.BaseType();
			}
			if (searchInterfaces)
			{
				Type[] interfaces = type.GetInterfaces();
				for (int i = 0; i < interfaces.Length; i++)
				{
					if (string.Equals(interfaces[i].Name, fullTypeName, StringComparison.Ordinal))
					{
						match = type;
						return true;
					}
				}
			}
			match = null;
			return false;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x000190A0 File Offset: 0x000172A0
		public static bool AssignableToTypeName(this Type type, string fullTypeName, bool searchInterfaces)
		{
			Type type2;
			return type.AssignableToTypeName(fullTypeName, searchInterfaces, out type2);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x000190B8 File Offset: 0x000172B8
		public static bool ImplementInterface(this Type type, Type interfaceType)
		{
			Type type2 = type;
			while (type2 != null)
			{
				foreach (Type type3 in ((IEnumerable<Type>)type2.GetInterfaces()))
				{
					if (type3 == interfaceType || (type3 != null && type3.ImplementInterface(interfaceType)))
					{
						return true;
					}
				}
				type2 = type2.BaseType();
			}
			return false;
		}
	}
}
