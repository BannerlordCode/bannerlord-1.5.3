using System;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D4 RID: 212
	internal enum QueryOperator
	{
		// Token: 0x040003D3 RID: 979
		None,
		// Token: 0x040003D4 RID: 980
		Equals,
		// Token: 0x040003D5 RID: 981
		NotEquals,
		// Token: 0x040003D6 RID: 982
		Exists,
		// Token: 0x040003D7 RID: 983
		LessThan,
		// Token: 0x040003D8 RID: 984
		LessThanOrEquals,
		// Token: 0x040003D9 RID: 985
		GreaterThan,
		// Token: 0x040003DA RID: 986
		GreaterThanOrEquals,
		// Token: 0x040003DB RID: 987
		And,
		// Token: 0x040003DC RID: 988
		Or,
		// Token: 0x040003DD RID: 989
		RegexEquals,
		// Token: 0x040003DE RID: 990
		StrictEquals,
		// Token: 0x040003DF RID: 991
		StrictNotEquals
	}
}
