// Лень дальше делать код умным и красивым. Будет как будет тогда, все равно никто не посмотрит.

class Program
{
    static void Main()
    {
        bool showMainMenu = true;
        bool showAddMenu = false;
        bool showRemoveMenu = false;
        bool showEditMenu = false;
        bool showListMenu = false;
        bool showStatsMenu = false;

        int largestWordIndex = 0;
        int spaceBeforeChoiceList = 3;
        int userMainChoice = 0;
        int countTasks = 0;
        int userChoiceLists = 0;
        int howTaskCompleted = 0;
        int howTaskAdd = 0;
        int howTaskEdit = 0;

        string[] userTasksList = new string[0];
        string[] userRemovesTasksLog = new string[0];

        while (showMainMenu)
        {
            // LOGO

            Console.ForegroundColor = ConsoleColor.Blue;
            string[] logoLines = { "==================== HELLO TASKS ====================",
            "Hint:  to quit press: 'Q'"};

            // LIST

            string[] choiceLines = { "1. Open tasks list", "2. Add task", "3. Remove task", "4. Edit task",
            "5. Completed tasks", " ", "Enter your choice: "};

            // USER TASKS LIST

            CenterXY(0, logoLines);

            Console.WriteLine();

            // LIST OF CHOICES

            Console.ForegroundColor = ConsoleColor.Red;

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

            CenterXY(spaceBeforeChoiceList, choiceLines, 1);

            // COORD

            void CenterXY(int yLength, string[] array, int isList = 0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    int effectiveWeight = (isList == 1) ? choiceLines[largestWordIndex].Length : array[i].Length;

                    int centerX1 = (Console.WindowWidth / 2) - (effectiveWeight / 2);
                    int centerY1 = yLength + i;

                    Console.SetCursorPosition(centerX1, centerY1);
                    Console.Write($"{array[i]}");
                }
            }

            // USER ENTER CHECK 

            NumberCheck(Console.ReadLine(), out userMainChoice);

            switch (userMainChoice)
            {
                case 1: UserTasksList(); break;
                case 2: AddTask(); break;
                case 3: RemoveTask(); break;
                case 4: EditTask(); break;
                case 5: Stats(); break;
                default:
                    Console.Clear(); Console.WriteLine("Error #2: We don't have the same option");
                    Thread.Sleep(1000); Console.Clear(); break;
            }
        }

        bool NumberCheck(string userEnter, out int outDigit)
        {   
            if (userEnter == "q")
            {
                Environment.Exit(0);
            }

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

        bool AskUserToContinue()
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
                Console.WriteLine("ADD TASKS ====================");
                Console.WriteLine();
                Console.Write("Task: ");
                task = Console.ReadLine();

                if (task.Length > 1)
                {
                    countTasks++;
                    Array.Resize(ref userTasksList, countTasks);
                    userTasksList[userTasksList.Length - 1] = task;
                }

                howTaskAdd++;

                Console.WriteLine();

                if (!AskUserToContinue())
                {
                    showAddMenu = false;
                }
                else
                {
                    showAddMenu = true;
                }
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
                Console.WriteLine("LIST OF TASKS ====================");
                Console.WriteLine();

                if (userTasksList.Length == 0)
                {
                    Console.WriteLine("You don't have tasks yet");
                }

                for (int i = 0; i < userTasksList.Length; i++)
                {
                    Console.WriteLine($"{i + 1} Task: {userTasksList[i]}");
                }

                Console.WriteLine();

                if (!AskUserToContinue())
                {
                    showListMenu = false;
                }
                else
                {
                    showListMenu = true;
                }
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
                Console.WriteLine("REMOVE TASK ====================");
                Console.WriteLine("Hint: qm - quit menu");
                Console.WriteLine();

                if (userTasksList.Length == 0)
                {
                    Console.WriteLine("You don't have tasks yet");
                    Console.WriteLine();

                    if (!AskUserToContinue())
                    {
                        showRemoveMenu = false;
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }

                for (int i = 0; i < userTasksList.Length; i++)
                {
                    Console.WriteLine($"{i + 1} Task: {userTasksList[i]}");
                }

                Console.WriteLine();
                Console.Write("Which one you want to remove: ");
                string? tempStrToParse = Console.ReadLine();

                if (tempStrToParse == "qm")
                {
                    if (!AskUserToContinue())
                    {
                        showEditMenu = false;
                        return;
                    }
                    else
                    {
                        showEditMenu = true;
                        continue;
                    }
                }

                if (!NumberCheck(Console.ReadLine(), out userChoiceLists))
                {
                    continue;
                }

                if (userChoiceLists > userTasksList.Length || userChoiceLists <= 0)
                {
                    ErrorPrint();
                    continue;
                }

                string[] newUserRemovesTasksLog = new string[userRemovesTasksLog.Length + 1];

                for (int i = 0; i < 1; i++)
                {
                    newUserRemovesTasksLog[i] = userTasksList[userChoiceLists - 1];
                    howTaskCompleted++;
                }

                userRemovesTasksLog = newUserRemovesTasksLog;

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

                Console.WriteLine();

                if (!AskUserToContinue())
                {
                    showRemoveMenu = false;
                }
                else
                {
                    showRemoveMenu = true;
                }
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
                Console.WriteLine("EDIT TASK ====================");
                Console.WriteLine("Hint: qm - quit menu");
                Console.WriteLine();

                if (userTasksList.Length == 0)
                {
                    Console.WriteLine("You don't have tasks yet");

                    if (!AskUserToContinue())
                    {
                        showEditMenu = false;
                        return;
                    }
                    else
                    {
                        showEditMenu = true;
                        continue;
                    }
                }

                for (int i = 0; i < userTasksList.Length; i++)
                {
                    Console.WriteLine($"{i + 1} Task: {userTasksList[i]}");
                }

                Console.WriteLine();
                Console.Write("Choose one from list to edit: ");
                string? tempStrToParse = Console.ReadLine();

                if(tempStrToParse == "qm")
                {
                    if (!AskUserToContinue())
                    {
                        showEditMenu = false;
                        return;
                    }
                    else
                    {
                        showEditMenu = true;
                        continue;
                    }
                }

                if (!NumberCheck(tempStrToParse, out userChoiceLists))
                {
                    continue;
                }

                if (userChoiceLists > userTasksList.Length || userChoiceLists <= 0)
                {
                    ErrorPrint();
                    continue;
                }

                Console.Write("New text: ");
                userTasksList[userChoiceLists - 1] = Console.ReadLine();
                howTaskEdit++;

                Console.WriteLine();

                if (!AskUserToContinue())
                {
                    showEditMenu = false;
                }
                else
                {
                    showEditMenu = true;
                }
            }
        }

        void Stats()
        {
            showMainMenu = false;
            showStatsMenu = true;
            while (showStatsMenu == true)
            {
                Console.ResetColor();
                Console.Clear();
                Console.WriteLine("STATS ====================");
                Console.WriteLine();
                Console.WriteLine("Deleted / completed tasks:");
                Console.WriteLine();

                if (userRemovesTasksLog.Length == 0)
                {
                    Console.WriteLine("You don't have tasks yet");
                }

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

                Console.WriteLine();

                if (!AskUserToContinue())
                {
                    showStatsMenu = false;
                }
                else
                {
                    showStatsMenu = true;
                }
            }
        }
    }
}