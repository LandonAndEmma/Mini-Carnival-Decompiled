public class RPGRoleStateCreator_Death : TStateFactory<RPGCenterController_Auto>
{
	public override TState<RPGCenterController_Auto> CreateState()
	{
		return new RPGRole_PlayerState_Death();
	}
}
