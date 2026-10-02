using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A4 RID: 164
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchema
	{
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x000243E8 File Offset: 0x000225E8
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x000243F0 File Offset: 0x000225F0
		public string Id { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x000243F9 File Offset: 0x000225F9
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x00024401 File Offset: 0x00022601
		public string Title { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0002440A File Offset: 0x0002260A
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x00024412 File Offset: 0x00022612
		public bool? Required { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0002441B File Offset: 0x0002261B
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x00024423 File Offset: 0x00022623
		public bool? ReadOnly { get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x0002442C File Offset: 0x0002262C
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x00024434 File Offset: 0x00022634
		public bool? Hidden { get; set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x0002443D File Offset: 0x0002263D
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x00024445 File Offset: 0x00022645
		public bool? Transient { get; set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x0002444E File Offset: 0x0002264E
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x00024456 File Offset: 0x00022656
		public string Description { get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x0002445F File Offset: 0x0002265F
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x00024467 File Offset: 0x00022667
		public JsonSchemaType? Type { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00024470 File Offset: 0x00022670
		// (set) Token: 0x06000893 RID: 2195 RVA: 0x00024478 File Offset: 0x00022678
		public string Pattern { get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x00024481 File Offset: 0x00022681
		// (set) Token: 0x06000895 RID: 2197 RVA: 0x00024489 File Offset: 0x00022689
		public int? MinimumLength { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x00024492 File Offset: 0x00022692
		// (set) Token: 0x06000897 RID: 2199 RVA: 0x0002449A File Offset: 0x0002269A
		public int? MaximumLength { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x000244A3 File Offset: 0x000226A3
		// (set) Token: 0x06000899 RID: 2201 RVA: 0x000244AB File Offset: 0x000226AB
		public double? DivisibleBy { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x000244B4 File Offset: 0x000226B4
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x000244BC File Offset: 0x000226BC
		public double? Minimum { get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x000244C5 File Offset: 0x000226C5
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x000244CD File Offset: 0x000226CD
		public double? Maximum { get; set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x000244D6 File Offset: 0x000226D6
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x000244DE File Offset: 0x000226DE
		public bool? ExclusiveMinimum { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x000244E7 File Offset: 0x000226E7
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x000244EF File Offset: 0x000226EF
		public bool? ExclusiveMaximum { get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x000244F8 File Offset: 0x000226F8
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x00024500 File Offset: 0x00022700
		public int? MinimumItems { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00024509 File Offset: 0x00022709
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x00024511 File Offset: 0x00022711
		public int? MaximumItems { get; set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x0002451A File Offset: 0x0002271A
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x00024522 File Offset: 0x00022722
		public IList<JsonSchema> Items { get; set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x0002452B File Offset: 0x0002272B
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x00024533 File Offset: 0x00022733
		public bool PositionalItemsValidation { get; set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x0002453C File Offset: 0x0002273C
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x00024544 File Offset: 0x00022744
		public JsonSchema AdditionalItems { get; set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x0002454D File Offset: 0x0002274D
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x00024555 File Offset: 0x00022755
		public bool AllowAdditionalItems { get; set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x0002455E File Offset: 0x0002275E
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x00024566 File Offset: 0x00022766
		public bool UniqueItems { get; set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0002456F File Offset: 0x0002276F
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x00024577 File Offset: 0x00022777
		public IDictionary<string, JsonSchema> Properties { get; set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x00024580 File Offset: 0x00022780
		// (set) Token: 0x060008B3 RID: 2227 RVA: 0x00024588 File Offset: 0x00022788
		public JsonSchema AdditionalProperties { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x00024591 File Offset: 0x00022791
		// (set) Token: 0x060008B5 RID: 2229 RVA: 0x00024599 File Offset: 0x00022799
		public IDictionary<string, JsonSchema> PatternProperties { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x000245A2 File Offset: 0x000227A2
		// (set) Token: 0x060008B7 RID: 2231 RVA: 0x000245AA File Offset: 0x000227AA
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x000245B3 File Offset: 0x000227B3
		// (set) Token: 0x060008B9 RID: 2233 RVA: 0x000245BB File Offset: 0x000227BB
		public string Requires { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x000245C4 File Offset: 0x000227C4
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x000245CC File Offset: 0x000227CC
		public IList<JToken> Enum { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x000245D5 File Offset: 0x000227D5
		// (set) Token: 0x060008BD RID: 2237 RVA: 0x000245DD File Offset: 0x000227DD
		public JsonSchemaType? Disallow { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x000245E6 File Offset: 0x000227E6
		// (set) Token: 0x060008BF RID: 2239 RVA: 0x000245EE File Offset: 0x000227EE
		public JToken Default { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x000245F7 File Offset: 0x000227F7
		// (set) Token: 0x060008C1 RID: 2241 RVA: 0x000245FF File Offset: 0x000227FF
		public IList<JsonSchema> Extends { get; set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00024608 File Offset: 0x00022808
		// (set) Token: 0x060008C3 RID: 2243 RVA: 0x00024610 File Offset: 0x00022810
		public string Format { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x00024619 File Offset: 0x00022819
		// (set) Token: 0x060008C5 RID: 2245 RVA: 0x00024621 File Offset: 0x00022821
		internal string Location { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x0002462A File Offset: 0x0002282A
		internal string InternalId
		{
			get
			{
				return this._internalId;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00024632 File Offset: 0x00022832
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x0002463A File Offset: 0x0002283A
		internal string DeferredReference { get; set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x00024643 File Offset: 0x00022843
		// (set) Token: 0x060008CA RID: 2250 RVA: 0x0002464B File Offset: 0x0002284B
		internal bool ReferencesResolved { get; set; }

		// Token: 0x060008CB RID: 2251 RVA: 0x00024654 File Offset: 0x00022854
		public JsonSchema()
		{
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0002468D File Offset: 0x0002288D
		public static JsonSchema Read(JsonReader reader)
		{
			return JsonSchema.Read(reader, new JsonSchemaResolver());
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0002469A File Offset: 0x0002289A
		public static JsonSchema Read(JsonReader reader, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			return new JsonSchemaBuilder(resolver).Read(reader);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x000246BE File Offset: 0x000228BE
		public static JsonSchema Parse(string json)
		{
			return JsonSchema.Parse(json, new JsonSchemaResolver());
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x000246CC File Offset: 0x000228CC
		public static JsonSchema Parse(string json, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(json, "json");
			JsonSchema jsonSchema;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				jsonSchema = JsonSchema.Read(jsonReader, resolver);
			}
			return jsonSchema;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00024718 File Offset: 0x00022918
		public void WriteTo(JsonWriter writer)
		{
			this.WriteTo(writer, new JsonSchemaResolver());
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00024726 File Offset: 0x00022926
		public void WriteTo(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			new JsonSchemaWriter(writer, resolver).WriteSchema(this);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0002474C File Offset: 0x0002294C
		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			this.WriteTo(new JsonTextWriter(stringWriter)
			{
				Formatting = Formatting.Indented
			});
			return stringWriter.ToString();
		}

		// Token: 0x0400030A RID: 778
		private readonly string _internalId = Guid.NewGuid().ToString("N");
	}
}
