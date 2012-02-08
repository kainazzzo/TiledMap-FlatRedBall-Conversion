using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TestBed
{
    [StructLayout(LayoutKind.Explicit)]
    public struct IncrementableFloat
    {
        [FieldOffset(0)]
        int mAsInt;
        [FieldOffset(0)]
        float mAsFloat;

        public float Value
        {
            get
            {
                return mAsFloat;
            }
            set
            {
                mAsFloat = value;
            }
        }

        public float IncreaseByMinimum()
        {
            mAsInt++;
            return mAsFloat;
        }

        public float DecreaseByMinimum()
        {
            mAsInt--;
            return mAsFloat;
        }
    }   
}
