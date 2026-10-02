using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x02000008 RID: 8
	public static class NativeMessageBox
	{
		// Token: 0x06000044 RID: 68
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);

		// Token: 0x06000045 RID: 69 RVA: 0x00002A10 File Offset: 0x00000C10
		public static NativeMessageBox.Result Show(string text, string caption = "Message", NativeMessageBox.Buttons buttons = NativeMessageBox.Buttons.OK, NativeMessageBox.Icon icon = NativeMessageBox.Icon.None)
		{
			switch (NativeMessageBox.MessageBox(IntPtr.Zero, text, caption, (uint)(buttons | (NativeMessageBox.Buttons)icon)))
			{
			case 1:
				return NativeMessageBox.Result.OK;
			case 2:
				return NativeMessageBox.Result.Cancel;
			case 6:
				return NativeMessageBox.Result.Yes;
			case 7:
				return NativeMessageBox.Result.No;
			}
			return NativeMessageBox.Result.OK;
		}

		// Token: 0x02000029 RID: 41
		public enum Buttons : uint
		{
			// Token: 0x040000C7 RID: 199
			OK,
			// Token: 0x040000C8 RID: 200
			OKCancel,
			// Token: 0x040000C9 RID: 201
			YesNo = 4U,
			// Token: 0x040000CA RID: 202
			YesNoCancel = 3U
		}

		// Token: 0x0200002A RID: 42
		public enum Icon : uint
		{
			// Token: 0x040000CC RID: 204
			None,
			// Token: 0x040000CD RID: 205
			Information = 64U,
			// Token: 0x040000CE RID: 206
			Warning = 48U,
			// Token: 0x040000CF RID: 207
			Error = 16U,
			// Token: 0x040000D0 RID: 208
			Question = 32U
		}

		// Token: 0x0200002B RID: 43
		public enum Result
		{
			// Token: 0x040000D2 RID: 210
			OK = 1,
			// Token: 0x040000D3 RID: 211
			Cancel,
			// Token: 0x040000D4 RID: 212
			Yes = 6,
			// Token: 0x040000D5 RID: 213
			No
		}
	}
}
