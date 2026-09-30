// LeetCode 1456 — Maximum Number of Vowels in a Substring of Given Length
// Technique: fixed-size Sliding Window
//   1) count the vowels in the first window [0, k)
//   2) slide one step at a time: add the entering char s[i], remove the leaving char s[i - k]
//   3) keep the maximum
// Time: O(n)   Space: O(1)

public class Solution
{
    public int MaxVowels(string s, int k)
    {
        int count = 0;

        // Build the first window once.
        for (int i = 0; i < k; i++)
        {
            if (IsVowel(s[i])) count++;
        }

        int max = count;

        // Slide the window: one char enters on the right, one leaves on the left.
        for (int i = k; i < s.Length; i++)
        {
            if (IsVowel(s[i])) count++;       // entering
            if (IsVowel(s[i - k])) count--;   // leaving

            if (count > max) max = count;
        }

        return max;
    }

    private static bool IsVowel(char c)
    {
        return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';
    }
}
