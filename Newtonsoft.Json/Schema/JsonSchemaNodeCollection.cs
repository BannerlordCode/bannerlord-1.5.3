using System;
using System.Collections.ObjectModel;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000AC RID: 172
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaNodeCollection : KeyedCollection<string, JsonSchemaNode>
	{
		// Token: 0x0600094B RID: 2379 RVA: 0x00026CF9 File Offset: 0x00024EF9
		protected override string GetKeyForItem(JsonSchemaNode item)
		{
			return item.Id;
		}
	}
}
