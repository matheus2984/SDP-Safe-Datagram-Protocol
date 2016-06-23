using System;

namespace SDP.FEC
{
    public sealed class GenericGf
    {
        public static GenericGf AztecData12 = new GenericGf(0x1069, 4096, 1); // x^12 + x^6 + x^5 + x^3 + 1
        public static GenericGf AztecData10 = new GenericGf(0x409, 1024, 1); // x^10 + x^3 + 1
        public static GenericGf AztecData6 = new GenericGf(0x43, 64, 1); // x^6 + x + 1
        public static GenericGf AztecParam = new GenericGf(0x13, 16, 1); // x^4 + x + 1
        public static GenericGf QrCodeField256 = new GenericGf(0x011D, 256, 0); // x^8 + x^4 + x^3 + x^2 + 1
        public static GenericGf DataMatrixField256 = new GenericGf(0x012D, 256, 1); // x^8 + x^5 + x^3 + x^2 + 1
        public static GenericGf AztecData8 = DataMatrixField256;
        public static GenericGf MaxicodeField64 = AztecData6;

        private int[] expTable;
        private int[] logTable;
        private GenericGfPoly zero;
        private GenericGfPoly one;
        private readonly int size;
        private readonly int primitive;
        private readonly int generatorBase;

        public GenericGf(int primitive, int size, int genBase)
        {
            this.primitive = primitive;
            this.size = size;
            this.generatorBase = genBase;

            expTable = new int[size];
            logTable = new int[size];
            int x = 1;
            for (int i = 0; i < size; i++)
            {
                expTable[i] = x;
                x <<= 1; // x = x * 2; 
                if (x >= size)
                {
                    x ^= primitive;
                    x &= size - 1;
                }
            }
            for (int i = 0; i < size - 1; i++)
            {
                logTable[expTable[i]] = i;
            }

            zero = new GenericGfPoly(this, new[] { 0 });
            one = new GenericGfPoly(this, new[] { 1 });
        }

        internal GenericGfPoly Zero
        {
            get
            {
                return zero;
            }
        }

        internal GenericGfPoly One
        {
            get
            {
                return one;
            }
        }

        internal GenericGfPoly BuildMonomial(int degree, int coefficient)
        {
            if (degree < 0)
            {
                throw new ArgumentException();
            }
            if (coefficient == 0)
            {
                return zero;
            }
            int[] coefficients = new int[degree + 1];
            coefficients[0] = coefficient;
            return new GenericGfPoly(this, coefficients);
        }

        internal static int AddOrSubtract(int a, int b)
        {
            return a ^ b;
        }

        internal int Exp(int a)
        {
            return expTable[a];
        }

        internal int Log(int a)
        {
            if (a == 0)
            {
                throw new ArgumentException();
            }
            return logTable[a];
        }

        internal int Inverse(int a)
        {
            if (a == 0)
            {
                throw new ArithmeticException();
            }
            return expTable[size - logTable[a] - 1];
        }

        internal int Multiply(int a, int b)
        {
            if (a == 0 || b == 0)
            {
                return 0;
            }
            return expTable[(logTable[a] + logTable[b]) % (size - 1)];
        }

        public int Size
        {
            get { return size; }
        }

        public int GeneratorBase
        {
            get { return generatorBase; }
        }

        public override String ToString()
        {
            return "GF(0x" + primitive.ToString("X") + ',' + size + ')';
        }
    }
}