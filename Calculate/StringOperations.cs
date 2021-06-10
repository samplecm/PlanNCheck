using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using Plan_n_Check.Plans;

namespace Plan_n_Check.Calculate
{
    public static class StringOperations
    {
        public static int FindPTVNumber(string name)
        {
            //sometimes there is the PTV number (2 digits) and then also a number for listing structures, so need to separate these and take the larger
            List<string> number = new List<string>();
            string num = "";
            bool isDigit = false;
            int index = 0;
            for (int i = 0; i < name.Length; i++)
            {
                if (Char.IsDigit(name[i]))
                {
                    num += name[i];
                    isDigit = true;
                }
                else
                {
                    if (isDigit)
                    {
                        number.Add(num);
                        isDigit = false;
                        num = "";
                        index++;
                    }
                }

            }
            if (num != "")
            {
                number.Add(num);
            }
            //now find the largest number
            int longest = 0;
            if (number.Count > 0)
            {
                for (int ind = 0; ind < number.Count; ind++)
                {
                    if (Convert.ToInt32(number[ind]) > longest)
                    {
                        longest = Convert.ToInt32(number[ind]);
                    }
                }

                if (longest > 1000) //If for some reason it's in cGy convert to Gy
                {
                    longest /= 100;
                }
                return longest;

            }
            else
            {

                return 0; //error 0 means that no number was found with the ptv name.
            }
        }

        public static int StringDistance(string firstText, string secondText) //Finding out how close strings are (Damerau Levenshtein)
        {
            var n = firstText.Length + 1;
            var m = secondText.Length + 1;
            var arrayD = new int[n, m];

            for (var i = 0; i < n; i++)
            {
                arrayD[i, 0] = i;
            }

            for (var j = 0; j < m; j++)
            {
                arrayD[0, j] = j;
            }

            for (var i = 1; i < n; i++)
            {
                for (var j = 1; j < m; j++)
                {
                    var cost = firstText[i - 1] == secondText[j - 1] ? 0 : 1;

                    arrayD[i, j] = Minimum(arrayD[i - 1, j] + 1, // delete
                                                            arrayD[i, j - 1] + 1, // insert
                                                            arrayD[i - 1, j - 1] + cost); // replacement

                    if (i > 1 && j > 1
                       && firstText[i - 1] == secondText[j - 2]
                       && firstText[i - 2] == secondText[j - 1])
                    {
                        arrayD[i, j] = Minimum(arrayD[i, j],
                        arrayD[i - 2, j - 2] + cost); // permutation
                    }
                }
            }

            return arrayD[n - 1, m - 1];
        }

