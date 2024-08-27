using FluentAssertions;
using IronBarCode;

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

        [TestCase("V01~A01|D01~T5J3S4|R01~ACME WIDGET SUPPLY|R02~|R03~|R04~5995 AVEBURY DRIVE|R05~|R06~MISSISSAUGA|R07~L5R3T8|S01~555000777|S02~555000999|S03~|S04~|S05~|S06~|S07~|S08~20140611|S09~2|S10~5|S11~500|S12~3000|S13~50|S14~TOR|S15~|B01~FRE|B02~PP")] // original string, fails
        [TestCase("Test Test Test")] // Passes
        [TestCase("AAAAAAAAAA AAAAAAAAAA AAAAAAAAAA AAAAAAAAAA")] // Passes
        [TestCase("V01~A01|D01~T5J3S4|")] // Passes
        [TestCase("V01~A01|D01~T5J3S4|R01~ACME WIDGET SUPPLY|R02~|")] // Passes
        [TestCase("V01~A01|D01~T5J3S4|R01~ACME WIDGET SUPPLY|R02~|R03~|R04~5995 AVEBURY DRIVE|R05~|R06~MISSISSAUGA")] // Passes
        [TestCase("V01~A01|D01~T5J3S4|R01~ACME WIDGET SUPPLY|R02~|R03~|R04~5995 AVEBURY DRIVE|R05~|R06~MISSISSAUGA|R07~L5R3T")] // Passes prior to adding the 8
        [TestCase("V01~A01|D01~T5J3S4|R01~ACME WIDGET SUPPLY|R02~|R03~|R04~5995 AVEBURY DRIVE|R05~|R06~MISSISSAUGA|R07~L5R3T8")] // Adding the 8 is where it fails
        [TestCase("V01~A01|D01~T5J3S4|R01~ACME WIDGET SUPPLY|R02~|R03~|R04~5995 AVEBURY DRIVE|R05~|R06~MISSISSAUGA|R07~L5R3TA")] // Attempt to use an A instead of 8 -- it works
        [TestCase("R07~L5R3T8")] // the "offending" piece seems to be fine on its own
        [TestCase("V01~A01|D01~T5J3S4|R01~ACME WIDGET SUPPLY|R02~|R03~|R04~5995 AVEBURY DRIVE|R05~|R06~MISSISSAUGA|S01~555000777|S02~555000999|S03~|S04~|S05~|S06~|S07~|S08~20140611|S09~2|S10~5|S11~500|S12~3000|S13~50|S14~TOR|S15~|B01~FRE|B02~PP")] // still fails if we cut out the R07 piece. Pointing to a length issue or data compaction issue?
        [TestCase("V01A01D01T5J3S4R01ACME WIDGET SUPPLYR02R03R045995 AVEBURY DRIVER05R06MISSISSAUGAR07L5R3T8S01555000777S02555000999S03S04S05S06S07S0820140611S092S105S11500S123000S1350S14TORS15B01FREB02PP")] // without tildes and pipe characters, original also fails

        public void PDF417ConversionTests(string barcodeInput)
        {
            // Trial key -- Iron Software can revoke as desired.
            IronBarCode.License.LicenseKey = "IRONSUITE.SKILLEEN.SCTSOFTWARE.COM.5208-D8329E34CC-BJNSYRBZ3DBARHMK-QE43RPD7QEB4-MISNTIJ775CT-2IMIOMPQST6J-EYDWAFLBNRQW-EIA4C57LFPJG-YLGBEP-T2HB3CPEMBKNUA-DEPLOYMENT.TRIAL-CEAHVX.TRIAL.EXPIRES.26.SEP.2024";
            GeneratedBarcode myBarcode = BarcodeWriter.CreateBarcode(barcodeInput, BarcodeEncoding.PDF417);

            myBarcode.SaveAsPng(filename);

            var readBarcode = BarcodeReader.Read(filename);

            readBarcode.First().ToString().Should().Be(barcodeInput);
        }
    }
}