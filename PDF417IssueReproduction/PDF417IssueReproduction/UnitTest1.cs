using FluentAssertions;
using IronBarCode;
using Microsoft.VisualBasic;

namespace PDF417IssueReproduction
{
    public class Tests
    {
        private string filename;

        [SetUp]
        public void Setup()
        {
            filename = Path.GetRandomFileName();
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(filename);
        }

        [TestCase("V01~A01|D01~T5J3S4|R01~ACME WIDGET SUPPLY|R02~|R03~|R04~5995 AVEBURY DRIVE|R05~|R06~MISSISSAUGA|R07~L5R3T8|S01~555000777|S02~555000999|S03~|S04~|S05~|S06~|S07~|S08~20140611|S09~2|S10~5|S11~500|S12~3000|S13~50|S14~TOR|S15~|B01~FRE|B02~PP")]
        [TestCase("Test Test Test")]
        [TestCase("AAAAAAAAAA AAAAAAAAAA AAAAAAAAAA AAAAAAAAAA")]
        [TestCase("V01~A01|D01~T5J3S4|")]
        public void Test1(string barcodeInput)
        {
            IronBarCode.License.LicenseKey = "IRONSUITE.SKILLEEN.SCTSOFTWARE.COM.5208-D8329E34CC-BJNSYRBZ3DBARHMK-QE43RPD7QEB4-MISNTIJ775CT-2IMIOMPQST6J-EYDWAFLBNRQW-EIA4C57LFPJG-YLGBEP-T2HB3CPEMBKNUA-DEPLOYMENT.TRIAL-CEAHVX.TRIAL.EXPIRES.26.SEP.2024";
            GeneratedBarcode myBarcode = BarcodeWriter.CreateBarcode(barcodeInput, BarcodeEncoding.PDF417);

            myBarcode.SaveAsPng(filename);

            var readBarcode = BarcodeReader.Read(filename);

            readBarcode.First().ToString().Should().Be(barcodeInput);
        }
    }
}