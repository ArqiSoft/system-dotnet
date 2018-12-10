list of custom changes that were made to get Jmol Cif reader working properly
1. comment out the next line of code 
      //asc.setCurrentModelInfo("unitCellParams", null);
   CifReader.java: private void setBondingAndMolecules()
2. float bondTolerance = vwr != null ? vwr.getFloat(T.bondtolerance) : 0;
   CifREader.java: private boolean createBonds(boolean doInit)
