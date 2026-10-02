using System;
using System.Collections.ObjectModel;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000050 RID: 80
	public class MissionAgentTakenDamageVM : ViewModel
	{
		// Token: 0x0600068D RID: 1677 RVA: 0x00017B16 File Offset: 0x00015D16
		public MissionAgentTakenDamageVM(Camera missionCamera)
		{
			this._missionCamera = missionCamera;
			this.TakenDamageList = new MBBindingList<MissionAgentTakenDamageItemVM>();
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00017B30 File Offset: 0x00015D30
		public void SetIsEnabled(bool isEnabled)
		{
			this._isEnabled = isEnabled;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00017B3C File Offset: 0x00015D3C
		internal void Tick(float dt)
		{
			if (this._isEnabled)
			{
				for (int i = 0; i < this.TakenDamageList.Count; i++)
				{
					this.TakenDamageList[i].Update();
				}
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00017B78 File Offset: 0x00015D78
		internal void OnMainAgentHit(int damage, float distance)
		{
			if (this._isEnabled && damage > 0)
			{
				Collection<MissionAgentTakenDamageItemVM> takenDamageList = this.TakenDamageList;
				Camera missionCamera = this._missionCamera;
				Agent main = Agent.Main;
				takenDamageList.Add(new MissionAgentTakenDamageItemVM(missionCamera, (main != null) ? main.Position : default(Vec3), damage, false, new Action<MissionAgentTakenDamageItemVM>(this.OnRemoveDamageItem)));
			}
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00017BCE File Offset: 0x00015DCE
		private void OnRemoveDamageItem(MissionAgentTakenDamageItemVM item)
		{
			this.TakenDamageList.Remove(item);
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x00017BDD File Offset: 0x00015DDD
		// (set) Token: 0x06000693 RID: 1683 RVA: 0x00017BE5 File Offset: 0x00015DE5
		[DataSourceProperty]
		public MBBindingList<MissionAgentTakenDamageItemVM> TakenDamageList
		{
			get
			{
				return this._takenDamageList;
			}
			set
			{
				if (value != this._takenDamageList)
				{
					this._takenDamageList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionAgentTakenDamageItemVM>>(value, "TakenDamageList");
				}
			}
		}

		// Token: 0x040002EA RID: 746
		private Camera _missionCamera;

		// Token: 0x040002EB RID: 747
		private bool _isEnabled;

		// Token: 0x040002EC RID: 748
		private MBBindingList<MissionAgentTakenDamageItemVM> _takenDamageList;
	}
}
