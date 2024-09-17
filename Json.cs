using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 暗夜辅助服务端
{
    public class Json
    {
        public static void writejson(string shuxing, string zhi)
        {
            
            if (File.Exists(MainForm.filePath))
            {
                string jsonf = File.ReadAllText(MainForm.filePath);
                JObject root = JObject.Parse(jsonf);
                root[shuxing] = zhi;
                jsonf = root.ToString();
                File.WriteAllText(MainForm.filePath, jsonf);
            }
            else
            {
                using (FileStream fs = File.Create(MainForm.filePath)) { }
                JObject root = new JObject();
                root[shuxing] = zhi;
                string jsonf = root.ToString();
                File.WriteAllText(MainForm.filePath, jsonf);
            }
        }
        public static void deljsonkm(string kami)
        {
            if (File.Exists(MainForm.km))
            {
                string jsonf = File.ReadAllText(MainForm.km);
                JObject root = JObject.Parse(jsonf);
                if (root.ContainsKey(kami))
                {
                    root.Remove(kami);
                    jsonf = root.ToString();
                    File.WriteAllText(MainForm.km, jsonf);
                }
                else
                {
                    
                }
            }
            else
            {
                
            }
            return;
        }
        public static string readjson(string shuxing)
        {
            if (File.Exists(MainForm.filePath))
            {
                string jsonf = File.ReadAllText(MainForm.filePath);
                JObject root = JObject.Parse(jsonf);
                if (root.ContainsKey(shuxing))
                {
                    return root[shuxing].ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        public static bool checkjson(string shuxing)
        {
            if (File.Exists(MainForm.filePath))
            {
                string jsonf = File.ReadAllText(MainForm.filePath);
                if (jsonf.Length == 0)
                {
                    JObject root2 = new JObject();
                    string jsonf2 = root2.ToString();
                    File.WriteAllText(MainForm.filePath, jsonf2);
                }
                jsonf = File.ReadAllText(MainForm.filePath);
                JObject root = JObject.Parse(jsonf);
                return root.ContainsKey(shuxing);
            }
            else
            {
                return false;
            }
        }
        public static void writejson2(string shuxing1, string shuxing2, string zhi)
        {
            if (File.Exists(MainForm.filePath))
            {
                string jsonf = File.ReadAllText(MainForm.filePath);
                JObject root = JObject.Parse(jsonf);
                if (checkjson2(shuxing1,shuxing2))
                {
                    
                    root[shuxing1][shuxing2] = zhi;
                }
                else
                {
                    if (checkjson(shuxing1))
                    {
                        if (root[shuxing1] != null)
                        {
                            JObject a2 = JObject.Parse(root[shuxing1].ToString());
                            a2[shuxing2] = zhi;
                            root[shuxing1] = a2;
                        }
                        else
                        {
                            JObject a1 = new JObject();
                            a1[shuxing2] = zhi;
                            root[shuxing1] = a1;
                        }
                    }
                    else
                    {
                        
                        JObject a1 = new JObject();
                        a1[shuxing2] = zhi;
                        root[shuxing1] = a1;
                    }
                }
                
                
                jsonf = root.ToString();
                File.WriteAllText(MainForm.filePath, jsonf);
            }
            else
            {
                using (FileStream fs = File.Create(MainForm.filePath)) { }
                JObject root = new JObject();
                if (checkjson2(shuxing1, shuxing2))
                {
                    Debug.WriteLine("都没有");
                    root[shuxing1][shuxing2] = zhi;
                }
                else
                {
                    if (checkjson(shuxing1))
                    {
                        Debug.WriteLine("有属性1");
                        if (root[shuxing1] != null)
                        {
                            JObject a2 = JObject.Parse(root[shuxing1].ToString());
                            a2[shuxing2] = zhi;
                            root[shuxing1] = a2;
                        }
                        else
                        {
                            JObject a1 = new JObject();
                            a1[shuxing2] = zhi;
                            root[shuxing1] = a1;
                        }
                    }
                    else
                    {
                        Debug.WriteLine("没有属性1");
                        JObject a1 = new JObject();
                        a1[shuxing2] = zhi;
                        root[shuxing1] = a1;
                    }
                }
                string jsonf = root.ToString();
                File.WriteAllText(MainForm.filePath, jsonf);
            }
        }
        public static string readjson2(string shuxing1, string shuxing2)
        {
            if (File.Exists(MainForm.filePath))
            {
                string jsonf = File.ReadAllText(MainForm.filePath);
                JObject root = JObject.Parse(jsonf);
                if (root.ContainsKey(shuxing1))
                {
                    JObject root2 = JObject.Parse(root[shuxing1].ToString());

                    if (root2.ContainsKey(shuxing2))
                    {
                        return root2[shuxing2].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        public static bool checkjson2(string shuxing1, string shuxing2)
        {
            if (File.Exists(MainForm.filePath))
            {
                string jsonf = File.ReadAllText(MainForm.filePath);
                JObject root = JObject.Parse(jsonf);
                if (root.ContainsKey(shuxing1))
                {
                    JObject root2 = JObject.Parse(root[shuxing1].ToString());
                    if (root2.ContainsKey(shuxing2))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static void writejsonhmd(string shuxing, string zhi)
        {
            if (File.Exists(MainForm.heimingdan))
            {
                string jsonf = File.ReadAllText(MainForm.heimingdan);
                JObject root = JObject.Parse(jsonf);
                root[shuxing] = zhi;
                jsonf = root.ToString();
                File.WriteAllText(MainForm.heimingdan, jsonf);
            }
            else
            {
                using (FileStream fs = File.Create(MainForm.heimingdan)) { }
                JObject root = new JObject();
                root[shuxing] = zhi;
                string jsonf = root.ToString();
                File.WriteAllText(MainForm.heimingdan, jsonf);
            }
        }
        public static string readjsonhmd(string shuxing)
        {
            if (File.Exists(MainForm.heimingdan))
            {
                string jsonf = File.ReadAllText(MainForm.heimingdan);
                JObject root = JObject.Parse(jsonf);
                if (root.ContainsKey(shuxing))
                {
                    return root[shuxing].ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        public static bool checkjsonhmd(string shuxing)
        {
            if (File.Exists(MainForm.heimingdan))
            {
                string jsonf = File.ReadAllText(MainForm.heimingdan);
                JObject root = JObject.Parse(jsonf);
                return root.ContainsKey(shuxing);
            }
            else
            {
                return false;
            }
        }

        public static void writejsonkm(string shuxing, string zhi)
        {
            if (File.Exists(MainForm.km))
            {
                string jsonf = File.ReadAllText(MainForm.km);
                JObject root = JObject.Parse(jsonf);
                root[shuxing] = zhi;
                jsonf = root.ToString();
                File.WriteAllText(MainForm.km, jsonf);
            }
            else
            {
                using (FileStream fs = File.Create(MainForm.km)) { }
                JObject root = new JObject();
                root[shuxing] = zhi;
                string jsonf = root.ToString();
                File.WriteAllText(MainForm.km, jsonf);
            }
        }
        public static string readjsonkm(string shuxing)
        {
            if (File.Exists(MainForm.km))
            {
                string jsonf = File.ReadAllText(MainForm.km);
                JObject root = JObject.Parse(jsonf);
                if (root.ContainsKey(shuxing))
                {
                    return root[shuxing].ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        public static bool checkjsonkm(string shuxing)
        {
            if (File.Exists(MainForm.km))
            {
                string jsonf = File.ReadAllText(MainForm.km);
                JObject root = JObject.Parse(jsonf);
                return root.ContainsKey(shuxing);
            }
            else
            {
                return false;
            }
        }
    }
}
