using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000DC RID: 220
	[NullableContext(1)]
	[Nullable(0)]
	internal class ScanMultipleFilter : PathFilter
	{
		// Token: 0x06000C1D RID: 3101 RVA: 0x000305D1 File Offset: 0x0002E7D1
		public ScanMultipleFilter(List<string> names)
		{
			this._names = names;
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x000305E0 File Offset: 0x0002E7E0
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, [Nullable(2)] JsonSelectSettings settings)
		{
			foreach (JToken c in current)
			{
				JToken value = c;
				for (;;)
				{
					JContainer jcontainer = value as JContainer;
					value = PathFilter.GetNextScanValue(c, jcontainer, value);
					if (value == null)
					{
						break;
					}
					JProperty property = value as JProperty;
					if (property != null)
					{
						foreach (string text in this._names)
						{
							if (property.Name == text)
							{
								yield return property.Value;
							}
						}
						List<string>.Enumerator enumerator2 = default(List<string>.Enumerator);
					}
					property = null;
				}
				value = null;
				c = null;
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040003E8 RID: 1000
		private List<string> _names;
	}
}
