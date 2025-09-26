using System.Collections.Generic;
using Protocol;
using Protocol.Binary;

public class UILobbyMessageHandler : UIEntity
{
	public delegate bool LobbyProcessMessage(UnPacker unpacker);

	protected Dictionary<byte, LobbyProcessMessage> _dictMsgProcessor = new Dictionary<byte, LobbyProcessMessage>();

	protected void OnMessage(byte cmd, LobbyProcessMessage proc)
	{
		if (_dictMsgProcessor.ContainsKey(cmd))
		{
			_dictMsgProcessor[cmd] = proc;
		}
		else
		{
			_dictMsgProcessor.Add(cmd, proc);
		}
	}

	public virtual bool HandleLobbyMessage(Protocol.Header header, UnPacker unpacker)
	{
		byte cCmd = header.m_cCmd;
		if (_dictMsgProcessor.ContainsKey(cCmd))
		{
			LobbyProcessMessage lobbyProcessMessage = _dictMsgProcessor[cCmd];
			return lobbyProcessMessage(unpacker);
		}
		return false;
	}
}
