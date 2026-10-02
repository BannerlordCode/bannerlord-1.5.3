using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq.JsonPath;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C4 RID: 196
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JToken : IJEnumerable<JToken>, IEnumerable<JToken>, IEnumerable, IJsonLineInfo, ICloneable, IDynamicMetaObjectProvider
	{
		// Token: 0x06000ABC RID: 2748 RVA: 0x0002A84A File Offset: 0x00028A4A
		public virtual Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0002A854 File Offset: 0x00028A54
		public Task WriteToAsync(JsonWriter writer, params JsonConverter[] converters)
		{
			return this.WriteToAsync(writer, default(CancellationToken), converters);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0002A872 File Offset: 0x00028A72
		public static Task<JToken> ReadFromAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return JToken.ReadFromAsync(reader, null, cancellationToken);
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x0002A87C File Offset: 0x00028A7C
		public static async Task<JToken> ReadFromAsync(JsonReader reader, [Nullable(2)] JsonLoadSettings settings, CancellationToken cancellationToken = default(CancellationToken))
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (reader.TokenType == JsonToken.None)
			{
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = ((settings != null && settings.CommentHandling == CommentHandling.Ignore) ? reader.ReadAndMoveToContentAsync(cancellationToken) : reader.ReadAsync(cancellationToken)).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader.");
				}
			}
			IJsonLineInfo jsonLineInfo = reader as IJsonLineInfo;
			switch (reader.TokenType)
			{
			case JsonToken.StartObject:
				return await JObject.LoadAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			case JsonToken.StartArray:
				return await JArray.LoadAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			case JsonToken.StartConstructor:
				return await JConstructor.LoadAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			case JsonToken.PropertyName:
				return await JProperty.LoadAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			case JsonToken.Comment:
			{
				object value = reader.Value;
				JValue jvalue = JValue.CreateComment((value != null) ? value.ToString() : null);
				jvalue.SetLineInfo(jsonLineInfo, settings);
				return jvalue;
			}
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Date:
			case JsonToken.Bytes:
			{
				JValue jvalue2 = new JValue(reader.Value);
				jvalue2.SetLineInfo(jsonLineInfo, settings);
				return jvalue2;
			}
			case JsonToken.Null:
			{
				JValue jvalue3 = JValue.CreateNull();
				jvalue3.SetLineInfo(jsonLineInfo, settings);
				return jvalue3;
			}
			case JsonToken.Undefined:
			{
				JValue jvalue4 = JValue.CreateUndefined();
				jvalue4.SetLineInfo(jsonLineInfo, settings);
				return jvalue4;
			}
			}
			throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader. Unexpected token: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0002A8CF File Offset: 0x00028ACF
		public static Task<JToken> LoadAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return JToken.LoadAsync(reader, null, cancellationToken);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0002A8D9 File Offset: 0x00028AD9
		public static Task<JToken> LoadAsync(JsonReader reader, [Nullable(2)] JsonLoadSettings settings, CancellationToken cancellationToken = default(CancellationToken))
		{
			return JToken.ReadFromAsync(reader, settings, cancellationToken);
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x0002A8E3 File Offset: 0x00028AE3
		public static JTokenEqualityComparer EqualityComparer
		{
			get
			{
				if (JToken._equalityComparer == null)
				{
					JToken._equalityComparer = new JTokenEqualityComparer();
				}
				return JToken._equalityComparer;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x0002A8FB File Offset: 0x00028AFB
		// (set) Token: 0x06000AC4 RID: 2756 RVA: 0x0002A903 File Offset: 0x00028B03
		[Nullable(2)]
		public JContainer Parent
		{
			[NullableContext(2)]
			[DebuggerStepThrough]
			get
			{
				return this._parent;
			}
			[NullableContext(2)]
			internal set
			{
				this._parent = value;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x0002A90C File Offset: 0x00028B0C
		public JToken Root
		{
			get
			{
				JContainer jcontainer = this.Parent;
				if (jcontainer == null)
				{
					return this;
				}
				while (jcontainer.Parent != null)
				{
					jcontainer = jcontainer.Parent;
				}
				return jcontainer;
			}
		}

		// Token: 0x06000AC6 RID: 2758
		internal abstract JToken CloneToken([Nullable(2)] JsonCloneSettings settings);

		// Token: 0x06000AC7 RID: 2759
		internal abstract bool DeepEquals(JToken node);

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000AC8 RID: 2760
		public abstract JTokenType Type { get; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000AC9 RID: 2761
		public abstract bool HasValues { get; }

		// Token: 0x06000ACA RID: 2762 RVA: 0x0002A935 File Offset: 0x00028B35
		[NullableContext(2)]
		public static bool DeepEquals(JToken t1, JToken t2)
		{
			return t1 == t2 || (t1 != null && t2 != null && t1.DeepEquals(t2));
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x0002A94C File Offset: 0x00028B4C
		// (set) Token: 0x06000ACC RID: 2764 RVA: 0x0002A954 File Offset: 0x00028B54
		[Nullable(2)]
		public JToken Next
		{
			[NullableContext(2)]
			get
			{
				return this._next;
			}
			[NullableContext(2)]
			internal set
			{
				this._next = value;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x0002A95D File Offset: 0x00028B5D
		// (set) Token: 0x06000ACE RID: 2766 RVA: 0x0002A965 File Offset: 0x00028B65
		[Nullable(2)]
		public JToken Previous
		{
			[NullableContext(2)]
			get
			{
				return this._previous;
			}
			[NullableContext(2)]
			internal set
			{
				this._previous = value;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x0002A970 File Offset: 0x00028B70
		public string Path
		{
			get
			{
				if (this.Parent == null)
				{
					return string.Empty;
				}
				List<JsonPosition> list = new List<JsonPosition>();
				JToken jtoken = null;
				for (JToken jtoken2 = this; jtoken2 != null; jtoken2 = jtoken2.Parent)
				{
					JTokenType type = jtoken2.Type;
					if (type - JTokenType.Array > 1)
					{
						if (type == JTokenType.Property)
						{
							JProperty jproperty = (JProperty)jtoken2;
							List<JsonPosition> list2 = list;
							JsonPosition jsonPosition = new JsonPosition(JsonContainerType.Object)
							{
								PropertyName = jproperty.Name
							};
							list2.Add(jsonPosition);
						}
					}
					else if (jtoken != null)
					{
						int num = ((IList<JToken>)jtoken2).IndexOf(jtoken);
						List<JsonPosition> list3 = list;
						JsonPosition jsonPosition = new JsonPosition(JsonContainerType.Array)
						{
							Position = num
						};
						list3.Add(jsonPosition);
					}
					jtoken = jtoken2;
				}
				list.FastReverse<JsonPosition>();
				return JsonPosition.BuildPath(list, null);
			}
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0002AA1D File Offset: 0x00028C1D
		internal JToken()
		{
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0002AA28 File Offset: 0x00028C28
		[NullableContext(2)]
		public void AddAfterSelf(object content)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			int num = this._parent.IndexOfItem(this);
			this._parent.TryAddInternal(num + 1, content, false, true);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0002AA68 File Offset: 0x00028C68
		[NullableContext(2)]
		public void AddBeforeSelf(object content)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			int num = this._parent.IndexOfItem(this);
			this._parent.TryAddInternal(num, content, false, true);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0002AAA5 File Offset: 0x00028CA5
		public IEnumerable<JToken> Ancestors()
		{
			return this.GetAncestors(false);
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0002AAAE File Offset: 0x00028CAE
		public IEnumerable<JToken> AncestorsAndSelf()
		{
			return this.GetAncestors(true);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0002AAB7 File Offset: 0x00028CB7
		internal IEnumerable<JToken> GetAncestors(bool self)
		{
			JToken current;
			for (current = (self ? this : this.Parent); current != null; current = current.Parent)
			{
				yield return current;
			}
			current = null;
			yield break;
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0002AACE File Offset: 0x00028CCE
		public IEnumerable<JToken> AfterSelf()
		{
			if (this.Parent == null)
			{
				yield break;
			}
			JToken o;
			for (o = this.Next; o != null; o = o.Next)
			{
				yield return o;
			}
			o = null;
			yield break;
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0002AADE File Offset: 0x00028CDE
		public IEnumerable<JToken> BeforeSelf()
		{
			if (this.Parent == null)
			{
				yield break;
			}
			JToken o = this.Parent.First;
			while (o != this && o != null)
			{
				yield return o;
				o = o.Next;
			}
			o = null;
			yield break;
		}

		// Token: 0x170001FC RID: 508
		[Nullable(2)]
		public virtual JToken this[object key]
		{
			[return: Nullable(2)]
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
			[param: Nullable(2)]
			set
			{
				throw new InvalidOperationException("Cannot set child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0002AB28 File Offset: 0x00028D28
		[NullableContext(2)]
		public virtual T Value<T>([Nullable(1)] object key)
		{
			JToken jtoken = this[key];
			if (jtoken != null)
			{
				return jtoken.Convert<JToken, T>();
			}
			return default(T);
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x0002AB50 File Offset: 0x00028D50
		[Nullable(2)]
		public virtual JToken First
		{
			[NullableContext(2)]
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x0002AB6C File Offset: 0x00028D6C
		[Nullable(2)]
		public virtual JToken Last
		{
			[NullableContext(2)]
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0002AB88 File Offset: 0x00028D88
		[return: Nullable(new byte[] { 0, 1 })]
		public virtual JEnumerable<JToken> Children()
		{
			return JEnumerable<JToken>.Empty;
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0002AB8F File Offset: 0x00028D8F
		[NullableContext(0)]
		[return: Nullable(new byte[] { 0, 1 })]
		public JEnumerable<T> Children<T>() where T : JToken
		{
			return new JEnumerable<T>(this.Children().OfType<T>());
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002ABA6 File Offset: 0x00028DA6
		[NullableContext(2)]
		[return: Nullable(new byte[] { 1, 2 })]
		public virtual IEnumerable<T> Values<T>()
		{
			throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0002ABC2 File Offset: 0x00028DC2
		public void Remove()
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			this._parent.RemoveItem(this);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0002ABE4 File Offset: 0x00028DE4
		public void Replace(JToken value)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			this._parent.ReplaceItem(this, value);
		}

		// Token: 0x06000AE2 RID: 2786
		public abstract void WriteTo(JsonWriter writer, params JsonConverter[] converters);

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0002AC06 File Offset: 0x00028E06
		public override string ToString()
		{
			return this.ToString(Formatting.Indented, Array.Empty<JsonConverter>());
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0002AC14 File Offset: 0x00028E14
		public string ToString(Formatting formatting, params JsonConverter[] converters)
		{
			string text;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				this.WriteTo(new JsonTextWriter(stringWriter)
				{
					Formatting = formatting
				}, converters);
				text = stringWriter.ToString();
			}
			return text;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0002AC68 File Offset: 0x00028E68
		[return: Nullable(2)]
		private static JValue EnsureValue(JToken value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			JProperty jproperty = value as JProperty;
			if (jproperty != null)
			{
				value = jproperty.Value;
			}
			return value as JValue;
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0002AC9C File Offset: 0x00028E9C
		private static string GetType(JToken token)
		{
			ValidationUtils.ArgumentNotNull(token, "token");
			JProperty jproperty = token as JProperty;
			if (jproperty != null)
			{
				token = jproperty.Value;
			}
			return token.Type.ToString();
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0002ACDA File Offset: 0x00028EDA
		private static bool ValidateToken(JToken o, JTokenType[] validTypes, bool nullable)
		{
			return Array.IndexOf<JTokenType>(validTypes, o.Type) != -1 || (nullable && (o.Type == JTokenType.Null || o.Type == JTokenType.Undefined));
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0002AD08 File Offset: 0x00028F08
		public static explicit operator bool(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BooleanTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return Convert.ToBoolean((int)bigInteger);
			}
			return Convert.ToBoolean(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0002AD7C File Offset: 0x00028F7C
		public static explicit operator DateTimeOffset(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.DateTimeTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is DateTimeOffset)
			{
				return (DateTimeOffset)value2;
			}
			string text = jvalue.Value as string;
			if (text != null)
			{
				return DateTimeOffset.Parse(text, CultureInfo.InvariantCulture);
			}
			return new DateTimeOffset(Convert.ToDateTime(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0002AE04 File Offset: 0x00029004
		[NullableContext(2)]
		public static explicit operator bool?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BooleanTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return new bool?(Convert.ToBoolean((int)bigInteger));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new bool?(Convert.ToBoolean(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0002AEA0 File Offset: 0x000290A0
		public static explicit operator long(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return (long)bigInteger;
			}
			return Convert.ToInt64(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x0002AF10 File Offset: 0x00029110
		[NullableContext(2)]
		public static explicit operator DateTime?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.DateTimeTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is DateTimeOffset)
			{
				return new DateTime?(((DateTimeOffset)value2).DateTime);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new DateTime?(Convert.ToDateTime(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0002AFA8 File Offset: 0x000291A8
		[NullableContext(2)]
		public static explicit operator DateTimeOffset?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.DateTimeTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			object value2 = jvalue.Value;
			if (value2 is DateTimeOffset)
			{
				DateTimeOffset dateTimeOffset = (DateTimeOffset)value2;
				return new DateTimeOffset?(dateTimeOffset);
			}
			string text = jvalue.Value as string;
			if (text != null)
			{
				return new DateTimeOffset?(DateTimeOffset.Parse(text, CultureInfo.InvariantCulture));
			}
			return new DateTimeOffset?(new DateTimeOffset(Convert.ToDateTime(jvalue.Value, CultureInfo.InvariantCulture)));
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0002B064 File Offset: 0x00029264
		[NullableContext(2)]
		public static explicit operator decimal?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return new decimal?((decimal)bigInteger);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new decimal?(Convert.ToDecimal(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0002B0FC File Offset: 0x000292FC
		[NullableContext(2)]
		public static explicit operator double?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return new double?((double)bigInteger);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new double?(Convert.ToDouble(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0002B194 File Offset: 0x00029394
		[NullableContext(2)]
		public static explicit operator char?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.CharTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Char.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return new char?((char)(ushort)bigInteger);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new char?(Convert.ToChar(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0002B22C File Offset: 0x0002942C
		public static explicit operator int(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return (int)bigInteger;
			}
			return Convert.ToInt32(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0002B29C File Offset: 0x0002949C
		public static explicit operator short(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Int16.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return (short)bigInteger;
			}
			return Convert.ToInt16(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0002B30C File Offset: 0x0002950C
		[CLSCompliant(false)]
		public static explicit operator ushort(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return (ushort)bigInteger;
			}
			return Convert.ToUInt16(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0002B37C File Offset: 0x0002957C
		[CLSCompliant(false)]
		public static explicit operator char(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.CharTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Char.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return (char)(ushort)bigInteger;
			}
			return Convert.ToChar(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0002B3EC File Offset: 0x000295EC
		public static explicit operator byte(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Byte.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return (byte)bigInteger;
			}
			return Convert.ToByte(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0002B45C File Offset: 0x0002965C
		[CLSCompliant(false)]
		public static explicit operator sbyte(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to SByte.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return (sbyte)bigInteger;
			}
			return Convert.ToSByte(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0002B4CC File Offset: 0x000296CC
		[NullableContext(2)]
		public static explicit operator int?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return new int?((int)bigInteger);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new int?(Convert.ToInt32(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0002B564 File Offset: 0x00029764
		[NullableContext(2)]
		public static explicit operator short?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Int16.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return new short?((short)bigInteger);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new short?(Convert.ToInt16(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0002B5FC File Offset: 0x000297FC
		[NullableContext(2)]
		[CLSCompliant(false)]
		public static explicit operator ushort?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return new ushort?((ushort)bigInteger);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new ushort?(Convert.ToUInt16(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0002B694 File Offset: 0x00029894
		[NullableContext(2)]
		public static explicit operator byte?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Byte.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return new byte?((byte)bigInteger);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new byte?(Convert.ToByte(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0002B72C File Offset: 0x0002992C
		[NullableContext(2)]
		[CLSCompliant(false)]
		public static explicit operator sbyte?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to SByte.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return new sbyte?((sbyte)bigInteger);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new sbyte?(Convert.ToSByte(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0002B7C4 File Offset: 0x000299C4
		public static explicit operator DateTime(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.DateTimeTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is DateTimeOffset)
			{
				return ((DateTimeOffset)value2).DateTime;
			}
			return Convert.ToDateTime(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0002B834 File Offset: 0x00029A34
		[NullableContext(2)]
		public static explicit operator long?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return new long?((long)bigInteger);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new long?(Convert.ToInt64(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0002B8CC File Offset: 0x00029ACC
		[NullableContext(2)]
		public static explicit operator float?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return new float?((float)bigInteger);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new float?(Convert.ToSingle(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0002B964 File Offset: 0x00029B64
		public static explicit operator decimal(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return (decimal)bigInteger;
			}
			return Convert.ToDecimal(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0002B9D4 File Offset: 0x00029BD4
		[NullableContext(2)]
		[CLSCompliant(false)]
		public static explicit operator uint?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return new uint?((uint)bigInteger);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new uint?(Convert.ToUInt32(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0002BA6C File Offset: 0x00029C6C
		[NullableContext(2)]
		[CLSCompliant(false)]
		public static explicit operator ulong?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return new ulong?((ulong)bigInteger);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new ulong?(Convert.ToUInt64(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0002BB04 File Offset: 0x00029D04
		public static explicit operator double(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return (double)bigInteger;
			}
			return Convert.ToDouble(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0002BB74 File Offset: 0x00029D74
		public static explicit operator float(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return (float)bigInteger;
			}
			return Convert.ToSingle(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0002BBE4 File Offset: 0x00029DE4
		[NullableContext(2)]
		public static explicit operator string(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.StringTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to String.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			byte[] array = jvalue.Value as byte[];
			if (array != null)
			{
				return Convert.ToBase64String(array);
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				return ((BigInteger)value2).ToString(CultureInfo.InvariantCulture);
			}
			return Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0002BC7C File Offset: 0x00029E7C
		[CLSCompliant(false)]
		public static explicit operator uint(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return (uint)bigInteger;
			}
			return Convert.ToUInt32(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0002BCEC File Offset: 0x00029EEC
		[CLSCompliant(false)]
		public static explicit operator ulong(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value2;
				return (ulong)bigInteger;
			}
			return Convert.ToUInt64(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0002BD5C File Offset: 0x00029F5C
		[NullableContext(2)]
		public static explicit operator byte[](JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BytesTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to byte array.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is string)
			{
				return Convert.FromBase64String(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
			}
			object value2 = jvalue.Value;
			if (value2 is BigInteger)
			{
				return ((BigInteger)value2).ToByteArray();
			}
			byte[] array = jvalue.Value as byte[];
			if (array != null)
			{
				return array;
			}
			throw new ArgumentException("Can not convert {0} to byte array.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0002BE10 File Offset: 0x0002A010
		public static explicit operator Guid(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.GuidTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Guid.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			byte[] array = jvalue.Value as byte[];
			if (array != null)
			{
				return new Guid(array);
			}
			object value2 = jvalue.Value;
			if (value2 is Guid)
			{
				return (Guid)value2;
			}
			return new Guid(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0002BE98 File Offset: 0x0002A098
		[NullableContext(2)]
		public static explicit operator Guid?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.GuidTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Guid.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			byte[] array = jvalue.Value as byte[];
			if (array != null)
			{
				return new Guid?(new Guid(array));
			}
			object value2 = jvalue.Value;
			Guid guid2;
			if (value2 is Guid)
			{
				Guid guid = (Guid)value2;
				guid2 = guid;
			}
			else
			{
				guid2 = new Guid(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
			}
			return new Guid?(guid2);
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0002BF4C File Offset: 0x0002A14C
		public static explicit operator TimeSpan(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.TimeSpanTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to TimeSpan.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is TimeSpan)
			{
				return (TimeSpan)value2;
			}
			return ConvertUtils.ParseTimeSpan(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0002BFBC File Offset: 0x0002A1BC
		[NullableContext(2)]
		public static explicit operator TimeSpan?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.TimeSpanTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to TimeSpan.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			object value2 = jvalue.Value;
			TimeSpan timeSpan2;
			if (value2 is TimeSpan)
			{
				TimeSpan timeSpan = (TimeSpan)value2;
				timeSpan2 = timeSpan;
			}
			else
			{
				timeSpan2 = ConvertUtils.ParseTimeSpan(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
			}
			return new TimeSpan?(timeSpan2);
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0002C050 File Offset: 0x0002A250
		[NullableContext(2)]
		public static explicit operator Uri(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.UriTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Uri.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			Uri uri = jvalue.Value as Uri;
			if (uri == null)
			{
				return new Uri(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
			}
			return uri;
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0002C0C8 File Offset: 0x0002A2C8
		private static BigInteger ToBigInteger(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BigIntegerTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			return ConvertUtils.ToBigInteger(jvalue.Value);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0002C114 File Offset: 0x0002A314
		private static BigInteger? ToBigIntegerNullable(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BigIntegerTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new BigInteger?(ConvertUtils.ToBigInteger(jvalue.Value));
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0002C176 File Offset: 0x0002A376
		public static implicit operator JToken(bool value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0002C17E File Offset: 0x0002A37E
		public static implicit operator JToken(DateTimeOffset value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0002C186 File Offset: 0x0002A386
		public static implicit operator JToken(byte value)
		{
			return new JValue((long)((ulong)value));
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0002C18F File Offset: 0x0002A38F
		public static implicit operator JToken(byte? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0002C19C File Offset: 0x0002A39C
		[CLSCompliant(false)]
		public static implicit operator JToken(sbyte value)
		{
			return new JValue((long)value);
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0002C1A5 File Offset: 0x0002A3A5
		[CLSCompliant(false)]
		public static implicit operator JToken(sbyte? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0002C1B2 File Offset: 0x0002A3B2
		public static implicit operator JToken(bool? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0002C1BF File Offset: 0x0002A3BF
		public static implicit operator JToken(long value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0002C1C7 File Offset: 0x0002A3C7
		public static implicit operator JToken(DateTime? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0002C1D4 File Offset: 0x0002A3D4
		public static implicit operator JToken(DateTimeOffset? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0002C1E1 File Offset: 0x0002A3E1
		public static implicit operator JToken(decimal? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0002C1EE File Offset: 0x0002A3EE
		public static implicit operator JToken(double? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0002C1FB File Offset: 0x0002A3FB
		[CLSCompliant(false)]
		public static implicit operator JToken(short value)
		{
			return new JValue((long)value);
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0002C204 File Offset: 0x0002A404
		[CLSCompliant(false)]
		public static implicit operator JToken(ushort value)
		{
			return new JValue((long)((ulong)value));
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0002C20D File Offset: 0x0002A40D
		public static implicit operator JToken(int value)
		{
			return new JValue((long)value);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0002C216 File Offset: 0x0002A416
		public static implicit operator JToken(int? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0002C223 File Offset: 0x0002A423
		public static implicit operator JToken(DateTime value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0002C22B File Offset: 0x0002A42B
		public static implicit operator JToken(long? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0002C238 File Offset: 0x0002A438
		public static implicit operator JToken(float? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0002C245 File Offset: 0x0002A445
		public static implicit operator JToken(decimal value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0002C24D File Offset: 0x0002A44D
		[CLSCompliant(false)]
		public static implicit operator JToken(short? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0002C25A File Offset: 0x0002A45A
		[CLSCompliant(false)]
		public static implicit operator JToken(ushort? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0002C267 File Offset: 0x0002A467
		[CLSCompliant(false)]
		public static implicit operator JToken(uint? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0002C274 File Offset: 0x0002A474
		[CLSCompliant(false)]
		public static implicit operator JToken(ulong? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0002C281 File Offset: 0x0002A481
		public static implicit operator JToken(double value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0002C289 File Offset: 0x0002A489
		public static implicit operator JToken(float value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0002C291 File Offset: 0x0002A491
		public static implicit operator JToken([Nullable(2)] string value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0002C299 File Offset: 0x0002A499
		[CLSCompliant(false)]
		public static implicit operator JToken(uint value)
		{
			return new JValue((long)((ulong)value));
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0002C2A2 File Offset: 0x0002A4A2
		[CLSCompliant(false)]
		public static implicit operator JToken(ulong value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0002C2AA File Offset: 0x0002A4AA
		public static implicit operator JToken(byte[] value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0002C2B2 File Offset: 0x0002A4B2
		public static implicit operator JToken([Nullable(2)] Uri value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0002C2BA File Offset: 0x0002A4BA
		public static implicit operator JToken(TimeSpan value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0002C2C2 File Offset: 0x0002A4C2
		public static implicit operator JToken(TimeSpan? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0002C2CF File Offset: 0x0002A4CF
		public static implicit operator JToken(Guid value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0002C2D7 File Offset: 0x0002A4D7
		public static implicit operator JToken(Guid? value)
		{
			return new JValue(value);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0002C2E4 File Offset: 0x0002A4E4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<JToken>)this).GetEnumerator();
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0002C2EC File Offset: 0x0002A4EC
		IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator()
		{
			return this.Children().GetEnumerator();
		}

		// Token: 0x06000B34 RID: 2868
		internal abstract int GetDeepHashCode();

		// Token: 0x170001FF RID: 511
		IJEnumerable<JToken> IJEnumerable<JToken>.this[object key]
		{
			get
			{
				return this[key];
			}
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0002C310 File Offset: 0x0002A510
		public JsonReader CreateReader()
		{
			return new JTokenReader(this);
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0002C318 File Offset: 0x0002A518
		internal static JToken FromObjectInternal(object o, JsonSerializer jsonSerializer)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			ValidationUtils.ArgumentNotNull(jsonSerializer, "jsonSerializer");
			JToken token;
			using (JTokenWriter jtokenWriter = new JTokenWriter())
			{
				jsonSerializer.Serialize(jtokenWriter, o);
				token = jtokenWriter.Token;
			}
			return token;
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0002C370 File Offset: 0x0002A570
		public static JToken FromObject(object o)
		{
			return JToken.FromObjectInternal(o, JsonSerializer.CreateDefault());
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0002C37D File Offset: 0x0002A57D
		public static JToken FromObject(object o, JsonSerializer jsonSerializer)
		{
			return JToken.FromObjectInternal(o, jsonSerializer);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0002C386 File Offset: 0x0002A586
		[NullableContext(2)]
		public T ToObject<T>()
		{
			return (T)((object)this.ToObject(typeof(T)));
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0002C3A0 File Offset: 0x0002A5A0
		[return: Nullable(2)]
		public object ToObject(Type objectType)
		{
			if (JsonConvert.DefaultSettings == null)
			{
				bool flag;
				PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(objectType, out flag);
				if (flag)
				{
					if (this.Type == JTokenType.String)
					{
						try
						{
							return this.ToObject(objectType, JsonSerializer.CreateDefault());
						}
						catch (Exception ex)
						{
							Type type = (objectType.IsEnum() ? objectType : Nullable.GetUnderlyingType(objectType));
							throw new ArgumentException("Could not convert '{0}' to {1}.".FormatWith(CultureInfo.InvariantCulture, (string)this, type.Name), ex);
						}
					}
					if (this.Type == JTokenType.Integer)
					{
						return Enum.ToObject(objectType.IsEnum() ? objectType : Nullable.GetUnderlyingType(objectType), ((JValue)this).Value);
					}
				}
				switch (typeCode)
				{
				case PrimitiveTypeCode.Char:
					return (char)this;
				case PrimitiveTypeCode.CharNullable:
					return (char?)this;
				case PrimitiveTypeCode.Boolean:
					return (bool)this;
				case PrimitiveTypeCode.BooleanNullable:
					return (bool?)this;
				case PrimitiveTypeCode.SByte:
					return (sbyte)this;
				case PrimitiveTypeCode.SByteNullable:
					return (sbyte?)this;
				case PrimitiveTypeCode.Int16:
					return (short)this;
				case PrimitiveTypeCode.Int16Nullable:
					return (short?)this;
				case PrimitiveTypeCode.UInt16:
					return (ushort)this;
				case PrimitiveTypeCode.UInt16Nullable:
					return (ushort?)this;
				case PrimitiveTypeCode.Int32:
					return (int)this;
				case PrimitiveTypeCode.Int32Nullable:
					return (int?)this;
				case PrimitiveTypeCode.Byte:
					return (byte)this;
				case PrimitiveTypeCode.ByteNullable:
					return (byte?)this;
				case PrimitiveTypeCode.UInt32:
					return (uint)this;
				case PrimitiveTypeCode.UInt32Nullable:
					return (uint?)this;
				case PrimitiveTypeCode.Int64:
					return (long)this;
				case PrimitiveTypeCode.Int64Nullable:
					return (long?)this;
				case PrimitiveTypeCode.UInt64:
					return (ulong)this;
				case PrimitiveTypeCode.UInt64Nullable:
					return (ulong?)this;
				case PrimitiveTypeCode.Single:
					return (float)this;
				case PrimitiveTypeCode.SingleNullable:
					return (float?)this;
				case PrimitiveTypeCode.Double:
					return (double)this;
				case PrimitiveTypeCode.DoubleNullable:
					return (double?)this;
				case PrimitiveTypeCode.DateTime:
					return (DateTime)this;
				case PrimitiveTypeCode.DateTimeNullable:
					return (DateTime?)this;
				case PrimitiveTypeCode.DateTimeOffset:
					return (DateTimeOffset)this;
				case PrimitiveTypeCode.DateTimeOffsetNullable:
					return (DateTimeOffset?)this;
				case PrimitiveTypeCode.Decimal:
					return (decimal)this;
				case PrimitiveTypeCode.DecimalNullable:
					return (decimal?)this;
				case PrimitiveTypeCode.Guid:
					return (Guid)this;
				case PrimitiveTypeCode.GuidNullable:
					return (Guid?)this;
				case PrimitiveTypeCode.TimeSpan:
					return (TimeSpan)this;
				case PrimitiveTypeCode.TimeSpanNullable:
					return (TimeSpan?)this;
				case PrimitiveTypeCode.BigInteger:
					return JToken.ToBigInteger(this);
				case PrimitiveTypeCode.BigIntegerNullable:
					return JToken.ToBigIntegerNullable(this);
				case PrimitiveTypeCode.Uri:
					return (Uri)this;
				case PrimitiveTypeCode.String:
					return (string)this;
				}
			}
			return this.ToObject(objectType, JsonSerializer.CreateDefault());
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0002C6C4 File Offset: 0x0002A8C4
		[NullableContext(2)]
		public T ToObject<T>([Nullable(1)] JsonSerializer jsonSerializer)
		{
			return (T)((object)this.ToObject(typeof(T), jsonSerializer));
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0002C6DC File Offset: 0x0002A8DC
		[NullableContext(2)]
		public object ToObject(Type objectType, [Nullable(1)] JsonSerializer jsonSerializer)
		{
			ValidationUtils.ArgumentNotNull(jsonSerializer, "jsonSerializer");
			object obj;
			using (JTokenReader jtokenReader = new JTokenReader(this))
			{
				JsonSerializerProxy jsonSerializerProxy = jsonSerializer as JsonSerializerProxy;
				if (jsonSerializerProxy != null)
				{
					CultureInfo cultureInfo;
					DateTimeZoneHandling? dateTimeZoneHandling;
					DateParseHandling? dateParseHandling;
					FloatParseHandling? floatParseHandling;
					int? num;
					string text;
					jsonSerializerProxy._serializer.SetupReader(jtokenReader, out cultureInfo, out dateTimeZoneHandling, out dateParseHandling, out floatParseHandling, out num, out text);
				}
				obj = jsonSerializer.Deserialize(jtokenReader, objectType);
			}
			return obj;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0002C748 File Offset: 0x0002A948
		public static JToken ReadFrom(JsonReader reader)
		{
			return JToken.ReadFrom(reader, null);
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0002C754 File Offset: 0x0002A954
		public static JToken ReadFrom(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			bool flag;
			if (reader.TokenType == JsonToken.None)
			{
				flag = ((settings != null && settings.CommentHandling == CommentHandling.Ignore) ? reader.ReadAndMoveToContent() : reader.Read());
			}
			else
			{
				flag = reader.TokenType != JsonToken.Comment || settings == null || settings.CommentHandling != CommentHandling.Ignore || reader.ReadAndMoveToContent();
			}
			if (!flag)
			{
				throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader.");
			}
			IJsonLineInfo jsonLineInfo = reader as IJsonLineInfo;
			switch (reader.TokenType)
			{
			case JsonToken.StartObject:
				return JObject.Load(reader, settings);
			case JsonToken.StartArray:
				return JArray.Load(reader, settings);
			case JsonToken.StartConstructor:
				return JConstructor.Load(reader, settings);
			case JsonToken.PropertyName:
				return JProperty.Load(reader, settings);
			case JsonToken.Comment:
			{
				JValue jvalue = JValue.CreateComment(reader.Value.ToString());
				jvalue.SetLineInfo(jsonLineInfo, settings);
				return jvalue;
			}
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Date:
			case JsonToken.Bytes:
			{
				JValue jvalue2 = new JValue(reader.Value);
				jvalue2.SetLineInfo(jsonLineInfo, settings);
				return jvalue2;
			}
			case JsonToken.Null:
			{
				JValue jvalue3 = JValue.CreateNull();
				jvalue3.SetLineInfo(jsonLineInfo, settings);
				return jvalue3;
			}
			case JsonToken.Undefined:
			{
				JValue jvalue4 = JValue.CreateUndefined();
				jvalue4.SetLineInfo(jsonLineInfo, settings);
				return jvalue4;
			}
			}
			throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader. Unexpected token: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0002C8A3 File Offset: 0x0002AAA3
		public static JToken Parse(string json)
		{
			return JToken.Parse(json, null);
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0002C8AC File Offset: 0x0002AAAC
		public static JToken Parse(string json, [Nullable(2)] JsonLoadSettings settings)
		{
			JToken jtoken2;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				JToken jtoken = JToken.Load(jsonReader, settings);
				while (jsonReader.Read())
				{
				}
				jtoken2 = jtoken;
			}
			return jtoken2;
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0002C8F4 File Offset: 0x0002AAF4
		public static JToken Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			return JToken.ReadFrom(reader, settings);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0002C8FD File Offset: 0x0002AAFD
		public static JToken Load(JsonReader reader)
		{
			return JToken.Load(reader, null);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0002C906 File Offset: 0x0002AB06
		[NullableContext(2)]
		internal void SetLineInfo(IJsonLineInfo lineInfo, JsonLoadSettings settings)
		{
			if (settings != null && settings.LineInfoHandling != LineInfoHandling.Load)
			{
				return;
			}
			if (lineInfo == null || !lineInfo.HasLineInfo())
			{
				return;
			}
			this.SetLineInfo(lineInfo.LineNumber, lineInfo.LinePosition);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0002C933 File Offset: 0x0002AB33
		internal void SetLineInfo(int lineNumber, int linePosition)
		{
			this.AddAnnotation(new JToken.LineInfoAnnotation(lineNumber, linePosition));
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0002C942 File Offset: 0x0002AB42
		bool IJsonLineInfo.HasLineInfo()
		{
			return this.Annotation<JToken.LineInfoAnnotation>() != null;
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x0002C950 File Offset: 0x0002AB50
		int IJsonLineInfo.LineNumber
		{
			get
			{
				JToken.LineInfoAnnotation lineInfoAnnotation = this.Annotation<JToken.LineInfoAnnotation>();
				if (lineInfoAnnotation != null)
				{
					return lineInfoAnnotation.LineNumber;
				}
				return 0;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000B48 RID: 2888 RVA: 0x0002C970 File Offset: 0x0002AB70
		int IJsonLineInfo.LinePosition
		{
			get
			{
				JToken.LineInfoAnnotation lineInfoAnnotation = this.Annotation<JToken.LineInfoAnnotation>();
				if (lineInfoAnnotation != null)
				{
					return lineInfoAnnotation.LinePosition;
				}
				return 0;
			}
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0002C98F File Offset: 0x0002AB8F
		[return: Nullable(2)]
		public JToken SelectToken(string path)
		{
			return this.SelectToken(path, null);
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0002C99C File Offset: 0x0002AB9C
		[return: Nullable(2)]
		public JToken SelectToken(string path, bool errorWhenNoMatch)
		{
			object obj;
			if (!errorWhenNoMatch)
			{
				obj = null;
			}
			else
			{
				(obj = new JsonSelectSettings()).ErrorWhenNoMatch = true;
			}
			JsonSelectSettings jsonSelectSettings = obj;
			return this.SelectToken(path, jsonSelectSettings);
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0002C9C4 File Offset: 0x0002ABC4
		[NullableContext(2)]
		public JToken SelectToken([Nullable(1)] string path, JsonSelectSettings settings)
		{
			JPath jpath = new JPath(path);
			JToken jtoken = null;
			foreach (JToken jtoken2 in jpath.Evaluate(this, this, settings))
			{
				if (jtoken != null)
				{
					throw new JsonException("Path returned multiple tokens.");
				}
				jtoken = jtoken2;
			}
			return jtoken;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0002CA24 File Offset: 0x0002AC24
		public IEnumerable<JToken> SelectTokens(string path)
		{
			return this.SelectTokens(path, null);
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0002CA30 File Offset: 0x0002AC30
		public IEnumerable<JToken> SelectTokens(string path, bool errorWhenNoMatch)
		{
			object obj;
			if (!errorWhenNoMatch)
			{
				obj = null;
			}
			else
			{
				(obj = new JsonSelectSettings()).ErrorWhenNoMatch = true;
			}
			JsonSelectSettings jsonSelectSettings = obj;
			return this.SelectTokens(path, jsonSelectSettings);
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0002CA58 File Offset: 0x0002AC58
		public IEnumerable<JToken> SelectTokens(string path, [Nullable(2)] JsonSelectSettings settings)
		{
			return new JPath(path).Evaluate(this, this, settings);
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0002CA68 File Offset: 0x0002AC68
		protected virtual DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DynamicProxyMetaObject<JToken>(parameter, this, new DynamicProxy<JToken>());
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0002CA76 File Offset: 0x0002AC76
		DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
		{
			return this.GetMetaObject(parameter);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0002CA7F File Offset: 0x0002AC7F
		object ICloneable.Clone()
		{
			return this.DeepClone();
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0002CA87 File Offset: 0x0002AC87
		public JToken DeepClone()
		{
			return this.CloneToken(null);
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0002CA90 File Offset: 0x0002AC90
		public JToken DeepClone(JsonCloneSettings settings)
		{
			return this.CloneToken(settings);
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0002CA9C File Offset: 0x0002AC9C
		public void AddAnnotation(object annotation)
		{
			if (annotation == null)
			{
				throw new ArgumentNullException("annotation");
			}
			if (this._annotations == null)
			{
				object obj;
				if (!(annotation is object[]))
				{
					obj = annotation;
				}
				else
				{
					(obj = new object[1])[0] = annotation;
				}
				this._annotations = obj;
				return;
			}
			object[] array = this._annotations as object[];
			if (array == null)
			{
				this._annotations = new object[] { this._annotations, annotation };
				return;
			}
			int num = 0;
			while (num < array.Length && array[num] != null)
			{
				num++;
			}
			if (num == array.Length)
			{
				Array.Resize<object>(ref array, num * 2);
				this._annotations = array;
			}
			array[num] = annotation;
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0002CB34 File Offset: 0x0002AD34
		[return: Nullable(2)]
		public T Annotation<T>() where T : class
		{
			if (this._annotations != null)
			{
				object[] array = this._annotations as object[];
				if (array == null)
				{
					return this._annotations as T;
				}
				foreach (object obj in array)
				{
					if (obj == null)
					{
						break;
					}
					T t = obj as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0002CBA0 File Offset: 0x0002ADA0
		[return: Nullable(2)]
		public object Annotation(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (this._annotations != null)
			{
				object[] array = this._annotations as object[];
				if (array == null)
				{
					if (type.IsInstanceOfType(this._annotations))
					{
						return this._annotations;
					}
				}
				else
				{
					foreach (object obj in array)
					{
						if (obj == null)
						{
							break;
						}
						if (type.IsInstanceOfType(obj))
						{
							return obj;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0002CC0E File Offset: 0x0002AE0E
		public IEnumerable<T> Annotations<T>() where T : class
		{
			if (this._annotations == null)
			{
				yield break;
			}
			object annotations2 = this._annotations;
			object[] annotations = annotations2 as object[];
			if (annotations != null)
			{
				int num;
				for (int i = 0; i < annotations.Length; i = num + 1)
				{
					object obj = annotations[i];
					if (obj == null)
					{
						break;
					}
					T t = obj as T;
					if (t != null)
					{
						yield return t;
					}
					num = i;
				}
				yield break;
			}
			T t2 = this._annotations as T;
			if (t2 == null)
			{
				yield break;
			}
			yield return t2;
			yield break;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0002CC1E File Offset: 0x0002AE1E
		public IEnumerable<object> Annotations(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (this._annotations == null)
			{
				yield break;
			}
			object annotations2 = this._annotations;
			object[] annotations = annotations2 as object[];
			if (annotations != null)
			{
				int num;
				for (int i = 0; i < annotations.Length; i = num + 1)
				{
					object obj = annotations[i];
					if (obj == null)
					{
						break;
					}
					if (type.IsInstanceOfType(obj))
					{
						yield return obj;
					}
					num = i;
				}
				yield break;
			}
			if (!type.IsInstanceOfType(this._annotations))
			{
				yield break;
			}
			yield return this._annotations;
			yield break;
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0002CC38 File Offset: 0x0002AE38
		public void RemoveAnnotations<T>() where T : class
		{
			if (this._annotations != null)
			{
				object[] array = this._annotations as object[];
				if (array == null)
				{
					if (this._annotations is T)
					{
						this._annotations = null;
						return;
					}
				}
				else
				{
					int i = 0;
					int j = 0;
					while (i < array.Length)
					{
						object obj = array[i];
						if (obj == null)
						{
							break;
						}
						if (!(obj is T))
						{
							array[j++] = obj;
						}
						i++;
					}
					if (j != 0)
					{
						while (j < i)
						{
							array[j++] = null;
						}
						return;
					}
					this._annotations = null;
				}
			}
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0002CCB4 File Offset: 0x0002AEB4
		public void RemoveAnnotations(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (this._annotations != null)
			{
				object[] array = this._annotations as object[];
				if (array == null)
				{
					if (type.IsInstanceOfType(this._annotations))
					{
						this._annotations = null;
						return;
					}
				}
				else
				{
					int i = 0;
					int j = 0;
					while (i < array.Length)
					{
						object obj = array[i];
						if (obj == null)
						{
							break;
						}
						if (!type.IsInstanceOfType(obj))
						{
							array[j++] = obj;
						}
						i++;
					}
					if (j != 0)
					{
						while (j < i)
						{
							array[j++] = null;
						}
						return;
					}
					this._annotations = null;
				}
			}
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0002CD44 File Offset: 0x0002AF44
		internal void CopyAnnotations(JToken target, JToken source)
		{
			object[] array = source._annotations as object[];
			if (array != null)
			{
				target._annotations = array.ToArray<object>();
				return;
			}
			target._annotations = source._annotations;
		}

		// Token: 0x04000390 RID: 912
		[Nullable(2)]
		private static JTokenEqualityComparer _equalityComparer;

		// Token: 0x04000391 RID: 913
		[Nullable(2)]
		private JContainer _parent;

		// Token: 0x04000392 RID: 914
		[Nullable(2)]
		private JToken _previous;

		// Token: 0x04000393 RID: 915
		[Nullable(2)]
		private JToken _next;

		// Token: 0x04000394 RID: 916
		[Nullable(2)]
		private object _annotations;

		// Token: 0x04000395 RID: 917
		private static readonly JTokenType[] BooleanTypes = new JTokenType[]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean
		};

		// Token: 0x04000396 RID: 918
		private static readonly JTokenType[] NumberTypes = new JTokenType[]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean
		};

		// Token: 0x04000397 RID: 919
		private static readonly JTokenType[] BigIntegerTypes = new JTokenType[]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean,
			JTokenType.Bytes
		};

		// Token: 0x04000398 RID: 920
		private static readonly JTokenType[] StringTypes = new JTokenType[]
		{
			JTokenType.Date,
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean,
			JTokenType.Bytes,
			JTokenType.Guid,
			JTokenType.TimeSpan,
			JTokenType.Uri
		};

		// Token: 0x04000399 RID: 921
		private static readonly JTokenType[] GuidTypes = new JTokenType[]
		{
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Guid,
			JTokenType.Bytes
		};

		// Token: 0x0400039A RID: 922
		private static readonly JTokenType[] TimeSpanTypes = new JTokenType[]
		{
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.TimeSpan
		};

		// Token: 0x0400039B RID: 923
		private static readonly JTokenType[] UriTypes = new JTokenType[]
		{
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Uri
		};

		// Token: 0x0400039C RID: 924
		private static readonly JTokenType[] CharTypes = new JTokenType[]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw
		};

		// Token: 0x0400039D RID: 925
		private static readonly JTokenType[] DateTimeTypes = new JTokenType[]
		{
			JTokenType.Date,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw
		};

		// Token: 0x0400039E RID: 926
		private static readonly JTokenType[] BytesTypes = new JTokenType[]
		{
			JTokenType.Bytes,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Integer
		};

		// Token: 0x020001CB RID: 459
		[NullableContext(0)]
		private class LineInfoAnnotation
		{
			// Token: 0x06000FB3 RID: 4019 RVA: 0x0004508A File Offset: 0x0004328A
			public LineInfoAnnotation(int lineNumber, int linePosition)
			{
				this.LineNumber = lineNumber;
				this.LinePosition = linePosition;
			}

			// Token: 0x040007DD RID: 2013
			internal readonly int LineNumber;

			// Token: 0x040007DE RID: 2014
			internal readonly int LinePosition;
		}
	}
}
