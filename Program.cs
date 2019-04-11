using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Globalization;
using System.Linq;

/*
*
*
* 08-04-2019
* Program developed by Alejandro López, from Venezuela, to continue in the interview process at Ringba's
* Junior developer postion
*
*
*/


namespace myApp
{
    static class Program
    {
        //
        // Objects used
        //
        public class Letter
        {
            public char letter;
            public int count;
        }

        public class Word
        {
            public string word;
            public int count;
        }

        public class Prefix
        {
            public string prefix;
            public int count;
            public int length;
        }

        //
        // Class for the main menu thats going to show to the user when the program starts.
        //
        public class Menu 
        {

            //
            // Method that handles the menu
            //
            public static void display() 
            {
                //
                // Menu options
                //
                Console.WriteLine("-----------------------------");
                Console.WriteLine("[1] Download file");
                Console.WriteLine("[2] Read file statistics");
                Console.WriteLine("[0] Exit");
                Console.WriteLine("-----------------------------");
                var answer = Console.ReadLine();

                switch (answer)
                {
                    case "1":
                        File_.download();
                        Menu.display();
                        break;
                    case "2":
                        File_.statistics();
                        Menu.display();
                        break;
                    case "0":
                        Console.Clear();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Please choose a valid option!");
                        Menu.display();
                        break;
                }

            }
        }
        
        //
        // Class to for the file thats going to be downloaded from the URL
        //
        public class File_
        {
            static string filePath = Directory.GetCurrentDirectory();
            static string fileName = "output.txt";
            static string completeFilePath = filePath + "\\" + fileName;

            //
            // Method that handles the download of the file, menu option [1]
            //
            public static void download() 
            {
                //
                // Only allows downloads when the text file its no present on the designated path
                //
                if(File.Exists(fileName)) 
                {
                    Console.Clear();
                    Console.WriteLine("File is already downloaded!");
                    Menu.display();
                } else
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile("http://ringba-test-html.s3-website-us-west-1.amazonaws.com/TestQuestions/output.txt", @"" + completeFilePath);
                }
            }

