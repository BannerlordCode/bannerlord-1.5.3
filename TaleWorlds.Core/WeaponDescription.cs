using System;
using System.Collections;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x0200004B RID: 75
	public class WeaponDescription : MBObjectBase
	{
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x00015A6E File Offset: 0x00013C6E
		// (set) Token: 0x0600063D RID: 1597 RVA: 0x00015A76 File Offset: 0x00013C76
		public WeaponClass WeaponClass { get; private set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x00015A7F File Offset: 0x00013C7F
		// (set) Token: 0x0600063F RID: 1599 RVA: 0x00015A87 File Offset: 0x00013C87
		public WeaponFlags WeaponFlags { get; private set; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x00015A90 File Offset: 0x00013C90
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x00015A98 File Offset: 0x00013C98
		public string ItemUsageFeatures { get; private set; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x00015AA1 File Offset: 0x00013CA1
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x00015AA9 File Offset: 0x00013CA9
		public bool RotatedInHand { get; private set; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x00015AB2 File Offset: 0x00013CB2
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x00015ABA File Offset: 0x00013CBA
		public bool IsHiddenFromUI { get; set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x00015AC3 File Offset: 0x00013CC3
		public MBReadOnlyList<CraftingPiece> AvailablePieces
		{
			get
			{
				return this._availablePieces;
			}
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00015ACC File Offset: 0x00013CCC
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.WeaponClass = ((node.Attributes["weapon_class"] != null) ? ((WeaponClass)Enum.Parse(typeof(WeaponClass), node.Attributes["weapon_class"].Value)) : WeaponClass.Undefined);
			this.ItemUsageFeatures = ((node.Attributes["item_usage_features"] != null) ? node.Attributes["item_usage_features"].Value : "");
			this.RotatedInHand = XmlHelper.ReadBool(node, "rotated_in_hand");
			this.UseCenterOfMassAsHandBase = XmlHelper.ReadBool(node, "use_center_of_mass_as_hand_base");
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "WeaponFlags")
				{
					using (IEnumerator enumerator2 = xmlNode.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode xmlNode2 = (XmlNode)obj2;
							this.WeaponFlags |= (WeaponFlags)Enum.Parse(typeof(WeaponFlags), xmlNode2.Attributes["value"].Value);
						}
						continue;
					}
				}
				if (xmlNode.Name == "AvailablePieces")
				{
					this._availablePieces = new MBList<CraftingPiece>();
					foreach (object obj3 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode3 = (XmlNode)obj3;
						if (xmlNode3.NodeType == XmlNodeType.Element)
						{
							string value = xmlNode3.Attributes["id"].Value;
							CraftingPiece @object = MBObjectManager.Instance.GetObject<CraftingPiece>(value);
							if (@object != null)
							{
								this._availablePieces.Add(@object);
							}
						}
					}
				}
			}
		}

		// Token: 0x040002F1 RID: 753
		public bool UseCenterOfMassAsHandBase;

		// Token: 0x040002F3 RID: 755
		private MBList<CraftingPiece> _availablePieces;
	}
}
