using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sds.Jmol
{
    public class CifReader
    {
        public static JmolCrystal Read(string path)
        {
            var crystal = new JmolCrystal();

            using (StreamReader sr = new StreamReader(path))
            {
                crystal.Cif = sr.ReadToEnd();
            }

            java.io.BufferedReader buffReader = new java.io.BufferedReader(new java.io.FileReader(path));

            java.util.Map parameters = new java.util.Hashtable();

            var ascr = org.jmol.adapter.smarter.SmarterJmolAdapter.staticGetAtomSetCollectionReader(path, null, buffReader, parameters);
            if (ascr is org.jmol.adapter.smarter.AtomSetCollectionReader)
            {
                var asc = org.jmol.adapter.smarter.SmarterJmolAdapter.staticGetAtomSetCollection((ascr as org.jmol.adapter.smarter.AtomSetCollectionReader)) as org.jmol.adapter.smarter.AtomSetCollection;

                if(asc != null)
                {
                    var auxInfo = new Dictionary<string, string>();

                    if (asc.atoms != null)
                    {
                        crystal.Atoms = asc.atoms.ToArray().Where(a => a != null).Select(a => new JmolAtom()
                        {
                            AtomName = a.atomName,
                            Symbol = a.elementSymbol,
                            X = a.x,
                            Y = a.y,
                            Z = a.z
                        }).ToList();
                    }

                    if (asc.bonds != null)
                    {
                        crystal.Bonds = asc.bonds.ToArray().Where(b => b != null).Select(b => new JmolBond()
                        {
                            AtomIndex1 = b.atomIndex1,
                            AtomIndex2 = b.atomIndex2
                        }).ToList();
                    }

                    for (var i = 0; i < asc.atomSetAuxiliaryInfo.Length; i++)
                    {
                        var info = asc.atomSetAuxiliaryInfo[i];
                        if (info != null)
                        {
                            var keys = info.keySet().toArray();

                            for (var i2 = 0; i2 < keys.Length; i2++)
                            {
                                var key = keys[i2];
                                var val = info.get(key);

                                if (val == null)
                                    continue;

                                if (key.Equals("chemicalName"))
                                {
                                    crystal.ChemicalName = val.ToString();
                                }
                                else if (key.Equals("formula"))
                                {
                                    crystal.ChemicalFormula = val.ToString();
                                }
                                else if (key.Equals("unitCellParams"))
                                {
                                    var array = val as Array;

                                    if(array.Length >= 6)
                                    {
                                        crystal.LengthA = Convert.ToDouble(array.GetValue(0));
                                        crystal.LengthB = Convert.ToDouble(array.GetValue(1));
                                        crystal.LengthC = Convert.ToDouble(array.GetValue(2));

                                        crystal.Alpha = Convert.ToDouble(array.GetValue(3));
                                        crystal.Beta = Convert.ToDouble(array.GetValue(4));
                                        crystal.Gamma = Convert.ToDouble(array.GetValue(5));
                                    }
                                }
                                else if (val is java.lang.Integer || val is java.lang.Boolean || val is System.String) {
                                    if (string.IsNullOrEmpty(val.ToString()) || val.ToString().Equals("null") || val.ToString().Equals("?"))
                                        continue;

                                    crystal.AuxInfo[key.ToString()] = val.ToString();
                                }
                            }
                        }
                    }
                }
            }

            return crystal;
        }
    }
}
