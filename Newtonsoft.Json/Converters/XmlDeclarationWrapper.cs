using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000EF RID: 239
	[NullableContext(2)]
	[Nullable(0)]
	internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x06000C99 RID: 3225 RVA: 0x00032962 File Offset: 0x00030B62
		[NullableContext(1)]
		public XmlDeclarationWrapper(XmlDeclaration declaration)
			: base(declaration)
		{
			this._declaration = declaration;
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x00032972 File Offset: 0x00030B72
		public string Version
		{
			get
			{
				return this._declaration.Version;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0003297F File Offset: 0x00030B7F
		// (set) Token: 0x06000C9C RID: 3228 RVA: 0x0003298C File Offset: 0x00030B8C
		public string Encoding
		{
			get
			{
				return this._declaration.Encoding;
			}
			set
			{
				this._declaration.Encoding = value;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x0003299A File Offset: 0x00030B9A
		// (set) Token: 0x06000C9E RID: 3230 RVA: 0x000329A7 File Offset: 0x00030BA7
		public string Standalone
		{
			get
			{
				return this._declaration.Standalone;
			}
			set
			{
				this._declaration.Standalone = value;
			}
		}

		// Token: 0x04000404 RID: 1028
		[Nullable(1)]
		private readonly XmlDeclaration _declaration;
	}
}
