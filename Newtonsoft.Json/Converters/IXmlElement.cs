using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F5 RID: 245
	[NullableContext(1)]
	internal interface IXmlElement : IXmlNode
	{
		// Token: 0x06000CC9 RID: 3273
		void SetAttributeNode(IXmlNode attribute);

		// Token: 0x06000CCA RID: 3274
		[return: Nullable(2)]
		string GetPrefixOfNamespace(string namespaceUri);

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000CCB RID: 3275
		bool IsEmpty { get; }
	}
}
