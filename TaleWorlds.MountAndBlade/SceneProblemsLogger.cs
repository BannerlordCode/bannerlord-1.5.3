using System;
using System.IO;
using System.Text;
using TaleWorlds.Engine;
using TaleWorlds.Library;

// Token: 0x02000005 RID: 5
public class SceneProblemsLogger
{
	// Token: 0x06000004 RID: 4 RVA: 0x00002058 File Offset: 0x00000258
	public SceneProblemsLogger(string fileName)
	{
		this._logPath = Utilities.GetLocalOutputPath() + "logs/";
		if (!Directory.Exists(this._logPath))
		{
			Directory.CreateDirectory(this._logPath);
		}
		int num = fileName.LastIndexOf('.');
		if (num != -1)
		{
			fileName.Substring(num);
		}
		else
		{
			fileName += ".txt";
		}
		this._fileName = fileName;
		MBDebug.Print("Log file creating.. (" + this._logPath + fileName + ")", 0, Debug.DebugColor.White, 17592186044416UL);
		string text = DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss");
		string text2 = string.Concat(new string[]
		{
			"==================== Scene Problems Log ====================",
			Environment.NewLine,
			"Created : ",
			text,
			Environment.NewLine,
			"============================================================",
			Environment.NewLine
		});
		File.WriteAllText(this._logPath + fileName, text2, Encoding.UTF8);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000216C File Offset: 0x0000036C
	public void LogScene(int sceneIndex, string sceneId, string log)
	{
		string text = DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss");
		string text2 = string.Concat(new string[]
		{
			Environment.NewLine,
			"------------------------------------------------------------",
			Environment.NewLine,
			string.Format("Index   : {0}", sceneIndex),
			Environment.NewLine,
			"Scene   : ",
			sceneId,
			Environment.NewLine,
			"Logged  : ",
			text,
			Environment.NewLine,
			"------------------------------------------------------------",
			Environment.NewLine,
			log.TrimEnd(Array.Empty<char>()),
			Environment.NewLine
		});
		File.AppendAllText(this._logPath + this._fileName, text2, Encoding.UTF8);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x0000223C File Offset: 0x0000043C
	public void FinishLogging()
	{
		string text = Environment.NewLine + "============================================================";
		File.AppendAllText(this._logPath + this._fileName, text);
	}

	// Token: 0x04000001 RID: 1
	private string _logPath = "";

	// Token: 0x04000002 RID: 2
	private string _fileName = "";
}
