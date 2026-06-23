// !test commit by git

class Program
{
    enum MainOptions : byte
    {
        ListOfTasks = 1,
        AddTasks,
        RemoveTasks,
        EditTasks,
        TimerForWork,
        DeletedTasks,
        StatsTasks
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        DateTime startTime = DateTime.Now; // Фиксируем время запуска для таймера сессии

        MainOptions mainOptions = MainOptions.StatsTasks;
    
        bool showMainMenu = true;
        bool showAddMenu = false;
        bool showRemoveMenu = false;
        bool showEditMenu = false;
        bool showListMenu = false;
        bool showTimerMenu = false;
        bool showStatsMenu = false;

        int largestWordIndex = 0; // Индекс самого длинного слова для центровки списка
        int spaceForListUnderTitle = 3; // Отступ списка от верхнего края
        int userMainChoice = 0; // Выбор пользователя в главном меню
        int countTasks = 0; // Актуальный счетчик задач (синхронизирован с файлом)
        int countLogRemoveTasks = 0; // Счетчик удаленных задач (синхронизирован с файлом)
        int userChoiceLists = 0; // Индекс задачи, выбранной пользователем внутри списков
        int userDigitForTimer = 0;  // Секунды для обратного отсчета таймера.


        // Статистика сессии
        int howTaskCompleted = 0;
        int howTaskAdd = 0;
        int howTaskEdit = 0;

        string ListfileName = "tasks.txt";
        string LogfileName = "removeLog.txt";
        string[] userTasksList = new string[0];
        string[] userRemovesTasksLog = new string[0];

        string[] logoLines = {
      "==================== HELLO TASKS 0.3.1 ====================",
      "Hint:  to quit press: 'Q'"
    };

        string[] choiceLines = {
      "1. Open tasks list", "2. Add task", "3. Remove task", "4. Edit task", "5. Timer for work",
      "6. Completed tasks", " ", "Enter your choice: "
    };

        string[] pageTitle = {
      "ADD TASKS ====================",      // [0]
            "LIST OF TASKS ====================",    // [1]
            "REMOVE TASK ====================",     // [2]
            "EDIT TASK ====================",       // [3]
            "STATS ====================",           // [4]
            "Hint: qm - quit menu",                // [5]
            "Deleted / completed tasks:",            // [6]
            "TIMER ===================="           // [7]
    };

        // --- ПОДГОТОВКА ФАЙЛОВОЙ СИСТЕМЫ ---

        if (!File.Exists(ListfileName)) // Проверка на наличие файла рядом с exe.
        {
            File.Create(ListfileName).Close(); // Если нету создаем и сразу же закрываем, чтоб можно было менять.
        }

        userTasksList = File.ReadAllLines(ListfileName); // Загружаем данные из файла в массив при старте
        countTasks = userTasksList.Length; // Синхронизируем счетчик с реальным количеством строк в файле

        if (!File.Exists(LogfileName))
        {
            File.Create(LogfileName).Close();
        }

        userRemovesTasksLog = File.ReadAllLines(LogfileName);
        countLogRemoveTasks = userRemovesTasksLog.Length;

        while (showMainMenu)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            CenterXY(0, logoLines);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;


            // Логика поиска самого длинного слова для красивой центровки списка выбора
            for (int i = 0; i < choiceLines.Length; i++)
            {

                for (int j = 1; j < choiceLines.Length; j++)
                {
                    if (choiceLines[i].Length < choiceLines[j].Length)
                    {
                        largestWordIndex = j;
                    }
                }
            }

            CenterXY(spaceForListUnderTitle, choiceLines, 1); // Отрисовка меню выбора

            NumberCheck(Console.ReadLine() ?? "", out userMainChoice); // Ввод пользователя и проверка (метод NumberCheck)

            if (Enum.IsDefined(typeof(MainOptions), (byte)userMainChoice))
                mainOptions = (MainOptions)userMainChoice;
            else
                ErrorPrint();

            switch (mainOptions) // выбор из списка ( выбор функции ).
            {
                case MainOptions.ListOfTasks : UserTasksList(); break;
                case MainOptions.AddTasks : AddTask(); break;
                case MainOptions.RemoveTasks : RemoveTask(); break;
                case MainOptions.EditTasks : EditTask(); break;
                case MainOptions.TimerForWork : Timer(); break;
                case MainOptions.StatsTasks: Stats(); break;
                default:
                    Console.Clear();
                    Console.WriteLine("Error #2: We don't have the same option");
                    Thread.Sleep(1000);
                    Console.Clear();
                    break;
            }
        }

        // --- ЛОКАЛЬНЫЕ МЕТОДЫ (ИНСТРУМЕНТЫ) ---


