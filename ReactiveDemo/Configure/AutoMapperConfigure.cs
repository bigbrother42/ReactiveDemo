using AutoMapper;
using ReactiveDemo.Base.ProfileBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Configure
{
    public class AutoMapperConfigure : IAppConfigure
    {
        public void Configure(LauncherContext context)
        {
            List<Type> startupMapperProfiles = new List<Type>();
            Assembly assembly = Assembly.GetExecutingAssembly();

            UriBuilder uri = new UriBuilder(assembly.CodeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string bin = Path.GetDirectoryName(path);
            string[] assemblies = Directory.GetFiles(bin, "*Demo.dll");
            assemblies = assemblies.Concat(Directory.GetFiles(bin, "*Demo.exe")).ToArray();

            foreach (string file in assemblies)
            {
                try
                {
                    if (File.Exists(file))
                    {
                        assembly = Assembly.LoadFrom(file);

                        var query = from t in assembly.GetTypes()
                                    where t.IsClass && t.IsSubclassOf(typeof(BaseProfile)) && !t.IsAbstract
                                    select t;

                        startupMapperProfiles.AddRange(query);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.ToString());
                }
            }

            if (startupMapperProfiles.Count > 0)
            {
                Mapper.Initialize(x =>
                {
                    foreach (var type in startupMapperProfiles)
                    {
                        x.AddProfile((BaseProfile)Activator.CreateInstance(type));
                    }
                });
            }
        }
    }
}
