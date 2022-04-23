using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominoC
{
    class MSPlayer
    {
        static public string PlayerName = "Балбес";
        static private List<MTable.SBone> lHand;


        //=== Готовые функции =================
        // инициализация игрока
        static public void Initialize()
        {
            lHand = new List<MTable.SBone>();
        }

        // Вывод на экран
        static public void PrintAll()
        { MTable.PrintAll(lHand); }

        // дать количество доминушек
        static public int GetCount()
        { return lHand.Count; }

        //=== Функции для разработки =================
        // добавить доминушку в свою руку
        static public void AddItem(MTable.SBone sb)
        {
            lHand.Add(sb);
        }

        // дать сумму очков на руке
        static public int GetScore()
        {
            int sum = 0;
            foreach (MTable.SBone sb in lHand)
            {
                if (sb.First == 0 && sb.Second == 0)
                    sum += 25;
                else
                    sum += sb.First + sb.Second;
            }
            return sum;
        }

        //***********************************************************************
        // сделать ход
        //***********************************************************************
        static public bool MakeStep(out MTable.SBone sb, out bool End)
        {
            int maxSore = 0;
            bool locatedFinish = false;
            sb.First = 7; sb.Second = 7;
            End = false;

            // Возвращает информацию о текущем раскладе на столе
            List<MTable.SBone> lgame = MTable.GetGameCollection();
            int firstly = lgame[0].First;
            int last = lgame[lgame.Count - 1].Second;

            for (int i = 0; i < lHand.Count; i++)
            {
                bool check = CheckCard(lHand[i], ref locatedFinish, firstly, last) && GetMaxCard(lHand[i], ref maxSore);

                if (check)
                {
                    sb = lHand[i];
                    End = locatedFinish;
                    if (maxSore == 25)
                    {
                        lHand.Remove(sb);
                        return true;
                    }
                }
                locatedFinish = false;
            }

            if (maxSore > 0)
            {
                lHand.Remove(sb);
                return true;
            }

            if (TakeFromShop(ref sb, ref End, firstly, last))
                return true;
            return false;
        }

        //***********************************************************************
        // Получение случайной доминошки (sb) из базара
        // Возвращает FALSE, если базар пустой 
        //***********************************************************************
        static public bool TakeFromShop(ref MTable.SBone sb, ref bool End, int firstly, int last)
        {
            while (true)
            {
                if (MTable.GetFromShop(out sb))
                {
                    if (CheckCard(sb, ref End, firstly, last))
                        return true;
                    else
                        lHand.Add(sb);
                }
                else
                    break;
            }
            return false;
        }

        //***********************************************************************
        // Проверить, удовлетворяет ли доминошка, чтобы положить на стол
        //***********************************************************************
        static public bool CheckCard(MTable.SBone sb, ref bool End, int firstly, int last)
        {
            if (sb.First == last || sb.Second == last)
            {
                End = true;
                return true;
            }
            else if (sb.First == firstly || sb.Second == firstly)
            {
                return true;
            }
            else
                return false;
        }

        //***********************************************************************
        // Найти карту с наибольшим количеством очков, если TRUE
        //***********************************************************************
        static public bool GetMaxCard(MTable.SBone sb, ref int maxScore)
        {
            if (sb.First == 0 && sb.Second == 0)
            {
                maxScore = 25;
                return true;
            }
            int score = sb.First + sb.Second;
            if (score > maxScore)
            {
                maxScore = score;
                return true;
            }
            return false;
        }
    }
}
