using System;
using System.Timers;


namespace Laboratornaya_6_N6
{
    internal class Program
    {
        static int employmentFirstOperator = 0;
        static int employmentSecondOperator = 0;
        //static int timeTalkFirst = 0;
        //static int timeTalkSecond = 0;
        static int timerWorkDay = 0;
        static int WorkDay = 1;

        static int queue = 0;

        static Timer timerWork;
        static Timer timerUpdater;
        static Timer timerTalkFirst;
        static Timer timerTalkSecond;


        static int RandomiseTimeTalk()
        {
            Random rand = new Random();
            int time = rand.Next(5000, 5000);
            return time;
        }
        static bool RandomiseClient()
        {
            Random rand = new Random();
            int client = rand.Next(0, 2);
            if (WorkDay == 0)
            {
                return false;
            }           
            return Convert.ToBoolean(client);
        }


        static void Main()
        {
            //int clients = 0;
            //Console.WriteLine("Введите длину рабочего дня в милисекундах: ");
            //timerWorkDay = Convert.ToInt32(Console.ReadLine());
            //Console.WriteLine("Введите количество клиентов: ");
            //clients = Convert.ToInt32(Console.ReadLine());

            ////
            timerWork = new Timer(60000);//мс
            timerWork.Elapsed += TimerWorkEvent;
            timerWork.AutoReset = true;
            timerWork.Enabled = true;
            Console.WriteLine("Начало рабочего дня в {0:HH:mm:ss}", DateTime.Now);
            Console.WriteLine("Длина рабочего дня 100000 / {0} мс\n", timerWorkDay);
            //Console.WriteLine("Количество клиентов {0}", clients);
            ////
            timerUpdater = new Timer(1000);//мс
            timerUpdater.Elapsed += timerUpdaterEvent;
            timerUpdater.AutoReset = true;
            timerUpdater.Enabled = true;

            Console.ReadLine();
        }


        private static void TimerWorkEvent(Object source, ElapsedEventArgs e)
        {
            WorkDay = 0;
            Console.WriteLine("\nРабочий день закончен в {0:HH:mm:ss}\nЖдём завершения текущих звонков, новые звонки не принимаются.\n", e.SignalTime);

            timerWork.Stop();
            timerWork.Dispose();
        }

        private static void TimeTalkFirstEvent(Object source, ElapsedEventArgs e)
        {
            queue--;
            Console.WriteLine("Звонок первому оператору завершен в {0:HH:mm:ss}", e.SignalTime);
            Console.WriteLine("Очередь обновлена.\nТекущая очередь: {0}", queue);
            employmentFirstOperator = 0;
            timerTalkFirst.Stop();
            timerTalkFirst.Dispose();
        }
        private static void TimeTalkSecondEvent(Object source, ElapsedEventArgs e)
        {
            queue--;
            Console.WriteLine("Звонок второму оператору завершен в {0:HH:mm:ss}", e.SignalTime);            
            Console.WriteLine("Очередь обновлена.\nТекущая очередь: {0}", queue);
            employmentSecondOperator = 0;
            timerTalkSecond.Stop();
            timerTalkSecond.Dispose();
        }

        private static void timerUpdaterEvent(Object source, ElapsedEventArgs e)
        {
            switch (RandomiseClient())
            {
                case true:
                    Console.WriteLine("{0:HH:mm:ss} Поступил звонок ", e.SignalTime);
                    if (WorkDay == 0)
                    {
                        Console.WriteLine("Рабочий день был закончен, звонки больше не принимаются...");
                        break;
                    }                    
                    queue++;
                    Console.WriteLine("Очередь обновлена.\nТекущая очередь: {0}", queue);
                    if (employmentFirstOperator == 0)
                    {
                        Console.WriteLine("Первый оператор принял звонок.");
                        employmentFirstOperator = 1;
                        timerTalkFirst = new Timer(RandomiseTimeTalk());
                        timerTalkFirst.Elapsed += TimeTalkFirstEvent;
                        timerTalkFirst.AutoReset = true;
                        timerTalkFirst.Enabled = true;
                        break;
                    }
                    if (employmentSecondOperator == 0)
                    {
                        Console.WriteLine("Второй оператор принял звонок.");
                        employmentSecondOperator = 1;
                        timerTalkSecond = new Timer(RandomiseTimeTalk());
                        timerTalkSecond.Elapsed += TimeTalkSecondEvent;
                        timerTalkSecond.AutoReset = true;
                        timerTalkSecond.Enabled = true;
                        break;
                    }
                    break;
                case false:
                    Console.WriteLine("{0:HH:mm:ss} ----- ", e.SignalTime);
                    break;
            }
            if(employmentFirstOperator == 0 && employmentSecondOperator == 0 && WorkDay == 0)
            {
                Console.WriteLine("\nВсе звонки завершены");
                Console.WriteLine("Необслужено клиентов: {0}", queue);
                timerUpdater.Stop();
                timerUpdater.Dispose();
            }     
        }
    }
}
