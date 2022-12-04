using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PascalCompiler
{

    internal class Lexer
    {
        private string Filepath;
        private InOutModule IOM;
        public Lexer(string filepath)
        {
            Filepath = filepath;
            IOM = new InOutModule(filepath);
        }
        public Lexer()
        { }
        public TokenType GetLexemaType(Litera lit)
        {
            switch (lit.Literavalue)
            {
                case ("+" or
                    "-" or
                    "<" or
                    ">" or
                    "<=" or
                    ">=" or
                    "<>" or
                    "=" or
                    ":=" or
                    ";" or
                    ".." or
                    "*" or
                    "/" or
                    "mod" or
                    "div" or
                    "(" or
                    ")" or
                    "{" or
                    "}"):
                return TokenType.TypeOper;

                case ("begin" or
                    "end" or
                    "var" or
                    "boolean" or
                    "integer" or
                    "real" or
                    "string"):
                return TokenType.TypeKeyWord;

                case ("True" or
                    "False"):
                    return TokenType.TypeConst;

                default:
                    if (lit.Literavalue[0] == '"' && lit.Literavalue[lit.Literavalue.Length - 1] == '"' && lit.Literavalue.Length > 1) return TokenType.TypeConst;
                    else if (lit.Literavalue[0] == '-' || (lit.Literavalue[0] >= '0' && lit.Literavalue[0] <= '9'))
                    {
                        int i = 1;
                        while (i < lit.Literavalue.Length && lit.Literavalue[i] != '.')
                        {
                            if (lit.Literavalue[i] < '0' || lit.Literavalue[i] > '9') return TokenType.None;
                            i++;
                        }

                        if (i < lit.Literavalue.Length)
                        {
                            if (lit.Literavalue[i] == '.') i++;
                            else return TokenType.None;
                        }


                        if (i < lit.Literavalue.Length)
                            for (; i < lit.Literavalue.Length; i++)
                                if (lit.Literavalue[i] < '0' || lit.Literavalue[i] > '9') return TokenType.None;
                        return TokenType.TypeConst;
                    }
                    else if (lit.Literavalue[0] >= 'a' && lit.Literavalue[0] <= 'z' || lit.Literavalue[0] >= 'A' && lit.Literavalue[0] <= 'Z')
                    {

                        for (int i = 1; i < lit.Literavalue.Length; i++)
                            if ((lit.Literavalue[i] >= '0' && lit.Literavalue[i] <= '9') ||
                                (lit.Literavalue[i] >= 'a' && lit.Literavalue[i] <= 'z') ||
                                (lit.Literavalue[i] >= 'A' && lit.Literavalue[i] <= 'Z'))
                            { }
                            else return TokenType.None;
                        return TokenType.TypeIdent;
                    }
                    else return TokenType.None;
            }
        }

        public TokenCode GetLexemaCode(Lexema lex)
        {
            TokenType tt = lex.LexemaType;
            switch (tt)
            {
                case TokenType.None:
                    return TokenCode.None;

                case TokenType.TypeIdent:
                    return TokenCode.Ident;

                case TokenType.TypeKeyWord:
                    string str = lex.Lexemavalue;
                    switch (str)
                    {
                        case ("begin"): return TokenCode.KWBegin;
                        case ("end"): return TokenCode.KWEnd;
                        case ("var"): return TokenCode.KWVar;
                        case ("boolean"): return TokenCode.KWBoolean;
                        case ("integer"): return TokenCode.KWIntger;
                        case ("real"): return TokenCode.KWReal;
                        case ("string"): return TokenCode.KWString;
                    }
                    break;

                case TokenType.TypeConst:
                    switch (lex.Lexemavalue[0])
                    {
                        case ('"'):
                            return TokenCode.ConstString;

                        case ('T' or 'F'):
                            return TokenCode.ConstBool;

                        default:
                            for (int i = 0; i < lex.Lexemavalue.Length; i++)
                            {
                                if (lex.Lexemavalue[i] == '.') return TokenCode.ConstReal;
                            }
                            return TokenCode.ConstInt;
                    }


                case TokenType.TypeOper:
                    switch (lex.Lexemavalue)
                    {
                        case ("+"): return TokenCode.OpPlus;
                        case ("-"): return TokenCode.OpMinus;
                        case ("<"): return TokenCode.OpLater;
                        case (">"): return TokenCode.OpGreater;
                        case ("<="): return TokenCode.OpLaterequal;
                        case (">="): return TokenCode.OpGraterequal;
                        case ("<>"): return TokenCode.OpLatergreater;
                        case ("="): return TokenCode.OpEqual;
                        case (":="): return TokenCode.OpAssign;
                        case (";"): return TokenCode.OpSemicolon;
                        case (".."): return TokenCode.OpTwopoints;
                        case ("*"): return TokenCode.OpStar;
                        case ("/"): return TokenCode.OpSlash;
                        case ("div"): return TokenCode.OpDiv;
                        case ("mod"): return TokenCode.OpMod;
                        case ("("): return TokenCode.OpRightpar;
                        case (")"): return TokenCode.OpLeftpar;
                        case ("{"): return TokenCode.OpFrightpar;
                        case ("}"): return TokenCode.OpFleftpar;
                        default:
                            return TokenCode.None;
                    }
            }
            return TokenCode.None;
        }

        public Lexema GetNext()
        {

            Litera lit = IOM.GetNext();
            Lexema lexema = new Lexema(lit);
            return lexema;
        }

        public void GoScan()
        {


            //InOutModule IOM = new InOutModule(Filepath);
            while (true)
            {
                Litera lit = IOM.GetNext();
                Lexema lexema = new Lexema(lit);

                //Console.WriteLine(lit.Literavalue + " " + lit.LiteraPosition.lineNumber + " " + lit.LiteraPosition.charNumber);
                Console.WriteLine(lexema.ToString());
            }
        }
    }
}
