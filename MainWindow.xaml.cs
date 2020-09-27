using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            string path = @"Ингредиенты1.csv";

            List<string> priceList = new List<string>();
            List<string> totalList = new List<string>();

            using (StreamReader streamReader = new StreamReader(path, Encoding.Default))
            {
                string s = streamReader.ReadLine();
                System.Windows.MessageBox.Show(s);

                string[] spl = s.Split(';');
                string str = "Price";
                priceList.Add("Цена"); //добавление первого элемента, чтобы не смещалось вверх при заполнении

                int i = -1;
                foreach (var item in spl)
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
                        var line = streamReader.ReadLine();
                        var values = line.Split(';');
                        priceList.Add(values[i]);
                    }
                    catch
                    {
                        streamReader.BaseStream.Position = 0;
                        break;
                    }
                }

                try
                {
                    for (int l = 0; i < priceList.Count; l++)
                    {
                        bool firstPoint = false;
                        for (int j = 0; j < priceList[l].Length; j++)
                        {
                            if (priceList[l][j] == ',' || priceList[l][j] == '.')
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
                            }
                        }
                    }
                }
                catch
                {

                }

                for(int p = 0; p < totalList.Count - 1; p++)
                {
                    var line = totalList[p];
                    if (line == s) continue; //пропуск головной строки

                    var value = line.Split(';'); //разделение строки на столбцы
                    value[i] = priceList[p]; //присваиваем нужному столбцу отпарсерное значение

                    for (int h = 0; h < value.Length; h++) //добавляем к каждой записи столбца ';', потому что при разделениис строки они слетают
                    {
                        value[h] += ';';
                    }

                    totalList[p] = string.Concat<string>(value); //соединяем все столбцы в один и присваиваем в лист
                }
            }

            string secondPath = @"C:\Users\Keima\Desktop\ParsedData.csv";

            File.WriteAllLines(secondPath, totalList, Encoding.Default);
        }
    }
}
