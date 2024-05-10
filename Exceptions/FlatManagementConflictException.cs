using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGMietkosten.Models;

namespace WGMietkosten.Exceptions
{
    class FlatManagementConflictException : Exception
    {
        public FlatSetup ExistingFlatSetup { get; }
        public FlatSetup IncomingFlatSetup { get; }

        public FlatManagementConflictException(FlatSetup existingFlatSetup, FlatSetup incomingFlatSetup)
        {
            ExistingFlatSetup = existingFlatSetup;
            IncomingFlatSetup = incomingFlatSetup;
        }

        public FlatManagementConflictException(string message, FlatSetup existingFlatSetup, FlatSetup incomingFlatSetup) : base(message)
        {
            ExistingFlatSetup = existingFlatSetup;
            IncomingFlatSetup = incomingFlatSetup;
        }

        public FlatManagementConflictException(string message, Exception innerException, FlatSetup existingFlatSetup, FlatSetup incomingFlatSetup) : base(message, innerException)
        {
            ExistingFlatSetup = existingFlatSetup;
            IncomingFlatSetup = incomingFlatSetup;
        }
    }
}
