namespace Protocol.Common
{
	public enum Cmd
	{
		heartbeat_cs = 0,
		session_key_cs = 1,
		server_maintain_s = 2,
		gateway_busy_s = 3,
		verify_session_c = 4,
		verify_session_result_s = 5
	}
}
