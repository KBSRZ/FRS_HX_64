using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAngine.DBUtility
{
    class TypeConverter
    {
        public static double[] ConvertByteArrayToDoubleArray(byte[] value)
        {
            int size = sizeof(double);
            if (value == null)

                throw new ArgumentNullException("value");

            // Create an array that holds the results.

            double[] result = new double[value.Length / size];

            for (int i = 0; i < value.Length; i = i + size)
            {

                byte[] b = new byte[size];

                // Copy the values (for the next double) of the byte array

                // to the temp array.

                Array.Copy(value, i, b, 0, b.Length);

                double d = BitConverter.ToDouble(b, 0);

                // Set the result in the result array.

                result[i / size] = d;

            }

            return result;

        }

        public static byte[] CreateByteArrayFromDoubleArray(double[] value)
        {
            int size = sizeof(double);
            if (value == null)

                throw new ArgumentNullException("value");

            // Create an array that holds the result.

            byte[] result = new byte[value.Length * size];

            for (int i = 0; i < value.Length; i++)
            {

                byte[] b = BitConverter.GetBytes(value[i]);

                // Copy the byte array to the result.

                Array.Copy(b, 0, result, (i * size), b.Length);

            }

            return result;

        }
        public static float[] ConvertByteArrayToFloatArray(byte[] value)
        {
            int size = sizeof(double);
            if (value == null)

                throw new ArgumentNullException("value");

            // Create an array that holds the results.

            float[] result = new float[value.Length / size];

            for (int i = 0; i < value.Length; i = i + size)
            {

                byte[] b = new byte[size];

                // Copy the values (for the next double) of the byte array

                // to the temp array.

                Array.Copy(value, i, b, 0, b.Length);

                float d = BitConverter.ToSingle (b, 0);

                // Set the result in the result array.

                result[i / size] = d;

            }

            return result;

        }

        public static byte[] CreateByteArrayFromFloatArray(float[] value)
        {
            int size = sizeof(double);
            if (value == null)

                throw new ArgumentNullException("value");

            // Create an array that holds the result.

            byte[] result = new byte[value.Length * size];

            for (int i = 0; i < value.Length; i++)
            {

                byte[] b = BitConverter.GetBytes(value[i]);

                // Copy the byte array to the result.

                Array.Copy(b, 0, result, (i * size), b.Length);

            }

            return result;

        }
    }
}
