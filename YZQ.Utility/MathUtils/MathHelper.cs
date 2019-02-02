using System;

namespace YZQ.Utility
{
    public class MathHelper
    {
        /// <summary>
        /// 小数点抹小数点右边
        /// </summary>
        /// <param name="val"></param>
        /// <param name="length">剩几位</param>
        /// <returns></returns>
        public static decimal PointRight(dynamic val, int length = 2)
        {
            decimal result = 0M;

            decimal pow = (decimal)System.Math.Pow(10, length),
                _pow = (decimal)System.Math.Pow(10, (length * -1));

            try
            {
                decimal _val = Convert.ToDecimal(val);
                result = ((int)(_val * pow)) * _pow;
            }
            catch (System.Exception ex)
            {
                LogHelper.Error(ex);
            }

            return result;
        }
    }
}
