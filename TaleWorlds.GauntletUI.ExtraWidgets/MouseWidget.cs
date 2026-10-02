using System;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x0200000F RID: 15
	public class MouseWidget : Widget
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x000058BB File Offset: 0x00003ABB
		public MouseWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000058C4 File Offset: 0x00003AC4
		protected override void OnUpdate(float dt)
		{
			if (base.IsVisible)
			{
				this.UpdatePressedKeys();
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000058D4 File Offset: 0x00003AD4
		public void UpdatePressedKeys()
		{
			Color color = new Color(1f, 0f, 0f, 1f);
			this.LeftMouseButton.Color = Color.White;
			this.RightMouseButton.Color = Color.White;
			this.MiddleMouseButton.Color = Color.White;
			this.MouseX1Button.Color = Color.White;
			this.MouseX2Button.Color = Color.White;
			this.MouseScrollUp.IsVisible = false;
			this.MouseScrollDown.IsVisible = false;
			this.KeyboardKeys.Text = "";
			if (Input.IsKeyDown(InputKey.LeftMouseButton))
			{
				this.LeftMouseButton.Color = color;
			}
			if (Input.IsKeyDown(InputKey.RightMouseButton))
			{
				this.RightMouseButton.Color = color;
			}
			if (Input.IsKeyDown(InputKey.MiddleMouseButton))
			{
				this.MiddleMouseButton.Color = color;
			}
			if (Input.IsKeyDown(InputKey.X1MouseButton))
			{
				this.MouseX1Button.Color = color;
			}
			if (Input.IsKeyDown(InputKey.X2MouseButton))
			{
				this.MouseX2Button.Color = color;
			}
			if (Input.IsKeyDown(InputKey.MouseScrollUp))
			{
				this.MouseScrollUp.IsVisible = true;
			}
			if (Input.IsKeyDown(InputKey.MouseScrollDown))
			{
				this.MouseScrollDown.IsVisible = true;
			}
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "UpdatePressedKeys");
			for (int i = 0; i < 256; i++)
			{
				if (Key.GetInputType((InputKey)i) == Key.InputType.Keyboard && Input.IsKeyDown((InputKey)i))
				{
					InputKey inputKey = (InputKey)i;
					mbstringBuilder.Append<string>(inputKey.ToString());
					mbstringBuilder.Append<string>(", ");
				}
			}
			this.KeyboardKeys.Text = mbstringBuilder.ToStringAndRelease().TrimEnd(MouseWidget._trimChars);
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00005A92 File Offset: 0x00003C92
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00005A9A File Offset: 0x00003C9A
		public Widget LeftMouseButton { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00005AA3 File Offset: 0x00003CA3
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00005AAB File Offset: 0x00003CAB
		public Widget RightMouseButton { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00005AB4 File Offset: 0x00003CB4
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00005ABC File Offset: 0x00003CBC
		public Widget MiddleMouseButton { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00005AC5 File Offset: 0x00003CC5
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00005ACD File Offset: 0x00003CCD
		public Widget MouseX1Button { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00005AD6 File Offset: 0x00003CD6
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00005ADE File Offset: 0x00003CDE
		public Widget MouseX2Button { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00005AE7 File Offset: 0x00003CE7
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00005AEF File Offset: 0x00003CEF
		public Widget MouseScrollUp { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00005AF8 File Offset: 0x00003CF8
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00005B00 File Offset: 0x00003D00
		public Widget MouseScrollDown { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00005B09 File Offset: 0x00003D09
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00005B11 File Offset: 0x00003D11
		public TextWidget KeyboardKeys { get; set; }

		// Token: 0x0400005D RID: 93
		private static readonly char[] _trimChars = new char[] { ' ', ',' };
	}
}
