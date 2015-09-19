using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.MiniPortal.Model.Exceptions
{
    /// <summary>
    /// Public exception for required string Fields which captures the error with epmty string
    /// </summary>
    public class EmptyStringException : Exception
    {
        public EmptyStringException() : base()
        {
        }
    }
}
