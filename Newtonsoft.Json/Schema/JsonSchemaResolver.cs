using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000AD RID: 173
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchemaResolver
	{
		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x00026D09 File Offset: 0x00024F09
		// (set) Token: 0x0600094E RID: 2382 RVA: 0x00026D11 File Offset: 0x00024F11
		public IList<JsonSchema> LoadedSchemas { get; protected set; }

		// Token: 0x0600094F RID: 2383 RVA: 0x00026D1A File Offset: 0x00024F1A
		public JsonSchemaResolver()
		{
			this.LoadedSchemas = new List<JsonSchema>();
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00026D30 File Offset: 0x00024F30
		public virtual JsonSchema GetSchema(string reference)
		{
			JsonSchema jsonSchema = this.LoadedSchemas.SingleOrDefault<JsonSchema>((JsonSchema s) => string.Equals(s.Id, reference, StringComparison.Ordinal));
			if (jsonSchema == null)
			{
				jsonSchema = this.LoadedSchemas.SingleOrDefault<JsonSchema>((JsonSchema s) => string.Equals(s.Location, reference, StringComparison.Ordinal));
			}
			return jsonSchema;
		}
	}
}
