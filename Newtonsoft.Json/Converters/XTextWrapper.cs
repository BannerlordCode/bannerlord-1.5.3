using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FA RID: 250
	[NullableContext(2)]
	[Nullable(0)]
	internal class XTextWrapper : XObjectWrapper
	{
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x00032F09 File Offset: 0x00031109
		[Nullable(1)]
		private XText Text
		{
			[NullableContext(1)]
			get
			{
				return (XText)base.WrappedNode;
			}
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x00032F16 File Offset: 0x00031116
		[NullableContext(1)]
		public XTextWrapper(XText text)
			: base(text)
		{
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x00032F1F File Offset: 0x0003111F
		// (set) Token: 0x06000CF9 RID: 3321 RVA: 0x00032F2C File Offset: 0x0003112C
		public override string Value
		{
			get
			{
				return this.Text.Value;
			}
			set
			{
				this.Text.Value = value ?? string.Empty;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x00032F43 File Offset: 0x00031143
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Text.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Text.Parent);
			}
		}
	}
}
