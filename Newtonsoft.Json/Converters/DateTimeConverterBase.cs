using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E2 RID: 226
	public abstract class DateTimeConverterBase : JsonConverter
	{
		// Token: 0x06000C3A RID: 3130 RVA: 0x00030F40 File Offset: 0x0002F140
		[NullableContext(1)]
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime) || objectType == typeof(DateTime?) || (objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?));
		}
	}
}
