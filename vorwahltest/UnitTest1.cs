using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace vorwahltest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void VorwahlFinden()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            Assert.AreEqual("02129 58369", v.Normalize("0212958369"));
        }

        [TestMethod]
        public void Schrott()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            var ergebnis = v.Analyze("bufgihwiuf");
            Assert.AreEqual(vorwahl.NummerTyp.Unknown, ergebnis.Item1);
            Assert.AreEqual("", ergebnis.Item2);
        }

        [TestMethod]
        public void MitSonderzeichen()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            Assert.AreEqual("02129 58369", v.Normalize("(02129) 58369"));
        }

        [TestMethod]
        public void MitLaendervorwahl()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            Assert.AreEqual("02129 58369", v.Normalize("+49 (2129) 58369"));
        }

        [TestMethod]
        public void MitLaendervorwahl00()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            Assert.AreEqual("02129 58369", v.Normalize("0049 2129 58369"));
        }

        [TestMethod]
        public void MitLaendervorwahlKlammerNull()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            Assert.AreEqual("0711 12345", v.Normalize("+49 (0)711-12345"));
        }

        [TestMethod]
        public void MitFirmendurchwahl()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            Assert.AreEqual("0211 94842-30", v.Normalize("0211-94842-30"));
        }

        [TestMethod]
        public void International()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            Assert.AreEqual("+593 32415406", v.Normalize("0059332415406"));
        }

        [TestMethod]
        public void AnalysiereFixed()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            var ergebnis = v.Analyze("0212958369");
            Assert.AreEqual(vorwahl.NummerTyp.Fixed, ergebnis.Item1);
            Assert.AreEqual("02129 58369", ergebnis.Item2);
        }

        [TestMethod]
        public void AnalysiereMobil()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            var ergebnis = v.Analyze("(0171)8765432");
            Assert.AreEqual(vorwahl.NummerTyp.Mobile, ergebnis.Item1);
            Assert.AreEqual("0171 8765432", ergebnis.Item2);
        }

        [TestMethod]
        public void AnalysiereSpezial()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            var ergebnis = v.Analyze("01370668");
            Assert.AreEqual(vorwahl.NummerTyp.Special, ergebnis.Item1);
            Assert.AreEqual("0137 0668", ergebnis.Item2);
        }

        [TestMethod]
        public void AnalysiereSchweiz()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            var ergebnis = v.Analyze("+41 44 412 11 11");
            Assert.AreEqual(vorwahl.NummerTyp.Fixed, ergebnis.Item1);
            Assert.AreEqual("+41 44412 1111", ergebnis.Item2);
        }

        [TestMethod]
        public void AnalysiereSchweizMobil()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            Assert.AreEqual(new Tuple<vorwahl.NummerTyp, string>(vorwahl.NummerTyp.Mobile, "+41 76378 2737"),
                v.Analyze("+41 76 378 27 37"));
        }

        [TestMethod]
        public void AnalysiereRumaenien()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            Assert.AreEqual(new Tuple<vorwahl.NummerTyp, string>(vorwahl.NummerTyp.Unknown, "+40 364107375"),
                v.Analyze("0040364/107375 (Rumänien)"));
        }


        [TestMethod]
        public void AnalysiereOstatt0()
        {
            vorwahl.Vorwahl v = new vorwahl.Vorwahl();
            Assert.AreEqual(new Tuple<vorwahl.NummerTyp, string>(vorwahl.NummerTyp.Fixed, "0711 765432"),
                v.Analyze("O711765432"));
        }
    }
}
