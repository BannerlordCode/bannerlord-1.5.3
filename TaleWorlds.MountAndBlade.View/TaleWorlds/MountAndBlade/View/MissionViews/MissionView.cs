using System;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade.View.Screens;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000080 RID: 128
	public abstract class MissionView : MissionBehavior
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00024F87 File Offset: 0x00023187
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x00024F8F File Offset: 0x0002318F
		public MissionScreen MissionScreen { get; internal set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00024F98 File Offset: 0x00023198
		public IInputContext Input
		{
			get
			{
				return this.MissionScreen.SceneLayer.Input;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00024FAA File Offset: 0x000231AA
		// (set) Token: 0x060004DD RID: 1245 RVA: 0x00024FB2 File Offset: 0x000231B2
		private protected bool IsViewSuspended { protected get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00024FBB File Offset: 0x000231BB
		public override MissionBehaviorType BehaviorType
		{
			get
			{
				return MissionBehaviorType.Other;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00024FBE File Offset: 0x000231BE
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x00024FC6 File Offset: 0x000231C6
		public bool IsFinalized { get; internal set; }

		// Token: 0x060004E1 RID: 1249 RVA: 0x00024FCF File Offset: 0x000231CF
		public virtual void OnMissionScreenTick(float dt)
		{
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00024FD1 File Offset: 0x000231D1
		public virtual bool OnEscape()
		{
			return false;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00024FD4 File Offset: 0x000231D4
		public virtual bool IsOpeningEscapeMenuOnFocusChangeAllowed()
		{
			return true;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00024FD7 File Offset: 0x000231D7
		public virtual bool IsPhotoModeAllowed()
		{
			return true;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00024FDA File Offset: 0x000231DA
		public virtual void OnFocusChangeOnGameWindow(bool focusGained)
		{
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00024FDC File Offset: 0x000231DC
		public virtual void OnSceneRenderingStarted()
		{
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00024FDE File Offset: 0x000231DE
		public virtual void OnMissionScreenInitialize()
		{
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00024FE0 File Offset: 0x000231E0
		public virtual void OnMissionScreenFinalize()
		{
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00024FE2 File Offset: 0x000231E2
		public virtual void OnMissionScreenActivate()
		{
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00024FE4 File Offset: 0x000231E4
		public virtual void OnMissionScreenDeactivate()
		{
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00024FE6 File Offset: 0x000231E6
		public virtual bool UpdateOverridenCamera(float dt)
		{
			return false;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00024FE9 File Offset: 0x000231E9
		public virtual bool IsReady()
		{
			return true;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00024FEC File Offset: 0x000231EC
		public virtual void OnPhotoModeActivated()
		{
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00024FEE File Offset: 0x000231EE
		public virtual void OnPhotoModeDeactivated()
		{
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00024FF0 File Offset: 0x000231F0
		public virtual void OnConversationBegin()
		{
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00024FF2 File Offset: 0x000231F2
		public virtual void OnConversationEnd()
		{
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00024FF4 File Offset: 0x000231F4
		protected virtual void OnSuspendView()
		{
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00024FF6 File Offset: 0x000231F6
		protected virtual void OnResumeView()
		{
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00024FF8 File Offset: 0x000231F8
		public virtual void OnDeploymentPlanMade(Team team, bool isFirstPlan)
		{
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00024FFA File Offset: 0x000231FA
		public void SuspendView()
		{
			this.OnSuspendView();
			this.IsViewSuspended = true;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00025009 File Offset: 0x00023209
		public void ResumeView()
		{
			this.OnResumeView();
			this.IsViewSuspended = false;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00025018 File Offset: 0x00023218
		public sealed override void OnEndMissionInternal()
		{
			this.OnEndMission();
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00025020 File Offset: 0x00023220
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
		}

		// Token: 0x040002CA RID: 714
		public int ViewOrderPriority;
	}
}
