using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C8 RID: 200
	[NullableContext(2)]
	[Nullable(0)]
	public class JTokenWriter : JsonWriter
	{
		// Token: 0x06000B6F RID: 2927 RVA: 0x0002D39D File Offset: 0x0002B59D
		[NullableContext(1)]
		internal override Task WriteTokenAsync(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments, CancellationToken cancellationToken)
		{
			if (reader is JTokenReader)
			{
				this.WriteToken(reader, writeChildren, writeDateConstructorAsDate, writeComments);
				return AsyncUtils.CompletedTask;
			}
			return base.WriteTokenSyncReadingAsync(reader, cancellationToken);
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000B70 RID: 2928 RVA: 0x0002D3C1 File Offset: 0x0002B5C1
		public JToken CurrentToken
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x0002D3C9 File Offset: 0x0002B5C9
		public JToken Token
		{
			get
			{
				if (this._token != null)
				{
					return this._token;
				}
				return this._value;
			}
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0002D3E0 File Offset: 0x0002B5E0
		[NullableContext(1)]
		public JTokenWriter(JContainer container)
		{
			ValidationUtils.ArgumentNotNull(container, "container");
			this._token = container;
			this._parent = container;
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0002D401 File Offset: 0x0002B601
		public JTokenWriter()
		{
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0002D409 File Offset: 0x0002B609
		public override void Flush()
		{
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0002D40B File Offset: 0x0002B60B
		public override void Close()
		{
			base.Close();
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0002D413 File Offset: 0x0002B613
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new JObject());
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0002D426 File Offset: 0x0002B626
		[NullableContext(1)]
		private void AddParent(JContainer container)
		{
			if (this._parent == null)
			{
				this._token = container;
			}
			else
			{
				this._parent.AddAndSkipParentCheck(container);
			}
			this._parent = container;
			this._current = container;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0002D454 File Offset: 0x0002B654
		private void RemoveParent()
		{
			this._current = this._parent;
			this._parent = this._parent.Parent;
			if (this._parent != null && this._parent.Type == JTokenType.Property)
			{
				this._parent = this._parent.Parent;
			}
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0002D4A5 File Offset: 0x0002B6A5
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new JArray());
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0002D4B8 File Offset: 0x0002B6B8
		[NullableContext(1)]
		public override void WriteStartConstructor(string name)
		{
			base.WriteStartConstructor(name);
			this.AddParent(new JConstructor(name));
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0002D4CD File Offset: 0x0002B6CD
		protected override void WriteEnd(JsonToken token)
		{
			this.RemoveParent();
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0002D4D5 File Offset: 0x0002B6D5
		[NullableContext(1)]
		public override void WritePropertyName(string name)
		{
			JObject jobject = this._parent as JObject;
			if (jobject != null)
			{
				jobject.Remove(name);
			}
			this.AddParent(new JProperty(name));
			base.WritePropertyName(name);
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0002D502 File Offset: 0x0002B702
		private void AddRawValue(object value, JTokenType type, JsonToken token)
		{
			this.AddJValue(new JValue(value, type), token);
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0002D514 File Offset: 0x0002B714
		internal void AddJValue(JValue value, JsonToken token)
		{
			if (this._parent != null)
			{
				if (this._parent.TryAdd(value))
				{
					this._current = this._parent.Last;
					if (this._parent.Type == JTokenType.Property)
					{
						this._parent = this._parent.Parent;
						return;
					}
				}
			}
			else
			{
				this._value = value ?? JValue.CreateNull();
				this._current = this._value;
			}
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0002D584 File Offset: 0x0002B784
		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				base.InternalWriteValue(JsonToken.Integer);
				this.AddRawValue(value, JTokenType.Integer, JsonToken.Integer);
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0002D5A6 File Offset: 0x0002B7A6
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddJValue(JValue.CreateNull(), JsonToken.Null);
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0002D5BB File Offset: 0x0002B7BB
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddJValue(JValue.CreateUndefined(), JsonToken.Undefined);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0002D5D0 File Offset: 0x0002B7D0
		public override void WriteRaw(string json)
		{
			base.WriteRaw(json);
			this.AddJValue(new JRaw(json), JsonToken.Raw);
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0002D5E6 File Offset: 0x0002B7E6
		public override void WriteComment(string text)
		{
			base.WriteComment(text);
			this.AddJValue(JValue.CreateComment(text), JsonToken.Comment);
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0002D5FC File Offset: 0x0002B7FC
		public override void WriteValue(string value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.String);
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0002D61D File Offset: 0x0002B81D
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddRawValue(value, JTokenType.Integer, JsonToken.Integer);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0002D634 File Offset: 0x0002B834
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			base.WriteValue(value);
			this.AddRawValue(value, JTokenType.Integer, JsonToken.Integer);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0002D64B File Offset: 0x0002B84B
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.Integer);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0002D661 File Offset: 0x0002B861
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.Integer);
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0002D677 File Offset: 0x0002B877
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.Float);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0002D68D File Offset: 0x0002B88D
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.Float);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0002D6A3 File Offset: 0x0002B8A3
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.Boolean);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0002D6BA File Offset: 0x0002B8BA
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddRawValue(value, JTokenType.Integer, JsonToken.Integer);
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0002D6D1 File Offset: 0x0002B8D1
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddRawValue(value, JTokenType.Integer, JsonToken.Integer);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0002D6E8 File Offset: 0x0002B8E8
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string text = value.ToString(CultureInfo.InvariantCulture);
			this.AddJValue(new JValue(text), JsonToken.String);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0002D717 File Offset: 0x0002B917
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddRawValue(value, JTokenType.Integer, JsonToken.Integer);
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0002D72E File Offset: 0x0002B92E
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddRawValue(value, JTokenType.Integer, JsonToken.Integer);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0002D745 File Offset: 0x0002B945
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.Float);
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0002D75B File Offset: 0x0002B95B
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			this.AddJValue(new JValue(value), JsonToken.Date);
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0002D780 File Offset: 0x0002B980
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.Date);
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0002D797 File Offset: 0x0002B997
		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value, JTokenType.Bytes), JsonToken.Bytes);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x0002D7B0 File Offset: 0x0002B9B0
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.String);
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0002D7C7 File Offset: 0x0002B9C7
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.String);
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0002D7DE File Offset: 0x0002B9DE
		public override void WriteValue(Uri value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.String);
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0002D7F8 File Offset: 0x0002B9F8
		[NullableContext(1)]
		internal override void WriteToken(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments)
		{
			JTokenReader jtokenReader = reader as JTokenReader;
			if (jtokenReader == null || !writeChildren || !writeDateConstructorAsDate || !writeComments)
			{
				base.WriteToken(reader, writeChildren, writeDateConstructorAsDate, writeComments);
				return;
			}
			if (jtokenReader.TokenType == JsonToken.None && !jtokenReader.Read())
			{
				return;
			}
			JToken jtoken = jtokenReader.CurrentToken.CloneToken(null);
			if (this._parent != null)
			{
				this._parent.Add(jtoken);
				this._current = this._parent.Last;
				if (this._parent.Type == JTokenType.Property)
				{
					this._parent = this._parent.Parent;
					base.InternalWriteValue(JsonToken.Null);
				}
			}
			else
			{
				this._current = jtoken;
				if (this._token == null && this._value == null)
				{
					this._token = jtoken as JContainer;
					this._value = jtoken as JValue;
				}
			}
			jtokenReader.Skip();
		}

		// Token: 0x040003B6 RID: 950
		private JContainer _token;

		// Token: 0x040003B7 RID: 951
		private JContainer _parent;

		// Token: 0x040003B8 RID: 952
		private JValue _value;

		// Token: 0x040003B9 RID: 953
		private JToken _current;
	}
}
