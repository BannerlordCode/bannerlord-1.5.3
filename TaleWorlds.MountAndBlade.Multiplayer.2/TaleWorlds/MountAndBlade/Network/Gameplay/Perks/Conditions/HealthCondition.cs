using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x0200004B RID: 75
	public class HealthCondition : MPPerkCondition
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000B004 File Offset: 0x00009204
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.HealthChange;
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000B007 File Offset: 0x00009207
		protected HealthCondition()
		{
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000B010 File Offset: 0x00009210
		protected override void Deserialize(XmlNode node)
		{
			string text;
			if (node == null)
			{
				text = null;
			}
			else
			{
				XmlAttributeCollection attributes = node.Attributes;
				if (attributes == null)
				{
					text = null;
				}
				else
				{
					XmlAttribute xmlAttribute = attributes["is_ratio"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			this._isRatio = ((text2 != null) ? text2.ToLower() : null) == "true";
			string text3;
			if (node == null)
			{
				text3 = null;
			}
			else
			{
				XmlAttributeCollection attributes2 = node.Attributes;
				if (attributes2 == null)
				{
					text3 = null;
				}
				else
				{
					XmlAttribute xmlAttribute2 = attributes2["min"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			string text4 = text3;
			if (text4 == null)
			{
				this._min = 0f;
			}
			else if (!float.TryParse(text4, out this._min))
			{
				Debug.FailedAssert("provided 'min' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\HealthCondition.cs", "Deserialize", 34);
			}
			string text5;
			if (node == null)
			{
				text5 = null;
			}
			else
			{
				XmlAttributeCollection attributes3 = node.Attributes;
				if (attributes3 == null)
				{
					text5 = null;
				}
				else
				{
					XmlAttribute xmlAttribute3 = attributes3["max"];
					text5 = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
				}
			}
			string text6 = text5;
			if (text6 == null)
			{
				this._max = (this._isRatio ? 1f : float.MaxValue);
				return;
			}
			if (!float.TryParse(text6, out this._max))
			{
				Debug.FailedAssert("provided 'max' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\HealthCondition.cs", "Deserialize", 44);
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000B12D File Offset: 0x0000932D
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000B144 File Offset: 0x00009344
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent != null)
			{
				float num = (this._isRatio ? (agent.Health / agent.HealthLimit) : agent.Health);
				return num >= this._min && num <= this._max;
			}
			return false;
		}

		// Token: 0x040000C7 RID: 199
		protected static string StringType = "Health";

		// Token: 0x040000C8 RID: 200
		private bool _isRatio;

		// Token: 0x040000C9 RID: 201
		private float _min;

		// Token: 0x040000CA RID: 202
		private float _max;
	}
}
