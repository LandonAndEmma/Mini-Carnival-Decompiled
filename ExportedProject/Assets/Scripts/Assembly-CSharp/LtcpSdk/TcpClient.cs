using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace LtcpSdk
{
	public class TcpClient
	{
		protected enum STATUS
		{
			kReady = 0,
			kConnecting = 1,
			kConnectError = 2,
			kConnected = 3,
			kClosed = 4
		}

		private class Event
		{
			public enum TYPE
			{
				kUnknown = 0,
				kConnected = 1,
				kPacked = 2,
				kConnectError = 3,
				kUserClose = 4,
				kSystemError = 5,
				kSystemClose = 6
			}

			public TYPE m_type;

			public Packet m_packet;
		}

		protected const int DefaultBufferSize = 32768;

		protected STATUS m_Status;

		protected Socket m_socket;

		protected byte[] m_RecvDataBuffer;

		protected int m_iRcvLength;

		protected int m_iCurPacketLength;

		protected List<byte> m_CurPacketBuffer;

		private CircleBuffer<Event> m_EventQueue;

		public TcpClient()
		{
			m_Status = STATUS.kReady;
			m_socket = null;
			m_RecvDataBuffer = new byte[32768];
			m_iRcvLength = 0;
			m_iCurPacketLength = 0;
			m_CurPacketBuffer = new List<byte>();
			m_EventQueue = new CircleBuffer<Event>(1024);
		}

		public static string GetHostAddresses(string hostNameOrAddress)
		{
			try
			{
				IPAddress[] hostAddresses = Dns.GetHostAddresses(hostNameOrAddress);
				return hostAddresses[0].ToString();
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}

		public void Connect(string ip, int port)
		{
			if (m_Status == STATUS.kReady)
			{
				m_Status = STATUS.kConnecting;
				m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint end_point = new IPEndPoint(IPAddress.Parse(ip), port);
				m_socket.BeginConnect(end_point, Connected, m_socket);
			}
		}

		public void SendPacket(Packet packet)
		{
			if (packet == null)
			{
				return;
			}
			if (m_Status != STATUS.kConnected)
			{
				Debug.Log("Send Error, STATUS.kConnected:" + m_Status);
				return;
			}
			try
			{
				m_socket.BeginSend(packet.ByteArray(), 0, packet.Length, SocketFlags.None, SendDataEnd, m_socket);
			}
			catch (Exception message)
			{
				Debug.Log(message);
			}
		}

		public void Close()
		{
			if (m_Status == STATUS.kReady)
			{
				return;
			}
			if (m_Status == STATUS.kConnected)
			{
				try
				{
					if (m_socket.Connected)
					{
						m_socket.Shutdown(SocketShutdown.Both);
					}
					m_socket.BeginDisconnect(true, UserDisconnectCallback, m_socket);
				}
				catch (Exception message)
				{
					Debug.Log(message);
				}
				m_Status = STATUS.kClosed;
			}
			else if (m_Status == STATUS.kConnecting || m_Status == STATUS.kConnectError)
			{
				try
				{
					m_socket.BeginDisconnect(true, UserDisconnectCallback, m_socket);
				}
				catch (Exception message2)
				{
					Debug.Log(message2);
				}
				m_Status = STATUS.kClosed;
			}
			else if (m_Status == STATUS.kClosed)
			{
				m_socket = null;
				m_Status = STATUS.kReady;
			}
		}

		public void Update()
		{
			Event data = new Event();
			while (m_EventQueue.read(ref data))
			{
				switch (data.m_type)
				{
				case Event.TYPE.kConnected:
					OnConnected();
					break;
				case Event.TYPE.kPacked:
					OnPacket(data.m_packet);
					break;
				case Event.TYPE.kConnectError:
					OnConnectError();
					break;
				case Event.TYPE.kUserClose:
					OnKilled();
					break;
				case Event.TYPE.kSystemError:
					SystemClose();
					break;
				case Event.TYPE.kSystemClose:
					OnClosed();
					break;
				}
			}
		}

		private void UserDisconnectCallback(IAsyncResult ar)
		{
			Socket socket = (Socket)ar.AsyncState;
			try
			{
				socket.EndDisconnect(ar);
			}
			catch (Exception)
			{
			}
			finally
			{
				Event obj = new Event();
				obj.m_type = Event.TYPE.kUserClose;
				m_EventQueue.write(obj);
			}
			m_socket = null;
			m_Status = STATUS.kReady;
		}

		private void SystemClose()
		{
			if (m_Status != STATUS.kConnected)
			{
				return;
			}
			try
			{
				if (m_socket.Connected)
				{
				}
				m_socket.BeginDisconnect(true, SystemDisconnectCallback, m_socket);
			}
			catch (Exception message)
			{
				Debug.Log(message);
			}
			m_Status = STATUS.kClosed;
		}

		private void SystemDisconnectCallback(IAsyncResult ar)
		{
			Socket socket = (Socket)ar.AsyncState;
			try
			{
				socket.EndDisconnect(ar);
			}
			catch (Exception)
			{
			}
			finally
			{
				Event obj = new Event();
				obj.m_type = Event.TYPE.kSystemClose;
				m_EventQueue.write(obj);
			}
			m_socket = null;
			m_Status = STATUS.kReady;
		}

		private void SendDataEnd(IAsyncResult iar)
		{
			if (m_Status == STATUS.kClosed)
			{
				return;
			}
			Socket socket = (Socket)iar.AsyncState;
			try
			{
				int num = socket.EndSend(iar);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		private void Connected(IAsyncResult iar)
		{
			if (m_Status != STATUS.kConnecting)
			{
				return;
			}
			Socket socket = (Socket)iar.AsyncState;
			try
			{
				socket.EndConnect(iar);
				socket.BeginReceive(m_RecvDataBuffer, m_iRcvLength, 32768 - m_iRcvLength, SocketFlags.None, RecvData, socket);
				m_Status = STATUS.kConnected;
				Event obj = new Event();
				obj.m_type = Event.TYPE.kConnected;
				m_EventQueue.write(obj);
			}
			catch (Exception)
			{
				m_Status = STATUS.kConnectError;
				Event obj2 = new Event();
				obj2.m_type = Event.TYPE.kConnectError;
				m_EventQueue.write(obj2);
			}
		}

		private void RecvData(IAsyncResult iar)
		{
			if (m_Status != STATUS.kConnected || !m_socket.Connected)
			{
				return;
			}
			try
			{
				int num = m_socket.EndReceive(iar);
				if (num > 0)
				{
					m_iRcvLength += num;
					bool flag = false;
					while (true)
					{
						if (m_iCurPacketLength == 0)
						{
							int num2 = OnCheckPacket(ref m_RecvDataBuffer, m_iRcvLength);
							if (num2 > 0)
							{
								m_iCurPacketLength = num2;
							}
							else
							{
								if (num2 == 0)
								{
									break;
								}
								if (num2 == -1)
								{
									flag = true;
									break;
								}
							}
						}
						int num3 = m_iCurPacketLength - m_CurPacketBuffer.Count;
						if (num3 > 0)
						{
							if (m_iRcvLength > num3)
							{
								for (int i = 0; i < num3; i++)
								{
									m_CurPacketBuffer.Add(m_RecvDataBuffer[i]);
								}
								m_iRcvLength -= num3;
								for (int j = 0; j < m_iRcvLength; j++)
								{
									m_RecvDataBuffer[j] = m_RecvDataBuffer[num3 + j];
								}
							}
							else
							{
								for (int k = 0; k < m_iRcvLength; k++)
								{
									m_CurPacketBuffer.Add(m_RecvDataBuffer[k]);
								}
								m_iRcvLength = 0;
							}
							num3 = m_iCurPacketLength - m_CurPacketBuffer.Count;
							if (num3 > 0)
							{
								break;
							}
						}
						if (num3 == 0)
						{
							Event obj = new Event();
							obj.m_type = Event.TYPE.kPacked;
							obj.m_packet = new Packet(m_CurPacketBuffer.ToArray(), false);
							m_EventQueue.write(obj);
							m_CurPacketBuffer.Clear();
							m_iCurPacketLength = 0;
						}
					}
					if (flag)
					{
						Event obj2 = new Event();
						obj2.m_type = Event.TYPE.kSystemError;
						m_EventQueue.write(obj2);
						Debug.LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!packet error!!!!!!!!!!!!!!!");
					}
					else
					{
						m_socket.BeginReceive(m_RecvDataBuffer, m_iRcvLength, 32768 - m_iRcvLength, SocketFlags.None, RecvData, null);
					}
				}
				else
				{
					Event obj3 = new Event();
					obj3.m_type = Event.TYPE.kSystemError;
					m_EventQueue.write(obj3);
				}
			}
			catch (Exception)
			{
				Event obj4 = new Event();
				obj4.m_type = Event.TYPE.kSystemError;
				m_EventQueue.write(obj4);
			}
		}

		protected virtual void OnConnected()
		{
		}

		protected virtual void OnClosed()
		{
		}

		protected virtual void OnKilled()
		{
		}

		protected virtual void OnConnectError()
		{
		}

		protected virtual void OnPacket(Packet packet)
		{
		}

		protected virtual int OnCheckPacket(ref byte[] data, int len)
		{
			return -1;
		}
	}
}
