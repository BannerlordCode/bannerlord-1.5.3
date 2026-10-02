using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000374 RID: 884
	public abstract class UsableMissionObjectComponent
	{
		// Token: 0x06003300 RID: 13056 RVA: 0x000D13B4 File Offset: 0x000CF5B4
		protected internal virtual void OnAdded(Scene scene)
		{
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x000D13B6 File Offset: 0x000CF5B6
		protected internal virtual void OnRemoved()
		{
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x000D13B8 File Offset: 0x000CF5B8
		protected internal virtual void OnFocusGain(Agent userAgent)
		{
		}

		// Token: 0x06003303 RID: 13059 RVA: 0x000D13BA File Offset: 0x000CF5BA
		protected internal virtual void OnFocusLose(Agent userAgent)
		{
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x000D13BC File Offset: 0x000CF5BC
		public virtual bool IsOnTickRequired()
		{
			return false;
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x000D13BF File Offset: 0x000CF5BF
		protected internal virtual void OnTick(float dt)
		{
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x000D13C1 File Offset: 0x000CF5C1
		protected internal virtual void OnEditorTick(float dt)
		{
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x000D13C3 File Offset: 0x000CF5C3
		protected internal virtual void OnEditorValidate()
		{
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x000D13C5 File Offset: 0x000CF5C5
		protected internal virtual void OnUse(Agent userAgent)
		{
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x000D13C7 File Offset: 0x000CF5C7
		protected internal virtual void OnUseStopped(Agent userAgent, bool isSuccessful = true)
		{
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x000D13C9 File Offset: 0x000CF5C9
		protected internal virtual void OnMissionReset()
		{
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x000D13CB File Offset: 0x000CF5CB
		protected internal virtual void OnMissionObjectDisabled()
		{
		}
	}
}
