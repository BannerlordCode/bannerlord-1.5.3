using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Chat
{
	// Token: 0x0200017D RID: 381
	public class ChatLogItemWidget : Widget
	{
		// Token: 0x060013E4 RID: 5092 RVA: 0x00036283 File Offset: 0x00034483
		public ChatLogItemWidget(UIContext context)
			: base(context)
		{
			this._fullyInsideAction = new Action<Widget>(this.UpdateWidgetFullyInside);
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x000362BC File Offset: 0x000344BC
		private void UpdateWidgetFullyInside(Widget widget)
		{
			widget.DoNotRenderIfNotFullyInsideScissor = false;
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x000362C5 File Offset: 0x000344C5
		protected override void OnParallelUpdate(float dt)
		{
			base.OnParallelUpdate(dt);
			base.ApplyActionToAllChildrenRecursive(this._fullyInsideAction);
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x000362DC File Offset: 0x000344DC
		private void PostMessage(string message)
		{
			if (message.IndexOf(this._detailOpeningTag, StringComparison.Ordinal) > 0)
			{
				foreach (ChatLogItemWidget.ChatMultiLineElement chatMultiLineElement in this.GetFormattedLinesFromMessage(message))
				{
					RichTextWidget richTextWidget = new RichTextWidget(base.Context)
					{
						Id = "FormattedLineRichTextWidget",
						WidthSizePolicy = SizePolicy.StretchToParent,
						HeightSizePolicy = SizePolicy.CoverChildren,
						Brush = this.OneLineTextWidget.ReadOnlyBrush,
						MarginTop = -2f,
						MarginBottom = -2f,
						IsEnabled = false,
						Text = chatMultiLineElement.Line,
						MarginLeft = (float)(chatMultiLineElement.IdentModifier * this._defaultMarginLeftPerIndent) * base._inverseScaleToUse,
						ClipContents = false,
						DoNotRenderIfNotFullyInsideScissor = false
					};
					this.CollapsableWidget.AddChild(richTextWidget);
				}
				this.CollapsableWidget.IsVisible = true;
				this.OneLineTextWidget.IsVisible = false;
				return;
			}
			this.OneLineTextWidget.Text = message;
			this.CollapsableWidget.IsVisible = false;
			this.OneLineTextWidget.IsVisible = true;
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x00036414 File Offset: 0x00034614
		private List<ChatLogItemWidget.ChatMultiLineElement> GetFormattedLinesFromMessage(string message)
		{
			List<ChatLogItemWidget.ChatMultiLineElement> list = new List<ChatLogItemWidget.ChatMultiLineElement>();
			XmlDocument xmlDocument = new XmlDocument();
			int num = message.IndexOf(this._detailOpeningTag, StringComparison.Ordinal);
			string text = message.Substring(0, num);
			string text2 = message.Substring(num, message.Length - num);
			text2 = this._detailOpeningTag + text2 + this._detailClosingTag;
			list.Add(new ChatLogItemWidget.ChatMultiLineElement(text, 0));
			try
			{
				xmlDocument.LoadXml(text2);
				this.AddLinesFromXMLRecur(xmlDocument.FirstChild, ref list, 0);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Couldn't parse chat log message: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Chat\\ChatLogItemWidget.cs", "GetFormattedLinesFromMessage", 111);
			}
			return list;
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x000364C8 File Offset: 0x000346C8
		private void AddLinesFromXMLRecur(XmlNode currentNode, ref List<ChatLogItemWidget.ChatMultiLineElement> lineList, int currentIndentModifier)
		{
			if (currentNode.NodeType == XmlNodeType.Text)
			{
				lineList.Add(new ChatLogItemWidget.ChatMultiLineElement(currentNode.InnerText, currentIndentModifier));
				for (int i = 0; i < currentNode.ChildNodes.Count; i++)
				{
					this.AddLinesFromXMLRecur(currentNode.ChildNodes.Item(i), ref lineList, currentIndentModifier + 1);
				}
				return;
			}
			for (int j = 0; j < currentNode.ChildNodes.Count; j++)
			{
				this.AddLinesFromXMLRecur(currentNode.ChildNodes.Item(j), ref lineList, currentIndentModifier + 1);
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x0003654A File Offset: 0x0003474A
		// (set) Token: 0x060013EB RID: 5099 RVA: 0x00036552 File Offset: 0x00034752
		[Editor(false)]
		public RichTextWidget OneLineTextWidget
		{
			get
			{
				return this._oneLineTextWidget;
			}
			set
			{
				if (this._oneLineTextWidget != value)
				{
					this._oneLineTextWidget = value;
				}
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x00036564 File Offset: 0x00034764
		// (set) Token: 0x060013ED RID: 5101 RVA: 0x0003656C File Offset: 0x0003476C
		[Editor(false)]
		public ChatCollapsableListPanel CollapsableWidget
		{
			get
			{
				return this._collapsableWidget;
			}
			set
			{
				if (this._collapsableWidget != value)
				{
					this._collapsableWidget = value;
				}
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x060013EE RID: 5102 RVA: 0x0003657E File Offset: 0x0003477E
		// (set) Token: 0x060013EF RID: 5103 RVA: 0x00036586 File Offset: 0x00034786
		[Editor(false)]
		public string ChatLine
		{
			get
			{
				return this._chatLine;
			}
			set
			{
				if (this._chatLine != value)
				{
					this._chatLine = value;
					this.PostMessage(value);
				}
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x060013F0 RID: 5104 RVA: 0x000365A4 File Offset: 0x000347A4
		// (set) Token: 0x060013F1 RID: 5105 RVA: 0x000365AC File Offset: 0x000347AC
		[Editor(false)]
		public ChatLogWidget ChatLogWidget
		{
			get
			{
				return this._chatLogWidget;
			}
			set
			{
				if (this._chatLogWidget != value)
				{
					this._chatLogWidget = value;
				}
			}
		}

		// Token: 0x04000907 RID: 2311
		private int _defaultMarginLeftPerIndent = 20;

		// Token: 0x04000908 RID: 2312
		private string _detailOpeningTag = "<Detail>";

		// Token: 0x04000909 RID: 2313
		private string _detailClosingTag = "</Detail>";

		// Token: 0x0400090A RID: 2314
		private Action<Widget> _fullyInsideAction;

		// Token: 0x0400090B RID: 2315
		private ChatLogWidget _chatLogWidget;

		// Token: 0x0400090C RID: 2316
		private string _chatLine;

		// Token: 0x0400090D RID: 2317
		private RichTextWidget _oneLineTextWidget;

		// Token: 0x0400090E RID: 2318
		private ChatCollapsableListPanel _collapsableWidget;

		// Token: 0x020001D7 RID: 471
		public struct ChatMultiLineElement
		{
			// Token: 0x060015C3 RID: 5571 RVA: 0x0003AF17 File Offset: 0x00039117
			public ChatMultiLineElement(string line, int identModifier)
			{
				this.Line = line;
				this.IdentModifier = identModifier;
			}

			// Token: 0x04000A81 RID: 2689
			public string Line;

			// Token: 0x04000A82 RID: 2690
			public int IdentModifier;
		}
	}
}
