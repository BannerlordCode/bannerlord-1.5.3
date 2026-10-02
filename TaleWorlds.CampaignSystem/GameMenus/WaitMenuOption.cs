using System;
using System.Reflection;
using System.Xml;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000F1 RID: 241
	public class WaitMenuOption
	{
		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x0600165B RID: 5723 RVA: 0x00065EB1 File Offset: 0x000640B1
		// (set) Token: 0x0600165C RID: 5724 RVA: 0x00065EB9 File Offset: 0x000640B9
		public int Priority { get; private set; }

		// Token: 0x0600165D RID: 5725 RVA: 0x00065EC2 File Offset: 0x000640C2
		internal WaitMenuOption()
		{
			this.Priority = 100;
			this._text = null;
			this._tooltip = "";
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00065EE4 File Offset: 0x000640E4
		internal WaitMenuOption(string idString, TextObject text, WaitMenuOption.OnConditionDelegate condition, WaitMenuOption.OnConsequenceDelegate consequence, int priority = 100, string tooltip = "")
		{
			this._idString = idString;
			this._text = text;
			this.OnCondition = condition;
			this.OnConsequence = consequence;
			this.Priority = priority;
			this._tooltip = tooltip;
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00065F1C File Offset: 0x0006411C
		public bool GetConditionsHold(Game game, MapState mapState)
		{
			if (this.OnCondition != null)
			{
				MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(mapState, this.Text);
				return this.OnCondition(menuCallbackArgs);
			}
			return true;
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001660 RID: 5728 RVA: 0x00065F4C File Offset: 0x0006414C
		public TextObject Text
		{
			get
			{
				return this._text;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001661 RID: 5729 RVA: 0x00065F54 File Offset: 0x00064154
		public string IdString
		{
			get
			{
				return this._idString;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001662 RID: 5730 RVA: 0x00065F5C File Offset: 0x0006415C
		public string Tooltip
		{
			get
			{
				return this._tooltip;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001663 RID: 5731 RVA: 0x00065F64 File Offset: 0x00064164
		public bool IsLeave
		{
			get
			{
				return this._isLeave;
			}
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x00065F6C File Offset: 0x0006416C
		public void RunConsequence(Game game, MapState mapState)
		{
			if (this.OnConsequence != null)
			{
				MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(mapState, this.Text);
				this.OnConsequence(menuCallbackArgs);
			}
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x00065F9C File Offset: 0x0006419C
		public void Deserialize(XmlNode node, Type typeOfWaitMenusCallbacks)
		{
			if (node.Attributes == null)
			{
				throw new TWXmlLoadException("node.Attributes != null");
			}
			this._idString = node.Attributes["id"].Value;
			XmlNode xmlNode = node.Attributes["text"];
			if (xmlNode != null)
			{
				this._text = new TextObject(xmlNode.InnerText, null);
			}
			if (node.Attributes["is_leave"] != null)
			{
				this._isLeave = true;
			}
			XmlNode xmlNode2 = node.Attributes["on_condition"];
			if (xmlNode2 != null)
			{
				string innerText = xmlNode2.InnerText;
				this._methodOnCondition = typeOfWaitMenusCallbacks.GetMethod(innerText);
				if (this._methodOnCondition == null)
				{
					throw new MBNotFoundException("Can not find WaitMenuOption condition:" + innerText);
				}
				this.OnCondition = (WaitMenuOption.OnConditionDelegate)Delegate.CreateDelegate(typeof(WaitMenuOption.OnConditionDelegate), null, this._methodOnCondition);
			}
			XmlNode xmlNode3 = node.Attributes["on_consequence"];
			if (xmlNode3 != null)
			{
				string innerText2 = xmlNode3.InnerText;
				this._methodOnConsequence = typeOfWaitMenusCallbacks.GetMethod(innerText2);
				if (this._methodOnConsequence == null)
				{
					throw new MBNotFoundException("Can not find WaitMenuOption consequence:" + innerText2);
				}
				this.OnConsequence = (WaitMenuOption.OnConsequenceDelegate)Delegate.CreateDelegate(typeof(WaitMenuOption.OnConsequenceDelegate), null, this._methodOnConsequence);
			}
		}

		// Token: 0x0400075B RID: 1883
		private string _idString;

		// Token: 0x0400075C RID: 1884
		private TextObject _text;

		// Token: 0x0400075D RID: 1885
		private string _tooltip;

		// Token: 0x0400075E RID: 1886
		private MethodInfo _methodOnCondition;

		// Token: 0x0400075F RID: 1887
		public WaitMenuOption.OnConditionDelegate OnCondition;

		// Token: 0x04000760 RID: 1888
		private MethodInfo _methodOnConsequence;

		// Token: 0x04000761 RID: 1889
		public WaitMenuOption.OnConsequenceDelegate OnConsequence;

		// Token: 0x04000762 RID: 1890
		private bool _isLeave;

		// Token: 0x02000596 RID: 1430
		// (Invoke) Token: 0x060050CF RID: 20687
		public delegate bool OnConditionDelegate(MenuCallbackArgs args);

		// Token: 0x02000597 RID: 1431
		// (Invoke) Token: 0x060050D3 RID: 20691
		public delegate void OnConsequenceDelegate(MenuCallbackArgs args);
	}
}
