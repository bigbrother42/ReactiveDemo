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
        ///   ロードされたアセンブリのタイトル
        /// </summary>
        /// <value>
        ///   <see cref="AssemblyTitleAttribute.Title"/> の値と同じです。
        /// </value>
        public string Title { get; set; }

        /// <summary>
        ///   ロードされたアセンブリの説明
        /// </summary>
        /// <value>
        ///   <see cref="AssemblyDescriptionAttribute.Description"/> の値と同じです。
        /// </value>
        public string Description { get; set; }

        /// <summary>
        ///   ロードされたアセンブリの会社名
        /// </summary>
        /// <value>
        ///   <see cref="AssemblyCompanyAttribute.Company"/> の値と同じです。
        /// </value>
        public string Company { get; set; }

        /// <summary>
        ///   ロードされたアセンブリの製品名
        /// </summary>
        /// <value>
        ///   <see cref="AssemblyProductAttribute.Product"/> の値と同じです。
        /// </value>
        public string Product { get; set; }

        /// <summary>
        ///   ロードされたアセンブリの著作権
        /// </summary>
        /// <value>
        ///   <see cref="AssemblyCopyrightAttribute.Copyright"/> の値と同じです。
        /// </value>
        public string Copyright { get; set; }

        /// <summary>
        ///   ロードされたアセンブリの商標
        /// </summary>
        /// <value>
        ///   <see cref="AssemblyTrademarkAttribute.Trademark"/> の値と同じです。
        /// </value>
        public string Trademark { get; set; }

        /// <summary>
        ///   ロードされたアセンブリのアセンブリバージョン
        /// </summary>
        /// <value>
        ///   <see cref="AssemblyName.Version"/> の <see cref="Version.ToString()"/> の戻り値と同じです。
        /// </value>
        public string AssemblyVersion { get; set; }

        /// <summary>
        ///   <para>ロードされたアセンブリの短縮されたアセンブリバージョン</para>
        /// </summary>
        /// <value>
        ///   <see cref="Version.Major"/> と <see cref="Version.Minor"/> の値のみ含む文字列
        /// </value>
        public string ShortAssemblyVersion { get; set; }

        /// <summary>
        ///   ロードされたアセンブリのファイルバージョン
        /// </summary>
        /// <value>
        ///   <see cref="AssemblyFileVersionAttribute.Version"/> の値と同じです。
        /// </value>
        public string FileVersion { get; set; }

        /// <summary>
        ///   ロードされたアセンブリのグローバルに一意の識別子値
        /// </summary>
        /// <value>
        ///   <see cref="GuidAttribute.Value"/> の値と同じです。
        /// </value>
        public string Guid { get; set; }

        /// <summary>
        ///   ロードされたアセンブリのデフォルト言語
        /// </summary>
        /// <value>
        ///   <see cref="NeutralResourcesLanguageAttribute.CultureName"/> の値と同じです。
        /// </value>
        public string NeutralLanguage { get; set; }

        /// <summary>
        ///   COMクライアントがロードされたアセンブリのマネージコードにアクセスする方法
        /// </summary>
        /// <value>
        ///   <see cref="ComVisibleAttribute.Value"/> の値と同じです。
        ///   <para>値が <c>true</c> の場合、管理対象タイプがCOMに表示されることを示します。</para>
        ///   <para>値が <c>false</c> の場合、アセンブリ内のすべてのパブリック型はCOMから非表示になります。</para>
        /// </value>
        public bool IsComVisible { get; set; } = false;

        /// <summary>
        ///   <para>アセンブリ情報を表すクラスのインスタンスを作成します。</para>
        ///   <para>ロードされるアセンブリは、 <see cref="Assembly.GetExecutingAssembly"/> メソッドの戻り値になります。</para>
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
