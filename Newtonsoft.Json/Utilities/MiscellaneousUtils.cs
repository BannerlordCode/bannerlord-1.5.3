using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000063 RID: 99
	[NullableContext(1)]
	[Nullable(0)]
	internal static class MiscellaneousUtils
	{
		// Token: 0x06000566 RID: 1382 RVA: 0x00017150 File Offset: 0x00015350
		[NullableContext(2)]
		[Conditional("DEBUG")]
		public static void Assert([DoesNotReturnIf(false)] bool condition, string message = null)
		{
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00017154 File Offset: 0x00015354
		[NullableContext(2)]
		public static bool ValueEquals(object objA, object objB)
		{
			if (objA == objB)
			{
				return true;
			}
			if (objA == null || objB == null)
			{
				return false;
			}
			if (!(objA.GetType() != objB.GetType()))
			{
				return objA.Equals(objB);
			}
			if (ConvertUtils.IsInteger(objA) && ConvertUtils.IsInteger(objB))
			{
				return Convert.ToDecimal(objA, CultureInfo.CurrentCulture).Equals(Convert.ToDecimal(objB, CultureInfo.CurrentCulture));
			}
			return (objA is double || objA is float || objA is decimal) && (objB is double || objB is float || objB is decimal) && MathUtils.ApproxEquals(Convert.ToDouble(objA, CultureInfo.CurrentCulture), Convert.ToDouble(objB, CultureInfo.CurrentCulture));
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00017208 File Offset: 0x00015408
		public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string paramName, object actualValue, string message)
		{
			string text = message + Environment.NewLine + "Actual value was {0}.".FormatWith(CultureInfo.InvariantCulture, actualValue);
			return new ArgumentOutOfRangeException(paramName, text);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00017238 File Offset: 0x00015438
		public static string ToString([Nullable(2)] object value)
		{
			if (value == null)
			{
				return "{null}";
			}
			string text = value as string;
			if (text == null)
			{
				return value.ToString();
			}
			return "\"" + text + "\"";
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00017270 File Offset: 0x00015470
		public static int ByteArrayCompare(byte[] a1, byte[] a2)
		{
			int num = a1.Length.CompareTo(a2.Length);
			if (num != 0)
			{
				return num;
			}
			for (int i = 0; i < a1.Length; i++)
			{
				int num2 = a1[i].CompareTo(a2[i]);
				if (num2 != 0)
				{
					return num2;
				}
			}
			return 0;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x000172B8 File Offset: 0x000154B8
		[return: Nullable(2)]
		public static string GetPrefix(string qualifiedName)
		{
			string text;
			string text2;
			MiscellaneousUtils.GetQualifiedNameParts(qualifiedName, out text, out text2);
			return text;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x000172D0 File Offset: 0x000154D0
		public static string GetLocalName(string qualifiedName)
		{
			string text;
			string text2;
			MiscellaneousUtils.GetQualifiedNameParts(qualifiedName, out text, out text2);
			return text2;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x000172E8 File Offset: 0x000154E8
		public static void GetQualifiedNameParts(string qualifiedName, [Nullable(2)] out string prefix, out string localName)
		{
			int num = StringUtils.IndexOf(qualifiedName, ':');
			if (num == -1 || num == 0 || qualifiedName.Length - 1 == num)
			{
				prefix = null;
				localName = qualifiedName;
				return;
			}
			prefix = qualifiedName.Substring(0, num);
			localName = qualifiedName.Substring(num + 1);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001732C File Offset: 0x0001552C
		internal static RegexOptions GetRegexOptions(string optionsText)
		{
			RegexOptions regexOptions = RegexOptions.None;
			foreach (char c in optionsText)
			{
				if (c <= 'm')
				{
					if (c != 'i')
					{
						if (c == 'm')
						{
							regexOptions |= RegexOptions.Multiline;
						}
					}
					else
					{
						regexOptions |= RegexOptions.IgnoreCase;
					}
				}
				else if (c != 's')
				{
					if (c == 'x')
					{
						regexOptions |= RegexOptions.ExplicitCapture;
					}
				}
				else
				{
					regexOptions |= RegexOptions.Singleline;
				}
			}
			return regexOptions;
		}
	}
}
