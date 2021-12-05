using ApiTests;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Linq;
using FilesTests.Models;

namespace FilesTests
{
    public class GpwFileTests
    {
        [Test]
        public void CheckIfNamesAreUnique()
        {
            var akcjeLista = GetDataFromFile();

            List<string> nazwyLista = new List<string>();

            foreach (var akcja in akcjeLista)
            {
                nazwyLista.Add(akcja.Nazwa);
            }

            Assert.AreEqual(nazwyLista.GroupBy(i => i).Count(), nazwyLista.Count);
        }

        [Test]
        public void CheckIfShortcutsAreUnique()
        {
            var akcjeLista = GetDataFromFile();

            Assert.AreEqual(akcjeLista.GroupBy(i => i.Skrot).Count(), akcjeLista.Count);
        }

        private IList<GpwModel> GetDataFromFile()
        {
            var dataString = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\Data//GpwData.json");
            return JsonConvert.DeserializeObject<IList<GpwModel>>(dataString);
        }

        [Test]
        public void CheckIfNamesAreNotNull()
        {
            CheckIfStringIsNullOrEmpty(i => i.Nazwa);
        }

        [Test]
        public void CheckIfShortcutsAreNotNull()
        {
            CheckIfStringIsNullOrEmpty(i => i.KursOstTransZamkn);
        }

        private void CheckIfStringIsNullOrEmpty(Func<GpwModel, string> funcGetPropertyName)
        {
            var akcjeLista = GetDataFromFile();

            bool isAny = akcjeLista.Any(i => String.IsNullOrEmpty(funcGetPropertyName(i)));
            Assert.IsFalse(isAny);
        }

        [Test]
        public void CheckIfMarketRateReferenceIsDecimal()
        {
            var akcjeLista = GetDataFromFile();

            foreach (var akcja in akcjeLista)
            {
                string tekstDocelowy = akcja.KursOdniesienia;

                if (tekstDocelowy.Contains(","))
                {
                    tekstDocelowy = tekstDocelowy.Replace(",", ".");
                }

                string message = $"Name:{akcja.Nazwa} kurs:{akcja.KursOdniesienia}";
                Assert.DoesNotThrow(() => decimal.Parse(tekstDocelowy), message);
            }
        }

        [Test]
        public void CheckIfIsLastDealIsDate()
        {
            var akcjeLista = GetDataFromFile();

            foreach (var akcja in akcjeLista)
            {
                string tekstDoSprawdzenia = akcja.CzasOstatniejTransakcji;

                if (tekstDoSprawdzenia == "-")
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    string message = $"Name:{akcja.Nazwa} czas:{akcja.CzasOstatniejTransakcji}";
                    Assert.DoesNotThrow(() => TimeSpan.Parse(tekstDoSprawdzenia), message);
                }
            }
        }

        [Test]
        public void CheckIfIsCurrencyIsPLN()
        {
            var akcjeLista = GetDataFromFile();

            Assert.IsFalse(akcjeLista.Any(i => i.Waluta != "PLN"));
        }

        [Test]
        public void CheckIfMaxMarketRateIsGreaterThanMinRate()
        {
            var akcjeLista = GetDataFromFile();

            foreach (var akcja in akcjeLista)
            {
                string kursMax = akcja.KursMax;
                string kursMin = akcja.KursMin;

                if (kursMax == "-" || kursMin == "-")
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    try
                    {
                        decimal decimalMax = decimal.Parse(kursMax);
                        decimal decimalMin = decimal.Parse(kursMin);
                        Assert.GreaterOrEqual(decimalMax, decimalMin);
                    }
                    catch
                    {
                        var message = $"nazwa: {akcja.Nazwa}, kurs max: {akcja.KursMax}, kurs min: {akcja.KursMin}";
                        Assert.IsTrue(false, message);
                    }
                }

            }
        }

        [Test]
        public void CheckIfVolumensAreIntegerPositive()
        {
            var akcjeLista = GetDataFromFile();

            foreach (var akcja in akcjeLista)
            {

                if (akcja.SprzedazWolumen == "-")
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    string message = $"Name:{akcja.Nazwa} wolumen sprzedaż: {akcja.SprzedazWolumen}";
                    Assert.DoesNotThrow(() => uint.Parse(akcja.SprzedazWolumen), message);
                }
            }
        }
    }
}
