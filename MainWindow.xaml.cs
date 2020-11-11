using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace NewParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path = @"Пользователи.csv"; // путь для источника данных

            List<string> currentList = new List<string>();
            List<string> totalList = new List<string>();

            using (StreamReader streamReader = new StreamReader(path, Encoding.Default))
            {
                string s = streamReader.ReadLine();
                System.Windows.MessageBox.Show(s);

                string[] spl = s.Split(';'); // разделяем строку на под строки для парса нужного столбца, 
                                             // если нужно парсить всю строку, заменяем разделитель на что нибудь другое

                string str = "Имя"; // заголовок столбца, данные которого нужно парсить
                currentList.Add("Имя"); // добавление первого элемента, чтобы не смещалось вверх при заполнении

                int i = -1;
                foreach (var item in spl) // выбор нужного столбца 
                {
                    i++;

                    if (item == str)
                        break;
                }

                spl = File.ReadAllLines(path, Encoding.Default);

                for (int k = 0; k < spl.Length; k++)
                {
                    totalList.Add(spl[k]);
                } 

                while (true)
                {
                    try
                    {
                        var line = streamReader.ReadLine(); // считываем линию
                        var values = line.Split(';'); // разделяем её разделителем - запятой, точка с запятой и т.п.
                        currentList.Add(values[i]);  // записываем столбец в лист, где i - нужный нам столбец
                    }
                    catch
                    {
                        streamReader.BaseStream.Position = 0; // возврат считывания в начало
                        break;
                    }
                }

                try
                {
                    for (int l = 0; l < currentList.Count; l++)
                    {
                        bool firstPoint = false;
                        for (int j = 0; j < currentList[l].Length; j++)
                        {
                            if (currentList[l][j] == ' ')
                            {
                                if (firstPoint == true)
                                {
                                    currentList[l] = currentList[l].Remove(j, 1);
                                    currentList[l] = currentList[l].Insert(j, ",");
                                    break;
                                }

                                firstPoint = true;
                                
                            }
                            /*if (priceList[l][j] == ',' || priceList[l][j] == '.')
                            {
                                if (firstPoint == true) priceList[l] = priceList[l].Remove(j, 1);

                                firstPoint = true;          
                                priceList[l] = priceList[l].Replace('.', ','); //замена точек на запятые потому что SQL так сказал
                                continue;
                            }

                            if (priceList[l][j] == '\'') //удаление апострофа
                            {
                                priceList[l] = priceList[l].Remove(j, 1);
                            }

                            if (!char.IsDigit(priceList[l][j])) //если проверенный символ не является числом, то удаляются все символы до конца строки 
                            {
                                priceList[l] = priceList[l].Remove(j, priceList[l].Length - j);
                            }*/
                        }
                    }
                }
                catch
                {

                }

                for (int p = 0; p < totalList.Count - 1; p++)
                {
                    var line = totalList[p];
                    if (line == s) continue; // пропуск головной строки

                    var value = line.Split(';'); // разделение строки на столбцы
                    value[i] = currentList[p]; // присваиваем нужному столбцу отпарсерное значение

                    //for (int h = 0; h < value.Length; h++) // (опционально) добавляем к каждой записи столбца ';', потому что при разделениис строки они слетают
                    //{
                    //    value[h] += ';';
                    //}

                    totalList[p] = string.Concat<string>(value); // соединяем все столбцы в один и присваиваем в лист
                }
            }

            string secondPath = @"C:\Users\Keima\Desktop\ParsedData.csv";

            File.WriteAllLines(secondPath, totalList, Encoding.UTF8);
        }
    }
}
