using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler
{
    public enum TokenType
    {
        None = 0,
        TypeIdent = 4,
        TypeConst = 1,
        TypeOper = 2,
        TypeKeyWord = 3
    }


    public enum TokenCode
    {
        None = 0,// пустота
        Ident = 400,// идентификатор

        KWProgram = 101,
        KWBegin = 102,
        KWEnd = 103,
        KWVar = 104,
        KWIntger = 105,
        KWReal = 106,
        KWString = 107,
        KWBoolean = 108,

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
        OpRightpar = 314, // )
        OpLeftpar = 315, // (
        OpFrightpar = 316, // }
        OpFleftpar = 317, // {
        OpTwopoints = 318, // ..
        OpSemicolon = 319, // ;
        OpColon = 320, // :
        OpPoint = 321, // .
        OpComma = 322 // ,
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

        public Lexema(Lexema l)
        {
            LexemaPosition = new TextPosition(l.LexemaPosition);
            Lexemavalue = l.Lexemavalue;
            LexemaType = l.LexemaType;
            LexemaCode = l.LexemaCode;
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


    internal struct Identifier
    {
        public string name;
        public TokenCode type;
        bool declared;

        public Identifier(string name, TokenCode type)
        {
            this.name = name;
            this.type = type;
            this.declared = false;
        }

        public Identifier Declared()
        {
            this.declared = true;
            return this;
        }

        public bool IsDeclared()
        {
            return declared;        
        }
        public TokenCode GetTypeCode()
        {
            return type;
        }

        public override string ToString()
        {
            return "name: " + name + " type: " + type + " " + " declared: " + declared;
        }

    }
}
