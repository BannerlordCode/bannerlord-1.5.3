using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000B1 RID: 177
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class ValidationEventArgs : EventArgs
	{
		// Token: 0x06000958 RID: 2392 RVA: 0x000274CB File Offset: 0x000256CB
		internal ValidationEventArgs(JsonSchemaException ex)
		{
			ValidationUtils.ArgumentNotNull(ex, "ex");
			this._ex = ex;
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x000274E5 File Offset: 0x000256E5
		public JsonSchemaException Exception
		{
			get
			{
				return this._ex;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x000274ED File Offset: 0x000256ED
		public string Path
		{
			get
			{
				return this._ex.Path;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x000274FA File Offset: 0x000256FA
		public string Message
		{
			get
			{
				return this._ex.Message;
			}
		}

		// Token: 0x0400036D RID: 877
		private readonly JsonSchemaException _ex;
	}
}
