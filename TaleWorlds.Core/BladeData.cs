using System;
using System.Xml;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000049 RID: 73
	public sealed class BladeData : MBObjectBase
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x00015693 File Offset: 0x00013893
		// (set) Token: 0x0600061E RID: 1566 RVA: 0x0001569B File Offset: 0x0001389B
		public DamageTypes ThrustDamageType { get; private set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x000156A4 File Offset: 0x000138A4
		// (set) Token: 0x06000620 RID: 1568 RVA: 0x000156AC File Offset: 0x000138AC
		public float ThrustDamageFactor { get; private set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x000156B5 File Offset: 0x000138B5
		// (set) Token: 0x06000622 RID: 1570 RVA: 0x000156BD File Offset: 0x000138BD
		public DamageTypes SwingDamageType { get; private set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x000156C6 File Offset: 0x000138C6
		// (set) Token: 0x06000624 RID: 1572 RVA: 0x000156CE File Offset: 0x000138CE
		public float SwingDamageFactor { get; private set; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x000156D7 File Offset: 0x000138D7
		// (set) Token: 0x06000626 RID: 1574 RVA: 0x000156DF File Offset: 0x000138DF
		public float BladeLength { get; private set; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x000156E8 File Offset: 0x000138E8
		// (set) Token: 0x06000628 RID: 1576 RVA: 0x000156F0 File Offset: 0x000138F0
		public float BladeWidth { get; private set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x000156F9 File Offset: 0x000138F9
		// (set) Token: 0x0600062A RID: 1578 RVA: 0x00015701 File Offset: 0x00013901
		public short StackAmount { get; private set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x0001570A File Offset: 0x0001390A
		// (set) Token: 0x0600062C RID: 1580 RVA: 0x00015712 File Offset: 0x00013912
		public string PhysicsMaterial { get; private set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x0001571B File Offset: 0x0001391B
		// (set) Token: 0x0600062E RID: 1582 RVA: 0x00015723 File Offset: 0x00013923
		public string BodyName { get; private set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x0001572C File Offset: 0x0001392C
		// (set) Token: 0x06000630 RID: 1584 RVA: 0x00015734 File Offset: 0x00013934
		public string HolsterMeshName { get; private set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x0001573D File Offset: 0x0001393D
		// (set) Token: 0x06000632 RID: 1586 RVA: 0x00015745 File Offset: 0x00013945
		public string HolsterBodyName { get; private set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x0001574E File Offset: 0x0001394E
		// (set) Token: 0x06000634 RID: 1588 RVA: 0x00015756 File Offset: 0x00013956
		public float HolsterMeshLength { get; private set; }

		// Token: 0x06000635 RID: 1589 RVA: 0x0001575F File Offset: 0x0001395F
		public BladeData(CraftingPiece.PieceTypes pieceType, float bladeLength)
		{
			this.PieceType = pieceType;
			this.BladeLength = bladeLength;
			this.ThrustDamageType = DamageTypes.Invalid;
			this.SwingDamageType = DamageTypes.Invalid;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00015784 File Offset: 0x00013984
		public override void Deserialize(MBObjectManager objectManager, XmlNode childNode)
		{
			this.Initialize();
			XmlAttribute xmlAttribute = childNode.Attributes["stack_amount"];
			XmlAttribute xmlAttribute2 = childNode.Attributes["blade_length"];
			XmlAttribute xmlAttribute3 = childNode.Attributes["blade_width"];
			XmlAttribute xmlAttribute4 = childNode.Attributes["physics_material"];
			XmlAttribute xmlAttribute5 = childNode.Attributes["body_name"];
			XmlAttribute xmlAttribute6 = childNode.Attributes["holster_mesh"];
			XmlAttribute xmlAttribute7 = childNode.Attributes["holster_body_name"];
			XmlAttribute xmlAttribute8 = childNode.Attributes["holster_mesh_length"];
			this.StackAmount = ((xmlAttribute != null) ? short.Parse(xmlAttribute.Value) : 1);
			this.BladeLength = ((xmlAttribute2 != null) ? (0.01f * float.Parse(xmlAttribute2.Value)) : this.BladeLength);
			this.BladeWidth = ((xmlAttribute3 != null) ? (0.01f * float.Parse(xmlAttribute3.Value)) : (0.15f + this.BladeLength * 0.3f));
			this.PhysicsMaterial = ((xmlAttribute4 != null) ? xmlAttribute4.InnerText : null);
			this.BodyName = ((xmlAttribute5 != null) ? xmlAttribute5.InnerText : null);
			this.HolsterMeshName = ((xmlAttribute6 != null) ? xmlAttribute6.InnerText : null);
			this.HolsterBodyName = ((xmlAttribute7 != null) ? xmlAttribute7.InnerText : null);
			this.HolsterMeshLength = 0.01f * ((xmlAttribute8 != null) ? float.Parse(xmlAttribute8.Value) : 0f);
			foreach (object obj in childNode.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				string name = xmlNode.Name;
				if (!(name == "Thrust"))
				{
					if (name == "Swing")
					{
						XmlAttribute xmlAttribute9 = xmlNode.Attributes["damage_type"];
						XmlAttribute xmlAttribute10 = xmlNode.Attributes["damage_factor"];
						this.SwingDamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), xmlAttribute9.Value, true);
						this.SwingDamageFactor = float.Parse(xmlAttribute10.Value);
					}
				}
				else
				{
					XmlAttribute xmlAttribute11 = xmlNode.Attributes["damage_type"];
					XmlAttribute xmlAttribute12 = xmlNode.Attributes["damage_factor"];
					this.ThrustDamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), xmlAttribute11.Value, true);
					this.ThrustDamageFactor = float.Parse(xmlAttribute12.Value);
				}
			}
		}

		// Token: 0x040002DE RID: 734
		public readonly CraftingPiece.PieceTypes PieceType;
	}
}
