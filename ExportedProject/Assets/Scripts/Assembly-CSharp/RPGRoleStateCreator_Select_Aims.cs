public class RPGRoleStateCreator_Select_Aims : TStateFactory<RPGCenterController_Auto>
{
	public override TState<RPGCenterController_Auto> CreateState()
	{
		return new RPGRole_PlayerState_Select_Aims();
	}
}
