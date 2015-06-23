using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IllySystem.Auditor
{
    public static class Auditor
    {
        /*
         * Main function to build the list of differences
         */ 
        public static List<string> GetChanges(object firstObj, object secondObj)
        {
            List<string> variances = new List<string>();
            FindDifferences(firstObj, secondObj, variances);
            return variances;
        }

        /*
         * Get the difference between 2 objects and qdd it to the list
         */ 
        private static List<string> FindDifferences(object firstObj, object secondObj, List<string> variances)
        {
            // Null test
            if (firstObj != null && secondObj != null)
            {
                Type firstObjType = firstObj.GetType();

                // Loop on eqch public property of the first object
                foreach (PropertyInfo propInfo in firstObjType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead))
                {
                    // Get the properties
                    object firstObjValue = propInfo.GetValue(firstObj, null);
                    object secondObjValue = propInfo.GetValue(secondObj, null);

                    // If they are comparable we can directly test the values
                    if (ImplementComparable(propInfo.PropertyType))
                    {
                        if (!AreEquals(firstObjValue, secondObjValue))
                        {
#if (DEBUG || MYTEST)
                            System.Diagnostics.Debug.WriteLine("Property {0}: {1} != {2}", propInfo.Name, firstObjValue, secondObjValue);
#endif
                            string variance = FormatResult(propInfo.Name, firstObjValue.ToString(), secondObjValue.ToString()); 
                            variances.Add(variance);
                        }
                    }
                    // Else if they implement IEnumerable we can cast and test eqch values to get the differences
                    else if (ImplementEnumerable(propInfo.PropertyType))
                    {
                        IEnumerable<object> enumObj1, enumObj2;
                        int obj1Length, obj2Length;

                        // If one is null, the objects are different
                        if (firstObjValue == null && secondObjValue != null || firstObjValue != null && secondObjValue == null)
                        {
#if (DEBUG || MYTEST)
                            System.Diagnostics.Debug.WriteLine("Property {0}: {1} != {2}", propInfo.Name, firstObjValue, secondObjValue);
#endif
                            string variance = FormatResult(propInfo.Name, firstObjValue.ToString(), secondObjValue.ToString());
                            variances.Add(variance);
                        }
                        else if (firstObjValue != null && secondObjValue != null)
                        {
                            enumObj1 = ((IEnumerable)firstObjValue).Cast<object>();
                            enumObj2 = ((IEnumerable)secondObjValue).Cast<object>();
                            obj1Length = enumObj1.Count();
                            obj2Length = enumObj2.Count();

                            // If the 'lists' don't have the same number of values they are different
                            if (obj1Length != obj2Length)
                            {
#if (DEBUG || MYTEST)
                                System.Diagnostics.Debug.WriteLine("Property {0}: {1} != {2}", propInfo.Name, firstObjValue, secondObjValue);
#endif
                                string variance = FormatResult(propInfo.Name, firstObjValue.ToString(), secondObjValue.ToString());
                                variances.Add(variance);
                            }
                            else
                            {
                                // Test all values
                                for (int i = 0; i < obj1Length; i++)
                                {
                                    object obj1, obj2;
                                    Type objType;

                                    obj1 = enumObj1.ElementAt(i);
                                    obj2 = enumObj2.ElementAt(i);
                                    objType = obj1.GetType();

                                    if (ImplementComparable(objType))
                                    {
                                        if (!AreEquals(obj1, obj2))
                                        {
#if (DEBUG || MYTEST)
                                            System.Diagnostics.Debug.WriteLine("Property {0}: {1} != {2}", propInfo.Name, firstObjValue, secondObjValue);
#endif
                                            string variance = FormatResult(propInfo.Name, firstObjValue.ToString(), secondObjValue.ToString());
                                            variances.Add(variance);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // If the object itself is a class we use the recursion
                    else if (propInfo.PropertyType.IsClass)
                    {
                        FindDifferences(firstObjValue, secondObjValue, variances);
                    }
                    else
                    {
#if (DEBUG || MYTEST)
                        System.Diagnostics.Debug.WriteLine("Cannot compare this property {0}", propInfo.Name);
#endif
                    }
                }
                return variances;
            }
            else
            {
                return variances;
            }
        }

        private static bool ImplementComparable(Type type)
        {
            return typeof(IComparable).IsAssignableFrom(type) || type.IsPrimitive || type.IsValueType;
        }

        private static bool ImplementEnumerable(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        private static bool AreEquals(object firstObj, object secondObj)
        {
            IComparable valueComparer = firstObj as IComparable;

            if (firstObj == null && secondObj != null || firstObj != null && secondObj == null)
            {
                return false;
            }
            if (valueComparer != null && valueComparer.CompareTo(secondObj) != 0)
            {
                return false;
            }
            if (!firstObj.Equals(secondObj))
            {
                return false;
            }
            return true;
        }

        private static string FormatResult(string propName, object firstObj, object secondObj)
        {
            string propertyName = String.Empty;

            foreach (Char c in propName)
            {
                if (c.Equals('_'))
                {
                    propertyName += " ";
                }
                else if (Char.IsUpper(c))
                {
                    if (!string.IsNullOrEmpty(propertyName))
                    {
                        propertyName += " ";
                        propertyName += Char.ToUpper(c);
                    }
                    else
                    {
                        propertyName += Char.ToUpper(c);
                    }
                }
                else
                {
                    propertyName += c;
                }
            }

            string output = Properties.Settings.Default.StrOutput;
            output = output.Replace(Properties.Settings.Default.VarPropertyName, propertyName);
            output = output.Replace(Properties.Settings.Default.VarPropertyValue1, firstObj.ToString());
            output = output.Replace(Properties.Settings.Default.VarPropertyValue2, secondObj.ToString());
            return output.Trim();
        }

    }
}
