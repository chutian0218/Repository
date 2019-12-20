using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Keywordretrieval
{

	class Program

	{
		static void Main()
		{
			var keyString = "人工智能";
			TestReadingFile(keyString);
			Console.WriteLine("---");
			TestStreamReaderEnumerable(keyString);

			Console.ReadKey();
		}


		public static void TestReadingFile(string keyString)
		{
			var memoryBefore = GC.GetTotalMemory(true);
			StreamReader sr;
			try
			{
				sr = File.OpenText("Document/CT/JC/JC.text");
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine(@"这个例子需要一个名为 C:\temp\tempFile.txt 的文件。");
				return;
			}
			var fileContents = new List<string>();
			while (!sr.EndOfStream)
			{
				fileContents.Add(sr.ReadLine());
			}


			var stringsFound =
				from line in fileContents
				where line.Contains(keyString)
				select line;

			sr.Close();
			Console.WriteLine("数量：" + stringsFound.Count());

			var memoryAfter = GC.GetTotalMemory(false);
			Console.WriteLine("不使用 Iterator 的内存用量 = \t" + string.Format(((memoryAfter - memoryBefore) / 1000).ToString(), "n") + "kb");
		}


		public static void TestStreamReaderEnumerable(string keyString)
		{
			var memoryBefore = GC.GetTotalMemory(true);
			IEnumerable<String> stringsFound;

			try
			{
				stringsFound =
					  from line in new StreamReaderEnumerable(@"Document/CT/JC/JC.text")
					  where line.Contains(keyString)
					  select line;
				Console.WriteLine("数量：" + stringsFound.Count());
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine(@"这个例子需要一个名为 C:\temp\tempFile.txt 的文件。");
				return;
			}

			var memoryAfter = GC.GetTotalMemory(false);
			Console.WriteLine("使用 Iterator 的内存用量 = \t" + string.Format(((memoryAfter - memoryBefore) / 1000).ToString(), "n") + "kb");
		}


	}


	public class StreamReaderEnumerable : IEnumerable<string>
	{
		private string _filePath;
		public StreamReaderEnumerable(string filePath)
		{
			_filePath = filePath;
		}


		public IEnumerator<string> GetEnumerator() => new StreamReaderEnumerator(_filePath);


		private IEnumerator GetEnumerator1() => this.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator1();


		public class StreamReaderEnumerator : IEnumerator<string>
		{
			private StreamReader _sr;
			public StreamReaderEnumerator(string filePath)
			{
				_sr = new StreamReader(filePath);
			}
			private string _current;

			public string Current
			{
				get
				{
					if (_sr == null || _current == null)
						throw new InvalidOperationException();
					return _current;
				}
			}
			private object Current1 => this.Current;
			object IEnumerator.Current => Current1;

			public bool MoveNext()
			{
				_current = _sr.ReadLine();
				if (_current == null)
					return false;
				return true;
			}
			public void Reset()
			{
				_sr.DiscardBufferedData();
				_sr.BaseStream.Seek(0, SeekOrigin.Begin);
				_current = null;
			}

			private bool disposedValue = false;
			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}
			protected virtual void Dispose(bool disposing)
			{
				if (!this.disposedValue)
				{
					if (disposing) { }
					_current = null;
					if (_sr != null)
					{
						_sr.Close();
						_sr.Dispose();
					}
				}
				this.disposedValue = true;
			}
			~StreamReaderEnumerator() { Dispose(false); }
		}
	}
}
