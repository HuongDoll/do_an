using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.Utils
{
    public class Utility
    {
        /// <summary>
        /// Chuyển tất cả giá trị của Enum về mảng
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Mảng chứa các giá trị của đối tượng enum</returns>
        public static List<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // You can't use type constraints on value types, so have to check & throw error.
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum type");

            Array enumValArray = Enum.GetValues(enumType);

            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }

        /// <summary>
        /// Lấy tên đối tượng
        /// </summary>
        /// <typeparam name="T">Đối tượng muốn lấy</typeparam>
        /// <returns>Tên đối tượng</returns>
        public static string GetEntityName<T>()
        {
            var entity = Activator.CreateInstance<T>();
            return entity.GetType().Name;
        }

        /// <summary>
        /// Chuyển kiểu dữ liệu
        /// </summary>
        /// <param name="value">giá trị cần chuyển</param>
        /// <param name="sqlDbType">Kiểu dữ liệu muốn chuyển</param>
        /// <returns>Giá trị chuyển dữ liệu thành công</returns>
        public static object ConvertType(string value, MySqlDbType sqlDbType)
        {
            object result;
            Type type = GetClrType(sqlDbType);

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            try
            {
                // trả về kiểu dữ liệu khi đã chuyển
                var converter = TypeDescriptor.GetConverter(type);
                result = converter.ConvertFromString(value);

            }
            catch (Exception exception)
            {

                // Log this exception if required.
                throw new InvalidCastException(string.Format("Unable to cast the {0} to type {1}", value, "newType", exception));
            }
            return result;
        }

        /// <summary>
        /// Kiểu dữ liệu cần chuyển
        /// </summary>
        /// <param name="sqlType"> Kiểu dữ liệu </param>
        /// <returns>Kiểu dữ liệu chuyển thành công</returns>
        public static Type GetClrType(MySqlDbType sqlType)
        {
            switch (sqlType)
            {
                case MySqlDbType.UInt16:
                case MySqlDbType.UInt24:
                case MySqlDbType.UInt32:
                case MySqlDbType.UInt64:
                    return typeof(long?);

                case MySqlDbType.Binary:
                //case MySqlDbType.Image:
                case MySqlDbType.Timestamp:
                case MySqlDbType.VarBinary:
                    return typeof(byte[]);

                case MySqlDbType.Bit:
                    return typeof(bool?);

                case MySqlDbType.VarChar:
                case MySqlDbType.LongText:
                case MySqlDbType.String:
                case MySqlDbType.Text:
                    return typeof(string);

                case MySqlDbType.DateTime:
                case MySqlDbType.Date:
                case MySqlDbType.Time:
                    return typeof(DateTime?);

                case MySqlDbType.Decimal:
                    return typeof(decimal?);

                case MySqlDbType.Float:
                    return typeof(double?);

                case MySqlDbType.Int32:
                case MySqlDbType.Int64:
                    return typeof(int?);

                case MySqlDbType.Int16:
                    return typeof(short?);

                default:
                    throw new ArgumentOutOfRangeException("sqlType");
            }
        }

        /// <summary>
        /// Chuyển kiểu dữ liệu từ string sang long
        /// </summary>
        /// <param name="input">Chuỗi string cần chuyển</param>
        /// <returns>Kiểu dữ liệu được chuyển</returns>
        public static long ConvertStringToLong(string input)
        {
            long result = -1;
            try
            {
                result = long.Parse(input);
            }
            catch (Exception)
            {
                return result;
            }
            return result;
        }

        /// <summary>
        /// Chuyển string sang date time
        /// </summary>
        /// <param name="input">string cần chuyển</param>
        /// <returns>Date time chuyển thành công</returns>
        public static DateTime ConvertStringToDateTime(string input)
        {
            DateTime result = new DateTime();
            try
            {
                //result = DateTime.Parse(input);
                result = DateTime.ParseExact(input + " 00:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return result;
            }
            return result;
        }

        /// <summary>
        /// Kiểm tra dữ liệu Email hợp lệ
        /// </summary>
        /// <param name="email">email cần kiểm tra</param>
        /// <returns>True: Hợp lệ</returns>
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email ?? "");
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra chuổi string có phải là dạng Guid hay không
        /// </summary>
        /// <param name="inputString">string cần kiểm tra có phải là guid hay không</param>
        /// <returns>true: Có; fale: không</returns>
        public static bool IsGuidFormat(String inputString)
        {
            Guid guidOutput;
            return Guid.TryParse(inputString, out guidOutput);
        }

        /// <summary>
        /// Hàm EscapeString loại bỏ các chuỗi SQLInjection 
        /// </summary>
        /// <param name="value">Chuỗi đầu vào</param>
        /// <returns>Chuỗi đã được loại bỏ hoặc null</returns>
        public static string EscapeString(string value)
        {
            return MySqlHelper.EscapeString(value) ?? null;
        }


        /// <summary>
        /// Sao chép giá trị đối tượng, nếu đối tượng cần sao chép không có trường đó thì bỏ qua
        /// </summary>
        /// <typeparam name="T">object</typeparam>
        /// <param name="sourceObject">Đầu vào bản cần sao chép</param>
        /// <param name="destObject">Đối tượng cần lấy sau khi sao chép</param>
        public static void CopyObject<T>(object sourceObject, ref T destObject)
        {
            if (sourceObject == null || destObject == null)
            {
                return;
            }

            Type sourceType = sourceObject.GetType();
            Type targetType = destObject.GetType();

            foreach (PropertyInfo p in sourceType.GetProperties())
            {
                //  Get the matching property in the destination object
                PropertyInfo targetObj = targetType.GetProperty(p.Name);
                //  If there is none, skip
                if (targetObj == null)
                    continue;
                //set value
                targetObj.SetValue(destObject, p.GetValue(sourceObject, null), null);
            }
        }

        /// <summary>
        /// Hàm parse string sang json tránh làm ngắt chương trình
        /// </summary>
        /// <param name="inputString">string cần chuyển qua JSON</param>
        /// <returns>Đối tượng được parse từ string dạng JSON</returns>
        public static JObject ParseJSONString(string inputString)
        {
            try
            {
                return JObject.Parse(inputString);
            }
            catch (Exception)
            {
                try
                {
                    return JObject.Parse(inputString.Replace("\\", ""));
                }
                catch (Exception)
                {
                    try
                    {
                        return JObject.Parse(inputString.Replace("\\", "").Replace("\n", ""));
                    }
                    catch (Exception)
                    {
                        return JObject.Parse("{}");
                    }
                }
            }
        }
    }
}
