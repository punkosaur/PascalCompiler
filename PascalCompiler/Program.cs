using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler
{
    class Program
    {

        public static void Main()
        {
            //  "D:\\Visual Studio projects\\PascalCompiler\\PascalCompiler\\Input.txt"
            // string LL = "D:\\Visual Studio projects\\PascalCompiler\\PascalCompiler\\Lexer_listing.txt";
            //InOutModule IOM = new InOutModule("D:\\Visual Studio projects\\PascalCompiler\\PascalCompiler\\Input.txt");
            //while (true)
            //{
            //    Litera lit = IOM.GetNext();
            //  //  Lexem

            //    Console.WriteLine(lit.Literavalue + " " + lit.LiteraPosition.lineNumber + " " + lit.LiteraPosition.charNumber);
            //}

            //Lexer lex = new Lexer("D:\\Visual Studio projects\\PascalCompiler\\PascalCompiler\\Input.txt");
            //lex.GoScan();

            SyntaxAnalizer syn = new SyntaxAnalizer("D:\\Visual Studio projects\\PascalCompiler\\PascalCompiler\\Input.txt");
            syn.GoScan();

            ///////////////////////////////////////////////////
            ///////////////////////////////////////////////////
            ///////////////////////////////////////////////////
            // https://t.me/papappapapapspds/40 голосуйте за бархатные тяги
            ///////////////////////////////////////////////////
            ///////////////////////////////////////////////////
            ///////////////////////////////////////////////////
        }


    }
}
