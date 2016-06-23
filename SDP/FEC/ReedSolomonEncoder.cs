using System;
using System.Collections.Generic;

namespace SDP.FEC
{
    public sealed class ReedSolomonEncoder
    {
        private readonly GenericGf field;
        private readonly IList<GenericGfPoly> cachedGenerators;

        public ReedSolomonEncoder(GenericGf field)
        {
            this.field = field;
            this.cachedGenerators = new List<GenericGfPoly>();
            cachedGenerators.Add(new GenericGfPoly(field, new[] { 1 }));
        }

        private GenericGfPoly BuildGenerator(int degree)
        {
            if (degree >= cachedGenerators.Count)
            {
                var lastGenerator = cachedGenerators[cachedGenerators.Count - 1];
                for (int d = cachedGenerators.Count; d <= degree; d++)
                {
                    var nextGenerator =
                        lastGenerator.Multiply(new GenericGfPoly(field,
                            new[] { 1, field.Exp(d - 1 + field.GeneratorBase) }));
                    cachedGenerators.Add(nextGenerator);
                    lastGenerator = nextGenerator;
                }
            }
            return cachedGenerators[degree];
        }

        public void Encode(int[] toEncode, int ecBytes)
        {
            if (ecBytes == 0)
            {
                throw new ArgumentException("No error correction bytes");
            }
            var dataBytes = toEncode.Length - ecBytes;
            if (dataBytes <= 0)
            {
                throw new ArgumentException("No data bytes provided");
            }

            var generator = BuildGenerator(ecBytes);
            var infoCoefficients = new int[dataBytes];
            Array.Copy(toEncode, 0, infoCoefficients, 0, dataBytes);

            var info = new GenericGfPoly(field, infoCoefficients);
            info = info.MultiplyByMonomial(ecBytes, 1);

            var remainder = info.Divide(generator)[1];
            var coefficients = remainder.Coefficients;
            var numZeroCoefficients = ecBytes - coefficients.Length;
            for (var i = 0; i < numZeroCoefficients; i++)
            {
                toEncode[dataBytes + i] = 0;
            }

            Array.Copy(coefficients, 0, toEncode, dataBytes + numZeroCoefficients, coefficients.Length);
        }
    }
}