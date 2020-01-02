using System;
using System.Collections.Generic;

namespace Playing_cards
{
    public class Class1
    {
        
        public int[] PaiXu(int[] card, int number = 0)
        {
            if (number == 0) { number = card.Length; }
            if (card.Length == 0) { return card; }

            // ==========  根据牌值进行排序  ===============
            int temp = 0;
            for (int i = 0; i < card.Length; i++)  //冒泡排序... 从大到小
            {
                for (int j = 0; j < card.Length - 1 - i; j++)
                {
                    if (card[j] < card[j + 1])
                    {
                        temp = card[j];
                        card[j] = card[j + 1];
                        card[j + 1] = temp;
                    }
                }
            }

            List<int> hei = new List<int>();
            List<int> hong = new List<int>();
            List<int> mei = new List<int>();
            List<int> fang = new List<int>();

            List<int> wang = new List<int>();

            for (int i = 0; i < card.Length; i++)
            {
                
                switch (sendFlower(card[i]))
                {
                    case 3: //黑桃                   
                        hei.Add(card[i]);
                        break;
                    case 2: //红桃                  
                        hong.Add(card[i]);
                        break;
                    case 1: //梅花                    
                        mei.Add(card[i]);
                        break;
                    case 0: //方片                    
                        fang.Add(card[i]);
                        break;
                    case 4: //小王
                    case 5: //大王
                        wang.Add(card[i]);
                        break;
                }
                
            }

            QuA(hei);  // 对A 的单独处理
            QuA(hong);
            QuA(mei);
            QuA(fang);

            #region ========== 合并 排序后的牌组========

            List<int> cardlist = new List<int>();
            for (int i = 0; i < wang.Count; i++)    //王
            {
                cardlist.Add(wang[i]);
            }

            
            List<int> cardtemp = new List<int>();

            cardtemp = PaiXuZuPin(hei, hong, mei, fang);
            for (int i = 0; i < cardtemp.Count; i++)
            {
                cardlist.Add(cardtemp[i]);
            }

            int[] cards = new int[cardlist.Count];
            for (int i = 0; i < cardlist.Count; i++)
            {
                cards[i] = cardlist[i];
            }

            #endregion
            return cards;

        }

        /// <summary>
        /// 取A   -- 把每个花色牌中的A，放到前面
        /// </summary>
        /// <param name="hei">花色牌</param> 
        void QuA(List<int> hei)
        {
            if (hei.Count == 0) return;

            List<int> cardlist = new List<int>();

            for (int i = 0; i < hei.Count; i++)  // 将牌添加到新列表
            {
                cardlist.Add(hei[i]);
            }
            if (hei.Count > 2)
            {
                if (hei[hei.Count - 2] % 13 == 1)    //如果有两个A (对两幅牌的处理)
                {
                    cardlist.Insert(0, hei[hei.Count - 2]);
                    cardlist.Insert(0, hei[hei.Count - 1]);
                    for (int i = 0; i < hei.Count; i++)
                    {
                        hei[i] = cardlist[i];
                    }
                    return;
                }
            }

            if (hei[hei.Count - 1] % 13 == 1)    //如果有一个A
            {
                cardlist.Insert(0, hei[hei.Count - 1]);
            }

            for (int i = 0; i < hei.Count; i++)
            {
                hei[i] = cardlist[i];
            }
        }

        /// <summary>
        ///  根据传入牌组 的顺序 进行组拼  
        /// </summary>
        public List<int> PaiXuZuPin(List<int> one, List<int> two, List<int> three, List<int> four)
        {

            List<int> cardlist = new List<int>();

            for (int i = 0; i < one.Count; i++)
            {
                cardlist.Add(one[i]);
            }
            for (int i = 0; i < two.Count; i++)
            {
                cardlist.Add(two[i]);
            }
            for (int i = 0; i < three.Count; i++)
            {
                cardlist.Add(three[i]);
            }

            for (int i = 0; i < four.Count; i++)
            {
                cardlist.Add(four[i]);
            }
            return cardlist;
        }

        /// <summary>
        /// 根据牌值取花色 5:大王 | 4:小王 | 3:黑桃 | 2:红桃 | 1:梅花 | 0:方片 
        /// </summary>
        /// <param name="card"></param>
        public int sendFlower(int card)
        {
            if (card >= 1 && card <= 13)
            {
                return 3;
            }
            else if (card >= 14 && card <= 26)
            {
                return 2;
            }
            else if (card >= 27 && card <= 39)
            {
                return 1;
            }
            else if (card >= 40 && card <= 52)
            {
                return 0;
            }
            else if (card == 53)
            {
                return 4;
            }
            return 5;
        }




    }
}
