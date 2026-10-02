using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FE RID: 254
	[NullableContext(2)]
	[Nullable(0)]
	internal class XObjectWrapper : IXmlNode
	{
		// Token: 0x06000D0C RID: 3340 RVA: 0x0003319E File Offset: 0x0003139E
		public XObjectWrapper(XObject xmlObject)
		{
			this._xmlObject = xmlObject;
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x000331AD File Offset: 0x000313AD
		public object WrappedNode
		{
			get
			{
				return this._xmlObject;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x000331B5 File Offset: 0x000313B5
		public virtual XmlNodeType NodeType
		{
			get
			{
				XObject xmlObject = this._xmlObject;
				if (xmlObject == null)
				{
					return XmlNodeType.None;
				}
				return xmlObject.NodeType;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x000331C8 File Offset: 0x000313C8
		public virtual string LocalName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x000331CB File Offset: 0x000313CB
		[Nullable(1)]
		public virtual List<IXmlNode> ChildNodes
		{
			[NullableContext(1)]
			get
			{
				return XmlNodeConverter.EmptyChildNodes;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x000331D2 File Offset: 0x000313D2
		[Nullable(1)]
		public virtual List<IXmlNode> Attributes
		{
			[NullableContext(1)]
			get
			{
				return XmlNodeConverter.EmptyChildNodes;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x000331D9 File Offset: 0x000313D9
		public virtual IXmlNode ParentNode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x000331DC File Offset: 0x000313DC
		// (set) Token: 0x06000D14 RID: 3348 RVA: 0x000331DF File Offset: 0x000313DF
		public virtual string Value
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x000331E6 File Offset: 0x000313E6
		[NullableContext(1)]
		public virtual IXmlNode AppendChild(IXmlNode newChild)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x000331ED File Offset: 0x000313ED
		public virtual string NamespaceUri
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0400040C RID: 1036
		private readonly XObject _xmlObject;
	}
}
