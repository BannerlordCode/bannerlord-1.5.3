using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F9 RID: 249
	[NullableContext(1)]
	[Nullable(0)]
	internal class XDocumentWrapper : XContainerWrapper, IXmlDocument, IXmlNode
	{
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x00032D6C File Offset: 0x00030F6C
		private XDocument Document
		{
			get
			{
				return (XDocument)base.WrappedNode;
			}
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00032D79 File Offset: 0x00030F79
		public XDocumentWrapper(XDocument document)
			: base(document)
		{
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x00032D84 File Offset: 0x00030F84
		public override List<IXmlNode> ChildNodes
		{
			get
			{
				List<IXmlNode> childNodes = base.ChildNodes;
				if (this.Document.Declaration != null && (childNodes.Count == 0 || childNodes[0].NodeType != XmlNodeType.XmlDeclaration))
				{
					childNodes.Insert(0, new XDeclarationWrapper(this.Document.Declaration));
				}
				return childNodes;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00032DD5 File Offset: 0x00030FD5
		protected override bool HasChildNodes
		{
			get
			{
				return base.HasChildNodes || this.Document.Declaration != null;
			}
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x00032DEF File Offset: 0x00030FEF
		public IXmlNode CreateComment([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XComment(text));
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00032DFC File Offset: 0x00030FFC
		public IXmlNode CreateTextNode([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x00032E09 File Offset: 0x00031009
		public IXmlNode CreateCDataSection([Nullable(2)] string data)
		{
			return new XObjectWrapper(new XCData(data));
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x00032E16 File Offset: 0x00031016
		public IXmlNode CreateWhitespace([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x00032E23 File Offset: 0x00031023
		public IXmlNode CreateSignificantWhitespace([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00032E30 File Offset: 0x00031030
		public IXmlNode CreateXmlDeclaration(string version, [Nullable(2)] string encoding, [Nullable(2)] string standalone)
		{
			return new XDeclarationWrapper(new XDeclaration(version, encoding, standalone));
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00032E3F File Offset: 0x0003103F
		[NullableContext(2)]
		[return: Nullable(1)]
		public IXmlNode CreateXmlDocumentType([Nullable(1)] string name, string publicId, string systemId, string internalSubset)
		{
			return new XDocumentTypeWrapper(new XDocumentType(name, publicId, systemId, internalSubset));
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00032E50 File Offset: 0x00031050
		public IXmlNode CreateProcessingInstruction(string target, string data)
		{
			return new XProcessingInstructionWrapper(new XProcessingInstruction(target, data));
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00032E5E File Offset: 0x0003105E
		public IXmlElement CreateElement(string elementName)
		{
			return new XElementWrapper(new XElement(elementName));
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00032E70 File Offset: 0x00031070
		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			return new XElementWrapper(new XElement(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceUri)));
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00032E88 File Offset: 0x00031088
		public IXmlNode CreateAttribute(string name, string value)
		{
			return new XAttributeWrapper(new XAttribute(name, value));
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00032E9B File Offset: 0x0003109B
		public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value)
		{
			return new XAttributeWrapper(new XAttribute(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceUri), value));
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x00032EB4 File Offset: 0x000310B4
		[Nullable(2)]
		public IXmlElement DocumentElement
		{
			[NullableContext(2)]
			get
			{
				if (this.Document.Root == null)
				{
					return null;
				}
				return new XElementWrapper(this.Document.Root);
			}
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00032ED8 File Offset: 0x000310D8
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			XDeclarationWrapper xdeclarationWrapper = newChild as XDeclarationWrapper;
			if (xdeclarationWrapper != null)
			{
				this.Document.Declaration = xdeclarationWrapper.Declaration;
				return xdeclarationWrapper;
			}
			return base.AppendChild(newChild);
		}
	}
}
