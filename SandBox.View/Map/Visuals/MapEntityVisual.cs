using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace SandBox.View.Map.Visuals
{
	// Token: 0x02000063 RID: 99
	public abstract class MapEntityVisual
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0001F105 File Offset: 0x0001D305
		public MapScreen MapScreen
		{
			get
			{
				return MapScreen.Instance;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060003EB RID: 1003
		public abstract CampaignVec2 InteractionPositionForPlayer { get; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060003EC RID: 1004
		public abstract MapEntityVisual AttachedTo { get; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0001F10C File Offset: 0x0001D30C
		public virtual bool IsMobileEntity
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0001F10F File Offset: 0x0001D30F
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x0001F117 File Offset: 0x0001D317
		public virtual MatrixFrame CircleLocalFrame { get; protected set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x0001F120 File Offset: 0x0001D320
		public virtual bool IsMainEntity
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x0001F123 File Offset: 0x0001D323
		public virtual float BearingRotation { get; }

		// Token: 0x060003F2 RID: 1010
		public abstract bool OnMapClick(bool followModifierUsed);

		// Token: 0x060003F3 RID: 1011
		public abstract void OnHover();

		// Token: 0x060003F4 RID: 1012
		public abstract void OnOpenEncyclopedia();

		// Token: 0x060003F5 RID: 1013
		public abstract bool IsVisibleOrFadingOut();

		// Token: 0x060003F6 RID: 1014
		public abstract Vec3 GetVisualPosition();

		// Token: 0x060003F7 RID: 1015 RVA: 0x0001F12B File Offset: 0x0001D32B
		public virtual void ReleaseResources()
		{
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001F12D File Offset: 0x0001D32D
		public virtual void OnHoverEnd()
		{
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0001F12F File Offset: 0x0001D32F
		public virtual void OnTrackAction()
		{
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0001F131 File Offset: 0x0001D331
		public virtual bool IsEnemyOf(IFaction faction)
		{
			return false;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0001F134 File Offset: 0x0001D334
		public virtual bool IsAllyOf(IFaction faction)
		{
			return false;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001F137 File Offset: 0x0001D337
		public virtual bool IsInSameFaction(IFaction faction)
		{
			return false;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0001F13A File Offset: 0x0001D33A
		public virtual bool IsInteractable()
		{
			return true;
		}
	}
}
