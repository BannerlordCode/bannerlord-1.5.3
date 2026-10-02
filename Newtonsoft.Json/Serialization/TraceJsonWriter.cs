using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A2 RID: 162
	[NullableContext(1)]
	[Nullable(0)]
	internal class TraceJsonWriter : JsonWriter
	{
		// Token: 0x06000846 RID: 2118 RVA: 0x00023994 File Offset: 0x00021B94
		public TraceJsonWriter(JsonWriter innerWriter)
		{
			this._innerWriter = innerWriter;
			this._sw = new StringWriter(CultureInfo.InvariantCulture);
			this._sw.Write("Serialized JSON: " + Environment.NewLine);
			this._textWriter = new JsonTextWriter(this._sw);
			this._textWriter.Formatting = Formatting.Indented;
			this._textWriter.Culture = innerWriter.Culture;
			this._textWriter.DateFormatHandling = innerWriter.DateFormatHandling;
			this._textWriter.DateFormatString = innerWriter.DateFormatString;
			this._textWriter.DateTimeZoneHandling = innerWriter.DateTimeZoneHandling;
			this._textWriter.FloatFormatHandling = innerWriter.FloatFormatHandling;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x00023A4A File Offset: 0x00021C4A
		public string GetSerializedJsonMessage()
		{
			return this._sw.ToString();
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00023A57 File Offset: 0x00021C57
		public override void WriteValue(decimal value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00023A78 File Offset: 0x00021C78
		public override void WriteValue(decimal? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00023AAF File Offset: 0x00021CAF
		public override void WriteValue(bool value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00023AD0 File Offset: 0x00021CD0
		public override void WriteValue(bool? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00023B07 File Offset: 0x00021D07
		public override void WriteValue(byte value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00023B28 File Offset: 0x00021D28
		public override void WriteValue(byte? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x00023B5F File Offset: 0x00021D5F
		public override void WriteValue(char value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00023B80 File Offset: 0x00021D80
		public override void WriteValue(char? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00023BB7 File Offset: 0x00021DB7
		[NullableContext(2)]
		public override void WriteValue(byte[] value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value == null)
			{
				base.WriteUndefined();
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00023BE2 File Offset: 0x00021DE2
		public override void WriteValue(DateTime value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00023C03 File Offset: 0x00021E03
		public override void WriteValue(DateTime? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00023C3A File Offset: 0x00021E3A
		public override void WriteValue(DateTimeOffset value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00023C5B File Offset: 0x00021E5B
		public override void WriteValue(DateTimeOffset? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00023C92 File Offset: 0x00021E92
		public override void WriteValue(double value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00023CB3 File Offset: 0x00021EB3
		public override void WriteValue(double? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00023CEA File Offset: 0x00021EEA
		public override void WriteUndefined()
		{
			this._textWriter.WriteUndefined();
			this._innerWriter.WriteUndefined();
			base.WriteUndefined();
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00023D08 File Offset: 0x00021F08
		public override void WriteNull()
		{
			this._textWriter.WriteNull();
			this._innerWriter.WriteNull();
			base.WriteUndefined();
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00023D26 File Offset: 0x00021F26
		public override void WriteValue(float value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x00023D47 File Offset: 0x00021F47
		public override void WriteValue(float? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00023D7E File Offset: 0x00021F7E
		public override void WriteValue(Guid value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x00023D9F File Offset: 0x00021F9F
		public override void WriteValue(Guid? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x00023DD6 File Offset: 0x00021FD6
		public override void WriteValue(int value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x00023DF7 File Offset: 0x00021FF7
		public override void WriteValue(int? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00023E2E File Offset: 0x0002202E
		public override void WriteValue(long value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x00023E4F File Offset: 0x0002204F
		public override void WriteValue(long? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x00023E88 File Offset: 0x00022088
		[NullableContext(2)]
		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				this._textWriter.WriteValue(value);
				this._innerWriter.WriteValue(value);
				base.InternalWriteValue(JsonToken.Integer);
				return;
			}
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value == null)
			{
				base.WriteUndefined();
				return;
			}
			base.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00023EE7 File Offset: 0x000220E7
		public override void WriteValue(sbyte value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00023F08 File Offset: 0x00022108
		public override void WriteValue(sbyte? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00023F3F File Offset: 0x0002213F
		public override void WriteValue(short value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00023F60 File Offset: 0x00022160
		public override void WriteValue(short? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00023F97 File Offset: 0x00022197
		[NullableContext(2)]
		public override void WriteValue(string value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00023FB8 File Offset: 0x000221B8
		public override void WriteValue(TimeSpan value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00023FD9 File Offset: 0x000221D9
		public override void WriteValue(TimeSpan? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00024010 File Offset: 0x00022210
		public override void WriteValue(uint value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00024031 File Offset: 0x00022231
		public override void WriteValue(uint? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00024068 File Offset: 0x00022268
		public override void WriteValue(ulong value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00024089 File Offset: 0x00022289
		public override void WriteValue(ulong? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x000240C0 File Offset: 0x000222C0
		[NullableContext(2)]
		public override void WriteValue(Uri value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value == null)
			{
				base.WriteUndefined();
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x000240F1 File Offset: 0x000222F1
		public override void WriteValue(ushort value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00024112 File Offset: 0x00022312
		public override void WriteValue(ushort? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00024149 File Offset: 0x00022349
		public override void WriteWhitespace(string ws)
		{
			this._textWriter.WriteWhitespace(ws);
			this._innerWriter.WriteWhitespace(ws);
			base.WriteWhitespace(ws);
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0002416A File Offset: 0x0002236A
		[NullableContext(2)]
		public override void WriteComment(string text)
		{
			this._textWriter.WriteComment(text);
			this._innerWriter.WriteComment(text);
			base.WriteComment(text);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0002418B File Offset: 0x0002238B
		public override void WriteStartArray()
		{
			this._textWriter.WriteStartArray();
			this._innerWriter.WriteStartArray();
			base.WriteStartArray();
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x000241A9 File Offset: 0x000223A9
		public override void WriteEndArray()
		{
			this._textWriter.WriteEndArray();
			this._innerWriter.WriteEndArray();
			base.WriteEndArray();
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x000241C7 File Offset: 0x000223C7
		public override void WriteStartConstructor(string name)
		{
			this._textWriter.WriteStartConstructor(name);
			this._innerWriter.WriteStartConstructor(name);
			base.WriteStartConstructor(name);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x000241E8 File Offset: 0x000223E8
		public override void WriteEndConstructor()
		{
			this._textWriter.WriteEndConstructor();
			this._innerWriter.WriteEndConstructor();
			base.WriteEndConstructor();
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00024206 File Offset: 0x00022406
		public override void WritePropertyName(string name)
		{
			this._textWriter.WritePropertyName(name);
			this._innerWriter.WritePropertyName(name);
			base.WritePropertyName(name);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00024227 File Offset: 0x00022427
		public override void WritePropertyName(string name, bool escape)
		{
			this._textWriter.WritePropertyName(name, escape);
			this._innerWriter.WritePropertyName(name, escape);
			base.WritePropertyName(name);
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0002424A File Offset: 0x0002244A
		public override void WriteStartObject()
		{
			this._textWriter.WriteStartObject();
			this._innerWriter.WriteStartObject();
			base.WriteStartObject();
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00024268 File Offset: 0x00022468
		public override void WriteEndObject()
		{
			this._textWriter.WriteEndObject();
			this._innerWriter.WriteEndObject();
			base.WriteEndObject();
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00024286 File Offset: 0x00022486
		[NullableContext(2)]
		public override void WriteRawValue(string json)
		{
			this._textWriter.WriteRawValue(json);
			this._innerWriter.WriteRawValue(json);
			base.InternalWriteValue(JsonToken.Undefined);
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x000242A8 File Offset: 0x000224A8
		[NullableContext(2)]
		public override void WriteRaw(string json)
		{
			this._textWriter.WriteRaw(json);
			this._innerWriter.WriteRaw(json);
			base.WriteRaw(json);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x000242C9 File Offset: 0x000224C9
		public override void Close()
		{
			this._textWriter.Close();
			this._innerWriter.Close();
			base.Close();
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x000242E7 File Offset: 0x000224E7
		public override void Flush()
		{
			this._textWriter.Flush();
			this._innerWriter.Flush();
		}

		// Token: 0x040002E5 RID: 741
		private readonly JsonWriter _innerWriter;

		// Token: 0x040002E6 RID: 742
		private readonly JsonTextWriter _textWriter;

		// Token: 0x040002E7 RID: 743
		private readonly StringWriter _sw;
	}
}
