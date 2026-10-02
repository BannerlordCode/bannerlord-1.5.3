using System;
using System.Xml;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x0200001B RID: 27
	public class BasicCultureObject : MBObjectBase
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00006607 File Offset: 0x00004807
		// (set) Token: 0x06000168 RID: 360 RVA: 0x0000660F File Offset: 0x0000480F
		public TextObject Name { get; private set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00006618 File Offset: 0x00004818
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00006620 File Offset: 0x00004820
		public bool IsMainCulture { get; private set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00006629 File Offset: 0x00004829
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00006631 File Offset: 0x00004831
		public bool IsBandit { get; private set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000663A File Offset: 0x0000483A
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00006642 File Offset: 0x00004842
		public bool CanHaveSettlement { get; private set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000664B File Offset: 0x0000484B
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00006653 File Offset: 0x00004853
		public uint Color { get; private set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000171 RID: 369 RVA: 0x0000665C File Offset: 0x0000485C
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00006664 File Offset: 0x00004864
		public uint Color2 { get; private set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000173 RID: 371 RVA: 0x0000666D File Offset: 0x0000486D
		// (set) Token: 0x06000174 RID: 372 RVA: 0x00006675 File Offset: 0x00004875
		public uint ClothAlternativeColor { get; private set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000175 RID: 373 RVA: 0x0000667E File Offset: 0x0000487E
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00006686 File Offset: 0x00004886
		public uint ClothAlternativeColor2 { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000668F File Offset: 0x0000488F
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00006697 File Offset: 0x00004897
		public uint BackgroundColor1 { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000066A0 File Offset: 0x000048A0
		// (set) Token: 0x0600017A RID: 378 RVA: 0x000066A8 File Offset: 0x000048A8
		public uint ForegroundColor1 { get; private set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600017B RID: 379 RVA: 0x000066B1 File Offset: 0x000048B1
		// (set) Token: 0x0600017C RID: 380 RVA: 0x000066B9 File Offset: 0x000048B9
		public uint BackgroundColor2 { get; private set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600017D RID: 381 RVA: 0x000066C2 File Offset: 0x000048C2
		// (set) Token: 0x0600017E RID: 382 RVA: 0x000066CA File Offset: 0x000048CA
		public uint ForegroundColor2 { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600017F RID: 383 RVA: 0x000066D3 File Offset: 0x000048D3
		// (set) Token: 0x06000180 RID: 384 RVA: 0x000066DB File Offset: 0x000048DB
		public string EncounterBackgroundMesh { get; set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000181 RID: 385 RVA: 0x000066E4 File Offset: 0x000048E4
		// (set) Token: 0x06000182 RID: 386 RVA: 0x000066EC File Offset: 0x000048EC
		public Banner Banner { get; private set; }

		// Token: 0x06000183 RID: 387 RVA: 0x000066F5 File Offset: 0x000048F5
		public override string ToString()
		{
			return this.Name.ToString();
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00006704 File Offset: 0x00004904
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.Name = new TextObject(node.Attributes["name"].Value, null);
			this.Color = ((node.Attributes["color"] == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes["color"].Value, 16));
			this.Color2 = ((node.Attributes["color2"] == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes["color2"].Value, 16));
			this.ClothAlternativeColor = ((node.Attributes["cloth_alternative_color1"] == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes["cloth_alternative_color1"].Value, 16));
			this.ClothAlternativeColor2 = ((node.Attributes["cloth_alternative_color2"] == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes["cloth_alternative_color2"].Value, 16));
			this.BackgroundColor1 = ((node.Attributes["banner_background_color1"] == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes["banner_background_color1"].Value, 16));
			this.ForegroundColor1 = ((node.Attributes["banner_foreground_color1"] == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes["banner_foreground_color1"].Value, 16));
			this.BackgroundColor2 = ((node.Attributes["banner_background_color2"] == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes["banner_background_color2"].Value, 16));
			this.ForegroundColor2 = ((node.Attributes["banner_foreground_color2"] == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes["banner_foreground_color2"].Value, 16));
			this.IsMainCulture = node.Attributes["is_main_culture"] != null && Convert.ToBoolean(node.Attributes["is_main_culture"].Value);
			this.EncounterBackgroundMesh = ((node.Attributes["encounter_background_mesh"] == null) ? null : node.Attributes["encounter_background_mesh"].Value);
			this.Banner = ((node.Attributes["faction_banner_key"] == null) ? new Banner() : new Banner(node.Attributes["faction_banner_key"].Value));
			this.IsBandit = false;
			this.IsBandit = node.Attributes["is_bandit"] != null && Convert.ToBoolean(node.Attributes["is_bandit"].Value);
			this.CanHaveSettlement = false;
			this.CanHaveSettlement = node.Attributes["can_have_settlement"] != null && Convert.ToBoolean(node.Attributes["can_have_settlement"].Value);
		}
	}
}
