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
        public void CheckIfShortsAreUniqu5e()
        {
            var akcjeLista = GetDataFromFile();

            Assert.AreEqual(akcjeLista.GroupBy(i => i.Skrot).Count(), akcjeLista.Count);
        }

        private IList<GpwModel> GetDataFromFile()
        {
            var dataString = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\Data//GpwData.json");
            return JsonConvert.DeserializeObject<IList<GpwModel>>(dataString);
        }

    }
}
