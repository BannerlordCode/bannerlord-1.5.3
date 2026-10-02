using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F3 RID: 243
	[NullableContext(2)]
	internal interface IXmlDeclaration : IXmlNode
	{
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000CC0 RID: 3264
		string Version { get; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000CC1 RID: 3265
		// (set) Token: 0x06000CC2 RID: 3266
		string Encoding { get; set; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000CC3 RID: 3267
		// (set) Token: 0x06000CC4 RID: 3268
		string Standalone { get; set; }
	}
}
