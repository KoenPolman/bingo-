namespace Polman_DevFile08_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program start = new Program();

            start.start();
        }
        void start()
        {//er worden hier drie verschillende arrays bij gehouden
            int[,] speelGridNum = new int[5, 5]; //een voor de nummers op de kaart 
            bool[,] speelGridPlayerCheck = new bool[5, 5]; //een voor de gestemplede vakjes van de speler
            bool[,] speelGridGameCheck = new bool[5, 5]; // en een voor de nummers die daadwerkelijk gerold zijn
            bool quit = false;
            while (!quit)
            {
                int bingMin = 1;
                int bingMax = 16; //parameters voor nummer generatie
                bool bingoGeroepen = false;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Random randomNumber0 = new Random();//nummer generator voor op de bingo kaart
                        speelGridNum[i, j] = randomNumber0.Next(bingMin, bingMax); // herhaalt alleen nog soms nummers :(
                        speelGridPlayerCheck[i, j] = false; //hier worden ook de bool arrays voor speler en spel op false gezet
                        speelGridGameCheck[i, j] = false;
                    }
                    bingMax = bingMax + 15;
                    bingMin = bingMin + 15;
                }
                speelGridNum[2, 2] = 0; //free-space word voor de speler afgestempeld
                speelGridPlayerCheck[2, 2] = true;
                speelGridGameCheck[2, 2] = true;
                Console.WriteLine("Druk op enter om te beginnen.");
                Console.ReadKey();
                Console.Clear();
                while (!bingoGeroepen)
                {
                    bool isParsable = false;
                    int processedInt = 0;
                    Random randomNumber0 = new Random(); //genereert een nummer om afgestempeld te worden
                    int bingNum = randomNumber0.Next(1, 76); //kan wel dezelfde nummers meerdere keeren rollen :(
                    for (int x = 0; x < 5; x++)
                    { //controlleert of het gerolde nummer op de kaart staat en houd bij dat het nummer op die gerold is
                        for (int y = 0; y < 5; y++)
                        {
                            if (speelGridNum[x, y] == bingNum)
                            {
                                speelGridGameCheck[x, y] = true;
                            }
                        }
                    }
                    //laat het nummer zien met mooie opmaak
                    Console.WriteLine("Het gerolde nummer is:");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(bingNum);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Thread.Sleep(3000);
                    Console.Clear();
                    BoardPrint(speelGridPlayerCheck, speelGridGameCheck, speelGridNum);
                    Console.WriteLine("Om een cijfer af te stempelen typ een nummer in corresponderend met het grid.\n\n0  1  2  3  4  \n5  6  7  8  9\n10 11 12 13 14 \n15 16 17 18 19 \n20 21 22 23 24\n\nVoer in 25 om te nog een getal te rollen.\nVoer in 26 als je bingo hebt");
                    while (!isParsable)//input systeem met 0 t/m 24 voor stempels zetten, 25 voor overslaan en 26 voor bingo roepen
                    {
                        string gebruikerInput = Console.ReadLine();
                        Console.Clear();
                        isParsable = Int32.TryParse(gebruikerInput, out processedInt);
                        if (!isParsable || (processedInt <= -1 && processedInt >= 27))
                        {
                            Console.WriteLine("Geef aub een geldige inpuT");
                            isParsable = false;
                        }
                    }
                    if (processedInt >= 0 && processedInt <= 24)
                    {
                        int inputX = (processedInt % 5); //berekent stempel locatie en markeert als true
                        int inputY = processedInt / 5;
                        speelGridPlayerCheck[inputX, inputY] = true;
                    }
                    else if (processedInt == 26)
                    {
                        bingoGeroepen = true;
                    }
                    BoardPrint(speelGridPlayerCheck, speelGridGameCheck, speelGridNum);
                    Thread.Sleep(500);
                }
                if (Winchecker(speelGridPlayerCheck, speelGridGameCheck)) //controlleert of de speler daadwerkelijk bingo heeft
                {
                    BoardPrint(speelGridPlayerCheck, speelGridGameCheck, speelGridNum);
                    Console.WriteLine("Je hebt bingo!!1!!!11!\nDruk op enter om nog een keer te spelen.\nDruk op q om het spel te verlaten");
                }
                else if (bingoGeroepen)
                {
                    Console.WriteLine("Je hebt geen bingo. :(\nDruk op enter om nog een keer te spelen.\nDruk op q om het spel te verlaten");
                }
                string userInput = Console.ReadLine(); //hier kan de speler het spel verlaten
                Console.Clear();
                if (userInput == "q")
                {
                    quit = true;
                }
            }
        }
        /// <summary>
        /// Hier word gecontrolleerd of de speler bingo heeft als die het vermeld. 
        /// </summary>
        /// <param name="boardPlayer"></param>
        /// <param name="boardTrue"></param>
        /// <returns></returns>
        bool Winchecker(bool[,] boardPlayer, bool[,] boardTrue)
        {
            bool[,] boardWinCheck = new bool[5, 5];
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)//combineert de gestempelde vakken van de speler en gerolde vakken om te controlleren
                {
                    if (boardPlayer[x, y] && boardTrue[x, y])
                    {
                        boardWinCheck[x, y] = true;
                    }
                }
            }
            int inARowCounter = 0;
            for (int x = 0; x < 5; x++)//horizontaale check
            {
                for (int y = 0; y < 5; y++)
                {
                    if (boardWinCheck[x, y])
                    {
                        inARowCounter++;
                        if (inARowCounter == 5)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        inARowCounter = 0;
                    }
                }
            }
            for (int y = 0; y < 5; y++)//verticaale check
            {
                for (int x = 0; x < 5; x++)
                {
                    if (boardWinCheck[x, y])
                    {
                        inARowCounter++;
                        if (inARowCounter == 5)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        inARowCounter = 0;
                    }
                }
            }
            int z = 0;
            for (int y = 0; y < 5; y++)//negatief diagonale check
            {
                if (!boardWinCheck[z, y])
                {
                    break;
                }
                else
                {
                    inARowCounter++;
                    if (inARowCounter == 5)
                    {
                        return true;
                    }
                }
                z++;
            }
            z = 4;
            for (int y = 0; y < 5; y++)//positief diagonale check
            {
                if (!boardWinCheck[z, y])
                {
                    break;
                }
                else
                {
                    inARowCounter++;
                    if (inARowCounter == 5)
                    {
                        return true;
                    }
                }
                z--;
            }
            return false;
        }
        /// <summary>
        /// Dit print de bingo kaart uit en kleurt de vakjes die de speler heeft "gestempelt" rood.
        /// </summary>
        /// <param name="doorSpelerGestempled"></param>
        /// <param name="num"></param>
        void BoardPrint(bool[,] doorSpelerGestempled, bool[,] daadwerkelijkGerold, int[,] num)
        {
            Console.Clear();
            for (int x = 0; x < 5; x++)
            {
                Console.Write("\n-----------------------------------------\n|");
                for (int y = 0; y < 5; y++)
                {
                    if (doorSpelerGestempled[y, x]/* && daadwerkelijkGerold[y, x]*/)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    //voor test eind doelen had ik hier het zo gemaakt dat gerolde getallen groen kleurde,
                    //afgestempelde maar niet gerolde blauw kleurde en gerolde en gestempelde rood kleuren.
                    else if (daadwerkelijkGerold[y, x])
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    else if (doorSpelerGestempled[y, x])
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                    }
                    Console.Write(num[y, x]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("\t|");
                }
            }
            Console.WriteLine("\n-----------------------------------------");
        }
    }
}
