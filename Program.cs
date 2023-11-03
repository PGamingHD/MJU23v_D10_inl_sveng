using System.Diagnostics.Tracing;

namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static List<SweEngGloss> dictionary = new List<SweEngGloss>();
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
                string?[] argument;
                try
                {
                    argument = Console.ReadLine().Split();
                } catch (NullReferenceException) {
                    Console.WriteLine("Unknown or no command found, try again.");
                    continue;
                } catch (Exception error)
                {
                    Console.WriteLine("Error: " + error.Message);
                    continue;
                }

                string command = argument[0];
                if (command == "quit")
                {
                    Console.WriteLine("Goodbye!");
                    break;
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
                    if(argument.Length >= 2)
                    {
                        LoadGlossList(argument[1]);
                    }
                    else if(argument.Length == 1)
                    {
                        LoadGlossList(defaultFile);
                    }
                    else
                    {
                        Console.WriteLine("Not enough arguments has been passed.");
                    }
                }
                else if (command == "list")
                {
                    if (dictionary.Count == 0)
                    {
                        Console.WriteLine("No words could be found in the dictionary, add some!");

                    } else
                    {
                        foreach (SweEngGloss gloss in dictionary)
                        {
                            Console.WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}");
                        }
                    }
                }
                else if (command == "new")
                {
                    if (argument.Length >= 3)
                    {
                        if (argument[1] == null || argument[2] == null)
                        {
                            Console.WriteLine("Could not find entered words, please try again.");
                            continue;
                        }
                        dictionary.Add(new(argument[1], argument[2]));
                    }
                    else if(argument.Length == 1)
                    {
                        string? swedishWord = Input("Write word in Swedish: ");
                        string? englishWord = Input("Write word in English: ");

                        if (swedishWord == null || englishWord == null)
                        {
                            Console.WriteLine("Could not find entered words, please try again.");
                            continue;
                        }

                        dictionary.Add(new(swedishWord, englishWord));
                    }
                    else
                    {
                        Console.WriteLine("Not enough arguments has been passed.");
                    }
                }
                else if (command == "delete")
                {
                    if (argument.Length >= 3)
                    {
                        DeleteGloss(argument[1], argument[2]);
                    }
                    else if (argument.Length == 1)
                    {
                        string? swedishWord = Input("Write word in Swedish: ");
                        string? englishWord = Input("Write word in English: ");

                        if (swedishWord == null || englishWord == null)
                        {
                            Console.WriteLine("Could not find entered words, please try again.");
                            continue;
                        }

                        DeleteGloss(swedishWord, englishWord);
                    } else
                    {
                        Console.WriteLine("Not enough arguments has been passed.");
                    }
                }
                else if (command == "translate")
                {
                    if (argument.Length >= 2)
                    {
                        TranslateGloss(argument[1]);
                    }
                    else if (argument.Length == 1)
                    {
                        string? glossWord = Input("Write word to be translated: ");

                        TranslateGloss(glossWord);
                    }
                    else
                    {
                        Console.WriteLine("Not enough arguments has been passed.");
                    }
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            }
            while (true);
        }

        public static string? Input(string addVariable)
        {
            Console.Write(addVariable);
            try
            {
                return Console.ReadLine();
            } catch (OutOfMemoryException)
            {
                Console.WriteLine("Not enough memory left to readline, please restart application!");
                return null;
            }
        }

        public static void TranslateGloss(string glossWord)
        {
            SweEngGloss? foundWord = dictionary.Find(gloss => gloss.word_swe == glossWord || gloss.word_eng == glossWord);

            if (foundWord != null && foundWord.word_swe == glossWord)
            {
                Console.WriteLine($"English for {foundWord.word_swe} is {foundWord.word_eng}");
            }
            else if (foundWord != null && foundWord.word_eng == glossWord)
            {
                Console.WriteLine($"Swedish for {foundWord.word_eng} is {foundWord.word_swe}");
            }
            else
            {
                Console.WriteLine("No word could be found with that translation.");
            }
        }

        public static void DeleteGloss(string swedishWord, string englishWord)
        {
            SweEngGloss? foundWord = dictionary.Find(gloss => gloss.word_swe == swedishWord && gloss.word_eng == englishWord);

            if (foundWord == null)
            {
                Console.WriteLine("No word could be found to delete, please try again.");
                return;
            }

            dictionary.Remove(foundWord);

            Console.WriteLine("Successfully deleted word from the dictionary.");
        }

        public static void LoadGlossList(string file)
        {
            try
            {
                using (StreamReader reader = new StreamReader(file)) //FIXME - Fix error handling for no file found!
                {
                    dictionary = new List<SweEngGloss>(); // Empty it!
                    string? line = reader.ReadLine();
                    while (line != null)
                    {
                        SweEngGloss gloss = new SweEngGloss(line);
                        dictionary.Add(gloss);
                        line = reader.ReadLine();
                    }
                }
            } catch (FileNotFoundException)
            {
                Console.WriteLine($"Could not find file '{file}'");
            } catch (Exception error)
            {
                Console.WriteLine("Error: " + error.Message);
            }
        }
    }
}