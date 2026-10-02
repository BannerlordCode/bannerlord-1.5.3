using System;
using System.IO;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x0200009B RID: 155
	public sealed class ManagedParameters : IManagedParametersInitializer
	{
		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0001D361 File Offset: 0x0001B561
		public static ManagedParameters Instance { get; } = new ManagedParameters();

		// Token: 0x060008E6 RID: 2278 RVA: 0x0001D368 File Offset: 0x0001B568
		public static float GetParameter(ManagedParametersEnum managedParameterType)
		{
			return ManagedParameters.Instance._managedParametersArray[(int)managedParameterType];
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0001D376 File Offset: 0x0001B576
		public static void SetParameter(ManagedParametersEnum managedParameterType, float newValue)
		{
			ManagedParameters.Instance._managedParametersArray[(int)managedParameterType] = newValue;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0001D388 File Offset: 0x0001B588
		public void Initialize(string relativeXmlPath)
		{
			XmlDocument mergedXmlForManaged = MBObjectManager.GetMergedXmlForManaged("CoreParameters", true, true, "");
			this.LoadFromXml(mergedXmlForManaged);
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0001D3AE File Offset: 0x0001B5AE
		private ManagedParameters()
		{
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0001D3C4 File Offset: 0x0001B5C4
		private static XmlDocument LoadXmlFile(string path)
		{
			Debug.Print("opening " + path, 0, Debug.DebugColor.White, 17592186044416UL);
			XmlDocument xmlDocument = new XmlDocument();
			StreamReader streamReader = new StreamReader(path);
			string text = streamReader.ReadToEnd();
			xmlDocument.LoadXml(text);
			streamReader.Close();
			return xmlDocument;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0001D410 File Offset: 0x0001B610
		private void LoadFromXml(XmlNode doc)
		{
			Debug.Print("loading managed_core_parameters.xml", 0, Debug.DebugColor.White, 17592186044416UL);
			if (doc.ChildNodes.Count < 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format.");
			}
			XmlNode xmlNode = doc.SelectSingleNode(".//managed_core_parameters");
			if (xmlNode == null)
			{
				throw new TWXmlLoadException("Incorrect XML document format.");
			}
			for (XmlNode xmlNode2 = xmlNode.ChildNodes[0]; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
			{
				ManagedParametersEnum managedParametersEnum;
				if (xmlNode2.Name == "managed_core_parameter" && xmlNode2.NodeType != XmlNodeType.Comment && Enum.TryParse<ManagedParametersEnum>(xmlNode2.Attributes["id"].Value, true, out managedParametersEnum))
				{
					this._managedParametersArray[(int)managedParametersEnum] = float.Parse(xmlNode2.Attributes["value"].Value);
				}
			}
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0001D4D9 File Offset: 0x0001B6D9
		public float GetManagedParameter(ManagedParametersEnum managedParameterEnum)
		{
			return this._managedParametersArray[(int)managedParameterEnum];
		}

		// Token: 0x04000504 RID: 1284
		private readonly float[] _managedParametersArray = new float[72];
	}
}
