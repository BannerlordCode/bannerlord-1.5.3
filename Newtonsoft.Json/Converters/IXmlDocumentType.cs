using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F4 RID: 244
	[NullableContext(2)]
	internal interface IXmlDocumentType : IXmlNode
	{
		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000CC5 RID: 3269
		[Nullable(1)]
		string Name
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000CC6 RID: 3270
		string System { get; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000CC7 RID: 3271
		string Public { get; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000CC8 RID: 3272
		string InternalSubset { get; }
	}
}
