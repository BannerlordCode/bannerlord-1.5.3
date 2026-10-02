using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using TaleWorlds.MountAndBlade.Network.Gameplay.Perks;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000317 RID: 791
	public abstract class MPPerkCondition
	{
		// Token: 0x06002D60 RID: 11616 RVA: 0x000AF674 File Offset: 0x000AD874
		static MPPerkCondition()
		{
			foreach (Type type in from t in PerkAssemblyCollection.GetPerkAssemblyTypes()
				where t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(MPPerkCondition))
				select t)
			{
				FieldInfo field = type.GetField("StringType", BindingFlags.Static | BindingFlags.NonPublic);
				string text = (string)((field != null) ? field.GetValue(null) : null);
				MPPerkCondition.Registered.Add(text, type);
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06002D61 RID: 11617 RVA: 0x000AF704 File Offset: 0x000AD904
		public virtual MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.None;
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06002D62 RID: 11618 RVA: 0x000AF707 File Offset: 0x000AD907
		public virtual bool IsPeerCondition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002D63 RID: 11619
		public abstract bool Check(MissionPeer peer);

		// Token: 0x06002D64 RID: 11620
		public abstract bool Check(Agent agent);

		// Token: 0x06002D65 RID: 11621 RVA: 0x000AF70A File Offset: 0x000AD90A
		protected virtual bool IsGameModesValid(List<string> gameModes)
		{
			return true;
		}

		// Token: 0x06002D66 RID: 11622
		protected abstract void Deserialize(XmlNode node);

		// Token: 0x06002D67 RID: 11623 RVA: 0x000AF710 File Offset: 0x000AD910
		public static MPPerkCondition CreateFrom(List<string> gameModes, XmlNode node)
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
					XmlAttribute xmlAttribute = attributes["type"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			MPPerkCondition mpperkCondition = (MPPerkCondition)Activator.CreateInstance(MPPerkCondition.Registered[text2], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, null, CultureInfo.InvariantCulture);
			mpperkCondition.Deserialize(node);
			return mpperkCondition;
		}

		// Token: 0x04001203 RID: 4611
		protected static Dictionary<string, Type> Registered = new Dictionary<string, Type>();

		// Token: 0x02000605 RID: 1541
		[Flags]
		public enum PerkEventFlags
		{
			// Token: 0x040020A4 RID: 8356
			None = 0,
			// Token: 0x040020A5 RID: 8357
			MoraleChange = 1,
			// Token: 0x040020A6 RID: 8358
			FlagCapture = 2,
			// Token: 0x040020A7 RID: 8359
			FlagRemoval = 4,
			// Token: 0x040020A8 RID: 8360
			HealthChange = 8,
			// Token: 0x040020A9 RID: 8361
			AliveBotCountChange = 16,
			// Token: 0x040020AA RID: 8362
			PeerControlledAgentChange = 32,
			// Token: 0x040020AB RID: 8363
			BannerPickUp = 64,
			// Token: 0x040020AC RID: 8364
			BannerDrop = 128,
			// Token: 0x040020AD RID: 8365
			SpawnEnd = 256,
			// Token: 0x040020AE RID: 8366
			MountHealthChange = 512,
			// Token: 0x040020AF RID: 8367
			MountChange = 1024,
			// Token: 0x040020B0 RID: 8368
			AgentEventsMask = 1576
		}
	}
}
