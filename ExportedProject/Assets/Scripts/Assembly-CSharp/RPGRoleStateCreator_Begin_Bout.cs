public class RPGRoleStateCreator_Begin_Bout : TStateFactory<RPGCenterController_Auto>
{
	public override TState<RPGCenterController_Auto> CreateState()
	{
		return new RPGRole_PlayerState_Begin_Bout();
	}
}
