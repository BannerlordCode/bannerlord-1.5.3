using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000AB RID: 171
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaNode
	{
		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600093E RID: 2366 RVA: 0x00026B4E File Offset: 0x00024D4E
		public string Id { get; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00026B56 File Offset: 0x00024D56
		public ReadOnlyCollection<JsonSchema> Schemas { get; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000940 RID: 2368 RVA: 0x00026B5E File Offset: 0x00024D5E
		public Dictionary<string, JsonSchemaNode> Properties { get; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x00026B66 File Offset: 0x00024D66
		public Dictionary<string, JsonSchemaNode> PatternProperties { get; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000942 RID: 2370 RVA: 0x00026B6E File Offset: 0x00024D6E
		public List<JsonSchemaNode> Items { get; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x00026B76 File Offset: 0x00024D76
		// (set) Token: 0x06000944 RID: 2372 RVA: 0x00026B7E File Offset: 0x00024D7E
		public JsonSchemaNode AdditionalProperties { get; set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x00026B87 File Offset: 0x00024D87
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x00026B8F File Offset: 0x00024D8F
		public JsonSchemaNode AdditionalItems { get; set; }

		// Token: 0x06000947 RID: 2375 RVA: 0x00026B98 File Offset: 0x00024D98
		public JsonSchemaNode(JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(new JsonSchema[] { schema });
			this.Properties = new Dictionary<string, JsonSchemaNode>();
			this.PatternProperties = new Dictionary<string, JsonSchemaNode>();
			this.Items = new List<JsonSchemaNode>();
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00026BF4 File Offset: 0x00024DF4
		private JsonSchemaNode(JsonSchemaNode source, JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(source.Schemas.Union<JsonSchema>(new JsonSchema[] { schema }).ToList<JsonSchema>());
			this.Properties = new Dictionary<string, JsonSchemaNode>(source.Properties);
			this.PatternProperties = new Dictionary<string, JsonSchemaNode>(source.PatternProperties);
			this.Items = new List<JsonSchemaNode>(source.Items);
			this.AdditionalProperties = source.AdditionalProperties;
			this.AdditionalItems = source.AdditionalItems;
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00026C88 File Offset: 0x00024E88
		public JsonSchemaNode Combine(JsonSchema schema)
		{
			return new JsonSchemaNode(this, schema);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00026C94 File Offset: 0x00024E94
		public static string GetId(IEnumerable<JsonSchema> schemata)
		{
			return string.Join("-", schemata.Select<JsonSchema, string>((JsonSchema s) => s.InternalId).OrderBy<string, string>((string id) => id, StringComparer.Ordinal));
		}
	}
}
