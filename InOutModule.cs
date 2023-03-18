using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler
{

    
    internal class InOutModule
    {
        private TextPosition IOMTextPosition;
        private StreamReader file;
        private bool IsFinished;
        private int LastInLine;
        private string curline;

        public InOutModule(string filepath)
        { 
            file = new StreamReader(filepath);
            IsFinished = false;
            IOMTextPosition = new TextPosition(-1, 0);
            GetNextLine();
        }


        public InOutModule()
        {

        }

        public void myCR()
        {
            Console.WriteLine();
        }

        public void myPrint(string str)
        {
            Console.Write(str);
        }


        private void GetNextLine()
        {
            if (!file.EndOfStream)
            {
                curline = file.ReadLine();
                IOMTextPosition.lineNumber++;
                IOMTextPosition.charNumber = 0;
                LastInLine = curline.Length - 1;
            }
            else 
            {
                //Fin();
                //Exit();

                file.Close(); 
                Environment.Exit(0);
            }
        
        }

        public Litera GetNext()
        {
            if (LastInLine == -1 || IOMTextPosition.charNumber > LastInLine)
            {
                while (LastInLine == -1 || IOMTextPosition.charNumber > LastInLine)
                {
                    GetNextLine();

                }
                myCR();
            }
            while (IOMTextPosition.charNumber < LastInLine)
                if (curline[IOMTextPosition.charNumber] == ' ' || curline[IOMTextPosition.charNumber] == '\t')
                    IOMTextPosition.charNumber++;
                else break;

            if (IOMTextPosition.charNumber == LastInLine)
                if (curline[IOMTextPosition.charNumber] == ' ' || curline[IOMTextPosition.charNumber] == '\t')
                {
                    myCR();
                    GetNextLine();
                    return GetNext();
                }
                else
                {
                    string litval = "" + curline[IOMTextPosition.charNumber];
                    Litera lit = new Litera(new TextPosition(IOMTextPosition), litval);
                    IOMTextPosition.charNumber++;
                    return lit;

                //switch (litval[0])


                //{

                //    case ('+' or '*' or '/' or ';' or '=' or '\'' or ',' or '{' or '}' or '(' or ')' or '[' or ']' or '-' or '<' or '>' or '='):
                //        return lit;
                //    default: return

                //}


                }   

            else
            {
                string litval = "" + curline[IOMTextPosition.charNumber];
                Litera lit = new Litera(new TextPosition(IOMTextPosition), litval);

                switch (litval[0])
                {
                    case ('+' or '*' or '/' or ',' or ';' or '=' or '\'' or ',' or '{' or '}' or '(' or ')' or '[' or ']'):
                        IOMTextPosition.charNumber++;
                        return lit;
                    case (':' or '>'):
                        if (IOMTextPosition.charNumber <= LastInLine && curline[IOMTextPosition.charNumber+1] == '=')
                        {
                            litval += curline[IOMTextPosition.charNumber + 1];
                            IOMTextPosition.charNumber++;
                            lit.Literavalue = litval;
                        }
                        IOMTextPosition.charNumber++;
                        return lit;
                    case ('.'):
                        IOMTextPosition.charNumber++;
                        if (IOMTextPosition.charNumber <= LastInLine && curline[IOMTextPosition.charNumber] == '.')
                        {
                            litval += curline[IOMTextPosition.charNumber];
                            IOMTextPosition.charNumber++;
                            lit.Literavalue = litval;
                        }
                        
                        return lit;

                    case ('<'):
                        IOMTextPosition.charNumber++;
                        if (IOMTextPosition.charNumber <= LastInLine && (curline[IOMTextPosition.charNumber] == '=' || curline[IOMTextPosition.charNumber] == '>'))
                        {
                            litval += curline[IOMTextPosition.charNumber];
                            IOMTextPosition.charNumber++;
                            lit.Literavalue = litval;
                        }
                        return lit;


                    case ('"'):
                        IOMTextPosition.charNumber++;
                        while (IOMTextPosition.charNumber <= LastInLine && curline[IOMTextPosition.charNumber] != '"')
                        {
                            litval += curline[IOMTextPosition.charNumber];
                            IOMTextPosition.charNumber++;
                            lit.Literavalue = litval;
                        }
                        if(IOMTextPosition.charNumber>LastInLine)
                        return lit;
                        if (curline[IOMTextPosition.charNumber] == '"')
                        {
                            litval += curline[IOMTextPosition.charNumber];
                            IOMTextPosition.charNumber++;
                            lit.Literavalue = litval;
                        }
                        return lit;

                    default:

                        if (litval[0] == '-' || (litval[0] >= '0' && litval[0] <= '9'))
                        {
                            while (true)
                            {
                                if (IOMTextPosition.charNumber < LastInLine && (curline[IOMTextPosition.charNumber + 1] >= '0' 
                                    && curline[IOMTextPosition.charNumber + 1] <= '9'|| 
                                    curline[IOMTextPosition.charNumber + 1] == '.'))
                                {
                                    IOMTextPosition.charNumber++;
                                    litval += curline[IOMTextPosition.charNumber];
                                }
                                else
                                {
                                    IOMTextPosition.charNumber++;
                                    lit.Literavalue = litval;
                                    return lit;
                                }
                            }
                        }

                        if (litval[0] >= 'a' && litval[0] <= 'z' || litval[0] >= 'A' && litval[0] <= 'Z')
                        {

                            while (true)
                            {
                                if (IOMTextPosition.charNumber < LastInLine &&
                                    ((curline[IOMTextPosition.charNumber + 1] >= '0' && curline[IOMTextPosition.charNumber + 1] <= '9') ||
                                    (curline[IOMTextPosition.charNumber + 1] >= 'a' && curline[IOMTextPosition.charNumber + 1] <= 'z') ||
                                    (curline[IOMTextPosition.charNumber + 1] >= 'A' && curline[IOMTextPosition.charNumber + 1] <= 'Z')))
                                {
                                    IOMTextPosition.charNumber++;
                                    litval += curline[IOMTextPosition.charNumber];
                                }
                                else
                                {
                                    IOMTextPosition.charNumber++;
                                    lit.Literavalue = litval;
                                    return lit;
                                }
                            }
                        }
                        else
                        {
                            IOMTextPosition.charNumber++;
                                return lit; 
                        }
                }
            }
        }
    }
}
