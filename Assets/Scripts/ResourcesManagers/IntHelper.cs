using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public static class IntHelper
{
    public static string numStr(double num)
    {
        int log10 = (int)Math.Log10(Math.Abs(num));
        if(log10 < -27)
            return "0.000";
        if(log10 % -3 < 0 )
            log10 -= 3;
        int log1000 = Math.Max(-8, Math.Min(log10 / 3, 8));
        double scaledNum = (num / Math.Pow(10, log1000 * 3));
        // Si log1000 es 0, no se agrega prefijo
        if (log1000 == 0 )
        {
            return scaledNum.ToString("##");
        }else if (log1000 == 1)
        {
            //string thousandValue = scaledNum.ToString("N6");
            //Debug.Log("KW " + thousandValue);
            // var aux = thousandValue[5];
            // thousandValue = thousandValue.Remove(4, 1);
            // thousandValue += "." + aux;
            string thousandValue = num.ToString("#,###", CultureInfo.InvariantCulture);
            return thousandValue;
            //return scaledNum.ToString("N6");
        }
        else
        {
            return scaledNum.ToString("###") + prefixeSI[log1000+8]; 
        }
    }
    
    static string[] prefixeSI =
    {
        "y", "z", "a", "f","p", "n", "µ", "m", "", " K", " M", " B", " T", " Qa", " Qi", " Sx", " Sp", " O", " N",
        " D", " Ud", " Dd", " Td", " qd", " sd", " Od", " Nd" , " V"
    };

    public static string CalculateUpgradeCostStr(int level, int baseCost, float upgradeCostMultiplier)
    {
        return numStr((long) (baseCost * Mathf.Pow(upgradeCostMultiplier, level)));
    }
    public static long CalculateUpgradeCost(int level, int baseCost, float upgradeCostMultiplier)
    {
        return (long) (baseCost * Mathf.Pow(upgradeCostMultiplier, level));
    } 

}