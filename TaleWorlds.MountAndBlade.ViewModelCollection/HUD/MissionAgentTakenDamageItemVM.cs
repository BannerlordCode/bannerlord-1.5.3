using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000051 RID: 81
	public class MissionAgentTakenDamageItemVM : ViewModel
	{
		// Token: 0x06000694 RID: 1684 RVA: 0x00017C03 File Offset: 0x00015E03
		public MissionAgentTakenDamageItemVM(Camera missionCamera, Vec3 affectorAgentPos, int damage, bool isRanged, Action<MissionAgentTakenDamageItemVM> onRemove)
		{
			this._affectorAgentPosition = affectorAgentPos;
			this.Damage = damage;
			this.IsRanged = isRanged;
			this._missionCamera = missionCamera;
			this._onRemove = onRemove;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00017C30 File Offset: 0x00015E30
		internal void Update()
		{
			if (this.IsRanged)
			{
				float num = 0f;
				float num2 = 0f;
				float num3 = 0f;
				MBWindowManager.WorldToScreen(this._missionCamera, this._affectorAgentPosition, ref num, ref num2, ref num3);
				this.ScreenPosOfAffectorAgent = new Vec2(num, num2);
				this.IsBehind = num3 < 0f;
			}
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00017C8A File Offset: 0x00015E8A
		public void ExecuteRemove()
		{
			this._onRemove(this);
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x00017C98 File Offset: 0x00015E98
		// (set) Token: 0x06000698 RID: 1688 RVA: 0x00017CA0 File Offset: 0x00015EA0
		[DataSourceProperty]
		public int Damage
		{
			get
			{
				return this._damage;
			}
			set
			{
				if (value != this._damage)
				{
					this._damage = value;
					base.OnPropertyChangedWithValue(value, "Damage");
				}
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x00017CBE File Offset: 0x00015EBE
		// (set) Token: 0x0600069A RID: 1690 RVA: 0x00017CC6 File Offset: 0x00015EC6
		[DataSourceProperty]
		public bool IsRanged
		{
			get
			{
				return this._isRanged;
			}
			set
			{
				if (value != this._isRanged)
				{
					this._isRanged = value;
					base.OnPropertyChangedWithValue(value, "IsRanged");
				}
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x00017CE4 File Offset: 0x00015EE4
		// (set) Token: 0x0600069C RID: 1692 RVA: 0x00017CEC File Offset: 0x00015EEC
		[DataSourceProperty]
		public bool IsBehind
		{
			get
			{
				return this._isBehind;
			}
			set
			{
				if (value != this._isBehind)
				{
					this._isBehind = value;
					base.OnPropertyChangedWithValue(value, "IsBehind");
				}
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x00017D0A File Offset: 0x00015F0A
		// (set) Token: 0x0600069E RID: 1694 RVA: 0x00017D12 File Offset: 0x00015F12
		[DataSourceProperty]
		public Vec2 ScreenPosOfAffectorAgent
		{
			get
			{
				return this._screenPosOfAffectorAgent;
			}
			set
			{
				if (value != this._screenPosOfAffectorAgent)
				{
					this._screenPosOfAffectorAgent = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosOfAffectorAgent");
				}
			}
		}

		// Token: 0x040002ED RID: 749
		private Action<MissionAgentTakenDamageItemVM> _onRemove;

		// Token: 0x040002EE RID: 750
		private Vec3 _affectorAgentPosition;

		// Token: 0x040002EF RID: 751
		private Camera _missionCamera;

		// Token: 0x040002F0 RID: 752
		private int _damage;

		// Token: 0x040002F1 RID: 753
		private bool _isBehind;

		// Token: 0x040002F2 RID: 754
		private bool _isRanged;

		// Token: 0x040002F3 RID: 755
		private Vec2 _screenPosOfAffectorAgent;
	}
}
