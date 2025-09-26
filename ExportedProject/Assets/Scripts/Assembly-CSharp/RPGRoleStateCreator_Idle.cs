public class RPGRoleStateCreator_Idle : TStateFactory<RPGCenterController_Auto>
{
	public override TState<RPGCenterController_Auto> CreateState()
	{
		return new RPGRole_PlayerState_Idle();
	}
}
