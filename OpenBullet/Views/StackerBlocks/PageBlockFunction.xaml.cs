using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Controls;
using RuriLib;
using RuriLib.Functions.Crypto;
using RuriLib.Functions.UserAgent;
using static RuriLib.BlockFunction;

namespace OpenBullet.Views.StackerBlocks
{
    /// <summary>
    /// Logica di interazione per PageBlockFunction.xaml
    /// </summary>
    public partial class PageBlockFunction : Page
    {
        BlockFunction vm;

        public PageBlockFunction(BlockFunction block)
        {
            InitializeComponent();
            vm = block;
            DataContext = vm;

            foreach (var t in Enum.GetNames(typeof(BlockFunction.Function)))
                functionTypeCombobox.Items.Add(t);

            functionTypeCombobox.SelectedIndex = (int)vm.FunctionType;

            foreach (var h in Enum.GetNames(typeof(Hash)))
            {
                hashTypeCombobox.Items.Add(h);
                hmacHashTypeCombobox.Items.Add(h);
                kdfAlgorithmCombobox.Items.Add(h);
            }

            hashTypeCombobox.SelectedIndex = (int)vm.HashType;
            hmacHashTypeCombobox.SelectedIndex = (int)vm.HashType;
            kdfAlgorithmCombobox.SelectedIndex = (int)vm.KdfAlgorithm;

            foreach (var b in Enum.GetNames(typeof(UserAgent.Browser)))
            {
                randomUABrowserCombobox.Items.Add(b);
            }

            randomUABrowserCombobox.SelectedIndex = (int)vm.UserAgentBrowser;

            foreach (var m in Enum.GetNames(typeof(CipherMode)))
            {
                aesModeCombobox.Items.Add(m);
            }

            aesModeCombobox.SelectedIndex = (int)vm.AesMode - 1;

            foreach (var p in Enum.GetNames(typeof(PaddingMode)))
            {
                aesPaddingCombobox.Items.Add(p);
            }

            foreach (var d in Enum.GetNames(typeof(DateToUnixTimeType)))
            {
                dateToUnixTimeCombobox.Items.Add(d);
            }

            encCombobox.Items.Add("utf-8");
            encCombobox.Items.Add("windows-1251");
            encCombobox.Items.Add(1251);

            foreach (var e in Enum.GetNames(typeof(EncodingMethods)))
            {
                encFuncCombobox.Items.Add(e);
            }

            foreach (var e in Enum.GetNames(typeof(ScryptMethods)))
            {
                if (e == nameof(ScryptMethods.Encode))
                {
                    scryptMethods.Items.Add(e);
                    break;
                }
            }

            foreach (var e in Enum.GetNames(typeof(BCryptMethods)))
            {
                bcryptMethods.Items.Add(e);
            }

            bcryptMethods.SelectedIndex = (int)vm.BCryptMeth;

            aesPaddingCombobox.SelectedIndex = (int)vm.AesPadding - 1;

            dictionaryRTB.AppendText(vm.GetDictionary());
        }

        private void functionTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.FunctionType = (BlockFunction.Function)((ComboBox)e.OriginalSource).SelectedIndex;
            try { functionInfoTextblock.Text = infoDic[vm.FunctionType.ToString()]; } catch { functionInfoTextblock.Text = "No additional information available for this function"; }

            switch (vm.FunctionType)
            {
                default:
                    functionTabControl.SelectedIndex = 0;
                    break;

                case BlockFunction.Function.Hash:
                    functionTabControl.SelectedIndex = 1;
                    break;

                case BlockFunction.Function.HMAC:
                    functionTabControl.SelectedIndex = 2;
                    break;

                case BlockFunction.Function.Translate:
                    functionTabControl.SelectedIndex = 3;
                    break;

                case BlockFunction.Function.DateToUnixTime:
                case BlockFunction.Function.UnixTimeToDate:
                    functionTabControl.SelectedIndex = 4;
                    break;

                case BlockFunction.Function.Replace:
                    functionTabControl.SelectedIndex = 5;
                    break;

                case BlockFunction.Function.RegexMatch:
                    functionTabControl.SelectedIndex = 6;
                    break;

                case BlockFunction.Function.RandomNum:
                    functionTabControl.SelectedIndex = 7;
                    break;

                case BlockFunction.Function.CountOccurrences:
                    functionTabControl.SelectedIndex = 8;
                    break;

                case BlockFunction.Function.RSAEncrypt:
                    functionTabControl.SelectedIndex = 9;
                    break;

                /*
            case BlockFunction.Function.RSADecrypt:
                functionTabControl.SelectedIndex = 10;
                break;
                */

                case BlockFunction.Function.RSAPKCS1PAD2:
                    functionTabControl.SelectedIndex = 11;
                    break;

                case BlockFunction.Function.CharAt:
                    functionTabControl.SelectedIndex = 12;
                    break;

                case BlockFunction.Function.Substring:
                    functionTabControl.SelectedIndex = 13;
                    break;

                case BlockFunction.Function.GetRandomUA:
                    functionTabControl.SelectedIndex = 14;
                    break;

                case BlockFunction.Function.AESEncrypt:
                case BlockFunction.Function.AESDecrypt:
                    functionTabControl.SelectedIndex = 15;
                    break;

                case BlockFunction.Function.PBKDF2PKCS5:
                    functionTabControl.SelectedIndex = 16;
                    break;

                case BlockFunction.Function.Remove:
                    functionTabControl.SelectedIndex = 17;
                    break;

                case BlockFunction.Function.Split:
                    functionTabControl.SelectedIndex = 18;
                    break;

                case BlockFunction.Function.Encoding:
                    functionTabControl.SelectedIndex = 19;
                    break;

                case BlockFunction.Function.SCrypt:
                    functionTabControl.SelectedIndex = 20;
                    break;

                case BlockFunction.Function.BCrypt:
                    functionTabControl.SelectedIndex = 21;
                    break;

            }
        }

