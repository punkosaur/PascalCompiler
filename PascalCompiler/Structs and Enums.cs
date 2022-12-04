using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler
{
    public enum TokenType
    {
        None = 0,
        TypeConst = 1,
        TypeIdent = 2,
        TypeOper = 3,
        TypeKeyWord = 4
    }


    public enum TokenCode
    {
        None = 0,// пустота
        Ident = 1,// идентификатор

        KWBegin = 101,
        KWEnd = 102,
        KWVar = 103,
        KWIntger = 104,
        KWReal = 105,
        KWString = 106,
        KWBoolean = 107,

        ConstInt = 201, // целое
        ConstReal = 202,// вещественное
        ConstString = 203,// строка
        ConstBool = 204,// труфолс


        OpPlus = 301, //+
        OpMinus = 302, // -
        OpSlash = 303, // /
        OpStar = 304, // *
        OpEqual = 305, // =
        OpAssign = 306, // :=
        OpDiv = 307, // DIV
        OpMod = 308, // MOD
        OpLater = 309, // <
        OpGreater = 310, // >
        OpLaterequal = 311, // <=
        OpGraterequal = 312, // >=
        OpLatergreater = 313, // <>
        OpRightpar = 314, // (
        OpLeftpar = 315, // )
        OpFrightpar = 316, // {
        OpFleftpar = 317, // }
        OpTwopoints = 318, // ..
        OpSemicolon = 319 // ;
    }

    internal struct Lexema
    {
        public TextPosition LexemaPosition;
        public string Lexemavalue;
        public TokenType LexemaType;
        public TokenCode LexemaCode;


        public Lexema(Litera lit)
        {
            LexemaPosition = new TextPosition(lit.LiteraPosition);
            Lexemavalue = lit.Literavalue;
            Lexer lex = new Lexer();
            LexemaType = lex.GetLexemaType(lit);
            LexemaCode = lex.GetLexemaCode(this);
        }

        public override string ToString()
        {
            return Lexemavalue + " " + "pos: " + LexemaPosition.lineNumber + " " + LexemaPosition.charNumber + " " + LexemaType + " " + LexemaCode;
        }

    }

    internal struct TextPosition
    {
        public int lineNumber; // номер строки
        public int charNumber; // номер позиции в строке

        public TextPosition(int ln, int c)
        {
            lineNumber = ln;
            charNumber = c;
        }

        public TextPosition(TextPosition a)
        {
            lineNumber = a.lineNumber;
            charNumber = a.charNumber;
        }
    }

    internal struct Err
    {
        public TextPosition errorPosition;
        public int errorCode;

        public Err(TextPosition errorPosition, int errorCode)
        {
            this.errorPosition = errorPosition;
            this.errorCode = errorCode;
        }
    }

    internal struct Litera
    {
        public TextPosition LiteraPosition;
        public string Literavalue;

        public Litera(TextPosition literapos, string literavalue)
        {
            this.LiteraPosition = literapos;
            this.Literavalue = literavalue;
        }

    }
}
