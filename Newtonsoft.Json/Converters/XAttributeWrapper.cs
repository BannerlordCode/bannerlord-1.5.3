using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FF RID: 255
	[NullableContext(2)]
	[Nullable(0)]
	internal class XAttributeWrapper : XObjectWrapper
	{
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x000331F0 File Offset: 0x000313F0
		[Nullable(1)]
		private XAttribute Attribute
		{
			[NullableContext(1)]
			get
			{
				return (XAttribute)base.WrappedNode;
			}
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x000331FD File Offset: 0x000313FD
		[NullableContext(1)]
		public XAttributeWrapper(XAttribute attribute)
			: base(attribute)
		{
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x00033206 File Offset: 0x00031406
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x00033213 File Offset: 0x00031413
		public override string Value
		{
			get
			{
				return this.Attribute.Value;
			}
			set
			{
				this.Attribute.Value = value ?? string.Empty;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x0003322A File Offset: 0x0003142A
		public override string LocalName
		{
			get
			{
				return this.Attribute.Name.LocalName;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000D1C RID: 3356 RVA: 0x0003323C File Offset: 0x0003143C
		public override string NamespaceUri
		{
			get
			{
				return this.Attribute.Name.NamespaceName;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x0003324E File Offset: 0x0003144E
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Attribute.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Attribute.Parent);
			}
		}
	}
}
