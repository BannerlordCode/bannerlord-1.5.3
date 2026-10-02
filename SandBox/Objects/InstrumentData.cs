using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace SandBox.Objects
{
	// Token: 0x02000039 RID: 57
	public class InstrumentData : MBObjectBase
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000CDF3 File Offset: 0x0000AFF3
		public MBReadOnlyList<ValueTuple<HumanBone, string>> InstrumentEntities
		{
			get
			{
				return this._instrumentEntities;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000CDFB File Offset: 0x0000AFFB
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000CE03 File Offset: 0x0000B003
		public string SittingAction { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000CE0C File Offset: 0x0000B00C
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000CE14 File Offset: 0x0000B014
		public string StandingAction { get; private set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000CE1D File Offset: 0x0000B01D
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000CE25 File Offset: 0x0000B025
		public string Tag { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000CE2E File Offset: 0x0000B02E
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000CE36 File Offset: 0x0000B036
		public bool IsDataWithoutInstrument { get; private set; }

		// Token: 0x06000202 RID: 514 RVA: 0x0000CE3F File Offset: 0x0000B03F
		public InstrumentData()
		{
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000CE47 File Offset: 0x0000B047
		public InstrumentData(string stringId)
			: base(stringId)
		{
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000CE50 File Offset: 0x0000B050
		public void InitializeInstrumentData(string sittingAction, string standingAction, bool isDataWithoutInstrument)
		{
			this.SittingAction = sittingAction;
			this.StandingAction = standingAction;
			this._instrumentEntities = new MBList<ValueTuple<HumanBone, string>>(0);
			this.IsDataWithoutInstrument = isDataWithoutInstrument;
			this.Tag = string.Empty;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000CE80 File Offset: 0x0000B080
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.SittingAction = Convert.ToString(node.Attributes["sittingAction"].Value);
			this.StandingAction = Convert.ToString(node.Attributes["standingAction"].Value);
			XmlAttribute xmlAttribute = node.Attributes["tag"];
			this.Tag = Convert.ToString((xmlAttribute != null) ? xmlAttribute.Value : null);
			this._instrumentEntities = new MBList<ValueTuple<HumanBone, string>>();
			if (node.HasChildNodes)
			{
				foreach (object obj in node.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (xmlNode.Name == "Entities")
					{
						foreach (object obj2 in xmlNode.ChildNodes)
						{
							XmlNode xmlNode2 = (XmlNode)obj2;
							if (xmlNode2.Name == "Entity")
							{
								XmlAttributeCollection attributes = xmlNode2.Attributes;
								if (((attributes != null) ? attributes["name"] : null) != null && xmlNode2.Attributes["bone"] != null)
								{
									string text = Convert.ToString(xmlNode2.Attributes["name"].Value);
									HumanBone humanBone;
									if (Enum.TryParse<HumanBone>(xmlNode2.Attributes["bone"].Value, out humanBone))
									{
										this._instrumentEntities.Add(new ValueTuple<HumanBone, string>(humanBone, text));
									}
									else
									{
										Debug.FailedAssert("Couldn't parse bone xml node for instrument.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Objects\\InstrumentData.cs", "Deserialize", 62);
									}
								}
								else
								{
									Debug.FailedAssert("Couldn't find required attributes of entity xml node in Instrument", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Objects\\InstrumentData.cs", "Deserialize", 67);
								}
							}
						}
					}
				}
			}
			this._instrumentEntities.Capacity = this._instrumentEntities.Count;
		}

		// Token: 0x040000BC RID: 188
		private MBList<ValueTuple<HumanBone, string>> _instrumentEntities;
	}
}
