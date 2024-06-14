using Microsoft.EntityFrameworkCore;
using context.Models;
using System.Linq;

namespace context;
public class Program
{
    public static void Main(string[] args)
    {
        Context db = new Context();
       
        Console.WriteLine("1 - Регистрация");
        Console.WriteLine("2 - Авторизация");

        int choose = Convert.ToInt32(Console.ReadLine());

        switch (choose)
        {
            case 1: // регистрация
                {
                    Console.WriteLine("Введите Фамилию");
                    string Surname = Console.ReadLine();

                    Console.WriteLine("Введите Имя");
                    string Name = Console.ReadLine();

                    Console.WriteLine("Введите Отчество");
                    string MiddleName = Console.ReadLine();

                    Console.WriteLine("Введите Логин");
                    string Login = Console.ReadLine();

                    Console.WriteLine("Введите Пароль");
                    string Password = Console.ReadLine();

                    Models.Users newUser = new Users()
                    {
                        Surname = Surname,
                        Name = Name,
                        MiddleName = MiddleName,
                        Login = Login,
                        Password = Password
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    Console.WriteLine("Вы зарегистрировались");
                    break;
                }

            case 2: // авторизация
                {
                    Console.WriteLine("Введите логин");
                    string Login = Console.ReadLine();

                    Console.WriteLine("Введите пароль");
                    string Password = Console.ReadLine();
                    
                    Users user = db.Users.FirstOrDefault(u => u.Login == Login && u.Password == Password);

                    if (Login == user.Login && Password == user.Password)
                    {
                        Console.WriteLine("Вы вошли");

                        Console.WriteLine("Выберите действие");

                        Console.WriteLine("0 - Выход");
                        Console.WriteLine("1 - Посмотреть таблицу с задачами");
                        Console.WriteLine("2 - Добавить задачу");
                        Console.WriteLine("3 - Изменить задачу");
                        Console.WriteLine("4 - Удалить задачу");
                        Console.WriteLine("5 - Просмотреть задачи на сегодня");
                        Console.WriteLine("6 - Просмотреть задачи на завтра");
                        Console.WriteLine("7 - Просмотреть задачи на неделю");
                        Console.WriteLine("8 - Просмотреть задачи которые предстоит выполнить");
                        Console.WriteLine("9 - Просмотреть задачи у которых срок выполнения прошел");

                        choose = Convert.ToInt32(Console.ReadLine());
                        while (choose != 0)
                        {

                            switch (choose)
                            {
                                case 1: // вывод таблицы
                                    {
                                        Console.WriteLine("Ваши задачи");

                                        foreach (var task in db.Tasks.Include(r => r.IdUserNavigation).Where(r => r.IdUser == user.Id))
                                        {
                                            Console.WriteLine($"Название задачи: {task.NameTask} - описание: {task.Description}  - срок выполнения: {task.Deadline}");
                                        }
                                        Console.WriteLine("\n------------------------------------------------------------\n");

                                        break;
                                    }
                                case 2: // добавление в таблицу
                                    {
                                        Console.WriteLine("Введите название задачи");
                                        string nameTask = Console.ReadLine();
                                        Console.WriteLine("Введите описание задачи");
                                        string description = Console.ReadLine();
                                        Console.WriteLine("Введите сроки выполнения в формате дд.мм.гггг");
                                        DateTime deadline = Convert.ToDateTime(Console.ReadLine());
                                        deadline = DateTime.SpecifyKind(deadline, DateTimeKind.Utc);

                                        Models.Tasks tasks = new Tasks()
                                        {
                                            NameTask = nameTask,
                                            Description = description,
                                            Deadline = deadline,
                                            IdUser = user.Id
                                        };
                                        db.Tasks.Add(tasks);
                                        db.SaveChanges();
                                        Console.WriteLine("Задача добавлена");
                                        break;
                                    }
                                case 3: // изменение задачи
                                    {
                                        Console.WriteLine("Введите название задачи которую хотите изменить");
                                        string nameTask = Console.ReadLine();

                                        Console.WriteLine("Введите новое название задачи");
                                        string newNameTask = Console.ReadLine();

                                        Console.WriteLine("Введите новое описание задачи");
                                        string newDescription = Console.ReadLine();

                                        Console.WriteLine("Введите новый срок выполнения задачи в формате дд.мм.гггг");
                                        DateTime newDeadline = Convert.ToDateTime(Console.ReadLine());
                                        newDeadline = DateTime.SpecifyKind(newDeadline, DateTimeKind.Utc);

                                        var updateTask = db.Tasks.Include(r => r.IdUserNavigation).FirstOrDefault(n => n.NameTask == nameTask);
                                        updateTask.NameTask = newNameTask;
                                        updateTask.Deadline = newDeadline;
                                        updateTask.Description = newDescription;
                                        db.SaveChanges();
                                        Console.WriteLine("Задача изменена");

                                        break;
                                    }
                                case 4: // удаление задачи
                                    {
                                        Console.WriteLine("Введите название задачи которую хотите удалить");
                                        string nameTask = Console.ReadLine();

                                        var removeTask = db.Tasks.Include(r => r.IdUserNavigation).FirstOrDefault(n => n.NameTask == nameTask);

                                        db.Tasks.Remove(removeTask);
                                        db.SaveChanges();

                                        Console.WriteLine("Задача удалена");
                                        break;
                                    }
                                case 5:
                                    {
                                        Console.WriteLine("Задачи на сегодня");

                                        foreach (var task in db.Tasks.Include(r => r.IdUserNavigation).Where(r => r.Deadline == DateTime.UtcNow.Date && r.IdUser == user.Id))
                                        {
                                            Console.WriteLine($"Название задачи: {task.NameTask} - описание: {task.Description}  - срок выполнения: {task.Deadline}");
                                        }
                                        Console.WriteLine("\n------------------------------------------------------------\n");

                                        break;
                                    }
                                case 6:
                                    {
                                        Console.WriteLine("Задачи на завтра");

                                        foreach (var task in db.Tasks.Include(r => r.IdUserNavigation).Where(r => r.Deadline == DateTime.UtcNow.Date.AddDays(1) && r.IdUser == user.Id))
                                        {
                                            Console.WriteLine($"Название задачи: {task.NameTask} - описание: {task.Description}  - срок выполнения: {task.Deadline}");
                                        }
                                        Console.WriteLine("\n------------------------------------------------------------\n");


                                        break;
                                    }
                                case 7:
                                    {
                                        Console.WriteLine("Задачи на неделю");

                                        foreach (var task in db.Tasks.Include(r => r.IdUserNavigation).Where(r => r.Deadline >= DateTime.UtcNow.Date && r.Deadline <= DateTime.UtcNow.Date.AddDays(7) && r.IdUser == user.Id))
                                        {
                                            Console.WriteLine($"Название задачи: {task.NameTask} - описание: {task.Description}  - срок выполнения: {task.Deadline}");
                                        }
                                        Console.WriteLine("\n------------------------------------------------------------\n");

                                        break;
                                    }
                                case 8:
                                    {
                                        Console.WriteLine("Задачи которые предстоит выполнить");

                                        foreach (var task in db.Tasks.Include(r => r.IdUserNavigation).Where(r => r.Deadline >= DateTime.UtcNow.Date && r.IdUser == user.Id))
                                        {
                                            Console.WriteLine($"Название задачи: {task.NameTask} - описание: {task.Description}  - срок выполнения: {task.Deadline}");
                                        }
                                        Console.WriteLine("\n------------------------------------------------------------\n");

                                        break;
                                    }
                                case 9:
                                    {
                                        Console.WriteLine("Задачи которые предстоит выполнить");

                                        foreach (var task in db.Tasks.Include(r => r.IdUserNavigation).Where(r => r.Deadline < DateTime.UtcNow.Date && r.IdUser == user.Id))
                                        {
                                            Console.WriteLine($"Название задачи: {task.NameTask} - описание: {task.Description}  - срок выполнения: {task.Deadline}");
                                        }
                                        Console.WriteLine("\n------------------------------------------------------------\n");

                                        break;
                                    }

                            }
                            Console.WriteLine("0 - Выход");
                            Console.WriteLine("1 - Посмотреть таблицу с задачами");
                            Console.WriteLine("2 - Добавить задачу");
                            Console.WriteLine("3 - Изменить задачу");
                            Console.WriteLine("4 - Удалить задачу");
                            Console.WriteLine("5 - Просмотреть задачи на сегодня");
                            Console.WriteLine("6 - Просмотреть задачи на завтра");
                            Console.WriteLine("7 - Просмотреть задачи на неделю");
                            Console.WriteLine("8 - Просмотреть задачи которые предстоит выполнить");
                            Console.WriteLine("9 - Просмотреть задачи у которых срок выполнения прошел");

                            choose = Convert.ToInt32(Console.ReadLine());
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не верный логин или пароль");
                    }
                    break;
                }
        }
    }
}