using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace barsGroupTaskThreads
{
    public class LaunchProgram
    {
        private DummyRequestHandler requestHandler = new DummyRequestHandler();
        private void sendMessage(string message, List<string> arguments, string id)
        {
            string returned = "";

            try
            {     
                returned = requestHandler.HandleRequest(message, arguments.ToArray<string>());
                consoleAnswer($"Сообщение с идентификатором {id} получило ответ - {returned}", true);
            }            
            catch (Exception ex)
            {
                consoleAnswer($"Сообщение с идентификатором {id} упало с ошибкой - {ex.Message}", false);
            }
        }
        private void consoleAnswer(string message, bool succed)
        {
            if (succed)
                changeForegroundColor(2);
            else
                changeForegroundColor(1);
            Console.WriteLine(message);
            changeForegroundColor(0);
        }
        private void changeForegroundColor(int color)
        {
            if (color == 1)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (color == 2)
                Console.ForegroundColor = ConsoleColor.Green;
            else 
                Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void Launch()
        {
            consoleAnswer("Приложение запущено.", true);

            List<string> arguments = new List<string>();

            string message = "", argument = "", id;

            Console.WriteLine("Введите текст запроса для отправки. Для выхода введите /exit");
            message = Console.ReadLine();

            while (message != "/exit")
            {
                argument = "";
                
                Console.WriteLine($"Будет послано сообщение '{message}'");
                Console.WriteLine("Введите аргумент сообщения. Если аргументы не нужны - введите /end");

                while (argument != "/end")
                {
                    argument = Console.ReadLine();
                    if (argument != "/end")
                    {
                        arguments.Add(argument);
                        Console.WriteLine("Введите следующий аргумент сообщения. Для окончания добавления аргументов введите /end");
                    }
                }
                id = Guid.NewGuid().ToString("D");
                consoleAnswer($"Было отправлено сообщение '{message}'. Присвоен идентификатор {id}", true);

                ThreadPool.QueueUserWorkItem(callBack => sendMessage(message, arguments, id));

                Console.WriteLine("Введите текст запроса для отправки. Для выхода введите /exit");
                message = Console.ReadLine();
            }
            Console.WriteLine("Завершение работы приложения");
        }
    }
}
