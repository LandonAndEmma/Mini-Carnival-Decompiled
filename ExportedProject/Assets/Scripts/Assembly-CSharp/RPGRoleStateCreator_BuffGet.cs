public class RPGRoleStateCreator_BuffGet : TStateFactory<RPGCenterController_Auto>
{
	public override TState<RPGCenterController_Auto> CreateState()
	{
		return new RPGRole_PlayerState_BuffGet();
	}
}
