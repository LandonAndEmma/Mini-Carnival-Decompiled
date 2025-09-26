public class COMA_Blood_PlayerStateCreator_Idle : TStateFactory<COMA_Blood_PlayerController>
{
	public override TState<COMA_Blood_PlayerController> CreateState()
	{
		return new COMA_Blood_PlayerState_Idle();
	}
}
