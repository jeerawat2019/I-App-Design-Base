using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCore.Comp.XFunc
{
    public class MatrixMath
    {
        public static void LinearRegression(double[] responses, double[,] inputs, ref double[] coef )
        {
            int nVars = coef.Length;
            int nData = responses.Length;

            // Row is eq#, col is var
            double[,] A = new double[nVars, nVars];

            A[0,0] = nData;
            for(int iVar=0; iVar<nVars-1; iVar++)
            {
                double varSum1 = 0;
                for(int d=0; d<nData; d++)
                {
                    varSum1 += inputs[iVar,d];
                }
                A[0,iVar+1] = varSum1;
            }
            double resSum1 = 0;
            for(int d=0; d<nData; d++)
            {
                resSum1 += responses[d];
            }
            
            coef[0] = resSum1;

            for (int eq = 1; eq < nVars; eq++)
            {
                int nVar = eq - 1;
                // Sum of all this var
                double sum0 = 0;
                for (int d = 0; d < nData; d++)
                {
                    sum0 += inputs[nVar, d];
                }
                A[eq, 0] = sum0;

                // Sum of this var*1st var
                for (int iVar = 0; iVar < nVars - 1; iVar++)
                {
                    double sum = 0;
                    for (int d = 0; d < nData; d++)
                    {
                        sum += inputs[eq-1, d] * inputs[iVar, d];
                    }
                    A[eq, iVar+1] = sum;
                }
                double resSum2 = 0;
                for (int d = 0; d < nData; d++)
                {
                    resSum2 += responses[d] * inputs[eq - 1, d];
                }
                coef[eq] = resSum2;
            }

            MatrixMath.FindCoefficients(A, ref coef, nVars);

        }

        private static void FindCoefficients(double[,] A, ref double[] BVect, int nCount)
        {
            int[] pvtIdx;
            double Factor;

            pvtIdx = new int[nCount];
            Factor = 1;
            LuDecomp(ref A, nCount, ref pvtIdx, ref Factor);
            LuBackSubst(ref A, nCount, ref pvtIdx, ref BVect);
        }

        private static void LuDecomp(ref double[,] A, int nNumRows, ref int[] pivotIndex, ref double Factor)
        {
            const double Epsilon = 1.0e-20;
            double[] VV;
            int iMax, i, j, k;
            double big, dum, fSum, temp;

            iMax = 0;

            VV = new double[nNumRows];
            Factor = 1.0;
            for (i = 0; i < nNumRows; i++)
            {
                big = 0.0;
                for (j = 0; j < nNumRows; j++)
                {
                    temp = Math.Abs(A[i, j]);
                    if (temp > big) //ToDo: Unsupported feature: assignment within expression.
                    {
                        big = temp;
                    }
                }
                if (big == 0.0)
                {
                    // ReportError("Singular matrix", "LuDecomp");
                    return;
                }
                VV[i] = 1.0 / big;
            }
            for (j = 0; j < nNumRows; j++)
            {
                for (i = 0; i < j; i++)
                {
                    fSum = A[i, j];
                    for (k = 0; k < i; k++)
                    {
                        fSum -= A[i, k] * A[k, j];
                    }
                    A[i, j] = fSum;
                }
                big = 0.0;
                for (i = j; i < nNumRows; i++)
                {
                    fSum = A[i, j];
                    for (k = 0; k < j; k++)
                    {
                        fSum -= A[i, k] * A[k, j];
                    }
                    A[i, j] = fSum;
                    dum = VV[i] * Math.Abs(fSum);
                    if (dum >= big)
                    {
                        big = dum;
                        iMax = i;
                    }
                }
                if (j != iMax)
                {
                    for (k = 0; k < nNumRows; k++)
                    {
                        dum = A[iMax, k];
                        A[iMax, k] = A[j, k];
                        A[j, k] = dum;
                    }
                    Factor = -Factor;
                    VV[iMax] = VV[j];
                }
                pivotIndex[j] = iMax;
                if (A[j, j] == 0.0)
                {
                    A[j, j] = Epsilon;
                }
                if (j != nNumRows - 1)
                {
                    dum = 1.0 / A[j, j];
                    for (i = j + 1; i < nNumRows; i++)
                    {
                        A[i, j] *= dum;
                    }
                }
            }
        }

        private static void LuBackSubst(ref double[,] A, int nNumRows, ref int[] pivotIndex, ref double[] BVect)
        {
            int i1, i, j, ip;
            double fSum;

            i1 = 0;
            for (i = 0; i < nNumRows; i++)
            {
                ip = pivotIndex[i];
                fSum = BVect[ip];
                BVect[ip] = BVect[i];
                if (i1 != 0)
                {
                    for (j = i1 - 1; j < i; j++)
                    {
                        fSum -= A[i, j] * BVect[j];
                    }
                }
                else if (fSum != 0.0)
                {
                    i1 = i + 1;
                }
                BVect[i] = fSum;
            }
            for (i = nNumRows - 1; i >= 0; i--)
            {
                fSum = BVect[i];
                for (j = i + 1; j < nNumRows; j++)
                {
                    fSum -= A[i, j] * BVect[j];
                }
                BVect[i] = fSum / A[i, i];
            }
        }
    }
}
