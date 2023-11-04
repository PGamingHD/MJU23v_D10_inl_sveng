using System.Diagnostics.Tracing;

namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static readonly List<Command> commands = new();
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

            /* Command Registration */

            RegisterCommand("quit", (args) =>
            {
                Console.WriteLine("Goodbye, hope to see you soon!");

                Environment.Exit(0);
            }, new string[] {"exit", "stop"});

            RegisterCommand("clear", (args) =>
            {
                Console.Clear();
                Console.WriteLine("Welcome to PG Addresslist v0.0.1!");
            }, new string[] { "cls", "clr" });

            RegisterCommand("help", (args) =>
            {
                Console.WriteLine(" <Help is always required, here is some for you aswell>");
                Console.WriteLine("  <() - Not required parameters | [] - Required parameters>");
                Console.WriteLine("  load (file) -> Load a file or the default file.");
                Console.WriteLine("  list -> List all words in the dictionary.");
                Console.WriteLine("  new (swe) (eng) -> Add a new word into the dictionary.");
                Console.WriteLine("  delete (swe) (eng) -> Delete a word from the dictionary.");
                Console.WriteLine("  translate (swe) (eng) -> Translate a word from swedish to english.");
                Console.WriteLine("  quit -> Quit the program and say goodbye.");
                Console.WriteLine(" <Help is always required, here is some for you aswell>");
            }, new string[] { "hp" });

            RegisterCommand("load", (args) =>
            {
                if (args.Length >= 1)
                {
                    LoadGlossList(args[0]);
                }
                else if (args.Length == 0)
                {
                    LoadGlossList(defaultFile);
                }
                else
                {
                    Console.WriteLine("Not enough arguments has been passed.");
                }
            }, new string[] { "ld" });

            RegisterCommand("list", (args) =>
            {
                if (dictionary.Count == 0)
                {
                    Console.WriteLine("No words could be found in the dictionary, add some!");

                }
                else
                {
                    foreach (SweEngGloss gloss in dictionary)
                    {
                        Console.WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}");
                    }
                }
            }, new string[] { "ls" });

            RegisterCommand("new", (args) =>
            {
                if (args.Length >= 2)
                {
                    if (args[0] == null || args[1] == null)
                    {
                        Console.WriteLine("Could not find entered words, please try again.");
                        return;
                    }

                    dictionary.Add(new(args[0], args[1]));
                }
                else if (args.Length == 0)
                {
                    string? swedishWord = Input("Write word in Swedish: ");
                    string? englishWord = Input("Write word in English: ");

                    if (swedishWord == null || englishWord == null)
                    {
                        Console.WriteLine("Could not find entered words, please try again.");
                        return;
                    }

                    dictionary.Add(new(swedishWord, englishWord));
                }
                else
                {
                    Console.WriteLine("Not enough arguments has been passed.");
                }
            }, new string[] { "add" });

            RegisterCommand("delete", (args) =>
            {
                if (args.Length >= 2)
                {
                    DeleteGloss(args[0], args[1]);
                }
                else if (args.Length == 0)
                {
                    string? swedishWord = Input("Write word in Swedish: ");
                    string? englishWord = Input("Write word in English: ");

                    if (swedishWord == null || englishWord == null)
                    {
                        Console.WriteLine("Could not find entered words, please try again.");
                        return;
                    }

                    DeleteGloss(swedishWord, englishWord);
                }
                else
                {
                    Console.WriteLine("Not enough arguments has been passed.");
                }
            }, new string[] { "del", "remove" });

            RegisterCommand("translate", (args) =>
            {
                if (args.Length >= 1)
                {
                    TranslateGloss(args[0]);
                }
                else if (args.Length == 1)
                {
                    string? glossWord = Input("Write word to be translated: ");

                    TranslateGloss(glossWord);
                }
                else
                {
                    Console.WriteLine("Not enough arguments has been passed.");
                }
            }, new string[] { "trans" });

            /* Command Registration */

            while (true)
            {
                //Get the console input
                string? input = Input("Command > ");

                //If no input then continue the loop and ask for new command to be entered.
                if (input == null) continue;

                //Split up input into arguments that is passed through the command.
                string[] inputArgs = input.Split(' ');

                //Run command if found
                RunCommand(inputArgs);
            }
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
            } catch (Exception error)
            {
                Console.WriteLine("Error: " + error.Message);
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
                using (StreamReader reader = new StreamReader(file))
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

        static public void RegisterCommand(string name, CommandAction action, string[] alias)
        {
            commands.Add(new(name, alias, action));
        }

        static public void RunCommand(string[] inputArgs)
        {
            if (inputArgs.Length == 0) return;

            string commandName = inputArgs[0];

            Command? foundcmd = commands.Find(cmd => cmd.GetCommand().ToLower() == commandName.ToLower());

            if (foundcmd != null)
            {
                // Pass the arguments (excluding the command name) to the command
                foundcmd.ExecuteCommand(inputArgs[1..]);
                return;
            }

            foreach (Command cmd in commands)
            {
                string[] aliases = cmd.GetAlias();

                foreach (string alias in aliases)
                {
                    if (alias.ToLower() == commandName.ToLower())
                    {
                        foundcmd = cmd;
                        break;
                    }
                }

                if (foundcmd != null) break;
            }

            if (foundcmd != null)
            {
                // Pass the arguments (excluding the command name) to the command
                foundcmd.ExecuteCommand(inputArgs[1..]);
                return;
            }

            Console.WriteLine("Command " + "'" + commandName + "'" + " is not recognized as an internal or external command. (use 'help')");
        }
    }
}