        // Центровка текста в консоли
        void CenterXY(int yLength, string[] array, int isList = 0)
        {
            for (int i = 0; i < array.Length; i++)
            {
                // Если это список, центруем по самому длинному элементу, иначе по текущей строке
                int effectiveWeight = (isList == 1) ? choiceLines[largestWordIndex].Length : array[i].Length;

                int centerX1 = (Console.WindowWidth / 2) - (effectiveWeight / 2);
                int centerY1 = yLength + i;

                Console.SetCursorPosition(centerX1, centerY1);
                Console.Write($"{array[i]}");
            }
        }

        // Универсальная проверка ввода на число и системные команды (q)
        bool NumberCheck(string userEnter, out int outDigit)
        {
            if (userEnter == "q") Environment.Exit(0); // Выход из программы.

            if (!int.TryParse(userEnter, out outDigit))
            {
                ErrorPrint();
                return false;
            }

            return true;
        }

        void ErrorPrint()
        {
            Console.Clear();
            Console.WriteLine("Error #1: Error input");
            Thread.Sleep(1000);
            Console.Clear();
        }

        bool AskUserToContinue() // Запрос на возврат в меню или продолжение цикла метода
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Do you want to continue? (y/n): ");

            if (Console.ReadLine() == "y")
            {
                Console.Clear();
                return true;
            }

