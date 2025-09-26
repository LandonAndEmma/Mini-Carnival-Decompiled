using System.Collections.Generic;

namespace HuffmanCode
{
	public class TreeList
	{
		private int count;

		private Node first;

		private static string[] signTable;

		private static string[] keyTable;

		private static int pos;

		public TreeList(string input)
		{
			List<char> list = new List<char>();
			for (int i = 0; i < input.Length; i++)
			{
				if (!list.Contains(input[i]))
				{
					list.Add(input[i]);
				}
			}
			signTable = new string[list.Count];
			keyTable = new string[list.Count];
		}

		public string[] GetSignTable()
		{
			return signTable;
		}

		public string[] GetKeyTable()
		{
			return keyTable;
		}

		public void AddLetter(string letter)
		{
			HuffmanTree newData = new HuffmanTree(letter);
			Node node = new Node(newData);
			if (first == null)
			{
				first = node;
			}
			else
			{
				node.link = first;
				first = node;
			}
			count++;
		}

		public void SortTree()
		{
			if (first == null || first.link == null)
			{
				return;
			}
			for (Node link = first; link != null; link = link.link)
			{
				for (Node link2 = link.link; link2 != null; link2 = link2.link)
				{
					if (link.data.Freq > link2.data.Freq)
					{
						HuffmanTree data = link.data;
						link.data = link2.data;
						link2.data = data;
					}
				}
			}
		}

		public HuffmanTree RemoveTree()
		{
			if (first != null)
			{
				HuffmanTree data = first.data;
				first = first.link;
				count--;
				return data;
			}
			return null;
		}

		public void InsertTree(HuffmanTree hTmp)
		{
			Node node = new Node(hTmp);
			if (first == null)
			{
				first = node;
			}
			else
			{
				Node link = first;
				while (link.link != null && (link.data.Freq > hTmp.Freq || link.link.data.Freq < hTmp.Freq))
				{
					link = link.link;
				}
				node.link = link.link;
				link.link = node;
			}
			count++;
		}

		public void MergeTree()
		{
			if (first != null && first.link != null)
			{
				HuffmanTree huffmanTree = RemoveTree();
				HuffmanTree huffmanTree2 = RemoveTree();
				HuffmanTree huffmanTree3 = new HuffmanTree("x");
				huffmanTree3.LChild = huffmanTree;
				huffmanTree3.RChild = huffmanTree2;
				huffmanTree3.Freq = huffmanTree.Freq + huffmanTree2.Freq;
				InsertTree(huffmanTree3);
			}
		}

		public int Lenght()
		{
			return count;
		}

		public void AddSign(string str)
		{
			if (first == null)
			{
				AddLetter(str);
				return;
			}
			for (Node link = first; link != null; link = link.link)
			{
				if (link.data.Letter == str)
				{
					link.data.IncFreq();
					return;
				}
			}
			AddLetter(str);
		}

		public static string Translate(string original)
		{
			string text = string.Empty;
			for (int i = 0; i < original.Length; i++)
			{
				for (int j = 0; j < signTable.Length; j++)
				{
					if (original[i].ToString() == signTable[j])
					{
						text += keyTable[j];
					}
				}
			}
			return text;
		}

		public static void MakeKey(HuffmanTree tree, string code)
		{
			if (tree.LChild == null)
			{
				signTable[pos] = tree.Letter;
				keyTable[pos] = code;
				pos++;
			}
			else
			{
				MakeKey(tree.LChild, code + "0");
				MakeKey(tree.RChild, code + "1");
			}
		}
	}
}
