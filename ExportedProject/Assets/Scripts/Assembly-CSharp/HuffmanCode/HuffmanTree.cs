namespace HuffmanCode
{
	public class HuffmanTree
	{
		private HuffmanTree lChild;

		private HuffmanTree rChild;

		private string letter;

		private int freq;

		public HuffmanTree LChild
		{
			get
			{
				return lChild;
			}
			set
			{
				lChild = value;
			}
		}

		public HuffmanTree RChild
		{
			get
			{
				return rChild;
			}
			set
			{
				rChild = value;
			}
		}

		public string Letter
		{
			get
			{
				return letter;
			}
			set
			{
				letter = value;
			}
		}

		public int Freq
		{
			get
			{
				return freq;
			}
			set
			{
				freq = value;
			}
		}

		public HuffmanTree(string letter)
		{
			this.letter = letter;
			freq = 1;
		}

		public void IncFreq()
		{
			freq++;
		}
	}
}