            else
            {
                Console.Clear();
                showMainMenu = true;
                return false;
            }
        }

        // --- ЛОГИКА МЕНЮ (РАБОЧИЕ МЕТОДЫ) ---

        void AddTask()
        {
            showMainMenu = false;
            showAddMenu = true;
            while (showAddMenu == true)
            {
                Console.ResetColor();
                showMainMenu = false;
                string? task;

                Console.Clear();
                Console.WriteLine(pageTitle[0]); // "ADD TASKS"
                Console.WriteLine();
                Console.Write("Task: ");
                task = Console.ReadLine() ?? "";

                if (task.Length > 1)
                {
                    countTasks++;
                    Array.Resize(ref userTasksList, countTasks); // Расширяем массив
                    userTasksList[userTasksList.Length - 1] = task; // Кладем в конец
                }

                File.WriteAllLines(ListfileName, userTasksList); // Сохраняем в txt
                howTaskAdd++;

                Console.WriteLine();

                showAddMenu = AskUserToContinue();
            }
        }

        void UserTasksList()
        {
            showMainMenu = false;
            showListMenu = true;
            while (showListMenu == true)
            {
                Console.ResetColor();
                Console.Clear();
                Console.WriteLine(pageTitle[1]); // "LIST OF TASKS"
                Console.WriteLine();

                if (userTasksList.Length == 0) Console.WriteLine("You don't have tasks yet");

                for (int i = 0; i < userTasksList.Length; i++)
                {
                    Console.WriteLine($"{i + 1} Task: {userTasksList[i]}");
                }

                Console.WriteLine();
                showListMenu = AskUserToContinue();
            }
        }

        void RemoveTask()
        {
            showMainMenu = false;
            showRemoveMenu = true;

            while (showRemoveMenu == true)
            {
                Console.ResetColor();
                Console.Clear();
                Console.WriteLine(pageTitle[2]); // "REMOVE TASK"
                Console.WriteLine(pageTitle[5]); // "Hint: qm"
                Console.WriteLine();

                if (userTasksList.Length == 0)
                {
                    Console.WriteLine("You don't have tasks yet");
                    Console.WriteLine();

                    if (!AskUserToContinue()) { showRemoveMenu = false; return; }
                    else continue;
                }

                for (int i = 0; i < userTasksList.Length; i++)
                {
                    Console.WriteLine($"{i + 1} Task: {userTasksList[i]}");
                }

                Console.WriteLine();
                Console.Write("Which one you want to remove: ");
                string? tempStrToParse = Console.ReadLine();

                if (tempStrToParse == "qm") // Выход в меню через команду
                {
                    Console.WriteLine();
                    showRemoveMenu = AskUserToContinue();
                    if (!showRemoveMenu) return; else continue;
                }

                if (!NumberCheck(tempStrToParse ?? "", out userChoiceLists)) continue;

                if (userChoiceLists > userTasksList.Length || userChoiceLists <= 0)
                {
                    ErrorPrint(); continue;
                }

                // Логика сохранения удаленной задачи в лог (Stats)
                string[] newUserRemovesTasksLog = userRemovesTasksLog;

                for (int i = 0; i < userRemovesTasksLog.Length; i++)
                {
                    newUserRemovesTasksLog[i] = userRemovesTasksLog[i];
                }

                newUserRemovesTasksLog[newUserRemovesTasksLog.Length - 1] = userTasksList[userChoiceLists - 1];
                userRemovesTasksLog = newUserRemovesTasksLog;

                File.WriteAllLines(LogfileName, newUserRemovesTasksLog); // Сохраняем в txt
                countLogRemoveTasks++;

                // Логика удаления из основного массива (создание уменьшенной копии)
                string[] newTasksListArray = new string[userTasksList.Length - 1];

                for (int i = 0; i < userChoiceLists - 1; i++)
                {
                    newTasksListArray[i] = userTasksList[i];
                }

                for (int i = userChoiceLists - 1 + 1; i < userTasksList.Length; i++)
                {
                    newTasksListArray[i - 1] = userTasksList[i];
                }

                userTasksList = newTasksListArray;
                countTasks = userTasksList.Length; // Обновляем счетчик для Resize
                File.WriteAllLines(ListfileName, userTasksList); // Синхронизируем с файлом
                howTaskCompleted++;

                Console.WriteLine();
                showRemoveMenu = AskUserToContinue();
            }
        }

        void EditTask()
        {
            showMainMenu = false;
            showEditMenu = true;
            while (showEditMenu == true)
            {
                Console.Clear();
                Console.ResetColor();
                Console.WriteLine(pageTitle[3]); // "EDIT TASK"
                Console.WriteLine(pageTitle[5]);
                Console.WriteLine();

                if (userTasksList.Length == 0)
                {
                    Console.WriteLine("You don't have tasks yet");
                    if (!AskUserToContinue()) { showEditMenu = false; return; }
                    else continue;
                }

                for (int i = 0; i < userTasksList.Length; i++)
                {
                    Console.WriteLine($"{i + 1} Task: {userTasksList[i]}");
                }

                Console.WriteLine();
                Console.Write("Choose one from list to edit: ");
                string? tempStrToParse = Console.ReadLine();

                if (tempStrToParse == "qm")
                {
                    showEditMenu = AskUserToContinue();
                    if (!showEditMenu) return; else continue;
                }

                if (!NumberCheck(tempStrToParse ?? "", out userChoiceLists)) continue;

                if (userChoiceLists > userTasksList.Length || userChoiceLists <= 0)
                {
                    ErrorPrint(); continue;
                }

                Console.Write("New text: ");
                userTasksList[userChoiceLists - 1] = Console.ReadLine() ?? "";

                File.WriteAllLines(ListfileName, userTasksList);
                howTaskEdit++;

                Console.WriteLine();
                showEditMenu = AskUserToContinue();
            }
        }

        void Timer()
        {
            showMainMenu = false;
            showTimerMenu = true;

            while (showTimerMenu == true)
            {
                Console.ResetColor();
                Console.Clear();
                Console.WriteLine(pageTitle[7]); // "TIMER"
                Console.WriteLine();

                Console.Write("Enter in seconds to start the timer: ");
                string? tempStrToParse = Console.ReadLine();

                if (!NumberCheck(tempStrToParse ?? "", out userDigitForTimer)) continue;

                if (userDigitForTimer < 0) continue;

                for (int i = userDigitForTimer; i < userDigitForTimer + 1 && i >= 0; i--)
                {
                    Thread.Sleep(1000);
                    Console.Clear();
                    Console.WriteLine($"Seconds: {i}");
                }

                Console.WriteLine();
                showTimerMenu = AskUserToContinue();
            }
        }

        void Stats()
        {
            showMainMenu = false;
            showStatsMenu = true;

            while (showStatsMenu == true)
            {
                // Считаем время сессии (текущее время минус время старта)
                TimeSpan sessionTime = DateTime.Now - startTime;

                Console.ResetColor();
                Console.Clear();
                Console.WriteLine(pageTitle[4]); // "STATS"
                Console.WriteLine(pageTitle[6]); // "Deleted / completed"
                Console.WriteLine();

                if (userRemovesTasksLog.Length == 0) Console.WriteLine("No history for this session");

                for (int i = 0; i < userRemovesTasksLog.Length; i++)
                {
                    Console.WriteLine($"{i + 1} Task: {userRemovesTasksLog[i]}");
                }

                Console.WriteLine();
                Console.ResetColor();
                Console.WriteLine("Digits stats:");
                Console.WriteLine();
                Console.WriteLine($"Add tasks: {howTaskAdd}");
                Console.WriteLine($"Edit tasks: {howTaskEdit}");
                Console.WriteLine($"Completed tasks: {howTaskCompleted}");

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Time in session: {sessionTime.Minutes}m {sessionTime.Seconds}s");
                Console.ResetColor();

                Console.WriteLine();
                showStatsMenu = AskUserToContinue();
            }
        }
    }
}