using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler
{
    internal class SyntaxAnalizer
    {

        private string Filepath;
        private Lexer lexer;

        public SyntaxAnalizer() { }

        public SyntaxAnalizer(string filepath)
        {
            Filepath = filepath;
            lexer = new Lexer(filepath);
        }














    }
}
