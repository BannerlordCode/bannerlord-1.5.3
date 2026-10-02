using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000ED RID: 237
	[NullableContext(1)]
	[Nullable(0)]
	internal class XmlDocumentWrapper : XmlNodeWrapper, IXmlDocument, IXmlNode
	{
		// Token: 0x06000C87 RID: 3207 RVA: 0x000327DE File Offset: 0x000309DE
		public XmlDocumentWrapper(XmlDocument document)
			: base(document)
		{
			this._document = document;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x000327EE File Offset: 0x000309EE
		public IXmlNode CreateComment([Nullable(2)] string data)
		{
			return new XmlNodeWrapper(this._document.CreateComment(data));
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00032801 File Offset: 0x00030A01
		public IXmlNode CreateTextNode([Nullable(2)] string text)
		{
			return new XmlNodeWrapper(this._document.CreateTextNode(text));
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00032814 File Offset: 0x00030A14
		public IXmlNode CreateCDataSection([Nullable(2)] string data)
		{
			return new XmlNodeWrapper(this._document.CreateCDataSection(data));
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00032827 File Offset: 0x00030A27
		public IXmlNode CreateWhitespace([Nullable(2)] string text)
		{
			return new XmlNodeWrapper(this._document.CreateWhitespace(text));
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0003283A File Offset: 0x00030A3A
		public IXmlNode CreateSignificantWhitespace([Nullable(2)] string text)
		{
			return new XmlNodeWrapper(this._document.CreateSignificantWhitespace(text));
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0003284D File Offset: 0x00030A4D
		public IXmlNode CreateXmlDeclaration(string version, [Nullable(2)] string encoding, [Nullable(2)] string standalone)
		{
			return new XmlDeclarationWrapper(this._document.CreateXmlDeclaration(version, encoding, standalone));
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x00032862 File Offset: 0x00030A62
		[NullableContext(2)]
		[return: Nullable(1)]
		public IXmlNode CreateXmlDocumentType([Nullable(1)] string name, string publicId, string systemId, string internalSubset)
		{
			return new XmlDocumentTypeWrapper(this._document.CreateDocumentType(name, publicId, systemId, null));
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00032878 File Offset: 0x00030A78
		public IXmlNode CreateProcessingInstruction(string target, string data)
		{
			return new XmlNodeWrapper(this._document.CreateProcessingInstruction(target, data));
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0003288C File Offset: 0x00030A8C
		public IXmlElement CreateElement(string elementName)
		{
			return new XmlElementWrapper(this._document.CreateElement(elementName));
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0003289F File Offset: 0x00030A9F
		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			return new XmlElementWrapper(this._document.CreateElement(qualifiedName, namespaceUri));
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x000328B3 File Offset: 0x00030AB3
		public IXmlNode CreateAttribute(string name, [Nullable(2)] string value)
		{
			return new XmlNodeWrapper(this._document.CreateAttribute(name))
			{
				Value = value
			};
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x000328CD File Offset: 0x00030ACD
		public IXmlNode CreateAttribute(string qualifiedName, [Nullable(2)] string namespaceUri, [Nullable(2)] string value)
		{
			return new XmlNodeWrapper(this._document.CreateAttribute(qualifiedName, namespaceUri))
			{
				Value = value
			};
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x000328E8 File Offset: 0x00030AE8
		[Nullable(2)]
		public IXmlElement DocumentElement
		{
			[NullableContext(2)]
			get
			{
				if (this._document.DocumentElement == null)
				{
					return null;
				}
				return new XmlElementWrapper(this._document.DocumentElement);
			}
		}

		// Token: 0x04000402 RID: 1026
		private readonly XmlDocument _document;
	}
}
