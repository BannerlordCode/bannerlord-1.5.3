using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BF RID: 191
	[NullableContext(1)]
	[Nullable(0)]
	public class JRaw : JValue
	{
		// Token: 0x06000A9F RID: 2719 RVA: 0x0002A60C File Offset: 0x0002880C
		public static async Task<JRaw> CreateAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			JRaw jraw;
			using (StringWriter sw = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
				{
					await jsonWriter.WriteTokenSyncReadingAsync(reader, cancellationToken).ConfigureAwait(false);
					jraw = new JRaw(sw.ToString());
				}
			}
			return jraw;
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0002A657 File Offset: 0x00028857
		public JRaw(JRaw other)
			: base(other, null)
		{
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0002A661 File Offset: 0x00028861
		internal JRaw(JRaw other, [Nullable(2)] JsonCloneSettings settings)
			: base(other, settings)
		{
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0002A66B File Offset: 0x0002886B
		[NullableContext(2)]
		public JRaw(object rawJson)
			: base(rawJson, JTokenType.Raw)
		{
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0002A678 File Offset: 0x00028878
		public static JRaw Create(JsonReader reader)
		{
			JRaw jraw;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
				{
					jsonTextWriter.WriteToken(reader);
					jraw = new JRaw(stringWriter.ToString());
				}
			}
			return jraw;
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0002A6E0 File Offset: 0x000288E0
		internal override JToken CloneToken([Nullable(2)] JsonCloneSettings settings)
		{
			return new JRaw(this, settings);
		}
	}
}
