using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace deneme4
{
    public partial class Form1 : Form
    {
        private string filePath;
        private string filePath2;
        private HashSet<string> stopwords = new HashSet<string>
        {
            "i", "me", "my", "myself", "we", "our", "ours", "ourselves", "you", "your", "yours", "yourself",
            "yourselves", "he", "him", "his", "himself", "she", "her", "hers", "herself", "it", "its", "itself",
            "they", "them", "their", "theirs", "themselves", "what", "which", "who", "whom", "this", "that", "these",
            "those", "am", "is", "are", "was", "were", "be", "been", "being", "have", "has", "had", "having", "do",
            "does", "did", "doing", "a", "an", "the", "and", "but", "if", "or", "because", "as", "until", "while",
            "of", "at", "by", "for", "with", "about", "against", "between", "into", "through", "during", "before",
            "after", "above", "below", "to", "from", "up", "down", "in", "out", "on", "off", "over", "under", "again",
            "further", "then", "once", "here", "there", "when", "where", "why", "how", "all", "any", "both", "each",
            "few", "more", "most", "other", "some", "such", "no", "nor", "not", "only", "own", "same", "so", "than",
            "too", "very", "s", "t", "can", "will", "just", "don", "should", "now"
        };

        public Form1()
        {
            InitializeComponent();
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "messi_tweets.csv");
            filePath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ronaldo_tweets.csv");

        }
        

        //toplam satır sayısı
        private int CountRows(string filePath)
        {
            return File.ReadLines(filePath).Count();
        }

        //toplam sütun sayısı
        private int CountColumns(string filePath)
        {
            return File.ReadLines(filePath).First().Split(',').Length;
        }

        // boş satır sayısı
        private int CountEmptyRows(string filePath)
        {
            return File.ReadLines(filePath).Count(line => string.IsNullOrWhiteSpace(line));
        }

        // boş sütun sayısı
        private int CountEmptyColumns(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            int emptyColumnCount = 0;
            if (lines.Length > 0)
            {
                int columnCount = lines[0].Split(',').Length;
                for (int i = 0; i < columnCount; i++)
                {
                    if (lines.All(line => string.IsNullOrWhiteSpace(line.Split(',')[i])))
                    {
                        emptyColumnCount++;
                    }
                }
            }
            return emptyColumnCount;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            label16.Text = "Messi Twits File";
            label17.Text = "Ronaldo Twits File";
            // İlk dosya için işlemler
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Thread rowCountThread = new Thread(() =>
            {
                int rowCount = CountRows(filePath);
                label1.Invoke((MethodInvoker)(() => label1.Text += $"Row Count: {rowCount}\n"));
            });
            rowCountThread.Start();

            Thread columnCountThread = new Thread(() =>
            {
                int columnCount = CountColumns(filePath);
                label1.Invoke((MethodInvoker)(() => label1.Text += $"Column Count: {columnCount}\n"));
            });
            columnCountThread.Start();

            Thread emptyRowCountThread = new Thread(() =>
            {
                int emptyRowCount = CountEmptyRows(filePath);
                label1.Invoke((MethodInvoker)(() => label1.Text += $"Empty Row Count: {emptyRowCount}\n"));
            });
            emptyRowCountThread.Start();

            Thread emptyColumnCountThread = new Thread(() =>
            {
                int emptyColumnCount = CountEmptyColumns(filePath);
                label1.Invoke((MethodInvoker)(() => label1.Text += $"Empty Column Count: {emptyColumnCount}\n"));
            });
            emptyColumnCountThread.Start();

            // İkinci dosya için işlemler
            Thread rowCountThread2 = new Thread(() =>
            {
                int rowCount2 = CountRows(filePath2);
                label3.Invoke((MethodInvoker)(() => label3.Text += $"Row Count: {rowCount2}\n"));
            });
            rowCountThread2.Start();

            Thread columnCountThread2 = new Thread(() =>
            {
                int columnCount2 = CountColumns(filePath2);
                label3.Invoke((MethodInvoker)(() => label3.Text += $"Column Count: {columnCount2}\n"));
            });
            columnCountThread2.Start();

            Thread emptyRowCountThread2 = new Thread(() =>
            {
                int emptyRowCount2 = CountEmptyRows(filePath2);
                label3.Invoke((MethodInvoker)(() => label3.Text += $"Empty Row Count: {emptyRowCount2}\n"));
            });
            emptyRowCountThread2.Start();

            Thread emptyColumnCountThread2 = new Thread(() =>
            {
                int emptyColumnCount2 = CountEmptyColumns(filePath2);
                label3.Invoke((MethodInvoker)(() => label3.Text += $"Empty Column Count: {emptyColumnCount2}\n"));
            });
            emptyColumnCountThread2.Start();

            stopwatch.Stop();

            label2.Invoke((MethodInvoker)(() => label2.Text = $"Elapsed Time: {stopwatch.Elapsed.TotalSeconds} seconds"));
        }




        private string[] GetFirstFiveRows(string filePath)
        {
            List<string> firstFiveRows = new List<string>();

            // Dosyadan ilk beş satırı oku
            using (StreamReader reader = new StreamReader(filePath))
            {
                for (int i = 0; i < 10; i++)
                {
                    string line = reader.ReadLine();
                    if (line != null)
                    {
                        firstFiveRows.Add(line);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return firstFiveRows.ToArray();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // İlk beş satırı al (ilk dosya)
                string[] firstFiveRowsFile1 = GetFirstFiveRows(filePath);

                // İlk beş satırı al (ikinci dosya)
                string[] firstFiveRowsFile2 = GetFirstFiveRows(filePath2);

                stopwatch.Stop();

                // İlk beş satırı birleştir (her dosya için)
                string firstFiveRowsTextFile1 = string.Join(Environment.NewLine, firstFiveRowsFile1);
                string firstFiveRowsTextFile2 = string.Join(Environment.NewLine, firstFiveRowsFile2);

                MessageBox.Show($"First Five Rows (File 1):\n{firstFiveRowsTextFile1}\n\nFirst Five Rows (File 2):\n{firstFiveRowsTextFile2}", "First Five Rows");

                label4.Invoke((MethodInvoker)(() => label4.Text = $"Elapsed Time: {stopwatch.Elapsed.TotalSeconds} seconds"));
            });

            thread.Start();
        }




        private void button2_Click(object sender, EventArgs e)
        {
            label13.Text="Messi Twits File";
            label15.Text = "Ronaldo Twits File";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Satır sayısı
            int rowCount = CountRows(filePath);
            int rowCount2 = CountRows(filePath2);

            // Sütun sayısı
            int columnCount = CountColumns(filePath);
            int columnCount2 = CountColumns(filePath2);

            // Boş satır sayısı
            int emptyRowCount = CountEmptyRows(filePath);
            int emptyRowCount2 = CountEmptyRows(filePath2);

            // Boş sütun sayısı
            int emptyColumnCount = CountEmptyColumns(filePath);
            int emptyColumnCount2 = CountEmptyColumns(filePath2);

            stopwatch.Stop();
            label6.Text = $"Elapsed Time: {stopwatch.Elapsed.TotalSeconds} seconds";

            label5.Text = $"Row Count: {rowCount}\nColumn Count: {columnCount}\nEmpty Row Count: {emptyRowCount}\nEmpty Column Count: {emptyColumnCount}";

            label9.Text = $"Row Count: {rowCount2}\nColumn Count: {columnCount2}\nEmpty Row Count: {emptyRowCount2}\nEmpty Column Count: {emptyColumnCount2}";
        }





        private string[] GetLastFiveRows(string filePath)
        {
            List<string> lastFiveRows = new List<string>();

            // Dosyadan son 5 satırı oku
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Dosyanın sonuna git
                reader.BaseStream.Seek(0, SeekOrigin.End);

                // İndex'i başlat
                int index = 0;
                // Satırı geri al
                string line;
                while (index < 5 && reader.BaseStream.Position > 0)
                {
                    reader.BaseStream.Seek(-2, SeekOrigin.Current);
                    char currentChar = (char)reader.Read();
                    if (currentChar == '\n')
                    {
                        index++;
                    }
                    reader.BaseStream.Seek(-2, SeekOrigin.Current);
                }

                // Son 5 satırı al
                while ((line = reader.ReadLine()) != null)
                {
                    lastFiveRows.Add(line);
                }
            }

            // Listeyi ters çevir
            lastFiveRows.Reverse();

            return lastFiveRows.ToArray();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // İlk dosyanın son beş satırını al
                string[] lastFiveRowsFile1 = GetLastFiveRows(filePath);

                // İkinci dosyanın son beş satırını al
                string[] lastFiveRowsFile2 = GetLastFiveRows(filePath2);

                stopwatch.Stop();

                // İlk dosyanın son beş satırını birleştir
                string lastFiveRowsTextFile1 = string.Join(Environment.NewLine, lastFiveRowsFile1);

                // İkinci dosyanın son beş satırını birleştir
                string lastFiveRowsTextFile2 = string.Join(Environment.NewLine, lastFiveRowsFile2);

                MessageBox.Show($"Last Five Rows (File 1):\n{lastFiveRowsTextFile1}\n\nLast Five Rows (File 2):\n{lastFiveRowsTextFile2}", "Last Five Rows");

                label7.Invoke((MethodInvoker)(() => label7.Text = $"Elapsed Time: {stopwatch.Elapsed.TotalSeconds} seconds"));
            });

            thread.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                long fileSizeInBytes1 = 0, fileSizeInBytes2 = 0;
                long fileSizeInKB1 = 0, fileSizeInKB2 = 0;
                long fileSizeInMB1 = 0, fileSizeInMB2 = 0;

                // iki dosyanın boyutunu paralel olarak hesapla
                Parallel.Invoke(
                    () =>
                    {
                        FileInfo fileInfo1 = new FileInfo(filePath);
                        fileSizeInBytes1 = fileInfo1.Length;
                        fileSizeInKB1 = fileSizeInBytes1 / 1024;
                        fileSizeInMB1 = fileSizeInKB1 / 1024;
                    },
                    () =>
                    {
                        FileInfo fileInfo2 = new FileInfo(filePath2);
                        fileSizeInBytes2 = fileInfo2.Length;
                        fileSizeInKB2 = fileSizeInBytes2 / 1024;
                        fileSizeInMB2 = fileSizeInKB2 / 1024;
                    });

                stopwatch.Stop();

                label10.Invoke((MethodInvoker)delegate {
                    label10.Text = $"File 1 Size: {fileSizeInBytes1} bytes ({fileSizeInKB1} KB, {fileSizeInMB1} MB)\n" +
                                   $"File 2 Size: {fileSizeInBytes2} bytes ({fileSizeInKB2} KB, {fileSizeInMB2} MB)";
                });

                label8.Invoke((MethodInvoker)(() => label8.Text = $"Elapsed Time: {stopwatch.Elapsed.TotalSeconds} seconds"));
            });

            thread.Start();
        }




        private void button7_Click(object sender, EventArgs e)
        {

            // İlk dosya yolunda işlem yapmak için bir thread 
            Thread thread1 = new Thread(() =>
            {
               
                ProcessFile(filePath);
                

            });

            // İkinci dosya yolunda işlem yapmak için bir thread 
            Thread thread2 = new Thread(() =>
            {
                ProcessFile(filePath2);
            });

            thread1.Start();
            thread2.Start();
        }

        private void ProcessFile(string filePath)
        {
            

            string[] firstFiveRowsWithStopwords = GetFirst100RowsWithStopwords(filePath, stopwords);

            MessageBox.Show(string.Join(Environment.NewLine, firstFiveRowsWithStopwords), "First Five Rows with Stopwords");
            
        }

        private string[] GetFirst100RowsWithStopwords(string filePath, HashSet<string> stopwords)
        {
            List<string> first100RowsWithStopwords = new List<string>();

            
            using (StreamReader reader = new StreamReader(filePath))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                int rowCount = 0;
                while (rowCount < 10 && !reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (ContainsStopwords(line, stopwords))
                    {
                        first100RowsWithStopwords.Add(line);
                        rowCount++;
                    }
                }
                stopwatch.Stop();
                label18.Invoke((MethodInvoker)(() => label18.Text = $"Elapsed Time: {stopwatch.Elapsed.TotalSeconds} seconds"));
            }

            return first100RowsWithStopwords.ToArray();
        }

        private bool ContainsStopwords(string line, HashSet<string> stopwords)
        {
            foreach (string word in line.Split(','))
            {
                if (stopwords.Contains(word.Trim()))
                {
                    return true;
                }
            }
            return false;
        }


        private void button8_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                List<string> first100RowsWithLink1 = new List<string>();
                List<string> first100RowsWithLink2 = new List<string>();

                // Paralel olarak işlemleri yürüt
                Parallel.Invoke(
                    () =>
                    {
                        first100RowsWithLink1 = GetFirst100RowsWithLink(filePath);
                    },
                    () =>
                    {
                        first100RowsWithLink2 = GetFirst100RowsWithLink(filePath2);
                    });

                stopwatch.Stop();

                MessageBox.Show($"First 100 Rows with Link in File 1:{Environment.NewLine}{string.Join(Environment.NewLine, first100RowsWithLink1)}" +
                                $"\n\nFirst 100 Rows with Link in File 2:{Environment.NewLine}{string.Join(Environment.NewLine, first100RowsWithLink2)}", "First 100 Rows with Link");

                label11.Invoke((MethodInvoker)(() => label11.Text = $"Elapsed Time: {stopwatch.Elapsed.TotalSeconds} seconds"));
            });

            thread.Start();
        }

        private List<string> GetFirst100RowsWithLink(string filePath)
        {
            List<string> first100RowsWithLink = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                int rowCount = 0;
                while (rowCount < 10 && !reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (ContainsLink(line))
                    {
                        first100RowsWithLink.Add(line);
                        rowCount++;
                    }
                }
            }

            return first100RowsWithLink;
        }

        private bool ContainsLink(string line)
        {
            // Satırın içinde bağlantı olup olmadığını kontrol et
            return line.Contains("http://") || line.Contains("https://");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();

                // "@" sembolü içeren ilk 100 satırı bul
                List<string> first100RowsWithAtSymbol = GetFirst100RowsWithAtSymbolFromFiles(filePath, filePath2);

                stopwatch.Stop();

                MessageBox.Show(string.Join(Environment.NewLine, first100RowsWithAtSymbol), "First 100 Rows with @ Symbol");

                label12.Invoke((MethodInvoker)(() => label12.Text = $"Elapsed Time: {stopwatch.Elapsed.TotalSeconds} seconds"));
            });

            thread.Start();
        }

        List<string> GetFirst100RowsWithAtSymbolFromFiles(string filePath, string filePath2)
        {
            List<string> first100RowsWithAtSymbol = new List<string>();

            // Dosya 1 için işlem
            using (StreamReader reader = new StreamReader(filePath))
            {
                int rowCount = 0;
                while (rowCount < 10 && !reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (ContainsAtSymbol(line))
                    {
                        first100RowsWithAtSymbol.Add(line);
                        rowCount++;
                    }
                }
            }

            // Dosya 2 için işlem
            using (StreamReader reader = new StreamReader(filePath2))
            {
                int rowCount = 0;
                while (rowCount < 10 && !reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (ContainsAtSymbol(line))
                    {
                        first100RowsWithAtSymbol.Add(line);
                        rowCount++;
                    }
                }
            }

            return first100RowsWithAtSymbol;
        }

        bool ContainsAtSymbol(string line)
        {
            // Satırda '@' sembolü var mı kontrol edilir.
            return line.Contains("@");
        }




        private async void button10_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Task<long> task1 = ProcessCsvFileAsync(filePath, "yeni_dosya1.csv", listBox1);
            Task<long> task2 = ProcessCsvFileAsync(filePath2, "yeni_dosya2.csv", listBox2);

            long totalMilliseconds1 = await task1;
            long totalMilliseconds2 = await task2;

            stopwatch.Stop();

            long totalMilliseconds = totalMilliseconds1 + totalMilliseconds2;
            TimeSpan totalTime = TimeSpan.FromMilliseconds(totalMilliseconds);

            Console.WriteLine($"Total elapsed time for both files: {totalTime.TotalSeconds} seconds");
            Console.WriteLine($"Elapsed Time for all operations: {stopwatch.Elapsed.TotalSeconds} seconds");
        }

        private async Task<long> ProcessCsvFileAsync(string sourceFilePath, string destinationFilePath, ListBox listBox)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            long totalMilliseconds = await Task.Run(() => RemoveUrlsAndCreateNewFile(sourceFilePath, destinationFilePath, listBox));

            stopwatch.Stop();

            Console.WriteLine($"Elapsed Time for {sourceFilePath}: {stopwatch.Elapsed.TotalSeconds} seconds");

            return totalMilliseconds;
        }

        private long RemoveUrlsAndCreateNewFile(string sourceFilePath, string destinationFilePath, ListBox listBox)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<string> modifiedLines = new List<string>();

            using (StreamReader reader = new StreamReader(sourceFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string modifiedLine = RemoveUrlAndAt(line, stopwords);
                    modifiedLines.Add(modifiedLine);

                    listBox.Invoke((MethodInvoker)delegate {
                        listBox.Items.Add(modifiedLine);
                    });

                    if (modifiedLines.Count > 10000)
                    {
                        break;
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(destinationFilePath))
            {
                foreach (string modifiedLine in modifiedLines)
                {
                    writer.WriteLine(modifiedLine);
                }
            }

            stopwatch.Stop();

            label14.Invoke((MethodInvoker)delegate {
                label14.Text = $"Elapsed Time: {stopwatch.Elapsed.TotalSeconds} seconds";
            });

            return stopwatch.ElapsedMilliseconds;
        }


        private string RemoveUrlAndAt(string line, HashSet<string> stopwords)
        {
            string cleanedLine = Regex.Replace(line, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?|[@?#]|[\p{Cs}+_\-\&]", "");

            string[] words = cleanedLine.Split(new char[] { ' ',  '.', ':', ';', '!', '?', '♥', '⚽', '✅', '❤' ,'(',')',}, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder resultBuilder = new StringBuilder();
            foreach (string word in words)
            {
                if (!stopwords.Contains(word.ToLower())) 
                {
                    resultBuilder.Append(word);
                    resultBuilder.Append(" ");
                }
            }

            return resultBuilder.ToString().Trim();
        }

       
    }
}

       





    

















