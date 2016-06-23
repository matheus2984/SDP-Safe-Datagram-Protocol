using System;
using System.Text;

namespace SDP.FEC
{
    internal sealed class GenericGfPoly
    {
        private readonly GenericGf field;
        private readonly int[] coefficients;

        internal GenericGfPoly(GenericGf field, int[] coefficients)
        {
            if (coefficients.Length == 0)
            {
                throw new ArgumentException();
            }
            this.field = field;
            int coefficientsLength = coefficients.Length;
            if (coefficientsLength > 1 && coefficients[0] == 0)
            {
                int firstNonZero = 1;
                while (firstNonZero < coefficientsLength && coefficients[firstNonZero] == 0)
                {
                    firstNonZero++;
                }
                if (firstNonZero == coefficientsLength)
                {
                    this.coefficients = new[] { 0 };
                }
                else
                {
                    this.coefficients = new int[coefficientsLength - firstNonZero];
                    Array.Copy(coefficients,
                        firstNonZero,
                        this.coefficients,
                        0,
                        this.coefficients.Length);
                }
            }
            else
            {
                this.coefficients = coefficients;
            }
        }

        internal int[] Coefficients
        {
            get { return coefficients; }
        }

        /// <summary>
        /// degree of this polynomial
        /// </summary>
        internal int Degree
        {
            get
            {
                return coefficients.Length - 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="GenericGfPoly"/> is zero.
        /// </summary>
        /// <value>true iff this polynomial is the monomial "0"</value>
        internal bool IsZero
        {
            get { return coefficients[0] == 0; }
        }

        /// <summary>
        /// coefficient of x^degree term in this polynomial
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <returns>coefficient of x^degree term in this polynomial</returns>
        internal int GetCoefficient(int degree)
        {
            return coefficients[coefficients.Length - 1 - degree];
        }

        /// <summary>
        /// evaluation of this polynomial at a given point
        /// </summary>
        /// <param name="a">A.</param>
        /// <returns>evaluation of this polynomial at a given point</returns>
        internal int EvaluateAt(int a)
        {
            int result = 0;
            if (a == 0)
            {
                // Just return the x^0 coefficient
                return GetCoefficient(0);
            }
            int size = coefficients.Length;
            if (a == 1)
            {
                // Just the sum of the coefficients
                foreach (var coefficient in coefficients)
                {
                    result = GenericGf.AddOrSubtract(result, coefficient);
                }
                return result;
            }
            result = coefficients[0];
            for (int i = 1; i < size; i++)
            {
                result = GenericGf.AddOrSubtract(field.Multiply(a, result), coefficients[i]);
            }
            return result;
        }

        internal GenericGfPoly AddOrSubtract(GenericGfPoly other)
        {
            if (!field.Equals(other.field))
            {
                throw new ArgumentException("GenericGFPolys do not have same GenericGF field");
            }
            if (IsZero)
            {
                return other;
            }
            if (other.IsZero)
            {
                return this;
            }

            int[] smallerCoefficients = this.coefficients;
            int[] largerCoefficients = other.coefficients;
            if (smallerCoefficients.Length > largerCoefficients.Length)
            {
                int[] temp = smallerCoefficients;
                smallerCoefficients = largerCoefficients;
                largerCoefficients = temp;
            }
            int[] sumDiff = new int[largerCoefficients.Length];
            int lengthDiff = largerCoefficients.Length - smallerCoefficients.Length;
            // Copy high-order terms only found in higher-degree polynomial's coefficients
            Array.Copy(largerCoefficients, 0, sumDiff, 0, lengthDiff);

            for (int i = lengthDiff; i < largerCoefficients.Length; i++)
            {
                sumDiff[i] = GenericGf.AddOrSubtract(smallerCoefficients[i - lengthDiff], largerCoefficients[i]);
            }

            return new GenericGfPoly(field, sumDiff);
        }

        internal GenericGfPoly Multiply(GenericGfPoly other)
        {
            if (!field.Equals(other.field))
            {
                throw new ArgumentException("GenericGFPolys do not have same GenericGF field");
            }
            if (IsZero || other.IsZero)
            {
                return field.Zero;
            }
            int[] aCoefficients = this.coefficients;
            int aLength = aCoefficients.Length;
            int[] bCoefficients = other.coefficients;
            int bLength = bCoefficients.Length;
            int[] product = new int[aLength + bLength - 1];
            for (int i = 0; i < aLength; i++)
            {
                int aCoeff = aCoefficients[i];
                for (int j = 0; j < bLength; j++)
                {
                    product[i + j] = GenericGf.AddOrSubtract(product[i + j],
                        field.Multiply(aCoeff, bCoefficients[j]));
                }
            }
            return new GenericGfPoly(field, product);
        }

        internal GenericGfPoly Multiply(int scalar)
        {
            if (scalar == 0)
            {
                return field.Zero;
            }
            if (scalar == 1)
            {
                return this;
            }
            int size = coefficients.Length;
            int[] product = new int[size];
            for (int i = 0; i < size; i++)
            {
                product[i] = field.Multiply(coefficients[i], scalar);
            }
            return new GenericGfPoly(field, product);
        }

        internal GenericGfPoly MultiplyByMonomial(int degree, int coefficient)
        {
            if (degree < 0)
            {
                throw new ArgumentException();
            }
            if (coefficient == 0)
            {
                return field.Zero;
            }
            int size = coefficients.Length;
            int[] product = new int[size + degree];
            for (int i = 0; i < size; i++)
            {
                product[i] = field.Multiply(coefficients[i], coefficient);
            }
            return new GenericGfPoly(field, product);
        }

        internal GenericGfPoly[] Divide(GenericGfPoly other)
        {
            if (!field.Equals(other.field))
            {
                throw new ArgumentException("GenericGFPolys do not have same GenericGF field");
            }
            if (other.IsZero)
            {
                throw new ArgumentException("Divide by 0");
            }

            GenericGfPoly quotient = field.Zero;
            GenericGfPoly remainder = this;

            int denominatorLeadingTerm = other.GetCoefficient(other.Degree);
            int inverseDenominatorLeadingTerm = field.Inverse(denominatorLeadingTerm);

            while (remainder.Degree >= other.Degree && !remainder.IsZero)
            {
                int degreeDifference = remainder.Degree - other.Degree;
                int scale = field.Multiply(remainder.GetCoefficient(remainder.Degree), inverseDenominatorLeadingTerm);
                GenericGfPoly term = other.MultiplyByMonomial(degreeDifference, scale);
                GenericGfPoly iterationQuotient = field.BuildMonomial(degreeDifference, scale);
                quotient = quotient.AddOrSubtract(iterationQuotient);
                remainder = remainder.AddOrSubtract(term);
            }

            return new[] { quotient, remainder };
        }

        public override String ToString()
        {
            StringBuilder result = new StringBuilder(8 * Degree);
            for (int degree = Degree; degree >= 0; degree--)
            {
                int coefficient = GetCoefficient(degree);
                if (coefficient != 0)
                {
                    if (coefficient < 0)
                    {
                        result.Append(" - ");
                        coefficient = -coefficient;
                    }
                    else
                    {
                        if (result.Length > 0)
                        {
                            result.Append(" + ");
                        }
                    }
                    if (degree == 0 || coefficient != 1)
                    {
                        int alphaPower = field.Log(coefficient);
                        if (alphaPower == 0)
                        {
                            result.Append('1');
                        }
                        else if (alphaPower == 1)
                        {
                            result.Append('a');
                        }
                        else
                        {
                            result.Append("a^");
                            result.Append(alphaPower);
                        }
                    }
                    if (degree != 0)
                    {
                        if (degree == 1)
                        {
                            result.Append('x');
                        }
                        else
                        {
                            result.Append("x^");
                            result.Append(degree);
                        }
                    }
                }
            }
            return result.ToString();
        }
    }
}