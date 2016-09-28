using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MetrologyAdmin
{
    public class SettingsManager 
    {

        public int SelectedLoginServerId
        {
            get
            {
                var result = Properties.Settings.Default[Strings.SelectedLoginServerId];
                if (!(result is int)) return 0;
                return (int)result;
            }
            set
            {
                if (value > 0)
                {
                    Properties.Settings.Default[Strings.SelectedLoginServerId] = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public int SelectedServerId
        {
            get
            {
                var result = Properties.Settings.Default[Strings.SelectedServerId];
                if (!(result is int)) return 0;
                return (int)result;
            }
            set
            {
                if (value > 0)
                {
                    Properties.Settings.Default[Strings.SelectedServerId] = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public bool SendMessageWhenLoginDetailsChanged
        {
            get
            {
                var result = Properties.Settings.Default[Strings.SendMessageWhenLoginDetailsChanged];
                if (!(result is bool)) return false;
                return (bool)result;
            }
            set
            {
                Properties.Settings.Default[Strings.SendMessageWhenLoginDetailsChanged] = value;
                Properties.Settings.Default.Save();
            }
        }

        /*
        public int GetSelectedOrganizationId(int serverId)
        {
            return SelectedOrganizationByServerId.GetValueOrDefault(serverId);
        }

        public void SetSelectedOrganizationId(int serverId, int organizationId)
        {
            var dict = SelectedOrganizationByServerId;
            if (dict.ContainsKey(serverId))
                dict.Remove(serverId);
            dict.Add(serverId,organizationId);
            SelectedOrganizationByServerId = dict;
        }

        private IDictionary<int,int> SelectedOrganizationByServerId
        {
            get
            {
                return DictionaryGet();
            }
            set
            {
                DictionarySet(value);
            }
        }

        private IDictionary<int,int> DictionaryGet()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SettingsItem[]), new XmlRootAttribute() { ElementName = "items" });
            var newDictionary = new Dictionary<int, int>();

            try
            {
                var settingString = Properties.Settings.Default[Strings.SelectedOrganizationIds];
                if (!(settingString is string)) return newDictionary;
                if (String.IsNullOrWhiteSpace((string)settingString)) return newDictionary;

                return ((SettingsItem[])serializer.Deserialize(GenerateStreamFromString((string)settingString))).ToDictionary(i => i.id, i => i.value);
            }
            catch
            {
                return newDictionary;
            }
        }

        private void DictionarySet(IDictionary<int,int> value)
        {
            var serializer = new XmlSerializer(typeof(SettingsItem[]), new XmlRootAttribute() { ElementName = "items" });
            var memoryStream = new MemoryStream();

            try
            {
                serializer.Serialize(memoryStream, value.Select(kv => new SettingsItem() { id = kv.Key, value = kv.Value }).ToArray());

                var resultString = GenerateStringFromStream(memoryStream);

                if (!String.IsNullOrWhiteSpace(resultString))
                {
                    Properties.Settings.Default[Strings.SelectedOrganizationIds] = resultString;
                    Properties.Settings.Default.Save();
                }
            }
            catch
            {

            }
        }
         * */

        private class Strings
        {
            public static string SelectedLoginServerId { get { return "SelectedLoginServerId"; } }
            public static string SelectedServerId { get { return "SelectedServerId"; } }
            public static string SelectedOrganizationIds { get { return "SelectedOrganizationIds"; } }
            public static string SendMessageWhenLoginDetailsChanged { get { return "SendMessageWhenLoginDetailsChanged"; } }
        }

        public class SettingsItem
        {
            [XmlAttribute]
            public int id;

            [XmlAttribute]
            public int value;
        }

        private MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

        private string GenerateStringFromStream(MemoryStream value)
        {
            return Encoding.UTF8.GetString(value.ToArray());
        }
         
    }


    public static class DictionaryHelper
    {
        public static TValue GetValueOrDefault<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : default(TValue);
        }

    }
}
