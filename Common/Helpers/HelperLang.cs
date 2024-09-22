using System.Globalization;

namespace Common.Helpers;

public static class HelperLang
{
    public const string LETTER_UZ_C = "ЙЦУКЕНГШЎЗХЪҒҲФҚВАПРОЛДЖЭЯЧСМИТЬБЮЁ- ’";
    public const string LETTER_RU_C = "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮЁ- ’";
    public const string LETTER_EN_L = "QWERTYUIOPASDFGHJKLZXCVBNM-’ ";
    public const string LETTER_KZ_C = "ЙЦУКЕНГШЩЗХЪҚӨҒҢҮӘФЫВАПРОЛДЖЭЯЧСМИТЬБЮЁ- ’";
    public const string LETTER_UZ_L = "QWERTYUIOPASDFGHJKLZXCVBNM-‘’ ";
    public const string LETTER_LAT = "QWERTYUIOPASDFGHJKLZXCVBNM";
    public const string LETTER_NUMBER = "0123456789";
    public const string LETTER_VOWEL = "УЕАОЭЯИЮЁЫ";

    public const string en = "en";
    public const string ru = "ru-Ru";
    public const string uz_cyr = "uz-Cyrl-UZ";
    public const string uz_lat = "uz-Latn-UZ";
    public const string uz_kaa = "kk-KZ";


    public static CultureInfo[] SupportedCultures = new CultureInfo[]{
            new CultureInfo(ru), new CultureInfo(en),
            new CultureInfo(uz_lat), new CultureInfo(uz_cyr),
        };
    public static string CameCase(string input)
    {
        if (input == null) return "";
        else return input[0].ToString() + input.Substring(1, input.Length - 1).ToLower();
    }

    public static string getBecauseString()
    {
        string result = "Sababi: ";
        switch (CultureInfo.CurrentCulture.Name)
        {
            case ru: result = "Причина: "; break;
            case uz_cyr: result = "Сабаби: "; break;
            case uz_lat: break;
            case uz_kaa: result = "Sebebı: "; break;
            case en: result = "Because: "; break;
        }
        return result;
    }

    /// <summary>
    /// Kirilcha matini lotincha matinga o'gradi
    /// </summary>
    /// <param name="in_ch"></param>
    /// <param name="cultureLang"></param>
    /// <returns></returns>
    public static String CirToLat(this string in_ch, string cultureLang, CaseType caseType = CaseType.FirstLetterToUpperCase)
    {

        if (cultureLang == uz_lat)
        {
            String result = "";
            for (int i = 0; i < in_ch.Length; i++)
                result += _cirToLatByChar(in_ch[i].ToString(), i, i > 0 ? in_ch[i - 1].ToString() : null);

            return caseType switch
            {
                CaseType.FirstLetterOfFullTextToUpperCase => result[0] + result.Substring(1).ToLower(),
                CaseType.ToLowerCase => result.ToLower(),
                CaseType.ToUpperCase => result,
                CaseType.FirstLetterToUpperCase => FirstLatterToUpperCase(result),
                _ => result.ToLower()
            };
        }
        else return in_ch;
    }

    public static String CirToLat2(this string in_ch, string cultureLang)
    {


        String result = "";
        for (int i = 0; i < in_ch.Length; i++)
            result += _cirToLatByChar(in_ch[i].ToString(), i, i > 0 ? in_ch[i - 1].ToString() : null);
        return FirstLatterToUpperCase(result);


    }

    /// <summary>
    /// Lotincha matini => kiril matiniga o'gradi
    /// </summary>
    /// <param name="in_ch"></param>
    /// <param name="cultureLang"></param>
    /// <returns></returns>
    public static String LatToCir(this string in_ch, string cultureLang)
    {
        if (cultureLang == uz_cyr && in_ch != null)
        {
            String result = "";

            for (int i = 0; i < in_ch.Length; i++)
                result += _latToCirByChar(in_ch[i].ToString(), i, i < in_ch.Length - 1 ? in_ch[i + 1].ToString() : null, i > 0 ? in_ch[i - 1].ToString() : null);
            return FirstLatterToUpperCase(result);
        }
        else return in_ch;

    }


    public static int GetDayOfWeekInt(DayOfWeek day_)
    {
        switch (day_)
        {
            case DayOfWeek.Monday:
                return 1;
            case DayOfWeek.Tuesday:
                return 2;
            case DayOfWeek.Wednesday:
                return 3;
            case DayOfWeek.Thursday:
                return 4;
            case DayOfWeek.Friday:
                return 5;
            case DayOfWeek.Saturday:
                return 6;
            case DayOfWeek.Sunday:
                return 7;

        }
        return 0;
    }

    #region private Method

