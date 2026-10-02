using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A0 RID: 160
	public class SnakeCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x0600082B RID: 2091 RVA: 0x00023756 File Offset: 0x00021956
		public SnakeCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0002376C File Offset: 0x0002196C
		public SnakeCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames)
			: this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0002377D File Offset: 0x0002197D
		public SnakeCaseNamingStrategy()
		{
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00023785 File Offset: 0x00021985
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToSnakeCase(name);
		}
	}
}
