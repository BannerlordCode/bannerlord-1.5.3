using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000075 RID: 117
	[NullableContext(1)]
	[Nullable(0)]
	internal class DefaultReferenceResolver : IReferenceResolver
	{
		// Token: 0x06000647 RID: 1607 RVA: 0x0001B104 File Offset: 0x00019304
		private BidirectionalDictionary<string, object> GetMappings(object context)
		{
			JsonSerializerInternalBase jsonSerializerInternalBase = context as JsonSerializerInternalBase;
			if (jsonSerializerInternalBase == null)
			{
				JsonSerializerProxy jsonSerializerProxy = context as JsonSerializerProxy;
				if (jsonSerializerProxy == null)
				{
					throw new JsonException("The DefaultReferenceResolver can only be used internally.");
				}
				jsonSerializerInternalBase = jsonSerializerProxy.GetInternalSerializer();
			}
			return jsonSerializerInternalBase.DefaultReferenceMappings;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001B140 File Offset: 0x00019340
		public object ResolveReference(object context, string reference)
		{
			object obj;
			this.GetMappings(context).TryGetByFirst(reference, out obj);
			return obj;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001B160 File Offset: 0x00019360
		public string GetReference(object context, object value)
		{
			BidirectionalDictionary<string, object> mappings = this.GetMappings(context);
			string text;
			if (!mappings.TryGetBySecond(value, out text))
			{
				this._referenceCount++;
				text = this._referenceCount.ToString(CultureInfo.InvariantCulture);
				mappings.Set(text, value);
			}
			return text;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0001B1A8 File Offset: 0x000193A8
		public void AddReference(object context, string reference, object value)
		{
			this.GetMappings(context).Set(reference, value);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001B1B8 File Offset: 0x000193B8
		public bool IsReferenced(object context, object value)
		{
			string text;
			return this.GetMappings(context).TryGetBySecond(value, out text);
		}

		// Token: 0x04000239 RID: 569
		private int _referenceCount;
	}
}
