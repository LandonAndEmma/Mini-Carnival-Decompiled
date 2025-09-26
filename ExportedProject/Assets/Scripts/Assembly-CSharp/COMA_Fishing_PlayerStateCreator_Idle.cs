public class COMA_Fishing_PlayerStateCreator_Idle : TStateFactory<COMA_Fishing_PlayerController>
{
	public override TState<COMA_Fishing_PlayerController> CreateState()
	{
		return new COMA_Fishing_PlayerState_Idle();
	}
}
