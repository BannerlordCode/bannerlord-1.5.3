using System;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F7 RID: 247
	[NullableContext(2)]
	[Nullable(0)]
	internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x00032CC2 File Offset: 0x00030EC2
		[Nullable(1)]
		internal XDeclaration Declaration
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x00032CCA File Offset: 0x00030ECA
		[NullableContext(1)]
		public XDeclarationWrapper(XDeclaration declaration)
			: base(null)
		{
			this.Declaration = declaration;
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00032CDA File Offset: 0x00030EDA
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.XmlDeclaration;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x00032CDE File Offset: 0x00030EDE
		public string Version
		{
			get
			{
				return this.Declaration.Version;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00032CEB File Offset: 0x00030EEB
		// (set) Token: 0x06000CDB RID: 3291 RVA: 0x00032CF8 File Offset: 0x00030EF8
		public string Encoding
		{
			get
			{
				return this.Declaration.Encoding;
			}
			set
			{
				this.Declaration.Encoding = value;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x00032D06 File Offset: 0x00030F06
		// (set) Token: 0x06000CDD RID: 3293 RVA: 0x00032D13 File Offset: 0x00030F13
		public string Standalone
		{
			get
			{
				return this.Declaration.Standalone;
			}
			set
			{
				this.Declaration.Standalone = value;
			}
		}
	}
}
