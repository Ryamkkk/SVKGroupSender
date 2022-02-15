using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace SVKGroupSender
{
    class Interface
    {
        static string token = null;
        static string text = null;
        //private static Dictionary<string, Data> Information = new Dictionary<string, Data>();
        public static void Menu()
        {
                Console.Clear();
                Console.Title = "SVKGroupSender";
                Console.WriteLine("SVKGroupSender - 1.0.0");
                Console.WriteLine("1. Ввести токен группы.");
                Console.WriteLine("2. Начать рассылку.");
                Console.WriteLine("3. Ввести текст рассылки.");
                Console.WriteLine("4. Перейти в мою группу.");
                Console.WriteLine("5. Выход.");
                int choice = AskForInt(": ");
                switch (choice)
                {
                    case 1:
                        AddGroupToken();
                        break;
                    case 2:
                        StartGroupSend();
                        break;
                    case 3:
                        ChangeTextToSend();
                        break;
                    case 4:
                        Process.Start("https://vk.com/setfpsnepidor");
                        break;
                    case 5:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
        }
        private static int AskForInt(string question)
        {
            int answer = 0;
            bool isInt = false;
            do
            {
                Console.Write(question);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    answer = int.Parse(Console.ReadLine());
                    Console.ResetColor();
                    isInt = true;
                }
                catch (FormatException)
                {
                    Console.ResetColor();
                    Console.WriteLine("Не ввел значение! Попробуй еще раз!");
                    Console.ReadKey();
                }
            } while (!isInt);
            return answer;
        }
        private static void ChangeTextToSend()
        {
            Console.WriteLine(("Введи текст рассылки: "));
            text = Console.ReadLine();
            if (text != null)
            {
                Console.WriteLine("Текст для рассылки успешно добавлен! Возвращаем в меню...");
                Thread.Sleep(1000);
                Menu();
            }
            else
            {
                Environment.Exit(0);
            }
        }
        private static void StartGroupSend()
        {
            if (text != null && token != null)
            {
                Console.WriteLine($"Твой токен: {token}");
                Console.WriteLine($"Твой текст: {text}");
                Thread.Sleep(1000);
                JObject keyValues = JObject.Parse(new WebClient().DownloadString($"https://api.vk.com/method/messages.getConversations?access_token={token}&v=5.130&count=180"));
               
                string value = (string)keyValues["response"]["count"];
                //Console.WriteLine($"Всего пользователей : {value}");
                for (int i = 0; 0 < value.Length; i++)
                {
                    bool isAllowed = (bool)keyValues["response"]["items"][i]["conversation"]["can_write"]["allowed"];
                    if ((string)keyValues["response"]["items"][i]["conversation"]["peer"]["type"] == "user" && isAllowed)
                    {
                        int peerid = (int)keyValues["response"]["items"][i]["conversation"]["peer"]["id"];
                        new WebClient().DownloadString($"https://api.vk.com/method/messages.send?access_token={token}&peer_id={peerid}&random_id=0&message={text}&v=5.130");
                    }

                }    
            }
            else if (text == null)
            {
                Console.WriteLine("Введи текст!");
                Thread.Sleep(1000);
                Menu();
            }
            else if (token == null)
            {
                Console.WriteLine("Введи токен!");
                Thread.Sleep(1000);
                Menu();
            }
            else
            {
                Console.WriteLine("Неизвестная ошибка!");
            }
        }
        private static void AddGroupToken()
        {
            Console.WriteLine(("Введи токен группы: "));
            token = Console.ReadLine();
            if (token != null)
            {
                Console.WriteLine("Токен успешно добавлен! Возвращаем в меню...");
                Thread.Sleep(1000);
                Menu();
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
