using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.ObjectSystem
{
	// Token: 0x02000013 RID: 19
	public static class XmlResource
	{
		// Token: 0x06000099 RID: 153 RVA: 0x00004FF8 File Offset: 0x000031F8
		private static XmlDocument LoadXmlDocumentWithLineInfo(string filePath)
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;
			xmlReaderSettings.CheckCharacters = false;
			xmlReaderSettings.XmlResolver = new XmlUrlResolver();
			xmlReaderSettings.MaxCharactersFromEntities = 0L;
			XmlResource.LineInfoXmlDocument lineInfoXmlDocument = new XmlResource.LineInfoXmlDocument();
			using (StreamReader streamReader = new StreamReader(filePath))
			{
				using (XmlReader xmlReader = XmlReader.Create(streamReader, xmlReaderSettings))
				{
					lineInfoXmlDocument.Load(xmlReader);
				}
			}
			return lineInfoXmlDocument;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000508C File Offset: 0x0000328C
		private static string GetLineHint(XmlNode node)
		{
			IXmlLineInfo xmlLineInfo = node as IXmlLineInfo;
			if (xmlLineInfo == null || !xmlLineInfo.HasLineInfo())
			{
				return "";
			}
			return string.Format(" (line {0})", xmlLineInfo.LineNumber);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000050C8 File Offset: 0x000032C8
		private static string GetLineHint(XObject node)
		{
			if (node == null || !((IXmlLineInfo)node).HasLineInfo())
			{
				return "";
			}
			return string.Format(" (line {0})", ((IXmlLineInfo)node).LineNumber);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005100 File Offset: 0x00003300
		public static void ReadXsdFileAndExtractInformation(string xsdFilePath)
		{
			try
			{
				XDocument xdocument = XDocument.Load(xsdFilePath, LoadOptions.SetLineInfo);
				XmlResource.XsdElementDictionary[xsdFilePath] = new Dictionary<string, XmlResource.XsdElement>();
				foreach (XElement xelement in xdocument.Descendants(XmlResource.XsNamespace + "element"))
				{
					string fullXPathOfElement = XmlResource.GetFullXPathOfElement(xelement, true);
					bool alwaysPreferMerge = XmlResource.GetAlwaysPreferMerge(xelement);
					XmlResource.XsdElement xsdElement = new XmlResource.XsdElement(fullXPathOfElement, alwaysPreferMerge);
					XmlResource.XsdElementDictionary[xsdFilePath][fullXPathOfElement] = xsdElement;
				}
				foreach (XElement xelement2 in xdocument.Descendants(XmlResource.XsNamespace + "unique").Concat<XElement>(xdocument.Descendants(XmlResource.XsNamespace + "key")))
				{
					string fullXPathOfElement2 = XmlResource.GetFullXPathOfElement(xelement2, true);
					string text = "/";
					XElement xelement3 = xelement2.Element(XmlResource.XsNamespace + "selector");
					string text2;
					if (xelement3 == null)
					{
						text2 = null;
					}
					else
					{
						XAttribute xattribute = xelement3.Attribute("xpath");
						text2 = ((xattribute != null) ? xattribute.Value : null);
					}
					string text3 = fullXPathOfElement2 + text + text2;
					foreach (XElement xelement4 in xelement2.Elements(XmlResource.XsNamespace + "field"))
					{
						string text4;
						if (xelement4 == null)
						{
							text4 = null;
						}
						else
						{
							XAttribute xattribute2 = xelement4.Attribute("xpath");
							text4 = ((xattribute2 != null) ? xattribute2.Value : null);
						}
						string text5 = text4;
						if (string.IsNullOrEmpty(text5))
						{
							throw new Exception(string.Concat(new string[]
							{
								"An <xs:field> element",
								XmlResource.GetLineHint(xelement4),
								" in '",
								xsdFilePath,
								"' is missing the required 'xpath' attribute."
							}));
						}
						if (!XmlResource.XsdElementDictionary[xsdFilePath].ContainsKey(text3))
						{
							throw new Exception(string.Concat(new string[]
							{
								"The selector xpath '",
								text3,
								"' of an <xs:unique>/<xs:key> element",
								XmlResource.GetLineHint(xelement2),
								" in '",
								xsdFilePath,
								"' does not match any declared element."
							}));
						}
						XmlResource.XsdElementDictionary[xsdFilePath][text3].UniqueAttributes.Add(text5.Substring(1));
					}
				}
			}
			catch (XmlException ex)
			{
				throw new Exception(string.Format("Malformed XSD schema in '{0}' at line {1}, position {2}: {3}", new object[] { xsdFilePath, ex.LineNumber, ex.LinePosition, ex.Message }), ex);
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005400 File Offset: 0x00003600
		private static bool GetAlwaysPreferMerge(XElement element)
		{
			XElement xelement = element.Element(XmlResource.XsNamespace + "annotation");
			if (xelement != null)
			{
				XElement xelement2 = xelement.Element(XmlResource.XsNamespace + "appinfo");
				if (xelement2 != null)
				{
					XElement xelement3 = xelement2.Element(XNamespace.None + "appSpecificNote");
					if (xelement3 != null && xelement3.Value.Trim() == "AlwaysPreferMerge")
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005474 File Offset: 0x00003674
		public static string GetFullXPathOfElement(XElement element, bool isXsd = true)
		{
			if (element == null)
			{
				return null;
			}
			if (isXsd)
			{
				if (element.Name != XmlResource.XsNamespace + "element")
				{
					return XmlResource.GetFullXPathOfElement(element.Parent, true) ?? "";
				}
				string text = "";
				if (element.Attribute("name") != null)
				{
					text = element.Attribute("name").Value;
				}
				else if (element.Attribute("ref") != null)
				{
					text = element.Attribute("ref").Value;
				}
				if (element.Parent == null)
				{
					return text ?? "";
				}
				return XmlResource.GetFullXPathOfElement(element.Parent, true) + "/" + text;
			}
			else
			{
				if (element.Parent == null)
				{
					return string.Format("/{0}", element.Name);
				}
				return string.Format("{0}/{1}", XmlResource.GetFullXPathOfElement(element.Parent, false), element.Name);
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005576 File Offset: 0x00003776
		public static void InitializeXmlInformationList(List<MbObjectXmlInformation> xmlInformation)
		{
			XmlResource.XmlInformationList = xmlInformation;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005580 File Offset: 0x00003780
		public static void GetMbprojxmls(string moduleName)
		{
			string mbprojPath = ModuleHelper.GetMbprojPath(moduleName);
			if (mbprojPath.Length > 0 && File.Exists(mbprojPath))
			{
				try
				{
					XmlNode xmlNode = XmlResource.LoadXmlDocumentWithLineInfo(mbprojPath).SelectSingleNode("base");
					if (xmlNode == null)
					{
						throw new Exception("Root element <base> not found in '" + mbprojPath + "'. The file may be empty or have an unexpected root element.");
					}
					XmlNodeList xmlNodeList = xmlNode.SelectNodes("file");
					if (xmlNodeList != null)
					{
						foreach (object obj in xmlNodeList)
						{
							XmlNode xmlNode2 = (XmlNode)obj;
							string lineHint = XmlResource.GetLineHint(xmlNode2);
							if (xmlNode2.Attributes["id"] == null)
							{
								throw new Exception(string.Concat(new string[] { "A <file> element", lineHint, " in '", mbprojPath, "' is missing the required 'id' attribute." }));
							}
							if (xmlNode2.Attributes["name"] == null)
							{
								throw new Exception(string.Concat(new string[] { "A <file> element", lineHint, " in '", mbprojPath, "' is missing the required 'name' attribute." }));
							}
							string innerText = xmlNode2.Attributes["id"].InnerText;
							string innerText2 = xmlNode2.Attributes["name"].InnerText;
							string xsdPath = ModuleHelper.GetXsdPath(innerText);
							if (File.Exists(xsdPath))
							{
								XmlResource.ReadXsdFileAndExtractInformation(xsdPath);
							}
							MbObjectXmlInformation mbObjectXmlInformation = new MbObjectXmlInformation
							{
								Id = innerText,
								Name = innerText2,
								ModuleName = moduleName,
								GameTypesIncluded = new List<string>()
							};
							XmlResource.MbprojXmls.Add(mbObjectXmlInformation);
						}
					}
				}
				catch (XmlException ex)
				{
					throw new Exception(string.Format("Malformed XML in '{0}' at line {1}, position {2}: {3}", new object[] { mbprojPath, ex.LineNumber, ex.LinePosition, ex.Message }), ex);
				}
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000057B0 File Offset: 0x000039B0
		public static void GetXmlListAndApply(string moduleName)
		{
			string path = ModuleHelper.GetPath(moduleName);
			try
			{
				XmlNode xmlNode = XmlResource.LoadXmlDocumentWithLineInfo(path).SelectSingleNode("Module");
				if (xmlNode == null)
				{
					throw new Exception("Root element <Module> not found in '" + path + "'. The file may be empty or have an unexpected root element.");
				}
				XmlNodeList xmlNodeList = xmlNode.SelectNodes("Xmls/XmlNode");
				if (xmlNodeList != null)
				{
					foreach (object obj in xmlNodeList)
					{
						XmlNode xmlNode2 = (XmlNode)obj;
						string lineHint = XmlResource.GetLineHint(xmlNode2);
						XmlNode xmlNode3 = xmlNode2.SelectSingleNode("XmlName");
						if (xmlNode3 == null)
						{
							throw new Exception(string.Concat(new string[] { "An <XmlNode> element", lineHint, " in '", path, "' is missing the required <XmlName> child element." }));
						}
						string lineHint2 = XmlResource.GetLineHint(xmlNode3);
						if (xmlNode3.Attributes["id"] == null)
						{
							throw new Exception(string.Concat(new string[] { "An <XmlName> element", lineHint2, " in '", path, "' is missing the required 'id' attribute." }));
						}
						if (xmlNode3.Attributes["path"] == null)
						{
							throw new Exception(string.Concat(new string[] { "An <XmlName> element", lineHint2, " in '", path, "' is missing the required 'path' attribute." }));
						}
						string innerText = xmlNode3.Attributes["id"].InnerText;
						string innerText2 = xmlNode3.Attributes["path"].InnerText;
						string xsdPath = ModuleHelper.GetXsdPath(innerText);
						if (File.Exists(xsdPath))
						{
							XmlResource.ReadXsdFileAndExtractInformation(xsdPath);
						}
						List<string> list = new List<string>();
						XmlNode xmlNode4 = xmlNode2.SelectSingleNode("IncludedGameTypes");
						if (xmlNode4 != null)
						{
							foreach (object obj2 in xmlNode4.ChildNodes)
							{
								XmlNode xmlNode5 = (XmlNode)obj2;
								if (xmlNode5.NodeType == XmlNodeType.Element)
								{
									string lineHint3 = XmlResource.GetLineHint(xmlNode5);
									if (xmlNode5.Attributes["value"] == null)
									{
										throw new Exception(string.Concat(new string[] { "An <IncludedGameTypes> child element", lineHint3, " in '", path, "' is missing the required 'value' attribute." }));
									}
									list.Add(xmlNode5.Attributes["value"].InnerText);
								}
							}
						}
						MbObjectXmlInformation mbObjectXmlInformation = new MbObjectXmlInformation
						{
							Id = innerText,
							Name = innerText2,
							ModuleName = moduleName,
							GameTypesIncluded = list
						};
						XmlResource.XmlInformationList.Add(mbObjectXmlInformation);
					}
				}
			}
			catch (XmlException ex)
			{
				throw new Exception(string.Format("Malformed XML in '{0}' at line {1}, position {2}: {3}", new object[] { path, ex.LineNumber, ex.LinePosition, ex.Message }), ex);
			}
		}

		// Token: 0x04000010 RID: 16
		public static List<MbObjectXmlInformation> XmlInformationList = new List<MbObjectXmlInformation>();

		// Token: 0x04000011 RID: 17
		public static List<MbObjectXmlInformation> MbprojXmls = new List<MbObjectXmlInformation>();

		// Token: 0x04000012 RID: 18
		public static Dictionary<string, Dictionary<string, XmlResource.XsdElement>> XsdElementDictionary = new Dictionary<string, Dictionary<string, XmlResource.XsdElement>>();

		// Token: 0x04000013 RID: 19
		public static XNamespace XsNamespace = "http://www.w3.org/2001/XMLSchema";

		// Token: 0x0200001A RID: 26
		public struct XsdElement
		{
			// Token: 0x060000E4 RID: 228 RVA: 0x00006432 File Offset: 0x00004632
			public XsdElement(string xPath, bool alwaysPreferMerge)
			{
				this.XPath = xPath;
				this.AlwaysPreferMerge = alwaysPreferMerge;
				this.UniqueAttributes = new List<string>();
			}

			// Token: 0x04000025 RID: 37
			public string XPath;

			// Token: 0x04000026 RID: 38
			public bool AlwaysPreferMerge;

			// Token: 0x04000027 RID: 39
			public List<string> UniqueAttributes;
		}

		// Token: 0x0200001B RID: 27
		private sealed class LineInfoXmlElement : XmlElement, IXmlLineInfo
		{
			// Token: 0x060000E5 RID: 229 RVA: 0x0000644D File Offset: 0x0000464D
			internal LineInfoXmlElement(string prefix, string localName, string namespaceUri, XmlDocument doc, IXmlLineInfo lineInfo)
				: base(prefix, localName, namespaceUri, doc)
			{
				if (lineInfo != null && lineInfo.HasLineInfo())
				{
					this._hasLineInfo = true;
					this._lineNumber = lineInfo.LineNumber;
					this._linePosition = lineInfo.LinePosition;
				}
			}

			// Token: 0x060000E6 RID: 230 RVA: 0x00006488 File Offset: 0x00004688
			public bool HasLineInfo()
			{
				return this._hasLineInfo;
			}

			// Token: 0x17000018 RID: 24
			// (get) Token: 0x060000E7 RID: 231 RVA: 0x00006490 File Offset: 0x00004690
			public int LineNumber
			{
				get
				{
					return this._lineNumber;
				}
			}

			// Token: 0x17000019 RID: 25
			// (get) Token: 0x060000E8 RID: 232 RVA: 0x00006498 File Offset: 0x00004698
			public int LinePosition
			{
				get
				{
					return this._linePosition;
				}
			}

			// Token: 0x04000028 RID: 40
			private readonly int _lineNumber;

			// Token: 0x04000029 RID: 41
			private readonly int _linePosition;

			// Token: 0x0400002A RID: 42
			private readonly bool _hasLineInfo;
		}

		// Token: 0x0200001C RID: 28
		private sealed class LineInfoXmlDocument : XmlDocument
		{
			// Token: 0x060000E9 RID: 233 RVA: 0x000064A0 File Offset: 0x000046A0
			public override void Load(XmlReader reader)
			{
				this._currentLineInfo = reader as IXmlLineInfo;
				try
				{
					base.Load(reader);
				}
				finally
				{
					this._currentLineInfo = null;
				}
			}

			// Token: 0x060000EA RID: 234 RVA: 0x000064DC File Offset: 0x000046DC
			public override XmlElement CreateElement(string prefix, string localName, string namespaceUri)
			{
				return new XmlResource.LineInfoXmlElement(prefix, localName, namespaceUri, this, this._currentLineInfo);
			}

			// Token: 0x0400002B RID: 43
			private IXmlLineInfo _currentLineInfo;
		}
	}
}
