using System.Net.Http.Json;

var client = new HttpClient();

Console.WriteLine("Расписание вуза");
Console.Write("Введите группу (например ИТ-21): ");
string group = Console.ReadLine()!.Trim();

Console.Write("Введите дату (дд.мм.гггг, например 11.12.2025): ");
if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
{
    Console.WriteLine("Неверная дата"); return;
}

try
{
    var lessons = await client.GetFromJsonAsync<List<Lesson>>(
        $"https://localhost:7233/api/schedule?group={group}&date={date:yyyy-MM-dd}");

    Console.WriteLine($"\nРасписание для группы {group} на {date:dd MMMM yyyy} (dddd):");
    Console.WriteLine("".PadRight(50, '═'));

    if (lessons == null || lessons.Count == 0)
    {
        Console.WriteLine("    Пар нет — можно спать =)");
    }
    else
    {
        foreach (var l in lessons.OrderBy(x => x.Time))
        {
            Console.WriteLine($"{l.Time} │ {l.Subject}");
            Console.WriteLine($"{" ".PadRight(12)}└ {l.Teacher}");
            Console.WriteLine();
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}

Console.WriteLine("\nНажмите любую клавишу…");
Console.ReadKey();

// Класс остаётся тем же
public class Lesson
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public DateTime Date { get; set; }
    public string Time { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Teacher { get; set; } = null!;
}