        public Dictionary<string, string> infoDic = new Dictionary<string, string>()
        {
            { "Constant", "This will just return anything written in the input string and store it in a variable, after possibly replacing all the input variables.\nUse this to chain constants and variables together." },
            { "Base64Encode", "Encodes a string to Base64" },
            { "Base64Decode", "Decodes a Base64 string to text" },
            { "Hash", "The input string will be hashed with the selected function. Remember you can chain variables if you need a salt." },
            { "HMAC", "The input string will be hashed with the selected Hashtype with the key you input." },
            { "Translate", "Format like headers (this: that), one per line." },
            { "DateToUnixTime", "This turns date to Unixtime. ex(input = 2020-01-01:01-01-01 to output = 1577840461" },
            { "Length", "This will count the Length of the input Ex(Input = apple Output = 5)" },
            { "ToLowercase", "Turns input to Lowercase Ex(A to a)" },
            { "ToUppercase", "Turns input to Uppercase Ex(a to A)" },
            { "Replace", "Replaces the Input from (Replace:) with the Input from (with:) from the Input string" },
            { "RegexMatch", "Matches the input with the given Regex (Match:)" },
            { "URLEncode", "URLEncodes the input. Ex(https://example.com to https%3A%2F%2Fexample.com)" },
            { "URLDecode", "URLDecodes the input. Ex(https%3A%2F%2Fexample.com to https://example.com)" },
            { "Unescape", "Unescapes the input." },
            { "HTMLEntityEncode", "HTMLEntity Encodes the input. Ex(© to &#169;)" },
            { "HTMLEntityDecode", "HTMLEntity Decodes the input. Ex(&#169; to ©)" },
            { "UnixTimeToDate", "This turns Unixtime to date. ex(input = 1577840461 to output = 2020-01-01:01-01-01" },
            { "CurrentUnixTime", "This Returns theh Current Unixtime. ex(1577840461)" },
            { "UnixTimeToISO8601", "This Returns theh Current Unixtime. ex(1577840461 to 2020-01-01T01:01:01.000Z)" },
            { "RandomNum", "Generates a random number based on ranges (min max) given." },
            { "RandomString", "?l = Lowercase, ?u = Uppercase, ?d = Digit, ?f = Uppercase + Lowercase, ?s = Symbol, ?h = Hex (Lowercase), ?m = Upper + Digits,?n = Lower + Digits ?i = Lower + Upper + Digits, ?a = Any"},
            { "EvaluateMathString", "Evaluating string '3 * 5 + Pow(2,3)' yield int 23"},
            { "Ceil", "This function rounds up to the input to the next full integer. Ex(input = 2.9 Output = 3"},
            { "Floor", "This function gets rid of the numbers after the decimal. Ex(input = 2.9 Output = 2"},
            { "Round", "This function round input to the nearest integer. Ex(input = 2.5 Output = 3"},
            { "Compute", "Calculates the value of a math expression, for example (6+3)*5 will return 45." },
            { "ClearCookies", "Removes/Clears all the Cookies." },
            { "CountOccurrences", "Counts the number of times that (Find:) input occurs in the input." },
            { "RSAEncrypt", "Encrypts data with RSA. All parameters must be provided as base64 strings" },
            { "RSADecrypt", "Decrypts data with RSA. All parameters must be provided as base64 strings" },
            { "RSAPKCS1PAD" , "Encrypts data with RSA Pkcs1Pad2. Modulus and exponent must be provided as HEX strings."},
            { "Delay", "Write the amount of MILLISECONDS you want to wait in the input field" },
            { "CharAt", "Returns the character at the specified index of the string in the input field" },
            { "Substring", "Returns a new string that is a substring of Input String. The substring begins at the specified Start Index and Length." },
            { "Reversestring", "Reverses the the characters if the Input String. Ex(input = abc output = cba" },
            { "Trim", "Removes whitespace from both ends of a Input string." },
            { "GetRandomUA", "Generates a random User Agent." },
            { "AESEncrypt", "Encrypts data with AES. All parameters must be provided as base64 strings. Uses SHA-256 to get a 256 bit key" },
            { "AESDecrypt", "Decrypts data with AES. All parameters must be provided as base64 strings. Uses SHA-256 to get a 256 bit key" },
            { "PBKDF2PKCS5", "Generates a key based on a password. The salt, if provided, must be a base64 string" },
            { "Ntlm", "Generates NTLM hash" },
            { "CurrentDate", "Gets current date in format dd.mm.yyyy" },
            { "CurrentTime", "Gets current time in format hh:mm" },
            { "DayOfWeek", "Get the current day of week in English" },
            { "CurrentDay", "Get the current day number" },
            { "CurrentMonth", "Get the current month number (without leading zero)" },
            { "CurrentYear", "Get the current year number" },
            { "ToLetter", "Find and extract all letters from a string" },
            { "ToLetter", "Find and extract all digits from a string" },
            { "ToLetterOrDigit", "Find and extract all letters and digits from a string" },
            { "NumberToWords", "Converts a number to english words (1 = one, 103 = one hundred and three)" },
            { "WordsToNumber", "Converts numbers written in english back to its numerical representation." },
            { "Abs", "Gets the modulus of a number" },
            { "Split", "Splits a string into substrings based on the strings in an array. You can specify whether the substrings include empty array elements." },
            { "Remove", "Removes a given amount of characters from the string starting from the given index. " },
            { "ReverseString", "Reverses the input string." },
            { "SCrypt", "Hashes the input string using SCrypt with specified options." },
            { "BCrypt", "Hashes the input string using BCrypt with specified options." },
            { "Capitalize", "Capitalizes the input string (the first letter becomes uppercase, the rest lowercase)." }
        };

