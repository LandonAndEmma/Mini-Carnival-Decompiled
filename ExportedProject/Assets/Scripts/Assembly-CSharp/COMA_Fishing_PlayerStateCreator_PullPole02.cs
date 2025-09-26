public class COMA_Fishing_PlayerStateCreator_PullPole02 : TStateFactory<COMA_Fishing_PlayerController>
{
	public override TState<COMA_Fishing_PlayerController> CreateState()
	{
		return new COMA_Fishing_PlayerState_PullPole02();
	}
}
