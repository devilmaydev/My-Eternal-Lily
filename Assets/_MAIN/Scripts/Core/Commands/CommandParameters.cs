using System.Collections.Generic;
using System.Globalization;

namespace _MAIN.Scripts.Core.Commands
{
    public class CommandParameters
    {
        private const char ParameterIdentifier = '-';

        private Dictionary<string, string> _parameters = new();
        private List<string> _unlabledParameters = new();

        public CommandParameters(string[] parameterArray, int startingIndex = 0) 
        {
            for (int i = startingIndex; i < parameterArray.Length; i++)
            {
                if (parameterArray[i].StartsWith(ParameterIdentifier) && !float.TryParse(parameterArray[i], out _))
                {
                    var pName = parameterArray[i];
                    var pValue = "";

                    if (i + 1 < parameterArray.Length && !parameterArray[i + 1].StartsWith(ParameterIdentifier))
                    {
                        pValue = parameterArray[i + 1];
                        i++;
                    }

                    _parameters.Add(pName, pValue);
                }
                else
                    _unlabledParameters.Add(parameterArray[i]);
            }
        }

        public bool TryGetValue<T>(string parameterName, out T value, T defaultValue = default(T)) => TryGetValue(new string[] { parameterName }, out value, defaultValue);

        public bool TryGetValue<T>(string[] parameterNames, out T value, T defaultValue = default(T))
        {
            foreach (string parameterName in parameterNames)
            {
                if (_parameters.TryGetValue(parameterName, out string parameterValue))
                {
                    if (TryCastParameter(parameterValue, out value))
                    {
                        return true;
                    }
                }
            }

            //if we reach here, no match was found in the identified parameters so search the unlabeled ones if present
            foreach (string parameterName in _unlabledParameters)
            {
                if (TryCastParameter(parameterName, out value))
                {
                    _unlabledParameters.Remove(parameterName);
                    return true;
                }
            }

            value = defaultValue;
            return false;
        }

        private bool TryCastParameter<T>(string parameterValue, out T value)
        {
            if (typeof(T) == typeof(bool))
            {
                if (bool.TryParse(parameterValue, out bool boolValue))
                {
                    value = (T)(object)boolValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (int.TryParse(parameterValue, out int intValue))
                {
                    value = (T)(object)intValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(float))
            {
                if (float.TryParse(parameterValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatValue))
                {
                    value = (T)(object)floatValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                value = (T)(object)parameterValue;
                return true;
            }

            value = default(T);
            return false;
        }
        
    }
}