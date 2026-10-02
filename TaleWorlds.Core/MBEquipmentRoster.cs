using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x0200009F RID: 159
	public class MBEquipmentRoster : MBObjectBase
	{
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x0001D952 File Offset: 0x0001BB52
		// (set) Token: 0x06000901 RID: 2305 RVA: 0x0001D95A File Offset: 0x0001BB5A
		public BasicCultureObject EquipmentCulture { get; private set; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x0001D963 File Offset: 0x0001BB63
		// (set) Token: 0x06000903 RID: 2307 RVA: 0x0001D96B File Offset: 0x0001BB6B
		public EquipmentCategories EquipmentCategories { get; private set; }

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x0001D974 File Offset: 0x0001BB74
		public MBReadOnlyList<Equipment> AllEquipments
		{
			get
			{
				if (this._equipments.IsEmpty<Equipment>())
				{
					return new MBList<Equipment>(1) { MBEquipmentRoster.EmptyEquipment };
				}
				return this._equipments;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x0001D99B File Offset: 0x0001BB9B
		public Equipment DefaultEquipment
		{
			get
			{
				if (!this._equipments.IsEmpty<Equipment>())
				{
					return this._equipments.FirstOrDefault<Equipment>();
				}
				return MBEquipmentRoster.EmptyEquipment;
			}
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0001D9BB File Offset: 0x0001BBBB
		public void Init(MBObjectManager objectManager, XmlNode node)
		{
			if (node.Name == "EquipmentRoster")
			{
				this.InitEquipment(objectManager, node);
				return;
			}
			Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\MBEquipmentRoster.cs", "Init", 78);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0001D9F0 File Offset: 0x0001BBF0
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			if (node.Attributes["culture"] != null)
			{
				this.EquipmentCulture = MBObjectManager.Instance.ReadObjectReferenceFromXml<BasicCultureObject>("culture", node);
			}
			if (this.EquipmentCulture == null)
			{
				Debug.Print("EquipmentRoster with id: " + base.StringId + " don't have culture definition, make sure this is intended", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "EquipmentSet")
				{
					this.InitEquipment(objectManager, xmlNode);
				}
				if (xmlNode.Name == "Flags")
				{
					foreach (object obj2 in xmlNode.Attributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj2;
						EquipmentCategories equipmentCategories = (EquipmentCategories)Enum.Parse(typeof(EquipmentCategories), xmlAttribute.Name);
						if (bool.Parse(xmlAttribute.InnerText))
						{
							this.EquipmentCategories |= equipmentCategories;
						}
					}
				}
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0001DB54 File Offset: 0x0001BD54
		private void InitEquipment(MBObjectManager objectManager, XmlNode node)
		{
			base.Initialize();
			Equipment.EquipmentType equipmentType = Equipment.EquipmentType.Battle;
			if (node.Attributes["equipmentType"] != null)
			{
				if (!Enum.TryParse<Equipment.EquipmentType>(node.Attributes["equipmentType"].Value, out equipmentType))
				{
					Debug.FailedAssert("This equipment definition is wrong", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\MBEquipmentRoster.cs", "InitEquipment", 128);
				}
			}
			else if (node.Attributes["civilian"] != null && bool.Parse(node.Attributes["civilian"].Value))
			{
				equipmentType = Equipment.EquipmentType.Civilian;
				node.Name == "EquipmentSet";
			}
			Equipment equipment = new Equipment(equipmentType);
			equipment.Deserialize(objectManager, node);
			this._equipments.Add(equipment);
			base.AfterInitialized();
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0001DC18 File Offset: 0x0001BE18
		public void AddEquipmentRoster(MBEquipmentRoster equipmentRoster, Equipment.EquipmentType equipmentType)
		{
			foreach (Equipment equipment in equipmentRoster._equipments.ToList<Equipment>())
			{
				if ((equipmentType == Equipment.EquipmentType.Stealth && equipment.IsStealth) || (equipmentType == Equipment.EquipmentType.Civilian && equipment.IsCivilian) || (equipmentType == Equipment.EquipmentType.Battle && equipment.IsBattle))
				{
					this._equipments.Add(equipment);
				}
			}
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0001DC98 File Offset: 0x0001BE98
		public void AddOverriddenEquipments(MBObjectManager objectManager, List<XmlNode> overridenEquipmentSlots)
		{
			List<Equipment> list = this._equipments.ToList<Equipment>();
			this._equipments.Clear();
			foreach (Equipment equipment in list)
			{
				this._equipments.Add(equipment.Clone(false));
			}
			foreach (XmlNode xmlNode in overridenEquipmentSlots)
			{
				foreach (Equipment equipment2 in this._equipments)
				{
					equipment2.DeserializeNode(objectManager, xmlNode);
				}
			}
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0001DD80 File Offset: 0x0001BF80
		public void OrderEquipments()
		{
			this._equipments = new MBList<Equipment>(this._equipments.OrderByDescending<Equipment, bool>((Equipment eq) => !eq.IsCivilian && !eq.IsStealth));
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0001DDB7 File Offset: 0x0001BFB7
		public void InitializeDefaultEquipment(string equipmentName)
		{
			if (this._equipments[0] == null)
			{
				this._equipments[0] = new Equipment(Equipment.EquipmentType.Battle);
			}
			this._equipments[0].FillFrom(Game.Current.GetDefaultEquipmentWithName(equipmentName), true);
		}

		// Token: 0x04000513 RID: 1299
		public static readonly Equipment EmptyEquipment = new Equipment(Equipment.EquipmentType.Civilian);

		// Token: 0x04000514 RID: 1300
		private MBList<Equipment> _equipments = new MBList<Equipment>();
	}
}
