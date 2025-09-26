public class COMA_Fishing_PlayerStateCreator_ShowItem : TStateFactory<COMA_Fishing_PlayerController>
{
	public override TState<COMA_Fishing_PlayerController> CreateState()
	{
		return new COMA_Fishing_PlayerState_ShowItem();
	}
}
