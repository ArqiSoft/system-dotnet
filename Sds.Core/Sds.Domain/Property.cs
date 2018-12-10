using System;
using System.Collections.Generic;

namespace Sds.Domain
{
    public class Property : ValueObject<Property>
    {
        public string Name { get; private set; }
        public object Value { get; private set; }
        public double? Error { get; private set; }

        public Property(string name, object value, double? error = null)
        {
            Name = name;
            Value = value;
            Error = error;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<Object>() { Name, Value, Error };
        }
    }

    public class Condition : Property
    {
        public Condition(string name, object value) : base(name, value)
        {
        }
    }

    public class Unit
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }

    //public class Software
    //{
    //    public string Name { get; set; }
    //    public string Description { get; set; }
    //    public string Version { get; set; }
    //    public string Uri { get; set; }
    //    public int? ManufacturerId { get; set; }
    //}

    //public class Provenance
    //{
    //    public string AuxInfo { get; set; }
    //    public Guid? EquipmentId { get; set; }
    //    public Guid? ReferenceId { get; set; }
    //    public Guid? SoftwareId { get; set; }
    //}

    public static class PropertyName
    {
        public const string ORIGINAL_SMILES = "SMILES";
        public const string STANDARDIZED_SMILES = "StdSMILES";
        public const string NON_STD_INCHI = "NonStdInChI";
        public const string NON_STD_INCHI_KEY = "NonStdInChIKey";
        public const string STD_INCHI = "StdInChI";
        public const string STD_INCHI_KEY = "StdInChIKey";
        public const string MOLECULAR_FORMULA = "MF";
        public const string MOLECULAR_WEIGHT = "MW";
        public const string MONOISOTOPIC_MASS = "MMass";
        public const string AVERAGE_MASS = "AMass";
        public const string NOMINAL_MASS = "NMass";
        public const string MOST_ABUNDANT_MASS = "MAMass";
        public const string CSID = "CSID";
        public const string INCHI_KEY = "InChIKey";
        public const string INCHI = "InChI";

        public const string MELTING_POINT = "MP";
        public const string BOILING_POINT = "BP";
        public const string OPTICAL_ROTATION = "OpRot";
        public const string IONIZATION_POTENTIAL = "IonPot";
        public const string VAPOUR_PRESSURE = "VapPress";
        public const string SOLUBILITY_IN_ORG_SOLVENT = "SolOrgSol";
        public const string LAMBDA_MAX = "LambdaMax";
        public const string EXPLOSURE_LIMITS_NIOSH_REL = "ExpLimNiosh";
        public const string LOG_P = "LogP";
        public const string APPEARENCE = "Appearance";
        public const string PRICE = "Price";
        public const string FLASH_POINT = "FP";
        public const string FREEZING_POINT = "FreezP";
        public const string DENSITY = "Density";
        public const string STABILITY = "Stability";
        public const string TOXICITY = "Tox";
        public const string SAFETY = "Safety";
        public const string FIRST_AID = "FirstAid";
        public const string EXPLOSURE_ROUTES = "ExpRoutes";
        public const string SYMPTOMS = "Symptoms";
        public const string TARGET_ORGANS = "TargetOrg";
        public const string INCOMPATIBILITY = "Incomp";
        public const string PERSONAL_PROTECTION = "PersProtect";
        public const string EXPLOSURE_LIMITS = "ExpLimits";
        public const string REFRACTION_INDEX = "RI";
        public const string SOLUBILITY_DESCRIPTION = "SolDesc";
        public const string SOLUBILITY_IN_WATER = "SolWater";
        public const string CHEMICAL_CLASS = "ChemClass";
        public const string THERAPEUTICAL_EFFECT = "TherapEffect";
        public const string DRUG_STATUS = "DrugStat";
        public const string COMPOUND_ORIGIN = "CompOrigin";
        public const string BIO_ACTIVITY = "BioActive";

        public const string COMMENT = "Comment";
        public const string PRESSURE = "Pressure";
        public const string THICKNESS = "Thickness";
        public const string ANNOTATIONS = "Annotations";
        public const string PH = "PH";

        public const string ENTHALPY_OF_VAPORIZATION = "HVap";
        public const string MOLAR_REFRACTIVITY = "MolRefract";
        public const string H_BOND_ACCEPTORS = "HBondAccept";
        public const string H_BOND_DONORS = "HBondDonor";
        public const string FREELY_ROTATING_BONDS = "FRB";
        public const string RULE_OF_5_VIOLATIONS = "Rule5Viol";
        public const string LOG_D = "LogD";
        public const string BCF = "BCF";
        public const string KOC = "KOC";
        public const string POLAR_SURFACE_AREA = "PSA";
        public const string POLARIZABILITY = "Polar";
        public const string SURFACE_TENSION = "Gamma";
        public const string MOLAR_VOLUME = "MolVol";
        public const string MOLARITY = "Molarity";
        public const string CONC = "Concentration";
        public const string WEIGHTPERCENTAGE = "WtPercent";
        public const string MOLARMASS = "MolarMass";
        public const string PURITY = "Purity";
        public const string NUMMOLES = "NumMoles";
        public const string VOL = "Vol";
        public const string MASS = "Mass";
        public const string DURATION = "Duration";
        public const string TEMP = "Temperature";
        public const string YIELD = "Yield";
        public const string EQUIVALENCE = "Equivalence";

        public const string REACTIONCLASS = "Reaction Class";
        public const string RXNOTYPE = "Named Reaction";
        public const string OTHERNAMEDREACTION = "Other Named Reaction";
        public const string MOPTYPE = "Molecular Process";
        public const string FLOWRATE = "Flow Rate";
        public const string POTENTIAL = "Potential";

        public const string EQUIPVOL = "EquipmentVol";
        public const string EQUIPINSTRTYPE = "EquipInstrType";
    }
}
