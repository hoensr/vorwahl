using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace vorwahl
{
    public enum NummerTyp
    {
        Fixed,
        Mobile,
        Special,
        Unknown
    };

    public class Vorwahl
    {
        private List<CountryCode> countryOrdering;

        public Vorwahl()
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            data = ser.Deserialize<VorwahlData>(VorwahlData.vorwahlJson);
            countryOrdering = new List<CountryCode>
            {
                data.cc49,
                data.cc41,
                data.cc43,
                data.cc33,
                data.cc44,
                data.cc1
            };
        }

        public string Normalize(string nummer)
        {
            return Analyze(nummer)?.Item2;
        }

        private string TestRegex(string nummerNurZiffern, CountryCode country, Regex r)
        {
            Match m = r.Match(nummerNurZiffern);
            if (m.Success && m.Groups.Count >= 4)
            {
                //                return m.Groups.Cast<Group>().Select(x => x.Value).Aggregate((a, b) => a + " " + b);
                if (country.prefix.Contains("49"))
                {
                    return String.Concat(country.national, m.Groups[2].Value, " ", m.Groups[3].Value);
                }
                else
                {
                    string international = country.prefix.Substring(
                        country.prefix.IndexOf('+'),
                        country.prefix.IndexOf('|') - country.prefix.IndexOf('+')
                        );

                    return String.Concat(international, " ", m.Groups[2].Value, " ", m.Groups[3].Value);
                }
            }

            return null;
        }

        private Tuple<NummerTyp, string> AnalyzeCountry(string nummerNurZiffern, CountryCode country)
        {
            string s = TestRegex(nummerNurZiffern, country, new Regex("^" + country.prefix + country.@fixed + @"(?:-)?(\d[-\d]*)"));
            if (s != null)
            {
                return new Tuple<NummerTyp, string>(NummerTyp.Fixed, s);
            }

            s = TestRegex(nummerNurZiffern, country, new Regex("^" + country.prefix + country.mobile + @"(?:-)?(\d[-\d]*)"));
            if (s != null)
            {
                return new Tuple<NummerTyp, string>(NummerTyp.Mobile, s);
            }

            s = TestRegex(nummerNurZiffern, country, new Regex("^" + country.prefix + country.special + @"(?:-)?(\d[-\d]*)"));
            if (s != null)
            {
                return new Tuple<NummerTyp, string>(NummerTyp.Special, s);
            }

            return null;
        }

        public Tuple<NummerTyp, string> Analyze(string nummer)
        {
            string nummerNurZiffern = Regex.Replace(nummer, @"[^+\-0-9]", "");

            foreach (var country in countryOrdering)
            {
                var resultCountry = AnalyzeCountry(nummerNurZiffern, country);
                if (resultCountry != null)
                {
                    return resultCountry;
                }
            }

            var r = new Regex("^" + data.international.prefix + data.international.cc + @"(?:-)?(\d[-\d]*)");
            var m = r.Match(nummerNurZiffern);
            if (m.Success && m.Groups.Count >= 4)
            {
                return new Tuple<NummerTyp, string>(NummerTyp.Unknown,
                    String.Concat("+", m.Groups[2].Value, " ", m.Groups[3].Value));
            }

            return new Tuple<NummerTyp, string>(NummerTyp.Unknown, nummerNurZiffern);
        }

        private VorwahlData data;

    }
}
