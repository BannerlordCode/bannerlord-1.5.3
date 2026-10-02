using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A7 RID: 167
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	[Serializable]
	public class JsonSchemaException : JsonException
	{
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x000258E3 File Offset: 0x00023AE3
		public int LineNumber { get; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x000258EB File Offset: 0x00023AEB
		public int LinePosition { get; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x000258F3 File Offset: 0x00023AF3
		public string Path { get; }

		// Token: 0x060008E9 RID: 2281 RVA: 0x000258FB File Offset: 0x00023AFB
		public JsonSchemaException()
		{
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00025903 File Offset: 0x00023B03
		public JsonSchemaException(string message)
			: base(message)
		{
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0002590C File Offset: 0x00023B0C
		public JsonSchemaException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00025916 File Offset: 0x00023B16
		public JsonSchemaException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00025920 File Offset: 0x00023B20
		internal JsonSchemaException(string message, Exception innerException, string path, int lineNumber, int linePosition)
			: base(message, innerException)
		{
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}
	}
}
