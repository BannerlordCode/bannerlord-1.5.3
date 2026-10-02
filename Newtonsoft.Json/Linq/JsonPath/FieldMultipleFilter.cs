using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D1 RID: 209
	[NullableContext(1)]
	[Nullable(0)]
	internal class FieldMultipleFilter : PathFilter
	{
		// Token: 0x06000BE9 RID: 3049 RVA: 0x0002ED01 File Offset: 0x0002CF01
		public FieldMultipleFilter(List<string> names)
		{
			this.Names = names;
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x0002ED10 File Offset: 0x0002CF10
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, [Nullable(2)] JsonSelectSettings settings)
		{
			foreach (JToken jtoken in current)
			{
				JObject o = jtoken as JObject;
				if (o != null)
				{
					foreach (string name in this.Names)
					{
						JToken jtoken2 = o[name];
						if (jtoken2 != null)
						{
							yield return jtoken2;
						}
						if (settings != null && settings.ErrorWhenNoMatch)
						{
							throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith(CultureInfo.InvariantCulture, name));
						}
						name = null;
					}
					List<string>.Enumerator enumerator2 = default(List<string>.Enumerator);
				}
				else if (settings != null && settings.ErrorWhenNoMatch)
				{
					throw new JsonException("Properties {0} not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, string.Join(", ", this.Names.Select<string, string>((string n) => "'" + n + "'")), jtoken.GetType().Name));
				}
				o = null;
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040003CD RID: 973
		internal List<string> Names;
	}
}
