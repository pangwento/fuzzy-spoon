using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = System.Random;

public class LeetCode : MonoBehaviour
{
    public string ReverseWords(string s)
    {
        var result = new StringBuilder();
        var start = -1;
        var end = -1;
        var addSpace = false;
        var isNew = false;
        for(var i = s.Length - 1; i >= 0; i--)
        {
            var c = s[i];
            if(c != ' ')
            {
                if(!isNew) 
                {
                    end = i;
                }
                start = i;
                isNew = true;
            }
            else
            {
                if(!isNew) continue;
                if (addSpace)
                {
                    result.Append(' ');
                    addSpace = false;
                }
                for(var j = start; j <= end; j++)
                {
                    result.Append(s[j]);
                    addSpace = true;
                }
                isNew = false;
            }
        }

        if (isNew)
        {
            for(var j = start; j <= end; j++)
            {
                result.Append(s[j]);
            }
        }
        return result.ToString();
    }
    
    public int RemoveDuplicates(int[] nums) {
        var k = 0;
        var next = 0;
        for (var i = 0; i < nums.Length; i++)
        {
            if (i < next) continue;
            next = i;
            while (next < nums.Length && nums[i] == nums[next])
            {
                next++;
            }
            if(next == nums.Length && nums[k] == nums[next - 1])
            {
                break;
            }
            k++;
            nums[k] = nums[next];
        }
        return k + 1;        
    }
    // 121.买卖股票的最佳时机
    public int MaxProfit(int[] prices)
    {
        var maxProfit = 0;
        var minPrices = int.MaxValue;
        foreach (var price in prices)
        {
            if (price < minPrices)
                minPrices = price;
            if (price - minPrices > maxProfit)
                maxProfit = price - minPrices;
        }
        return maxProfit;
    }
    // 169.多数元素
    public int MajorityElement(int[] nums)
    {
        var major = nums[0];
        var maxCount = 1;

        var dict = new Dictionary<int, int>();
        foreach (var num in nums)
        {
            if (!dict.TryGetValue(num, out var count))
            {
                count = 0;
                dict.Add(num, count);
            }
            dict[num] = ++count;

            if (count <= maxCount) continue;
            maxCount = count;
            major = num;
        }
        return major;
    }
    // 274.H指数
    public int HIndex(int[] citations)
    {
        var h = 0;
        Array.Sort(citations);
        for (var i = citations.Length - 1; i >= 0; i--)
        {
            h++;
            if (h >= citations[i])
            {
                break;
            }
        }
        return h;
    }
    // [380] O(1) 时间插入、删除和获取随机元素
    public class RandomizedSet
    {
        private List<int> nums;
        private Dictionary<int, int> indices;
        private Random random;
        public RandomizedSet() 
        {
            nums = new List<int>();
            indices = new Dictionary<int, int>();
            random = new Random();
        }
    
        public bool Insert(int val)
        {
            if (indices.ContainsKey(val))
                return false;
            var index = nums.Count;
            nums.Add(val);
            indices.Add(val, index);
            return true;
        }
    
        public bool Remove(int val)
        {
            if (!indices.TryGetValue(val, out var index))
                return false;
            var last = nums[^1];
            nums[index] = last;
            indices[last] = index;
            nums.RemoveAt(nums.Count - 1);
            indices.Remove(val);
            return true;
        }
    
