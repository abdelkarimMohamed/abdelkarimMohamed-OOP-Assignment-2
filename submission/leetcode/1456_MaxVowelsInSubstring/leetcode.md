# LeetCode 1456 — Maximum Number of Vowels in a Substring of Given Length

- Problem: https://leetcode.com/problems/maximum-number-of-vowels-in-a-substring-of-given-length
- My accepted submission: https://leetcode.com/problems/maximum-number-of-vowels-in-a-substring-of-given-length/submissions/PASTE_YOUR_SUBMISSION_ID/

**Approach:** fixed-size sliding window — count the vowels in the first `k` characters once, then slide one step at a time, adding the entering character and removing the leaving one, while tracking the maximum. `O(n)` time, `O(1)` space.
