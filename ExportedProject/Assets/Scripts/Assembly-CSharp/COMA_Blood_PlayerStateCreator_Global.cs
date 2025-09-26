public class COMA_Blood_PlayerStateCreator_Global : TStateFactory<COMA_Blood_PlayerController>
{
	public override TState<COMA_Blood_PlayerController> CreateState()
	{
		return new COMA_Blood_PlayerState_Global();
	}
}
