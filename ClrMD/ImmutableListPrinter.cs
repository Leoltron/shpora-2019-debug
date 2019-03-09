using System;
using Microsoft.Diagnostics.Runtime;

namespace ClrMD
{
    public class ImmutableListPrinter : IClrObjectPrinter
    {
        public bool Supports(ClrType type)
        {
            return type.Name == "System.Collections.Immutable.ImmutableList<System.String>";
        }

        public void Print(ClrObject clrObject)
        {
            PrintImmutableListNode(clrObject.GetObjectField("_root"));
        }

        private static void PrintImmutableListNode(ClrObject clrObject)
        {
            if (clrObject.IsNull)
                return;

            PrintImmutableListNode(clrObject.GetObjectField("_left"));
            var objectField = clrObject.GetObjectField("_key");
            if (!objectField.IsNull)
                Console.WriteLine(ClrMdHelper.ToString(objectField));
            PrintImmutableListNode(clrObject.GetObjectField("_right"));
        }
    }
}
