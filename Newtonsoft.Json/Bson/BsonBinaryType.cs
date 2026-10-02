using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000102 RID: 258
	internal enum BsonBinaryType : byte
	{
		// Token: 0x0400041B RID: 1051
		Binary,
		// Token: 0x0400041C RID: 1052
		Function,
		// Token: 0x0400041D RID: 1053
		[Obsolete("This type has been deprecated in the BSON specification. Use Binary instead.")]
		BinaryOld,
		// Token: 0x0400041E RID: 1054
		[Obsolete("This type has been deprecated in the BSON specification. Use Uuid instead.")]
		UuidOld,
		// Token: 0x0400041F RID: 1055
		Uuid,
		// Token: 0x04000420 RID: 1056
		Md5,
		// Token: 0x04000421 RID: 1057
		UserDefined = 128
	}
}
