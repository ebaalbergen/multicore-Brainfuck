using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiFuck;

namespace MultiFucktestProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            MultiFuckInterpreter multiFuckinterpreter = new MultiFuckInterpreter(2, "++++++++++++++++++++[[>>++++++++++++++>>++++++++++++++++++++>>++++++>>++<<<<<<<<--]]>>++++..>>++..++++++++++++++....++++++..>>++++..<<<<++++++++++++++++++++++++++++++..>>..++++++..------------..----------------..>>++..>>..");
            multiFuckinterpreter.RunProgram();
            Console.Read();
        }
    }
}
