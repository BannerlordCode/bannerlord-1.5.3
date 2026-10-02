using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x02000030 RID: 48
	public class SaveOutput
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000A90D File Offset: 0x00008B0D
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000A915 File Offset: 0x00008B15
		public GameData Data { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000A91E File Offset: 0x00008B1E
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000A926 File Offset: 0x00008B26
		public SaveResult Result { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000A92F File Offset: 0x00008B2F
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000A937 File Offset: 0x00008B37
		public SaveError[] Errors { get; private set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000A940 File Offset: 0x00008B40
		public bool Successful
		{
			get
			{
				return this.Result == SaveResult.Success;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000A94B File Offset: 0x00008B4B
		public bool IsContinuing
		{
			get
			{
				Task<SaveResultWithMessage> continuingTask = this._continuingTask;
				return continuingTask != null && !continuingTask.IsCompleted;
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000A961 File Offset: 0x00008B61
		private SaveOutput()
		{
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A969 File Offset: 0x00008B69
		internal static SaveOutput CreateSuccessful(GameData data)
		{
			return new SaveOutput
			{
				Data = data,
				Result = SaveResult.Success
			};
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000A97E File Offset: 0x00008B7E
		internal static SaveOutput CreateFailed(IEnumerable<SaveError> errors, SaveResult result)
		{
			return new SaveOutput
			{
				Result = result,
				Errors = errors.ToArray<SaveError>()
			};
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A998 File Offset: 0x00008B98
		internal static SaveOutput CreateContinuing(Task<SaveResultWithMessage> continuingTask)
		{
			SaveOutput saveOutput = new SaveOutput();
			saveOutput._continuingTask = continuingTask;
			saveOutput._continuingTask.ContinueWith(delegate(Task<SaveResultWithMessage> t)
			{
				saveOutput.Result = t.Result.SaveResult;
			});
			return saveOutput;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A9E8 File Offset: 0x00008BE8
		public void PrintStatus()
		{
			Task<SaveResultWithMessage> continuingTask = this._continuingTask;
			if (continuingTask != null && continuingTask.IsCompleted)
			{
				this.Result = this._continuingTask.Result.SaveResult;
				this.Errors = new SaveError[0];
			}
			if (this.Result == SaveResult.Success)
			{
				Debug.Print("------Successfully saved------", 0, Debug.DebugColor.White, 17592186044416UL);
				return;
			}
			Debug.Print("Couldn't save because of errors listed below.", 0, Debug.DebugColor.White, 17592186044416UL);
			for (int i = 0; i < this.Errors.Length; i++)
			{
				SaveError saveError = this.Errors[i];
				Debug.Print(string.Concat(new object[] { "[", i, "]", saveError.Message }), 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.FailedAssert(string.Concat(new object[] { "SAVE FAILED: [", i, "]", saveError.Message, "\n" }), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\Save\\SaveOutput.cs", "PrintStatus", 74);
			}
			Debug.Print("--------------------", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x04000095 RID: 149
		private Task<SaveResultWithMessage> _continuingTask;
	}
}
