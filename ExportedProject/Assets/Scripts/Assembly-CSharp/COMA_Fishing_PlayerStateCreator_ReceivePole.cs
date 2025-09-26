public class COMA_Fishing_PlayerStateCreator_ReceivePole : TStateFactory<COMA_Fishing_PlayerController>
{
	public override TState<COMA_Fishing_PlayerController> CreateState()
	{
		return new COMA_Fishing_PlayerState_ReceivePole();
	}
}
