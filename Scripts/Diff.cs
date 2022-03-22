using System;
using System.Collections;
using System.Collections.Generic;
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

        int aIndex = 0;
        
        foreach (var aStr in aStr_Arr)
        {
            int bIndex = 0;
            foreach (var bStr in bStr_Arr)
            {
                if (alreadyA.Contains(aIndex))
                {
                    continue;
                }

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
                    if (aStr[m_Start-1] == bStr[n_Start-1])
                    {
                        //일치하는 문자가 있을시 이전 대각선 값 + 1
                        compare_Arr[m_Start, n_Start] = compare_Arr[m_Start - 1, n_Start - 1] + 1;
                    }
                    else
                    {
                        //Debug.Log(m_Start);
                        //Debug.Log(n_Start);
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

                if (compare_Arr[m_End, n_End] == aStr.Length && compare_Arr[m_End,n_End] == bStr.Length)
                {
                    alreadyA.Add(aIndex);
                    alreadyB.Add(bIndex);
                    
                    //Debug.Log("Already : " + bStr_Arr[bIndex]);
                }

                /*
                string debugStr = "";

                debugStr += "   - ";
        
                for (int n = 0; n < bStr.Length; n++)
                {
                    debugStr += bStr[n] + " ";
                }

                debugStr += "\n";
        
                for(int m=0;m<=aStr.Length;m++)
                {
                    if (m == 0)
                    {
                        debugStr += "- ";
                    }
                    else
                    {
                        debugStr += aStr[m-1] + " ";
                    }

            
                    for (int n = 0; n <= bStr.Length; n++)
                    {
                        debugStr += compare_Arr[m, n] + " ";
                    }

                    debugStr += "\n";
                }
                Debug.Log(debugStr); */
                bIndex++;
            }

            aIndex++;
        }

        left_Text.text = "";
        right_Text.text = "";
        
        for (int i = 0; i < aStr_Arr.Length; i++)
        {
            if (!alreadyA.Contains(i))
            {
                aStr_Arr[i] = "<color=red>" + aStr_Arr[i] + "</color>";
            }
            
            left_Text.text += aStr_Arr[i] + "\n";
        }
        
        for (int i = 0; i < bStr_Arr.Length; i++)
        {
            if (!alreadyB.Contains(i))
            {
                bStr_Arr[i] = "<color=green>" + bStr_Arr[i] + "</color>";
                //Debug.Log("Already : " + bStr_Arr[i]);
            }

            right_Text.text += bStr_Arr[i] + "\n";
        }

        
    }

    private void Compare(string a, string b)
    {
        
    }

        /*   
        compare_Arr = new int[a.Length+1,b.Length+1]; 
        //Index 0 값을 따로 설정을 안해 줌으로써 Value는 0로 셋.

        int m_Start= 1, m_End = a.Length;
        int n_Start = 1, n_End = b.Length;

        while (m_Start <= m_End)
        {
            if (a[m_Start-1] == b[n_Start-1])
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

                ///*if (m_Start == m_End)
                {
                    Debug.Log("End");
                }
                
                if (!check)
                {
                    compare_Arr[m_Start, n_Start] = compare_Arr[m_Start - 1, n_Start]; 
                }
                else
                {
                    compare_Arr[m_Start, n_Start] = compare_Arr[m_Start, n_Start - 1];
                }//마무리

                ///*
                if (m_Start == m_End)
                {
                    if (n_Start == 1)
                    {
                        compare_Arr[m_Start, n_Start] = compare_Arr[m_Start-1, n_Start];  
                    }
                    else
                    {
                        compare_Arr[m_Start, n_Start] = compare_Arr[m_Start, n_Start - 1];
                    }
                }
                else
                {
                    compare_Arr[m_Start, n_Start] = compare_Arr[m_Start, n_Start - 1];
                }
                //마무리
            }

            n_Start++;

            if (n_Start > n_End)
            {
                m_Start++;
                n_Start = 1;
            }
        }


        //Debug.Log(compare_Arr[m_Start - 1, n_Start]);
        //Debug.Log(compare_Arr[m_Start, n_Start - 1]);
        BackCheck(a,b);

        string debugStr = "";

        debugStr += "   - ";
        
        for (int n = 0; n < b.Length; n++)
        {
            debugStr += b[n] + " ";
        }

        debugStr += "\n";
        
        for(int m=0;m<=a.Length;m++)
        {
            if (m == 0)
            {
                debugStr += "- ";
            }
            else
            {
                debugStr += a[m-1] + " ";
            }

            
            for (int n = 0; n <= b.Length; n++)
            {
                debugStr += compare_Arr[m, n] + " ";
            }

            debugStr += "\n";
        }
        Debug.Log(debugStr);
        
        //Debug.Log(Trace(a,b));
        */
    
    /*private string BackCheck(string a, string b)
    {
        int m_Start= 1, m_End = a.Length;
        int n_Start = 1, n_End = b.Length;

        List<int> findIndexs = new List<int>();
        
        m_Start = m_End;
        n_Start = n_End;
        
        while (m_Start > 0 || n_Start > 0)
        {
            if (m_Start >= 1 && n_Start >= 1 && compare_Arr[m_Start - 1, n_Start] >= compare_Arr[m_Start, n_Start - 1])
            {
                //Debug.Log(a[m_Start-1]);
                findIndexs.Add(n_Start);
                m_Start--;
            }
            else
            {
                if (n_Start >= 1)
                {
                    n_Start--;
                    continue;
                }
                else
                {
                    m_Start--;
                    n_Start = n_End;                    
                }
            }
        }
        
        string traceStr = "";
        
        for (int i=findIndexs.Count-1;i>0;i--)
        {
            traceStr += b[findIndexs[i] - 1];
            //Debug.Log();
        }
        Debug.Log(traceStr);
        

        return traceStr;
    }*/
    #endregion
}
