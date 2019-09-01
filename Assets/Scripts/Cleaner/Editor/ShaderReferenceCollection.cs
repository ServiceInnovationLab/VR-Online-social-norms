/**
*asset cleaner
*Copyright (c) 2015 Tatsuhiko Yamamura
*
*This software is released under the MIT License.
*http://opensource.org/licenses/mit-license.php
*/

//============================================
//
//Modified By Bathur Lu
//
//Date:     2019.3.23
//Website:  http://bathur.cn/
//
//============================================

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;

namespace AssetClean
{
    public class ShaderReferenceCollection
    {
        // shader name / shader file guid
        public Dictionary<string, string> shaderFileList = new Dictionary<string, string>();
        public Dictionary<string, List<string>> shaderReferenceList = new Dictionary<string, List<string>>();

        public void Collection()
        {
            CollectionShaderFiles();
            CheckReference();
        }

        void CollectionShaderFiles()
        {
            var shaderFiles = Directory.GetFiles("Assets", "*.shader", SearchOption.AllDirectories);
            foreach (var shaderFilePath in shaderFiles)
            {
                var code = File.ReadAllText(shaderFilePath);
                var match = Regex.Match(code, "Shader \"(?<name>.*)\"");
                if (match.Success)
                {
                    var shaderName = match.Groups["name"].ToString();
                    //Shaders with the same name will not appear.
                    if (shaderFileList.ContainsKey(shaderName) == false)
                    {
                        shaderFileList.Add(shaderName, AssetDatabase.AssetPathToGUID(shaderFilePath));
                    }
                }
            }

            var cgFiles = Directory.GetFiles("Assets", "*.cg", SearchOption.AllDirectories);

            //Files with the same name will cause an error.
            /*
            foreach (var cgFilePath in cgFiles)
            {
                var file = Path.GetFileName(cgFilePath);
                shaderFileList.Add(file, cgFilePath);
            }
            */

            //Add an index to the file name to distinguish files with the same name.
            for (int i = 0; i < cgFiles.Length; i++)
            {
                //Asterisk(*) does not appear in the file name.
                //Use it to distinguish files with the same name.
                var file = Path.GetFileName(cgFiles[i]) + "*" + i;
                shaderFileList.Add(file, cgFiles[i]);
            }

            var cgincFiles = Directory.GetFiles("Assets", "*.cginc", SearchOption.AllDirectories);

            //Files with the same name will cause an error.
            /*
            foreach (var cgincPath in cgincFiles)
            {
                var file = Path.GetFileName(cgincPath);
                shaderFileList.Add(file, cgincPath);
            }
            */

            //Add an index to the file name to distinguish files with the same name.
            for (int i = 0; i < cgincFiles.Length; i++)
            {
                //Asterisk(*) does not appear in the file name.
                //Use it to distinguish files with the same name.
                var file = Path.GetFileName(cgincFiles[i]) + "*" + i;
                shaderFileList.Add(file, cgincFiles[i]);
            }
        }

        void CheckReference()
        {
            foreach (var shader in shaderFileList)
            {
                var shaderFilePath = AssetDatabase.GUIDToAssetPath(shader.Value);
                //Prevent empty paths.
                if (shaderFilePath == string.Empty)
                {
                    continue;
                }
                //Restore file name.
                var shaderName = shader.Key.Split('*')[0];

                List<string> referenceList = new List<string>();
                shaderReferenceList.Add(shaderName, referenceList);

                var code = File.ReadAllText(shaderFilePath);

                foreach (var checkingShaderName in shaderFileList.Keys)
                {
                    if (Regex.IsMatch(code, string.Format("{0}", checkingShaderName)))
                    {
                        var filePath = shaderFileList[checkingShaderName];
                        referenceList.Add(filePath);
                    }
                }
            }
        }
    }
}
