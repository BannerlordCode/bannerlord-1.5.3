using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000071 RID: 113
	public class CamelCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x060005FC RID: 1532 RVA: 0x00019165 File Offset: 0x00017365
		public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001917B File Offset: 0x0001737B
		public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames)
			: this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0001918C File Offset: 0x0001738C
		public CamelCaseNamingStrategy()
		{
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00019194 File Offset: 0x00017394
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToCamelCase(name);
		}
	}
}
