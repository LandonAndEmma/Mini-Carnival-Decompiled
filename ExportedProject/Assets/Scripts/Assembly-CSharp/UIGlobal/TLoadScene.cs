namespace UIGlobal
{
	public class TLoadScene
	{
		public string _sceneName;

		public ELoadLevelParam _param;

		public TLoadScene(string name, ELoadLevelParam param)
		{
			_sceneName = name;
			_param = param;
		}
	}
}