        public int GetRandom()
        {
            var index = random.Next(nums.Count);
            return nums[index];
        }
    }
    //238. 除自身以外数组的乘积
    public int[] ProductExceptSelf(int[] nums)
    {
        var result = new int[nums.Length];
        var left = 1;
        for (var i = 0; i < nums.Length; i++)
        {
            result[i] = left;
            left *= nums[i];
        }

        var right = 1;
        for (var i = nums.Length - 1; i >= 0; i--)
        {
            result[i] *= right;
            right *= nums[i];
        }
        return result;
    }
    // 134. 加油站
    public int CanCompleteCircuit(int[] gas, int[] cost)
    {
        var length = gas.Length;
        var start = 0;
        while(start < length)
        {
            var current = start;
            var sum = 0;
            var count = 0;
            while(count < length)
            {
                sum += gas[current];
                sum -= cost[current];
                if (sum < 0)
                {
                    start += count + 1;
                    break;
                }

                current = (current + 1) % length;
                count++;
            }

            if (count == length)
                return start;
        }

        return -1;
    }
    // [13] 罗马数字转整数
    public int RomanToInt(string s) {
        var answer = 0;
        var len = s.Length;
        for(var i = 0; i < len; i++)
        {
            var v = Get(s[i]);
            if(i < (len - 1) && v < Get(s[i+1]))
                answer -= v;
            else
                answer += v;
        }        
        return answer;
    }
    public int Get(char c)
    {
        switch (c)
        {
            case 'I':
                return 1;
            case 'V':
                return 5;
            case 'X':
                return 10;
            case 'L':
                return 50;
            case 'C':
                return 100;
            case 'D':
                return 500;
            case 'M':
                return 1000;
        }
        return 0;
    }    
    // [1306] 跳跃游戏 III
    public bool CanReach(int[] arr, int start) {
        var vis = new bool[arr.Length];
        for (var i = 0; i < vis.Length; i++)
        {
            vis[i] = false;
        }

        bool dfs(int u)
        {
            if (u < 0 || u >= arr.Length) return false;
            if (vis[u]) return false;
            if (arr[u] == 0) return true;
            vis[u] = true;
            return dfs(u + arr[u]) || dfs(u - arr[u]);
        }

        return dfs(start);
        // bfs
        // var queue = new Queue<int>();
        // queue.Enqueue(start);
        // while(queue.Count != 0)
        // {
        //     var u = queue.Dequeue();
        //     if(u < 0 || u >= arr.Length) continue;
        //     if(vis[u]) continue;
        //     if(arr[u] == 0) return true;
        //     vis[u] = true;
        //     queue.Enqueue(u + arr[u]);
        //     queue.Enqueue(u - arr[u]);
        // }
        // return false;
    }
    // 62.不同路径
    public int UniquePaths(int m, int n) {
        // long ans = 1;
        // for(var i = 0; i < Math.Min(m, n) - 1; i++)
        // {
        //     ans *= m + n - 2 - i;
        //     ans /= i + 1;
        // }
        // return (int)ans;
        int[,] C = new int[m + n, m + n];
        for(var i = 0; i < m + n; i ++)
        {
            for (var j = 0; j <= i; j++)
            {
                if(j == 0 || j == i) 
                    C[i,j] = 1;
                else
                    C[i,j] = C[i-1,j-1] + C[i-1,j];
            }
        }
        return C[n + m - 2, n - 1];
    }
    // 63.不同路径2
    public int UniquePathsWithObstacles(int[][] obstacleGrid) {
        var n = obstacleGrid.Length;
        var m = obstacleGrid[0].Length;
        var dp = new int[n,m];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < m; j++)
            {
                if (obstacleGrid[i][j] == 1) dp[i, j] = 0;
                else if (i == 0 && j == 0) dp[i, j] = 1;
                else
                {
                    dp[i, j] = 0;
                    if (i > 0) dp[i, j] += dp[i - 1, j];
                    if (j > 0) dp[i, j] += dp[i, j - 1];
                }
            }
        }
        return dp[n - 1, m - 1];
    }
    // 322.零钱兑换
    public int CoinChange(int[] coins, int amount) {
        // 1. C0~Ci组成面额为S的需要的最小张数F[S]
        // 2. F[0] = 0, F[1] = 1, F[2] = 1, F[5] = 1
        // 3. F[S] = min(F[S], min(F[S-C0], F[S-C1],...,F[S-Cn]) + 1)
        int[] dp = new int[amount+1];
        dp[0] = 0;
        for(var i = 1; i <= amount; i++)
        {
            dp[i] = amount + 1;
            for(var j = 0; j < coins.Length; j++)
            {
                if(coins[j] <= i)
                    dp[i] = Math.Min(dp[i], dp[i - coins[j]] + 1);
            }
        }
        return dp[amount] > amount ? -1 : dp[amount];
    }
    //[1009] 十进制整数的反码
    public int BitwiseComplement(int n)
    {
        var highBit = 0;
        var i = 0;
        while(n >= (1 << i)) {
            highBit = i;
            i++;
        }
        var mask = (highBit == 30 ? 0x7fffffff : (1 << (highBit + 1)) - 1);
        return n ^ mask;
    }
    //[101] 对称二叉树
    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;

        public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
        {
            this.val = val;
            this.left = left;
            this.right = right;
        }
    }
    public bool IsSymmetric(TreeNode root)
    {
        return IsSymmetricCompare(root.left, root.right);
    }
    public bool IsSymmetricCompare(TreeNode root1, TreeNode root2)
    {
        if(root1 == null && root2 == null) return true;
        if(root1 == null || root2 == null || root1.val != root2.val) return false;
        return IsSymmetricCompare(root1.left, root2.right) & IsSymmetricCompare(root1.right, root2.left);
    }
    // [9] 回文数
    public bool IsPalindrome(int x) {
        if(x < 0) return false;
        var ans = 0;
        var r = x;
        while(r > 0)
        {
            ans *= 10;
            ans += r % 10;
            r /= 10;
        }
        return ans == x;
    }
    // [28] 找出字符串中第一个匹配项的下标
    public int StrStr(string haystack, string needle) {
        for(var i = 0; i <= haystack.Length - needle.Length; i++)
        {
            var s = i;
            for(var j = 0; j < needle.Length; j++)
            {
                if(haystack[s]!= needle[j])
                {
                    break;
                }
                s++;
            }
            if(s == (i + needle.Length))
                return i;
        }
        return -1;
    }
    // [637] 二叉树的层平均值
    public IList<double> AverageOfLevels(TreeNode root)
    {
        var avgList = new List<double>();
        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            var cnt = queue.Count;
            var i = cnt;
            var sum = 0.0;
            while (i > 0)
            {
                var node = queue.Dequeue();
                if(node.left != null) queue.Enqueue(node.left);
                if(node.right != null) queue.Enqueue(node.right);
                
                sum += node.val;
                i--;
            }
            avgList.Add(sum / cnt);
        }
        return avgList;
    }
    //[383] 赎金信
    public bool CanConstruct(string ransomNote, string magazine)
    {
        if (ransomNote.Length > magazine.Length)
            return false;
        var cnt = new int[26];
        foreach (var c in magazine)
        {
            cnt[c - 'a']++;
        }

        foreach (var c in ransomNote)
        {
            cnt[c - 'a']--;
            if (cnt[c - 'a'] < 0) 
                return false;
        }
        return true;
    }
    // [66] 加一
    public int[] PlusOne(int[] digits) {
        for (var i = digits.Length - 1; i >= 0; i--)
        {
            if (digits[i] == 9) 
                digits[i] = 0;
            else
            {
                digits[i] += 1;
                return digits;
            }
        }

        var digitsNew = new int[digits.Length + 1];
        digitsNew[0] = 1;
        return digitsNew;
    }
    // [67] 二进制求和
    public string AddBinary(string a, string b)
    {
        var m = a.Length;
        var n = b.Length;
        var max = Math.Max(m, n);
        var carry = 0;
        var ans = new StringBuilder();
        for (var i = 1; i <= max; i++)
        {
            var sum = carry;
            if (m - i >= 0)
            {
                var va =  a[m - i] == '1' ? 1 : 0 ;
                sum += va ;
            }
            if (n - i >= 0)
            {
                var vb = b[n - i] == '1' ? 1  : 0;
                sum += vb;
            }
            if (sum >= 2)
            {
                carry = 1;
                sum -= 2;
            }
            else
                carry = 0;
            ans.Append( sum == 0 ? '0' : '1');
        }
        if (carry == 1)
            ans.Append('1');
        var chars = ans.ToString().ToCharArray();
        Array.Reverse(chars);
        return chars.ToString();
    }
    //[392] 判断子序列
    public bool IsSubsequence(string s, string t)
    {
        var dp = new int[t.Length + 1];
        for (var i = 1; i <= t.Length; i++)
        {
            dp[i] = dp[i - 1];
            if (dp[i] == s.Length) return true;
            if (t[i-1] == s[dp[i - 1]]) dp[i]++;
        }
        return dp[t.Length] >= s.Length;
    }
    //[125] 验证回文串
    public bool IsPalindrome(string s)
    {
        var l = 0;
        var r = s.Length-1;
        while (l < r)
        {
            while (l < r && !IsLetterOrDigit(s[l]))
            {
                l++;
            }
            while (l < r && !IsLetterOrDigit(s[r]))
            {
                r--;
            }
            if (l < r)
            {
                if (char.ToLower(s[l]) != char.ToLower(s[r])) return false;
                l++;
                r--;
            }
        }
        return true;
    }
    public bool IsLetterOrDigit(char a)
    {
        return (a >= 'A' && a <= 'Z') ||
               (a >= 'a' && a <= 'z') ||
               (a >= '0' && a <= '9');
    }
    // [141] 环形链表
    public class ListNode
    {
        public int val;
        public ListNode next;

        public ListNode(int x)
        {
            val = x;
            next = null;
        }
    }
    public bool HasCycle(ListNode head)
    {
        if (head == null) return false;
        var ptr1 = head;
        var ptr2 = head.next;
        while (ptr1 != null && ptr2 != null)
        {
            if (ptr1 == ptr2) return true;
            ptr1 = ptr1.next;
            ptr2 = ptr2.next?.next;
        }
        return false;
    }
    // [205] 同构字符串
    public bool IsIsomorphic(string s, string t)
    {
        for (var i = 0; i < s.Length; i++)
        {
            if (s.IndexOf(s[i]) != t.IndexOf(t[i]))
                return false;
        }
        return true;
    }
    // [290].单词规律
    public bool WordPattern(string pattern, string s)
    {
        Dictionary<char, string> lookup1 = new Dictionary<char, string>();
        Dictionary<string, char> lookup2 = new Dictionary<string, char>();

        var sList = s.Split(' ');
        if (sList.Length != pattern.Length) return false;

        for (var i = 0; i < sList.Length; i++)
        {
            var str = sList[i];
            var c = pattern[i];
            if (!lookup1.TryGetValue(c, out var _str) && !lookup2.TryGetValue(str, out var _c))
            {
                lookup1.Add(c, str);
                lookup2.Add(str, c);
            }
            else
            {
                if (str != _str) return false;
                if (!lookup2.ContainsKey(str) || lookup2[str] != c) return false;
            }
        }
        return true;
    }
    // [242] 有效的字母异位词
    public bool IsAnagram(string s, string t)
    {
        if (s.Length != t.Length) return false;
        var counter = new Dictionary<char, int>();
        for (var i = 0; i < s.Length; i++)
        {
            if (counter.TryGetValue(s[i], out var count))
            {
                counter[s[i]] = count+1;
            }
            else 
                counter.Add(s[i], 1);
        }
        for (var i = 0; i < t.Length; i++)
        {
            if (!counter.TryGetValue(t[i], out var count) || count == 0)
            {
                return false;
            }
            counter[t[i]] = count-1;
        }
        return true;
    }
    //[136] 只出现一次的数字
    public int SingleNumber(int[] nums)
    {
        var ans = 0;
        for (var i = 0; i < nums.Length; i++)
        {
            ans ^= nums[i];
        }
        return ans;
    }
    //[215]数组中的第k个最大元素
    public int FindKthLargest(int[] nums, int k) {
        var n = nums.Length;
        BuildMaxHeap(nums, n);
        for (var i = n - 1; i >= (n - k); i--)
        {
            (nums[0], nums[i]) = (nums[i], nums[0]);
            MaxHeapify(nums, i, 0);
        }
        return nums[n - k];
    }

    private void BuildMaxHeap(int[] nums, int n)
    {
        for (var i = n / 2 - 1; i >= 0; i--)
        {
            MaxHeapify(nums, n, i);
        }
    }
    private void MaxHeapify(int[] nums, int n, int i)
    {
        var largest = i;
        var left = 2 * i + 1;
        var right = 2 * i + 2;
        if (left < n && nums[left] > nums[largest])
            largest = left;
        if (right < n && nums[right] > nums[largest])
            largest = right;
        if (largest != i)
        {
            (nums[i], nums[largest]) = (nums[largest], nums[i]);
            MaxHeapify(nums, n, largest);
        }
    }
    // [222] 完全二叉树的节点个数
    public int CountNodes(TreeNode root)
    {
        if (root == null) return 0;
        var level = 0;
        var node = root;
        while (node.left != null)
        {
            level++;
            node = node.left;
        }
        var left = 1 << level;
        var right = (1 << (level + 1)) - 1;
        while (left < right)
        {
            var mid = left + (right - left + 1) / 2;
            if (Exist(root, level, mid))
            {
                left = mid;
            }
            else
            {
                right = mid - 1;
            }
        }
        return left;
    }
    private bool Exist(TreeNode root, int level, int k)
    {
        var bits = 1 << (level -1);
        var node = root;
        while (node != null && bits > 0)
        {
            if ((bits & k) == 0)
            {
                node = node.left;
            }
            else
            {
                node = node.right;
            }

            bits >>= 1;
        }
        return node != null;
    }
    // [146] LRU 缓存
    public class LRUCache
    {
        public class LinkListNode
        {
            public int key;
            public int value;
            public LinkListNode prev;
            public LinkListNode next;

            public LinkListNode()
            {
            }

            public LinkListNode(int k, int v)
            {
                key = k;
                value = v;
            }
        }
        private int capacity;
        private Dictionary<int, LinkListNode> keys;
        private LinkListNode head = new LinkListNode();
        private LinkListNode tail = new LinkListNode();
        public LRUCache(int capacity)
        {
            this.capacity = capacity;
            keys = new Dictionary<int, LinkListNode>(capacity);
            head.prev = null;
            head.next = tail;
            tail.prev = head;
            tail.next = null;
        }
    
        public int Get(int key) {
            if (!keys.ContainsKey(key)) return -1;
            var node = keys[key];
            Remove(node);
            AddFirst(node);
            return keys[key].value;
        }
    
        public void Put(int key, int value) {
            if (keys.ContainsKey(key))
            {
                var node = keys[key];
                node.value = value;
                Remove(node);
                AddFirst(node);
            }
            else
            {
                var node = new LinkListNode(key, value);
                if (keys.Count >= capacity)
                {
                    keys.Remove(tail.prev.key);
                    RemoveLast();
                }
                AddFirst(node);
                keys.Add(key, node);
            }
        }
        public void Remove(LinkListNode node)
        {
            if (node == null) return;
            node.prev.next = node.next;
            node.next.prev = node.prev;
        }

        public void AddFirst(LinkListNode node)
        {
            node.next = head.next;
            node.next.prev = node;
            node.prev = head;
            head.next = node;
        }

        public void RemoveLast()
        {
            var node = tail.prev;
            tail.prev = node.prev;
            node.prev.next = tail;
        }
    }
    // [219] 存在重复元素 II
    public bool ContainsNearbyDuplicate(int[] nums, int k)
    {
        var dict = new Dictionary<int, int>();
        for (var i = 0; i < nums.Length; i++)
        {
            if (dict.TryGetValue(nums[i], out var v))
            {
                if (i - v <= k)
                    return true;
                dict[nums[i]] = i;
            }
            else
                dict.Add(nums[i], i);
        }
        return false;
    }
    //[49] 字母异位词分组
    public IList<IList<string>> GroupAnagrams(string[] strs)
    {
        IList<IList<string>> ans = new List<IList<string>>();
        var dict = new Dictionary<string, List<string>>();
        foreach (var str in strs)
        {
            var chars = new int[26];
            foreach (var c in str)
            {
                chars[c - 'a']++;
            }

            var sb = new StringBuilder();
            for (var i = 0; i < 26; i++)
            {
                if (chars[i] != 0)
                {
                    sb.Append(i + 'a');
                    sb.Append(chars[i]);
                }
            }

            var key = sb.ToString();
            if (dict.TryGetValue(key, out var list))
            {
                list.Add(str);
            }
            else
            {
                list = new List<string>() {str};
                dict.Add(key, list);
                ans.Add(list);
            }
        }
        return ans;
    }
    // [128] 最长连续序列
    public int LongestConsecutive(int[] nums)
    {
        var set = new HashSet<int>();
        for (var i = 0; i < nums.Length; i++)
        {
            set.Add(nums[i]);
        }
        var ans = 0;
        foreach (var num in set)
        {
            if (!set.Contains(num - 1))
            {
                var len = 1;
                var s = num;
                while (set.Contains(s + 1))
                {
                    s++;
                    len++;
                }
                if (len > ans)
                    ans = len;
            }
        }
        return ans;
    }
    //[202] 快乐数
    public bool IsHappy(int n)
    {
        var set = new HashSet<int>();
        n = GetNext(n);
        while (n != 1 && !set.Contains(n))
        {
            set.Add(n);
            n = GetNext(n);
        }
        return n == 1;
    }

    public int GetNext(int n)
    {
        var value = 0;
        while (n > 0)
        {
            var a = n % 10;
            value += a * a;
            n = n / 10;
        }
        return value;
    }
    // [155] 最小栈
    public class MinStack
    {
        private Stack<int> stack = new Stack<int>();
        private Stack<int> minStack = new Stack<int>();
        public MinStack() {
            minStack.Push(Int32.MaxValue);
        }
    
        public void Push(int val) {
            stack.Push(val);
            minStack.Push(Math.Min(val, minStack.Peek()));
        }
    
        public void Pop()
        {
            stack.Pop();
            minStack.Pop();
        }
    
        public int Top()
        {
            return stack.Peek();
        }
    
        public int GetMin()
        {
            return minStack.Peek();
        }
    }
    // [71] 简化路径
    public string SimplifyPath(string path)
    {
        var strs = path.Split('/');
        var list = new List<string>();
        foreach (var str in strs)
        {
            if (str.Equals(".."))
            {
                if (list.Count > 0)
                    list.RemoveAt(list.Count - 1);
            }
            else if (str.Length > 0 && !str.Equals("."))
            {
                list.Add(str);
            }
        }
        var sb = new StringBuilder();
        if (list.Count == 0)
            sb.Append('/');
        foreach (var v in list)
        {
            sb.Append('/');
            sb.Append(v);
        }
        return sb.ToString();
    }
    //[150] 逆波兰表达式求值
    public int EvalRPN(string[] tokens)
    {
        var stack = new Stack<int>();
        foreach (var token in tokens)
        {
            if (token.Equals("+"))
            {
                var v1 = stack.Pop();
                var v2 = stack.Pop();
                stack.Push(v1 + v2);
            }
            else if (token.Equals("-"))
            {
                var v1 = stack.Pop();
                var v2 = stack.Pop();
                stack.Push(v2 - v1);
            }
            else if (token.Equals("*"))
            {
                var v1 = stack.Pop();
                var v2 = stack.Pop();
                stack.Push(v1 * v2);
            }
            else if (token.Equals("/"))
            {
                var v1 = stack.Pop();
                var v2 = stack.Pop();
                stack.Push(v2 / v1);
            }
            else
            {
                var value = int.Parse(token);
                stack.Push(value);
            }
        }
        return stack.Pop();
    }
    // [224] 基本计算器
    public int Calculate(string s)
    {
        var stack = new Stack<int>();
        stack.Push(1);
        var sign = 1;
        var len = s.Length;
        var i = 0;
        var ans = 0;
        while (i < len)
        {
            if (s[i] == ' ')
                i++;
            else if (s[i] == '+')
            {
                sign = stack.Peek();
                i++;
            }
            else if (s[i] == '-')
            {
                sign = -stack.Peek();
                i++;
            }
            else if (s[i] == '(')
            {
                stack.Push(sign);
                i++;
            }
            else if (s[i] == ')')
            {
                stack.Pop();
                i++;
            }
            else
            {
                var num = 0;
                while (i < len && s[i] >= '0' && s[i] <= '9')
                {
                    num = num * 10 + s[i] - '0';
                    i++;
                }
                ans += sign * num;
            }
        }
        return ans;
    }
}
