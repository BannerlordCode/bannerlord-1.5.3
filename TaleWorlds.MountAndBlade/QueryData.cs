using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200017F RID: 383
	public class QueryData<T> : IQueryData
	{
		// Token: 0x0600144C RID: 5196 RVA: 0x0004A696 File Offset: 0x00048896
		public QueryData(Func<T> valueFunc, float lifetime)
		{
			this._cachedValue = default(T);
			this._expireTime = 0f;
			this._lifetime = lifetime;
			this._valueFunc = valueFunc;
			this._syncGroup = null;
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0004A6CA File Offset: 0x000488CA
		public QueryData(Func<T> valueFunc, float lifetime, T defaultCachedValue)
		{
			this._cachedValue = defaultCachedValue;
			this._expireTime = 0f;
			this._lifetime = lifetime;
			this._valueFunc = valueFunc;
			this._syncGroup = null;
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0004A6F9 File Offset: 0x000488F9
		public void Evaluate(float currentTime)
		{
			this.SetValue(this._valueFunc(), currentTime);
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0004A70D File Offset: 0x0004890D
		public void SetValue(T value, float currentTime)
		{
			this._cachedValue = value;
			this._expireTime = currentTime + this._lifetime;
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0004A724 File Offset: 0x00048924
		public T GetCachedValue()
		{
			return this._cachedValue;
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0004A72C File Offset: 0x0004892C
		public T GetCachedValueUnlessTooOld()
		{
			return this._cachedValue;
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0004A734 File Offset: 0x00048934
		public T GetCachedValueWithMaxAge(float age)
		{
			if (Mission.Current.CurrentTime > this._expireTime - this._lifetime + MathF.Min(this._lifetime, age))
			{
				this.Expire();
				return this.Value;
			}
			return this._cachedValue;
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001453 RID: 5203 RVA: 0x0004A770 File Offset: 0x00048970
		public T Value
		{
			get
			{
				float currentTime = Mission.Current.CurrentTime;
				if (currentTime >= this._expireTime)
				{
					if (this._syncGroup != null)
					{
						IQueryData[] syncGroup = this._syncGroup;
						for (int i = 0; i < syncGroup.Length; i++)
						{
							syncGroup[i].Evaluate(currentTime);
						}
					}
					this.Evaluate(currentTime);
				}
				return this._cachedValue;
			}
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0004A7C4 File Offset: 0x000489C4
		public void Expire()
		{
			this._expireTime = 0f;
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0004A7D4 File Offset: 0x000489D4
		public static void SetupSyncGroup(params IQueryData[] groupItems)
		{
			for (int i = 0; i < groupItems.Length; i++)
			{
				groupItems[i].SetSyncGroup(groupItems);
			}
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0004A7FA File Offset: 0x000489FA
		public void SetSyncGroup(IQueryData[] syncGroup)
		{
			this._syncGroup = syncGroup;
		}

		// Token: 0x04000540 RID: 1344
		private T _cachedValue;

		// Token: 0x04000541 RID: 1345
		private float _expireTime;

		// Token: 0x04000542 RID: 1346
		private readonly float _lifetime;

		// Token: 0x04000543 RID: 1347
		private readonly Func<T> _valueFunc;

		// Token: 0x04000544 RID: 1348
		private IQueryData[] _syncGroup;
	}
}
