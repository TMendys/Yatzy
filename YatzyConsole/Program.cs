using static System.Environment;
using static System.Console;
using YatzyLibrary;
using YatzyConsole;

bool play = true;

while (play)
{
    Clear();
    WriteLine("Spela Yatsy!" + NewLine);
    List<Player> players = NumberOfPlayers();
    _ = new Game(players);

    Clear();
    WriteLine("Vill du spela igen? (J/N)");
    ConsoleKeyInfo input = ReadKey();
    if (input.Key != ConsoleKey.J)
        play = false;
}

static List<Player> NumberOfPlayers()
{
    List<Player> players = new();
    int numberOfPlayers;
    bool success;

    do
    {
        WriteLine("Ange hur många spelare (1-5):  ");
        success = int.TryParse(ReadLine(), out numberOfPlayers);

        if (numberOfPlayers > 5 && numberOfPlayers < 1)
        {
            Clear();
            WriteLine("Du måste skriva in ett nummer mellan 1-5\n");
            success = false;
        }
    } while (!success);

    for (int i = 1; i <= numberOfPlayers; i++)
    {
        string? name;
        do
        {
            Clear();
            WriteLine($"Spelare {i} skriv in ditt namn:  ");
            name = ReadLine();
            if (name?.Length > 9)
            {
                name = name[0..9];
            }
        } while (string.IsNullOrWhiteSpace(name));

        Player player = new(name);
        players.Add(player);
    }

    return players;
}