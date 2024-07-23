using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Less04
{
    internal class Program
    {

        class person
        {
            public person() { }
            public person(string name, string surname, uint day, uint month, uint year)
            {
                _name = name;
                _surname = surname;
                _day = day;
                _month = month;
                _year = year;
            }
       
            string _name;
            string _surname;
            uint _day;
            uint _month;
            uint _year;


            public string name
            {
                get { return _name; }
                set { _name = value; }
            }

            public string surname
            {
                get { return _surname; }
                set { _surname = value; }
            }

            public uint day
            {
                get { return _day; }
                set { _day = value; }
            }

            public uint month
            {
                get { return _month; }
                set { _month = value; }
            }

            public uint year
            {
                get { return _year; }
                set { _year = value; }
            }

            public void print() // вывод на экран
            {
                Console.WriteLine(_name + " " + _surname + " " + _day + "/" + _month + "/" + _year);
            }

            public string getText() // вернуть обьект в виде строки
            {
                return (_name + " " + _surname + " " + _day + " " + _month + " " + _year);
            }

            // перегрузка операторов сравнения для дальнейшей сортировки обьектов person
            public static bool operator < (person p1, person p2) 
            {
                if (p1.month == p2.month)
                    return p1.day > p2.day;
                else if (p1.month > p2.month)
                    return p1.month > p2.month;
                else return false;

            }

            public static bool operator > (person p1, person p2)
            {
                if (p1.month == p2.month)
                    return p1.day > p2.day;
                else if (p1.month > p2.month) 
                    return p1.month > p2.month;
                else return false;
            }


        }


        class friendFactory // Функциональность приложения вынес в отдельный класс
        {
            public person addFriend() // Добавление друга
            {

                while (true)
                {
                    Console.WriteLine("Введите имя: ");
                    _name = Console.ReadLine();
                    
                    if (_name != "")
                        break;
                }

                while (true)
                {
                    Console.WriteLine("Введите фамилию: ");
                    _surname = Console.ReadLine();
                    
                    if (_surname != "")
                        break;
                }
                
                while (true)
                {
                    Console.WriteLine("Введите день рождения");
                    _day = uint.Parse(Console.ReadLine());

                    if (_day <= 31)
                        break;
                }

                while (true)
                {
                    Console.WriteLine("Введите месяц рождения");
                    _month = uint.Parse(Console.ReadLine());

                    if (_month <= 12)
                        break;
                }

                while (true)
                {
                    Console.WriteLine("Введите год рождения");
                    _year = uint.Parse(Console.ReadLine());

                    if (_year <= 9999)
                        break;
                }

                person friend = new person(_name, _surname, _day, _month, _year); // создали друга
                File.AppendAllText("./friends.txt", _name + " " + _surname + " " + _day + " " + _month + " " + _year + "\n"); // записали строкой в файл
                Console.WriteLine("Друг был добавлен");

                return friend;

            }

            // Данный метод читает файл списка друзей, создает и заполняет массив обьектов person
            public person[] selectAllFriends() 
            {
                string[] paragraph = File.ReadAllText("./friends.txt").Split(new char[] { '\n' }); //Split делит текст на массив строк
                
                int counter = -1; // для подсчета строк
                foreach (string s in paragraph)
                {
                    counter++;
                }

                person[] p = new person[counter]; // создаем массив person на количество записей друзей

                string allText = File.ReadAllText("./friends.txt"); // получили список друзей одной строкой
                allText = allText.Replace("\n", " "); // убрали все переновы строк /n заменив их на пробел
                allText = allText.Trim(); // убрали поледний пробел в конце строки, так как он делает лишний элемент в массиве слов

                string[] words = allText.Split(new char[] { ' ' }); // получили массив слов
                

                int step = 0; // шаг заполнения полей обьекта person
                int index = 0; // индекс массива person
                foreach (string t in words) // поочередно заполняем поля каждого обьекта person
                {
                    switch (step)
                    {
                        case 0:
                            p[index] = new person();
                            p[index].name = t; 
                            break;

                        case 1:
                            p[index].surname = t;
                            break;

                        case 2 :
                            p[index].day = uint.Parse(t);
                            break;

                        case 3 :
                            p[index].month = uint.Parse(t);
                            break;
                        
                        case 4 :
                            p[index].year = uint.Parse(t);
                            break;

                    }
                    step++;
                    if(step > 4) // обнуляем шаг заполнения полей, и сдвигаемся к следующему элементу массива
                    {
                        step = 0;
                        index++;
                    }
                }

                return p;
                
            }

            //Удаление друга
            public void deleteFriend(string line)
            {
                line += '\n'; // чтобы небыло пустой строки после удаления
                string allText = File.ReadAllText("./friends.txt"); // получили список друзей одной строкой
                allText = allText.Replace(line, ""); //заменили запись друга на "" 
                File.WriteAllText("./friends.txt", allText); //перезаписали файл
                Console.WriteLine(line + " был удален из списка друзей");
            }

            // Редактирование друга
            public void editFriend (string lineOld, string lineNew)
            {
                string allText = File.ReadAllText("./friends.txt"); // получили список друзей одной строкой
                allText = allText.Replace(lineOld, lineNew); //заменили старую запись на новую 
                File.WriteAllText("./friends.txt", allText); //перезаписали файл
                Console.WriteLine(lineOld + " был замене на " + lineNew);
            }

            // Сортировка друзей
            public person[] sortFriend(person[] p)
            {
                if (p.Length > 2) // проверка чтобы было минимум два друга в списке
                {
                    person buffer;
                    bool stop;
                    while (true)
                    {
                        stop = true;
                        for (int i = 0; i < p.Length - 1; i++)
                        {

                            if (p[i] > p[i + 1]) // сравнимаем по датам два соседних обьекта person
                            {
                                buffer = p[i];
                                p[i] = p[i + 1];
                                p[i + 1] = buffer;
                                stop = false;
                            }

                        }
                        if (stop)
                            break;
                    }
                    return p;
                }
                else return null;
            }

            string _name;
            string _surname;
            uint _day;
            uint _month;
            uint _year;

        }
        static void Main(string[] args)
        {
            friendFactory f = new friendFactory();
            
            Console.WriteLine("0.Добавить друга\n1.Показать друзей\n2.Удалить друга\n3.Редактировать друга\n4.Отсортировать друзей по датам\n5.Показать ДР ближайшего");

            while (true)
            {
                uint num = uint.Parse(Console.ReadLine());
                if (num > 6)
                    break;

                switch (num)
                {
                    case 0:// добавить друга
                        f.addFriend();
                        break;

                    case 1:// показать друзей
                        person[] per = f.selectAllFriends();
                        uint number = 0; // для визуального отображения индексов
                        foreach (person item in per)
                        {
                            Console.Write(number + ": "); // показываем индекс
                            item.print(); // затем показываем друга
                            number++;
                        }
                        break;

                    case 2:// удалить друга
                        person[] per2 = f.selectAllFriends();
                        uint number2 = 0; // для визуального отображения индексов
                        foreach (person item in per2)
                        {
                            Console.Write(number2 + ": "); // показываем индекс
                            item.print(); // затем показываем друга
                            number2++;
                        }
                        Console.WriteLine("Введите индекс друга которого хотите удалить:");
                        uint index = uint.Parse(Console.ReadLine()); // индекс удаляемого друга
                        if (index < per2.Length) // если индекс не больше длины массива друзей
                            f.deleteFriend(per2[index].getText()); // удаляем друга
                        else
                            Console.WriteLine("Нет такого индекса");
                        break;

                    case 3:// редактировать друга

                        person[] per3 = f.selectAllFriends();
                        uint number3 = 0; // для визуального отображения индексов
                        foreach (person item in per3)
                        {
                            Console.Write(number3 + ": "); // показываем индекс
                            item.print(); // затем показываем друга
                            number3++;
                        }
                        Console.WriteLine("Введите индекс редактируемого друга:");
                        uint index3 = uint.Parse(Console.ReadLine()); // индекс редактируемого друга
                        if (index3 < per3.Length)
                        {
                            f.editFriend(per3[index3].getText(), f.addFriend().getText()); // метод editFriend принимает две строки, первая старая строка, вторая новая
                        }
                        else
                            Console.WriteLine("Нет такого индекса");
                        break;

                    case 4:// отсортировать друзей
                        
                        person[] per4 = f.sortFriend(f.selectAllFriends());

                            foreach (person item4 in per4)
                            {
                                item4.print();
                            }
                        
                        break;
                        case 5:// показать др ближайшего
                                           
                        person dateNow = new person("date", "now", (uint)DateTime.Now.Day, (uint)DateTime.Now.Month, (uint)DateTime.Now.Year);
                        person[] allFriends = f.sortFriend(f.selectAllFriends());
                        foreach (person item4 in allFriends)
                        {

                            if (item4.month == dateNow.month && item4.day == dateNow.day)
                            {
                                Console.WriteLine("Сегодня день рождение у " + item4.getText());
                                break;
                            }
                            else if (item4 > dateNow)
                            {
                                Console.WriteLine("Ближайший день рождения " + item4.getText());
                                break;
                            }
                            
                        }

                            break;
                  
                }
            }

        }

    }
}
