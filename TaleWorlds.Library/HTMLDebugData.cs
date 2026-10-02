using System;
using System.IO;
using System.Net;
using System.Text;

namespace TaleWorlds.Library
{
	// Token: 0x0200005F RID: 95
	internal class HTMLDebugData
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x00008318 File Offset: 0x00006518
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x00008320 File Offset: 0x00006520
		internal HTMLDebugCategory Info { get; private set; }

		// Token: 0x060002B6 RID: 694 RVA: 0x0000832C File Offset: 0x0000652C
		internal HTMLDebugData(string log, HTMLDebugCategory info)
		{
			this._log = log;
			this.Info = info;
			this._currentTime = DateTime.Now.ToString("yyyy/M/d h:mm:ss.fff");
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x00008368 File Offset: 0x00006568
		private string Color
		{
			get
			{
				string text = "000000";
				switch (this.Info)
				{
				case HTMLDebugCategory.General:
					text = "000000";
					break;
				case HTMLDebugCategory.Connection:
					text = "FF00FF";
					break;
				case HTMLDebugCategory.IncomingMessage:
					text = "EE8800";
					break;
				case HTMLDebugCategory.OutgoingMessage:
					text = "AA6600";
					break;
				case HTMLDebugCategory.Database:
					text = "00008B";
					break;
				case HTMLDebugCategory.Warning:
					text = "0000FF";
					break;
				case HTMLDebugCategory.Error:
					text = "FF0000";
					break;
				case HTMLDebugCategory.Other:
					text = "000000";
					break;
				}
				return text;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x000083EC File Offset: 0x000065EC
		private ConsoleColor ConsoleColor
		{
			get
			{
				ConsoleColor consoleColor = ConsoleColor.Green;
				HTMLDebugCategory info = this.Info;
				if (info != HTMLDebugCategory.Warning)
				{
					if (info == HTMLDebugCategory.Error)
					{
						consoleColor = ConsoleColor.Red;
					}
				}
				else
				{
					consoleColor = ConsoleColor.Yellow;
				}
				return consoleColor;
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00008414 File Offset: 0x00006614
		internal void Print(FileStream fileStream, Encoding encoding, bool writeToConsole = true)
		{
			if (writeToConsole)
			{
				Console.ForegroundColor = this.ConsoleColor;
				Console.WriteLine(this._log);
				Console.ForegroundColor = this.ConsoleColor;
			}
			int byteCount = encoding.GetByteCount("</table>");
			string color = this.Color;
			string text = string.Concat(new string[]
			{
				"<tr>",
				this.TableCell(this._log, color).Replace("\n", "<br/>"),
				this.TableCell(this.Info.ToString(), color),
				this.TableCell(this._currentTime, color),
				"</tr></table>"
			});
			byte[] bytes = encoding.GetBytes(text);
			fileStream.Seek((long)(-(long)byteCount), SeekOrigin.End);
			fileStream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000084E0 File Offset: 0x000066E0
		private string TableCell(string innerText, string color)
		{
			return string.Concat(new string[]
			{
				"<td><font color='#",
				color,
				"'>",
				WebUtility.HtmlEncode(innerText),
				"</font></td><td>"
			});
		}

		// Token: 0x0400011E RID: 286
		private string _log;

		// Token: 0x04000120 RID: 288
		private string _currentTime;
	}
}
