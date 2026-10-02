using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D0 RID: 208
	[NullableContext(2)]
	[Nullable(0)]
	internal class FieldFilter : PathFilter
	{
		// Token: 0x06000BE7 RID: 3047 RVA: 0x0002ECD4 File Offset: 0x0002CED4
		public FieldFilter(string name)
		{
			this.Name = name;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x0002ECE3 File Offset: 0x0002CEE3
		[NullableContext(1)]
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, [Nullable(2)] JsonSelectSettings settings)
		{
			foreach (JToken jtoken in current)
			{
				JObject jobject = jtoken as JObject;
				if (jobject != null)
				{
					if (this.Name != null)
					{
						JToken jtoken2 = jobject[this.Name];
						if (jtoken2 != null)
						{
							yield return jtoken2;
						}
						else if (settings != null && settings.ErrorWhenNoMatch)
						{
							throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith(CultureInfo.InvariantCulture, this.Name));
						}
					}
					else
					{
						foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
						{
							yield return keyValuePair.Value;
						}
						IEnumerator<KeyValuePair<string, JToken>> enumerator2 = null;
					}
				}
				else if (settings != null && settings.ErrorWhenNoMatch)
				{
					throw new JsonException("Property '{0}' not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, this.Name ?? "*", jtoken.GetType().Name));
				}
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040003CC RID: 972
		internal string Name;
	}
}