            //
            // Method that handles all the statistics shown on the menu option [2]
            //
            public static void statistics() 
            {
                string fileText = File.ReadAllText(@"" + completeFilePath);
                char[] fileTextArray = fileText.ToCharArray();

                List<Letter> lettersList = new List<Letter>
                {
                    new Letter(){letter='a'}, new Letter(){letter='b'},
                    new Letter(){letter='c'}, new Letter(){letter='d'},
                    new Letter(){letter='e'}, new Letter(){letter='f'},
                    new Letter(){letter='g'}, new Letter(){letter='h'},
                    new Letter(){letter='i'}, new Letter(){letter='j'},
                    new Letter(){letter='k'}, new Letter(){letter='l'},
                    new Letter(){letter='m'}, new Letter(){letter='n'},
                    new Letter(){letter='o'}, new Letter(){letter='p'},
                    new Letter(){letter='q'}, new Letter(){letter='r'},
                    new Letter(){letter='s'}, new Letter(){letter='t'},
                    new Letter(){letter='u'}, new Letter(){letter='v'},
                    new Letter(){letter='w'}, new Letter(){letter='x'},
                    new Letter(){letter='y'}, new Letter(){letter='z'}
                };
                List<Word> wordsList = new List<Word>{};
                //
                // Assuming these are "all" english prefixes
                List<Prefix> prefixList = new List<Prefix>{
                    new Prefix(){prefix="a"},       new Prefix(){prefix="an"},
                    new Prefix(){prefix="ab"},      new Prefix(){prefix="ad"},
                    new Prefix(){prefix="ac"},      new Prefix(){prefix="as"},
                    new Prefix(){prefix="ante"},    new Prefix(){prefix="anti"},
                    new Prefix(){prefix="auto"},    new Prefix(){prefix="ben"},
                    new Prefix(){prefix="bi"},      new Prefix(){prefix="circum"},
                    new Prefix(){prefix="co"},      new Prefix(){prefix="com"},
                    new Prefix(){prefix="con"},     new Prefix(){prefix="contra"},
                    new Prefix(){prefix="counter"}, new Prefix(){prefix="de"},
                    new Prefix(){prefix="di"},      new Prefix(){prefix="dif"}, 
                    new Prefix(){prefix="dis"},     new Prefix(){prefix="en"}, 
                    new Prefix(){prefix="em"},      new Prefix(){prefix="epi"},
                    new Prefix(){prefix="eu"},      new Prefix(){prefix="ex"}, 
                    new Prefix(){prefix="exo"},     new Prefix(){prefix="ecto"}, 
                    new Prefix(){prefix="extra"},   new Prefix(){prefix="extro"},
                    new Prefix(){prefix="fore"},    new Prefix(){prefix="hemi"},
                    new Prefix(){prefix="hyper"},   new Prefix(){prefix="hypo"},
                    new Prefix(){prefix="il"},      new Prefix(){prefix="im"},
                    new Prefix(){prefix="in"},      new Prefix(){prefix="infra"},
                    new Prefix(){prefix="ir"},      new Prefix(){prefix="inter"},
                    new Prefix(){prefix="intra"},   new Prefix(){prefix="macro"}, 
                    new Prefix(){prefix="mal"},     new Prefix(){prefix="micro"}, 
                    new Prefix(){prefix="mis"},     new Prefix(){prefix="mono"}, 
                    new Prefix(){prefix="multi"},   new Prefix(){prefix="non"}, 
                    new Prefix(){prefix="ob"},      new Prefix(){prefix="o"}, 
                    new Prefix(){prefix="oc"},      new Prefix(){prefix="op"}, 
                    new Prefix(){prefix="omni"},    new Prefix(){prefix="over"},
                    new Prefix(){prefix="para"},    new Prefix(){prefix="peri"},
                    new Prefix(){prefix="poly"},    new Prefix(){prefix="post"},
                    new Prefix(){prefix="pre"},     new Prefix(){prefix="pro"},
                    new Prefix(){prefix="quad"},    new Prefix(){prefix="re"},
                    new Prefix(){prefix="semi"},    new Prefix(){prefix="sub"},
                    new Prefix(){prefix="sup"},     new Prefix(){prefix="sus"},
                    new Prefix(){prefix="super"},   new Prefix(){prefix="supra"},
                    new Prefix(){prefix="sym"},     new Prefix(){prefix="syn"},
                    new Prefix(){prefix="trans"},   new Prefix(){prefix="therm"},
                    new Prefix(){prefix="tri"},     new Prefix(){prefix="ultra"},
                    new Prefix(){prefix="un"},      new Prefix(){prefix="uni"}
                };

                //
                // Ive ordered the prefix list so i can always detect the longer prefix first on every word.
                // If we dont do that, the program would not identify when (for example) the 
                // prefix is *ab* instead of *a*
                //
                prefixList = prefixList.OrderByDescending(o=>o.prefix.Length).ToList();

                int i;
                int capitalLettersCount;
                int prefixMaxCount;
                
                String decodedWord = "";

                Prefix maxRepPrefix = new Prefix();

                i=0;
                capitalLettersCount = 0;
                while(i< fileTextArray.Length)
                {
                    // STATISTIC # 1
                    //
                    // Method that counts how many times a letter is repeated in the file
                    //
                    foreach(Letter letter in lettersList)
                    {
                        if(fileTextArray[i] == letter.letter || fileTextArray[i] == Char.ToUpper(letter.letter) )
                        {
                            letter.count++;
                        }
                    }

                    // STATISTIC # 2
                    //
                    // Method that counts how many capitalized letters are in the file
                    //
                    if(Char.IsUpper(fileTextArray[i]))
                    {
                        capitalLettersCount++;
                    }

                    i++;
                }
                // Show required data STATISTICS # 1 # 2
                Console.WriteLine("-----------------------------");
                foreach(Letter letter in lettersList)
                {
                    Console.WriteLine("Letter: " + letter.letter + " is repeated " + letter.count + " times");
                }
                Console.WriteLine("-----------------------------");
                Console.WriteLine("-----------------------------");
                Console.WriteLine("There are " + capitalLettersCount + " capitalized letters.");
                Console.WriteLine("-----------------------------");

                //
                // Method that finds all words on the file
                //
                i=0;
                while(i< fileTextArray.Length)
                {
                    if(char.IsUpper(fileTextArray[i]))
                    {
                        decodedWord = fileTextArray[i].ToString();
                        i++;
                    } else
                    {
                        decodedWord = decodedWord + fileTextArray[i].ToString();
                        i++;
                        if (i < fileTextArray.Length)
                        {
                            if (char.IsUpper(fileTextArray[i]))
                            {
                                wordsList.Add(new Word() {word=decodedWord.ToLower()});
                                decodedWord="";
                            }
                        } else
                        {
                            wordsList.Add(new Word() {word=decodedWord.ToLower()});
                            decodedWord="";
                        }
                    }
                }

                //
                // Method that counts how many times a word is repeated
                //
                foreach(Word word1 in wordsList)
                {
                    foreach(Word word2 in wordsList)
                    {
                        if (word1.word == word2.word)
                        {
                            word1.count++;
                        }

                    }
                }

                //
                // Method that fills the length property on the prefix object
                //
                foreach(Prefix prefix in prefixList)
                {
                    prefix.length = prefix.prefix.Length;
                }

                // STATISTIC # 3
                //
                // Method that identifies the most repeated word
                //
                Word maxRepWord = new Word();
                foreach(Word word in wordsList)
                {
                    if (maxRepWord.count < word.count)
                    {
                        maxRepWord = word;
                    }
                }
                Console.WriteLine("-----------------------------");
                Console.WriteLine("The most repeated word is: " + maxRepWord.word + " with " + maxRepWord.count + " times repeated");
                Console.WriteLine("-----------------------------");

                //
                // Method that detects all the prefixes on each word
                //
                foreach(Word word in wordsList)
                {
                    // Validate the word size is greater than 2 characters
                    //  Note that the word "in" does not contain the prefix "in" 
                    //  but the word "indirectly" does.
                    if(word.word.Length > 2)
                    {
                        foreach(Prefix prefix in prefixList)
                        {
/*                             if(prefix.prefix.Length == 2)
                            { */
                                // Validate the substring found in the word is a prefix
                                if (prefix.prefix.Length < word.word.Length)
                                {
                                    if(word.word.Substring(0, prefix.prefix.Length) == prefix.prefix)
                                    {
                                        prefix.count++;
                                        break;
                                    }
                                }
                           /*  } */
                        }
                    }
                }

                // STATISTIC # 4
                //
                // Method that identifies the most repeated two character prefix
                //
                prefixMaxCount = 0;
                foreach(Prefix prefix in prefixList)
                {
                    if(prefix.prefix.Length == 2)
                    {
                        if (prefixMaxCount < prefix.count)
                        {
                            maxRepPrefix.prefix = prefix.prefix;
                            maxRepPrefix.count = prefix.count;
                            prefixMaxCount = maxRepPrefix.count;
                        }
                    }

                }
                Console.WriteLine("-----------------------------");
                Console.WriteLine("The most repeated two character prefix is: " + maxRepPrefix.prefix + ", with " + maxRepPrefix.count + " times repeated");
                Console.WriteLine("-----------------------------");

                // BONUS STATISTIC # 5
                //
                // Method that identifies the most common and complex prefix of any length greater than 1, 
                // the number of times it has been seen, and the words that contain the prefix
                //
                Prefix complexPrefix = new Prefix();
                foreach(Prefix prefix in prefixList)
                {
                    if(complexPrefix.length < prefix.length && complexPrefix.count < prefix.count)
                    {
                        complexPrefix = prefix;
                    } else if (complexPrefix.length == prefix.length )
                    {
                        if (complexPrefix.count < prefix.count)
                        {
                            complexPrefix = prefix;
                        }
                    }
                }
                Console.WriteLine("-----------------------------");
                Console.WriteLine("The most repeated and complex prefix is: " + complexPrefix.prefix + ", with " + complexPrefix.count + " times repeated");
                Console.WriteLine("-----------------------------");
            }
        }

        //
        // Main class, heres where the program starts
        //
        static void Main(string[] args)
        {
            Menu.display();
        }
    }
}
