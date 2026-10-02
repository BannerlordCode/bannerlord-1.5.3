using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000EE RID: 238
	[NullableContext(1)]
	[Nullable(0)]
	internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement, IXmlNode
	{
		// Token: 0x06000C95 RID: 3221 RVA: 0x00032909 File Offset: 0x00030B09
		public XmlElementWrapper(XmlElement element)
			: base(element)
		{
			this._element = element;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0003291C File Offset: 0x00030B1C
		public void SetAttributeNode(IXmlNode attribute)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)attribute;
			this._element.SetAttributeNode((XmlAttribute)xmlNodeWrapper.WrappedNode);
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x00032947 File Offset: 0x00030B47
		[return: Nullable(2)]
		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return this._element.GetPrefixOfNamespace(namespaceUri);
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x00032955 File Offset: 0x00030B55
		public bool IsEmpty
		{
			get
			{
				return this._element.IsEmpty;
			}
		}

		// Token: 0x04000403 RID: 1027
		private readonly XmlElement _element;
	}
}
