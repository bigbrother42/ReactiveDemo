using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Util
{
    public class AssemblyInfoUtil
    {
        /// <summary>
        ///   Loaded assembly title
        /// </summary>
        /// <value>
        ///  is the same as the value of <see cref="AssemblyTitleAttribute.Title"/>
        /// </value>
        public string Title { get; set; }

        /// <summary>
        ///   Loaded assembly description
        /// </summary>
        /// <value>
        /// is the same as the value of  <see cref="AssemblyDescriptionAttribute.Description"/>
        /// </value>
        public string Description { get; set; }

        /// <summary>
        ///   Company name of loaded assembly
        /// </summary>
        /// <value>
        /// is the same as the value of  <see cref="AssemblyCompanyAttribute.Company"/>
        /// </value>
        public string Company { get; set; }

        /// <summary>
        ///   Product name of loaded assembly
        /// </summary>
        /// <value>
        /// is the same as the value of  <see cref="AssemblyProductAttribute.Product"/>
        /// </value>
        public string Product { get; set; }

        /// <summary>
        ///   Loaded assembly copyright
        /// </summary>
        /// <value>
        ///  is the same as the value of <see cref="AssemblyCopyrightAttribute.Copyright"/>
        /// </value>
        public string Copyright { get; set; }

        /// <summary>
        ///   Loaded assembly trademark
        /// </summary>
        /// <value>
        /// is the same as the value of  <see cref="AssemblyTrademarkAttribute.Trademark"/>
        /// </value>
        public string Trademark { get; set; }

        /// <summary>
        ///   Assembly version of loaded assembly
        /// </summary>
        /// <value>
        /// is the same as the value of  <see cref="AssemblyName.Version"/> の <see cref="Version.ToString()"/>
        /// </value>
        public string AssemblyVersion { get; set; }

        /// <summary>
        ///   <para>Shortened assembly version of loaded assembly</para>
        /// </summary>
        /// <value>
        ///  a string containing only the values of <see cref="Version.Major"/> と <see cref="Version.Minor"/>
        /// </value>
        public string ShortAssemblyVersion { get; set; }

        /// <summary>
        ///   File version of loaded assembly
        /// </summary>
        /// <value>
        ///  is the same as the value of <see cref="AssemblyFileVersionAttribute.Version"/>
        /// </value>
        public string FileVersion { get; set; }

        /// <summary>
        ///   Globally unique identifier value for loaded assemblies
        /// </summary>
        /// <value>
        ///  is the same as the value of <see cref="GuidAttribute.Value"/>
        /// </value>
        public string Guid { get; set; }

        /// <summary>
        ///   Default language of loaded assemblies
        /// </summary>
        /// <value>
        ///  is the same as the value of <see cref="NeutralResourcesLanguageAttribute.CultureName"/> 
        /// </value>
        public string NeutralLanguage { get; set; }

        /// <summary>
        ///   How a COM client accesses managed code in a loaded assembly
        /// </summary>
        /// <value>
        ///  is the same as the value of <see cref="ComVisibleAttribute.Value"/>
        ///   <para>A value of <c>true</c> indicates that the managed type is visible to COM. </para>
        ///   <para>If the value is <c>false</c>, all public types in the assembly are hidden from COM. </para>
        /// </value>
        public bool IsComVisible { get; set; } = false;

        /// <summary>
        ///  <para>Create an instance of a class representing assembly information. </para>
        ///  <para>The assembly that is loaded is the return value of the <see cref="Assembly.GetExecutingAssembly"/> method. </para>
        /// </summary>
        public AssemblyInfoUtil() : this(Assembly.GetExecutingAssembly())
        {
        }

        public AssemblyInfoUtil(Assembly assembly)
        {
            AssemblyTitleAttribute titleAttr = GetAssemblyAttribute<AssemblyTitleAttribute>(assembly);
            if (titleAttr != null)
            {
                Title = titleAttr.Title;
            }

            AssemblyDescriptionAttribute assemblyAttr = GetAssemblyAttribute<AssemblyDescriptionAttribute>(assembly);
            if (assemblyAttr != null)
            {
                Description = assemblyAttr.Description;
            }

            AssemblyCompanyAttribute companyAttr = GetAssemblyAttribute<AssemblyCompanyAttribute>(assembly);
            if (companyAttr != null)
            {
                Company = companyAttr.Company;
            }

            AssemblyProductAttribute productAttr = GetAssemblyAttribute<AssemblyProductAttribute>(assembly);
            if (productAttr != null)
            {
                Product = productAttr.Product;
            }

            AssemblyCopyrightAttribute copyrightAttr = GetAssemblyAttribute<AssemblyCopyrightAttribute>(assembly);
            if (copyrightAttr != null)
            {
                Copyright = copyrightAttr.Copyright;
            }

            AssemblyTrademarkAttribute trademarkAttr = GetAssemblyAttribute<AssemblyTrademarkAttribute>(assembly);
            if (trademarkAttr != null)
            {
                Trademark = trademarkAttr.Trademark;
            }

            AssemblyVersion = assembly.GetName().Version.ToString();

            ShortAssemblyVersion = string.Format("{0}.{1}", assembly.GetName().Version.Major, assembly.GetName().Version.Minor);

            AssemblyFileVersionAttribute fileVersionAttr = GetAssemblyAttribute<AssemblyFileVersionAttribute>(assembly);
            if (fileVersionAttr != null)
            {
                FileVersion = fileVersionAttr.Version;
            }

            GuidAttribute guidAttr = GetAssemblyAttribute<GuidAttribute>(assembly);
            if (guidAttr != null)
            {
                Guid = guidAttr.Value;
            }

            NeutralResourcesLanguageAttribute languageAttr = GetAssemblyAttribute<NeutralResourcesLanguageAttribute>(assembly);
            if (languageAttr != null)
            {
                NeutralLanguage = languageAttr.CultureName;
            }

            ComVisibleAttribute comAttr = GetAssemblyAttribute<ComVisibleAttribute>(assembly);

            if (comAttr != null)
            {
                IsComVisible = comAttr.Value;
            }
        }

        public static T GetAssemblyAttribute<T>(Assembly assembly) where T : Attribute
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(T), true);
            if ((attributes == null) || (attributes.Length == 0))
            {
                return null;
            }
            else
            {
                return (T)attributes[0];
            }
        }
    }
}
