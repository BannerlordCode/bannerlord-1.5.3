using System;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000C3 RID: 195
	public class ShipSlot : MBObjectBase
	{
		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x00022FD4 File Offset: 0x000211D4
		// (set) Token: 0x06000AC9 RID: 2761 RVA: 0x00022FDC File Offset: 0x000211DC
		public string TypeId { get; private set; }

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x00022FE5 File Offset: 0x000211E5
		// (set) Token: 0x06000ACB RID: 2763 RVA: 0x00022FED File Offset: 0x000211ED
		public string MainPrefabId { get; private set; }

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x00022FF6 File Offset: 0x000211F6
		public MBReadOnlyList<ShipUpgradePiece> MatchingPieces
		{
			get
			{
				return this._matchingPieces;
			}
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x00022FFE File Offset: 0x000211FE
		public ShipSlot()
		{
			this._matchingPieces = new MBList<ShipUpgradePiece>();
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00023011 File Offset: 0x00021211
		public override void AfterRegister()
		{
			base.AfterRegister();
			this.Initialize();
			base.IsReady = true;
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00023026 File Offset: 0x00021226
		public void AddMatchingPiece(ShipUpgradePiece upgradePiece)
		{
			if (!this._matchingPieces.Contains(upgradePiece))
			{
				this._matchingPieces.Add(upgradePiece);
			}
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00023042 File Offset: 0x00021242
		public TextObject GetSlotTypeName()
		{
			return GameTexts.FindText("str_ship_slot_type", this.TypeId);
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00023054 File Offset: 0x00021254
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			XmlAttribute xmlAttribute = node.Attributes["prefab_id"];
			this.MainPrefabId = ((xmlAttribute != null) ? xmlAttribute.Value : null) ?? base.StringId;
			string typeId = this.TypeId;
			XmlAttribute xmlAttribute2 = node.Attributes["type_id"];
			this.TypeId = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null) ?? base.StringId;
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name.Equals("ShipUpgradePieces"))
				{
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.Name.Equals("ShipUpgradePiece"))
						{
							string value = xmlNode2.Attributes["id"].Value;
							ShipUpgradePiece @object = MBObjectManager.Instance.GetObject<ShipUpgradePiece>(value);
							this.AddMatchingPiece(@object);
							@object.AddTargetSlot(this);
						}
					}
				}
			}
		}

		// Token: 0x04000605 RID: 1541
		private MBList<ShipUpgradePiece> _matchingPieces;
	}
}
