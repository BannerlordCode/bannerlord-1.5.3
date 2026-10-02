using System;
using System.Xml;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace SandBox.Objects
{
	// Token: 0x0200003C RID: 60
	public class SettlementMusicData : MBObjectBase
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000D793 File Offset: 0x0000B993
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0000D79B File Offset: 0x0000B99B
		public string MusicPath { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000D7A4 File Offset: 0x0000B9A4
		// (set) Token: 0x06000228 RID: 552 RVA: 0x0000D7AC File Offset: 0x0000B9AC
		public CultureObject Culture { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000D7B5 File Offset: 0x0000B9B5
		public MBReadOnlyList<InstrumentData> Instruments
		{
			get
			{
				return this._instruments;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000D7BD File Offset: 0x0000B9BD
		// (set) Token: 0x0600022B RID: 555 RVA: 0x0000D7C5 File Offset: 0x0000B9C5
		public string LocationId { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000D7CE File Offset: 0x0000B9CE
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0000D7D6 File Offset: 0x0000B9D6
		public int Tempo { get; private set; }

		// Token: 0x0600022E RID: 558 RVA: 0x0000D7E0 File Offset: 0x0000B9E0
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.MusicPath = Convert.ToString(node.Attributes["event_id"].Value);
			this.Culture = Game.Current.ObjectManager.ReadObjectReferenceFromXml<CultureObject>("culture", node);
			this.LocationId = Convert.ToString(node.Attributes["location"].Value);
			this.Tempo = Convert.ToInt32(node.Attributes["tempo"].Value);
			this._instruments = new MBList<InstrumentData>();
			if (node.HasChildNodes)
			{
				foreach (object obj in node.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (xmlNode.Name == "Instruments")
					{
						foreach (object obj2 in xmlNode.ChildNodes)
						{
							XmlNode xmlNode2 = (XmlNode)obj2;
							if (xmlNode2.Name == "Instrument")
							{
								XmlAttributeCollection attributes = xmlNode2.Attributes;
								if (((attributes != null) ? attributes["id"] : null) != null)
								{
									string text = Convert.ToString(xmlNode2.Attributes["id"].Value);
									InstrumentData @object = MBObjectManager.Instance.GetObject<InstrumentData>(text);
									if (@object != null)
									{
										this._instruments.Add(@object);
									}
								}
								else
								{
									Debug.FailedAssert("Couldn't find required attributes of instrument xml node in Track", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Objects\\SettlementMusicData.cs", "Deserialize", 57);
								}
							}
						}
					}
				}
			}
			this._instruments.Capacity = this._instruments.Count;
		}

		// Token: 0x040000D8 RID: 216
		private MBList<InstrumentData> _instruments;
	}
}
