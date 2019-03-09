using System;
using System.Collections.Generic;
using Microsoft.Diagnostics.Runtime;

namespace ClrMD
{
    public class ListPrinter : IClrObjectPrinter
    {
        public bool Supports(ClrType type)
        {
            return type.Name == "System.Collections.Generic.List<System.String>";
        }

        public void Print(ClrObject clrObject)
        {
            var size = clrObject.GetField<int>("_size");
            
            var items =  clrObject.GetObjectField("_items");
            var arrayType = items.Type;

            for (int i = 0; i < size; i++)
            {
                var entryAddr = arrayType.GetArrayElementAddress(items.Address, i);
                var heap = clrObject.Type.Heap;
                heap.ReadPointer(entryAddr, out var objectAddress);

                Console.WriteLine(ClrMdHelper.ToString(new ClrObject(
                    objectAddress,
                    heap.GetObjectType(objectAddress))));
            }
        }
    }
}