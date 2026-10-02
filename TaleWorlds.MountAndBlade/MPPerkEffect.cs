using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using TaleWorlds.MountAndBlade.Network.Gameplay.Perks;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000319 RID: 793
	public abstract class MPPerkEffect : MPPerkEffectBase
	{
		// Token: 0x06002D6C RID: 11628 RVA: 0x000AF94C File Offset: 0x000ADB4C
		static MPPerkEffect()
		{
			foreach (Type type in from t in PerkAssemblyCollection.GetPerkAssemblyTypes()
				where t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(MPPerkEffect))
				select t)
			{
				FieldInfo field = type.GetField("StringType", BindingFlags.Static | BindingFlags.NonPublic);
				string text = (string)((field != null) ? field.GetValue(null) : null);
				MPPerkEffect.Registered.Add(text, type);
			}
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x000AF9DC File Offset: 0x000ADBDC
		public static MPPerkEffect CreateFrom(XmlNode node)
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
			MPPerkEffect mpperkEffect = (MPPerkEffect)Activator.CreateInstance(MPPerkEffect.Registered[text2], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, null, CultureInfo.InvariantCulture);
			mpperkEffect.Deserialize(node);
			return mpperkEffect;
		}

		// Token: 0x04001204 RID: 4612
		protected static Dictionary<string, Type> Registered = new Dictionary<string, Type>();
	}
}
