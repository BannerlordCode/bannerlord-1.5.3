using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A9 RID: 169
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaModel
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x000262E8 File Offset: 0x000244E8
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x000262F0 File Offset: 0x000244F0
		public bool Required { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x000262F9 File Offset: 0x000244F9
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x00026301 File Offset: 0x00024501
		public JsonSchemaType Type { get; set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x0002630A File Offset: 0x0002450A
		// (set) Token: 0x06000909 RID: 2313 RVA: 0x00026312 File Offset: 0x00024512
		public int? MinimumLength { get; set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x0002631B File Offset: 0x0002451B
		// (set) Token: 0x0600090B RID: 2315 RVA: 0x00026323 File Offset: 0x00024523
		public int? MaximumLength { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x0002632C File Offset: 0x0002452C
		// (set) Token: 0x0600090D RID: 2317 RVA: 0x00026334 File Offset: 0x00024534
		public double? DivisibleBy { get; set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x0002633D File Offset: 0x0002453D
		// (set) Token: 0x0600090F RID: 2319 RVA: 0x00026345 File Offset: 0x00024545
		public double? Minimum { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x0002634E File Offset: 0x0002454E
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x00026356 File Offset: 0x00024556
		public double? Maximum { get; set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x0002635F File Offset: 0x0002455F
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x00026367 File Offset: 0x00024567
		public bool ExclusiveMinimum { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x00026370 File Offset: 0x00024570
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x00026378 File Offset: 0x00024578
		public bool ExclusiveMaximum { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x00026381 File Offset: 0x00024581
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x00026389 File Offset: 0x00024589
		public int? MinimumItems { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x00026392 File Offset: 0x00024592
		// (set) Token: 0x06000919 RID: 2329 RVA: 0x0002639A File Offset: 0x0002459A
		public int? MaximumItems { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x000263A3 File Offset: 0x000245A3
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x000263AB File Offset: 0x000245AB
		public IList<string> Patterns { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x000263B4 File Offset: 0x000245B4
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x000263BC File Offset: 0x000245BC
		public IList<JsonSchemaModel> Items { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x000263C5 File Offset: 0x000245C5
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x000263CD File Offset: 0x000245CD
		public IDictionary<string, JsonSchemaModel> Properties { get; set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x000263D6 File Offset: 0x000245D6
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x000263DE File Offset: 0x000245DE
		public IDictionary<string, JsonSchemaModel> PatternProperties { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x000263E7 File Offset: 0x000245E7
		// (set) Token: 0x06000923 RID: 2339 RVA: 0x000263EF File Offset: 0x000245EF
		public JsonSchemaModel AdditionalProperties { get; set; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x000263F8 File Offset: 0x000245F8
		// (set) Token: 0x06000925 RID: 2341 RVA: 0x00026400 File Offset: 0x00024600
		public JsonSchemaModel AdditionalItems { get; set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x00026409 File Offset: 0x00024609
		// (set) Token: 0x06000927 RID: 2343 RVA: 0x00026411 File Offset: 0x00024611
		public bool PositionalItemsValidation { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x0002641A File Offset: 0x0002461A
		// (set) Token: 0x06000929 RID: 2345 RVA: 0x00026422 File Offset: 0x00024622
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0002642B File Offset: 0x0002462B
		// (set) Token: 0x0600092B RID: 2347 RVA: 0x00026433 File Offset: 0x00024633
		public bool AllowAdditionalItems { get; set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x0002643C File Offset: 0x0002463C
		// (set) Token: 0x0600092D RID: 2349 RVA: 0x00026444 File Offset: 0x00024644
		public bool UniqueItems { get; set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x0002644D File Offset: 0x0002464D
		// (set) Token: 0x0600092F RID: 2351 RVA: 0x00026455 File Offset: 0x00024655
		public IList<JToken> Enum { get; set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x0002645E File Offset: 0x0002465E
		// (set) Token: 0x06000931 RID: 2353 RVA: 0x00026466 File Offset: 0x00024666
		public JsonSchemaType Disallow { get; set; }

		// Token: 0x06000932 RID: 2354 RVA: 0x0002646F File Offset: 0x0002466F
		public JsonSchemaModel()
		{
			this.Type = JsonSchemaType.Any;
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
			this.Required = false;
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00026494 File Offset: 0x00024694
		public static JsonSchemaModel Create(IList<JsonSchema> schemata)
		{
			JsonSchemaModel jsonSchemaModel = new JsonSchemaModel();
			foreach (JsonSchema jsonSchema in schemata)
			{
				JsonSchemaModel.Combine(jsonSchemaModel, jsonSchema);
			}
			return jsonSchemaModel;
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x000264E4 File Offset: 0x000246E4
		private static void Combine(JsonSchemaModel model, JsonSchema schema)
		{
			model.Required = model.Required || schema.Required.GetValueOrDefault();
			model.Type &= schema.Type ?? JsonSchemaType.Any;
			model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
			model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
			model.DivisibleBy = MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
			model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
			model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
			model.ExclusiveMinimum = model.ExclusiveMinimum || schema.ExclusiveMinimum.GetValueOrDefault();
			model.ExclusiveMaximum = model.ExclusiveMaximum || schema.ExclusiveMaximum.GetValueOrDefault();
			model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
			model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
			model.PositionalItemsValidation = model.PositionalItemsValidation || schema.PositionalItemsValidation;
			model.AllowAdditionalProperties = model.AllowAdditionalProperties && schema.AllowAdditionalProperties;
			model.AllowAdditionalItems = model.AllowAdditionalItems && schema.AllowAdditionalItems;
			model.UniqueItems = model.UniqueItems || schema.UniqueItems;
			if (schema.Enum != null)
			{
				if (model.Enum == null)
				{
					model.Enum = new List<JToken>();
				}
				model.Enum.AddRangeDistinct<JToken>(schema.Enum, JToken.EqualityComparer);
			}
			model.Disallow |= schema.Disallow.GetValueOrDefault();
			if (schema.Pattern != null)
			{
				if (model.Patterns == null)
				{
					model.Patterns = new List<string>();
				}
				model.Patterns.AddDistinct(schema.Pattern);
			}
		}
	}
}
