using System;
using System.Collections.Generic;

namespace TaleWorlds.Engine
{
	// Token: 0x02000054 RID: 84
	public class JobManager
	{
		// Token: 0x060008B0 RID: 2224 RVA: 0x00006D44 File Offset: 0x00004F44
		public JobManager()
		{
			this._jobs = new List<Job>();
			this._locker = new object();
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00006D64 File Offset: 0x00004F64
		public void AddJob(Job job)
		{
			object locker = this._locker;
			lock (locker)
			{
				this._jobs.Add(job);
			}
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00006DAC File Offset: 0x00004FAC
		internal void OnTick(float dt)
		{
			object locker = this._locker;
			lock (locker)
			{
				for (int i = 0; i < this._jobs.Count; i++)
				{
					Job job = this._jobs[i];
					job.DoJob(dt);
					if (job.Finished)
					{
						this._jobs.RemoveAt(i);
						i--;
					}
				}
			}
		}

		// Token: 0x040000B7 RID: 183
		private List<Job> _jobs;

		// Token: 0x040000B8 RID: 184
		private object _locker;
	}
}
