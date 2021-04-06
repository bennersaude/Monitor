using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Monitor.Data.Types;

namespace Monitor.Data.Infra
{
    public static class DomainLoader
    {
        public static void ForceAssemblyLoading(string namePattern, TipoAplicacao? tipoAplicacao = null)
        {
            string binPath = (tipoAplicacao == TipoAplicacao.StandAlone)
                    ? System.AppDomain.CurrentDomain.BaseDirectory
                    : System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");

            //// Forçar o carregamento de todas as DLLs para o AppDomain
            foreach (string dll in Directory.GetFiles(binPath, namePattern + "*.dll", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    if (!AppDomain.CurrentDomain.GetAssemblies().Any(x => x.GetName().Name.ToLower() == Path.GetFileNameWithoutExtension(dll).ToLower()))
                    {
                        var name = AssemblyName.GetAssemblyName(dll);
                        Assembly.Load(name);
                    }
                }
                catch (FileLoadException)
                { } // The Assembly has already been loaded.
                catch (BadImageFormatException)
                { } // If a BadImageFormatException exception is thrown, the file is not an assembly.
            }
        }
    }
}
