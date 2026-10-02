using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000098 RID: 152
	public class KebabCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x06000805 RID: 2053 RVA: 0x000232E5 File Offset: 0x000214E5
		public KebabCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x000232FB File Offset: 0x000214FB
		public KebabCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames)
			: this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0002330C File Offset: 0x0002150C
		public KebabCaseNamingStrategy()
		{
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00023314 File Offset: 0x00021514
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToKebabCase(name);
		}
	}
}
