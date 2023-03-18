using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PascalCompiler
{
    internal class SyntaxAnalizer
    {
        //private Dictionary<string, Identifier> identDic;
        private Dictionary<string, Identifier> identDic;
        private string Filepath;
        private Lexer lexer;
        Lexema curlexema;
        Identifier curidntifier;
        InOutModule SinIOM;

        public SyntaxAnalizer() { }

        public SyntaxAnalizer(string filepath)
        {
            Filepath = filepath;
            lexer = new Lexer(filepath);
            curlexema = lexer.GetNext();
            identDic = new Dictionary<string, Identifier>();
            SinIOM = new InOutModule();
        }

        public void Printval()
        {
            //Console.Write(curlexema.Lexemavalue + " ");
            SinIOM.myPrint(curlexema.Lexemavalue + " ");
        }
        public void Printval(string str)
        {
            //Console.Write(str);
            SinIOM.myPrint(str);
        }

        public void Printnewl()
        {
            //Console.WriteLine();
            SinIOM.myCR();
        }

        public void GetNext(bool print = true, bool getnext = true)
        {
            if(print)
            Printval();
            if(getnext)
            curlexema = lexer.GetNext();
        }

        public void GoScan() // main shtuka
        {
            try
            {
                BNF_Program_Start();
                if(curlexema.LexemaCode == TokenCode.KWVar)
                BNF_Var_Start();
                BNF_CompoundOperator_Start();
                accept(TokenCode.OpPoint);
                Printval("\nКонец программы\n");


            }
            catch (Exception ex)
            {
                errorPrinter(ex.Message);
            }
            while (true)
            {
                try
                {
                    accept(TokenCode.None);
                }
                catch (Exception ex)
                {
                    errorPrinter(ex.Message);
                    Printnewl();
                }
            }
        }
        public void SkipExceptionPlace(TokenCode[] TC, string ExcMessage)
        {
            TextPosition tp = new TextPosition(curlexema.LexemaPosition);

            GetNext();
            while (!TC.Contains(curlexema.LexemaCode))
            {
                GetNext();
            }

            errorPrinter(ExcMessage, tp);
        }

        public void errorPrinter(string str)
        {
            Printval();
            Printnewl();
            string strk = "";
            for (int i = 0; i < curlexema.LexemaPosition.charNumber + 1; i++) strk += " ";
            strk += "^ " + str;
            Printval(strk);
            Printnewl();
            GetNext(false);
        }

        public void errorPrinter(string str, TextPosition tp)
        {
            Printnewl();
            string strk = "";
            for (int i = 0; i < tp.charNumber + 1; i++) strk += " ";
            strk += "^ " + str;
            Printval(strk);
            Printnewl();
            Printnewl();
        }

        public void accept(TokenCode TC)
        {
            if (curlexema.LexemaCode == TC)
            {
                Printval();
                curlexema = lexer.GetNext();
            }
            else
            {
               // Printval();
                throw new Exception("Ожидался токен: " + TC);          
            }
        }

        public void accept(TokenType TC)
        {
            if (curlexema.LexemaType == TC)
            {
                Printval();
                curlexema = lexer.GetNext();
            }
            else
            {
               // Printval();
                throw new Exception("Ожидался токен: " + TC);
            }

        }

        #region program

        public void BNF_Program_Start()
        {
            try
            {
                accept(TokenCode.KWProgram);
                Identifier id = new Identifier(curlexema.Lexemavalue, TokenCode.KWProgram);
                accept(TokenCode.Ident);
                identDic.Add(id.name, id);
                accept(TokenCode.OpSemicolon);
                //Console.WriteLine("норм робит");

            }
            catch (Exception ex)
            {
                //errorPrinter(ex.Message);
                TokenCode[] TC = { TokenCode.KWVar, TokenCode.KWBegin };
                SkipExceptionPlace(TC, ex.Message);
            }
            
        }

        #endregion

        #region var
        public void BNF_Var_Start()
        {
            try
            {
                accept(TokenCode.KWVar);
            }
            catch (Exception ex)
            {
                errorPrinter(ex.Message);
                //TokenCode[] TC = { TokenCode.Ident, TokenCode.KWBegin };
                //SkipExceptionPlace(TC, ex.Message);
            }


            do
            {
                try
                {
                    BNF_VarDeclarateStart();
                    accept(TokenCode.OpSemicolon);
                }
                catch (Exception ex)
                {
                    //errorPrinter(ex.Message);
                    TokenCode[] TC = { TokenCode.Ident, TokenCode.KWBegin };
                    SkipExceptionPlace(TC, ex.Message);
                }
            }
            while (curlexema.LexemaCode == TokenCode.Ident);

            //try
            //{
            //    do 
            //    {
            //        BNF_VarDeclarateStart();
            //        accept(TokenCode.OpSemicolon);
            //    }
            //    while (curlexema.LexemaCode == TokenCode.Ident);

            //}
            //catch (Exception ex)
            //{
            //    errorPrinter(ex.Message);
            //}

            //foreach (var i in identDic) Console.WriteLine($"key: {i.Key}  value: {i.Value}");
            //Console.WriteLine(identDic["loba"]);
        }

        public void BNF_VarDeclarateStart()
        {
            List<string> identnames = new List<string>();
            identnames.Add(curlexema.Lexemavalue);
            accept(TokenCode.Ident);
            while (curlexema.LexemaCode == TokenCode.OpComma)
            {
                GetNext();
                identnames.Add(curlexema.Lexemavalue);
                accept(TokenCode.Ident);
            }
            accept(TokenCode.OpColon);
            TokenCode temp = CheckType();

            for(int i=0;i<identnames.Count;i++)
            {
                Identifier id = new Identifier(identnames[i], temp);
                identDic.Add(identnames[i], id);
            }
        }

        public TokenCode CheckType()
        {
            switch (curlexema.LexemaCode)
            {
                case (TokenCode.KWBoolean):
                    GetNext();
                    return TokenCode.KWBoolean;

                case (TokenCode.KWIntger):
                    GetNext();
                    return TokenCode.KWIntger;
                
                case (TokenCode.KWReal):
                    GetNext();
                    return TokenCode.KWReal;
                
                case (TokenCode.KWString):
                    GetNext();
                    return TokenCode.KWString;
            }
            throw new Exception("Ожидался существующий тип данных");
        }


        #endregion

        #region compound operator

        public void BNF_CompoundOperator_Start()
        {
            try
            { 
                accept(TokenCode.KWBegin);
            }
            catch (Exception ex)
            {
                errorPrinter(ex.Message);
            }

            try
            {
               // BNF_Assign();
                while (curlexema.LexemaCode != TokenCode.KWEnd)
                {
                    BNF_Assign();
                }

            }
            catch (Exception ex)
            {
                errorPrinter(ex.Message);
            }


            try
            {
                accept(TokenCode.KWEnd);
            }
            catch (Exception ex)
            {
                errorPrinter(ex.Message);
            }

        }

        #endregion

        #region assign

        public void BNF_Assign()
        {

            try
            {
                string DicKey = curlexema.Lexemavalue;
                curidntifier = identDic[DicKey].Declared();
                accept(TokenCode.Ident);
                accept(TokenCode.OpAssign);

                switch (curidntifier.GetTypeCode())
                {
                    case (TokenCode.KWIntger):
                        BNF_Expression_Numeric_Start();
                        break;

                    case (TokenCode.KWReal):
                        BNF_Expression_Numeric_Start();
                        break;

                    case (TokenCode.KWBoolean):
                        BNF_Expression_Bool_Start();
                        break;

                    case (TokenCode.KWString):
                        BNF_Expression_String_Start();
                        break;

                    case (TokenCode.KWProgram):
                        throw new Exception("Невозможно присвоить значение идентификатору программы");
                        break;
                }

                identDic[DicKey] = curidntifier;

                accept(TokenCode.OpSemicolon);

            }
            catch (Exception ex)
            {
                //errorPrinter(ex.Message);
                TokenCode[] TC = { TokenCode.Ident, TokenCode.KWEnd };
                SkipExceptionPlace(TC, ex.Message);
            }
        }

        #endregion

        #region expression string

        public void BNF_Expression_String_Start()
        {
            switch (curlexema.LexemaCode)
            {
                case (TokenCode.ConstString):
                    accept(TokenCode.ConstString);
                    return;

                case (TokenCode.Ident):
                    BNF_Expression_String_Ident();
                    return;

                case (TokenCode.OpLeftpar):
                    BNF_Expression_String_Leftpar();
                    return;

            }
            throw new Exception("Ожидалось строковое выражение");
        }


        public void BNF_Expression_String_Leftpar()
        {
            accept(TokenCode.OpLeftpar);
            BNF_Expression_String_Start();
            accept(TokenCode.OpRightpar);
            return;
        }


        public void BNF_Expression_String_Ident()
        {
            Identifier tempid = identDic[curlexema.Lexemavalue];
            if (!tempid.IsDeclared()) throw new Exception("Идентификатор не содержит значений");
            if (tempid.GetTypeCode() != TokenCode.KWString) throw new Exception("Несовместимость строкового и нестрокового типов");
            accept(TokenCode.Ident);
            return;
        }

        #endregion

        #region expression bool

        public void BNF_Expression_Bool_Start()
        {
            switch (curlexema.LexemaCode)
            {
                case (TokenCode.ConstBool):
                    accept(TokenCode.ConstBool);
                    return;

                case (TokenCode.Ident):
                    BNF_Expression_Bool_Ident();
                    return;

                case (TokenCode.OpLeftpar):
                    BNF_Expression_Bool_Leftpar();
                    return;

            }
            throw new Exception("Ожидалось логическое выражение");
        }


        public void BNF_Expression_Bool_Leftpar()
        {
            accept(TokenCode.OpLeftpar);
            BNF_Expression_Bool_Start();
            accept(TokenCode.OpRightpar);
            return;
        }


        public void BNF_Expression_Bool_Ident()
        {
            Identifier tempid = identDic[curlexema.Lexemavalue];
            if (!tempid.IsDeclared()) throw new Exception("Идентификатор не содержит значений");
            if (tempid.GetTypeCode() != TokenCode.KWBoolean) throw new Exception("Несовместимость логического и нелогического типов");
            accept(TokenCode.Ident);
            return;
        }

        #endregion

        #region expression numeric

        public void BNF_Expression_Numeric_Start()
        {
            //try
            {
                switch (curlexema.LexemaCode)
                {
                    case (TokenCode.ConstInt):
                        BNF_Expression_Numeric_Number();
                        return;


                    case (TokenCode.ConstReal):
                        BNF_Expression_Numeric_Real();
                        return;

                    case (TokenCode.Ident):
                        BNF_Expression_Numeric_Ident();
                        return;

                    case (TokenCode.OpLeftpar):
                        BNF_Expression_Numeric_Leftpar();
                        return;

                    case (TokenCode.OpMinus):
                        GetNext();
                        BNF_Expression_Numeric_Start();
                        return;

                    case (TokenCode.OpPlus):
                        GetNext();
                        BNF_Expression_Numeric_Start();
                        return;
                }
                throw new Exception("Ожидалось численное выражение");
            }
            //catch (Exception ex)
            //{
            //    errorPrinter(ex.Message);

            //    //GetNext();
            //}
        }



        public void BNF_Expression_Numeric_Number()
        {
            accept(TokenCode.ConstInt);
            switch (curlexema.LexemaCode)
            {
                case (TokenCode.OpPlus):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;

                case (TokenCode.OpMinus):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;

                case (TokenCode.OpStar):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;

                case (TokenCode.OpSlash):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;
            }
            return;

        }


        public void BNF_Expression_Numeric_Real()
        {
            if (curidntifier.GetTypeCode() != TokenCode.KWReal) throw new Exception("Несовместимость типов integer и real");
            accept(TokenCode.ConstReal);
            switch (curlexema.LexemaCode)
            {
                case (TokenCode.OpPlus):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;

                case (TokenCode.OpMinus):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;

                case (TokenCode.OpStar):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;

                case (TokenCode.OpSlash):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;
            }
            return;

        }

        public void BNF_Expression_Numeric_Ident()
        {
            Identifier tempid = identDic[curlexema.Lexemavalue];
            if(!tempid.IsDeclared()) throw new Exception("Идентификатор не содержит значений");
            if (tempid.GetTypeCode() == TokenCode.KWReal && curidntifier.GetTypeCode() == TokenCode.KWIntger) throw new Exception("Несовместимость типов integer и real");
            if(tempid.GetTypeCode() != TokenCode.KWReal && tempid.GetTypeCode() != TokenCode.KWIntger) throw new Exception("Несовместимость числового и нечислового типов");
            accept(TokenCode.Ident);
            switch (curlexema.LexemaCode)
            {
                case (TokenCode.OpPlus):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;

                case (TokenCode.OpMinus):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;

                case (TokenCode.OpStar):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;

                case (TokenCode.OpSlash):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;
            }
            return;

        }

        public void BNF_Expression_Numeric_Leftpar()
        {
            accept(TokenCode.OpLeftpar);
            BNF_Expression_Numeric_Start();
            accept(TokenCode.OpRightpar);
            switch (curlexema.LexemaCode)
            {
                case (TokenCode.OpPlus):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;

                case (TokenCode.OpMinus):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;

                case (TokenCode.OpStar):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;

                case (TokenCode.OpSlash):
                    GetNext();
                    BNF_Expression_Numeric_Start();
                    return;
            }
            return;

        }

        #endregion


        //public Lexema BNF_expression(Lexema curlex)
        //{ 
        //    Lexema left = new Lexema(curlex);
        //    curlex = lexer.GetNext();

        //    switch(curlex.LexemaType) 
        //    {
        //        case TokenType.TypeOper:



        //    }

        //}










    }
}
