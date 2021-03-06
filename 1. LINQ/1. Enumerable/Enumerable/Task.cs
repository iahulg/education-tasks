﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableTask
{

    public class Task
	{
		/// <summary> Transforms all strings to uppercase</summary>
		/// <param name="data">source string sequence</param>
		/// <returns>
		///   Returns sequence of source strings in uppercase
		/// </returns>
		/// <example>
		///    {"a","b","c"} => { "A", "B", "C" }
		///    { "A", "B", "C" } => { "A", "B", "C" }
		///    { "a", "A", "", null } => { "A", "A", "", null }
		/// </example>
		public IEnumerable<string> GetUppercaseStrings(IEnumerable<string> data)
		{
            return data.Select(d => d?.ToUpper());
		}

		/// <summary> Transforms an each string from sequence to its length</summary>
		/// <param name="data">source strings sequence</param>
		/// <returns>
		///   Returns sequence of strings length
		/// </returns>
		/// <example>
		///   { } => { }
		///   {"a","aa","aaa" } => { 1, 2, 3 }
		///   {"aa","bb","cc", "", "  ", null } => { 2, 2, 2, 0, 2, 0 }
		/// </example>
		public IEnumerable<int> GetStringsLength(IEnumerable<string> data)
		{
            return data.Select(d => LenWithNulls(d));
		}

		/// <summary>Transforms int sequence to its square sequence, f(x) = x * x </summary>
		/// <param name="data">source int sequence</param>
		/// <returns>
		///   Returns sequence of squared items
		/// </returns>
		/// <example>
		///   { } => { }
		///   { 1, 2, 3, 4, 5 } => { 1, 4, 9, 16, 25 }
		///   { -1, -2, -3, -4, -5 } => { 1, 4, 9, 16, 25 }
		/// </example>
		public IEnumerable<long> GetSquareSequence(IEnumerable<int> data)
		{
            return data.Select(d =>(long) d * d);
		}

		/// <summary>Transforms int sequence to its moving sum sequence, 
		///          f[n] = x[0] + x[1] + x[2] +...+ x[n] 
		///       or f[n] = f[n-1] + x[n]   
		/// </summary>
		/// <param name="data">source int sequence</param>
		/// <returns>
		///   Returns sequence of sum of all source items with less or equals index
		/// </returns>
		/// <example>
		///   { } => { }
		///   { 1, 1, 1, 1 } => { 1, 2, 3, 4 }
		///   { 5, 5, 5, 5 } => { 5, 10, 15, 20 }
		///   { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } => { 1, 3, 6, 10, 15, 21, 28, 36, 45, 55 }
		///   { 1, -1, 1, -1, -1 } => { 1, 0, 1, 0, 1 }
		/// </example>
		public IEnumerable<long> GetMovingSumSequence(IEnumerable<int> data)
		{
            long sum = 0;
            return data.Select(d => (sum += d));
		}

		/// <summary> Filters a string sequence by a prefix value (case insensitive)</summary>
		/// <param name="data">source string sequence</param>
		/// <param name="prefix">prefix value to filter</param>
		/// <returns>
		///  Returns items from data that started with required prefix (case insensitive)
		/// </returns>
		/// <exception cref="System.ArgumentNullException">prefix is null</exception>
		/// <example>
		///  { "aaa", "bbbb", "ccc", null }, prefix="b"  =>  { "bbbb" }
		///  { "aaa", "bbbb", "ccc", null }, prefix="B"  =>  { "bbbb" }
		///  { "a","b","c" }, prefix="D"  => { }
		///  { "a","b","c" }, prefix=""   => { "a","b","c" }
		///  { "a","b","c", null }, prefix=""   => { "a","b","c" }
		///  { "a","b","c" }, prefix=null => exception
		/// </example>
		public IEnumerable<string> GetPrefixItems(IEnumerable<string> data, string prefix)
		{
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }
            return data.Where(d => !string.IsNullOrEmpty(d) && d.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
		}

		/// <summary> Returns every second item from source sequence</summary>
		/// <typeparam name="T">the type of the elements of data</typeparam>
		/// <param name="data">source sequence</param>
		/// <returns>Returns a subsequence that consists of every second item from source sequence</returns>
		/// <example>
		///  { 1,2,3,4,5,6,7,8,9,10 } => { 2,4,6,8,10 }
		///  { "a","b","c" , null } => { "b", null }
		///  { "a" } => { }
		/// </example>
		public IEnumerable<T> GetEvenItems<T>(IEnumerable<T> data)
		{
            return data.Where((d, index) => index % 2 != 0);
		}

		/// <summary> Propagate every item in sequence its position times</summary>
		/// <typeparam name="T">the type of the elements of data</typeparam>
		/// <param name="data">source sequence</param>
		/// <returns>
		///   Returns a sequence that consists of: one first item, two second items, tree third items etc. 
		/// </returns>
		/// <example>
		///   { } => { }
		///   { 1 } => { 1 }
		///   { "a","b" } => { "a", "b","b" }
		///   { "a", "b", "c", null } => { "a", "b","b", "c","c","c", null,null,null,null }
		///   { 1,2,3,4,5} => { 1, 2,2, 3,3,3, 4,4,4,4, 5,5,5,5,5 }
		/// </example>
		public IEnumerable<T> PropagateItemsByPositionIndex<T>(IEnumerable<T> data)
        {
            // Для повторения одно и того же элемента есть у класса Enumerable
            // есть статик метод data.SelectMany((item, index) => Enumerable.Repeat(item, index + 1));
            return data.SelectMany((d, index) => Enumerable.Range(0, index + 1).Select(r => d));
		}

		/// <summary>Finds all used char in string sequence</summary>
		/// <param name="data">source string sequence</param>
		/// <returns>
		///   Returns set of chars used in string sequence.
		///   Order of result does not matter.
		/// </returns>
		/// <example>
		///   { "aaa", "bbb", "cccc", "abc" } => { 'a', 'b', 'c' } or { 'c', 'b', 'a' }
		///   { " ", null, "   ", "" } => { ' ' }
		///   { "", null } => { }
		///   { } => { }
		/// </example>
		public IEnumerable<char> GetUsedChars(IEnumerable<string> data)
		{
            // Distinct довольно затратная операция, тут не обязательно её два раза вызывать
            // data.Where(d => !string.IsNullOrEmpty(d)).SelectMany(d => d).Distinct();
            return data.Where(d => !string.IsNullOrEmpty(d)).SelectMany(d => d.Distinct()).Distinct();
		}

		/// <summary> Converts a source sequence to a string</summary>
		/// <typeparam name="T">the type of the elements of data</typeparam>
		/// <param name="data">source sequence</param>
		/// <returns>
		///   Returns a string respresentation of source sequence 
		/// </returns>
		/// <example>
		///   { } => ""
		///   { 1,2,3 } => "1,2,3"
		///   { "a", "b", "c", null, ""} => "a,b,c,null,"
		///   { "", "" } => ","
		/// </example>
		public string GetStringOfSequence<T>(IEnumerable<T> data)
		{
            return string.Join(",", data.Select(d => d == null ? "null" : d.ToString()));
		}

		/// <summary> Finds the 3 largest numbers from a sequence</summary>
		/// <param name="data">source sequence</param>
		/// <returns>
		///   Returns the 3 largest numbers from a sequence
		/// </returns>
		/// <example>
		///   { } => { }
		///   { 1, 2 } => { 2, 1 }
		///   { 1, 2, 3 } => { 3, 2, 1 }
		///   { 1,2,3,4,5,6,7,8,9,10 } => { 10, 9, 8 }
		///   { 10, 10, 10, 10 } => { 10, 10, 10 }
		/// </example>
		public IEnumerable<int> Get3TopItems(IEnumerable<int> data)
		{
            return data.OrderByDescending(d => d).Take(3);
		}

		/// <summary> Calculates the count of numbers that are greater then 10</summary>
		/// <param name="data">source sequence</param>
		/// <returns>
		///   Returns the number of items that are > 10
		/// </returns>
		/// <example>
		///   { } => 0
		///   { 1, 2, 10 } => 0
		///   { 1, 2, 3, 11 } => 1
		///   { 1, 20, 30, 40 } => 3
		/// </example>
		public int GetCountOfGreaterThen10(IEnumerable<int> data)
		{
            const int threshold = 10;
            return data.Count(d => d > threshold);
		}

		/// <summary> Find the first string that contains "first" (case insensitive search)</summary>
		/// <param name="data">source sequence</param>
		/// <returns>
		///   Returns the first string that contains "first" or null if such an item does not found.
		/// </returns>
		/// <example>
		///   { "a", "b", null} => null
		///   { "a", "IT IS FIRST", "first item", "I am really first!" } => "IT IS FIRST"
		///   { } => null
		/// </example>
		public string GetFirstContainsFirst(IEnumerable<string> data)
        {
            // Для сравнения строк лучше использовать доп. параметр StringComparsion
            // data.FirstOrDefault(item => !string.IsNullOrEmpty(item) && item.IndexOf("first", StringComparison.OrdinalIgnoreCase) >= 0);
            const string strToFind = "FIRST";
            return data.FirstOrDefault(d => !string.IsNullOrEmpty(d) && d.ToUpperInvariant().Contains(strToFind));
		}

		/// <summary> Counts the number of unique strings with length=3 </summary>
		/// <param name="data">source sequence</param>
		/// <returns>
		///   Returns the number of unique strings with length=3.
		/// </returns>
		/// <example>
		///   { "a", "b", null, "aaa"} => 1    ("aaa")
		///   { "a", "bbb", null, "", "ccc" } => 2  ("bbb","ccc")
		///   { "aaa", "aaa", "aaa", "bbb" } => 2   ("aaa", "bbb") 
		///   { } => 0
		/// </example>
		public int GetCountOfStringsWithLengthEqualsTo3(IEnumerable<string> data)
		{
            return data.Distinct().Count(d => LenWithNulls(d) == 3);
		}

		/// <summary> Counts the number of each strings in sequence </summary>
		/// <param name="data">source sequence</param>
		/// <returns>
		///   Returns the list of strings and number of occurence for each string.
		/// </returns>
		/// <example>
		///   { "a", "b", "a", "aa", "b"} => { ("a",2), ("b",2), ("aa",1) }    
		///   { "a", "a", null, "", "ccc", "" } => { ("a",2), (null,1), ("",2), ("ccc",1) }
		///   { "aaa", "aaa", "aaa" } =>  { ("aaa", 3) } 
		///   { } => { }
		/// </example>
		public IEnumerable<Tuple<string, int>> GetCountOfStrings(IEnumerable<string> data)
		{
            return data.GroupBy(d => d, (key, group) => new Tuple<string, int>(key, group.Count()));
		}

		/// <summary> Counts the number of strings with max length in sequence </summary>
		/// <param name="data">source sequence</param>
		/// <returns>
		///   Returns the number of strings with max length. (Assuming that null has Length=0).
		/// </returns>
		/// <example>
		///   { "a", "b", "a", "aa", "b"} => 1 
		///   { "a", "aaa", null, "", "ccc", "" } => 2   
		///   { "aaa", "aaa", "aaa" } =>  3
		///   { "", null, "", null } => 4
		///   { } => { }
		/// </example>
		public int GetCountOfStringsWithMaxLength(IEnumerable<string> data)
		{            
            if (!data.Any())
            {
                return 0;
            }
            return data.GroupBy(d => LenWithNulls(d))
                       .OrderByDescending(g => g.Key)
                       .Select(g => g.Count())
                       .FirstOrDefault();
        }

		/// <summary> Counts the digit chars in a string</summary>
		/// <param name="data">source string</param>
		/// <returns>
		///   Returns number of digit chars in the string
		/// </returns>
		/// <exception cref="System.ArgumentNullException">data is null</exception>
		/// <example>
		///    "aaaa" => 0
		///    "1234" => 4
		///    "A1*B2" => 2
		///    "" => 0
		///    null => exception
		/// </example>
		public int GetDigitCharsCount(string data)
		{
            return data.Count(c => char.IsDigit(c));
		}

		/// <summary>Counts the system log events of required type</summary>
		/// <param name="value">the type of log event (Error, Event, Information etc)</param>
		/// <returns>
		///   Returns the number of System log entries of specified type
		/// </returns>
		public int GetSpecificEventEntriesCount(EventLogEntryType value)
		{
            return (new EventLog("System")).Entries
                                           .Cast<EventLogEntry>()
                                           .Where(e => e.EntryType == value)
                                           .Count();
        }

		/// <summary>Calculates sales sum by quarter</summary>
		/// <param name="sales">the source sales data : Item1 is sales date, Item2 is amount</param>
		/// <returns>
		///     Returns array of sales sum by quarters, 
		///     result[0] is sales sum for Q1, result[1] is sales sum for Q2 etc  
		/// </returns>
		/// <example>
		///    {} => { 0, 0, 0, 0}
		///    {(1/1/2010, 10)  , (2/2/2010, 10), (3/3/2010, 10) } => { 30, 0, 0, 0 }
		///    {(1/1/2010, 10)  , (4/4/2010, 10), (10/10/2010, 10) } => { 10, 10, 0, 10 }
		/// </example>
		public int[] GetQuarterSales(IEnumerable<Tuple<DateTime, int>> sales)
        {            
            // RTFM: How to use Aggregate.
            // sales
            //   .Aggregate(new int[4], (sum, sale) =>
            //   {
            //       sum[(sale.Item1.Month - 1) / 3] += sale.Item2;
            //       return sum;
            //   });

            var emptyQuarters = Enumerable.Range(1, 4).Select(x => new KeyValuePair<int, int>(x, 0));
            return sales.Select(s => new KeyValuePair<int, int>((s.Item1.Month + 2) / 3, s.Item2))
                        .Concat(emptyQuarters)
                        .GroupBy(s => s.Key)
                        .OrderBy(g => g.Key)
                        .Select(s => s.Sum(x => x.Value))
                        .ToArray();
        }

		/// <summary> Sorts string by length and alphabet </summary>
		/// <param name="data">the source data</param>
		/// <returns>
		/// Returns sequence of strings sorted by length and alphabet
		/// </returns>
		/// <example>
		///  {} => {}
		///  {"c","b","a"} => {"a","b","c"}
		///  {"c","cc","b","bb","a,"aa"} => {"a","b","c","aa","bb","cc"}
		/// </example>
		public IEnumerable<string> SortStringsByLengthAndAlphabet(IEnumerable<string> data)
		{
            return data.OrderBy(d => d.Length).ThenBy(d => d);
		}

		/// <summary> Finds all missing digits </summary>
		/// <param name="data">the source data</param>
		/// <returns>
		/// Return all digits that are not in the string sequence
		/// </returns>
		/// <example>
		///   {} => {'0','1','2','3','4','5','6','7','8','9'}
		///   {"aaa","a1","b","c2","d","e3","f01234"} => {'5','6','7','8','9'}
		///   {"a","b","c","9876543210"} => {}
		/// </example>
		public IEnumerable<char> GetMissingDigits(IEnumerable<string> data)
		{
            const string digits = "0123456789";
            return digits.Except(data.SelectMany(d => d).Where(d => char.IsDigit(d)));
		}

		/// <summary> Sorts digit names </summary>
		/// <param name="data">the source data</param>
		/// <returns>
		/// Return all digit names sorted by numeric order
		/// </returns>
		/// <example>
		///   {} => {}
		///   {"nine","one"} => {"one","nine"}
		///   {"one","two","three"} => {"one","two","three"}
		///   {"nine","eight","nine","eight"} => {"eight","eight","nine","nine"}
		///   {"one","one","one","zero"} => {"zero","one","one","one"}
		/// </example>
		public IEnumerable<string> SortDigitNamesByNumericOrder(IEnumerable<string> data)
		{
            var digits = new Dictionary<string, int>
            {
                { "ZERO",   0 },
                { "ONE",    1 },
                { "TWO",    2 },
                { "THREE",  3 },
                { "FOUR",   4 },
                { "FIVE",   5 },
                { "SIX",    6 },
                { "SEVEN",  7 },
                { "EIGHT",  8 },
                { "NINE",   9 },
            };
            return data.Select(d => new KeyValuePair<int, string>(digits[d.ToUpperInvariant()], d))
                       .OrderBy(d => d.Key)
                       .Select(d => d.Value);
		}

		/// <summary> Combines numbers and fruits </summary>
		/// <param name="numbers">string sequience of numbers</param>
		/// <param name="fruits">string sequence of fruits</param>
		/// <returns>
		///   Returns new sequence of merged number and fruit items
		/// </returns>
		/// <example>
		///  {"one","two","three"}, {"apple", "bananas","pineapples"} => {"one apple","two bananas","three pineapples"}
		///  {"one"}, {"apple", "bananas","pineapples"} => {"one apple"}
		///  {"one","two","three"}, {"apple", "bananas" } => {"one apple","two bananas"}
		///  {"one","two","three"}, { } => { }
		///  { }, {"apple", "bananas" } => { }
		/// </example>
		public IEnumerable<string> CombineNumbersAndFruits(IEnumerable<string> numbers, IEnumerable<string> fruits)
		{
            return numbers.Zip(fruits, (f, s) => f + " " + s);
		}

		/// <summary> Finds all chars that are common for all words </summary>
		/// <param name="data">sequence of words</param>
		/// <returns>
		///  Returns set of chars that are occured in all words (sorted in alphabetical order)
		/// </returns>
		/// <example>
		///   {"ab","ac","ad"} => {"a"}
		///   {"a","b","c"} => { }
		///   {"a","aa","aaa"} => {"a"}
		///   {"ab","ba","aabb","baba"} => {"a","b"}
		/// </example>
		public IEnumerable<char> GetCommonChars(IEnumerable<string> data)
		{
            return data.SelectMany(d => d).Distinct().Where(c => data.All(s => s.Contains(c)));
		}

		/// <summary> Calculates sum of all integers from object array </summary>
		/// <param name="data">source data</param>
		/// <returns>
		///    Returns the sum of all inetegers from object array
		/// </returns>
		/// <example>
		///    { 1, true, "a","b", false, 1 } => 2
		///    { true, false } => 0
		///    { 10, "ten", 10 } => 20 
		///    { } => 0
		/// </example>
		public int GetSumOfAllInts(object[] data)
        {
            // RTFM: How to use OfType<T>()
            //data
            //  .OfType<int>()
            //  .Sum();
            return data.Where(d => d is int)
                       .Select(d => (int) d)
                       .Sum(); 
		}

		/// <summary> Finds all strings in array of objects</summary>
		/// <param name="data">source array</param>
		/// <returns>
		///   Return subsequence of string from source array of objects
		/// </returns>
		/// <example>
		///   { "a", 1, 2, null, "b", true, 4.5, "c" } => { "a", "b", "c" }
		///   { "a", "b", "c" } => { "a", "b", "c" }
		///   { 1,2,3, true, false } => { }
		///   { } => { }
		/// </example>
		public IEnumerable<string> GetStringsOnly(object[] data)
        {
            // Также ошибка что и в предыдущем примере, не нужно делать
            // никаких лишних преобразований data.OfType<string>();
            return data.Where(d => d is string)
                       .Select(d => (string) d);
        }

		/// <summary> Calculates the total length of strings</summary>
		/// <param name="data">source string sequence</param>
		/// <returns>
		///   Return sum of length of all strings
		/// </returns>
		/// <example>
		///   {"a","b","c","d","e","f"} => 6
		///   { "a","aa","aaa" } => 6
		///   { "1234567890" } => 10
		///   { null, "", "a" } => 1
		///   { null } => 0
		///   { } => 0
		/// </example>
		public int GetTotalStringsLength(IEnumerable<string> data)
		{
            return string.Join("", data).Length;
		}

		/// <summary> Determines whether sequence has null elements</summary>
		/// <param name="data">source string sequence</param>
		/// <returns>
		///  true if the source sequence contains null elements; otherwise, false.
		/// </returns>
		/// <example>
		///   { "a", "b", "c", "d", "e", "f" } => false
		///   { "a", "aa", "aaa", null } => true
		///   { "" } => false
		///   { null, null, null } => true
		///   { } => false
		/// </example>
		public bool IsSequenceHasNulls(IEnumerable<string> data)
		{
            return data.Any(d => d == null);
		}

		/// <summary> Determines whether all strings in sequence are uppercase</summary>
		/// <param name="data">source string sequence</param>
		/// <returns>
		///  true if all strings from source sequence are uppercase; otherwise, false.
		/// </returns>
		/// <example>
		///   { "A", "B", "C", "D", "E", "F" } => true
		///   { "AA", "AA", "AAA", "AAAa" } => false
		///   { "" } => false
		///   { } => false
		/// </example>
		public bool IsAllStringsAreUppercase(IEnumerable<string> data)
        {
            // Более логично мне кажется пробежаться по всем буквам в массиве
            // и проверить их на причастность к заглавным буквам, чем делать преобразования.
            // data.SelectMany(item => item).DefaultIfEmpty().All(char.IsUpper);
            return data.Any() && data.All(d => !string.IsNullOrEmpty(d) && 
                                                string.Equals(d, d.ToUpperInvariant()));
		}

		/// <summary> Finds first subsequence of negative integers </summary>
		/// <param name="data">source integer sequence</param>
		/// <returns>
		///   Returns the first subsequence of negative integers from source
		/// </returns>
		/// <example>
		///    { -2, -1 , 0, 1, 2 } => { -2, -1 }
		///    { 2, 1, 0, -1, -2 } => { -1, -2 }
		///    { 1, 1, 1, -1, -1, -1, 0, 0, 0, -2, -2, -2 } => { -1, -1, -1 }
		///    { -1 , 0, -2 } => { -1 }
		///    { 1, 2, 3 } => { }
		///    { } => { }
		/// </example>
		public IEnumerable<int> GetFirstNegativeSubsequence(IEnumerable<int> data)
		{
            return data.SkipWhile(d => d >= 0).TakeWhile(d => d < 0);
		}

		/// <summary> 
		/// Compares two numeric sequences
		/// </summary>
		/// <param name="integers">sequence of integers</param>
		/// <param name="doubles">sequence of doubles</param>
		/// <returns>
		/// true if integers are equals doubles; otherwise, false.
		/// </returns>
		/// <example>
		/// { 1,2,3 } , { 1.0, 2.0, 3.0 } => true
		/// { 0,0,0 } , { 1.0, 2.0, 3.0 } => false
		/// { 3,2,1 } , { 1.0, 2.0, 3.0 } => false
		/// { -10 } => { -10.0 } => true
		/// </example>
		public bool AreNumericListsEqual(IEnumerable<int> integers, IEnumerable<double> doubles)
		{
            return doubles.SequenceEqual(integers.Select(i =>(double) i));
		}

		/// <summary>
		/// Finds the next after specified version from the list 
		/// </summary>
		/// <param name="versions">source list of versions</param>
		/// <param name="currentVersion">specified version</param>
		/// <returns>
		///   Returns the next after specified version from the list; otherwise, null.
		/// </returns>
		/// <example>
		///    { "1.1", "1.2", "1.5", "2.0" }, "1.1" => "1.2"
		///    { "1.1", "1.2", "1.5", "2.0" }, "1.2" => "1.5"
		///    { "1.1", "1.2", "1.5", "2.0" }, "1.4" => null
		///    { "1.1", "1.2", "1.5", "2.0" }, "2.0" => null
		/// </example>
		public string GetNextVersionFromList(IEnumerable<string> versions, string currentVersion)
		{
            return versions.SkipWhile(v => !string.Equals(v, currentVersion, StringComparison.OrdinalIgnoreCase))
                           .Skip(1)
                           .FirstOrDefault();
		}

		/// <summary>
		///  Calcuates the sum of two vectors:
		///  (x1, x2, ..., xn) + (y1, y2, ..., yn) = (x1+y1, x2+y2, ..., xn+yn)
		/// </summary>
		/// <param name="vector1">source vector 1</param>
		/// <param name="vector2">source vector 2</param>
		/// <returns>
		///  Returns the sum of two vectors
		/// </returns>
		/// <example>
		///   { 1, 2, 3 } + { 10, 20, 30 } => { 11, 22, 33 }
		///   { 1, 1, 1 } + { -1, -1, -1 } => { 0, 0, 0 }
		/// </example>
		public IEnumerable<int> GetSumOfVectors(IEnumerable<int> vector1, IEnumerable<int> vector2)
		{
            return vector1.Zip(vector2, (first, second) => first + second);
		}

		/// <summary>
		///  Calcuates the product of two vectors:
		///  (x1, x2, ..., xn) + (y1, y2, ..., yn) = x1*y1 + x2*y2 + ... + xn*yn
		/// </summary>
		/// <param name="vector1">source vector 1</param>
		/// <param name="vector2">source vector 2</param>
		/// <returns>
		///  Returns the product of two vectors
		/// </returns>
		/// <example>
		///   { 1, 2, 3 } * { 1, 2, 3 } => 1*1 + 2*2 + 3*3 = 14
		///   { 1, 1, 1 } * { -1, -1, -1 } => 1*-1 + 1*-1 + 1*-1 = -3
		///   { 1, 1, 1 } * { 0, 0, 0 } => 1*0 + 1*0 +1*0 = 0
		/// </example>
		public int GetProductOfVectors(IEnumerable<int> vector1, IEnumerable<int> vector2)
		{
            return vector1.Zip(vector2, (first, second) => first * second).Sum();
		}

		/// <summary>
		///   Finds all boy+girl pair
		/// </summary>
		/// <param name="boys">boys' names</param>
		/// <param name="girls">girls' names</param>
		/// <returns>
		///   Returns all combination of boys and girls names 
		/// </returns>
		/// <example>
		///  {"John", "Josh", "Jacob" }, {"Ann", "Alice"} => {"John+Ann","John+Alice", "Josh+Ann","Josh+Alice", "Jacob+Ann", "Jacob+Alice" }
		///  {"John"}, {"Alice"} => {"John+Alice"}
		///  {"John"}, { } => { }
		///  { }, {"Alice"} => { }
		/// </example>
		public IEnumerable<string> GetAllPairs(IEnumerable<string> boys, IEnumerable<string> girls)
        {
            // Не совсем понял зачем тут Distinct, можно обойтись и без него.
            // boys.SelectMany(boy => girls.Select(girl => $"{boy}+{girl}"));
            return boys.SelectMany(b => girls.Select(g => string.Format("{0}+{1}", b, g))).Distinct();
		}

		/// <summary>
		///   Calculates the average of all double values from object collection
		/// </summary>
		/// <param name="data">the source sequence</param>
		/// <returns>
		///  Returns the average of all double values
		/// </returns>
		/// <example>
		///  { 1.0, 2.0, null, "a" } => 1.5
		///  { "1.0", "2.0", "3.0" } => 0.0  (no double values, strings only)
		///  { null, 1.0, true } => 1.0
		///  { } => 0.0
		/// </example>
		public double GetAverageOfDoubleValues(IEnumerable<object> data)
		{
            if (!data.Any(d => d is double))
            {
                return 0;
            }
            return data.Where(d => d is double).Select(d => (double)d).Average();
		}

        private int LenWithNulls(string arg)
        {
            return (arg ?? string.Empty).Length;
        }
	}
}
