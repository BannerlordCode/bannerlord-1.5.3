using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000B5 RID: 181
	[NullableContext(1)]
	[Nullable(0)]
	public static class Extensions
	{
		// Token: 0x06000960 RID: 2400 RVA: 0x00027507 File Offset: 0x00025707
		public static IJEnumerable<JToken> Ancestors<[Nullable(0)] T>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany<T, JToken>((T j) => j.Ancestors()).AsJEnumerable();
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0002753E File Offset: 0x0002573E
		public static IJEnumerable<JToken> AncestorsAndSelf<[Nullable(0)] T>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany<T, JToken>((T j) => j.AncestorsAndSelf()).AsJEnumerable();
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x00027575 File Offset: 0x00025775
		public static IJEnumerable<JToken> Descendants<[Nullable(0)] T>(this IEnumerable<T> source) where T : JContainer
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany<T, JToken>((T j) => j.Descendants()).AsJEnumerable();
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x000275AC File Offset: 0x000257AC
		public static IJEnumerable<JToken> DescendantsAndSelf<[Nullable(0)] T>(this IEnumerable<T> source) where T : JContainer
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany<T, JToken>((T j) => j.DescendantsAndSelf()).AsJEnumerable();
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x000275E3 File Offset: 0x000257E3
		public static IJEnumerable<JProperty> Properties(this IEnumerable<JObject> source)
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany<JObject, JProperty>((JObject d) => d.Properties()).AsJEnumerable<JProperty>();
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0002761A File Offset: 0x0002581A
		public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source, [Nullable(2)] object key)
		{
			return source.Values<JToken, JToken>(key).AsJEnumerable();
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00027628 File Offset: 0x00025828
		public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source)
		{
			return source.Values(null);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00027631 File Offset: 0x00025831
		[return: Nullable(new byte[] { 1, 2 })]
		public static IEnumerable<U> Values<[Nullable(2)] U>(this IEnumerable<JToken> source, object key)
		{
			return source.Values<JToken, U>(key);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0002763A File Offset: 0x0002583A
		[return: Nullable(new byte[] { 1, 2 })]
		public static IEnumerable<U> Values<[Nullable(2)] U>(this IEnumerable<JToken> source)
		{
			return source.Values<JToken, U>(null);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00027643 File Offset: 0x00025843
		[NullableContext(2)]
		public static U Value<U>([Nullable(1)] this IEnumerable<JToken> value)
		{
			return value.Value<JToken, U>();
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0002764B File Offset: 0x0002584B
		[return: Nullable(2)]
		public static U Value<[Nullable(0)] T, [Nullable(2)] U>(this IEnumerable<T> value) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JToken jtoken = value as JToken;
			if (jtoken == null)
			{
				throw new ArgumentException("Source value must be a JToken.");
			}
			return jtoken.Convert<JToken, U>();
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00027671 File Offset: 0x00025871
		[return: Nullable(new byte[] { 1, 2 })]
		internal static IEnumerable<U> Values<[Nullable(0)] T, [Nullable(2)] U>(this IEnumerable<T> source, [Nullable(2)] object key) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			if (key == null)
			{
				foreach (T t in source)
				{
					JValue jvalue = t as JValue;
					if (jvalue != null)
					{
						yield return jvalue.Convert<JValue, U>();
					}
					else
					{
						foreach (JToken jtoken in t.Children())
						{
							yield return jtoken.Convert<JToken, U>();
						}
						IEnumerator<JToken> enumerator2 = null;
					}
				}
				IEnumerator<T> enumerator = null;
			}
			else
			{
				foreach (T t2 in source)
				{
					JToken jtoken2 = t2[key];
					if (jtoken2 != null)
					{
						yield return jtoken2.Convert<JToken, U>();
					}
				}
				IEnumerator<T> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x00027688 File Offset: 0x00025888
		public static IJEnumerable<JToken> Children<[Nullable(0)] T>(this IEnumerable<T> source) where T : JToken
		{
			return source.Children<T, JToken>().AsJEnumerable();
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00027695 File Offset: 0x00025895
		[return: Nullable(new byte[] { 1, 2 })]
		public static IEnumerable<U> Children<[Nullable(0)] T, [Nullable(2)] U>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany<T, JToken>((T c) => c.Children()).Convert<JToken, U>();
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x000276CC File Offset: 0x000258CC
		[return: Nullable(new byte[] { 1, 2 })]
		internal static IEnumerable<U> Convert<[Nullable(0)] T, [Nullable(2)] U>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			foreach (T t in source)
			{
				yield return t.Convert<JToken, U>();
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x000276DC File Offset: 0x000258DC
		[NullableContext(2)]
		internal static U Convert<[Nullable(0)] T, U>([Nullable(1)] this T token) where T : JToken
		{
			if (token == null)
			{
				return default(U);
			}
			if (token is U)
			{
				U u = token as U;
				if (typeof(U) != typeof(IComparable) && typeof(U) != typeof(IFormattable))
				{
					return u;
				}
			}
			JValue jvalue = token as JValue;
			if (jvalue == null)
			{
				throw new InvalidCastException("Cannot cast {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, token.GetType(), typeof(T)));
			}
			object value = jvalue.Value;
			if (value is U)
			{
				return (U)((object)value);
			}
			Type type = typeof(U);
			if (ReflectionUtils.IsNullableType(type))
			{
				if (jvalue.Value == null)
				{
					return default(U);
				}
				type = Nullable.GetUnderlyingType(type);
			}
			return (U)((object)global::System.Convert.ChangeType(jvalue.Value, type, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x000277EC File Offset: 0x000259EC
		public static IJEnumerable<JToken> AsJEnumerable(this IEnumerable<JToken> source)
		{
			return source.AsJEnumerable<JToken>();
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x000277F4 File Offset: 0x000259F4
		public static IJEnumerable<T> AsJEnumerable<[Nullable(0)] T>(this IEnumerable<T> source) where T : JToken
		{
			if (source == null)
			{
				return null;
			}
			IJEnumerable<T> ijenumerable = source as IJEnumerable<T>;
			if (ijenumerable != null)
			{
				return ijenumerable;
			}
			return new JEnumerable<T>(source);
		}
	}
}
