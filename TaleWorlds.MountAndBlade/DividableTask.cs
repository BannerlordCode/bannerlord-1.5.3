using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200013F RID: 319
	public class DividableTask
	{
		// Token: 0x06000F56 RID: 3926 RVA: 0x00029322 File Offset: 0x00027522
		public DividableTask(DividableTask continueToTask = null)
		{
			this._continueToTask = continueToTask;
			this.ResetTaskStatus();
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00029337 File Offset: 0x00027537
		public void ResetTaskStatus()
		{
			this._isMainTaskFinished = false;
			this._isTaskCompletelyFinished = false;
			this._lastActionCalled = false;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x0002934E File Offset: 0x0002754E
		public void SetTaskFinished(bool callLastAction = false)
		{
			if (callLastAction)
			{
				Action lastAction = this._lastAction;
				if (lastAction != null)
				{
					lastAction();
				}
				this._lastActionCalled = true;
			}
			this._isTaskCompletelyFinished = true;
			this._isMainTaskFinished = true;
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0002937C File Offset: 0x0002757C
		public bool Update()
		{
			if (!this._isTaskCompletelyFinished)
			{
				if (!this._isMainTaskFinished && this.UpdateExtra())
				{
					this._isMainTaskFinished = true;
				}
				if (this._isMainTaskFinished)
				{
					DividableTask continueToTask = this._continueToTask;
					this._isTaskCompletelyFinished = continueToTask == null || continueToTask.Update();
				}
			}
			if (this._isTaskCompletelyFinished && !this._lastActionCalled)
			{
				Action lastAction = this._lastAction;
				if (lastAction != null)
				{
					lastAction();
				}
				this._lastActionCalled = true;
			}
			return this._isTaskCompletelyFinished;
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x000293F6 File Offset: 0x000275F6
		public void SetLastAction(Action action)
		{
			this._lastAction = action;
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x000293FF File Offset: 0x000275FF
		protected virtual bool UpdateExtra()
		{
			return true;
		}

		// Token: 0x040003C4 RID: 964
		private bool _isTaskCompletelyFinished;

		// Token: 0x040003C5 RID: 965
		private bool _isMainTaskFinished;

		// Token: 0x040003C6 RID: 966
		private bool _lastActionCalled;

		// Token: 0x040003C7 RID: 967
		private DividableTask _continueToTask;

		// Token: 0x040003C8 RID: 968
		private Action _lastAction;
	}
}
