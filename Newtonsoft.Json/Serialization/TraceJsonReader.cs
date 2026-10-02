using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A1 RID: 161
	[NullableContext(1)]
	[Nullable(0)]
	internal class TraceJsonReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x0600082F RID: 2095 RVA: 0x00023790 File Offset: 0x00021990
		public TraceJsonReader(JsonReader innerReader)
		{
			this._innerReader = innerReader;
			this._sw = new StringWriter(CultureInfo.InvariantCulture);
			this._sw.Write("Deserialized JSON: " + Environment.NewLine);
			this._textWriter = new JsonTextWriter(this._sw);
			this._textWriter.Formatting = Formatting.Indented;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x000237F1 File Offset: 0x000219F1
		public string GetDeserializedJsonMessage()
		{
			return this._sw.ToString();
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x000237FE File Offset: 0x000219FE
		public override bool Read()
		{
			bool flag = this._innerReader.Read();
			this.WriteCurrentToken();
			return flag;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00023811 File Offset: 0x00021A11
		public override int? ReadAsInt32()
		{
			int? num = this._innerReader.ReadAsInt32();
			this.WriteCurrentToken();
			return num;
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00023824 File Offset: 0x00021A24
		[NullableContext(2)]
		public override string ReadAsString()
		{
			string text = this._innerReader.ReadAsString();
			this.WriteCurrentToken();
			return text;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00023837 File Offset: 0x00021A37
		[NullableContext(2)]
		public override byte[] ReadAsBytes()
		{
			byte[] array = this._innerReader.ReadAsBytes();
			this.WriteCurrentToken();
			return array;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0002384A File Offset: 0x00021A4A
		public override decimal? ReadAsDecimal()
		{
			decimal? num = this._innerReader.ReadAsDecimal();
			this.WriteCurrentToken();
			return num;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0002385D File Offset: 0x00021A5D
		public override double? ReadAsDouble()
		{
			double? num = this._innerReader.ReadAsDouble();
			this.WriteCurrentToken();
			return num;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00023870 File Offset: 0x00021A70
		public override bool? ReadAsBoolean()
		{
			bool? flag = this._innerReader.ReadAsBoolean();
			this.WriteCurrentToken();
			return flag;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00023883 File Offset: 0x00021A83
		public override DateTime? ReadAsDateTime()
		{
			DateTime? dateTime = this._innerReader.ReadAsDateTime();
			this.WriteCurrentToken();
			return dateTime;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00023896 File Offset: 0x00021A96
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? dateTimeOffset = this._innerReader.ReadAsDateTimeOffset();
			this.WriteCurrentToken();
			return dateTimeOffset;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x000238A9 File Offset: 0x00021AA9
		public void WriteCurrentToken()
		{
			this._textWriter.WriteToken(this._innerReader, false, false, true);
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x000238BF File Offset: 0x00021ABF
		public override int Depth
		{
			get
			{
				return this._innerReader.Depth;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x000238CC File Offset: 0x00021ACC
		public override string Path
		{
			get
			{
				return this._innerReader.Path;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x000238D9 File Offset: 0x00021AD9
		// (set) Token: 0x0600083E RID: 2110 RVA: 0x000238E6 File Offset: 0x00021AE6
		public override char QuoteChar
		{
			get
			{
				return this._innerReader.QuoteChar;
			}
			protected internal set
			{
				this._innerReader.QuoteChar = value;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x000238F4 File Offset: 0x00021AF4
		public override JsonToken TokenType
		{
			get
			{
				return this._innerReader.TokenType;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x00023901 File Offset: 0x00021B01
		[Nullable(2)]
		public override object Value
		{
			[NullableContext(2)]
			get
			{
				return this._innerReader.Value;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x0002390E File Offset: 0x00021B0E
		[Nullable(2)]
		public override Type ValueType
		{
			[NullableContext(2)]
			get
			{
				return this._innerReader.ValueType;
			}
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0002391B File Offset: 0x00021B1B
		public override void Close()
		{
			this._innerReader.Close();
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00023928 File Offset: 0x00021B28
		bool IJsonLineInfo.HasLineInfo()
		{
			IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
			return jsonLineInfo != null && jsonLineInfo.HasLineInfo();
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x0002394C File Offset: 0x00021B4C
		int IJsonLineInfo.LineNumber
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LineNumber;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x00023970 File Offset: 0x00021B70
		int IJsonLineInfo.LinePosition
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LinePosition;
			}
		}

		// Token: 0x040002E2 RID: 738
		private readonly JsonReader _innerReader;

		// Token: 0x040002E3 RID: 739
		private readonly JsonTextWriter _textWriter;

		// Token: 0x040002E4 RID: 740
		private readonly StringWriter _sw;
	}
}
