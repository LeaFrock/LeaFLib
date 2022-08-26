using ClosedXML.Excel;

namespace LeaFLib.Demos.Console.Samples
{
    internal partial class RandomizeListSample : ISample
    {
        // https://bost.ocks.org/mike/algorithms/#shuffling
        public Task RunAsync()
        {
            var table1 = Shuffle();
            var table2 = Shuffle_OrderBy();
            var table3 = Shuffle_Sort();

            ExportFile(new KeyValuePair<string, int[,]>[]
            {
                new(nameof(Shuffle), table1),
                new(nameof(Shuffle_OrderBy), table2),
                new(nameof(Shuffle_Sort), table3),
            });

            return Task.CompletedTask;
        }

        private static void ExportFile(KeyValuePair<string, int[,]>[] pairs)
        {
            using var book = new XLWorkbook();
            var sheet = book.AddWorksheet();

            for (int k = 0; k < pairs.Length; k++)
            {
                var table = pairs[k].Value;
                int rank = table.GetLength(0);
                int colOffset = k * (rank + 1);
                int rs = 2, cs = 1 + colOffset; // Row|Col Start
                int re = 1 + rank, ce = colOffset + rank; // Row|Col End

                sheet.Cell(1, cs).SetValue(pairs[k].Key);
                sheet.Range(1, cs, 1, ce).Merge();

                for (int i = 0; i < rank; i++)
                {
                    for (int j = 0; j < rank; j++)
                    {
                        sheet.Cell(i + 2, j + 1 + colOffset).SetValue(table[i, j]);
                    }
                }
                sheet.Cell(1, ce + 1).SetFormulaR1C1($"STDEVPA(R{rs}C{cs}:R{re}C{ce})");
                sheet.Range(rs, cs, re, ce)
                    .AddConditionalFormat()
                    .ColorScale()
                    .LowestValue(XLColor.White)
                    .HighestValue(XLColor.Red);
            }

            sheet.Columns().AdjustToContents();

            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"random_{DateTimeOffset.Now.ToUnixTimeSeconds():x}.xlsx");
            book.SaveAs(file);
        }
    }

    internal sealed partial class RandomizeListSample
    {
        private readonly Random Random = new();
        private readonly int[] Template;

        public RandomizeListSample(int rank, int times)
        {
            Rank = rank;
            Times = times;
            Template = Enumerable.Range(0, Rank).ToArray();
        }

        public int Rank { get; }

        public int Times { get; }

        public int[,] Shuffle()
        {
            int[,] table = new int[Rank, Rank];
            for (int i = 0; i < Times; i++)
            {
                var source = new List<int>(Template);
                ShuffleCore(source);
                Fill(table, source);
            }
            return table;
        }

        public int[,] Shuffle_OrderBy()
        {
            int[,] table = new int[Rank, Rank];
            for (int i = 0; i < Times; i++)
            {
                var source = new List<int>(Template).OrderBy(_ => Random.Next()).ToList();
                Fill(table, source);
            }
            return table;
        }

        public int[,] Shuffle_Sort()
        {
            int[,] table = new int[Rank, Rank];
            for (int i = 0; i < Times; i++)
            {
                var source = new List<int>(Template);
                source.Sort((_, _) => Random.Next(0, 2) > 0 ? 1 : -1);
                Fill(table, source);
            }
            return table;
        }

        private static void Fill(int[,] table, List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                table[list[i], i] += 1;
            }
        }

        internal static void ShuffleCore<T>(List<T> source)
        {
            int total = source.Count;
            int randIndex, tailIndex = total - 1;
            var rand = Random.Shared;
            for (int i = 0; i < total - 1; i++)
            {
                randIndex = rand.Next(0, tailIndex + 1);
                if (randIndex != tailIndex)
                {
                    (source[randIndex], source[tailIndex]) = (source[tailIndex], source[randIndex]);
                }
                tailIndex--;
            }
        }
    }
}