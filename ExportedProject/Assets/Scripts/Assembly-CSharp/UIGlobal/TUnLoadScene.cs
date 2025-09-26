namespace UIGlobal
{
	public class TUnLoadScene
	{
		public string _sceneName;

		public EUnloadLevelParam _param;

		public TUnLoadScene(string name, EUnloadLevelParam param)
		{
			_sceneName = name;
			_param = param;
		}
	}
}