    /// <summary>
    /// Kiril harifini lotin harfiga o'grish
    /// </summary>
    /// <param name="latter">Harf</param>
    /// <param name="index">Harifni matindagi index</param>
    /// <param name="beforeLetter">O'zidan oldingi harf</param>
    /// <returns></returns>
    private static string _cirToLatByChar(string latter, int index, string beforeLetter)
    {
        //  char character = char.Parse(latter);
        // char.IsLower(char.Parse(latter));
        switch (latter.ToUpper())
        {
            case "А": latter = "A"; break;
            case "Б": latter = "B"; break;
            case "В": latter = "V"; break;
            case "Г": latter = "G"; break;
            case "Д": latter = "D"; break;
            case "Е":
                if (index == 0)
                    latter = "YE";
                else
                    if (!(LETTER_VOWEL.Contains(beforeLetter)))
                    latter = "E";
                else
                    latter = "YE";
                break;
            case "Ё": latter = "YO"; break;
            case "Ж": latter = "J"; break;
            case "З": latter = "Z"; break;
            case "И": latter = "I"; break;
            case "Й": latter = "Y"; break;
            case "К": latter = "K"; break;
            case "Л": latter = "L"; break;
            case "М": latter = "M"; break;
            case "Н": latter = "N"; break;
            case "О": latter = "O"; break;
            case "П": latter = "P"; break;
            case "Р": latter = "R"; break;
            case "С": latter = "S"; break;
            case "Т": latter = "T"; break;
            case "У": latter = "U"; break;
            case "Ф": latter = "F"; break;
            case "Х": latter = "X"; break;
            case "Ц":
                if (index == 0)
                    latter = "S";
                else
                    if (!(LETTER_VOWEL.Contains(beforeLetter)))
                    latter = "S";
                else
                    latter = "TS";
                break;
            case "Ч": latter = "CH"; break;
            case "Ш": latter = "SH"; break;
            case "Ъ": latter = "’"; break;
            case "Э": latter = "E"; break;
            case "Ю": latter = "YU"; break;
            case "Я": latter = "YA"; break;
            case "Ў": latter = "O‘"; break;
            case "Қ": latter = "Q"; break;
            case "Ғ": latter = "G‘"; break;
            case "Ҳ": latter = "H"; break;
            case "Ы": latter = "I"; break;
            case "Щ": latter = "SH"; break;
            case "Ь": latter = ""; break;
            case "Ұ": latter = "Y"; break;
            case "Ө": latter = "O"; break;
            case "Һ": latter = "H"; break;
            case "Ң": latter = "H"; break;
            case "Ү": latter = "Y"; break;
            case "Ә": latter = "E"; break;
            case "«":
            case "»":
            case "\"": latter = ""; break;

            default: break;
        }
        return latter;
    }

    /// <summary>
    /// Lotin so'zlarni kirilga almashtramiz
    /// </summary>
    /// <param name="latter"></param>
    /// <param name="index"></param>
    /// <param name="afterLatter"></param>
    /// <returns></returns>
    private static string _latToCirByChar(string latter, int index, string afterLatter, string beforLatter)
    {
        string _afterLatter = afterLatter?.ToUpper() ?? null;
        string _beforLatter = beforLatter?.ToUpper() ?? null;
        switch (latter.ToUpper())
        {
            case "A": if (_beforLatter == "Y") latter = ""; else latter = "A"; break;
            case "B": latter = "Б"; break;
            case "V": latter = "В"; break;
            case "G":
                {
                    if (_afterLatter == "‘")
                        latter = "Ғ";
                    else latter = "Г";
                    break;
                }
            case "D": latter = "Д"; break;
            case "J": latter = "Ж"; break;
            case "Z": latter = "З"; break;
            case "I": latter = "И"; break;
            case "K": latter = "К"; break;
            case "L": latter = "Л"; break;
            case "M": latter = "М"; break;
            case "N": latter = "Н"; break;
            case "O":
                if (_afterLatter == "‘" || _afterLatter == "/'" || _afterLatter == "`") latter = "Ў";
                else latter = "О"; break;
            case "P": latter = "П"; break;
            case "Q": latter = "Қ"; break;
            case "R": latter = "Р"; break;
            case "T":
                if (_afterLatter == "S")
                    latter = "Ц";
                else latter = "Т"; break;
            case "U": latter = "У"; break;
            case "F": latter = "Ф"; break;
            case "X": latter = "Х"; break;
            case "H":
                if (_beforLatter == "C" || _beforLatter == "S") latter = "";
                else latter = "Ҳ";
                break;
            case "Y":
                if (_afterLatter == "O") latter = "Ё";
                else if (_afterLatter == "U") latter = "Ю";
                else if (_afterLatter == "A") latter = "Я";
                else if (_afterLatter == "E") latter = "";
                else latter = "Й";
                break;
            case "C":
                if (_afterLatter == "H") latter = "Ч";
                else latter = "C";
                break;
            case "S":
                if (_afterLatter == "H") latter = "Ш";
                else latter = "С"; break;
            case "’": latter = "Ъ"; break;
            case "E":
                if (index == 0)
                    latter = "Э";
                else latter = "Е"; break;

            case "«":
            case "»":
            case "\"":
            case "`":
            case "‘":
            case "\'":
                latter = ""; break;
            default: break;
        }
        return latter;
    }


    private static string FirstLatterToUpperCase(string str)
    {
        string[] strArray = str.Split(' ');
        int index = 0;
        for (int i = 0; i < strArray.Length; i++)
        {
            strArray[i] = strArray[i].Trim(' ');
            if (strArray[i].Length > 1)
            {
                if (strArray[i][0].ToString() == "\"") index = 2;
                else index = 1;
                strArray[i] = strArray[i][index - 1].ToString().ToUpper() + strArray[i].Substring(index, strArray[i].Length - index).ToLower();
            }
        }
        return String.Join(" ", strArray);
    }

    public static string UppercaseFirst(string s)
    {
        if (s != null && s.Length > 2)
        {
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }
        return s;

    }

    #endregion

}
public enum CaseType
{
    ToLowerCase,
    ToUpperCase,
    FirstLetterToUpperCase,
    FirstLetterOfFullTextToUpperCase,
}

