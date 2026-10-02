using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x0200000E RID: 14
	public class SelectionGroup : ViewModel
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00007B10 File Offset: 0x00005D10
		public SelectionGroup(string name, List<string> textList = null)
		{
			this._name = name;
			if (textList != null)
			{
				this._textList = textList;
			}
			this.Text = ((this._textList.Count > 0) ? this._textList[0] : "");
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00007B68 File Offset: 0x00005D68
		protected virtual void ClickSelectionLeft()
		{
			this._index--;
			if (this._index < 0)
			{
				this._index = this._textList.Count - 1;
			}
			this.Text = ((this._textList.Count > 0) ? this._textList[this._index] : "");
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00007BCC File Offset: 0x00005DCC
		protected virtual void ClickSelectionRight()
		{
			this._index++;
			this._index %= this._textList.Count;
			this.Text = ((this._textList.Count > 0) ? this._textList[this._index] : "");
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00007C2B File Offset: 0x00005E2B
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00007C33 File Offset: 0x00005E33
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00007C56 File Offset: 0x00005E56
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00007C5E File Offset: 0x00005E5E
		public List<string> TextList
		{
			get
			{
				return this._textList;
			}
			set
			{
				if (value != this._textList)
				{
					this._textList = value;
					this.Text = ((this._textList.Count > 0) ? this._textList[this._index] : "");
				}
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00007C9C File Offset: 0x00005E9C
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00007CA4 File Offset: 0x00005EA4
		public int Index
		{
			get
			{
				return this._index;
			}
			private set
			{
				value = this._index;
			}
		}

		// Token: 0x0400007C RID: 124
		protected List<string> _textList = new List<string>();

		// Token: 0x0400007D RID: 125
		private int _index;

		// Token: 0x0400007E RID: 126
		private string _name;

		// Token: 0x0400007F RID: 127
		private string _text;
	}
}
