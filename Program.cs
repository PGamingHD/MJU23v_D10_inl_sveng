namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static List<SweEngGloss> dictionary;
        class SweEngGloss
        {
            public string word_swe, word_eng;
            public SweEngGloss(string word_swe, string word_eng)
            {
                this.word_swe = word_swe; this.word_eng = word_eng;
            }
            public SweEngGloss(string line)
            {
                string[] words = line.Split('|');
                this.word_swe = words[0]; this.word_eng = words[1];
            }
        }
        static void Main(string[] args)
        {
            string defaultFile = "..\\..\\..\\dict\\sweeng.lis";
            Console.WriteLine("Welcome to the dictionary app!");
            do
            {
                Console.Write("> ");
                string[] argument = Console.ReadLine().Split(); //TODO - Add NullReferenceException error handling when no argument is passed. (And other exceptions)
                string command = argument[0];
                if (command == "quit")
                {
                    Console.WriteLine("Goodbye!");
                }
                else if (command == "help")
                {
                    Console.WriteLine(" <Help is always required, here is some for you aswell>");
                    Console.WriteLine(" <() - Not required parameters | [] - Required parameters>");
                    Console.WriteLine("  load (file) -> Load a file or the default file.");
                    Console.WriteLine("  list -> List all words in the dictionary.");
                    Console.WriteLine("  new (swe) (eng) -> Add a new word into the dictionary.");
                    Console.WriteLine("  delete (swe) (eng) -> Delete a word from the dictionary.");
                    Console.WriteLine("  translate (swe) (eng) -> Translate a word from swedish to english.");
                    Console.WriteLine("  quit -> Quit the program and say goodbye.");
                    Console.WriteLine(" <Help is always required, here is some for you aswell>");
                }
                else if (command == "load")
                {
                    if(argument.Length == 2)
                    {
                        //TODO - Add error handler for System.IO.FileNotFoundException (And other exceptions) 
                        using (StreamReader reader = new StreamReader(argument[1]))
                        {
                            dictionary = new List<SweEngGloss>(); // Empty it!
                            string line = reader.ReadLine();
                            while (line != null)
                            {
                                SweEngGloss gloss = new SweEngGloss(line);
                                dictionary.Add(gloss);
                                line = reader.ReadLine();
                            }
                        }
                    }
                    else if(argument.Length == 1)
                    {
                        //TODO - Add error handler for System.IO.FileNotFoundException (And other exceptions) 
                        using (StreamReader reader = new StreamReader(defaultFile))
                        {
                            dictionary = new List<SweEngGloss>(); // Empty it!
                            string line = reader.ReadLine();
                            while (line != null)
                            {
                                SweEngGloss gloss = new SweEngGloss(line);
                                dictionary.Add(gloss);
                                line = reader.ReadLine();
                            }
                        }
                    }
                }
                else if (command == "list")
                {
                    //TODO - Add NullReferenceException error handler over here! (And other exceptions)
                    foreach (SweEngGloss gloss in dictionary)
                    {
                        Console.WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}");
                    }
                }
                else if (command == "new")
                {
                    if (argument.Length == 3)
                    {
                        dictionary.Add(new SweEngGloss(argument[1], argument[2]));
                    }
                    else if(argument.Length == 1)
                    {
                        Console.WriteLine("Write word in Swedish: ");
                        string swedishWord = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        string englishWord = Console.ReadLine();
                        dictionary.Add(new SweEngGloss(swedishWord, englishWord)); //TODO - Add error handler for NullReferenceException. (And other exceptions)
                    }
                }
                else if (command == "delete")
                {
                    if (argument.Length == 3)
                    {
                        int index = -1;
                        for (int i = 0; i < dictionary.Count; i++) //TODO - Add error handler for NullReferenceException if delete ran with no commands in list! (And other exceptions)
                        {
                            SweEngGloss gloss = dictionary[i];
                            if (gloss.word_swe == argument[1] && gloss.word_eng == argument[2])
                                index = i;
                        }
                        dictionary.RemoveAt(index);
                    }
                    else if (argument.Length == 1)
                    {
                        Console.WriteLine("Write word in Swedish: ");
                        string swedishWord = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        string englishWord = Console.ReadLine();
                        int index = -1;
                        for (int i = 0; i < dictionary.Count; i++) //TODO - Add error handler for NullReferenceException if delete ran with no commands in list! (And other exceptions)
                        {
                            SweEngGloss gloss = dictionary[i];
                            if (gloss.word_swe == swedishWord && gloss.word_eng == englishWord)
                                index = i;
                        }
                        dictionary.RemoveAt(index);
                    }
                }
                else if (command == "translate")
                {
                    if (argument.Length == 2)
                    {
                        foreach(SweEngGloss gloss in dictionary) //FIXME - Add better way to find the word? (List<T>.Find)?
                        {
                            if (gloss.word_swe == argument[1])
                                Console.WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}");
                            if (gloss.word_eng == argument[1])
                                Console.WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}");
                        }
                    }
                    else if (argument.Length == 1)
                    {
                        Console.WriteLine("Write word to be translated: ");
                        string swedishWord = Console.ReadLine();
                        foreach (SweEngGloss gloss in dictionary) //FIXME - Add better way to find the word? (List<T>.Find)?
                        {
                            if (gloss.word_swe == swedishWord)
                                Console.WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}");
                            if (gloss.word_eng == swedishWord)
                                Console.WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            }
            while (true);
        }
    }
}