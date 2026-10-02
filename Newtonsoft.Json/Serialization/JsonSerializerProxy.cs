using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000095 RID: 149
	[NullableContext(1)]
	[Nullable(0)]
	internal class JsonSerializerProxy : JsonSerializer
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060007AC RID: 1964 RVA: 0x000229C5 File Offset: 0x00020BC5
		// (remove) Token: 0x060007AD RID: 1965 RVA: 0x000229D3 File Offset: 0x00020BD3
		[Nullable(new byte[] { 2, 1 })]
		public override event EventHandler<ErrorEventArgs> Error
		{
			add
			{
				this._serializer.Error += value;
			}
			remove
			{
				this._serializer.Error -= value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x000229E1 File Offset: 0x00020BE1
		// (set) Token: 0x060007AF RID: 1967 RVA: 0x000229EE File Offset: 0x00020BEE
		[Nullable(2)]
		public override IReferenceResolver ReferenceResolver
		{
			[NullableContext(2)]
			get
			{
				return this._serializer.ReferenceResolver;
			}
			[NullableContext(2)]
			set
			{
				this._serializer.ReferenceResolver = value;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x000229FC File Offset: 0x00020BFC
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x00022A09 File Offset: 0x00020C09
		[Nullable(2)]
		public override ITraceWriter TraceWriter
		{
			[NullableContext(2)]
			get
			{
				return this._serializer.TraceWriter;
			}
			[NullableContext(2)]
			set
			{
				this._serializer.TraceWriter = value;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x00022A17 File Offset: 0x00020C17
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x00022A24 File Offset: 0x00020C24
		[Nullable(2)]
		public override IEqualityComparer EqualityComparer
		{
			[NullableContext(2)]
			get
			{
				return this._serializer.EqualityComparer;
			}
			[NullableContext(2)]
			set
			{
				this._serializer.EqualityComparer = value;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00022A32 File Offset: 0x00020C32
		public override JsonConverterCollection Converters
		{
			get
			{
				return this._serializer.Converters;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00022A3F File Offset: 0x00020C3F
		// (set) Token: 0x060007B6 RID: 1974 RVA: 0x00022A4C File Offset: 0x00020C4C
		public override DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._serializer.DefaultValueHandling;
			}
			set
			{
				this._serializer.DefaultValueHandling = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x00022A5A File Offset: 0x00020C5A
		// (set) Token: 0x060007B8 RID: 1976 RVA: 0x00022A67 File Offset: 0x00020C67
		public override IContractResolver ContractResolver
		{
			get
			{
				return this._serializer.ContractResolver;
			}
			set
			{
				this._serializer.ContractResolver = value;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x00022A75 File Offset: 0x00020C75
		// (set) Token: 0x060007BA RID: 1978 RVA: 0x00022A82 File Offset: 0x00020C82
		public override MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._serializer.MissingMemberHandling;
			}
			set
			{
				this._serializer.MissingMemberHandling = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x00022A90 File Offset: 0x00020C90
		// (set) Token: 0x060007BC RID: 1980 RVA: 0x00022A9D File Offset: 0x00020C9D
		public override NullValueHandling NullValueHandling
		{
			get
			{
				return this._serializer.NullValueHandling;
			}
			set
			{
				this._serializer.NullValueHandling = value;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00022AAB File Offset: 0x00020CAB
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x00022AB8 File Offset: 0x00020CB8
		public override ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._serializer.ObjectCreationHandling;
			}
			set
			{
				this._serializer.ObjectCreationHandling = value;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x00022AC6 File Offset: 0x00020CC6
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x00022AD3 File Offset: 0x00020CD3
		public override ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._serializer.ReferenceLoopHandling;
			}
			set
			{
				this._serializer.ReferenceLoopHandling = value;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x00022AE1 File Offset: 0x00020CE1
		// (set) Token: 0x060007C2 RID: 1986 RVA: 0x00022AEE File Offset: 0x00020CEE
		public override PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._serializer.PreserveReferencesHandling;
			}
			set
			{
				this._serializer.PreserveReferencesHandling = value;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00022AFC File Offset: 0x00020CFC
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x00022B09 File Offset: 0x00020D09
		public override TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._serializer.TypeNameHandling;
			}
			set
			{
				this._serializer.TypeNameHandling = value;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x00022B17 File Offset: 0x00020D17
		// (set) Token: 0x060007C6 RID: 1990 RVA: 0x00022B24 File Offset: 0x00020D24
		public override MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return this._serializer.MetadataPropertyHandling;
			}
			set
			{
				this._serializer.MetadataPropertyHandling = value;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x00022B32 File Offset: 0x00020D32
		// (set) Token: 0x060007C8 RID: 1992 RVA: 0x00022B3F File Offset: 0x00020D3F
		[Obsolete("TypeNameAssemblyFormat is obsolete. Use TypeNameAssemblyFormatHandling instead.")]
		public override FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return this._serializer.TypeNameAssemblyFormat;
			}
			set
			{
				this._serializer.TypeNameAssemblyFormat = value;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x00022B4D File Offset: 0x00020D4D
		// (set) Token: 0x060007CA RID: 1994 RVA: 0x00022B5A File Offset: 0x00020D5A
		public override TypeNameAssemblyFormatHandling TypeNameAssemblyFormatHandling
		{
			get
			{
				return this._serializer.TypeNameAssemblyFormatHandling;
			}
			set
			{
				this._serializer.TypeNameAssemblyFormatHandling = value;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x00022B68 File Offset: 0x00020D68
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x00022B75 File Offset: 0x00020D75
		public override ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._serializer.ConstructorHandling;
			}
			set
			{
				this._serializer.ConstructorHandling = value;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x00022B83 File Offset: 0x00020D83
		// (set) Token: 0x060007CE RID: 1998 RVA: 0x00022B90 File Offset: 0x00020D90
		[Obsolete("Binder is obsolete. Use SerializationBinder instead.")]
		public override SerializationBinder Binder
		{
			get
			{
				return this._serializer.Binder;
			}
			set
			{
				this._serializer.Binder = value;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x00022B9E File Offset: 0x00020D9E
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x00022BAB File Offset: 0x00020DAB
		public override ISerializationBinder SerializationBinder
		{
			get
			{
				return this._serializer.SerializationBinder;
			}
			set
			{
				this._serializer.SerializationBinder = value;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x00022BB9 File Offset: 0x00020DB9
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x00022BC6 File Offset: 0x00020DC6
		public override StreamingContext Context
		{
			get
			{
				return this._serializer.Context;
			}
			set
			{
				this._serializer.Context = value;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x00022BD4 File Offset: 0x00020DD4
		// (set) Token: 0x060007D4 RID: 2004 RVA: 0x00022BE1 File Offset: 0x00020DE1
		public override Formatting Formatting
		{
			get
			{
				return this._serializer.Formatting;
			}
			set
			{
				this._serializer.Formatting = value;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x00022BEF File Offset: 0x00020DEF
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x00022BFC File Offset: 0x00020DFC
		public override DateFormatHandling DateFormatHandling
		{
			get
			{
				return this._serializer.DateFormatHandling;
			}
			set
			{
				this._serializer.DateFormatHandling = value;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x00022C0A File Offset: 0x00020E0A
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x00022C17 File Offset: 0x00020E17
		public override DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return this._serializer.DateTimeZoneHandling;
			}
			set
			{
				this._serializer.DateTimeZoneHandling = value;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00022C25 File Offset: 0x00020E25
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x00022C32 File Offset: 0x00020E32
		public override DateParseHandling DateParseHandling
		{
			get
			{
				return this._serializer.DateParseHandling;
			}
			set
			{
				this._serializer.DateParseHandling = value;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x00022C40 File Offset: 0x00020E40
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x00022C4D File Offset: 0x00020E4D
		public override FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return this._serializer.FloatFormatHandling;
			}
			set
			{
				this._serializer.FloatFormatHandling = value;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00022C5B File Offset: 0x00020E5B
		// (set) Token: 0x060007DE RID: 2014 RVA: 0x00022C68 File Offset: 0x00020E68
		public override FloatParseHandling FloatParseHandling
		{
			get
			{
				return this._serializer.FloatParseHandling;
			}
			set
			{
				this._serializer.FloatParseHandling = value;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x00022C76 File Offset: 0x00020E76
		// (set) Token: 0x060007E0 RID: 2016 RVA: 0x00022C83 File Offset: 0x00020E83
		public override StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return this._serializer.StringEscapeHandling;
			}
			set
			{
				this._serializer.StringEscapeHandling = value;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00022C91 File Offset: 0x00020E91
		// (set) Token: 0x060007E2 RID: 2018 RVA: 0x00022C9E File Offset: 0x00020E9E
		public override string DateFormatString
		{
			get
			{
				return this._serializer.DateFormatString;
			}
			set
			{
				this._serializer.DateFormatString = value;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00022CAC File Offset: 0x00020EAC
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x00022CB9 File Offset: 0x00020EB9
		public override CultureInfo Culture
		{
			get
			{
				return this._serializer.Culture;
			}
			set
			{
				this._serializer.Culture = value;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x00022CC7 File Offset: 0x00020EC7
		// (set) Token: 0x060007E6 RID: 2022 RVA: 0x00022CD4 File Offset: 0x00020ED4
		public override int? MaxDepth
		{
			get
			{
				return this._serializer.MaxDepth;
			}
			set
			{
				this._serializer.MaxDepth = value;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x00022CE2 File Offset: 0x00020EE2
		// (set) Token: 0x060007E8 RID: 2024 RVA: 0x00022CEF File Offset: 0x00020EEF
		public override bool CheckAdditionalContent
		{
			get
			{
				return this._serializer.CheckAdditionalContent;
			}
			set
			{
				this._serializer.CheckAdditionalContent = value;
			}
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00022CFD File Offset: 0x00020EFD
		internal JsonSerializerInternalBase GetInternalSerializer()
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader;
			}
			return this._serializerWriter;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00022D14 File Offset: 0x00020F14
		public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
		{
			ValidationUtils.ArgumentNotNull(serializerReader, "serializerReader");
			this._serializerReader = serializerReader;
			this._serializer = serializerReader.Serializer;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00022D3A File Offset: 0x00020F3A
		public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
		{
			ValidationUtils.ArgumentNotNull(serializerWriter, "serializerWriter");
			this._serializerWriter = serializerWriter;
			this._serializer = serializerWriter.Serializer;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00022D60 File Offset: 0x00020F60
		[NullableContext(2)]
		internal override object DeserializeInternal([Nullable(1)] JsonReader reader, Type objectType)
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader.Deserialize(reader, objectType, false);
			}
			return this._serializer.Deserialize(reader, objectType);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00022D86 File Offset: 0x00020F86
		internal override void PopulateInternal(JsonReader reader, object target)
		{
			if (this._serializerReader != null)
			{
				this._serializerReader.Populate(reader, target);
				return;
			}
			this._serializer.Populate(reader, target);
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00022DAB File Offset: 0x00020FAB
		[NullableContext(2)]
		internal override void SerializeInternal([Nullable(1)] JsonWriter jsonWriter, object value, Type rootType)
		{
			if (this._serializerWriter != null)
			{
				this._serializerWriter.Serialize(jsonWriter, value, rootType);
				return;
			}
			this._serializer.Serialize(jsonWriter, value);
		}

		// Token: 0x040002C9 RID: 713
		[Nullable(2)]
		private readonly JsonSerializerInternalReader _serializerReader;

		// Token: 0x040002CA RID: 714
		[Nullable(2)]
		private readonly JsonSerializerInternalWriter _serializerWriter;

		// Token: 0x040002CB RID: 715
		internal readonly JsonSerializer _serializer;
	}
}
