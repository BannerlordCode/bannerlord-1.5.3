using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x0200004F RID: 79
	public class MountHealthCondition : MPPerkCondition
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000B520 File Offset: 0x00009720
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.MountHealthChange | MPPerkCondition.PerkEventFlags.MountChange;
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000B527 File Offset: 0x00009727
		protected MountHealthCondition()
		{
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000B530 File Offset: 0x00009730
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
				Debug.FailedAssert("provided 'min' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\MountHealthCondition.cs", "Deserialize", 34);
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
				Debug.FailedAssert("provided 'max' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\MountHealthCondition.cs", "Deserialize", 44);
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000B64D File Offset: 0x0000984D
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000B664 File Offset: 0x00009864
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && !agent.IsMount) ? agent.MountAgent : agent);
			if (agent != null)
			{
				float num = (this._isRatio ? (agent.Health / agent.HealthLimit) : agent.Health);
				return num >= this._min && num <= this._max;
			}
			return false;
		}

		// Token: 0x040000D1 RID: 209
		protected static string StringType = "MountHealth";

		// Token: 0x040000D2 RID: 210
		private bool _isRatio;

		// Token: 0x040000D3 RID: 211
		private float _min;

		// Token: 0x040000D4 RID: 212
		private float _max;
	}
}
