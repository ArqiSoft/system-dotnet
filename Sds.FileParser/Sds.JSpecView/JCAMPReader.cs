using jspecview.source;
using System.Collections.Generic;

namespace Sds.JSpecView
{
	public class JcampReader
	{
		public bool Validate(string jcamp)
		{
			return false;
		}

		public static JSVSpectrum Read(string jcamp)
		{
			//	clear cache first...
			jspecview.common.JSVFileManager.htCorrelationCache.clear();

			java.io.InputStream stream = new java.io.StringBufferInputStream(jcamp);

			JDXSource source = JDXReader.createJDXSourceFromStream(stream, false, false, 0);

            var numOfSpec = source.getNumberOfSpectra();

            if (source.getNumberOfSpectra() == 0)
                throw new JcampHasNoSpectrumException();

            if (source.getNumberOfSpectra() > 1)
                throw new JcampHasTooManySpectraException("Only the case when JCAMP file has one spectrum currently supported");

            var spec = source.getJDXSpectrum(0);

            var mol = jspecview.common.JSVFileManager.htCorrelationCache.get("mol");

            return new JSVSpectrum
            {
                DataType = spec.getTypeLabel(),
                DataClass = spec.getDataClass(),
                Date = spec.getDate(),
                Dx = spec.getJcampdx(),
                Jcamp = jcamp,
                Mol = mol != null ? mol.ToString() : null,
                Origin = spec.getOrigin(),
                Owner = spec.getOwner(),
                Time = spec.getTime(),
                Title = spec.getTitle(),
            };
        }
    }
}