        static int Minimum(int a, int b)
        {
            if (a < b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        static int Minimum(int a, int b, int c)
        {
            if ((a < b) && (a < c))
            {
                return a;
            }
            else if (b < c)
            {
                return b;
            }
            else
            {
                return c;
            }
        }

        public static int LongestSubstring(string X, string Y) //Longest common substring
        {
            int m = X.Length;
            int n = Y.Length;
            // Create an array to store lengths of  
            // longest common suffixes of substrings. 

            int[,] LCStuff = new int[m + 1, n + 1];

            // To store length of the longest common 
            // substring 
            int result = 0;

            // Following steps build LCSuff[m+1][n+1]  
            // in bottom up fashion 
            for (int i = 0; i <= m; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    if (i == 0 || j == 0)
                        LCStuff[i, j] = 0;
                    else if (Char.ToLower(X[i - 1]) == Char.ToLower(Y[j - 1]))
                    {
                        LCStuff[i, j] =
                                LCStuff[i - 1, j - 1] + 1;

                        result = Math.Max(result,
                                          LCStuff[i, j]);
                    }
                    else
                        LCStuff[i, j] = 0;
                }
            }

            return result;
        }
        public static List<Structure> AssignStructure(StructureSet ss, ROI roi) //return a list containing a list of structures matching the ROI for each constrained ROI
        {
            string name = roi.Name;
            List<Structure> dicomStructures = new List<Structure>();
            //Now need to find out which structure matches with the constraint ROI.
            //Now compare all structures with ROI using Damershau Lamershtein test for similarity. For PTVs, have to be more careful though since there can be variable amounts of 
            //different kinds. 

            if (!name.ToLower().Contains("ptv")) //If not PTV 
            {
                //First filter out structures without a substring of at least 3
                List<Structure> filteredStructures = new List<Structure>();
                foreach (Structure structure in ss.Structures)
                {
                    string structureName = structure.Name; //get name of structure in eclipse
                    bool valid = true;
                    if (StringOperations.LongestSubstring(structureName, name) < 3)
                    {
                        valid = false;
                    }
                    else if (!AllowedToMatch(structureName, name))
                    {
                        valid = false;
                    }
                    if (valid)
                    {
                        filteredStructures.Add(structure);
                    }
                }
                int closeness = 0; //structure giving smallest will be the match.
                int closest = 100;
                foreach (Structure structure in filteredStructures)
                {
                    string structureName = structure.Name;
                    //Now test string closeness
                    List<string> closestStrings = new List<string>();
                    List<int> closestInts = new List<int>();


                    closeness = StringOperations.StringDistance(structureName, name);
                    if (closeness < closest)
                    {
                        dicomStructures.Clear();
                        dicomStructures.Add(structure);
                        closest = closeness;
                    }
                    else if (closeness == closest)  //add it to the list if its equal in closeness to other. 
                    {
                        dicomStructures.Add(structure);
                    }

                }

            }
            else //It is a PTV:
            {
                //Get the type of PTV 
                int PTV_Type = StringOperations.FindPTVNumber(name);
                if (PTV_Type == 0)
                {

                }
                else  //if PTV has a number
                {
                    foreach (Structure structure in ss.Structures)
                    {

                        if ((structure.Name.ToLower().Contains(PTV_Type.ToString())) && (structure.Name.ToLower().Contains("ptv")))
                        {
                            dicomStructures.Add(structure);
                        }

                    }
                }

            }
            return dicomStructures;
        }

        public static bool AllowedToMatch(string s1, string s2) //disqualify structures from matching if they don't have special words in common. 
        {
            bool allowed = true;
            List<string> keywords = new List<string>();
            keywords.Add("prv");
            keywords.Add("ptv");
            keywords.Add("stem");
            keywords.Add("cord");
            keywords.Add("chi");
            keywords.Add("opt");
            keywords.Add("oral");
            keywords.Add("nerv");
            keywords.Add("par");
            keywords.Add("globe");
            keywords.Add("lip");
            keywords.Add("cav");
            keywords.Add("sub");
            keywords.Add("test");
            keywords.Add("fact");
            int num;
            for (int i = 0; i < keywords.Count; i++)
            {
                num = 0;
                if (s1.ToLower().Contains(keywords[i]))
                {
                    num += 1;
                }
                if (s2.ToLower().Contains(keywords[i]))
                {
                    num += 1;
                }
                if (num == 1)
                {
                    allowed = false;
                }
            }
            //Also can't have left and no L in another. Or right and no R.
            if ((s1.ToLower().Contains("left")) || (s2.ToLower().Contains("left")))
            {
                num = 0;
                if (s1.ToLower().Contains("l"))
                {
                    num += 1;
                }
                if (s2.ToLower().Contains("l"))
                {
                    num += 1;
                }
                if (num == 1)
                {
                    allowed = false;
                }
            }
            if ((s1.ToLower().Contains("right")) || (s2.ToLower().Contains("right")))
            {
                num = 0;
                if (s1.ToLower().Contains("r"))
                {
                    num += 1;
                }
                if (s2.ToLower().Contains("r"))
                {
                    num += 1;
                }
                if (num == 1)
                {
                    allowed = false;
                }
            }
            //also an issue if has _L_ or " L " or " L-" or "left" in only one. 
            if ((s1.ToLower().Contains("l sub")) || (s1.ToLower().Contains("l par")) || (s1.ToLower().Contains("lpar")) || (s1.ToLower().Contains("lsub")) || (s1.ToLower().Contains("_l_")) || (s1.ToLower().Contains(" l ")) || (s1.ToLower().Contains(" l-")) || (s1.ToLower().Contains("-l-")) || (s1.ToLower().Contains(" l_")) || (s1.ToLower().Contains("_l ")) || (s1.ToLower().Contains("-l ")) || (s1.ToLower().Contains("left")) || (s1.ToLower().StartsWith("l ")) || (s1.ToLower().Contains("_lt_")) || (s1.ToLower().Contains(" lt ")) || (s1.ToLower().Contains(" lt-")) || (s1.ToLower().Contains("-lt-")) || (s1.ToLower().Contains(" lt_")) || (s1.ToLower().Contains("_lt ")) || (s1.ToLower().Contains("-lt ")) || (s1.ToLower().Contains("left")) || (s1.ToLower().StartsWith("lt ")) || (s1.ToLower().StartsWith("lt ")))
            {
                if (!((s2.ToLower().Contains("l sub")) || (s2.ToLower().Contains("l par")) || (s2.ToLower().Contains("lpar")) || (s2.ToLower().Contains("lsub")) || (s2.ToLower().Contains("_l_")) || (s2.ToLower().Contains(" l ")) || (s2.ToLower().Contains(" l-")) || (s2.ToLower().Contains("-l-")) || (s2.ToLower().Contains(" l_")) || (s2.ToLower().Contains("_l ")) || (s2.ToLower().Contains("-l ")) || (s2.ToLower().Contains("left")) || (s2.ToLower().StartsWith("l ")) || (s2.ToLower().Contains("_lt_")) || (s2.ToLower().Contains(" lt ")) || (s2.ToLower().Contains(" lt-")) || (s2.ToLower().Contains("-lt-")) || (s2.ToLower().Contains(" lt_")) || (s2.ToLower().Contains("_lt ")) || (s2.ToLower().Contains("-lt ")) || (s2.ToLower().Contains("left")) || (s2.ToLower().StartsWith("lt ")) || (s2.ToLower().StartsWith("lt "))))
                {
                    allowed = false;
                }
            }
            if ((s2.ToLower().Contains("l sub")) || (s2.ToLower().Contains("l par")) || (s2.ToLower().Contains("lpar")) || (s2.ToLower().Contains("lsub")) || (s2.ToLower().Contains("_l_")) || (s2.ToLower().Contains(" l ")) || (s2.ToLower().Contains(" l-")) || (s2.ToLower().Contains("-l-")) || (s2.ToLower().Contains(" l_")) || (s2.ToLower().Contains("_l ")) || (s2.ToLower().Contains("-l ")) || (s2.ToLower().Contains("left")) || (s2.ToLower().StartsWith("l ")) || (s2.ToLower().Contains("_lt_")) || (s2.ToLower().Contains(" lt ")) || (s2.ToLower().Contains(" lt-")) || (s2.ToLower().Contains("-lt-")) || (s2.ToLower().Contains(" lt_")) || (s2.ToLower().Contains("_lt ")) || (s2.ToLower().Contains("-lt ")) || (s2.ToLower().Contains("left")) || (s2.ToLower().StartsWith("lt ")) || (s2.ToLower().StartsWith("lt ")))
            {
                if (!((s1.ToLower().Contains("l sub")) || (s1.ToLower().Contains("l par")) || (s1.ToLower().Contains("lpar")) || (s1.ToLower().Contains("lsub")) || (s1.ToLower().Contains("_l_")) || (s1.ToLower().Contains(" l ")) || (s1.ToLower().Contains(" l-")) || (s1.ToLower().Contains("-l-")) || (s1.ToLower().Contains(" l_")) || (s1.ToLower().Contains("_l ")) || (s1.ToLower().Contains("-l ")) || (s1.ToLower().Contains("left")) || (s1.ToLower().StartsWith("l ")) || (s1.ToLower().Contains("_lt_")) || (s1.ToLower().Contains(" lt ")) || (s1.ToLower().Contains(" lt-")) || (s1.ToLower().Contains("-lt-")) || (s1.ToLower().Contains(" lt_")) || (s1.ToLower().Contains("_lt ")) || (s1.ToLower().Contains("-lt ")) || (s1.ToLower().Contains("left")) || (s1.ToLower().StartsWith("lt ")) || (s1.ToLower().StartsWith("lt "))))
                {
                    allowed = false;
                }
            }
            if ((s1.ToLower().Contains("r sub")) || (s1.ToLower().Contains("r par")) || (s1.ToLower().Contains("rpar")) || (s1.ToLower().Contains("rsub")) || (s1.ToLower().Contains("_r_")) || (s1.ToLower().Contains(" r ")) || (s1.ToLower().Contains(" r-")) || (s1.ToLower().Contains("-r-")) || (s1.ToLower().Contains(" r_")) || (s1.ToLower().Contains("_r ")) || (s1.ToLower().Contains("-r ")) || (s1.ToLower().Contains("right")) || (s1.ToLower().StartsWith("r ")) || (s1.ToLower().Contains("_rt_")) || (s1.ToLower().Contains(" rt ")) || (s1.ToLower().Contains(" rt-")) || (s1.ToLower().Contains("-rt-")) || (s1.ToLower().Contains(" rt_")) || (s1.ToLower().Contains("_rt ")) || (s1.ToLower().Contains("-rt ")) || (s1.ToLower().Contains("right")) || (s1.ToLower().StartsWith("rt ")) || (s1.ToLower().StartsWith("rt ")))
            {
                if (!((s2.ToLower().Contains("r sub")) || (s2.ToLower().Contains("r par")) || (s2.ToLower().Contains("rpar")) || (s2.ToLower().Contains("rsub")) || (s2.ToLower().Contains("_r_")) || (s2.ToLower().Contains(" r ")) || (s2.ToLower().Contains(" r-")) || (s2.ToLower().Contains("-r-")) || (s2.ToLower().Contains(" r_")) || (s2.ToLower().Contains("_r ")) || (s2.ToLower().Contains("-r ")) || (s2.ToLower().Contains("right")) || (s2.ToLower().StartsWith("r ")) || (s2.ToLower().Contains("_rt_")) || (s2.ToLower().Contains(" rt ")) || (s2.ToLower().Contains(" rt-")) || (s2.ToLower().Contains("-rt-")) || (s2.ToLower().Contains(" rt_")) || (s2.ToLower().Contains("_rt ")) || (s2.ToLower().Contains("-rt ")) || (s2.ToLower().Contains("right")) || (s2.ToLower().StartsWith("rt ")) || (s2.ToLower().StartsWith("rt "))))
                {
                    allowed = false;
                }
            }
            if ((s2.ToLower().Contains("r sub")) || (s2.ToLower().Contains("r par")) || (s2.ToLower().Contains("rpar")) || (s2.ToLower().Contains("rsub")) || (s2.ToLower().Contains("_r_")) || (s2.ToLower().Contains(" r ")) || (s2.ToLower().Contains(" r-")) || (s2.ToLower().Contains("-r-")) || (s2.ToLower().Contains(" r_")) || (s2.ToLower().Contains("_r ")) || (s2.ToLower().Contains("-r ")) || (s2.ToLower().Contains("right")) || (s2.ToLower().StartsWith("r ")) || (s2.ToLower().Contains("_rt_")) || (s2.ToLower().Contains(" rt ")) || (s2.ToLower().Contains(" rt-")) || (s2.ToLower().Contains("-rt-")) || (s2.ToLower().Contains(" rt_")) || (s2.ToLower().Contains("_rt ")) || (s2.ToLower().Contains("-rt ")) || (s2.ToLower().Contains("right")) || (s2.ToLower().StartsWith("rt ")) || (s2.ToLower().StartsWith("rt ")))
            {
                if (!((s1.ToLower().Contains("r sub")) || (s1.ToLower().Contains("r par")) || (s1.ToLower().Contains("rpar")) || (s1.ToLower().Contains("rsub")) || (s1.ToLower().Contains("_r_")) || (s1.ToLower().Contains(" r ")) || (s1.ToLower().Contains(" r-")) || (s1.ToLower().Contains("-r-")) || (s1.ToLower().Contains(" r_")) || (s1.ToLower().Contains("_r ")) || (s1.ToLower().Contains("-r ")) || (s1.ToLower().Contains("right")) || (s1.ToLower().StartsWith("r ")) || (s1.ToLower().Contains("_rt_")) || (s1.ToLower().Contains(" rt ")) || (s1.ToLower().Contains(" rt-")) || (s1.ToLower().Contains("-rt-")) || (s1.ToLower().Contains(" rt_")) || (s1.ToLower().Contains("_rt ")) || (s1.ToLower().Contains("-rt ")) || (s1.ToLower().Contains("right")) || (s1.ToLower().StartsWith("rt ")) || (s1.ToLower().StartsWith("rt "))))
                {
                    allowed = false;
                }
            }


            //parotids are tricky, so basically if it has par, and an L its left, and if no L its right.
            if ((s1.ToLower().Contains("par")) && (s2.ToLower().Contains("par")))
            {
                if (((s1.ToLower().Contains("l")) && !(s2.ToLower().Contains("l"))) || ((s2.ToLower().Contains("l")) && !(s1.ToLower().Contains("l"))))
                {
                    allowed = false;
                }
            }
            return allowed;
        }
    }
}
