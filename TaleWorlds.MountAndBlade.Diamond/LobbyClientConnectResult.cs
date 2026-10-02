using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Diamond;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000128 RID: 296
	public class LobbyClientConnectResult
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x0000BA78 File Offset: 0x00009C78
		// (set) Token: 0x060007AC RID: 1964 RVA: 0x0000BA80 File Offset: 0x00009C80
		public bool Connected { get; private set; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x0000BA89 File Offset: 0x00009C89
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x0000BA91 File Offset: 0x00009C91
		public TextObject Error { get; private set; }

		// Token: 0x060007AF RID: 1967 RVA: 0x0000BA9A File Offset: 0x00009C9A
		public LobbyClientConnectResult(bool connected, TextObject error)
		{
			this.Connected = connected;
			this.Error = error;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0000BAB0 File Offset: 0x00009CB0
		public static LobbyClientConnectResult FromServerConnectResult(string errorCode, Dictionary<string, string> parameters)
		{
			TextObject textObject = GameTexts.FindText("str_login_error", errorCode);
			if (textObject == null)
			{
				Debug.FailedAssert("Error text is not handled: " + errorCode, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\LobbyClient.cs", "FromServerConnectResult", 2345);
				textObject = new TextObject("{=tzQxtv27}Unknown error.", null);
			}
			else if (parameters != null)
			{
				foreach (string text in parameters.Keys)
				{
					if (text == "BANREASON")
					{
						if (parameters[text].StartsWith("Custom:"))
						{
							textObject.SetTextVariable(text, parameters[text].Substring("Custom:".Length));
						}
						else
						{
							TextObject textObject2 = GameTexts.FindText("str_ban_reason", parameters[text]);
							textObject.SetTextVariable(text, textObject2.ToString());
						}
					}
					else if (text == "ACCESSERROR")
					{
						TextObject textObject3 = GameTexts.FindText("str_access_error", parameters[text]);
						textObject.SetTextVariable(text, textObject3.ToString());
					}
					else
					{
						textObject.SetTextVariable(text, parameters[text]);
					}
				}
			}
			return new LobbyClientConnectResult(errorCode == LoginErrorCode.None.ToString(), textObject);
		}
	}
}
