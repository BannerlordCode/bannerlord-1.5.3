using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000BB RID: 187
	public class MissionSiegeWeapon : IMissionSiegeWeapon
	{
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x00020C8F File Offset: 0x0001EE8F
		public int Index
		{
			get
			{
				return this._index;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x00020C97 File Offset: 0x0001EE97
		public SiegeEngineType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x00020C9F File Offset: 0x0001EE9F
		public float Health
		{
			get
			{
				return this._health;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x00020CA7 File Offset: 0x0001EEA7
		public float InitialHealth
		{
			get
			{
				return this._initialHealth;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060009F6 RID: 2550 RVA: 0x00020CAF File Offset: 0x0001EEAF
		public float MaxHealth
		{
			get
			{
				return this._maxHealth;
			}
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00020CB7 File Offset: 0x0001EEB7
		private MissionSiegeWeapon(int index, SiegeEngineType type, float health, float maxHealth)
		{
			this._index = index;
			this._type = type;
			this._initialHealth = health;
			this._health = this._initialHealth;
			this._maxHealth = maxHealth;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00020CE8 File Offset: 0x0001EEE8
		public static MissionSiegeWeapon CreateDefaultWeapon(SiegeEngineType type)
		{
			return new MissionSiegeWeapon(-1, type, (float)type.BaseHitPoints, (float)type.BaseHitPoints);
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00020CFF File Offset: 0x0001EEFF
		public static MissionSiegeWeapon CreateCampaignWeapon(SiegeEngineType type, int index, float health, float maxHealth)
		{
			return new MissionSiegeWeapon(index, type, health, maxHealth);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00020D0A File Offset: 0x0001EF0A
		public void SetHealth(float health)
		{
			this._health = health;
		}

		// Token: 0x04000576 RID: 1398
		private float _health;

		// Token: 0x04000577 RID: 1399
		private readonly int _index;

		// Token: 0x04000578 RID: 1400
		private readonly SiegeEngineType _type;

		// Token: 0x04000579 RID: 1401
		private readonly float _initialHealth;

		// Token: 0x0400057A RID: 1402
		private readonly float _maxHealth;
	}
}
