using System;
using System.Collections;
using System.Text;

namespace MultiFuck
{
    public class MultiFuckInterpreter
    {
        private class Processor
        {
            private int id { get; }
            private Memory RAM;
            private string program;
            private int instructionPointer;
            private int dataPointer;

            public Processor(string program, int id = 0)
            {
                this.id = id;
                this.program = program;
                this.instructionPointer = 0;
                this.dataPointer = 0;
                this.RAM = new Memory();
            }

            public Processor(string program, ref Memory RAM, int id = 0)
            {
                this.id = id;
                this.program = program;
                this.instructionPointer = 0;
                this.dataPointer = 0;
                this.RAM = RAM;
            }

            public void SetRamUsed(ref Memory RAM)
            {
                this.RAM = RAM;
            }

            public void handleTick()
            {
                if (instructionPointer >= program.Length)
                    return;
                char command = program[instructionPointer];

                switch (command)
                {
                    case '+':
                        RAM.IncrementValue(dataPointer);
                        break;
                    case '-':
                        RAM.DecrementValue(dataPointer);
                        break;
                    case '<':
                        if (dataPointer == 0)
                            RAM.InsertCell(dataPointer);
                        else
                            dataPointer--;
                        break;
                    case '>':
                        if (dataPointer == RAM.GetSize() - 1)
                            RAM.InsertCell(dataPointer + 1);
                        dataPointer++;
                        break;

                    case '[':
                        {
                            if (RAM.GetByteFromLocation(dataPointer) == 0)
                            {
                                int numOfOpening = 1;
                                int x = instructionPointer;
                                while (!(numOfOpening == 0 && program[x] == ']'))
                                {
                                    x++;
                                    if (program[x] == '[')
                                        numOfOpening++;
                                    if (program[x] == ']')
                                    {
                                        numOfOpening--;
                                        if (numOfOpening == 0)
                                            instructionPointer = x;
                                    }
                                }
                            }
                        }
                        break;
                    case ']':
                        {
                            int numOfOpening = 1;
                            int x = instructionPointer;
                            while (!(numOfOpening == 0 && program[x] == '['))
                            {
                                x--;
                                if (program[x] == ']')
                                    numOfOpening++;
                                if (program[x] == '[')
                                {
                                    numOfOpening--;
                                    if (numOfOpening == 0)
                                        instructionPointer = x - 1;
                                }
                            }
                        }
                        break;
                    case '.':
                        byte[] letter = { Convert.ToByte(RAM.GetByteFromLocation(dataPointer)) };
                        char[] print = Encoding.ASCII.GetChars(letter);
                        Console.Write(print);
                        break;
                    case ',':
                        char[] input = { 'a' };
                        input[0] = Console.ReadKey().KeyChar;
                        RAM.SetByteAtLocation(dataPointer, (int)Encoding.ASCII.GetBytes(input)[0]);
                        break;
                    default:
                        break;
                }

                instructionPointer++;
            }

            public bool IsExecutionFinised()
            {
                if (instructionPointer == program.Length)
                    return true;
                return false;
            }
        }

        private class Memory
        {
            private int id { get; }
            private ArrayList data = new ArrayList();

            public Memory(int id = 0)
            {
                this.id = id;
                data.Add(0);
            }

            public int GetByteFromLocation(int location)
            {
                if (location >= data.Count || location < 0)
                    throw new Exception("The index received was was out of bound, the index was: " + location + " whereas the size of the RAM is: " + data.Count);

                return (int)data[location];
            }

            public void SetByteAtLocation(int location, int value)
            {
                if (location >= data.Count || location < 0)
                    throw new Exception("The index received was was out of bound, the index was: " + location + " whereas the size of the RAM is: " + data.Count);

                if (value > 255)
                    throw new Exception("Value too large for byte in RAM. The value must be between 0 and 255. Received Value is: " + value);

                data[location] = value;
            }

            public void IncrementValue(int location)
            {
                if ((int)data[location] > 255)
                    data[location] = 0;
                else
                    data[location] = (int)data[location] + 1;
            }

            public void DecrementValue(int location)
            {
                if ((int)data[location] <= 0)
                    data[location] = 255;
                else
                    data[location] = (int)data[location] - 1;
            }

            public void InsertCell(int location, int value = 0)
            {
                data.Insert(location, value);
            }

            internal int GetSize()
            {
                return data.Count;
            }
        }

        private int numberOfProcessors;
        private Processor[] processors;
        private Memory[] RAMDataStorage;
        private string mainProgram;

        public MultiFuckInterpreter(int numberOfProcessors, string mainProgram)
        {
            this.numberOfProcessors = numberOfProcessors;
            this.processors = new Processor[numberOfProcessors];
            this.RAMDataStorage = new Memory[numberOfProcessors];
            this.mainProgram = mainProgram;

            for (int x = 0; x < numberOfProcessors; x++)
            {
                this.RAMDataStorage[x] = new Memory(x);
                this.processors[x] = new Processor(GetProgram(x, numberOfProcessors), ref this.RAMDataStorage[x], x);
            }
        }

        public void RunProgram()
        {
            bool runProgram = true;
            while (runProgram == true)
            {
                for (int processorID = 0; processorID < numberOfProcessors; processorID++)
                {
                    runProgram = !processors[processorID].IsExecutionFinised();
                    if (runProgram)
                        processors[processorID].handleTick();
                }
            }
        }

        private string GetProgram(int processorNumber, int numberOfProcessors)
        {
            StringBuilder programBuilder = new StringBuilder();

            int instructionForProcessor = 0;
            for (int iterator = 0; iterator < mainProgram.Length; iterator++)
            {
                if (instructionForProcessor >= numberOfProcessors)
                    instructionForProcessor = 0;

                char possibleInstruction = mainProgram[iterator];
                if (
                    possibleInstruction == '<' ||
                    possibleInstruction == '>' ||
                    possibleInstruction == '+' ||
                    possibleInstruction == '-' ||
                    possibleInstruction == '.' ||
                    possibleInstruction == ',' ||
                    possibleInstruction == '[' ||
                    possibleInstruction == ']'
                    )
                {
                    if (instructionForProcessor == processorNumber)
                        programBuilder.Append(possibleInstruction);

                    instructionForProcessor++;
                }
            }

            return programBuilder.ToString();
        }
    }
}