        private void dictionaryRTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            vm.SetDictionary(dictionaryRTB.Lines());
        }

        private void hashTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.HashType = (Hash)((ComboBox)e.OriginalSource).SelectedIndex;
        }

        private void hmacHashTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.HashType = (Hash)((ComboBox)e.OriginalSource).SelectedIndex;
        }

        private void aesModeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.AesMode = (CipherMode)(((ComboBox)e.OriginalSource).SelectedIndex + 1);
        }

        private void aesPaddingCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.AesPadding = (PaddingMode)(((ComboBox)e.OriginalSource).SelectedIndex + 1);
        }

        private void kdfAlgorithmCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.KdfAlgorithm = (Hash)((ComboBox)e.OriginalSource).SelectedIndex;
        }

        private void randomUABrowserCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.UserAgentBrowser = (UserAgent.Browser)((ComboBox)e.OriginalSource).SelectedIndex;
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try { SetItemToComboBox(); } catch { }
        }

        private void SetItemToComboBox()
        {
            dateToUnixTimeCombobox.SelectedIndex = (int)vm.UnixTimeType;
        }

        private void scryptMethods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (scryptMethods.SelectedIndex)
                {
                    case 0://ScryptMethods.Encode
                        dockPanelCompInput.Visibility = System.Windows.Visibility.Collapsed;
                        stackPanelEncode.Visibility = System.Windows.Visibility.Visible;
                        break;
                    case 1:// ScryptMethods.Compare
                        stackPanelEncode.Visibility = System.Windows.Visibility.Collapsed;
                        dockPanelCompInput.Visibility = System.Windows.Visibility.Visible;
                        break;
                    default:
                        stackPanelEncode.Visibility = System.Windows.Visibility.Collapsed;
                        dockPanelCompInput.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                }
            }
            catch { }
        }

        private void bcryptMethods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (bcryptMethods.SelectedIndex)
                {
                    case 0://BCryptMethods.Encode
                        dockPanelVerifyInput.Visibility = System.Windows.Visibility.Collapsed;
                        dockPanelBcryptSalt.Visibility = System.Windows.Visibility.Visible;
                        break;
                    case 2://BCryptMethods.Verify
                        dockPanelBcryptSalt.Visibility = System.Windows.Visibility.Collapsed;
                        dockPanelVerifyInput.Visibility = System.Windows.Visibility.Visible;
                        break;

                    default:
                        dockPanelBcryptSalt.Visibility = System.Windows.Visibility.Collapsed;
                        dockPanelVerifyInput.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                }
            }
            catch { }
        }
    }
}
