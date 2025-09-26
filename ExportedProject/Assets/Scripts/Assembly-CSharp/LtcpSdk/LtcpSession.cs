using System;
using Protocol;
using Protocol.Binary;
using Protocol.Common;
using Protocol.Common.S2C;
using UnityEngine;

namespace LtcpSdk
{
	public class LtcpSession : TcpClient
	{
		public delegate void OnSessionEventCallback();

		public delegate void OnSessionDataCallback(Protocol.Header header, UnPacker unpacker);

		public OnSessionEventCallback m_callback_connected;

		public OnSessionEventCallback m_callback_connecterror;

		public OnSessionEventCallback m_callback_connecttimeout;

		public OnSessionEventCallback m_callback_closed;

		public OnSessionDataCallback m_callback_data;

		private byte[] m_xxtea_key;

		private BlowFish m_blow_fish;

		private bool m_need_blowfish = true;

		private float m_timeout;

		private float m_waittime;

		public LtcpSession(float timeout)
		{
			m_timeout = timeout;
			m_waittime = 0f;
			string key = KeyHolder.GetKey();
			if (key != null && key.Length > 0)
			{
				m_need_blowfish = true;
				m_blow_fish = new BlowFish(key);
			}
			else
			{
				m_need_blowfish = false;
			}
		}

		public void Clear()
		{
			m_callback_connected = null;
			m_callback_closed = null;
			m_callback_connecterror = null;
			m_callback_connecttimeout = null;
			m_waittime = 0f;
		}

		public void Update(float deltaTime)
		{
			Update();
			if (m_Status == STATUS.kConnecting && m_timeout > 0f)
			{
				m_waittime += deltaTime;
				if (m_waittime >= m_timeout && m_callback_connecttimeout != null)
				{
					Close();
					m_callback_connecttimeout();
				}
			}
		}

		public void SendProtoCmd(BaseCmd cmd)
		{
			Packer packer = new Packer();
			cmd.Serialize(packer);
			Packet packet = packer.MakePacket(1, 1, m_xxtea_key);
			if (m_need_blowfish)
			{
				ulong val = 0uL;
				packet.Position = 0;
				packet.PopUInt64(ref val);
				m_blow_fish.Encrypt(ref val);
				packet.Position = 0;
				packet.PushUInt64(val);
			}
			SendPacket(packet);
		}

		protected override void OnConnected()
		{
			Debug.Log("LtcpSession::OnConnected, wait server sessionkey");
		}

		protected override void OnClosed()
		{
			Debug.Log("LtcpSession::OnClosed");
			if (m_callback_closed != null)
			{
				m_callback_closed();
			}
		}

		protected override void OnConnectError()
		{
			Debug.Log("LtcpSession::OnConnectError");
			if (m_callback_connecterror != null)
			{
				m_callback_connecterror();
			}
		}

		protected override void OnPacket(Packet packet)
		{
			UnPacker unPacker = new UnPacker();
			if (!unPacker.ParserPacket(packet, m_xxtea_key))
			{
				return;
			}
			Protocol.Header header = new Protocol.Header();
			if (!header.Parse(unPacker))
			{
				Debug.DebugBreak();
				return;
			}
			if (header.m_cProtocol == 0)
			{
				Cmd cCmd = (Cmd)header.m_cCmd;
				if (cCmd != Cmd.heartbeat_cs && cCmd == Cmd.session_key_cs)
				{
					Debug.Log("recv session key");
					if (m_xxtea_key == null)
					{
						SessionKeyCmd sessionKeyCmd = new SessionKeyCmd();
						if (!sessionKeyCmd.Parser(unPacker))
						{
							Debug.DebugBreak();
						}
						byte b = sessionKeyCmd.m_key[89];
						byte sourceIndex = sessionKeyCmd.m_key[88];
						m_xxtea_key = new byte[b];
						m_xxtea_key.Initialize();
						Array.Copy(sessionKeyCmd.m_key, sourceIndex, m_xxtea_key, 0, m_xxtea_key.Length);
						if (m_callback_connected != null)
						{
							m_callback_connected();
						}
					}
					else
					{
						Debug.DebugBreak();
					}
					return;
				}
			}
			if (m_callback_data != null)
			{
				m_callback_data(header, unPacker);
			}
		}

		protected override int OnCheckPacket(ref byte[] data, int len)
		{
			if (len < 8)
			{
				return 0;
			}
			if (m_need_blowfish)
			{
				uint num = (uint)((data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3]);
				uint num2 = (uint)((data[4] << 24) | (data[5] << 16) | (data[6] << 8) | data[7]);
				ulong num3 = num;
				num3 = (num3 << 32) + num2;
				m_blow_fish.Decrypt(ref num3);
				num = (uint)(num3 >> 32);
				num2 = (uint)num3;
				data[0] = (byte)(((num & 0xFF000000u) >> 24) & 0xFF);
				data[1] = (byte)(((num & 0xFF0000) >> 16) & 0xFF);
				data[2] = (byte)(((num & 0xFF00) >> 8) & 0xFF);
				data[3] = (byte)(num & 0xFF & 0xFF);
				data[4] = (byte)(((num2 & 0xFF000000u) >> 24) & 0xFF);
				data[5] = (byte)(((num2 & 0xFF0000) >> 16) & 0xFF);
				data[6] = (byte)(((num2 & 0xFF00) >> 8) & 0xFF);
				data[7] = (byte)(num2 & 0xFF & 0xFF);
			}
			uint num4 = BufferUtils.WatchUInt32(data, 0);
			if (num4 < 6)
			{
				return -1;
			}
			return (int)num4;
		}
	}
}
