using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TaleWorlds.InputSystem;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200005D RID: 93
	public class IntegerInputTextWidget : EditableTextWidget
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x0001AEFC File Offset: 0x000190FC
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x0001AF04 File Offset: 0x00019104
		public bool EnableClamp { get; set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x0001AF0D File Offset: 0x0001910D
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x0001AF15 File Offset: 0x00019115
		public bool UpdateValueOnDone { get; set; }

		// Token: 0x0600064E RID: 1614 RVA: 0x0001AF1E File Offset: 0x0001911E
		public IntegerInputTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001AF44 File Offset: 0x00019144
		public override void HandleInput(IReadOnlyList<int> lastKeysPressed)
		{
			int count = lastKeysPressed.Count;
			for (int i = 0; i < count; i++)
			{
				int num = lastKeysPressed[i];
				if (char.IsDigit(Convert.ToChar(num)))
				{
					if (num != 60 && num != 62)
					{
						this.HandleInput(num);
					}
					this._cursorDirection = EditableTextWidget.CursorMovementDirection.None;
					this._isSelection = false;
				}
			}
			int tickCount = Environment.TickCount;
			bool flag = false;
			bool flag2 = false;
			if (Input.IsKeyPressed(InputKey.Left))
			{
				this._cursorDirection = EditableTextWidget.CursorMovementDirection.Left;
				flag = true;
			}
			else if (Input.IsKeyPressed(InputKey.Right))
			{
				this._cursorDirection = EditableTextWidget.CursorMovementDirection.Right;
				flag = true;
			}
			else if ((this._cursorDirection == EditableTextWidget.CursorMovementDirection.Left && !Input.IsKeyDown(InputKey.Left)) || (this._cursorDirection == EditableTextWidget.CursorMovementDirection.Right && !Input.IsKeyDown(InputKey.Right)))
			{
				this._cursorDirection = EditableTextWidget.CursorMovementDirection.None;
				if (!Input.IsKeyDown(InputKey.LeftShift))
				{
					this._isSelection = false;
				}
			}
			else if (Input.IsKeyReleased(InputKey.LeftShift))
			{
				this._isSelection = false;
			}
			else if (Input.IsKeyDown(InputKey.Home))
			{
				this._cursorDirection = EditableTextWidget.CursorMovementDirection.Left;
				flag2 = true;
			}
			else if (Input.IsKeyDown(InputKey.End))
			{
				this._cursorDirection = EditableTextWidget.CursorMovementDirection.Right;
				flag2 = true;
			}
			if (flag || flag2)
			{
				this._nextRepeatTime = tickCount + 500;
				if (Input.IsKeyDown(InputKey.LeftShift))
				{
					if (!this._editableText.IsAnySelected())
					{
						this._editableText.BeginSelection();
					}
					this._isSelection = true;
				}
			}
			if (this._cursorDirection != EditableTextWidget.CursorMovementDirection.None && (flag || flag2 || tickCount >= this._nextRepeatTime))
			{
				if (flag)
				{
					int num2 = (int)this._cursorDirection;
					if (Input.IsKeyDown(InputKey.LeftControl))
					{
						num2 = base.FindNextWordPosition(num2) - this._editableText.CursorPosition;
					}
					base.MoveCursor(num2, this._isSelection);
					if (tickCount >= this._nextRepeatTime)
					{
						this._nextRepeatTime = tickCount + 30;
					}
				}
				else if (flag2)
				{
					int num3 = ((this._cursorDirection == EditableTextWidget.CursorMovementDirection.Left) ? (-this._editableText.CursorPosition) : (this._editableText.VisibleText.Length - this._editableText.CursorPosition));
					base.MoveCursor(num3, this._isSelection);
					if (tickCount >= this._nextRepeatTime)
					{
						this._nextRepeatTime = tickCount + 30;
					}
				}
			}
			bool flag3 = false;
			if (Input.IsKeyPressed(InputKey.BackSpace))
			{
				flag3 = true;
				this._keyboardAction = EditableTextWidget.KeyboardAction.BackSpace;
				this._nextRepeatTime = tickCount + 500;
			}
			else if (Input.IsKeyPressed(InputKey.Delete))
			{
				flag3 = true;
				this._keyboardAction = EditableTextWidget.KeyboardAction.Delete;
				this._nextRepeatTime = tickCount + 500;
			}
			if ((this._keyboardAction == EditableTextWidget.KeyboardAction.BackSpace && !Input.IsKeyDown(InputKey.BackSpace)) || (this._keyboardAction == EditableTextWidget.KeyboardAction.Delete && !Input.IsKeyDown(InputKey.Delete)))
			{
				this._keyboardAction = EditableTextWidget.KeyboardAction.None;
			}
			if (Input.IsKeyReleased(InputKey.Enter) || Input.IsKeyReleased(InputKey.NumpadEnter))
			{
				int num4;
				if (int.TryParse(base.RealText, out num4))
				{
					this.ForceSetInteger(num4);
				}
				base.EventFired("TextEntered", Array.Empty<object>());
				return;
			}
			if (this._keyboardAction == EditableTextWidget.KeyboardAction.BackSpace || this._keyboardAction == EditableTextWidget.KeyboardAction.Delete)
			{
				if (flag3 || tickCount >= this._nextRepeatTime)
				{
					if (this._editableText.IsAnySelected())
					{
						base.DeleteText(this._editableText.SelectedTextBegin, this._editableText.SelectedTextEnd);
					}
					else if (Input.IsKeyDown(InputKey.LeftControl))
					{
						if (this._keyboardAction == EditableTextWidget.KeyboardAction.BackSpace)
						{
							base.DeleteText(base.FindNextWordPosition(-1), this._editableText.CursorPosition);
						}
						else
						{
							base.DeleteText(this._editableText.CursorPosition, base.FindNextWordPosition(1));
						}
					}
					else
					{
						base.DeleteChar(this._keyboardAction == EditableTextWidget.KeyboardAction.Delete);
					}
					this.TrySetStringAsInteger(base.RealText);
					if (tickCount >= this._nextRepeatTime)
					{
						this._nextRepeatTime = tickCount + 30;
						return;
					}
				}
			}
			else if (Input.IsKeyDown(InputKey.LeftControl) && !Input.IsKeyDown(InputKey.RightAlt))
			{
				if (Input.IsKeyPressed(InputKey.A))
				{
					this._editableText.SelectAll();
					return;
				}
				if (Input.IsKeyPressed(InputKey.C))
				{
					base.CopyText(this._editableText.SelectedTextBegin, this._editableText.SelectedTextEnd);
					return;
				}
				if (Input.IsKeyPressed(InputKey.X))
				{
					base.CopyText(this._editableText.SelectedTextBegin, this._editableText.SelectedTextEnd);
					base.DeleteText(this._editableText.SelectedTextBegin, this._editableText.SelectedTextEnd);
					this.TrySetStringAsInteger(base.RealText);
					return;
				}
				if (Input.IsKeyPressed(InputKey.V))
				{
					base.DeleteText(this._editableText.SelectedTextBegin, this._editableText.SelectedTextEnd);
					string text = Regex.Replace(Input.GetClipboardText(), "[<>]+", " ");
					text = new string(text.Where<char>((char c) => char.IsDigit(c)).ToArray<char>());
					base.AppendText(text);
					this.TrySetStringAsInteger(base.RealText);
				}
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001B404 File Offset: 0x00019604
		private void HandleInput(int lastPressedKey)
		{
			bool flag = false;
			string text = base.RealText;
			string text2;
			if (this._editableText.SelectedTextBegin != this._editableText.SelectedTextEnd)
			{
				if (this._editableText.SelectedTextEnd > base.RealText.Length)
				{
					text2 = Convert.ToChar(lastPressedKey).ToString();
					flag = true;
				}
				else
				{
					text = base.RealText.Substring(0, this._editableText.SelectedTextBegin) + base.RealText.Substring(this._editableText.SelectedTextEnd, base.RealText.Length - this._editableText.SelectedTextEnd);
					if (this._editableText.SelectedTextEnd - this._editableText.SelectedTextBegin >= base.RealText.Length)
					{
						this._editableText.SetCursorPosition(0, true);
						this._editableText.ResetSelected();
						flag = true;
					}
					else
					{
						this._editableText.SetCursorPosition(this._editableText.SelectedTextBegin, true);
					}
					int cursorPosition = this._editableText.CursorPosition;
					char c = Convert.ToChar(lastPressedKey);
					text2 = text.Substring(0, cursorPosition) + c.ToString() + text.Substring(cursorPosition, text.Length - cursorPosition);
				}
				this._editableText.ResetSelected();
			}
			else if (base.MaxLength > -1 && base.Text.Length >= base.MaxLength)
			{
				text2 = base.RealText;
			}
			else
			{
				if (this._editableText.CursorPosition == base.RealText.Length)
				{
					flag = true;
				}
				int cursorPosition2 = this._editableText.CursorPosition;
				char c2 = Convert.ToChar(lastPressedKey);
				text2 = text.Substring(0, cursorPosition2) + c2.ToString() + text.Substring(cursorPosition2, text.Length - cursorPosition2);
				if (!flag)
				{
					this._editableText.SetCursor(cursorPosition2 + 1, true, false);
				}
			}
			this.TrySetStringAsInteger(text2);
			if (flag)
			{
				this._editableText.SetCursorPosition(base.RealText.Length, true);
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001B603 File Offset: 0x00019803
		private void SetInteger(int newInteger)
		{
			if (this.UpdateValueOnDone)
			{
				base.RealText = newInteger.ToString();
				base.Text = newInteger.ToString();
				return;
			}
			this.ForceSetInteger(newInteger);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001B630 File Offset: 0x00019830
		private void ForceSetInteger(int newInteger)
		{
			if (this.EnableClamp && (newInteger > this.MaxInt || newInteger < this.MinInt))
			{
				newInteger = ((newInteger > this.MaxInt) ? this.MaxInt : this.MinInt);
				base.ResetSelected();
			}
			this.IntText = newInteger;
			if (this.IntText.ToString() != base.RealText)
			{
				base.RealText = this.IntText.ToString();
				base.Text = this.IntText.ToString();
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0001B6C0 File Offset: 0x000198C0
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			int num;
			if (!base.IsFocused && int.TryParse(base.RealText, out num))
			{
				this.ForceSetInteger(num);
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0001B6F4 File Offset: 0x000198F4
		private bool TrySetStringAsInteger(string str)
		{
			int num;
			if (!int.TryParse(str, out num))
			{
				if (!string.IsNullOrWhiteSpace(str))
				{
					return false;
				}
				num = 0;
			}
			this.SetInteger(num);
			if (this._editableText.SelectedTextEnd - this._editableText.SelectedTextBegin >= base.RealText.Length)
			{
				this._editableText.SetCursorPosition(0, true);
				this._editableText.ResetSelected();
			}
			else if (this._editableText.SelectedTextBegin != 0 || this._editableText.SelectedTextEnd != 0)
			{
				this._editableText.SetCursorPosition(this._editableText.SelectedTextBegin, true);
			}
			if (this._editableText.CursorPosition > base.RealText.Length)
			{
				this._editableText.SetCursorPosition(base.RealText.Length, true);
			}
			return true;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0001B7C0 File Offset: 0x000199C0
		public override void SetAllText(string text)
		{
			base.DeleteText(0, base.RealText.Length);
			string text2 = Regex.Replace(text, "[<>]+", " ");
			text2 = new string(text2.Where<char>((char c) => char.IsDigit(c)).ToArray<char>());
			this.TrySetStringAsInteger(text2);
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x0001B828 File Offset: 0x00019A28
		// (set) Token: 0x06000657 RID: 1623 RVA: 0x0001B830 File Offset: 0x00019A30
		[Editor(false)]
		public int IntText
		{
			get
			{
				return this._intText;
			}
			set
			{
				if (this._intText != value)
				{
					this._intText = value;
					base.OnPropertyChanged(value, "IntText");
					base.RealText = value.ToString();
					base.Text = value.ToString();
				}
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x0001B868 File Offset: 0x00019A68
		// (set) Token: 0x06000659 RID: 1625 RVA: 0x0001B870 File Offset: 0x00019A70
		[Editor(false)]
		public int MaxInt
		{
			get
			{
				return this._maxInt;
			}
			set
			{
				if (this._maxInt != value)
				{
					this._maxInt = value;
				}
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x0001B882 File Offset: 0x00019A82
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x0001B88A File Offset: 0x00019A8A
		[Editor(false)]
		public int MinInt
		{
			get
			{
				return this._minInt;
			}
			set
			{
				if (this._minInt != value)
				{
					this._minInt = value;
				}
			}
		}

		// Token: 0x040002FB RID: 763
		private int _intText = -1;

		// Token: 0x040002FC RID: 764
		private int _maxInt = int.MaxValue;

		// Token: 0x040002FD RID: 765
		private int _minInt = int.MinValue;
	}
}
