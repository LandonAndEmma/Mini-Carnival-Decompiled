public class RPGRoleStateCreator_Select_Aims_Manual : TStateFactory<RPGCenterController_Auto>
{
	public override TState<RPGCenterController_Auto> CreateState()
	{
		return new RPGRole_PlayerState_Select_Aims_Manual();
	}
}
