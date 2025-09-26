public class RPGRoleStateCreator_Attack : TStateFactory<RPGCenterController_Auto>
{
	public override TState<RPGCenterController_Auto> CreateState()
	{
		return new RPGRole_PlayerState_Attack();
	}
}
