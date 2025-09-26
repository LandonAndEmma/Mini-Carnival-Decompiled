public class RemoteFileData
{
	public string _md5;

	public byte[] _data;

	public RemoteFileData(string m, byte[] data)
	{
		_md5 = m;
		_data = data;
	}
}
