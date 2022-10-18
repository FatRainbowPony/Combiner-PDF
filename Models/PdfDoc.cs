using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using PdfSharp;
using PdfSharp.Pdf;

namespace Combiner_PDF.Models
{
    public class PdfDoc
    {
        #region Properties

        #region Private

        #endregion

        #region Public
        public string PathToPdfDocument { get; set; }
        public ImageSource IconDoc { get; set; }
        #endregion

        #endregion
    }
}
