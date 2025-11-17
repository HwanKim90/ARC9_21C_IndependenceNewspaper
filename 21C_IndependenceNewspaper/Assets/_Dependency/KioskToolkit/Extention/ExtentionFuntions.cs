using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 종종 사용되는 기능에 대한 확장 기능 모음
/// </summary>
namespace Arc9.Unity.KioskToolkit
{
    public static class Ex
    {
        public static double Map(double x, double in_min, double in_max, double out_min, double out_max, bool clamp = false)
        {
            if (clamp) x = Math.Max(in_min, Math.Min(x, in_max));
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static void DeleteFileInDirectory(string dir, string fileName = "*.*")
        {
            try
            {
                if(Directory.Exists(dir))
                {
                    string[] files = Directory.GetFiles(dir, fileName);

                    foreach (string f in files)
                    {
                        File.Delete(f);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Debug.Log("[DeleteAllFileInDirectory]" + ex.Message);
            }
        }
    }
}
