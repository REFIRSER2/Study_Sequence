using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Diff : MonoBehaviour
{
    [SerializeField]private Text left_Text;
    [SerializeField]private Text right_Text;

    #region 유니티 기본 내장 함수 
    private void Awake()
    {
        
    }
    #endregion
    
    #region 비교
    private int checkCount = 0;
    
    public void Compare_OnClick()
    {
        //Compare("GAC", "AGCAT");
        Compare_Start(left_Text.text, right_Text.text);
        //Compare("XMJYAUZ", "MZJAWXU");
    }

    private void Compare_Start(string a, string b)
    {
        //int[,] compare_Arr;
        
        //공백 제거
        a = a.Replace(" ", "");
        //a = a.Replace("\n", "");
        //공백 제거
        b = b.Replace(" ", "");
        //b = b.Replace("\n", "");

        string[] aStr_Arr = a.Split('\n');
        string[] bStr_Arr = b.Split('\n');

        List<int> alreadyA = new List<int>();
        List<int> alreadyB = new List<int>();

        right_Text.text = "";
        
            int aIndex = 0;
            List<int> check = new List<int>();
            Queue<string> aQueue = new Queue<string>();
            Queue<string> bQueue = new Queue<string>();
            foreach (var aStr in aStr_Arr)
            {
                bool isCheck = false;

                int bIndex = 0;
                int min = 0, index = -1;
                string added = "";
                foreach (var bStr in bStr_Arr)
                {
                    if (alreadyB.Contains(bIndex))
                    {
                        bIndex++;
                        continue;
                    }

                    int[,] compare_Arr;
                    compare_Arr = new int[aStr.Length+1,bStr.Length+1]; 
                    
                    int m_Start= 1, m_End = aStr.Length;
                    int n_Start = 1, n_End = bStr.Length;
                    
                    while (m_Start <= m_End)
                    {
                        //Debug.Log("n : " + n_Start);
                        if (aStr[m_Start-1] == bStr[n_Start-1])
                        {
                            //일치하는 문자가 있을시 이전 대각선 값 + 1
                            compare_Arr[m_Start, n_Start] = compare_Arr[m_Start - 1, n_Start - 1] + 1;
                        }
                        else
                        {
                            //일치하는 문자가 없을시 
                            //가로 줄 인덱스가 1일 경우 바로 위 값을 가져옴 
                            //또는 바로 위 값이 바로 옆 값보다 큰 경우 값을 바로 위 값으로 설정함
                            compare_Arr[m_Start, n_Start] = n_Start == 1 || compare_Arr[m_Start - 1, n_Start] >= compare_Arr[m_Start, n_Start - 1] ? 
                                compare_Arr[m_Start - 1, n_Start] : compare_Arr[m_Start, n_Start - 1];
                        }

                        n_Start++;

                        if (n_Start > n_End)
                        {
                            m_Start++;
                            n_Start = 1;
                        }
                    }
                    
                    string diff = Trace(compare_Arr, aStr, bStr);

                    //Debug.Log("diff : " + diff);
                    //Debug.Log("a : " + aStr);
                    //Debug.Log("b : " + bStr);
                    if (diff == bStr && diff.Length == aStr.Length && !check.Contains(bIndex))
                    {
                        isCheck = true;
                        check.Add(bIndex);
                        break;
                    }


                    bIndex++;
                }

                if (isCheck)
                {
                    right_Text.text += aStr + "\n";
                }
                else
                {
                    right_Text.text += "<color=red>- " + aStr + "</color>\n";
                }

                aIndex++;
            }

            for (int i = 0; i < bStr_Arr.Length; i++)
            {
                if (!check.Contains(i))
                {
                    //right_Text.text += "<color=red>- " + aStr_Arr[i] + "</color>\n"; 
                    right_Text.text += "<color=green>+ " + bStr_Arr[i] + "</color>\n"; 
                }
                else
                {
                    //right_Text.text += bStr_Arr[i] + "\n";
                }
            }

    }
    
    private string Trace(int[,] compare_Arr, string a, string b)
    {
        checkCount = 0;
        
        int m_Start = 1, mEnd = a.Length;
        int n_Start = 1, nEnd = b.Length;

        m_Start = mEnd;
        n_Start = nEnd;

        string diff = "";

        List<int> matchM = new List<int>();
        List<int> matchN = new List<int>();

        char[,] checkChar = new char[a.Length+1,b.Length+1];
        while (m_Start > 1 || n_Start >= 1)
        {
            if (m_Start > 0 && compare_Arr[m_Start - 1, n_Start] > compare_Arr[m_Start, n_Start - 1])
            {
                if (m_Start > 1)
                {
                    //Debug.Log("removed : " + m_Start);
                    //removedM.Add(m_Start);
                    m_Start--;
                    n_Start = nEnd;                    
                }
            }
            else
            {
                if (compare_Arr[m_Start, n_Start - 1] != compare_Arr[m_Start, n_Start])
                {
                    matchM.Add(m_Start);
                    matchN.Add(n_Start);
                    if (m_Start > 0)
                    {
                        m_Start--;
                        n_Start--;
                    }
                }
                else
                {
                    if (n_Start >= 1)
                    {
                        n_Start--;
                    }
                }
            }

            if (n_Start <= 1 && m_Start > 1)
            {
                n_Start = b.Length;
                m_Start--;
            }

            checkCount++;
        }

        checkCount += (Mathf.Abs(a.Length - b.Length));

        if (compare_Arr[mEnd, nEnd] == 0)
        {
            checkCount = -1;
        }

        string check = "";

        foreach (var c in checkChar)
        {
            check += c + " ";
        }

        if (a.Length >= b.Length)
        {
            for (int i = 1; i <= a.Length; i++)
            {
                if (matchM.Contains(i))
                {
                    diff += a[i-1]; 
                }
            }
        }
        else
        {
            for (int i = 1; i <= b.Length; i++)
            {
                if (matchN.Contains(i))
                {
                    diff += b[i-1];
                }
            }               
        }

        return diff;
    }

    #endregion
}
