using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A3 RID: 163
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public static class Extensions
	{
		// Token: 0x0600087E RID: 2174 RVA: 0x00024300 File Offset: 0x00022500
		[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
		public static bool IsValid(this JToken source, JsonSchema schema)
		{
			bool valid = true;
			source.Validate(schema, delegate(object sender, ValidationEventArgs args)
			{
				valid = false;
			});
			return valid;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00024334 File Offset: 0x00022534
		[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
		public static bool IsValid(this JToken source, JsonSchema schema, out IList<string> errorMessages)
		{
			IList<string> errors = new List<string>();
			source.Validate(schema, delegate(object sender, ValidationEventArgs args)
			{
				errors.Add(args.Message);
			});
			errorMessages = errors;
			return errorMessages.Count == 0;
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00024377 File Offset: 0x00022577
		[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
		public static void Validate(this JToken source, JsonSchema schema)
		{
			source.Validate(schema, null);
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00024384 File Offset: 0x00022584
		[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
		public static void Validate(this JToken source, JsonSchema schema, ValidationEventHandler validationEventHandler)
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			ValidationUtils.ArgumentNotNull(schema, "schema");
			using (JsonValidatingReader jsonValidatingReader = new JsonValidatingReader(source.CreateReader()))
			{
				jsonValidatingReader.Schema = schema;
				if (validationEventHandler != null)
				{
					jsonValidatingReader.ValidationEventHandler += validationEventHandler;
				}
				while (jsonValidatingReader.Read())
				{
				}
			}
		}
	}
}
