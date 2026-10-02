using System;
using System.Collections;
using System.Xml;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x0200009C RID: 156
	public class MBBodyProperty : MBObjectBase
	{
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x0001D4EF File Offset: 0x0001B6EF
		// (set) Token: 0x060008EF RID: 2287 RVA: 0x0001D4F7 File Offset: 0x0001B6F7
		public string HairTags { get; set; } = "";

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x0001D500 File Offset: 0x0001B700
		// (set) Token: 0x060008F1 RID: 2289 RVA: 0x0001D508 File Offset: 0x0001B708
		public string BeardTags { get; set; } = "";

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x0001D511 File Offset: 0x0001B711
		// (set) Token: 0x060008F3 RID: 2291 RVA: 0x0001D519 File Offset: 0x0001B719
		public string TattooTags { get; set; } = "";

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x0001D522 File Offset: 0x0001B722
		public BodyProperties BodyPropertyMin
		{
			get
			{
				return this._bodyPropertyMin;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x0001D52A File Offset: 0x0001B72A
		public BodyProperties BodyPropertyMax
		{
			get
			{
				return this._bodyPropertyMax;
			}
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0001D532 File Offset: 0x0001B732
		public MBBodyProperty(string stringId)
			: base(stringId)
		{
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0001D55C File Offset: 0x0001B75C
		public MBBodyProperty()
		{
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0001D588 File Offset: 0x0001B788
		public static MBBodyProperty CreateFrom(MBBodyProperty bodyProperty)
		{
			MBBodyProperty mbbodyProperty = MBObjectManager.Instance.CreateObject<MBBodyProperty>();
			mbbodyProperty.HairTags = bodyProperty.HairTags;
			mbbodyProperty.BeardTags = bodyProperty.BeardTags;
			mbbodyProperty.TattooTags = bodyProperty.TattooTags;
			mbbodyProperty._bodyPropertyMin = bodyProperty._bodyPropertyMin;
			mbbodyProperty._bodyPropertyMax = bodyProperty._bodyPropertyMax;
			return mbbodyProperty;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0001D5DC File Offset: 0x0001B7DC
		public void Init(BodyProperties bodyPropertyMin, BodyProperties bodyPropertyMax)
		{
			base.Initialize();
			this._bodyPropertyMin = bodyPropertyMin;
			this._bodyPropertyMax = bodyPropertyMax;
			if (this._bodyPropertyMax.Age <= 0f)
			{
				this._bodyPropertyMax = this._bodyPropertyMin;
			}
			if (this._bodyPropertyMin.Age <= 0f)
			{
				this._bodyPropertyMin = this._bodyPropertyMax;
			}
			base.AfterInitialized();
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0001D640 File Offset: 0x0001B840
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "BodyPropertiesMin")
				{
					BodyProperties.FromXmlNode(xmlNode, out this._bodyPropertyMin);
				}
				else if (xmlNode.Name == "BodyPropertiesMax")
				{
					BodyProperties.FromXmlNode(xmlNode, out this._bodyPropertyMax);
				}
				else
				{
					if (xmlNode.Name == "hair_tags")
					{
						using (IEnumerator enumerator2 = xmlNode.ChildNodes.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								object obj2 = enumerator2.Current;
								XmlNode xmlNode2 = (XmlNode)obj2;
								string hairTags = this.HairTags;
								XmlAttributeCollection attributes = xmlNode2.Attributes;
								this.HairTags = hairTags + ((attributes != null) ? attributes["name"].Value : null) + ",";
							}
							continue;
						}
					}
					if (xmlNode.Name == "beard_tags")
					{
						using (IEnumerator enumerator2 = xmlNode.ChildNodes.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								object obj3 = enumerator2.Current;
								XmlNode xmlNode3 = (XmlNode)obj3;
								string beardTags = this.BeardTags;
								XmlAttributeCollection attributes2 = xmlNode3.Attributes;
								this.BeardTags = beardTags + ((attributes2 != null) ? attributes2["name"].Value : null) + ",";
							}
							continue;
						}
					}
					if (xmlNode.Name == "tattoo_tags")
					{
						foreach (object obj4 in xmlNode.ChildNodes)
						{
							XmlNode xmlNode4 = (XmlNode)obj4;
							string tattooTags = this.TattooTags;
							XmlAttributeCollection attributes3 = xmlNode4.Attributes;
							this.TattooTags = tattooTags + ((attributes3 != null) ? attributes3["name"].Value : null) + ",";
						}
					}
				}
			}
			if (this._bodyPropertyMax.Age <= 0f)
			{
				this._bodyPropertyMax = this._bodyPropertyMin;
			}
			if (this._bodyPropertyMin.Age <= 0f)
			{
				this._bodyPropertyMin = this._bodyPropertyMax;
			}
		}

		// Token: 0x04000509 RID: 1289
		private BodyProperties _bodyPropertyMin;

		// Token: 0x0400050A RID: 1290
		private BodyProperties _bodyPropertyMax;
	}
}
