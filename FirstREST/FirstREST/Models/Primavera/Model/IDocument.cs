using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models.Primavera.Model
{
    public interface IDocument
    {
        DateTime DocumentDate { get; set; }
    }
}