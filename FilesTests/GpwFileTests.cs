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
using ExcelDataReader;

namespace FilesTests
{
    public class GpwFileTests
    {
        private IList<GpwModel> GetDataFromXLS()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var a = $"{AppDomain.CurrentDomain.BaseDirectory}\\Data//akcje_2021-12-03.xlsx";
            using (var stream = File.Open(a, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    IList<GpwModel> akcjeListaObiektow = new List<GpwModel>();

                    while (reader.Read())
                    {
                        // Gdy wartość pierwszej komórki jest pusta to pomin wiersz
                        if (reader.GetValue(0) == null)
                        {
                            continue;
                        }

                        GpwModel akcja = new GpwModel();

                        akcja.Nazwa = reader.GetValue(0).ToString();
                        akcja.Skrot = reader.GetValue(1).ToString();
                        akcja.Waluta = reader.GetValue(2).ToString();
                        akcja.CzasOstatniejTransakcji = reader.GetValue(3).ToString();
                        akcja.KursOdniesienia = reader.GetValue(4).ToString();
                        akcja.TKO = reader.GetValue(5).ToString();
                        akcja.KursOtwarcia = reader.GetValue(6).ToString();
                        akcja.KursMin = reader.GetValue(7).ToString();
                        akcja.KursMax = reader.GetValue(8).ToString();
                        akcja.KursOstTransZamkn = reader.GetValue(9).ToString();
                        akcja.ZmianaDoKursuOdn = reader.GetValue(10).ToString();
                        akcja.KupnoLiczbaZlecen = reader.GetValue(11).ToString();
                        akcja.KupnoWolumen = reader.GetValue(12).ToString();
                        akcja.KupnoLimit = reader.GetValue(13).ToString();
                        akcja.SprzedazLimit = reader.GetValue(14).ToString();
                        akcja.SprzedazWolumen = reader.GetValue(15).ToString();
                        akcja.SprzedazLiczbaZlecen = reader.GetValue(16).ToString();
                        akcja.WolumenOstTrans = reader.GetValue(17).ToString();
                        akcja.LiczbaTransakcji = reader.GetValue(18).ToString();
                        akcja.WolObrSkumul = reader.GetValue(19).ToString();
                        akcja.WartObrSkumul = reader.GetValue(20).ToString();

                        akcjeListaObiektow.Add(akcja);

                    }
                    return akcjeListaObiektow;
                }
            }
        }



        [Test]
        public void CheckIfNamesAreUnique()
        {
            IList<GpwModel> akcjeLista = GetDataFromXLS();
            //IList<GpwModel> akcjeLista = PobierzZXls();

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
            var akcjeLista = GetDataFromXLS();

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

        [Test]
        public void CheckIfNumberOfMarketOrderIsInteger()
        {
            CheckIfValueIsInteger(i => i.LiczbaTransakcji, nameof(GpwModel.LiczbaTransakcji));
        }

        [Test]
        public void CheckIfVolumenLastDealIsInteger()
        {
            CheckIfValueIsInteger(i => i.WolumenOstTrans, nameof(GpwModel.WolumenOstTrans));
        }

        private void CheckIfValueIsInteger(Func<GpwModel, string> funcGetPropertyValue, string propertyName)
        {
            var akcjeLista = GetDataFromFile();
            foreach (var akcja in akcjeLista)
            {
                var propertyValue = funcGetPropertyValue(akcja);

                if (propertyValue == "-")
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    string message = $"Nazwa spółki:{akcja.Nazwa}, {propertyName}: {propertyValue}";
                    Assert.DoesNotThrow(() => uint.Parse(propertyValue), message);
                }

            }
        }
    }
}
