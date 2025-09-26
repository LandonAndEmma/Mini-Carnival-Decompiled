using System.Threading;
using UnityEngine;

public class MultiThreadTest : MonoBehaviour
{
	public class MyThread
	{
		public int count;

		private string thrdName;

		public MyThread(string nam)
		{
			count = 0;
			thrdName = nam;
		}

		public void run()
		{
			Debug.Log("start run a thread");
			do
			{
				Thread.Sleep(1000);
				Debug.Log("in child threadcount=" + count);
				count++;
			}
			while (count < 20);
			Debug.Log("end thread");
		}
	}

	private void Start()
	{
		Debug.Log("start main" + Time.time);
		MyThread myThread = new MyThread("CHILE ");
		Thread thread = new Thread(myThread.run);
		thread.Start();
	}

	private void Update()
	{
		Debug.Log(Time.time);
	}
}
