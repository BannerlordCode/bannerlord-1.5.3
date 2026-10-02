using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F2 RID: 242
	[NullableContext(1)]
	internal interface IXmlDocument : IXmlNode
	{
		// Token: 0x06000CB3 RID: 3251
		IXmlNode CreateComment([Nullable(2)] string text);

		// Token: 0x06000CB4 RID: 3252
		IXmlNode CreateTextNode([Nullable(2)] string text);

		// Token: 0x06000CB5 RID: 3253
		IXmlNode CreateCDataSection([Nullable(2)] string data);

		// Token: 0x06000CB6 RID: 3254
		IXmlNode CreateWhitespace([Nullable(2)] string text);

		// Token: 0x06000CB7 RID: 3255
		IXmlNode CreateSignificantWhitespace([Nullable(2)] string text);

		// Token: 0x06000CB8 RID: 3256
		IXmlNode CreateXmlDeclaration(string version, [Nullable(2)] string encoding, [Nullable(2)] string standalone);

		// Token: 0x06000CB9 RID: 3257
		[NullableContext(2)]
		[return: Nullable(1)]
		IXmlNode CreateXmlDocumentType([Nullable(1)] string name, string publicId, string systemId, string internalSubset);

		// Token: 0x06000CBA RID: 3258
		IXmlNode CreateProcessingInstruction(string target, string data);

		// Token: 0x06000CBB RID: 3259
		IXmlElement CreateElement(string elementName);

		// Token: 0x06000CBC RID: 3260
		IXmlElement CreateElement(string qualifiedName, string namespaceUri);

		// Token: 0x06000CBD RID: 3261
		IXmlNode CreateAttribute(string name, string value);

		// Token: 0x06000CBE RID: 3262
		IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value);

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000CBF RID: 3263
		[Nullable(2)]
		IXmlElement DocumentElement
		{
			[NullableContext(2)]
			get;
		}
	}
}
