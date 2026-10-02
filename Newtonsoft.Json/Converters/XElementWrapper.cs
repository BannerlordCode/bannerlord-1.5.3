using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000100 RID: 256
	[NullableContext(1)]
	[Nullable(0)]
	internal class XElementWrapper : XContainerWrapper, IXmlElement, IXmlNode
	{
		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x0003326F File Offset: 0x0003146F
		private XElement Element
		{
			get
			{
				return (XElement)base.WrappedNode;
			}
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0003327C File Offset: 0x0003147C
		public XElementWrapper(XElement element)
			: base(element)
		{
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00033288 File Offset: 0x00031488
		public void SetAttributeNode(IXmlNode attribute)
		{
			XObjectWrapper xobjectWrapper = (XObjectWrapper)attribute;
			this.Element.Add(xobjectWrapper.WrappedNode);
			this._attributes = null;
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x000332B4 File Offset: 0x000314B4
		public override List<IXmlNode> Attributes
		{
			get
			{
				if (this._attributes == null)
				{
					if (!this.Element.HasAttributes && !this.HasImplicitNamespaceAttribute(this.NamespaceUri))
					{
						this._attributes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._attributes = new List<IXmlNode>();
						foreach (XAttribute xattribute in this.Element.Attributes())
						{
							this._attributes.Add(new XAttributeWrapper(xattribute));
						}
						string namespaceUri = this.NamespaceUri;
						if (this.HasImplicitNamespaceAttribute(namespaceUri))
						{
							this._attributes.Insert(0, new XAttributeWrapper(new XAttribute("xmlns", namespaceUri)));
						}
					}
				}
				return this._attributes;
			}
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00033388 File Offset: 0x00031588
		private bool HasImplicitNamespaceAttribute(string namespaceUri)
		{
			if (!StringUtils.IsNullOrEmpty(namespaceUri))
			{
				IXmlNode parentNode = this.ParentNode;
				if (namespaceUri != ((parentNode != null) ? parentNode.NamespaceUri : null) && StringUtils.IsNullOrEmpty(this.GetPrefixOfNamespace(namespaceUri)))
				{
					bool flag = false;
					if (this.Element.HasAttributes)
					{
						foreach (XAttribute xattribute in this.Element.Attributes())
						{
							if (xattribute.Name.LocalName == "xmlns" && StringUtils.IsNullOrEmpty(xattribute.Name.NamespaceName) && xattribute.Value == namespaceUri)
							{
								flag = true;
							}
						}
					}
					if (!flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00033458 File Offset: 0x00031658
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			IXmlNode xmlNode = base.AppendChild(newChild);
			this._attributes = null;
			return xmlNode;
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x00033468 File Offset: 0x00031668
		// (set) Token: 0x06000D25 RID: 3365 RVA: 0x00033475 File Offset: 0x00031675
		[Nullable(2)]
		public override string Value
		{
			[NullableContext(2)]
			get
			{
				return this.Element.Value;
			}
			[NullableContext(2)]
			set
			{
				this.Element.Value = value ?? string.Empty;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000D26 RID: 3366 RVA: 0x0003348C File Offset: 0x0003168C
		[Nullable(2)]
		public override string LocalName
		{
			[NullableContext(2)]
			get
			{
				return this.Element.Name.LocalName;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x0003349E File Offset: 0x0003169E
		[Nullable(2)]
		public override string NamespaceUri
		{
			[NullableContext(2)]
			get
			{
				return this.Element.Name.NamespaceName;
			}
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x000334B0 File Offset: 0x000316B0
		[return: Nullable(2)]
		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return this.Element.GetPrefixOfNamespace(namespaceUri);
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x000334C3 File Offset: 0x000316C3
		public bool IsEmpty
		{
			get
			{
				return this.Element.IsEmpty;
			}
		}

		// Token: 0x0400040D RID: 1037
		[Nullable(new byte[] { 2, 1 })]
		private List<IXmlNode> _attributes;
	}
}
