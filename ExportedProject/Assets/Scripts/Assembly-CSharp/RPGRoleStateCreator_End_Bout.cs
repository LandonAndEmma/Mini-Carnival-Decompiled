public class RPGRoleStateCreator_End_Bout : TStateFactory<RPGCenterController_Auto>
{
	public override TState<RPGCenterController_Auto> CreateState()
	{
		return new RPGRole_PlayerState_End_Bout();
	}
}
