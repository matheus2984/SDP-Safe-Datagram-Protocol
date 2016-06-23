namespace SDP.FEC
{
    public sealed class ReedSolomonDecoder
    {
        private readonly GenericGf field;

        public ReedSolomonDecoder(GenericGf field)
        {
            this.field = field;
        }

        public bool Decode(int[] received, int twoS)
        {
            var poly = new GenericGfPoly(field, received);
            var syndromeCoefficients = new int[twoS];
            var noError = true;
            for (var i = 0; i < twoS; i++)
            {
                var eval = poly.EvaluateAt(field.Exp(i + field.GeneratorBase));
                syndromeCoefficients[syndromeCoefficients.Length - 1 - i] = eval;
                if (eval != 0)
                {
                    noError = false;
                }
            }
            if (noError)
            {
                return true;
            }
            var syndrome = new GenericGfPoly(field, syndromeCoefficients);

            var sigmaOmega = RunEuclideanAlgorithm(field.BuildMonomial(twoS, 1), syndrome, twoS);
            if (sigmaOmega == null)
                return false;

            var sigma = sigmaOmega[0];
            var errorLocations = FindErrorLocations(sigma);
            if (errorLocations == null)
                return false;

            var omega = sigmaOmega[1];
            var errorMagnitudes = FindErrorMagnitudes(omega, errorLocations);
            for (var i = 0; i < errorLocations.Length; i++)
            {
                var position = received.Length - 1 - field.Log(errorLocations[i]);
                if (position < 0)
                {
                    return false;
                }
                received[position] = GenericGf.AddOrSubtract(received[position], errorMagnitudes[i]);
            }

            return true;
        }

        internal GenericGfPoly[] RunEuclideanAlgorithm(GenericGfPoly a, GenericGfPoly b, int R)
        {
            if (a.Degree < b.Degree)
            {
                GenericGfPoly temp = a;
                a = b;
                b = temp;
            }

            GenericGfPoly rLast = a;
            GenericGfPoly r = b;
            GenericGfPoly tLast = field.Zero;
            GenericGfPoly t = field.One;

            while (r.Degree >= R / 2)
            {
                GenericGfPoly rLastLast = rLast;
                GenericGfPoly tLastLast = tLast;
                rLast = r;
                tLast = t;

                if (rLast.IsZero)
                {
                    return null;
                }
                r = rLastLast;
                GenericGfPoly q = field.Zero;
                int denominatorLeadingTerm = rLast.GetCoefficient(rLast.Degree);
                int dltInverse = field.Inverse(denominatorLeadingTerm);
                while (r.Degree >= rLast.Degree && !r.IsZero)
                {
                    int degreeDiff = r.Degree - rLast.Degree;
                    int scale = field.Multiply(r.GetCoefficient(r.Degree), dltInverse);
                    q = q.AddOrSubtract(field.BuildMonomial(degreeDiff, scale));
                    r = r.AddOrSubtract(rLast.MultiplyByMonomial(degreeDiff, scale));
                }

                t = q.Multiply(tLast).AddOrSubtract(tLastLast);

                if (r.Degree >= rLast.Degree)
                {
                    return null;
                }
            }

            int sigmaTildeAtZero = t.GetCoefficient(0);
            if (sigmaTildeAtZero == 0)
            {
                return null;
            }

            int inverse = field.Inverse(sigmaTildeAtZero);
            GenericGfPoly sigma = t.Multiply(inverse);
            GenericGfPoly omega = r.Multiply(inverse);
            return new[] { sigma, omega };
        }

        private int[] FindErrorLocations(GenericGfPoly errorLocator)
        {
            int numErrors = errorLocator.Degree;
            if (numErrors == 1)
            {
                return new[] { errorLocator.GetCoefficient(1) };
            }
            int[] result = new int[numErrors];
            int e = 0;
            for (int i = 1; i < field.Size && e < numErrors; i++)
            {
                if (errorLocator.EvaluateAt(i) == 0)
                {
                    result[e] = field.Inverse(i);
                    e++;
                }
            }
            if (e != numErrors)
            {
                return null;
            }
            return result;
        }

        private int[] FindErrorMagnitudes(GenericGfPoly errorEvaluator, int[] errorLocations)
        {
            int s = errorLocations.Length;
            int[] result = new int[s];
            for (int i = 0; i < s; i++)
            {
                int xiInverse = field.Inverse(errorLocations[i]);
                int denominator = 1;
                for (int j = 0; j < s; j++)
                {
                    if (i != j)
                    {
                        int term = field.Multiply(errorLocations[j], xiInverse);
                        int termPlus1 = (term & 0x1) == 0 ? term | 1 : term & ~1;
                        denominator = field.Multiply(denominator, termPlus1);
                    }
                }
                result[i] = field.Multiply(errorEvaluator.EvaluateAt(xiInverse), field.Inverse(denominator));
                if (field.GeneratorBase != 0)
                {
                    result[i] = field.Multiply(result[i], xiInverse);
                }
            }
            return result;
        }
    }
}