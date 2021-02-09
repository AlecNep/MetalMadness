using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickFunctions : MonoBehaviour
{
   
    public static int CountBits(byte n)
    {
        int count = 0;
        while (n != 0)
        {
            count++;
            n &= (byte)(n - 1);
        }
        return count;
    }

    public static int CountBits(int n)
    {
        int count = 0;
        while (n != 0)
        {
            count++;
            n &= (n - 1);
        }
        return count;
    }


}
