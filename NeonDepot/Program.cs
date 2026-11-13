string[] names = ["Вода", "Пайки", "Аптечки", "Батареи", "Фильтры"];
int[] quantities = [18, 9, 3, 12, 5];
int[] minimums = [10, 8, 5, 6, 4];

void PrintMenu()
{
	Console.Write("========= NEON DEPOT =========\n1. Показать весь склад\n2. Найти товар\n3. Принять поставку\n4. Списать товар\n5. Статистика склада\n6. Товары ниже минимума\n0. Выход\nВыберите действие: ");
}

void PrintInventory()
{
	Console.WriteLine(
		$"{"№",-3} | {"Товар",-12} | {"Количество",-12} | {"Минимум",-10} | Статус"
	);

	Console.WriteLine(new string('-', 65));

	for (int i = 0; i < names.Length; i++)
	{
		string status = quantities[i] <= minimums[i]
			? "ПОПОЛНИТЬ"
			: "НОРМА";

		string quantity = $"{quantities[i]} шт.";
		string minimum = $"мин. {minimums[i]}";

		Console.WriteLine(
			$"{i,-3} | {names[i],-12} | {quantity,-12} | {minimum,-10} | {status}"
		);
	}
}

int FindProductIndex(string query)
{
	query = query.Trim();
	for (int i = 0; i < names.Length; i++)
	{
		bool founded = string.Equals(
			names[i], query,
			StringComparison.OrdinalIgnoreCase);
		if (founded) return i;
	}
	return -1;
}

bool ChangeStock(int index, int delta)
{
	if (index < 0 || index >= quantities.Length || quantities[index] + delta < 0)
	{
		return false;
	}
	quantities[index] += delta;
	return true;
}

int SumStock()
{
	return quantities.Sum();
}

int FindMaxIndex()
{
	int max = quantities.Max();
	for (int i = 0; i < quantities.Length; i++)
	{
		if (quantities[i] == max) return i;
	}
	return -1;
}

int CountBelowMinimum()
{
	int counter = 0;
	for (int i = 0; i < quantities.Length; i++)
	{
		if (quantities[i] < minimums[i]) counter++;
	}

	return counter;
}

void PrintDeficits()
{
	for (int i = 0; i < quantities.Length; i++)
	{
		if (quantities[i] < minimums[i])
		{
			Console.WriteLine($"{names[i]} — не хватает {minimums[i] - quantities[i]} шт.");
		}
	}
}

while (true)
{
	PrintMenu();
	string? input = Console.ReadLine();
	if (!int.TryParse(input, out int choice))
	{
		Console.WriteLine("Нужно ввести число!");
		continue;
	}

	if (choice == 0)
	{
		Console.WriteLine("Выход из программы...");
		break;
	}
	else if (choice == 1)
	{
		PrintInventory();
	}
	else if (choice == 2)
	{
		Console.Write("Название товара для поиска: ");
		string? query = Console.ReadLine();
		if (String.IsNullOrEmpty(query))
		{
			Console.WriteLine("Название товара не может быть пустым.");
			continue;
		}
		int index = FindProductIndex(query);
		if (index == -1)
		{
			Console.WriteLine("Товар не найден.");
			continue;
		}
		Console.WriteLine($"Товар найден, индекс: {index}");
		Console.WriteLine($"Имя: {names[index]}");
		Console.WriteLine($"Остаток: {quantities[index]}");
	}
	else if (choice == 3)
	{
		Console.Write($"Индекс товара для пополнения: ");
		string? indexRaw = Console.ReadLine();
		if (!int.TryParse(indexRaw, out int index))
		{
			Console.WriteLine("Индекс товара должен быть положительным числом.");
			continue;
		}
		Console.Write("Сколько товара принять: ");
		string? deltaRaw = Console.ReadLine();
		if (!int.TryParse(deltaRaw, out int delta) || delta < 1)
		{
			Console.WriteLine("Количество товара должно быть положительным числом.");
			continue;
		}

		if (ChangeStock(index, delta))
		{
			Console.WriteLine($"Количество товара успешно пополнено на {delta} шт. Новое количество {quantities[index]} шт.");
		}
		else
		{
			Console.WriteLine("Не удалось пополнить товар. Неверный индекс.");
		}
	}
	else if (choice == 4)
	{
		Console.Write($"Индекс товара для списания: ");
		string? indexRaw = Console.ReadLine();
		if (!int.TryParse(indexRaw, out int index))
		{
			Console.WriteLine("Индекс товара должен быть положительным числом.");
			continue;
		}
		Console.Write("Сколько товара списать: ");
		string? deltaRaw = Console.ReadLine();
		if (!int.TryParse(deltaRaw, out int delta) || delta < 1)
		{
			Console.WriteLine("Количество товара для списания должно быть положительным числом.");
			continue;
		}

		if (ChangeStock(index, -delta))
		{
			Console.WriteLine($"Успешно списано {delta} шт. Новое количество {quantities[index]} шт.");
		}
		else
		{
			Console.WriteLine($"Не удалось списать товар. Неверный индекс или {delta} единиц товара нет.");
		}
	}
	else if (choice == 5)
	{
		Console.WriteLine($"Сумма всех товаров: {SumStock()} шт.");
		Console.WriteLine($"Самый крупный запас (индекс): {FindMaxIndex()}");
		Console.WriteLine($"Дефицитных товаров: {CountBelowMinimum()}");
	}
	else if (choice == 6)
	{
		PrintDeficits();
	}
	else
	{
		Console.WriteLine("Действие не найдено.");
	}
}