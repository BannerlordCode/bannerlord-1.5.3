using System;
using System.Numerics;
using System.Text.RegularExpressions;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000005 RID: 5
	public class EditableText : RichText
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002060 File Offset: 0x00000260
		public int CursorPosition { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002069 File Offset: 0x00000269
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002071 File Offset: 0x00000271
		public bool HighlightStart { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000207A File Offset: 0x0000027A
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002082 File Offset: 0x00000282
		public bool HighlightEnd { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000208B File Offset: 0x0000028B
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002093 File Offset: 0x00000293
		public int SelectedTextBegin { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000209C File Offset: 0x0000029C
		// (set) Token: 0x0600000C RID: 12 RVA: 0x000020A4 File Offset: 0x000002A4
		public int SelectedTextEnd { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000020AD File Offset: 0x000002AD
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000020B5 File Offset: 0x000002B5
		public float BlinkTimer { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000020BE File Offset: 0x000002BE
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000020C6 File Offset: 0x000002C6
		public string VisibleText { get; set; }

		// Token: 0x06000011 RID: 17 RVA: 0x000020D0 File Offset: 0x000002D0
		public EditableText(int width, int height, Font font, Func<int, Font> getUsableFontForCharacter)
			: base(width, height, font, getUsableFontForCharacter)
		{
			this._cursorVisible = false;
			this.CursorPosition = 0;
			this._visibleStart = 0;
			this.VisibleText = "";
			this.BlinkTimer = 0f;
			this.HighlightStart = false;
			this.HighlightEnd = true;
			this._selectionAnchor = 0;
			string text = "\\w+";
			this._nextWordRegex = new Regex(text);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000213C File Offset: 0x0000033C
		public void SetCursorPosition(int position, bool visible)
		{
			if (this.CursorPosition != position || this._cursorVisible != visible)
			{
				this.CursorPosition = position;
				if (this._visibleStart > this.CursorPosition)
				{
					this._visibleStart = this.CursorPosition;
				}
				this._cursorVisible = visible;
				base.SetAllDirty();
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002189 File Offset: 0x00000389
		public void BlinkCursor()
		{
			this._cursorVisible = !this._cursorVisible;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000219A File Offset: 0x0000039A
		public bool IsCursorVisible()
		{
			return this._cursorVisible;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000021A2 File Offset: 0x000003A2
		public void ResetSelected()
		{
			this._selectionAnchor = 0;
			this.SelectedTextBegin = 0;
			this.SelectedTextEnd = 0;
			this.HighlightStart = false;
			this.HighlightEnd = true;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000021C7 File Offset: 0x000003C7
		public void BeginSelection()
		{
			this._selectionAnchor = this.CursorPosition;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000021D5 File Offset: 0x000003D5
		public bool IsAnySelected()
		{
			return this.SelectedTextEnd != this.SelectedTextBegin;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000021E8 File Offset: 0x000003E8
		public Vector2 GetCursorPosition()
		{
			StyleFontContainer.FontData fontData = base.StyleFontContainer.GetFontData("Default");
			Font font = fontData.Font;
			float num = fontData.FontSize / (float)font.Size;
			float num2 = (float)font.LineHeight * num;
			float wordWidth = this.GetWordWidth(this._realVisibleText);
			float wordWidth2 = this.GetWordWidth(this._realVisibleText.Substring(0, Math.Min(this._realVisibleText.Length, this.CursorPosition - this._visibleStart)));
			float num3 = 0f;
			if (base.HorizontalAlignment == TextHorizontalAlignment.Center)
			{
				num3 = ((float)base.Width - wordWidth) * 0.5f;
			}
			else if (base.HorizontalAlignment == TextHorizontalAlignment.Right)
			{
				num3 = (float)base.Width - wordWidth;
			}
			float num4 = 0f;
			if (base.VerticalAlignment == TextVerticalAlignment.Center)
			{
				num4 = ((float)base.Height - num2 + 2.5f) * 0.5f;
			}
			else if (base.VerticalAlignment == TextVerticalAlignment.Bottom)
			{
				num4 = (float)base.Height - num2 + 2.5f;
			}
			return new Vector2(num3 + wordWidth2, num4);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000022E8 File Offset: 0x000004E8
		private float GetWordWidth(string word)
		{
			float num = 0f;
			for (int i = 0; i < word.Length; i++)
			{
				num += this.GetCharacterWidth(word[i]);
			}
			return num;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002320 File Offset: 0x00000520
		private float GetCharacterWidth(char character)
		{
			StyleFontContainer.FontData fontData = base.StyleFontContainer.GetFontData("Default");
			Font font = fontData.Font;
			float num2;
			if (!font.Characters.ContainsKey((int)character))
			{
				Font font2 = this._getUsableFontForCharacter((int)character) ?? fontData.Font;
				float num = fontData.FontSize / (float)font2.Size;
				num2 = font2.GetCharacterWidth(character, 0.5f) * num;
			}
			else
			{
				float num = fontData.FontSize / (float)font.Size;
				num2 = font.GetCharacterWidth(character, 0.5f) * num;
			}
			return num2;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000023B8 File Offset: 0x000005B8
		private void UpdateSelectedText(float dt, Vector2 mousePosition)
		{
			string text = this.VisibleText;
			this._visibleStart = Math.Min(this._visibleStart, this.CursorPosition);
			StyleFontContainer.FontData fontData = base.StyleFontContainer.GetFontData("Default");
			float num = fontData.FontSize / (float)fontData.Font.Size;
			int num2 = 10;
			int num3 = 0;
			while (num3 < this._visibleStart && text != "" && this.GetWordWidth(text) > (float)(base.Width - num2 - num2))
			{
				text = text.Substring(1);
				num3++;
			}
			this._visibleStart = num3;
			while (text.Length > this.CursorPosition - this._visibleStart && text != "")
			{
				if (this.GetWordWidth(text) <= (float)(base.Width - num2 - num2))
				{
					break;
				}
				text = text.Substring(0, text.Length - 1);
			}
			while (text != "" && this.GetWordWidth(text) > (float)(base.Width - num2 - num2))
			{
				text = text.Substring(1);
				num3++;
				this._visibleStart = Math.Min(this._visibleStart + 1, this.CursorPosition);
			}
			Vector2 vector = mousePosition;
			if (base.TextOutput != null && base.HorizontalAlignment != TextHorizontalAlignment.Left)
			{
				if (base.HorizontalAlignment == TextHorizontalAlignment.Center)
				{
					vector.X -= ((float)base.Width - base.TextOutput.GetLine(0).Width) * 0.5f;
				}
				else if (base.HorizontalAlignment == TextHorizontalAlignment.Right)
				{
					vector.X -= (float)base.Width - base.TextOutput.GetLine(0).Width;
				}
			}
			if (this.HighlightStart)
			{
				int num4 = this.FindCharacterPosition(dt, this.VisibleText, text, num, vector, num3);
				this.HighlightStart = false;
				this.SetCursor(num4, true, false);
				this.BeginSelection();
			}
			if (!this.HighlightEnd)
			{
				int num5 = this.FindCharacterPosition(dt, this.VisibleText, text, num, vector, num3);
				this.SetCursor(num5, true, true);
			}
			int num6 = Math.Min(Math.Max(this.SelectedTextBegin - num3, 0), text.Length);
			int num7 = Math.Min(Math.Max(this.SelectedTextEnd - num3, 0), text.Length);
			if (num6 > num7)
			{
				int num8 = num6;
				num6 = num7;
				num7 = num8;
			}
			string text2 = string.Concat(new string[]
			{
				text.Substring(0, num6),
				"<span style=\"Highlight\">",
				text.Substring(num6, num7 - num6),
				"</span>",
				text.Substring(num7, text.Length - num7)
			});
			this._realVisibleText = text.Substring(0, num6) + text.Substring(num6, num7 - num6) + text.Substring(num7, text.Length - num7);
			base.Value = text2;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002680 File Offset: 0x00000880
		public override void Update(float dt, SpriteData spriteData, Vector2 focusPosition, bool focus, bool isFixedWidth, bool isFixedHeight, float renderScale)
		{
			base.Update(dt, spriteData, focusPosition, focus, isFixedWidth, isFixedHeight, renderScale);
			this.UpdateSelectedText(dt, focusPosition);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000269B File Offset: 0x0000089B
		public void SelectAll()
		{
			this.SelectedTextBegin = 0;
			this._selectionAnchor = 0;
			this.SetCursor(this.VisibleText.Length, true, true);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000026C0 File Offset: 0x000008C0
		public int FindNextWordPosition(int direction)
		{
			MatchCollection matchCollection = this._nextWordRegex.Matches(this.VisibleText);
			int num = 0;
			int num2 = this.VisibleText.Length;
			foreach (object obj in matchCollection)
			{
				int index = ((Match)obj).Index;
				if (index < this.CursorPosition)
				{
					num = index;
				}
				else if (index > this.CursorPosition)
				{
					num2 = index;
					break;
				}
			}
			if (direction <= 0)
			{
				return num;
			}
			return num2;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002754 File Offset: 0x00000954
		public void SetCursor(int position, bool visible = true, bool withSelection = false)
		{
			this.BlinkTimer = 0f;
			int num = Mathf.Clamp(position, 0, this.VisibleText.Length);
			this.SetCursorPosition(num, visible);
			if (withSelection)
			{
				this.SelectedTextBegin = Math.Min(num, this._selectionAnchor);
				this.SelectedTextEnd = Math.Max(num, this._selectionAnchor);
				return;
			}
			this.SelectedTextBegin = 0;
			this.SelectedTextEnd = 0;
			this._selectionAnchor = 0;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000027C4 File Offset: 0x000009C4
		private int FindCharacterPosition(float dt, string fullText, string visibleText, float scale, Vector2 mousePosition, int omitCount)
		{
			if (mousePosition.X > (float)base.Width + 15f * scale)
			{
				int num = (int)((mousePosition.X - (float)base.Width) / (15f * scale));
				if (this._scrollTextWhileDraggingCooldown > 0f)
				{
					this._scrollTextWhileDraggingCooldown -= dt;
					return Math.Min(omitCount + visibleText.Length, fullText.Length);
				}
				this._scrollTextWhileDraggingCooldown = 0.033f;
				return Math.Min(omitCount + visibleText.Length + num, fullText.Length);
			}
			else
			{
				if (mousePosition.X >= -15f * scale)
				{
					this._scrollTextWhileDraggingCooldown = 0f;
					int i = 0;
					float num2 = 0f;
					while (i < visibleText.Length)
					{
						float num3 = num2;
						num2 += this.GetCharacterWidth(visibleText[i]);
						if (num2 > mousePosition.X)
						{
							float num4 = mousePosition.X - num3;
							if (num2 - mousePosition.X <= num4)
							{
								return omitCount + i + 1;
							}
							return omitCount + i;
						}
						else
						{
							i++;
						}
					}
					return omitCount + i;
				}
				int num5 = (int)(-mousePosition.X / (15f * scale));
				if (this._scrollTextWhileDraggingCooldown > 0f)
				{
					this._scrollTextWhileDraggingCooldown -= dt;
					return Math.Max(omitCount, 0);
				}
				this._scrollTextWhileDraggingCooldown = 0.033f;
				return Math.Max(omitCount - num5, 0);
			}
		}

		// Token: 0x04000009 RID: 9
		private bool _cursorVisible;

		// Token: 0x0400000B RID: 11
		private int _visibleStart;

		// Token: 0x04000010 RID: 16
		private int _selectionAnchor;

		// Token: 0x04000013 RID: 19
		private string _realVisibleText;

		// Token: 0x04000014 RID: 20
		private Regex _nextWordRegex;

		// Token: 0x04000015 RID: 21
		private float _scrollTextWhileDraggingCooldown;
	}
}
