using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sds.Jmol
{
    public class JmolCrystal
    {
        public JmolCrystal()
        {
            AuxInfo = new Dictionary<string, string>();
        }

        public string Cif { get; set; }
        public string ChemicalName { get; set; }
        public string ChemicalFormula { get; set; }
        public double Alpha { get; set; }
        public double Beta { get; set; }
        public double Gamma { get; set; }
        public double LengthA { get; set; }
        public double LengthB { get; set; }
        public double LengthC { get; set; }
        public IEnumerable<JmolAtom> Atoms { get; set; }
        public IEnumerable<JmolBond> Bonds { get; set; }
        public IDictionary<string, string> AuxInfo { get; set; }
    }
}
