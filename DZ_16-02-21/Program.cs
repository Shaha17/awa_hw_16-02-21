using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DZ_16_02_21
{
	class Program
	{
		private static List<string> list = new List<string>() { "A", "B", "C", "!", "`", "~", "∂", "å", "©", "ç", "®", "∑", "≈", "µ", "ƒ", "ß", "œ", "®", "†", "¥", "¨", "∆", "ˆ", "ø", "π", "ƒ", "ç", "√", "∫", "˜", "˙", "µ", "≥", "¥", "ƒ", "ß" };
		static int winHeight = Console.WindowHeight;
		static int winWidth = Console.WindowWidth;
		static object locker = new object();
		static void Main(string[] args)
		{
			Console.CursorVisible = false;
			var rnd = new Random();
			Console.Clear();
			var xList = new List<int>();
			int colsCount = 1000;
			//Заполняем список с индексми поцизии по оси X, чтобы в следующем цикле могли запускать стобцы с этими позициями
			for (int i = 0; i < colsCount; i++)
			{
				xList.Add(rnd.Next(1, winWidth));
			}
			//Из полученного списка позиций параллельно запускаем потоки с прорисоквкой столбцов
			Parallel.ForEach(xList, (item) =>
			{
				var rnd = new Random();
				int beginIndex = rnd.Next(0, list.Count / 4);
				int endIndex = beginIndex + rnd.Next(4, list.Count / 4);
				var randomizeList = list.GetRange(beginIndex, endIndex);

				Thread.Sleep(new Random().Next(50, 500));
				// Ради Джасура 🙄, надо раскоментировать это и закоментировать предыдущую строчку
				// int index = xList.FindIndex(x => x == item);
				// Thread.Sleep(index*100);

				Draw(item, -randomizeList.Count, randomizeList);
			});
		}

		static void Draw(int x, int y, List<string> symList)
		{
			Thread.Sleep(100);
			int nextY = y + 1;
			foreach (var item in symList)
			{
				lock (locker)
				{
					if (y >= winHeight - 1) continue;
					if (y < 0)
					{
						y++;
						continue;
					}
					Console.SetCursorPosition(x, y);
					y++;
					var tmpCol = Console.ForegroundColor;
					if (symList.FindIndex(x => x == item) < symList.Count - 1)
					{
						Console.ForegroundColor = ConsoleColor.Green;
					}
					if (symList.FindIndex(x => x == item) < symList.Count - 3)
					{
						Console.ForegroundColor = ConsoleColor.DarkGreen;
					}
					if (symList.FindIndex(x => x == item) < symList.Count - 6)
					{
						Console.ForegroundColor = ConsoleColor.DarkGray;
					}
					Console.Write(item);
					Console.ForegroundColor = tmpCol;
				}

			}
			lock (locker)
			{
				var tmp = symList[0];
				symList.RemoveAt(0);
				symList.Add(tmp);
				if (nextY > 1)
				{
					Console.SetCursorPosition(x, nextY - 2);
					Console.Write(" ");
				}
			}

			if (nextY < winHeight)
			{
				Draw(x, nextY, symList);
			}
		}
	}
}
